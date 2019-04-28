using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace App_UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .InstalledApp("com.prudx.grocerymate")
                    //.ApkFile(@"C:\ionictest\eanew\platforms\android\build\outputs\apk\android-debug.apk")
                    .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}