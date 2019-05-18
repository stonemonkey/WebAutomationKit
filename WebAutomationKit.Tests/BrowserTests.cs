using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using WebAutomationKit.Selenium;

namespace WebAutomationKit.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class BrowserTests
    {
        private const string ChromeProcessName = "chromedriver";
        private const string FirefoxProcessName = "geckodriver";

        private static readonly string[] Browsers = { "chrome", "Firefox" };

        private static readonly object[] Configurations =
        {
            new object[] { new WebDriverConfig { Name = "Chrome" }, ChromeProcessName },
            new object[] { new WebDriverConfig
                {
                    Name = "Chrome",
                    CommandTimeoutMin = "1",
                    PageLoadTimeoutMs = "30000",
                    ElementWaitTimeoutMs = "15000",
                    ImplicitElementWaitTimeoutMs = "100",
                    Arguments = new [] { "--incognito", "--dns-prefetch-disable" },
                    DriverLocation = Utils.GetExecutigAssemblyPath(),
                    ScreenshotsFolder = Utils.GetExecutigAssemblyPath(),
                }, ChromeProcessName },

            new object[] { new WebDriverConfig { Name = "Firefox" }, FirefoxProcessName },
            new object[] { new WebDriverConfig
                {
                    Name = "firefox",
                    CommandTimeoutMin = "1",
                    PageLoadTimeoutMs = "30000",
                    ElementWaitTimeoutMs = "15000",
                    ImplicitElementWaitTimeoutMs = "100",
                    Arguments = new [] { "--private" },
                    DriverLocation = Utils.GetExecutigAssemblyPath(),
                    ScreenshotsFolder = Utils.GetExecutigAssemblyPath(),
                }, FirefoxProcessName },
        };

        [TearDown]
        public void KillAllrunningDriverProcesses()
        {
            WebDriverConfigExtensions.KillAllRunningDriverProcesses();
        }

        [Test, TestCaseSource("Configurations")]
        public void Can_open_close_browser(WebDriverConfig config, string processName)
        {
            var driver = config.CreateDriver();
            Assert.IsNotNull(GetProcess(processName));

            driver.Dispose();

            Assert.IsNull(GetProcess(processName));
        }

        [Test, TestCaseSource("Configurations")]
        public void Can_open_two_browsers(WebDriverConfig config, string processName)
        {
            var driver = config.CreateDriver();
            Assert.AreEqual(1, GetProcesses(processName).Count());

            var browser2 = config.CreateDriver();

            Assert.AreEqual(2, GetProcesses(processName).Count());

            browser2.Dispose();
            driver.Dispose();

            Assert.AreEqual(0, GetProcesses(processName).Count());
        }

        [Test, TestCaseSource("Browsers")]
        public void Can_save_screen_shot_to_file(string browserName)
        {
            string filePath = null;
            using (var driver = browserName.CreateDriver())
            {
                filePath = driver.SaveScreenToPng();
            }

            Assert.IsTrue(File.Exists(filePath));
            File.Delete(filePath); // cleanup
        }

        [Test, TestCaseSource("Browsers")]
        public void Can_access_capabilities(string browserName)
        {
            using (var driver = browserName.CreateDriver())
            {
                Console.WriteLine($"Browser name: {driver.GetName()}");
            }

            Assert.Pass();
        }

        [Test, TestCaseSource("Browsers")]
        public void Can_automate_google_search(string browserName)
        {
            string firstResultTitle = null;
            using (var driver = browserName.CreateDriver())
            {
                driver.NavigateTo("https://google.com");

                var queryInput = "//input[@name='q']";
                queryInput.ToXPathBy()
                    .WaitToBecomeAvailable(driver)
                    .FindElement(driver)
                    .SendKeys("google" + Keys.Return);

                var resultTitles = "//div[@class='rc']/div[@class='r']/a/h3";
                firstResultTitle = resultTitles.ToXPathBy()
                    .WaitToBecomeAvailable(driver)
                    .FindElements(driver)
                    .First()
                    .Text;
            }
            Assert.AreEqual("Google", firstResultTitle);
        }

        [TestCase("chrome", 1000)]
        [TestCase("firefox", 2000)] // firefox driver is slower
        public void Can_set_implicit_wait_timeout(string browserName, int timeoutMs)
        {
            using (var driver = browserName.CreateDriver())
            {
                // 1. Verify initial timeout is 0
                Assert.AreEqual(0, driver.GetImplicitElementWaitTimeoutMs());

                driver.NavigateTo($"{Utils.GetExecutigAssemblyPath()}/testPage.html");

                // 2. Verify root element is there right away
                const string rootElement = "root";
                rootElement.ToIdBy()
                    .FindElement(driver); // this will throw if element is not there

                // 3. Set a timer to add a new element after <timeoutMs>
                const string newElement = "new_element";
                driver.ExecuteScript(
                    "setTimeout(function () {" +
                        "newElement = document.createElement('p');" +
                        $"newElement.setAttribute('id', '{newElement}');" +
                        "var text = document.createTextNode('New element');" +
                        "newElement.appendChild(text);" +
                        $"document.getElementById('{rootElement}').appendChild(newElement);" +
                    $"}}, {timeoutMs});");

                // 4. Verify the element is not there right away
                var exception = Assert.Throws<NoSuchElementException>(() =>
                    newElement.ToIdBy()
                        .FindElement(driver));
                Assert.That(exception.Message.Contains(newElement));

                // 5. Set the driver to implicitly wait 3 seconds for elements to appear in the DOM
                driver.SetImplicitElementWaitTimeoutMs(timeoutMs);
                Assert.AreEqual(timeoutMs, driver.GetImplicitElementWaitTimeoutMs());

                // 6. Verify element is found because the driver waits 3 seconds to appear in the DOM
                newElement.ToIdBy()
                    .FindElement(driver);
            }
        }

        private Process GetProcess(string name)
        {
            return GetProcesses(name)
                .SingleOrDefault();
        }

        private Process[] GetProcesses(string name)
        {
            Thread.Sleep(50); // driver process may take little while to close or open 

            return Process.GetProcessesByName(name);
        }
    }
}
