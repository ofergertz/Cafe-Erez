using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace CafeErez.Shared.Infrastructure.ConfigurationRepository
{
    public class ConfigurationRepository
    {
        public string GetValueByKey(string sectionName, string key)
        {
            var AppSetting = new ConfigurationBuilder()
                           .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                           .AddJsonFile(@"C:\Repos\Cafe Erez\CafeErez\CafeErez\Server\appsettings.json")
                           .Build();

            return AppSetting.GetSection(sectionName)[key];
        }
    }
}
