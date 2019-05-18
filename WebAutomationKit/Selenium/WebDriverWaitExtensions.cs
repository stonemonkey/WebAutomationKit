using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverWaitExtensions
    {
        public static IWebDriver WaitForJQueryToFinishAjaxActivity(this IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(d => (bool)driver.ExecuteScript("return jQuery.active === 0"));
            return driver;
        }

        public static IWebDriver WaitForJQueryToFinishAjaxActivity(this IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(d => (bool)driver.ExecuteScript("return jQuery.active === 0"));
            return driver;
        }

        public static IWebDriver WaitForDocumentReadyStateToBecomeComplete(this IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(d => driver.ExecuteScript("return document.readyState").Equals("complete"));
            return driver;
        }

        public static IWebDriver WaitForDocumentReadyStateToBecomeComplete(this IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(d => driver.ExecuteScript("return document.readyState").Equals("complete"));
            return driver;
        }

        /// <summary>
        /// Creates a wait object that can be used to wait for browser events or content.
        /// </summary>
        public static WebDriverWait CreateWait(this IWebDriver driver, int waitTimeOutMs) =>
            new WebDriverWait(driver.ValidateNotNull(nameof(driver)), TimeSpan.FromMilliseconds(waitTimeOutMs));

    }
}
