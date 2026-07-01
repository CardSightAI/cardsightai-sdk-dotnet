using System;
using System.Net.Http;
using CardSightAI;
using CardSightAI.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CardSightAI.Extensions
{
    /// <summary>
    /// Extension methods for configuring CardSight AI services with dependency injection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds CardSight AI SDK services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">Configuration section containing CardSight AI options</param>
        /// <param name="sectionName">The name of the configuration section (default: "CardSightAI")</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCardSightAI(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "CardSightAI")
        {
            services.Configure<CardSightAIOptions>(configuration.GetSection(sectionName));
            return services.AddCardSightAI();
        }

        /// <summary>
        /// Adds CardSight AI SDK services to the service collection with explicit options
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Action to configure CardSight AI options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCardSightAI(
            this IServiceCollection services,
            Action<CardSightAIOptions> configureOptions)
        {
            services.Configure(configureOptions);
            return services.AddCardSightAI();
        }

        /// <summary>
        /// Adds CardSight AI SDK services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddCardSightAI(this IServiceCollection services)
        {
            // Register as singleton with factory
            services.AddSingleton<ICardSightAIClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CardSightAIOptions>>();
                return new CardSightAIClient(options);
            });

            return services;
        }
    }
}