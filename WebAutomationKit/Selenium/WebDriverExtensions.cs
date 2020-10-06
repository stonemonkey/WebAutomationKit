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

        public static object ClickElementById(this IWebDriver driver, string id) =>
            driver.ExecuteScript($"document.getElementById('{id}').click();");

        public static object ClickElementByXPath(this IWebDriver driver, string xPath) =>
            driver.ExecuteScript($"document.evaluate(\"{xPath}\", document, null, 9).singleNodeValue.click();");

        public static object ScrollToElementById(this IWebDriver driver, string id) =>
            driver.ExecuteScript($"document.getElementById('{id}').scrollIntoView(false);");

        public static object ScrollToElementByXPath(this IWebDriver driver, string xPath) =>
            driver.ExecuteScript($"document.evaluate(\"{xPath}\", document, null, 9).singleNodeValue.scrollIntoView(false);");

        public static object ExecuteScript(this IWebDriver driver, string js, params object[] args) =>
            ((IJavaScriptExecutor) driver.ValidateNotNull(nameof(driver))).ExecuteScript(js.ValidateNotNullOrWhitespace(nameof(js)), args);
    }
}
