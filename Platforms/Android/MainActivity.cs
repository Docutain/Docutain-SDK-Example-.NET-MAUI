using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Docutain_SDK_Example_.NET_MAUI;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        App.Current.RequestedThemeChanged += ApplyStatusBarColor;
    }

    private void ApplyStatusBarColor(object sender, EventArgs args)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            Window.SetStatusBarColor(Resources.GetColor(Resource.Color.colorPrimaryDark, Theme));
        else
            Window.SetStatusBarColor(Resources.GetColor(Resource.Color.colorPrimaryDark));
    }
}
