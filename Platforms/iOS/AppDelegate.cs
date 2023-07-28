using Foundation;
using UIKit;

namespace Docutain_SDK_Example_.NET_MAUI;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var rc = base.FinishedLaunching(application, launchOptions);
        Platform.Init(() => Window.RootViewController);
        return rc;
    }
}
