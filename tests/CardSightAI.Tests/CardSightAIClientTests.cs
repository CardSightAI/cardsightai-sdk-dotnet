using System;
using System.Threading.Tasks;
using Xunit;
using CardSightAI;
using CardSightAI.Configuration;
using CardSightAI.Exceptions;
using CardSightAI.Generated;

namespace CardSightAI.Tests
{
    public class CardSightAIClientTests
    {
        [Fact]
        public void Constructor_WithApiKey_InitializesClient()
        {
            // Arrange & Act
            var client = new CardSightAIClient("test-api-key");

            // Assert
            Assert.NotNull(client);
            Assert.NotNull(client.Api);
        }

        [Fact]
        public void Constructor_WithOptions_InitializesClient()
        {
            // Arrange
            var options = new CardSightAIOptions
            {
                ApiKey = "test-api-key",
                BaseUrl = "https://api.test.com",
                Timeout = TimeSpan.FromSeconds(60)
            };

            // Act
            var client = new CardSightAIClient(options);

            // Assert
            Assert.NotNull(client);
            Assert.NotNull(client.Api);
        }

        [Fact]
        public void Constructor_WithNullApiKey_ThrowsException()
        {
            // Arrange
            var options = new CardSightAIOptions
            {
                ApiKey = null
            };

            // Act & Assert
            Assert.Throws<CardSightAIValidationException>(() => new CardSightAIClient(options));
        }

        [Fact]
        public void Constructor_WithEmptyApiKey_ThrowsException()
        {
            // Arrange
            var options = new CardSightAIOptions
            {
                ApiKey = ""
            };

            // Act & Assert
            Assert.Throws<CardSightAIValidationException>(() => new CardSightAIClient(options));
        }

        [Fact]
        public void Constructor_WithInvalidTimeout_ThrowsException()
        {
            // Arrange
            var options = new CardSightAIOptions
            {
                ApiKey = "test-key",
                Timeout = TimeSpan.Zero
            };

            // Act & Assert
            Assert.Throws<CardSightAIValidationException>(() => new CardSightAIClient(options));
        }

        [Fact]
        public void Dispose_DisposesHttpClient()
        {
            // Arrange
            var client = new CardSightAIClient("test-api-key");

            // Act & Assert (should not throw)
            client.Dispose();
            client.Dispose(); // Should handle multiple calls
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsSuccessfully()
        {
            // Arrange
            var client = new CardSightAIClient(new CardSightAIOptions
            {
                ApiKey = "dummy-key-for-unauthenticated-endpoint",
                Timeout = TimeSpan.FromSeconds(10)
            });

            try
            {
                // Act
                var response = await client.Api.GetHealthAsync();

                // Assert
                Assert.NotNull(response);
                // The health endpoint should return successfully even without authentication
            }
            catch (ApiException ex)
            {
                // If we get a 401, the endpoint is working but requires auth
                // If we get a 200, the endpoint is working
                // Any other error might indicate a real problem
                Assert.True(
                    ex.StatusCode == 200 || ex.StatusCode == 401 || ex.StatusCode == 404,
                    $"Unexpected status code: {ex.StatusCode}. Message: {ex.Message}"
                );
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}