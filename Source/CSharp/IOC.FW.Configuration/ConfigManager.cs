using System.Configuration;

namespace IOC.FW.Configuration
{
    public class ConfigManager
    {
        private const string IocFrameworkSectionKey = "iocFramework";

        public static IoCFrameworkSection GetConfig()
        {
            var section = ConfigurationManager.GetSection(IocFrameworkSectionKey);
            return (IoCFrameworkSection)section;
        }
    }
}