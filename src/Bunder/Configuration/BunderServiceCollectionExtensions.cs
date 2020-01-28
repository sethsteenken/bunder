using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Text.Json;

namespace Bunder
{
    /// <summary>
    /// Service collection extensions for registering Bunder interfaces and classes.
    /// </summary>
    public static class BunderServiceCollectionExtensions
    {
        /// <summary>
        /// Register Bunder services with the service collection.
        /// Parameters allow for custom settings. If parameters are null, default json-based configuration will be applied.
        /// Settings at <see cref="BunderSettings"/> will be loaded from appsettings.json config section named "Bunder".
        /// Bundling configuration <see cref="IBundlingConfiguration"/> will load configuration from json file at <see cref="IHostingEnvironment.ContentRootPath"/> + <see cref="BunderSettings.BundlesConfigFilePath"/>.
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        /// <param name="settings">Custom settings object that will be stored as a singleton.</param>
        /// <param name="bundlingConfiguration">Optional custom bundling configuration that will compile list of registered bundles. By default, values under <see cref="BunderSettings"/> will be used to construct default bundling configuration.</param>
        public static IServiceCollection AddBunder(
            this IServiceCollection services, 
            BunderSettings settings, 
            IBundlingConfiguration bundlingConfiguration = null)
        {
            Guard.IsNotNull(services, nameof(services));

            services.TryAddSingleton<JsonSerializerOptions>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            services.TryAddSingleton<ISerializer, SystemTextJsonSerializer>();
            services.AddHttpContextAccessor();

            services.TryAddSingleton<BunderSettings>(settings ?? new BunderSettings());
   
            if (bundlingConfiguration != null)
            {
                services.TryAddSingleton<IBundlingConfiguration>(bundlingConfiguration);
            }
            else
            {
                services.TryAddSingleton<IBundlingConfiguration>((serviceProvider) =>
                {
                    var bunderSettings = serviceProvider.GetRequiredService<BunderSettings>();

                    var fileProvider = serviceProvider.GetService<IFileProvider>();
                    if (fileProvider == null)
                        fileProvider = serviceProvider.GetRequiredService<IHostingEnvironment>().ContentRootFileProvider;
                                                      
                    var file = fileProvider.GetFileInfo(bunderSettings.BundlesConfigFilePath);
                    if (file == null || !file.Exists)
                        throw new BundleConfigurationException($"Configuration file {bunderSettings.BundlesConfigFilePath} was not found.");

                    return new BundlingJsonConfiguration(bunderSettings.OutputDirectories,
                                    serviceProvider.GetRequiredService<ISerializer>(),
                                    file.PhysicalPath);
                });
            }
            
            services.TryAddSingleton<IEnumerable<Bundle>>((serviceProvider) => serviceProvider.GetRequiredService<IBundlingConfiguration>().Build());
            services.TryAddSingleton<IBundleLookup, BundleLookup>();
            services.TryAddSingleton<IVersioningFormatter, FileVersioningFormatter>();
            services.TryAddScoped<IPathFormatter>((serviceProvider) =>
            {
                return new UrlPathFormatter(
                    serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.Request.GetBaseUrl(),
                    serviceProvider.GetRequiredService<IVersioningFormatter>());
            });

            services.TryAddScoped<IAssetResolver, AssetResolver>();

            return services;
        }

        /// <summary>
        /// Register Bunder services with the service collection.
        /// Configuration will be utilized to bind section under <paramref name="sectionName"/> to settings <see cref="BunderSettings"/>.
        /// Bundling configuration <see cref="IBundlingConfiguration"/> will load configuration from json file at <see cref="IHostingEnvironment.ContentRootPath"/> + <see cref="BunderSettings.BundlesConfigFilePath"/>. 
        /// </summary>
        /// <param name="services">Existing service collection on which to register Bunder services.</param>
        /// <param name="configuration">Established configuration from the executing application.</param>
        /// <param name="sectionName">Optional section name under the configuration <paramref name="configuration"/>. Defaults to "Bunder".</param>
        /// <param name="bundlingConfiguration">Optional custom bundling configuration that will compile list of registered bundles. By default, values under <see cref="BunderSettings"/> will be used to construct default bundling configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddBunder(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName = "Bunder",
            IBundlingConfiguration bundlingConfiguration = null)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(sectionName, nameof(sectionName));

            var bunderSection = configuration.GetSection(sectionName, required: true);
            var settings = new BunderSettings();
            bunderSection.Bind(settings);

            return AddBunder(services, settings, bundlingConfiguration);
        }
    }
}
