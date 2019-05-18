using OpenQA.Selenium;

namespace WebAutomationKit.Selenium
{
    public static class ByExtensions
    {
        public static By ToIdBy(this string id)
        {
            id.ValidateNotNullOrWhitespace(nameof(id));

            return By.Id(id);
        }

        public static By ToXPathBy(this string xpath)
        {
            xpath.ValidateNotNullOrWhitespace(nameof(xpath));

            return By.XPath(xpath);
        }

        public static By ToClassNameBy(this string className)
        {
            className.ValidateNotNullOrWhitespace(nameof(className));

            return By.ClassName(className);
        }
    }
}
