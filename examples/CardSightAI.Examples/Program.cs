using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CardSightAI;
using CardSightAI.Configuration;
using CardSightAI.Generated;

namespace CardSightAI.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("CardSight AI SDK Examples");
            Console.WriteLine("=========================\n");

            // Initialize the client
            // You can set your API key via environment variable: CARDSIGHTAI_API_KEY
            // Or pass it directly: new CardSightAIClient("your-api-key")
            var client = new CardSightAIClient(new CardSightAIOptions
            {
                ApiKey = Environment.GetEnvironmentVariable("CARDSIGHTAI_API_KEY") ?? "your-api-key-here",
                Timeout = TimeSpan.FromSeconds(30)
            });

            try
            {
                // Run health check first
                await RunHealthCheck(client);

                // Show menu
                while (true)
                {
                    Console.WriteLine("\nSelect an example to run:");
                    Console.WriteLine("1. Health Check");
                    Console.WriteLine("2. Get Catalog Statistics");
                    Console.WriteLine("3. List Segments");
                    Console.WriteLine("4. List Manufacturers");
                    Console.WriteLine("5. Search Cards");
                    Console.WriteLine("6. List Sets");
                    Console.WriteLine("0. Exit");
                    Console.Write("\nEnter your choice: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await RunHealthCheck(client);
                            break;
                        case "2":
                            await RunGetStatistics(client);
                            break;
                        case "3":
                            await RunListSegments(client);
                            break;
                        case "4":
                            await RunListManufacturers(client);
                            break;
                        case "5":
                            await RunSearchCards(client);
                            break;
                        case "6":
                            await RunListSets(client);
                            break;
                        case "0":
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task RunHealthCheck(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- Health Check ---");

            try
            {
                // Using the auto-generated API client
                var health = await client.Api.GetHealthAsync();
                Console.WriteLine($"API Status: OK");
                Console.WriteLine($"Response received successfully");
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Health check failed: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Health check failed: {ex.Message}");
            }
        }

        static async Task RunGetStatistics(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- Catalog Statistics ---");

            try
            {
                var stats = await client.Api.GetStatisticsAsync();

                if (stats != null)
                {
                    Console.WriteLine("\nCatalog Overview:");

                    // The response has nested objects for segments, manufacturers, releases, sets, etc.
                    if (stats.Cards != null)
                    {
                        Console.WriteLine($"Total Cards: {stats.Cards.Total:N0}");
                    }

                    if (stats.Sets != null)
                    {
                        Console.WriteLine($"Total Sets: {stats.Sets.Total:N0}");
                    }

                    if (stats.Manufacturers != null)
                    {
                        Console.WriteLine($"Total Manufacturers: {stats.Manufacturers.Total:N0}");
                    }

                    if (stats.Segments != null)
                    {
                        Console.WriteLine($"Total Segments: {stats.Segments.Total:N0}");
                    }

                    Console.WriteLine("\nStatistics retrieved successfully!");
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Failed to get statistics: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get statistics: {ex.Message}");
            }
        }

        static async Task RunListSegments(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- List Segments ---");

            try
            {
                var response = await client.Api.GetSegmentsAsync(take: 10);

                if (response?.Segments != null && response.Segments.Any())
                {
                    Console.WriteLine($"\nFound {response.Segments.Count} segments:");
                    foreach (var segment in response.Segments)
                    {
                        Console.WriteLine($"  - {segment.Name} (ID: {segment.Id})");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo segments found.");
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Failed to list segments: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to list segments: {ex.Message}");
            }
        }

        static async Task RunListManufacturers(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- List Manufacturers ---");

            try
            {
                var response = await client.Api.GetManufacturersAsync(take: 10);

                if (response?.Manufacturers != null && response.Manufacturers.Any())
                {
                    Console.WriteLine($"\nFound {response.Manufacturers.Count} manufacturers:");
                    foreach (var manufacturer in response.Manufacturers)
                    {
                        Console.WriteLine($"  - {manufacturer.Name} (ID: {manufacturer.Id})");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo manufacturers found.");
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Failed to list manufacturers: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to list manufacturers: {ex.Message}");
            }
        }

        static async Task RunSearchCards(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- Search Cards ---");
            Console.Write("Enter search query (e.g., 'Michael Jordan'): ");
            var query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
            {
                query = "Michael Jordan";
                Console.WriteLine($"Using default query: {query}");
            }

            try
            {
                // Use the autocomplete cards search endpoint
                var response = await client.Api.AutocompleteCardsAsync(q: query);

                if (response?.Suggestions != null && response.Suggestions.Any())
                {
                    Console.WriteLine($"\nFound {response.Suggestions.Count} suggestions:");
                    foreach (var suggestion in response.Suggestions)
                    {
                        Console.WriteLine($"  - {suggestion}");
                    }
                }
                else
                {
                    Console.WriteLine($"\nNo suggestions found matching '{query}'.");
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Search failed: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search failed: {ex.Message}");
            }
        }

        static async Task RunListSets(ICardSightAIClient client)
        {
            Console.WriteLine("\n--- List Sets ---");
            Console.Write("Enter year (or press Enter for all): ");
            var yearStr = Console.ReadLine();

            try
            {
                string? year = null;
                if (!string.IsNullOrWhiteSpace(yearStr))
                {
                    year = yearStr;
                    Console.WriteLine($"Filtering by year: {year}");
                }

                // GetSetsAsync is the method for listing sets
                var response = await client.Api.GetSetsAsync(
                    year: year,
                    take: 10
                );

                if (response?.Sets != null && response.Sets.Any())
                {
                    Console.WriteLine($"\nFound {response.Sets.Count} sets:");
                    foreach (var set in response.Sets)
                    {
                        Console.WriteLine($"  - {set.Name} (ID: {set.Id})");
                        Console.WriteLine($"    Cards: {set.CardCount:N0}");
                        if (!string.IsNullOrWhiteSpace(set.Description))
                        {
                            Console.WriteLine($"    {set.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nNo sets found.");
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Failed to list sets: {ex.Message}");
                Console.WriteLine($"Status Code: {ex.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to list sets: {ex.Message}");
            }
        }
    }
}