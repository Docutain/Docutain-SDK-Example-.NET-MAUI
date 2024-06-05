using Docutain.SDK.MAUI;
using System.Diagnostics;

namespace Docutain_SDK_Example_.NET_MAUI;

public partial class TextResultPage : ContentPage
{
    public TextResultPage(FileResult fileResult)
    {
        InitializeComponent();

        LoadData(fileResult);
    }

    private void LoadData(FileResult fileResult)
    {
        Task.Run(() =>
        {
            try
            {
                // If a FileResult is available, it means we have imported a file. If so, we need to load it into the SDK first.
                if (fileResult != null)
                {
                    // If a uri is available, it means we have imported a file. If so, we need to load it into the SDK first
                    if (!DocumentDataReader.LoadFile(fileResult.FullPath))
                    {
                        // An error occurred, get the latest error message
                        Debug.WriteLine($"DocumentDataReader.LoadFile failed, last error: {DocutainSDK.LastError}");
                        return;
                    }
                }

                // Get the text of all currently loaded pages.
                // If you want text of just one specific page, define the page number.
                // See https://docs.docutain.com/docs/Xamarin/textDetection for more details.
                string text = DocumentDataReader.GetText();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    resultLabel.Text = text;
                });

            }
            catch (Exception ex)
            {

            }
        });
    }
}