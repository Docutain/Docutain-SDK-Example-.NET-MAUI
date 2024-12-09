using Docutain.SDK.MAUI;
using System.Collections.ObjectModel;

namespace Docutain_SDK_Example_.NET_MAUI;

public partial class MainPage : ContentPage
{
    ObservableCollection<TestItem> lv_Actions_Source = new ObservableCollection<TestItem>();

    public MainPage()
    {
        InitializeComponent();

        Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;

        lv_Actions.BindingContext = this;
        lv_Actions.ItemsSource = lv_Actions_Source;
        lv_Actions.ItemTapped += (sender, args) => { ((TestItem)args.Item).TapAction?.Invoke(); };

        lv_Actions_Source.Add(new TestItem
        {
            Header = AppResources.document_scanner,
            Description = AppResources.docScannerDesc,
            ImgSrc = "document_scanner_",
            TapAction = () => { StartScan(); }
        });
        lv_Actions_Source.Add(new TestItem
        {
            Header = AppResources.data_extraction,
            Description = AppResources.dataExtractDesc,
            ImgSrc = "quick_reference_",
            TapAction = () => { StartDataExtraction(); }
        });
        lv_Actions_Source.Add(new TestItem
        {
            Header = AppResources.text_recognition,
            Description = AppResources.textRecognitionDesc,
            ImgSrc = "insert_text_",
            TapAction = () => { StartTextOCR(); }
        });
        lv_Actions_Source.Add(new TestItem
        {
            Header = AppResources.generate_pdf,
            Description = AppResources.generatePDFDesc,
            ImgSrc = "picture_as_pdf_",
            TapAction = () => { GeneratePDF(); }
        });
        lv_Actions_Source.Add(new TestItem
        {
            Header = AppResources.settings,
            Description = AppResources.settingsDESC,
            ImgSrc = "settings_",
            TapAction = () => { Settings(); }
        });
    }

