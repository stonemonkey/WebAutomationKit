using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace WebAutomationKit.Selenium
{
    public static class ByWaitExtensions
    {
        /// <summary>
        /// Waits maximum ElementWaitTimeoutMs for the element to become available.
        /// </summary>
        public static By WaitToBecomeAvailable(this By by, IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(ExpectedConditions.ElementExists(by));
            return by;
        }

        /// <summary>
        /// Waits maximum specified amount of time for the element to become available.
        /// </summary>
        public static By WaitToBecomeAvailable(this By by, IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(ExpectedConditions.ElementExists(by));
            return by;
        }

        /// <summary>
        /// Waits maximum ElementWaitTimeoutMs for the element to become clickable.
        /// </summary>
        public static By WaitToBecomeClickable(this By by, IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return by;
        }

        /// <summary>
        /// Waits maximum specified amount of time for the element to become clickable.
        /// </summary>
        public static By WaitToBecomeClickable(this By by, IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
            return by;
        }

        /// <summary>
        /// Waits maximum ElementWaitTimeoutMs for the element to become visible.
        /// </summary>
        public static By WaitToBecomeVisible(this By by, IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(ExpectedConditions.ElementIsVisible(by));
            return by;
        }

        /// <summary>
        /// Waits maximum specified amount of time for the element to become visible.
        /// </summary>
        public static By WaitToBecomeVisible(this By by, IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(ExpectedConditions.ElementIsVisible(by));
            return by;
        }

        /// <summary>
        /// Waits maximum ElementWaitTimeoutMs for the element to become invisible.
        /// </summary>
        public static By WaitToBecomeInvisible(this By by, IWebDriver driver)
        {
            var wait = driver.CreateWait(driver.GetElementWaitTimeoutMs());
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
            return by;
        }
        
        /// <summary>
        /// Waits maximum specified amount of time for the element to become invisible.
        /// </summary>
        public static By WaitToBecomeInvisible(this By by, IWebDriver driver, int miliseconds)
        {
            var wait = driver.CreateWait(miliseconds);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
            return by;
        }
    }
}
