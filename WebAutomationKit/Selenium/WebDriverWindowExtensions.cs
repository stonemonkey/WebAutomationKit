using System.Drawing;
using OpenQA.Selenium;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverWindowExtensions
    {
        public static IWindow GetWindow(this IWebDriver driver) =>
            driver.ValidateNotNull(nameof(driver)).Manage().Window;

        public static int GetWindowWidth(this IWebDriver driver) => 
            GetWindow(driver).Size.Width;

        public static int GetWindowHeight(this IWebDriver driver) => 
            GetWindow(driver).Size.Height;

        /// <summary>
        /// Maximizes the current window if it is not already maximized.
        /// </summary>
        public static void Maximize(this IWebDriver driver)
        {
            GetWindow(driver).Maximize();
        }

        /// <summary>
        /// Sets the size of the outer browser window, including title bars and window borders.
        /// </summary>
        public static void SetWindowSize(this IWebDriver driver, int width, int height)
        {
            GetWindow(driver).Size = new Size(width, height);
        }
    }
}
