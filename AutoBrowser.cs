
using System;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace login
{
    public class AutoBrowser
    {
        StringBuilder sb;

        IDailyAction[] actions;

        IConfiguration cfg;
        public AutoBrowser(IEnumerable<IDailyAction> actions,IConfiguration cfg)
        {
            this.actions = actions.ToArray();
            this.cfg = cfg;
            sb = new StringBuilder();
        }

        public void Exec()
        {
            using (var driver = new FirefoxDriver())
            {
                foreach(IDailyAction action in actions){
                    try
                    {
                        action.Configure(cfg);
                        action.DoDailyAction(driver);
                    }
                    catch (Exception e)
                    {
                        sb.AppendLine($"Eccezione nell'esecuzione dell'azione {action.ActionName}");                    
                        sb.AppendLine($"{e.Message}");
                        sb.AppendLine($"{action.GetLog()}");
                    }
                }
            }
            Console.WriteLine(sb.ToString());
        }

    }
}