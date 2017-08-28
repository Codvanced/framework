using System.Configuration;

namespace IOC.FW.Configuration
{
    public class ConfigManager
    {
        public static IoCFrameworkSection GetConfig()
        {
            var section = ConfigurationManager.GetSection(ConfigurationVariables.IocFrameworkSectionKey);
            return (IoCFrameworkSection)section;
        }
    }
}