    private async void GeneratePDF()
    {
        var result = await GetProcessingInput();
        if (result.Canceled)
            return;

        await Task.Run(() =>
        {
            if (result.File != null)
            {
                // If a FileResult is available, it means we have imported a file. If so, we need to load it into the SDK first.
                if (!DocumentDataReader.LoadFile(result.File.FullPath))
                {
                    // An error occurred, get the latest error message.
                    Console.WriteLine($"DocumentDataReader.LoadFile failed, last error: {DocutainSDK.LastError}");
                    return;
                }
            }
            // Define the output file for the PDF.
            string fileName = FileSystem.CacheDirectory + "/SamplePDF.pdf";

            // Generate the PDF from the currently loaded document.
            // The generated PDF also contains the detected text, making the PDF searchable.
            // See https://docs.docutain.com/docs/Xamarin/pdfCreation for more details.
            var pdfFile = Document.WritePDF(fileName, true, PDFPageFormat.A4);
            if (pdfFile == null)
            {
                // An error occurred, get the latest error message.
                Console.WriteLine($"DocumentDataReader.loadFile failed, last error: {DocutainSDK.LastError}");
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Display the PDF by using the system's default viewer for demonstration purposes.
                Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(pdfFile)
                });
            });
        });
    }

    private async Task<FileResult> PickAndShow(FilePickerFileType fileType)
    {
        try
        {
            return await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = fileType
            });
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
        return null;
    }

    private async void StartDataExtraction()
    {
        var result = await GetProcessingInput();
        if (result.Canceled)
            return;
        await Navigation.PushAsync(new DataResultPage(result.File));
    }

    private async void StartTextOCR()
    {
        var result = await GetProcessingInput();
        if (result.Canceled)
            return;
        await Navigation.PushAsync(new TextResultPage(result.File));
    }

    private async void Settings()
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async Task<(FileResult File, bool Canceled)> GetProcessingInput()
    {
        string action = await DisplayActionSheet(AppResources.title_import_option, "Cancel", null,
            AppResources.input_option_scan,
            AppResources.input_option_gallery,
            AppResources.input_option_image,
            AppResources.input_option_PDF);

        if (action == "Cancel")
            return (null, true);
        if (action == AppResources.input_option_scan)
        {
            var result = await StartScan();
            return (null, !result);
        }
        else if (action == AppResources.input_option_gallery)
        {
            var result = await StartScan(true);
            return (null, !result);
        }
        else if (action == AppResources.input_option_image)
        {
            var result = await PickAndShow(FilePickerFileType.Images);
            return (result, result == null ? true : false);
        }
        else if (action == AppResources.input_option_PDF)
        {
            var result = await PickAndShow(FilePickerFileType.Pdf);
            return (result, result == null ? true : false);
        }
        return (null, true);
    }

    private async Task<bool> StartScan(bool imageImport = false)
    {
        //There are a lot of settings to configure the scanner to match your specific needs
        //Check out the documentation to learn more https://docs.docutain.com/docs/MAUI/docScan#change-default-scan-behaviour
        var scanConfig = new DocumentScannerConfiguration();

        if (imageImport)
            scanConfig.Source = Source.GalleryMultiple;

        //In this sample app we provide a settings page which the user can use to alter the scan settings
        //The settings are stored in and read from SharedPreferences
        //This is supposed to be just an example, you do not need to implement it in that exact way
        //If you do not want to provide your users the possibility to alter the settings themselves at all
        //You can just set the settings according to the apps needs

        //set edit settings
        scanConfig.PageEditConfig.AllowPageFilter = DocutainPreferences.Get(DocutainPreferences.EditSettings.AllowPageFilter);
        scanConfig.PageEditConfig.AllowPageRotation = DocutainPreferences.Get(DocutainPreferences.EditSettings.AllowPageRotation);
        scanConfig.PageEditConfig.AllowPageArrangement = DocutainPreferences.Get(DocutainPreferences.EditSettings.AllowPageArrangement);
        scanConfig.PageEditConfig.AllowPageCropping = DocutainPreferences.Get(DocutainPreferences.EditSettings.AllowPageCropping);
        scanConfig.PageEditConfig.PageArrangementShowDeleteButton = DocutainPreferences.Get(DocutainPreferences.EditSettings.PageArrangementShowDeleteButton);
        scanConfig.PageEditConfig.PageArrangementShowPageNumber = DocutainPreferences.Get(DocutainPreferences.EditSettings.PageArrangementShowPageNumber);

        //set scan settings
        scanConfig.AllowCaptureModeSetting = DocutainPreferences.Get(DocutainPreferences.ScanSettings.AllowCaptureModeSetting);
        scanConfig.AutoCapture = DocutainPreferences.Get(DocutainPreferences.ScanSettings.AutoCapture);
        scanConfig.AutoCrop = DocutainPreferences.Get(DocutainPreferences.ScanSettings.AutoCrop);
        scanConfig.MultiPage = DocutainPreferences.Get(DocutainPreferences.ScanSettings.MultiPage);
        scanConfig.PreCaptureFocus = DocutainPreferences.Get(DocutainPreferences.ScanSettings.PreCaptureFocus);
        scanConfig.DefaultScanFilter = (ScanFilter)(DocutainPreferences.GetInteger(DocutainPreferences.ScanSettings.DefaultScanFilter));

        //set color settings
        var ColorPrimary = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorPrimary);
        scanConfig.ColorConfig.ColorPrimary = (ColorPrimary.Item1, ColorPrimary.Item2);
        var ColorSecondary = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorSecondary);
        scanConfig.ColorConfig.ColorSecondary = (ColorSecondary.Item1, ColorSecondary.Item2);
        var ColorOnSecondary = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorOnSecondary);
        scanConfig.ColorConfig.ColorOnSecondary = (ColorOnSecondary.Item1, ColorOnSecondary.Item2);
        var ColorScanButtonsLayoutBackground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorScanButtonsLayoutBackground);
        scanConfig.ColorConfig.ColorScanButtonsLayoutBackground = (ColorScanButtonsLayoutBackground.Item1, ColorScanButtonsLayoutBackground.Item2);
        var ColorScanButtonsForeground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorScanButtonsForeground);
        scanConfig.ColorConfig.ColorScanButtonsForeground = (ColorScanButtonsForeground.Item1, ColorScanButtonsForeground.Item2);
        var ColorScanPolygon = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorScanPolygon);
        scanConfig.ColorConfig.ColorScanPolygon = (ColorScanPolygon.Item1, ColorScanPolygon.Item2);
        var ColorBottomBarBackground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorBottomBarBackground);
        scanConfig.ColorConfig.ColorBottomBarBackground = (ColorBottomBarBackground.Item1, ColorBottomBarBackground.Item2);
        var ColorBottomBarForeground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorBottomBarForeground);
        scanConfig.ColorConfig.ColorBottomBarForeground = (ColorBottomBarForeground.Item1, ColorBottomBarForeground.Item2);
        var ColorTopBarBackground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorTopBarBackground);
        scanConfig.ColorConfig.ColorTopBarBackground = (ColorTopBarBackground.Item1, ColorTopBarBackground.Item2);
        var ColorTopBarForeground = DocutainPreferences.Get(DocutainPreferences.ColorSettings.ColorTopBarForeground);
        scanConfig.ColorConfig.ColorTopBarForeground = (ColorTopBarForeground.Item1, ColorTopBarForeground.Item2);

        // alter the onboarding image source if you like
        //scanConfig.OnboardingImageSource = ...

        //start the scanner using the provided config
        return await UI.ScanDocument(scanConfig);
    }

    private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        //Damit der IValueConverter nochmal aufgerufen wird
        lv_Actions.ItemsSource = null;
        lv_Actions.ItemsSource = lv_Actions_Source;
    }

    public class TestItem
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public string ImgSrc { get; set; }
        public string ImgSrcDark { get; set; }
        public Action TapAction { get; set; }
        public Action ThemeInit { get; set; }
    }
}