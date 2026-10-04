# Changelog

All notable changes to the CardSight AI .NET SDK will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Nothing yet

### Changed
- Nothing yet

### Deprecated
- Nothing yet

### Removed
- Nothing yet

### Fixed
- Nothing yet

### Security
- Nothing yet

## [3.1.0] - 2026-10-04

### Added
- **CardMagic: listing-ready card images from a photo** — new `CardMagic` tag and `ProcessCardImageAsync(...)` (`POST /v1/cardmagic/process`). Upload a phone photo of one or more cards and get each card back as a clean, straightened image. Because the response is binary, the method returns a `FileResponse` (`IDisposable`: `Stream`, `Headers`, `StatusCode`) instead of a DTO.
  - Signature: `ProcessCardImageAsync(Mode? mode = null, double? paddingPercent = null, string paddingFill = null, AutoLevels? autoLevels = null, OutputFormat? outputFormat = null, int? longEdge = null, Corners? corners = null, FileParameter image = null, CancellationToken cancellationToken = default)`. All query options are optional; `image` is required in practice (a `null` image throws `ArgumentNullException`). Use named arguments.
  - Options: `mode` (`Mode.Process` straightens and squares each card, the default; `Mode.Crop` trims each card as it appears), `paddingPercent` (0–50, default 5), `paddingFill` (`"background"` or `"#RRGGBB"`), `autoLevels` (`AutoLevels.True` default / `AutoLevels.False`), `outputFormat` (`OutputFormat.Jpeg` default / `OutputFormat.Png`), `longEdge` (32–2100 px), `corners` (`Corners.True` adds close-ups of each card's four corners plus a combined sheet).
  - Input: pass the photo as a `FileParameter`; it is sent as `multipart/form-data` with the file in the `image` field. Set the part content type to match the photo (`image/jpeg`, `image/png`, `image/webp`, `image/heic` or `image/heif`). Max 20 MB and 8192 px per side. The generated client sends the multipart form only; the raw `image/*` request-body form is not exposed.
  - Output: one card returns `image/jpeg` or `image/png` (per `outputFormat`); two or more cards, or `corners=true` even for one card, return `application/zip` containing `card_0.<ext>`, `card_1.<ext>`, and so on. The response headers are `X-CardMagic-Count` (cards in the photo), plus `X-CardMagic-Width` and `X-CardMagic-Height` (single image only, padding included). The content type is available as the `Content-Type` entry in `Headers`.
  - Errors: non-success statuses throw `ApiException<ErrorResponse>` (`400`, `401`, `408`, `422`, `429`, `500`, `503`). A photo with no card is `422` with `Result.Code == "NO_CARD_FOUND"`.
  - New enums in `CardSightAI.Generated`: `Mode`, `AutoLevels`, `OutputFormat`, `Corners`.

  ```csharp
  using System.IO.Compression;
  using CardSightAI;
  using CardSightAI.Generated;

  using var client = new CardSightAIClient("your_api_key");

  using var photo = File.OpenRead("photo.jpg");
  var image = new FileParameter(photo, "photo.jpg", "image/jpeg");   // image/jpeg, png, webp, heic or heif

  try
  {
      using FileResponse response = await client.Api.ProcessCardImageAsync(
          mode: Mode.Process,
          outputFormat: OutputFormat.Jpeg,
          longEdge: 1200,
          image: image);

      // Header names keep the server's casing (often lowercase), so look them up case-insensitively
      string? Header(string name) => response.Headers
          .FirstOrDefault(h => string.Equals(h.Key, name, StringComparison.OrdinalIgnoreCase))
          .Value?.FirstOrDefault();

      var count = int.Parse(Header("X-CardMagic-Count") ?? "0");

      if (Header("Content-Type")?.StartsWith("application/zip") == true)
      {
          // Two or more cards (or corners: Corners.True): card_0.jpg, card_1.jpg, ...
          using var zip = new ZipArchive(response.Stream, ZipArchiveMode.Read);
          foreach (var entry in zip.Entries)
              entry.ExtractToFile(Path.Combine("out", entry.Name), overwrite: true);
          Console.WriteLine($"{count} cards extracted");
      }
      else
      {
          // One card: image/jpeg or image/png, pixel size in X-CardMagic-Width / X-CardMagic-Height
          using var file = File.Create(Path.Combine("out", "card.jpg"));
          await response.Stream.CopyToAsync(file);
          Console.WriteLine($"1 card, {Header("X-CardMagic-Width")}x{Header("X-CardMagic-Height")} px");
      }
  }
  catch (ApiException<ErrorResponse> ex) when (ex.StatusCode == 422 && ex.Result.Code == "NO_CARD_FOUND")
  {
      Console.WriteLine("No card found in the photo");
  }
  ```
- **Detection counts on identification** — `IdentifyCardResponse` gained `DetectedCount` (cards found in the image, whether or not they were identified; also present on unsuccessful identifications) and `IdentifiedCount` (detections whose `Card` matched the catalog). Both are `long` and read `0` when the API omits them, so `DetectedCount - IdentifiedCount` is the number of cards found but not identified.
- **Slab certification number** — `SlabGradingDetail` gained `CertNumber` (`string`), the certification number read from the slab label; `null` when it could not be read.
- **Card variations** — `CardSummary`, `CardWithOptionalParallel`, `DetailedCard`, and `DetailedCardResponse` gained `Variations` (`ICollection<string>`): the UUIDs of cards that are variations of that card. `null` when the card has no variations.

### Changed
- **Unidentified cards in graded slabs are now returned.** A card inside a graded slab that cannot be identified is now returned as a detection with an empty `Card` plus its `Grading`. Code that assumes every detection with `Grading` has a matched card should check for an exact or set-level match first (for example `!string.IsNullOrEmpty(detection.Card.Id)`, or compare `IdentifiedCount` to `DetectedCount`). No schema change.
- Bumped the SDK user-agent to `CardSightAI-DotNet-SDK/3.1.0`.
- Regenerated the NSwag client from the latest OpenAPI spec (80 paths, 364 schemas, 19 tags). The change is additive: no endpoints, parameters, or DTOs were removed or renamed.

## [3.0.0] - 2026-09-11

### Breaking
- **`CardDetails.Parallel` removed.** The single `ParallelSummary` field on `CardDetails` (and on `CardSuggestion`) has been replaced by `ParallelSuggestions` (`ICollection<ParallelSuggestion>`), a ranked list of possible parallels ordered best-match-first. Each `ParallelSuggestion` carries the same `Id`/`Name`/`Description`/`IsPartial`/`NumberedTo`/`Cards` fields as the old `ParallelSummary`, plus an optional `Confidence` (`ParallelSuggestionConfidence`: `High` | `Medium` | `Low`) — a missing value means the tier was not assessed, not `Low`.
  - **Migration:** replace `card.Parallel` with `card.ParallelSuggestions?.FirstOrDefault()` to get the best-ranked candidate; iterate the full collection to see all remaining possibilities.

### Added
- **Card pricing timeseries** — `GetCardPricingTimeseriesAsync(Interval interval, string card_id, ...)` (`GET /v1/pricing/{card_id}/timeseries`), tagged **Pricing**. Returns OHLC-style candle series bucketed by `interval` (`daily` | `weekly` | `monthly`), split into `Raw` (ungraded) and `Graded` (grouped by grading company/grade) sections. Optional filters: `periods`, `as_of_date`, `listing_type`, `parallel_id`, `grade_id`. New DTOs: `TimeseriesResponse`, `TimeseriesQueryEcho`, `RawTimeseriesSection`, `TimeseriesCompanyGroup`, `TimeseriesGradeGroup`, `TimeseriesTypeTotals`, `CandlePeriod`, `CandleStats`, and the `Interval` enum.
- **`CardSuggestion` now carries full card fields** (`Id`, `SegmentId`, `ReleaseId`, `SetId`, `Year`, `Manufacturer`, `ReleaseName`, `SetName`, `Name`, `Number`, `Description`, `NumberedTo`, `Attributes`, `VariationOf`, `Fields`) when detection confidence is `Medium` or `Low`, instead of a minimal shape.
- **`SearchResult` gained `SegmentName`, `CardNumber`, and `MatchKind`** (`SearchResultMatchKind`: `exact` | `fuzzy`) — present on every result of a page only when fuzzy matching engaged for that request.
- **`FeedbackResponseStatus` gained new values**: `New`, `Confirmed_bug`, `Enhancement_backlog`, `Enhancement_planned`, `Released`, `Not_an_issue`, `Closed` (alongside the existing lifecycle values).
- Card `Fields` (`CardDetails.Fields` / `CardSuggestion.Fields`) may now include a `CARD_LANGUAGE` entry with the ISO 639-1 code of the scanned card's language.
- Documented `409 Conflict` response on feedback submission, and `408 Request Timeout` / `503 Service Unavailable` on catalog search.

### Changed
- Bumped the SDK user-agent to `CardSightAI-DotNet-SDK/3.0.0`.
- Regenerated the NSwag client from the latest OpenAPI spec (79 paths, 364 schemas, 18 tags).
- Catalog title search `q` parameter's documented minimum length is now 2 characters.
- Parallel catalog endpoints (`GetParallelsAsync`, `GetParallelAsync`) are no longer labelled as part of the free tier in the spec.

## [2.1.0] - 2026-07-15

### Added
- **Pricing history paging** — `GetCardPricingAsync` accepts an optional `as_of_date` query param; responses are capped at 500 rows with an advisory `Messages` array.
- **Catalog `/N` slash search** — `SearchResult` now includes `NumberedTo`.
- **Server advisory messages** — `Messages` (`ServerMessage[]`) arrays added to `PaginatedCardsResponse`, `CatalogSearchResponse`, and `PricingResponse`.

### Changed
- Regenerated the NSwag client from the latest OpenAPI spec.

### Notes
- The new `as_of_date` parameter in `GetCardPricingAsync` is inserted mid-list in the server-declared order, between `period` and `listing_type`. Use named arguments when calling this method to avoid binding issues.

## [2.0.0] - 2026-06-30

### Added
- **Pricing endpoints** (completed sales, grouped into raw/graded sections):
  - `GetCardPricingAsync` — price history for a single card (`GET /v1/pricing/{card_id}`)
  - `GetBulkPricingAsync` — price history for multiple cards (`POST /v1/pricing/`)
  - `SearchPricingByTitleAsync` — free-text search of completed sales by listing title (`GET /v1/pricing/search`)
- **Marketplace endpoints** (active listings):
  - `GetCardMarketplaceAsync` — active listings for a card (`GET /v1/marketplace/{card_id}`)
  - `SearchMarketplaceByTitleAsync` — free-text search of active listings by title (`GET /v1/marketplace/search`)
- **Population report endpoints** — graded population counts by card, set, or release (`GetCardPopulationAsync`, `GetSetPopulationAsync`, `GetReleasePopulationAsync`)
- **Release calendar** — `GetReleaseCalendarAsync` for upcoming and recent product releases
- **Typed convenience extension methods** — `ICardSightApiClient.AddCollectionCardAsync` and `AddCardToListAsync` accept the strongly-typed `CollectionCardItemInput` / `ListCardItemInput` instead of the loosely-typed generated request bodies (`CreateCollectionCardInput` / `AddCardToListInput`, which the code generator emits as free-form objects because their OpenAPI schemas are `anyOf` single-or-batch)

### Changed
- **BREAKING:** Regenerated the NSwag API client and DTOs against the latest OpenAPI specification. Method signatures, generated DTO types, and response shapes reflect the current API surface and may differ from 1.0.0.
- Bumped the SDK user-agent to `CardSightAI-DotNet-SDK/2.0.0`.

### Documentation
- Rewrote the README to mirror the structure of the official CardSight AI Node.js SDK, with task-oriented C# examples for identification, catalog, pricing, marketplace, population, collections, and the full endpoint set.
- Added a "Why CardSight AI?" positioning section (targeting both developers and AI agents) and aligned the package `Description` and NuGet `PackageTags` with the marketing/SEO of the Node.js SDK.

## [1.0.0] - 2025-01-04

### Added
- Initial release of the CardSight AI .NET SDK
- Auto-generated API client using NSwag from OpenAPI specification
- Support for all CardSight AI API endpoints
- Multi-targeting support for .NET 9, .NET 8, and .NET Standard 2.1
- Comprehensive configuration options via `CardSightAIOptions`
- Environment variable support for API key (`CARDSIGHTAI_API_KEY`)
- Dependency injection support for ASP.NET Core applications
- Custom exception types for better error handling:
  - `CardSightAIValidationException` for invalid parameters
  - `CardSightAIAuthenticationException` for auth failures
  - `CardSightAINotFoundException` for missing resources
  - `CardSightAIRateLimitException` for rate limiting
  - `CardSightAITimeoutException` for request timeouts
  - `CardSightAIServerException` for server errors
- Support for custom HTTP clients and headers
- Complete example console application
- Unit tests for core functionality
- Comprehensive README documentation

### Features
- **Auto-Generated Client**: Automatically stays in sync with API changes
- **Card Identification**: AI-powered card identification from images
- **Catalog Operations**: Search and browse card catalog
- **Collection Management**: Manage personal card collections
- **Market Segments**: Access different card market segments
- **Manufacturers**: Browse card manufacturers
- **Sets and Releases**: Access card sets and releases
- **Statistics**: Get comprehensive catalog statistics
- **Health Checks**: Monitor API health and authentication status

### Known Issues
- The generated client contains some duplicate nested object classes (e.g., Prices2-12) due to inline schema definitions in the OpenAPI specification. This does not affect functionality, and these duplicates will be resolved in future API updates

[Unreleased]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/compare/v3.1.0...HEAD
[3.1.0]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/compare/v3.0.0...v3.1.0
[3.0.0]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/compare/v2.1.0...v3.0.0
[2.1.0]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/CardSightAI/cardsightai-sdk-dotnet/releases/tag/v1.0.0