using OpenQA.Selenium;
using System;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Loads the page into the current tab window performing a GET.
        /// </summary>
        public static IWebDriver NavigateTo(this IWebDriver driver, string url)
        {
            driver.ValidateNotNull(nameof(driver)).Url = new Uri(url).ToString(); // poor man's validation;
            return driver;
        }

        public static object ExecuteScript(this IWebDriver driver, string js, params object[] args) =>
            ((IJavaScriptExecutor) driver.ValidateNotNull(nameof(driver))).ExecuteScript(js.ValidateNotNullOrWhitespace(nameof(js)), args);
    }
}
