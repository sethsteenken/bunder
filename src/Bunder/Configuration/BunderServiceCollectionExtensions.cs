using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Bunder
{
    /// <summary>
    /// Service collection extensions for registering Bunder interfaces and classes.
    /// </summary>
    public static class BunderServiceCollectionExtensions
    {
        /// <summary>
        /// Register Bunder services with the service collection.
        /// Settings at <see cref="BunderSettings"/> will be loaded from appsettings.json config section named "Bunder".
        /// Bundling configuration <see cref="IBundlingConfiguration"/> will load configuration from json file at <see cref="IHostingEnvironment.ContentRootPath"/> + <see cref="BunderSettings.BundlesConfigFilePath"/>.
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        public static IServiceCollection AddBunder(this IServiceCollection services)
        {
            return AddBunder(services, null, null);
        }

        /// <summary>
        /// Register Bunder services with the service collection.
        /// Bundling configuration <see cref="IBundlingConfiguration"/> will load configuration from json file at <see cref="IHostingEnvironment.ContentRootPath"/> + <see cref="BunderSettings.BundlesConfigFilePath"/>.
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        /// <param name="settings">Custom settings object that will be stored as a singleton.</param>
        public static IServiceCollection AddBunder(this IServiceCollection services, BunderSettings settings)
        {
            return AddBunder(services, settings, null);
        }

        /// <summary>
        /// Register Bunder services with the service collection.
        /// Settings at <see cref="BunderSettings"/> will be loaded from appsettings.json config section named "Bunder".
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        /// <param name="bundlingConfiguration">Custom bundling configuration that will compile list of registered bundles.</param>
        public static IServiceCollection AddBunder(this IServiceCollection services, IBundlingConfiguration bundlingConfiguration)
        {
            return AddBunder(services, null, bundlingConfiguration);
        }

        /// <summary>
        /// Register Bunder services with the service collection.
        /// Parameters allow for custom settings. If parameters are null, default json-based configuration will be applied.
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        /// <param name="settings">Custom settings object that will be stored as a singleton.</param>
        /// <param name="bundlingConfiguration">Custom bundling configuration that will compile list of registered bundles.</param>
        public static IServiceCollection AddBunder(this IServiceCollection services, BunderSettings settings, IBundlingConfiguration bundlingConfiguration)
        {
            Guard.IsNotNull(services, nameof(services));
            
            services.TryAddSingleton<JsonSerializer>();

            if (settings != null)
                services.TryAddSingleton<BunderSettings>(settings);
            else
                services.AddCustomConfigurationSettings<BunderSettings>(BunderSettings.DefaultSectionName, singleton: true);

            if (bundlingConfiguration != null)
            {
                services.TryAddSingleton<IBundlingConfiguration>(bundlingConfiguration);
            }
            else
            {
                services.TryAddSingleton<IBundlingConfiguration>((serviceProvider) =>
                {
                    var bunderSettings = serviceProvider.GetRequiredService<BunderSettings>();
                    string configPath = Path.Combine(serviceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath,
                                                    bunderSettings.BundlesConfigFilePath);

                    return new BundlingJsonConfiguration(bunderSettings.OutputDirectories,
                                    serviceProvider.GetRequiredService<JsonSerializer>(),
                                    configPath);
                });
            }
            
            services.TryAddSingleton<IEnumerable<Bundle>>((serviceProvider) => serviceProvider.GetRequiredService<IBundlingConfiguration>().Build());
            services.TryAddSingleton<IBundleLookup, BundleLookup>();
            services.TryAddSingleton<IVersioningFormatter>((serviceProvider) =>
            {
                return new FileVersioningFormatter(
                    serviceProvider.GetRequiredService<IHostingEnvironment>().WebRootFileProvider,
                    serviceProvider.GetService<IMemoryCache>()
                );
            });

            services.TryAddScoped<IPathFormatter>((serviceProvider) =>
            {
                return new UrlPathFormatter(
                                serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request.GetBaseUrl(),
                                serviceProvider.GetRequiredService<IVersioningFormatter>());
            });

            services.TryAddScoped<IAssetResolver, AssetResolver>();

            return services;
        }
    }
}
