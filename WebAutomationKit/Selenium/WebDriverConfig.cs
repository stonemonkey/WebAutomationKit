using System.Collections.Generic;

namespace WebAutomationKit.Selenium
{
    public class WebDriverConfig
    {
        /// <summary>
        /// The name of the driver (case insensitive) e.g. Chrome, Firefox.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The platform name e.g. WIN10, Linux.
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// The version of the browser e.g. 85 for Chrome, 27 for Firefox.
        /// </summary>
        public string BrowserVersion { get; set; }

        /// <summary>
        /// Full path to the folder containing driver executable or the URL containing the address of the WebDriver remote server. 
        /// </summary>
        public string DriverLocation { get; set; }

        /// <summary>
        /// Path to the folder where to save browser screen shot files. 
        public string ScreenshotsFolder { get; set; }

        /// <summary>
        /// The amount of time the driver should wait for a command to finish execution e.g. navigating to a page.
        /// </summary>
        public string CommandTimeoutMin { get; set; }

        /// <summary>
        /// The amount of time the driver wait extension methods should wait for an element to change state e.g. become awailable.
        /// </summary>
        public string ElementWaitTimeoutMs { get; set; }

        /// <summary>
        /// The amount of time the driver should wait for a page to load when setting the Url (NavigatTo).
        /// </summary>
        public string PageLoadTimeoutMs { get; set; }

        /// <summary>
        /// The amount of time the driver should wait when searching for an element if it's not imediately present.
        /// </summary>
        public string ImplicitElementWaitTimeoutMs { get; set; }

        /// <summary>
        /// Command line arguments added when lunching the driver process e.g. --incognito.
        /// </summary>
        public string[] Arguments { get; set; }

        public Dictionary<string, string> AdditionalCapabilities { get; set; }
    }
}
