using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace login
{
    public interface IDailyAction
    {
        string ActionName { get; }

        void Configure(IConfiguration cfg);
        void DoDailyAction(IWebDriver driver);

        string GetLog();
    }
}