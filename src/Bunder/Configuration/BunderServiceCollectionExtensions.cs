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
    public static class BunderServiceCollectionExtensions
    {
        internal static readonly IDictionary<string, string> _defaultOutputDirectoryLookup = new Dictionary<string, string>()
        {
            { "js", "/content/js" },
            { "css", "/content/css" }
        };

        public static IServiceCollection AddBunder(this IServiceCollection services)
        {
            return AddBunder(services, null, null);
        }

        public static IServiceCollection AddBunder(this IServiceCollection services, BunderSettings settings)
        {
            return AddBunder(services, settings, null);
        }

        public static IServiceCollection AddBunder(this IServiceCollection services, IBundlingConfiguration bundlingConfiguration)
        {
            return AddBunder(services, null, bundlingConfiguration);
        }

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
                    string configPath = Path.Combine(serviceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath,
                                                    serviceProvider.GetRequiredService<BunderSettings>().BundlesConfigFilePath);

                    return new BundlingJsonConfiguration(_defaultOutputDirectoryLookup,
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
