using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace login
{
    public class SixthContinentAmazonLogin : IDailyAction
    {
        string url = "https://sixthcontinent.com/";
        string cssSelAccedi = "sxc-popover.col > a:nth-child(1)";
        string loginWithAmazonId = "LoginWithAmazon";
        string emailTagId = "ap_email";
        string passwordTagId = "ap_password";
        string submitButtonSel = ".button-text";
        string profileTagSel = ".profile-img";

        string email;
        string password;

        StringBuilder sb;
        public string ActionName { get; private set; }
        public SixthContinentAmazonLogin()
        {
            sb = new StringBuilder();
        }

        public void Configure(IConfiguration cfg){
            email = cfg["sixthcontinent:amazon:email"]??"";
            password = cfg["sixthcontinent:amazon:password"]??"";

        }
        public string GetLog()
        {
            return sb.ToString();
        }

        public void DoDailyAction(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(@"https://sixthcontinent.com/");
            // handle di sixthcontinent
            string sixthcontinentHandle = driver.CurrentWindowHandle;
            sb.AppendLine($"Sixthcontinent window handle {sixthcontinentHandle}");

            sb.AppendLine("Ricerca link Accedi");

            IWebElement l1 = driver.FindElement(By.CssSelector(cssSelAccedi));
            if (l1 == null)
            {
                sb.AppendLine($"Link non trovato con selector {cssSelAccedi}");
                return;
            }

            var jsToBeExecuted = $"window.scroll(0, {l1.Location.Y});";
            ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            var readyElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(cssSelAccedi)));
            sb.AppendLine("Click su accedi");
            readyElement.Click();


            sb.AppendLine("Aspetta elemento Login With Amazon");
            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            readyElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(loginWithAmazonId)));

            PopupWindowFinder finder = new PopupWindowFinder(driver);
            sb.AppendLine("Click su Login with Amazon");
            string amazonWindowHandle = finder.Click(readyElement);
            //clickableElement.Click();

            driver.WindowHandles.ToList().ForEach(wh => Console.WriteLine());

            driver.SwitchTo().Window(amazonWindowHandle);

            sb.AppendLine("Aspetta email text field");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id(emailTagId)));

            sb.AppendLine("Set username");
            var textEl = driver.FindElement(By.Id(emailTagId));
            textEl.SendKeys(email);

            sb.AppendLine("Set password");
            textEl = driver.FindElement(By.Id(passwordTagId));
            textEl.SendKeys(password);

            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            readyElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(submitButtonSel)));
            readyElement.Click();
            //readyElement

            driver.SwitchTo().Window(sixthcontinentHandle);

            wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            readyElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(profileTagSel)));
        }
    }
}