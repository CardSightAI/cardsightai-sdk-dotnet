using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CardSightAI.Configuration;
using CardSightAI.Generated;
using Microsoft.Extensions.Options;

namespace CardSightAI
{
    /// <summary>
    /// Main client for interacting with the CardSight AI API.
    /// This is a wrapper around the auto-generated NSwag client.
    /// </summary>
    public class CardSightAIClient : ICardSightAIClient
    {
        private readonly HttpClient _httpClient;
        private readonly CardSightAIOptions _options;
        private readonly ICardSightApiClient _generatedClient;
        private readonly bool _disposeHttpClient;

        /// <summary>
        /// Gets the auto-generated API client with all endpoints
        /// </summary>
        public ICardSightApiClient Api => _generatedClient;

        /// <summary>
        /// Creates a new instance of the CardSight AI client with an API key
        /// </summary>
        /// <param name="apiKey">Your CardSight AI API key</param>
        public CardSightAIClient(string apiKey) : this(new CardSightAIOptions { ApiKey = apiKey })
        {
        }

        /// <summary>
        /// Creates a new instance of the CardSight AI client with options
        /// </summary>
        /// <param name="options">Configuration options</param>
        public CardSightAIClient(CardSightAIOptions options) : this(Options.Create(options))
        {
        }

        /// <summary>
        /// Creates a new instance of the CardSight AI client with options
        /// </summary>
        /// <param name="options">Configuration options</param>
        public CardSightAIClient(IOptions<CardSightAIOptions> options)
        {
            _options = options.Value;
            _options.Validate();

            // Create HTTP client with authentication
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_options.BaseUrl),
                Timeout = _options.Timeout
            };

            // Add API key header
            if (!string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", _options.ApiKey);
            }

            // Add user agent
            if (!string.IsNullOrWhiteSpace(_options.UserAgent))
            {
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
            }

            // Add any additional headers
            if (_options.AdditionalHeaders != null)
            {
                foreach (var header in _options.AdditionalHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            _disposeHttpClient = true;

            // Create the auto-generated client (it has its own BaseUrl property)
            _generatedClient = new CardSightApiClient(_httpClient);
        }

        /// <summary>
        /// Creates a new instance of the CardSight AI client with a custom HTTP client
        /// </summary>
        /// <param name="httpClient">Pre-configured HTTP client</param>
        /// <param name="options">Configuration options</param>
        public CardSightAIClient(HttpClient httpClient, IOptions<CardSightAIOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options.Value;
            _options.Validate();

            _disposeHttpClient = false;

            // Add API key header if not already present
            if (!string.IsNullOrWhiteSpace(_options.ApiKey) && !_httpClient.DefaultRequestHeaders.Contains("X-API-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", _options.ApiKey);
            }

            // Create the auto-generated client
            _generatedClient = new CardSightApiClient(_httpClient);
        }

        /// <summary>
        /// Disposes of the HTTP client if it was created internally
        /// </summary>
        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                _httpClient?.Dispose();
            }
        }
    }

    /// <summary>
    /// Interface for the CardSight AI client
    /// </summary>
    public interface ICardSightAIClient : IDisposable
    {
        /// <summary>
        /// Gets the auto-generated API client with all endpoints
        /// </summary>
        ICardSightApiClient Api { get; }
    }
}