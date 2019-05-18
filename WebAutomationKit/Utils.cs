using System.IO;
using System.Reflection;
//using Microsoft.Extensions.Configuration;

namespace WebAutomationKit
{
    public class Utils
    {
        /// <summary>
        /// Retrives the absolute path to the executing assembly.
        /// </summary>
        public static string GetExecutigAssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Retrives an instance of the configuration from a JSON file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonConfigFilePath">The path to a json file containing the configuration relative to the executing assembly.</param>
        //public static T GetConfigFromJson<T>(string jsonConfigFilePath, string sectionName = null)
        //{
        //    jsonConfigFilePath.ValidateNotNullOrWhitespace(nameof(jsonConfigFilePath));

        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(GetExecutigAssemblyPath())
        //        .AddJsonFile(jsonConfigFilePath, optional: true);
        //    var config = builder.Build();

        //    return string.IsNullOrWhiteSpace(sectionName) ?
        //        config.Get<T>() :
        //        config.GetSection(sectionName).Get<T>();
        //}
    }
}
