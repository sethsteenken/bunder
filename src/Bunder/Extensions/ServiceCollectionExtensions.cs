using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Bunder
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register a custom class and interface that represents a given configuration section on the <see cref="IConfiguration"/> instance.
        /// </summary>
        /// <typeparam name="TSettingsType">Class that represents the configuration section.</typeparam>
        /// <param name="services">Existing service collection.</param>
        /// <param name="sectionName">Name of the section found in <see cref="IConfiguration"/> that will be read into <typeparamref name="TSettingsType"/>.</param>
        /// <param name="singleton">Whether or not <typeparamref name="TSettingsType"/> is registered as a singleton. If not, configuration settings can be altered without an application restart at the cost of performance.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfigurationSettings<TSettingsType>(this IServiceCollection services, string sectionName, bool singleton)
            where TSettingsType : class, new()
        {
            Guard.IsNotNull(services, nameof(services));

            var section = GetConfigurationSection(services, sectionName);
            services.Configure<TSettingsType>(section);

            // Register as singleton or scoped. 
            // Singleton is one instance for the app. 
            // Scoped settings will be read on every request (no app restart on setting changes at the drawback of performance)
            // IOptionsSnapshot is also registered under Scoped to allow this always-updated feature.
            if (singleton)
                services.AddSingleton<TSettingsType>(sp => sp.GetRequiredService<IOptions<TSettingsType>>().Value);
            else
                services.AddScoped<TSettingsType>(sp => sp.GetRequiredService<IOptionsSnapshot<TSettingsType>>().Value);

            return services;
        }

        /// <summary>
        /// Get a specific section by name from the registered <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="services">Existing service collection.</param>
        /// <param name="sectionName">Name of the section in the configuration instance.</param>
        /// <returns></returns>
        public static IConfigurationSection GetConfigurationSection(this IServiceCollection services, string sectionName)
        {
            Guard.IsNotNull(services, nameof(services));
            Guard.IsNotNull(sectionName, nameof(sectionName));

            var section = services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection(sectionName);
            if (section == null || !section.Exists())
                throw new ApplicationException($"Configuration section '{sectionName}' was not found.");
            return section;
        }
    }
}
