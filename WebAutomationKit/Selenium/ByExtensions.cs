using OpenQA.Selenium;
using Polly;
using System;
using System.Linq;

namespace WebAutomationKit.Selenium
{
    public static class ByExtensions
    {
        /// <summary>
        /// Waits to become available, scrolls into view, then retries to wait to become available and click.
        /// </summary>
        public static void Click(this By selector, IWebDriver driver, int retryCount = 3, int miliseconds = 500)
        {
            var policy = CreateClickRetryPolicy(retryCount, miliseconds);
            selector.Click(driver, policy);
        }

        /// <summary>
        /// Waits to become available, scrolls into view, then retries to wait to become available and click.
        /// </summary>
        public static void Click(this By selector, IWebDriver driver, Policy retryPolicy)
        {
            selector
                .WaitToBecomeAvailable(driver)
                .FindElement(driver)
                .ScrollIntoView(driver);
            retryPolicy.Execute(
                () => selector
                    .WaitToBecomeClickable(driver)
                    .FindElement(driver)
                    .Click());
        }

        private static Policy CreateClickRetryPolicy(int retryCount, int miliseconds) => Policy
            .Handle<ElementClickInterceptedException>()
            .WaitAndRetry(Enumerable.Range(0, retryCount).Select(i => TimeSpan.FromMilliseconds(miliseconds)));
    }
}
