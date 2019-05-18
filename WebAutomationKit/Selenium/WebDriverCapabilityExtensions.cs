using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverCapabilityExtensions
    {
        public static string GetName(this IWebDriver driver) =>
            driver.GetCapability("browserName");

        public static ICapabilities GetCapabilities(this IWebDriver driver) =>
            ((RemoteWebDriver)driver.ValidateNotNull(nameof(driver))).Capabilities;

        public static string GetCapability(this IWebDriver driver, string name) =>
            driver.GetCapabilities()[name.ValidateNotNullOrWhitespace(nameof(name))].ToString();
    }
}
