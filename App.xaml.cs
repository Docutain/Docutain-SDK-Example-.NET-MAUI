using Docutain.SDK.MAUI;

namespace Docutain_SDK_Example_.NET_MAUI;

public partial class App : Application
{
    string licenseKey = "YOUR_LICENSE_KEY_HERE";
    bool licenseEmpty = false;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        //Initialize the Docutain SDK
        if (!DocutainSDK.InitSDK(licenseKey))
        {
            //init of Docutain SDK failed, get the last error message
            System.Console.WriteLine("Initialization of the Docutain SDK failed: " + DocutainSDK.LastError);
            //your logic to deactivate access to SDK functionality
            if (licenseKey == "YOUR_LICENSE_KEY_HERE")
            {
                licenseEmpty = true;
                return;
            }
        }

        //If you want to use text detection (OCR) and/or data extraction features, you need to set the AnalyzeConfiguration
        //in order to start all the necessary processes
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

    protected async override void OnStart()
    {
        if (licenseEmpty)
        {
            if (await Current.MainPage.DisplayAlert("License empty", "A valid license key is required. Please contact us via sdk@Docutain.com to get a trial license.", "Get License", "Cancel"))
            {
                var mail = new EmailMessage("Trial License Request .NET MAUI", "", new[] { "sdk@Docutain.com" });
                await Email.ComposeAsync(mail);
            }
            Environment.Exit(0);
        }      
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    { 
    }
}