# WebAutomationKit   
    
Lightweight library for fluent Web UI automations with Selenium. Contains a collection of extension methods on top of Selenium types for creating  and configuring driver instances and for manipulating elements.

__In order to use it, install WebAutomationKit NuGet package together with:__
1.   DotNetSeleniumExtras.WaitHelpers (tested with 3.11.0)
2.   Selenium.Support (tested with 3.141.0)
3.   Selenium.WebDriver (tested with 3.141.0)
4.   One or multiple browser drivers (tested with Selenium.Chrome.WebDriver 74.0.0 and Selenium.Firefox.WebDriver 0.24.0) 

WebAutomationKit NuGet package instalation will add a project folder (WebAutomatioKit.x.y.z) containing source files. The package doesn't contain/add assemblies to the project.

## Samples

Check WebAutomationKit.Tests project for running code.

### Configure and create a driver

```csharp
	var config = new WebDriverConfig
	{
		Name = "firefox",
		CommandTimeoutMin = "1",
		PageLoadTimeoutMs = "30000",
		ElementWaitTimeoutMs = "15000",
		ImplicitElementWaitTimeoutMs = "100",
		Arguments = new [] { "--private" },
		DriverLocation = Utils.GetExecutigAssemblyPath(),
		ScreenshotsFolder = Utils.GetExecutigAssemblyPath(),
    };
	
	var driver = config.CreateDriver();
	
	// ... do something with the driver
	
	driver.Dispose();
```

### Automate a Google search, default driver configuration

```csharp
	[Test]
	public void Can_automate_google_search()
	{
		string firstResultTitle = null;
		using (var driver = "Chrome".CreateDriver())
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
```
