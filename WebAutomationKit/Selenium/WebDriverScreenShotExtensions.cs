using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace WebAutomationKit.Selenium
{
    public static class WebDriverScreenShotExtensions
    {
        private static ConcurrentDictionary<string, string> _screenshotsFolders = 
            new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Sets the folder where screen shot files will be saved. Setting it to null will
        /// default to the executing assembly folder.
        /// </summary>
        public static IWebDriver SetScreenshotsFolder(this IWebDriver driver, string folder)
        {
            // TODO: add validation, pay attention new Uri(folder) has troubles with spaces ...
            _screenshotsFolders.TryRemove(driver.CurrentWindowHandle, out string existingFolder);
            if (!string.IsNullOrWhiteSpace(folder))
            {
                _screenshotsFolders.TryAdd(driver.CurrentWindowHandle, folder);
            }

            return driver;
        }

        /// <summary>
        /// Retrives the folder where screen shot files are saved.
        /// </summary>
        /// <returns>Defaults to executing assembly folder if not previously set or it was set to null.</returns>
        public static string GetScreenshotsFolder(this IWebDriver driver)
        {
            if(_screenshotsFolders.TryGetValue(driver.CurrentWindowHandle, out string existingFolder))
            {
                return existingFolder;
            }

            return Utils.GetExecutigAssemblyPath();
        }

        /// <summary>
        /// Saves current browser screen to a png file at the location specified with SetScreenshotsFolder 
        /// or relative to the current assembly if the not previously set.
        /// </summary>
        /// <param name="pngFileName">The png file name with or without extension. The parameter is
        /// optional. If it is not specified the file will be named '<timestamp> <browsername>.png'.
        /// </param>
        /// <returns>Absolute file path.</returns>
        public static string SaveScreenToPng(this IWebDriver driver, string pngFileName = null)
        {
            var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}-{driver.GetName()}";
            if (pngFileName != null)
            {
                fileName += $"-{pngFileName}";
            }
            const string pngExtension = ".png";
            var extension = Path.GetExtension(pngFileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fileName += pngExtension;
            }

            if (string.Equals(pngExtension, extension, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid file extension!", nameof(pngFileName));
            }

            var folder = driver.GetScreenshotsFolder();
            if (!Path.IsPathRooted(folder))
            {
                folder = Path.Combine(Utils.GetExecutigAssemblyPath(), folder);
            }

            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, fileName);
            var screenshot = driver.TakeScreenshot();
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

            return filePath;
        }
    }
}
