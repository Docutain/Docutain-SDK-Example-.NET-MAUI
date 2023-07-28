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

            Device.BeginInvokeOnMainThread(() =>
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

    private async Task<(FileResult File, bool Canceled)> GetProcessingInput()
    {
        string action = await DisplayActionSheet(AppResources.title_import_option, "Cancel", null,
            AppResources.input_option_scan,
            AppResources.input_option_PDF,
            AppResources.input_option_image);

        if (action == "Cancel")
            return (null, true);
        if (action == AppResources.input_option_scan)
        {
            await StartScan();
            return (null, false);
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

    private async Task<bool> StartScan()
    {
        // Define a DocumentScannerConfiguration to alter the scan process and define a custom theme to match your branding
        var scanConfig = new DocumentScannerConfiguration();
        scanConfig.AllowCaptureModeSetting = true; // defaults to false
        scanConfig.PageEditConfig.AllowPageFilter = true; // defaults to true
        scanConfig.PageEditConfig.AllowPageRotation = true; // defaults to true
        // alter the onboarding image source if you like
        //scanConfig.OnboardingImageSource = ...

        // detailed information about theming possibilities can be found here: https://docs.docutain.com/docs/Xamarin/theming
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