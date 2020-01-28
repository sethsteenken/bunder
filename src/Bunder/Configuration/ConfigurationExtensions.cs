using Microsoft.Extensions.Configuration;

namespace Bunder
{
    internal static class ConfigurationExtensions
    {
        /// <summary>
        /// Get section from configuration. Optionally force the section to be required.
        /// </summary>
        /// <param name="configuration">Established configuration.</param>
        /// <param name="name">Name of the section. "AppSettings", "ConnectionStrings", etc. </param>
        /// <param name="required">Whether the section is required to exist in the configuration.</param>
        /// <returns></returns>
        public static IConfigurationSection GetSection(this IConfiguration configuration, string name, bool required)
        {
            Guard.IsNotNull(configuration, nameof(configuration));
            Guard.IsNotNull(name, nameof(name));

            var section = configuration.GetSection(name);
            if (!required)
                return section;

            if (section == null || !section.Exists())
                throw new BundleConfigurationException($"Required configuration section '{name}' was not found.");

            return section;
        }
    }
}
