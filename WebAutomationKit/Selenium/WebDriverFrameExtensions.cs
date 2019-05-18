using System;
using System.Threading;
using OpenQA.Selenium;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverFrameExtensions
    {
        /// <summary>
        /// Selects a frame by its (zero-based) index.
        /// </summary>
        public static IWebDriver SwitchToFrame(this IWebDriver driver, int frameIndex)
        {
            driver.GetTargetLocator().Frame(frameIndex);
            driver.ChromeWorkaround();

            return driver;
        }

        /// <summary>
        /// Selects a frame by its name or id.
        /// </summary>
        public static IWebDriver SwitchToFrame(this IWebDriver driver, string frameName)
        {
            driver.GetTargetLocator().Frame(frameName);
            driver.ChromeWorkaround();

            return driver;
        }

        /// <summary>
        /// Selects either the first frame on the page or the main document when a page contains iFrames.
        /// </summary>
        public static IWebDriver SwitchToDefaultContent(this IWebDriver driver)
        {
            driver.GetTargetLocator().DefaultContent();
            driver.ChromeWorkaround();

            return driver;
        }

        public static ITargetLocator GetTargetLocator(this IWebDriver driver) =>
            driver.ValidateNotNull(nameof(driver)).SwitchTo();

        private static void ChromeWorkaround(this IWebDriver driver)
        {
            // TODO: Create sample to reproduce and reise a ticket to ChromeDriver.
            // This is a workaround for a bug in ChromeDriver. In Firefox works as expected.
            if (string.Equals(driver.GetName(), "Chrome", StringComparison.OrdinalIgnoreCase))
            {
                Thread.Sleep(2000);
            }
        }
    }
}
