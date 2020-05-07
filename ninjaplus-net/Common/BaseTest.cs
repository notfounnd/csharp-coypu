using System;
using System.IO;
using Coypu;
using Coypu.Drivers.Selenium;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace NinjaPlus.Common
{
    // public => acessada por qualquer código (declaração minusculo)
    // public String nome
    
    // private => acessado somente pela classe (declaração com "_" no inicio e tudo minusculo)
    // private String _nome

    // protected => acessado somente acessa por ela ou por herança (declaração primeira maiusculo)
    // public String Nome

    public class BaseTest
    {
        protected BrowserSession Browser;
        
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            var sessionConfig = new SessionConfiguration
            {
                AppHost = "http://ninjaplus-web",
                Port = 5000,
                SSL = false,
                Driver = typeof(SeleniumWebDriver),
                // Browser tratado pelo arquivo JSON
                // Browser = Coypu.Drivers.Browser.Chrome,
                Timeout = TimeSpan.FromSeconds(10)
                // Configuração nativa para trabalhar com elementos invisíveis
                // ConsiderInvisibleElements = true
            };

            if (config["browser"].Equals("chrome"))
            {
                sessionConfig.Browser = Coypu.Drivers.Browser.Chrome;
            }

            if (config["browser"].Equals("firefox"))
            {
                sessionConfig.Browser = Coypu.Drivers.Browser.Firefox;
            }

            Browser = new BrowserSession(sessionConfig);

            Browser.MaximiseWindow();
        }

        public string CoverPath()
        {
            var outputPath = Environment.CurrentDirectory;
            return outputPath + "\\Images\\";
        }

        public void TakeScreenshot()
        {
            var resultId = TestContext.CurrentContext.Test.ID;
            var shotPath = Environment.CurrentDirectory + "\\Screenshots";

            if(!Directory.Exists(shotPath))
            {
                Directory.CreateDirectory(shotPath);
            }

            var screenshot = $"{shotPath}\\{resultId}.png";

            Browser.SaveScreenshot(screenshot);
            TestContext.AddTestAttachment(screenshot);
        }

        [TearDown]
        public void Finish()
        {
            try
            {
                TakeScreenshot();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro ao capturar o Screeshot :(");
                throw new Exception(e.Message);
            }
            finally
            {
                Browser.Dispose();
            }
        }
    }
}