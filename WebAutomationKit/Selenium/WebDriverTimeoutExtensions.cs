using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverTimeoutExtensions
    {
        public static ITimeouts GetTimeouts(this IWebDriver driver) =>
            driver.ValidateNotNull(nameof(driver)).Manage().Timeouts();

        private static ConcurrentDictionary<string, int> _elementWaitTimeouts =
            new ConcurrentDictionary<string, int>();


        /// <summary>
        /// Sets the amount of time the driver wait extension methods should wait for an element to change state e.g. become awailable.
        /// </summary>
        public static IWebDriver SetElementWaitTimeoutMs(this IWebDriver driver, int miliseconds)
        {
            _elementWaitTimeouts.TryRemove(driver.CurrentWindowHandle, out int existingTimeout);
            _elementWaitTimeouts.TryAdd(driver.CurrentWindowHandle, miliseconds);

            return driver;
        }

        /// <summary>
        /// Retrives the amount of time the driver wait extension methods should wait for an element to change state e.g. become awailable.
        /// </summary>
        /// <returns>Defaults to 0 seconds if not previously set.</returns>
        public static int GetElementWaitTimeoutMs(this IWebDriver driver)
        {
            if (_elementWaitTimeouts.TryGetValue(driver.CurrentWindowHandle, out int existingTimeout))
            {
                return existingTimeout;
            }

            return 0;
        }

        private static ConcurrentDictionary<string, int> _implicitElementWaitTimeouts =
            new ConcurrentDictionary<string, int>();

        /// <summary>
        /// Sets the amount of time the driver should wait when searching for an element if it's not imediately present.
        /// </summary>
        public static IWebDriver SetImplicitElementWaitTimeoutMs(this IWebDriver driver, int miliseconds)
        {
            driver.GetTimeouts().ImplicitWait = TimeSpan.FromMilliseconds(miliseconds);

            // ITimeouts.ImplicitWait getter is not implemented by the Selenium WebDriver
            // that is why we save it locally (https://github.com/SeleniumHQ/selenium/issues/6055)
            _implicitElementWaitTimeouts.TryRemove(driver.CurrentWindowHandle, out int existingTimeout);
            _implicitElementWaitTimeouts.TryAdd(driver.CurrentWindowHandle, miliseconds);

            return driver;
        }

        /// <summary>
        /// Retrives the amount of time the driver should wait when searching for an element if it's not imediately present.
        /// </summary>
        /// <exception cref="InvalidOperationException">If a value was not set before.</exception>
        public static int GetImplicitElementWaitTimeoutMs(this IWebDriver driver)
        {
            if (_implicitElementWaitTimeouts.TryGetValue(driver.CurrentWindowHandle, out int existingTimeout))
            {
                return existingTimeout;
            }

            throw new InvalidOperationException($"No value set yet for ImplicitElementWaitTimeout!");
        }

        private static ConcurrentDictionary<string, int> _pageLoadTimeouts =
            new ConcurrentDictionary<string, int>();

        /// <summary>
        /// Sets the amount of time the driver should wait for a page to load when setting the Url (NavigatTo).
        /// </summary>
        public static IWebDriver SetPageLoadTimeoutMs(this IWebDriver driver, int miliseconds)
        {
            driver.GetTimeouts().PageLoad = TimeSpan.FromMilliseconds(miliseconds);

            // ITimeouts.PageLoad getter is not implemented by the Selenium WebDriver
            // that is why we save it locally (https://github.com/SeleniumHQ/selenium/issues/6055)
            _pageLoadTimeouts.TryRemove(driver.CurrentWindowHandle, out int existingTimeout);
            _pageLoadTimeouts.TryAdd(driver.CurrentWindowHandle, miliseconds);

            return driver;
        }

        /// <summary>
        /// Retrives the amount of time the driver should wait for a page to load when setting the Url (NavigatTo).
        /// </summary>
        /// <exception cref="InvalidOperationException">If a value was not set before.</exception>
        public static int GetPageLoadTimeoutMs(this IWebDriver driver)
        {
            if (_pageLoadTimeouts.TryGetValue(driver.CurrentWindowHandle, out int existingTimeout))
            {
                return existingTimeout;
            }

            throw new InvalidOperationException($"No value set yet for PageLoadTimeout!");
        }
    }
}
