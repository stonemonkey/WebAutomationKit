using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverConfigExtensions
    {
        public static IWebDriver CreateDriver(this string driverName)
        {
            var config = new WebDriverConfig { Name = driverName };
            return config.CreateDriver();
        }

        public static IWebDriver CreateDriver(this WebDriverConfig config)
        {
            var name = config?.Name ?? "null";
            switch (name.ToLower())
            {
                case "chrome": return config.CreateChromeDriver();
                case "firefox": return config.CreateFirefoxDriver();
            }

            throw new ArgumentException(
                $"Invalid driver name '{config.Name}'! " +
                $"Supported drivers are: Chrome and FireFox.");
        }

        public static IWebDriver CreateChromeDriver(this WebDriverConfig config)
        {
            config.ValidateNotNull(nameof(config));
            config.ValidateDriverName("Chrome");

            var options = new ChromeOptions();
            options.PlatformName = config.PlatformName;
            options.BrowserVersion = config.BrowserVersion;
            options.AddAdditionalCapabilies(config);
            if (config.Arguments.IsNotNullOrEmpty())
            {
                options.AddArguments(config.Arguments);
            }
            var commandTimeout = GetCommandTimeout(config);

            return (config.IsLocal()
                ? new ChromeDriver(GetDriverPath(config), options, commandTimeout)
                : new RemoteWebDriver(GetRemoteHubUri(config), options.ToCapabilities(), commandTimeout))
            .ApplyCommon(config);
        }

        public static IWebDriver CreateFirefoxDriver(this WebDriverConfig config)
        {
            config.ValidateNotNull(nameof(config));
            config.ValidateDriverName("Firefox");

            var options = new FirefoxOptions();
            options.PlatformName = config.PlatformName;
            options.BrowserVersion = config.BrowserVersion;
            options.AddAdditionalCapabilies(config);
            if (config.Arguments.IsNotNullOrEmpty())
            {
                options.AddArguments(config.Arguments);
            }
            var commandTimeout = GetCommandTimeout(config);

            return (config.IsLocal()
                    ? new FirefoxDriver(GetDriverPath(config), options, commandTimeout)
                    : new RemoteWebDriver(GetRemoteHubUri(config), options.ToCapabilities(), commandTimeout))
                .ApplyCommon(config);
        }

        public static void KillAllLocalRunningDriverProcesses()
        {
            var driverProcessNames = new[] { "chromedriver", "geckodriver" };
            foreach (var name in driverProcessNames)
            {
                var processes = Process.GetProcessesByName(name);
                foreach (var process in processes)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
            }
        }

        public static IWebDriver LogConfig(this IWebDriver driver)
        {
            Console.WriteLine("=================================================================================");
            Console.WriteLine(driver.GetName() + " - configuration");
            Console.WriteLine();
            Console.WriteLine("PageLoadTimeoutMs: " + driver.GetPageLoadTimeoutMs());
            Console.WriteLine("ElementWaitTimeoutMs: " + driver.GetElementWaitTimeoutMs());
            Console.WriteLine("ImplicitElementWaitTimeoutMs: " + driver.GetImplicitElementWaitTimeoutMs());
            Console.WriteLine("=================================================================================");

            return driver;
        }

        private static IWebDriver ApplyCommon(this IWebDriver driver, WebDriverConfig config)
        {
            driver.ApplyElementWaitTimeout(config)
                .ApplyPageLoadTimeout(config)
                .ApplyImplicitWaitTimeout(config)
                .SetScreenshotsFolder(config.ScreenshotsFolder);

            return driver;
        }

        private static IWebDriver ApplyElementWaitTimeout(this IWebDriver driver, WebDriverConfig config)
        {
            if (config.ElementWaitTimeoutMs != null)
            {
                var isValid = int.TryParse(config.ElementWaitTimeoutMs, out int elementWaitTimeoutMs);
                if (!isValid)
                {
                    throw new ArgumentException($"Invalid {nameof(config.ElementWaitTimeoutMs)}!");
                }
                driver.SetElementWaitTimeoutMs(elementWaitTimeoutMs);
            }
            else
            {
                driver.SetElementWaitTimeoutMs(5000); // 5 seconds
            }

            return driver;
        }

        private static IWebDriver ApplyPageLoadTimeout(this IWebDriver driver, WebDriverConfig config)
        {
            if (config.PageLoadTimeoutMs != null)
            {
                var isValid = int.TryParse(config.PageLoadTimeoutMs, out int pageLoadTimeoutMs);
                if (!isValid)
                {
                    throw new ArgumentException($"Invalid {nameof(config.PageLoadTimeoutMs)}!");
                }
                driver.SetPageLoadTimeoutMs(pageLoadTimeoutMs);
            }
            else
            {
                driver.SetPageLoadTimeoutMs(60000); // 60 seconds
            }

            return driver;
        }

        private static IWebDriver ApplyImplicitWaitTimeout(this IWebDriver driver, WebDriverConfig config)
        {
            if (config.ImplicitElementWaitTimeoutMs != null)
            {
                var isValid = int.TryParse(config.ImplicitElementWaitTimeoutMs, out int implicitElementWaitTimeoutMs);
                if (!isValid)
                {
                    throw new ArgumentException($"Invalid {nameof(config.ImplicitElementWaitTimeoutMs)}!");
                }
                driver.SetImplicitElementWaitTimeoutMs(implicitElementWaitTimeoutMs);
            }
            else
            {
                driver.SetImplicitElementWaitTimeoutMs(0); // no wait
            }

            return driver;
        }

        private static void AddAdditionalCapabilies(this DriverOptions options, WebDriverConfig config)
        {
            if (config.AdditionalCapabilities != null)
            {
                foreach (var capability in config.AdditionalCapabilities)
                {
                    options.AddAdditionalCapability(capability.Key, capability.Value);
                }
            }
        }

        private static bool IsLocal(this WebDriverConfig config) =>
            !string.IsNullOrWhiteSpace(config.DriverLocation) && !config.DriverLocation.StartsWith("http", StringComparison.OrdinalIgnoreCase);

        private static string GetDriverPath(this WebDriverConfig config)
        {
            var folder = config.DriverLocation;
            if (folder == null)
            {
                return Utils.GetExecutigAssemblyPath();
            }
            if (!Path.IsPathRooted(folder))
            {
                folder = Path.Combine(Utils.GetExecutigAssemblyPath(), folder);
            }
            return folder;
        }

        private static Uri GetRemoteHubUri(this WebDriverConfig config)
        {
            if (config.DriverLocation == null)
            {
                return new Uri("http://127.0.0.1:4444/wd/hub/");
            }
            else
            {
                return new Uri(config.DriverLocation);
            }
        }

        private static TimeSpan GetCommandTimeout(this WebDriverConfig config)
        {
            var isValid = int.TryParse(config.CommandTimeoutMin, out var commandTimeoutMin);
            if (!isValid)
            {
                commandTimeoutMin = 1; // default
            }

            return TimeSpan.FromMinutes(commandTimeoutMin);
        }

        public static void ValidateDriverName(this WebDriverConfig config, string driverName)
        {
            if (!string.Equals(config.Name, driverName, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Invalid driver name '{config.Name}'! " +
                    $"Can't create {driverName}Driver from a configuration that specifies a diffrent driver than {driverName}.");
            }
        }
    }
}
