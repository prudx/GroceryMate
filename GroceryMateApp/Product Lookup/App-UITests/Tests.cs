using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace App_UITests
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            
        }

        /*
        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }
        */

        [Test]
        public void TestEnterText()
        {
            //app.Repl();
            app.Tap(x => x.Id("queryInput"));
            app.EnterText(x => x.Id("queryInput"), "Did this test work?");

            Assert.IsNotNull(app.Query(x => x.Text("Did this test work?")).Any());

            /*
            app
            Assert.IsNotEmpty(test)
            app.Tap(x => x.Text("Add"));
            app.DismissKeyboard();
            app.EnterText(x => x.Id("txtDesc"), "Description");
            app.DismissKeyboard();
            app.Tap(x => x.Id("save_button"));
            app.WaitForElement(x => x.Text("EA"));
            //app.ScrollDownTo(x => x.Text("EA"));
            var elementCount = app.Query(x => x.Id("recyclerView").All().Text("EA")).Count();
            Assert.That(elementCount, Is.EqualTo(1), "There is no such element being added in app list");
            app.SwipeRightToLeft();
            app.SwipeLeftToRight();
            */
        }
    }
}
