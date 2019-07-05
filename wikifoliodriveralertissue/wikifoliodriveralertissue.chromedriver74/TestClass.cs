using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace wikifoliodriveralertissue.chromedriver74
{
    [TestFixture]
    public class Class1
    {
        private IWebDriver driver;

        [Test]
        public void AlertShouldNotDissappearAfterExceptionIsThrown()
        {
            // GIVEN: a page that is marked as dirty has been opened
            driver.Url = "https://codepen.io/grafbumsdi/full/PrdzOo";
            var element =
                new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementExists(By.CssSelector("div#result-iframe-wrap")));
            new Actions(driver).Click(element).Perform();

            // WHEN: I try to navigate away
            // THEN: I will not receive any UnhandledAlertException yet
            Assert.That(
                () => driver.Url = "https://www.google.at",
                Throws.Nothing);
            // THEN: an alert will be available and stay available
            Assert.That(() => driver.SwitchTo().Alert(), Throws.Nothing, "First time fetching an alert should work.");
            Assert.That(() => driver.SwitchTo().Alert(), Throws.Nothing, "Second time fetching an alert should work.");

            // WHEN: I try to interact somehow with the driver
            // THEN: an UnhandledAlertException will be thrown
            Assert.That(
                () => driver.FindElements(By.CssSelector("div#result-iframe-wrap")),
                Throws.TypeOf<UnhandledAlertException>());
            // THEN: an alert will be available and stay available
            Assert.That(() => driver.SwitchTo().Alert(), Throws.Nothing, $"Fetching an alert after thrown {nameof(UnhandledAlertException)} should work.");
        }

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            this.driver?.Quit();
            this.driver?.Dispose();
        }
    }
}
