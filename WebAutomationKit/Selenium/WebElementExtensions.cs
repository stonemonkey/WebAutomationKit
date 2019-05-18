using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace WebAutomationKit.Selenium
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// Scrolls element center into the view.
        /// </summary>
        public static IWebElement ScrollIntoView(this IWebElement element, IWebDriver driver)
        {
            element.ValidateNotNull(nameof(element));

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView(false);", element);

            return element;
        }

        /// <summary>
        /// Selects the content of an element and replaces it (Ctrl+A, <text>).
        /// </summary>
        public static IWebElement ReplaceContentWith(this IWebElement element, IWebDriver driver, string text)
        {
            element.ValidateNotNull(nameof(element));

            var actions = new Actions(driver)
                .MoveToElement(element)
                .Click(element)
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(text);
            actions.Perform();

            return element;
        }

        /// <summary>
        /// Moves the mouse to the specified element forcing a scroll.
        /// </summary>
        /// <remarks>The scroll is not performed in Firefox</remarks>
        public static IWebElement MoveToElement(this IWebElement instance, IWebDriver driver)
        {
            instance.ValidateNotNull(nameof(instance));

            var actions = new Actions(driver);
            actions.MoveToElement(instance);
            actions.Perform();

            return instance;
        }

        /// <summary>
        /// Clears the value property of the element.
        /// </summary>
        public static IWebElement ClearValue(this IWebElement element, IWebDriver driver)
        {
            element.ValidateNotNull(nameof(element));

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].value = '';", element);

            return element;
        }
    }
}
