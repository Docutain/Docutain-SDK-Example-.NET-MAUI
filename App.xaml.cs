using Docutain.SDK.MAUI;

namespace Docutain_SDK_Example_.NET_MAUI;

public partial class App : Application
{
    //A valid license key is required, you can generate one on our website https://sdk.docutain.com/TrialLicense?Source=1966342
    string licenseKey = "YOUR_LICENSE_KEY_HERE";

    bool initFailed = false;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        //the Docutain SDK needs to be initialized prior to using any functionality of it
        //a valid license key is required, you can generate one on our website https://sdk.docutain.com/TrialLicense?Source=1966342
        if (!DocutainSDK.InitSDK(licenseKey))
        {
            //init of Docutain SDK failed, get the last error message
            System.Console.WriteLine("Initialization of the Docutain SDK failed: " + DocutainSDK.LastError);
            //your logic to deactivate access to SDK functionality
            initFailed = true;
            return;
        }

        //Reading payment state and BIC when getting the analyzed data is disabled by default
        //If you want to analyze these 2 fields as well, you need to set the AnalyzeConfig accordingly
        //A good place to do this, is right after initializing the Docutain SDK
        var analyzeConfig = new AnalyzeConfiguration
        {
            ReadBIC = true, //defaults to false
            ReadPaymentState = true //defaults to false
        };
        if (!DocumentDataReader.SetAnalyzeConfiguration(analyzeConfig))
        {
            System.Console.WriteLine("Setting AnalyzeConfiguration failed: " + DocutainSDK.LastError);
        }

        //Depending on your needs, you can set the Logger's level
        Logger.SetLogLevel(Logger.Level.Verbose);

        //Depending on the log level that you have set, some temporary files get written on the filesystem
        //You can delete all temporary files by using the following method
        DocutainSDK.DeleteTempFiles(true);
    }    

    private async void ShowLicenseEmptyInfo()
    {
        await Current.MainPage.DisplayAlert("License empty", "A valid license key is required. Please click \"Get License\" in order to create a " +
            "free trial license key on our website.", "Get License");

        try
        {
            Uri uri = new Uri("https://sdk.docutain.com/TrialLicense?Source=1966342");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.External);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
            System.Console.WriteLine("No Browser App available, please contact us manually via sdk@Docutain.com");
        }
        ShowLicenseEmptyInfo(); //keep alert opened
    }

    private async void ShowLicenseErrorInfo()
    {
        await Current.MainPage.DisplayAlert("License error", "A valid license key is required. Please contact our support to get an extended " +
            "trial license.", "Contact Support");

        try
        {
            var mail = new EmailMessage("Trial License Error", "Please keep your following trial license key in this e-mail: " + licenseKey,
                new[] { "support.sdk@Docutain.com" });
            await Email.ComposeAsync(mail);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No mail app may be installed on the device.
            System.Console.WriteLine("No Mail App available, please contact us manually via sdk@Docutain.com");
        }
        await Task.Delay(1000); //we need to wait a bit for the mail compose view controller to close to be able to show alert again
        ShowLicenseErrorInfo(); //keep alert opened
    }

    protected override void OnStart()
    {
        base.OnStart();

        if (initFailed)
        {
            if (licenseKey == "YOUR_LICENSE_KEY_HERE")
                ShowLicenseEmptyInfo();
            else
                ShowLicenseErrorInfo();
        }
    }
}