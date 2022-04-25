using Microsoft.Extensions.Configuration;

namespace Hannet.Api.Extentsions
{
    public static class ConfigurationExtensions
    {
        public static string GetDefaultConnectionString(this IConfiguration configuration)
           => configuration.GetConnectionString("HannetSolutionDB");
    }
}
