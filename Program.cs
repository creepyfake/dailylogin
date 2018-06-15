using System;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace login
{
    class Program
    {
        static int Main(string[] args)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")))
            {
                Console.WriteLine("Impossibile trovare il file appsettings.json con la configurazione");                
                return -1;
            }

            IConfiguration cfgRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();



            Console.WriteLine("Hello World!");

            Container c = new Container();
            c.Collection.Register<IDailyAction>(Assembly.GetCallingAssembly());
            c.Register<AutoBrowser>();
            c.RegisterInstance<IConfiguration>(cfgRoot);
            c.Verify();


            c.GetInstance<AutoBrowser>().Exec();
            return 0;
        }
    }
}
