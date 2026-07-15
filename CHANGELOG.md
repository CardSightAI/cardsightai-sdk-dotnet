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

[Unreleased]: https://github.com/CardSightAI/cardsight-sdk-dotnet/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/CardSightAI/cardsight-sdk-dotnet/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/CardSightAI/cardsight-sdk-dotnet/releases/tag/v1.0.0