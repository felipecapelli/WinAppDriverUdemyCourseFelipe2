using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;

namespace MSTestOverview
{
    [TestClass]
    public class AlarmsAndClockSmokeTest
    {
        /*
         ***Ordem de execução:***
         
            [ClassInitialize]
                [TestInitialize]
                    [TestMethod]
                [TestCleanup]
            [ClassCleanup]
        */


        static WindowsDriver<WindowsElement> sessionAlarms;

        [ClassInitialize]
        public static void PrepareForTestingAlarms(TestContext testContext)
        {
            Debug.WriteLine("Hello ClassInitialize");
            AppiumOptions capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
            AlarmsAndClockSmokeTest.sessionAlarms = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/"), capabilities);
        }

        [ClassCleanup]
        public static void CleanupAfterAllAlarmsTests()
        {
            Debug.WriteLine("Hello ClassCleanup");
            if (sessionAlarms != null)
            {
                sessionAlarms.Quit();
            }
        }

        [TestInitialize]
        public void BeforeATest()
        {
            Debug.WriteLine("Before a test, calling TestInitialize");
        }

        [TestCleanup]
        public void AfterAtest()
        {
            Debug.WriteLine("After a test, calling TestCleanup");
        }

        [TestMethod]
        public void JustAnotherTest()
        {
            Debug.WriteLine("Hello another test.");

            //Assert.AreEqual("1", "1");
        }

        [TestMethod]
        public void TestAlarmsAndClockIsLaunchingSuccessfully()
        {

            Assert.AreEqual("Alarmes e Relógio", sessionAlarms.Title, false,
                    $"Actual title doesn't match expected title: {sessionAlarms.Title}"
                );
        }

        [TestMethod]
        public void VerifyNewClockCanBeAdded()
        {
            sessionAlarms.FindElementByAccessibilityId("ClockButton").Click();
            sessionAlarms.FindElementByName("Adicionar nova cidade").Click();

            //MANEIRAS DE FAZER O CÓDIGO AGUARDAR
            //System.Threading.Thread.Sleep(1000); //Aqui sempre espera 1000 milisegundos
            WebDriverWait waitForMe = new WebDriverWait(sessionAlarms, TimeSpan.FromSeconds(10)); //espera até 10 ou quando a condição estiver satisfeita

            var txtLocation = sessionAlarms.FindElementByName("Inserir um local");

            //USANDO O WEBDRIVERWAIT PARA AGUARTAR 10 SEGUNDOS, OU ATÉ TXTLOCATION SER MOSTRADO
            waitForMe.Until(prep => txtLocation.Displayed);

            //DIGITA COISAS EM UM CAMPO DE TEXTO
            txtLocation.SendKeys("Roma, Itália");

            //APERTA UMA TECLA ESPECIAL DO TECLADO
            txtLocation.SendKeys(Keys.Enter);

            //VAI RETORNAR UMA LISTA DE ELEMENTOS COM ESSE NOME (METODO NO PLURAL)
            var clockItems = sessionAlarms.FindElementsByClassName("NamedContainerAutomationPeer");

            WindowsElement tileFound = null;

            bool wasClockTitleFound = false;
            foreach (WindowsElement clockTile in clockItems)
            {
                if (clockTile.Text.StartsWith("Roma, Itália"))
                {
                    wasClockTitleFound = true;
                    Debug.WriteLine("Clock found.");
                    tileFound = clockTile;
                    break;
                }
            }

            Assert.IsTrue(wasClockTitleFound, "No clock tile found");
            Actions actionsForRightClick = new Actions(sessionAlarms);

            // MOVER O MOUSE PARA UM ELEMENTO
            actionsForRightClick.MoveToElement(tileFound);
            actionsForRightClick.Click();// AS VEZES O CLICK COM O BOTAO DIREITO PODE FALHAR (nesse caso), ENTAO CLICAMOS PRIMEIRO NORMAL NESSA LINHA
            
            //CLICK COM O BOTAO DIREITO
            actionsForRightClick.ContextClick();
            actionsForRightClick.Perform();

            //PARA USAR O MENU DE CONTEXTO PRECISAMOS CRIAR UMA NOVA SESSION
            AppiumOptions capDesktop = new AppiumOptions();
            capDesktop.AddAdditionalCapability("app","Root");

            WindowsDriver<WindowsElement> sessionDesktop =
                new WindowsDriver<WindowsElement>(
                    new Uri("http://127.0.0.1:4723"), capDesktop);

            var ContextItemDelete = sessionDesktop.FindElementByAccessibilityId("ContextMenuDelete");

            //PARA ESPERAR O MENU APARECER TEMOS QUE CONFIGURAR NOVAMENTE O WAIT, PORQUE SE TRATA DE UMA NOVA SESSION
            WebDriverWait desktopWaitForMe = new WebDriverWait(sessionDesktop, TimeSpan.FromSeconds(10));
            desktopWaitForMe.Until(prep => ContextItemDelete.Displayed);

            ContextItemDelete.Click();
        }
    }
}
