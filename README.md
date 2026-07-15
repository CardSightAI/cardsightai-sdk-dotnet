# CardSight AI .NET SDK

![NuGet Version](https://img.shields.io/nuget/v/CardSightAI)
![NuGet Downloads](https://img.shields.io/nuget/dt/CardSightAI)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![.NET](https://img.shields.io/badge/.NET-9.0%20%7C%208.0%20%7C%20Standard%202.1-512BD4)

**Official .NET SDK for [CardSight AI](https://cardsight.ai) REST API**

The most comprehensive baseball card identification and collection management platform.
**12M+ Cards** • **AI-Powered Recognition** • **Free Tier Available**

**Quick Links:** [Getting Started](#getting-started) • [Installation](#installation) • [Examples](#usage-examples) • [API Documentation](https://api.cardsight.ai/documentation) • [Support](#support)

---

## Why CardSight AI?

Anything you build with sports or trading cards — a collection app, a pricing tool, a marketplace, or an AI agent that reasons over cards — starts with reliable card data. CardSight AI provides it through a single REST API that this SDK wraps end to end:

- **Recognize any card from a photo** — AI-powered multi-card detection with confidence levels, plus parallel, serial-numbering, and graded-slab detection.
- **12M+ cards across every category** — sports (baseball, football, basketball, and more) and trading card games (Pokémon, Magic: The Gathering, Yu-Gi-Oh!), unified under one flexible metadata model so you never have to branch per game.
- **Real market data built in** — completed-sales pricing, active marketplace listings, and graded population reports, looked up by card ID or by free-text listing title.
- **Built for developers and AI agents** — every endpoint returns strongly-typed, structured C# objects, ideal both for production apps and for grounding LLMs and AI agents in accurate card data, with a natural-language query endpoint for agentic use.
- **Complete and always current** — 100% API coverage, generated from the OpenAPI spec so it never drifts, and a free tier to start (no credit card required).

## Features

- **Auto-Generated, Strongly-Typed Client** - The full API surface is generated from the OpenAPI specification, so the SDK stays in sync with the API and every request/response is a typed C# object
- **Multi-Card Detection** - Identify multiple cards in a single image with per-detection confidence levels
- **Flexible Metadata via Fields** - Surface arbitrary card properties (HP, Rarity, Artist, Mana Cost, etc.) across any trading card game
- **Pricing, Marketplace & Population Data** - Completed-sales pricing, active listings, and graded population reports, by card ID or free-text title
- **Multi-Target Support** - Builds for .NET 9, .NET 8, and .NET Standard 2.1
- **Async/Await Throughout** - Every endpoint is fully asynchronous and cancellation-aware
- **Dependency Injection Ready** - First-class `IServiceCollection` integration for ASP.NET Core
- **100% API Coverage** - All CardSight AI endpoints are exposed through the typed client

## Key Capabilities

All endpoints are reached through the typed `client.Api` surface. Methods follow the `{Operation}Async` naming generated from the API's operation IDs.

| Feature | Description | Primary Methods |
|---------|-------------|-----------------|
| **Card Identification** | Identify multiple cards from images using AI; free pre-flight set-identifiability lookups | `IdentifyCardAsync`, `IdentifyCardBySegmentAsync`, `ListIdentifiableSetsAsync`, `CheckSetIdentifiableAsync` |
| **Card Detection** | Check whether trading cards are present in an image | `DetectCardAsync` |
| **Catalog Search** | Fuzzy search across cards, sets, releases, parallels | `SearchCatalogAsync`, `GetCardsAsync` |
| **Random Catalog** | Pack-opening simulations with parallel odds | `GetRandomCardsAsync`, `GetRandomSetsAsync`, `GetRandomReleasesAsync` |
| **Collections** | Manage owned card collections with analytics | `CreateCollectionAsync`, `AddCollectionCardsAsync`, `GetCollectionAnalyticsAsync` |
| **Collectors** | Manage collector profiles | `CreateCollectorAsync`, `UpdateCollectorAsync` |
| **Lists** | Track wanted cards (wishlists) | `CreateListAsync`, `AddCardsToListAsync` |
| **Binders** | Organize collection subsets | `CreateBinderAsync`, `AddCardToBinderAsync` |
| **Pricing** | Completed sales data for cards; free-text title search | `GetCardPricingAsync`, `GetBulkPricingAsync`, `SearchPricingByTitleAsync` |
| **Marketplace** | Active marketplace listings for cards; free-text title search | `GetCardMarketplaceAsync`, `SearchMarketplaceByTitleAsync` |
| **Population Reports** | Graded population counts by card, set, or release | `GetCardPopulationAsync`, `GetSetPopulationAsync`, `GetReleasePopulationAsync` |
| **Grading** | PSA, TAG, BGS, SGC grade information | `GetGradingCompaniesAsync`, `GetGradingTypesAsync`, `GetGradesAsync` |
| **AI Search** | Natural language queries | `ProcessAIQueryAsync` |
| **Autocomplete** | Search suggestions for all entities | `AutocompleteCardsAsync`, `AutocompleteSetsAsync`, … |
| **Field Catalog** | Browse flexible metadata fields (Artist, HP, Rarity, etc.) — powers cross-TCG metadata search | `GetFieldsAsync`, `GetFieldByIdAsync` |
| **Release Calendar** | Upcoming and recent product releases across segments and manufacturers | `GetReleaseCalendarAsync` |

## Requirements

- .NET 9.0, .NET 8.0, or a .NET Standard 2.1-compatible runtime
- API Key from [cardsight.ai](https://cardsight.ai) (free tier available)

## Installation

```bash
# .NET CLI
dotnet add package CardSightAI
```

```powershell
# Package Manager Console
Install-Package CardSightAI
```

```xml
<!-- PackageReference -->
<PackageReference Include="CardSightAI" Version="2.0.0" />
```

## Getting Started

### Get Your Free API Key

Get started in minutes with a **free API key** from [cardsight.ai](https://cardsight.ai) - no credit card required!

### Quick Start (< 5 minutes)

```csharp
using CardSightAI;
using CardSightAI.Generated;

// 1. Initialize the client
using var client = new CardSightAIClient("your_api_key_here");

// 2. Identify a card from an image
using var stream = File.OpenRead("card.jpg");
var image = new FileParameter(stream, "card.jpg", "image/jpeg");
var result = await client.Api.IdentifyCardAsync(image);

// 3. Access the identification results
if (result.Success && result.Detections.Count > 0)
{
    // The API can detect multiple cards in a single image
    var detection = result.Detections.First();          // Best match first
    Console.WriteLine($"Card: {detection.Card.Name}");        // Exact match only
    Console.WriteLine($"Set: {detection.Card.ReleaseName}");  // Exact + set-level match
    Console.WriteLine($"Confidence: {detection.Confidence}"); // High, Medium, or Low
    Console.WriteLine($"Total cards detected: {result.Detections.Count}");
}
```

That's it! The SDK handles all API communication, authentication, type safety, and serialization automatically.

## Usage Examples

> All API calls are made through `client.Api`, which exposes the strongly-typed `ICardSightApiClient`. Every method has an optional trailing `CancellationToken`.

### Card Identification

The identification endpoint uses AI to detect cards in images. It can identify multiple cards in a single image and returns a confidence level for each detection. Use `IdentifyCardAsync` for baseball (the default segment) or `IdentifyCardBySegmentAsync` to target a specific sport.

```csharp
using CardSightAI;
using CardSightAI.Generated;

using var client = new CardSightAIClient("your_api_key");

// From a file on disk
using var stream = File.OpenRead("path/to/card.jpg");
var image = new FileParameter(stream, "card.jpg", "image/jpeg");
var result = await client.Api.IdentifyCardAsync(image);

// Process the results
if (result.Success && result.Detections.Count > 0)
{
    Console.WriteLine($"Detected {result.Detections.Count} card(s)");

    foreach (var detection in result.Detections)
    {
        Console.WriteLine($"\nConfidence: {detection.Confidence}");

        // Card is always present — field completeness depends on match level
        if (!string.IsNullOrEmpty(detection.Card.Id))
        {
            // Exact match — all fields populated
            Console.WriteLine($"  Name: {detection.Card.Name}");
            Console.WriteLine($"  Year: {detection.Card.Year}");
            Console.WriteLine($"  Manufacturer: {detection.Card.Manufacturer}");
            Console.WriteLine($"  Set: {detection.Card.SetName ?? detection.Card.ReleaseName}");
            Console.WriteLine($"  Number: {detection.Card.Number ?? "N/A"}");
            Console.WriteLine($"  Card ID: {detection.Card.Id}");
        }
        else if (!string.IsNullOrEmpty(detection.Card.SetId))
        {
            // Set-level match — no specific card, but set info available
            Console.WriteLine($"  Release: {detection.Card.ReleaseName}");
            Console.WriteLine($"  Set: {detection.Card.SetName}");
            Console.WriteLine($"  Year: {detection.Card.Year}");
        }
        else
        {
            // No match — card detected in image but not identified
            Console.WriteLine("  Could not identify this card");
        }
    }

    // Access request metadata
    Console.WriteLine($"\nRequest ID: {result.RequestId}");
    Console.WriteLine($"Processing time: {result.ProcessingTime}ms");

    // Check for server advisory messages (e.g., image quality warnings)
    foreach (var msg in result.Messages)
    {
        Console.WriteLine($"[{msg.Type}] {msg.Message}");
    }
}

// Segment-specific identification (football, basketball, etc.)
var footballResult = await client.Api.IdentifyCardBySegmentAsync("football", image);
```

#### Response Structure

Each detection (`IdentificationData`) has a `Confidence` level and a `Card` (`CardDetails`). The `Card` is always present, but its fields are populated based on the match level:

- **Exact match**: `Card.Id` present — all fields populated including `Name`, `Number`, and optionally `Parallel`
- **Set-level match**: `Card.SetId` present but no `Card.Id` — release/set info available, no specific card
- **No match**: `Card` fields are empty — a card was detected in the image but couldn't be identified

When the card is inside a graded slab, the detection also carries a `Grading` object (`SlabGradingDetail`) describing the detected company, grade, and any qualifier or autograph grade.

#### Checking Set Identifiability (free pre-flight)

Before spending a billed identify call, you can confirm whether a set is supported. These endpoints are **free** — they do not count toward your billed API usage.

```csharp
// List every set the system can identify (paginated)
var sets = await client.Api.ListIdentifiableSetsAsync(take: 20, skip: 0);
Console.WriteLine($"{sets.Total_count} identifiable sets");

// Check whether a specific set is identifiable by its set ID
var check = await client.Api.CheckSetIdentifiableAsync(setId);
if (check.Is_identifiable)
{
    Console.WriteLine($"Set {check.Set_id} is identifiable");
}
```

### Card Detection (Presence Check)

The detection endpoint is a lightweight alternative to full identification — it checks whether trading cards are present in an image without identifying them. This is faster and cheaper when you only need to know if cards exist in the image.

```csharp
using var client = new CardSightAIClient("your_api_key");

using var stream = File.OpenRead("path/to/image.jpg");
var image = new FileParameter(stream, "image.jpg", "image/jpeg");
var result = await client.Api.DetectCardAsync(image);

Console.WriteLine($"Cards detected: {result.Detected}");  // true/false
Console.WriteLine($"Number of cards: {result.Count}");     // 0, 1, 2, ...

foreach (var msg in result.Messages)
{
    Console.WriteLine($"[{msg.Type}] {msg.Message}");
}
```

### Working with Identification Results

The SDK returns plain typed objects, so you can use ordinary LINQ to work with multi-card detection results. The `Confidence` enum is ordered `High` (0) → `Medium` (1) → `Low` (2).

```csharp
using System.Linq;

var result = await client.Api.IdentifyCardAsync(image);

// Highest-confidence detection (best match)
var bestMatch = result.Detections
    .OrderBy(d => d.Confidence)        // High sorts first
    .FirstOrDefault();

if (bestMatch is not null)
{
    Console.WriteLine($"Best match: {bestMatch.Card.Year} {bestMatch.Card.ReleaseName} " +
                      $"{bestMatch.Card.SetName} {bestMatch.Card.Name} #{bestMatch.Card.Number}");
}

// Exact matches only (detections that resolved to a specific card)
var exactMatches = result.Detections
    .Where(d => !string.IsNullOrEmpty(d.Card.Id))
    .ToList();
Console.WriteLine($"{exactMatches.Count} exact match(es)");

// Filter by confidence
var highConfidence = result.Detections
    .Where(d => d.Confidence == IdentificationDataConfidence.High)
    .ToList();

// Did we detect anything?
if (result.Detections.Any())
{
    Console.WriteLine($"Found {result.Detections.Count} card(s)");
}
```

#### Flexible Metadata, Parallels, and Numbered Cards

Every detection's `Card` (`CardDetails`) carries extra context beyond the core identity fields:

- `NumberedTo` — print run for numbered base cards (e.g. `25` for a `/25`), independent of parallels
- `Fields` — key/value metadata tailored to the TCG (e.g. `HP`, `RARITY`, `ARTIST`, `MANA_COST`)
- `Parallel` (`ParallelSummary`) — present when a parallel variant (Refractor, Prizm, numbered parallel, etc.) is detected
- `Attributes` — catalog attribute identifiers attached to the card

```csharp
var detection = result.Detections.FirstOrDefault();
if (detection is not null && detection.Card.Parallel is not null)
{
    var parallel = detection.Card.Parallel;
    Console.WriteLine($"Parallel: {parallel.Name}");
    if (detection.Card.NumberedTo > 0)
    {
        Console.WriteLine($"  🔥 Numbered to /{detection.Card.NumberedTo}");
    }
}
```

See [Fields (Flexible Metadata System)](#fields-flexible-metadata-system) for end-to-end Pokémon and Magic: The Gathering examples.

### Catalog Search

Search across cards, sets, releases, and parallels with a single query:

```csharp
// Global fuzzy search
var results = await client.Api.SearchCatalogAsync(
    q: "Ken Griffey Jr",  // Required search query
    take: 10,
    skip: 0);

// Filter by entity type, segment, and year range
var cardResults = await client.Api.SearchCatalogAsync(
    q: "Patrick Mahomes",
    // The generated enum is named `Type`; qualify it to avoid clashing with System.Type
    type: CardSightAI.Generated.Type.Card,   // Card | Set | Release | Parallel
    segment: "football",
    min_year: "2020",
    max_year: "2024");

// Slash notation: append a standalone "/N" term to hard-filter to cards/parallels
// serial-numbered to that value. Matches include NumberedTo on the result.
var numberedResults = await client.Api.SearchCatalogAsync(q: "aaron judge /25");

// Process results
Console.WriteLine($"Found {results.Total_count} results");
foreach (var r in results.Results)
{
    Console.WriteLine($"[{r.Type}] {r.Name} (relevance: {r.Relevance})");
    if (!string.IsNullOrEmpty(r.SetName)) Console.WriteLine($"  Set: {r.SetName}");
    if (!string.IsNullOrEmpty(r.Year)) Console.WriteLine($"  Year: {r.Year}");
    if (r.NumberedTo > 0) Console.WriteLine($"  Numbered to /{r.NumberedTo}");
}

// Messages is an advisory array (e.g. an ignored/unrecognized query parameter);
// it's omitted from the response when there's nothing to report.
if (results.Messages is { Count: > 0 })
{
    foreach (var m in results.Messages) Console.WriteLine($"  Notice: {m.Message}");
}
```

### Fields (Flexible Metadata System)

Every trading card game has different metadata: Pokémon cards have HP and Rarity, Magic: The Gathering cards have Mana Cost and Artist, Yu-Gi-Oh! cards have Attribute and Level. Rather than hard-coding columns per game, CardSight exposes a flexible **Fields** system — any card, set, release, or segment can carry key/value metadata, and the catalog exposes it as a first-class browsable entity. One SDK surface works across every TCG.

**Browse available fields, sorted by how prevalent they are:**

```csharp
// Fields are ranked by how many catalog entities carry each one
var fields = await client.Api.GetFieldsAsync(
    take: 20,
    sort: Sort12.UsageCount,
    order: Order12.Desc);

foreach (var f in fields.Fields)
{
    Console.WriteLine($"{f.Name} ({f.Key}) — used on {f.UsageCount} entities");
}

// Look up a single field by ID
var field = await client.Api.GetFieldByIdAsync("field_uuid");
```

**Surface metadata directly from an identification result:**

Identification responses include a `Fields` value on every detected card, so you can read rarity, artist, mana cost, etc. straight from a scan:

```csharp
var result = await client.Api.IdentifyCardBySegmentAsync("magic", mtgImage);
var detection = result.Detections.FirstOrDefault();
if (detection?.Card.Fields is not null)
{
    // Fields is the flexible TCG metadata bag attached to the detected card
    Console.WriteLine($"{detection.Card.Name} metadata: {detection.Card.Fields}");
}
```

You can also constrain catalog queries to cards carrying specific field values via the `field` parameter on `SearchCatalogAsync`, `GetCardsAsync`, and `GetRandomCardsAsync`.

### Pricing (Completed Sales)

Get completed sales pricing data for cards, grouped into raw (ungraded) and graded sections:

```csharp
// Pricing for a single card
var pricing = await client.Api.GetCardPricingAsync("card_uuid");

// Card context
Console.WriteLine($"Card: {pricing.Card.Name}");

// Raw (ungraded) sales
Console.WriteLine($"Ungraded sales: {pricing.Raw.Count}");
foreach (var sale in pricing.Raw.Records)
{
    Console.WriteLine($"  ${sale.Price} - {sale.Date} ({sale.Source})");
}

// Graded sales (grouped by company → grade)
foreach (var company in pricing.Graded)
{
    foreach (var grade in company.Grades)
    {
        Console.WriteLine($"  Grade {grade.Grade_value}: {grade.Count} sales");
    }
}

// Filter by parallel, grade, time period, and listing type
// Use named arguments to ensure correct parameter binding
var filtered = await client.Api.GetCardPricingAsync(
    card_id: "card_uuid",
    parallel_id: "parallel_uuid",  // Specific parallel (omit for all)
    grade_id: "grade_uuid",        // Specific grade (omit for all)
    period: "90d",                  // "7d", "2w", "3m", "1y", "all"
    listing_type: Listing_type.Both,
    limit: 50);

// Each call returns the most-recent listings up to a cap of 500 rows ending at
// as_of_date (default: today, US Eastern). If the cap is hit, pricing.Messages
// carries an advisory and you can page further back through history by setting
// as_of_date to the oldest `date` seen in the response.
if (pricing.Messages is { Count: > 0 })
{
    foreach (var m in pricing.Messages) Console.WriteLine($"  Notice: {m.Message}");
}

var olderPage = await client.Api.GetCardPricingAsync(
    card_id: "card_uuid",
    as_of_date: "2026-01-01");  // YYYY-MM-DD, US Eastern; defaults to today

// Bulk pricing for multiple cards (up to 100)
var bulk = await client.Api.GetBulkPricingAsync(new BulkPricingRequestInput
{
    Card_ids = new[] { Guid.Parse("card_uuid_1"), Guid.Parse("card_uuid_2") },
    Period = "90d",
});

Console.WriteLine($"Requested: {bulk.Meta.Requested}, Successful: {bulk.Meta.Successful}");
```

### Pricing Search (Free-Text Title)

Search completed sales by listing title when you don't have a card ID. Returns a flat, relevance-ranked list that spans multiple cards and may include listings never matched to a canonical card:

```csharp
var results = await client.Api.SearchPricingByTitleAsync(
    q: "Ken Griffey Jr 1989 Upper Deck",  // Required, 3–300 characters
    period: "90d",                         // Optional: "7d", "2w", "3m", "1y", "all"
    listing_type: Listing_type2.Both,      // Optional: Auction, Fixed, Both
    limit: 25);                            // Optional: default 100, max 500

Console.WriteLine($"Found {results.Results.Count} sales");
foreach (var sale in results.Results)
{
    Console.WriteLine($"${sale.Price} - {sale.Title} ({sale.Source})");
}
```

### Marketplace (Active Listings)

Get currently active marketplace listings for a card:

```csharp
var listings = await client.Api.GetCardMarketplaceAsync("card_uuid");

Console.WriteLine($"Ungraded listings: {listings.Raw.Count}");
foreach (var listing in listings.Raw.Records)
{
    Console.WriteLine($"  ${listing.Price} ({listing.Source})");
}

// Filter by parallel, grade, and listing type
var filtered = await client.Api.GetCardMarketplaceAsync(
    card_id: "card_uuid",
    listing_type: Listing_type3.Fixed,  // Auction, Fixed (buy-it-now), Both
    limit: 25);
```

### Marketplace Search (Free-Text Title)

Search active marketplace listings by title when you don't have a card ID. Same flat, relevance-ranked shape as pricing search (active listings instead of completed sales):

```csharp
var results = await client.Api.SearchMarketplaceByTitleAsync(
    q: "Ken Griffey Jr 1989 Upper Deck",  // Required, 3–300 characters
    listing_type: Listing_type4.Both,
    limit: 25);

Console.WriteLine($"Found {results.Results.Count} active listings");
foreach (var listing in results.Results)
{
    Console.WriteLine($"${listing.Price} - {listing.Title} ({listing.Source})");
}
```

### Population Reports

Get graded population counts (how many copies have been graded, by company and grade) for a card, set, or release:

```csharp
// Population for a single card
var cardPop = await client.Api.GetCardPopulationAsync("card_uuid");

// Optionally narrow to one grading company
var psaPop = await client.Api.GetCardPopulationAsync("card_uuid", grading_company_id: "psa_uuid");

// Aggregate population for an entire set or release
var setPop = await client.Api.GetSetPopulationAsync("set_uuid");
var releasePop = await client.Api.GetReleasePopulationAsync("release_uuid");
```

### Catalog Operations

Search and retrieve cards, sets, releases, and other catalog data:

```csharp
// Search for specific cards
var cards = await client.Api.GetCardsAsync(
    year: "2023",
    manufacturer: "Topps",
    name: "Aaron Judge",
    take: 10,
    skip: 0);

// Get a specific card by ID
var card = await client.Api.GetCardAsync("card_uuid");

// Search sets, then get the cards in one
var sets = await client.Api.GetSetsAsync(year: "2023", manufacturer: "Topps", take: 20);
var setCards = await client.Api.GetSetCardsAsync("set_uuid", take: 50);

// Search releases (product lines like "Chrome", "Series 1")
var releases = await client.Api.GetReleasesAsync(name: "Chrome", min_year: "2020", max_year: "2024");
var releaseCards = await client.Api.GetReleaseCardsAsync("release_uuid");

// Reference data
var manufacturers = await client.Api.GetManufacturersAsync();
var segments = await client.Api.GetSegmentsAsync();
var parallels = await client.Api.GetParallelsAsync();
var parallel = await client.Api.GetParallelAsync("parallel_uuid");

// Catalog statistics
var stats = await client.Api.GetStatisticsAsync();
Console.WriteLine($"Total cards: {stats.Cards.Total}");
Console.WriteLine($"Total sets: {stats.Sets.Total}");
```

### Release Calendar

Browse upcoming and recent card product releases, sorted by release date (newest first). Useful for "coming soon" pages, recent-release feeds, and pre-order discovery.

```csharp
var calendar = await client.Api.GetReleaseCalendarAsync(
    manufacturer: "Panini",   // UUID or name (case-insensitive)
    year: "2026",
    take: 20,
    skip: 0);

foreach (var entry in calendar.Release_calendar)
{
    Console.WriteLine($"{entry.Name} — releases {entry.Release_date} (pre-order: {entry.Pre_order_date})");
}
Console.WriteLine($"Total upcoming: {calendar.Total_count}");
```

Filters (`segment`, `manufacturer`, `year`) all accept UUIDs or case-insensitive names.

### Random Catalog (Pack Opening & Discovery)

The random endpoints enable pack-opening simulations and discovery features by returning random results instead of paginated sorted results:

```csharp
// Pack-opening simulation — 10 random cards from a set, with parallel odds
var pack = await client.Api.GetRandomCardsAsync(
    setId: "set_uuid",
    count: 10,
    includeParallels: true);  // Enable parallel conversion odds

foreach (var card in pack.Cards)
{
    Console.WriteLine($"{card.Name} #{card.Number}");
}

// Discovery — 5 random releases from 2023
var randomReleases = await client.Api.GetRandomReleasesAsync(year: "2023", count: 5);

// Random sets from a specific release
var randomSets = await client.Api.GetRandomSetsAsync(releaseId: "release_uuid", count: 6);
```

When `includeParallels: true` is set, each card has a weighted probability of converting to a parallel variant (numbered parallels rolled individually, unlimited parallels rolled collectively, base card otherwise). **Note:** `setId` and `releaseId` are mutually exclusive on the cards endpoint.

### Collection Management

Manage personal card collections with full CRUD operations:

```csharp
// Create a new collection (linked to a collector profile)
var collection = await client.Api.CreateCollectionAsync(new CreateCollectionInput
{
    CollectorId = Guid.Parse("collector_uuid"),  // Required
    Name = "My Vintage Cards",
    Description = "Pre-1980 baseball cards",
});

// Collection identifiers are returned as strings; collection endpoints take a Guid
var collectionId = Guid.Parse(collection.Id);

// Add a card to the collection. AddCollectionCardAsync is an SDK convenience wrapper that
// accepts a strongly-typed item (the generated AddCollectionCardsAsync body is loosely typed).
await client.Api.AddCollectionCardAsync(collectionId, new CollectionCardItemInput
{
    CardId = Guid.Parse("card_uuid"),
    Quantity = 1,
    BuyPrice = "50.00",   // optional; ParallelId, GradeId, BuyDate, SellPrice, SoldPrice, SoldDate also optional
});

// Update a collection card (e.g., after selling)
await client.Api.UpdateCollectionCardAsync(new UpdateCollectionCardInput
{
    Quantity = 1,
}, collectionId, Guid.Parse("collection_card_uuid"));

// Analytics and breakdowns
var analytics = await client.Api.GetCollectionAnalyticsAsync(collectionId);
var breakdown = await client.Api.GetCollectionBreakdownAsync(GroupBy.Year, collectionId);

// List collections for a collector
var collections = await client.Api.GetCollectionsAsync(collectorId: Guid.Parse("collector_uuid"), take: 10);
```

### Collectors

Collections belong to collector profiles. Create and manage them directly:

```csharp
var collector = await client.Api.CreateCollectorAsync(new CreateCollectorInput { Name = "Jane Collector" });
var collectorId = Guid.Parse(collector.Id);   // Id is a string; collector endpoints take a Guid

var collectors = await client.Api.GetCollectorsAsync(take: 20);
var one = await client.Api.GetCollectorAsync(collectorId);
await client.Api.UpdateCollectorAsync(new UpdateCollectorInput { Name = "Jane Q. Collector" }, collectorId);
```

### Binders (Collection Organization)

Organize collections into binders (subsets):

```csharp
Guid collectionId = Guid.Parse("collection_uuid");

// Create a binder within a collection
var binder = await client.Api.CreateBinderAsync(new CreateBinderInput
{
    Name = "Hall of Famers",
    Description = "Cards of HOF players",
}, collectionId);

// Add a collection card to the binder
await client.Api.AddCardToBinderAsync(new AddCardToBinderInput
{
    CollectionCardId = Guid.Parse("collection_card_uuid"),
}, collectionId, binder.Id);

// List cards in the binder
var binderCards = await client.Api.GetBinderCardsAsync(collectionId, binder.Id);
```

### Lists (Want Lists / Wishlists)

Track cards you want to acquire:

```csharp
// Create a want list
var list = await client.Api.CreateListAsync(new CreateListInput
{
    Name = "Rookies to Find",
    Description = "2024 rookie cards I need",
});

// Add a card to the list. AddCardToListAsync is an SDK convenience wrapper that accepts a
// strongly-typed item (the generated AddCardsToListAsync body is loosely typed).
await client.Api.AddCardToListAsync(list.Id, new ListCardItemInput { CardId = "card_uuid" });

// Get all cards in the list
var listCards = await client.Api.GetListCardsAsync(list.Id);

// Remove a card when acquired
await client.Api.RemoveCardFromListAsync(list.Id, "card_uuid");
```

### Grading Information

Access grading company data and grade values:

```csharp
// All grading companies (PSA, BGS, SGC, etc.)
var companies = await client.Api.GetGradingCompaniesAsync();

// Grading types for a company (e.g., PSA Regular, PSA DNA)
var types = await client.Api.GetGradingTypesAsync(companyId);

// Specific grades for a grading type
var grades = await client.Api.GetGradesAsync(companyId, typeId);
```

### AI-Powered Search

Use natural language to search the catalog:

```csharp
var response = await client.Api.ProcessAIQueryAsync(new AIQueryRequestInput
{
    Query = "Show me Mike Trout rookie cards worth over $100",
});
```

### Autocomplete

Provide search suggestions for users:

```csharp
// Card name suggestions
var suggestions = await client.Api.AutocompleteCardsAsync(q: "aaron");

// Autocomplete for other entities
var setSuggestions = await client.Api.AutocompleteSetsAsync(q: "chrome");
var mfrSuggestions = await client.Api.AutocompleteManufacturersAsync(q: "top");
var releaseSuggestions = await client.Api.AutocompleteReleasesAsync(q: "series");
var segmentSuggestions = await client.Api.AutocompleteSegmentsAsync(q: "base");
var yearSuggestions = await client.Api.AutocompleteYearsAsync(q: "202");
```

### Image Retrieval

Card images are returned as a streamed `FileResponse`:

```csharp
// Get a card image and save it to disk
using var imageResponse = await client.Api.GetCardImageAsync(Guid.Parse("card_uuid"));
await using (var fileStream = File.Create("card.jpg"))
{
    await imageResponse.Stream.CopyToAsync(fileStream);
}

// Request a placeholder fallback instead of a 404 when no image exists
using var withFallback = await client.Api.GetCardImageAsync(
    Guid.Parse("card_uuid"), format: null, @default: Default.True);

// Collection card images and thumbnails
var collectionId = Guid.Parse("collection_uuid");
var collectionCardId = Guid.Parse("collection_card_uuid");
using var collectionImage = await client.Api.GetCollectionCardImageAsync(collectionId, collectionCardId);
using var thumbnail = await client.Api.GetCollectionCardImageThumbnailAsync(collectionId, collectionCardId);
```

### Feedback System

Submit feedback to improve the platform:

```csharp
// Report an identification issue (the identify request ID is the path argument)
await client.Api.SubmitIdentifyFeedbackAsync(new FeedbackInputInput
{
    Feedback_type = FeedbackInputInputFeedback_type.Data_error,
    Message = "Wrong year detected",
}, Guid.Parse("identification_request_id"));

// General feedback
await client.Api.SubmitGeneralFeedbackAsync(new FeedbackInputInput
{
    Feedback_type = FeedbackInputInputFeedback_type.Bug,
    Message = "Search isn't finding parallel cards",
});

// Entity-specific feedback (card, set, release, manufacturer, segment)
await client.Api.SubmitCardFeedbackAsync(new FeedbackInputInput
{
    Feedback_type = FeedbackInputInputFeedback_type.Data_error,
    Message = "Player name is misspelled",
}, Guid.Parse("card_uuid"));
```

## Strong Typing

The entire client is generated from the OpenAPI specification, so every endpoint, parameter, and response is a strongly-typed C# member with XML documentation surfaced in IntelliSense. The project enables [nullable reference types](https://learn.microsoft.com/dotnet/csharp/nullable-references), so optional fields are expressed as nullable types.

```csharp
using CardSightAI;
using CardSightAI.Generated;

using var client = new CardSightAIClient("your_api_key");

// The compiler knows the exact shape of every response
DetailedCardResponse card = await client.Api.GetCardAsync("card_uuid");

// Enums for constrained parameters
var auctions = await client.Api.GetCardPricingAsync("card_uuid", listing_type: Listing_type.Auction);

// Use the typed interface directly (e.g., for mocking in tests)
ICardSightApiClient api = client.Api;
```

All generated types live in the `CardSightAI.Generated` namespace.

## Error Handling

API calls throw `ApiException` (from `CardSightAI.Generated`) for non-success HTTP responses. Configuration problems detected when constructing the client throw `CardSightAIValidationException` (from `CardSightAI.Exceptions`).

```csharp
using CardSightAI.Generated;

try
{
    var result = await client.Api.IdentifyCardAsync(image);
}
catch (ApiException ex)
{
    Console.WriteLine($"API Error {ex.StatusCode}: {ex.Message}");
    Console.WriteLine($"Response: {ex.Response}");

    switch (ex.StatusCode)
    {
        case 401: Console.WriteLine("Invalid API key"); break;
        case 404: Console.WriteLine("Resource not found"); break;
        case 429: Console.WriteLine("Rate limit exceeded"); break;
        case 503: Console.WriteLine("Service temporarily unavailable — retry later"); break;
    }
}
```

`ApiException<T>` is also generated for endpoints with typed error bodies, and exposes the deserialized payload via its `Result` property.

## Configuration

```csharp
using CardSightAI;
using CardSightAI.Configuration;

// Simple: API key only
using var client = new CardSightAIClient("your_api_key");

// Advanced: explicit options
using var configured = new CardSightAIClient(new CardSightAIOptions
{
    ApiKey = "your_api_key",
    Timeout = TimeSpan.FromSeconds(60),
    AdditionalHeaders = new Dictionary<string, string>
    {
        ["X-Custom-Header"] = "value"
    }
});

// Bring your own HttpClient (e.g., to share handlers/connection pooling)
var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
using var withHttpClient = new CardSightAIClient(
    httpClient,
    Options.Create(new CardSightAIOptions { ApiKey = "your_api_key" }));
```

The API key is sent on every request via the `X-API-Key` header.

### Dependency Injection (ASP.NET Core)

Register the client once and inject `ICardSightAIClient` anywhere:

```csharp
using CardSightAI;
using CardSightAI.Extensions;

// From configuration (binds the "CardSightAI" section)
builder.Services.AddCardSightAI(builder.Configuration);

// Or configure explicitly
builder.Services.AddCardSightAI(options =>
{
    options.ApiKey = "your_api_key";
    options.Timeout = TimeSpan.FromSeconds(60);
});
```

```jsonc
// appsettings.json
{
  "CardSightAI": {
    "ApiKey": "your_api_key",
    "Timeout": "00:00:30"
  }
}
```

```csharp
public class MyService(ICardSightAIClient cardSight)
{
    public Task<CatalogStatisticsResponse> GetStatsAsync()
        => cardSight.Api.GetStatisticsAsync();
}
```

## Environment Variables

If no API key is provided in code, the SDK reads it from the environment:

```bash
CARDSIGHTAI_API_KEY=your_api_key_here  # API key for authentication
```

```csharp
// Picks up CARDSIGHTAI_API_KEY automatically
using var client = new CardSightAIClient(new CardSightAIOptions());
```

## API Endpoint Coverage

The SDK provides complete coverage of the CardSight AI REST API (96 operations), all reached through `client.Api`:

| Category | Endpoints | Representative Methods |
|----------|-----------|------------------------|
| **Health** | 2 | `GetHealthAsync`, `GetHealthAuthenticatedAsync` |
| **Identification** | 4 | `IdentifyCardAsync`, `IdentifyCardBySegmentAsync`, `ListIdentifiableSetsAsync`, `CheckSetIdentifiableAsync` |
| **Detection** | 1 | `DetectCardAsync` |
| **Catalog** | 21 | `SearchCatalogAsync`, `GetCardsAsync`/`GetCardAsync`, `GetSetsAsync`, `GetReleasesAsync`, `GetFieldsAsync`, `GetParallelsAsync`, `GetRandomCardsAsync` |
| **Release Calendar** | 1 | `GetReleaseCalendarAsync` |
| **Collections** | 26 | `CreateCollectionAsync`, `AddCollectionCardsAsync`, `GetCollectionAnalyticsAsync`, binder & image operations |
| **Collectors** | 5 | `CreateCollectorAsync`, `GetCollectorsAsync`, `UpdateCollectorAsync`, `DeleteCollectorAsync` |
| **Lists** | 8 | `CreateListAsync`, `AddCardsToListAsync`, `GetListCardsAsync`, `RemoveCardFromListAsync` |
| **Pricing** | 3 | `GetCardPricingAsync`, `GetBulkPricingAsync`, `SearchPricingByTitleAsync` |
| **Marketplace** | 2 | `GetCardMarketplaceAsync`, `SearchMarketplaceByTitleAsync` |
| **Population** | 3 | `GetCardPopulationAsync`, `GetSetPopulationAsync`, `GetReleasePopulationAsync` |
| **Grading** | 3 | `GetGradingCompaniesAsync`, `GetGradingTypesAsync`, `GetGradesAsync` |
| **Autocomplete** | 6 | `AutocompleteCardsAsync`, `AutocompleteSetsAsync`, … |
| **AI** | 1 | `ProcessAIQueryAsync` |
| **Images** | 1 | `GetCardImageAsync` |
| **Feedback** | 8 | `SubmitIdentifyFeedbackAsync`, `SubmitGeneralFeedbackAsync`, `SubmitCardFeedbackAsync`, … |
| **Subscription** | 1 | `GetSubscriptionAsync` |

## Building from Source

The API client is generated at build time by [NSwag](https://github.com/RicoSuter/NSwag) from the live OpenAPI specification, so building requires network access to `https://api.cardsight.ai/documentation/json`.

```bash
# Clone the repository
git clone https://github.com/CardSightAI/cardsight-sdk-dotnet.git
cd cardsight-sdk-dotnet

# Build the solution (regenerates the client from the OpenAPI spec)
dotnet build

# Run tests
dotnet test

# Pack the NuGet package
dotnet pack -c Release
```

## Testing

```bash
# Run all tests
dotnet test

# Run a single test by name
dotnet test --filter "FullyQualifiedName~HealthEndpoint_ReturnsSuccessfully"
```

Some tests make a live call to the public health endpoint, so a full test run depends on network connectivity.

## Platform Support

The SDK multi-targets `net9.0`, `net8.0`, and `netstandard2.1`, so it runs anywhere those frameworks are supported — including ASP.NET Core, console apps, worker services, Blazor, MAUI, and Unity (via .NET Standard 2.1).

## Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines. Note that the API client in `src/CardSightAI/Generated/` is auto-generated from the OpenAPI specification and must not be edited by hand.

## License

This SDK is released under the MIT License. See the [LICENSE](LICENSE) file for details.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed list of changes and version history.

## Support

- **Email**: support@cardsight.ai
- **Website**: [cardsight.ai](https://cardsight.ai)
- **API Documentation**: [api.cardsight.ai/documentation](https://api.cardsight.ai/documentation)
- **Issues**: [GitHub Issues](https://github.com/CardSightAI/cardsight-sdk-dotnet/issues)

---

*Built with ❤️ by CardSight AI*
