using System;
using System.Collections.Generic;
using CardSightAI.Exceptions;

namespace CardSightAI.Configuration
{
    /// <summary>
    /// Configuration options for the CardSight AI SDK
    /// </summary>
    public class CardSightAIOptions
    {
        /// <summary>
        /// The API key for authentication. Can also be set via CARDSIGHTAI_API_KEY environment variable.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// The base URL for the CardSight AI API. Defaults to https://api.cardsight.ai
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.cardsight.ai";

        /// <summary>
        /// The timeout for HTTP requests. Defaults to 120 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(120);

        /// <summary>
        /// The timeout for image upload requests. Defaults to 30 seconds.
        /// </summary>
        public TimeSpan ImageUploadTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Additional headers to include in all API requests
        /// </summary>
        public Dictionary<string, string> AdditionalHeaders { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Maximum number of retry attempts for failed requests. Defaults to 3.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Whether to throw exceptions for non-success status codes. Defaults to true.
        /// </summary>
        public bool ThrowOnError { get; set; } = true;

        /// <summary>
        /// User agent string for API requests
        /// </summary>
        public string UserAgent { get; set; } = $"CardSightAI-DotNet-SDK/2.1.0 (.NET/{Environment.Version})";

        /// <summary>
        /// Validates the configuration options
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                // Check environment variable
                ApiKey = Environment.GetEnvironmentVariable("CARDSIGHTAI_API_KEY");

                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    throw new CardSightAIValidationException(
                        "API key is required. Set it via CardSightAIOptions.ApiKey or CARDSIGHTAI_API_KEY environment variable.");
                }
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new CardSightAIValidationException("Base URL is required.");
            }

            if (Timeout <= TimeSpan.Zero)
            {
                throw new CardSightAIValidationException("Timeout must be greater than zero.");
            }

            if (ImageUploadTimeout <= TimeSpan.Zero)
            {
                throw new CardSightAIValidationException("Image upload timeout must be greater than zero.");
            }

            if (MaxRetryAttempts < 0)
            {
                throw new CardSightAIValidationException("Max retry attempts must be non-negative.");
            }
        }
    }
}