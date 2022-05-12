using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
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
            WindowsDriver<WindowsElement> sessionAlarms;
            AppiumOptions capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
            sessionAlarms = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723/"), capabilities);
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
        public static void BeforeATest()
        {
            Debug.WriteLine("Before a test, calling TestInitialize");
        }

        [TestCleanup]
        public static void AfterAtest()
        {
            Debug.WriteLine("After a test, calling TestCleanup");
        }

        [TestMethod]
        public static void JustAnotherTest()
        {
            Debug.WriteLine("Hello another test.");
        }

        [TestMethod]
        public void TestAlarmsAndClockIsLaunchingSuccessfully()
        {

            Assert.AreEqual("Alarmes e Relógio2", sessionAlarms.Title, false,
                    $"Actual title doesn't match expected title: {sessionAlarms.Title}"
                );
        }
    }
}
