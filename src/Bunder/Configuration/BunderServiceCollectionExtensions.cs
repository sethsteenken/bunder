using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            return null;

        }


        public static IServiceCollection AddBunder(this IServiceCollection services, BunderSettings settings)
        {
            return null;
        }

        public static IServiceCollection AddBunder(this IServiceCollection services, IBundlingConfiguration bundlingConfiguration)
        {
            return null;
        }


        public static IServiceCollection AddBunder(this IServiceCollection services, BunderSettings settings, IBundlingConfiguration bundlingConfiguration)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(settings, nameof(settings));
            Guard.IsNotNull(bundlingConfiguration, nameof(bundlingConfiguration));

            services.TryAddSingleton<JsonSerializer>();

            services.TryAddSingleton<BunderSettings>(settings);
            services.TryAddSingleton<IBundlingConfiguration>((serviceProvider) =>
            {
                string configPath = Path.Combine(serviceProvider.GetRequiredService<IHostingEnvironment>().ContentRootPath,
                                                serviceProvider.GetRequiredService<BunderSettings>().BundlesConfigFilePath);

                return new BundlingJsonConfiguration(_defaultOutputDirectoryLookup, 
                                serviceProvider.GetRequiredService<JsonSerializer>(),
                                configPath);
            });

            services.TryAddSingleton<IEnumerable<Bundle>>((serviceProvider) => serviceProvider.GetRequiredService<IBundlingConfiguration>().Build());
            services.TryAddSingleton<IBundleLookup, BundleLookup>();
            services.TryAddSingleton<IVersioningFormatter>((serviceProvider) =>
            {
                return new FileVersioningFormatter(
                    serviceProvider.GetRequiredService<IHostingEnvironment>().WebRootFileProvider,
                    serviceProvider.GetRequiredService<IMemoryCache>()
                );
            });

            services.TryAddScoped<IPathFormatter, UrlPathFormatter>();
            services.TryAddScoped<IAssetResolver, AssetResolver>();


            //serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request.PathBase

            return services;
        }
    }
}
