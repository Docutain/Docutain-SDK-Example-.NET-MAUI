using Docutain.SDK.MAUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Docutain_SDK_Example_.NET_MAUI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataResultPage : ContentPage
    {
        public DataResultPage(FileResult fileResult)
        {
            InitializeComponent();

            LoadData(fileResult);
        }

        private void LoadData(FileResult fileResult)
        {
            Task.Run(() =>
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

                // Analyze the currently loaded document and get the detected data
                string json = DocumentDataReader.Analyze();
                if (string.IsNullOrEmpty(json))
                {
                    // No data detected
                    return;
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    FillEntries(json);
                });
            });
        }

        private void FillEntries(string json)
        {
            try
            {
                JObject result = (JObject)JsonConvert.DeserializeObject(json);
                DateEntry.Text = result.GetValue("Date")?.ToString();
                DateEntry.IsVisible = !string.IsNullOrWhiteSpace(DateEntry.Text);
                DateLabel.IsVisible = DateEntry.IsVisible;
                AmountEntry.Text = result.GetValue("Amount")?.ToString();
                AmountEntry.IsVisible = !string.IsNullOrWhiteSpace(AmountEntry.Text) && AmountEntry.Text != "0.00";
                AmountLabel.IsVisible = AmountEntry.IsVisible;
                InvoiceIdEntry.Text = result.GetValue("InvoiceId")?.ToString();
                InvoiceIdEntry.IsVisible = !string.IsNullOrWhiteSpace(InvoiceIdEntry.Text);
                InvoiceIdLabel.IsVisible = InvoiceIdEntry.IsVisible;
                ReferenceEntry.Text = result.GetValue("Reference")?.ToString();
                ReferenceEntry.IsVisible = !string.IsNullOrWhiteSpace(ReferenceEntry.Text);
                ReferenceLabel.IsVisible = ReferenceEntry.IsVisible;
                PaymentStateEntry.Text = result.GetValue("PaymentState")?.ToString();
                PaymentStateEntry.IsVisible = !string.IsNullOrWhiteSpace(PaymentStateEntry.Text);
                PaymentStateLabel.IsVisible = PaymentStateEntry.IsVisible;

                JObject address = (JObject)JsonConvert.DeserializeObject(result.GetValue("Address").ToString());

                Name1Entry.Text = address.GetValue("Name1")?.ToString();
                Name1Entry.IsVisible = !string.IsNullOrWhiteSpace(Name1Entry.Text);
                Name1Label.IsVisible = Name1Entry.IsVisible;
                Name2Entry.Text = address.GetValue("Name2")?.ToString();
                Name2Entry.IsVisible = !string.IsNullOrWhiteSpace(Name2Entry.Text);
                Name2Label.IsVisible = Name2Entry.IsVisible;
                Name3Entry.Text = address.GetValue("Name3")?.ToString();
                Name3Entry.IsVisible = !string.IsNullOrWhiteSpace(Name3Entry.Text);
                Name3Label.IsVisible = Name3Entry.IsVisible;
                ZipCodeEntry.Text = address.GetValue("Zipcode")?.ToString();
                ZipCodeEntry.IsVisible = !string.IsNullOrWhiteSpace(ZipCodeEntry.Text);
                ZipCodeLabel.IsVisible = ZipCodeEntry.IsVisible;
                CityEntry.Text = address.GetValue("City")?.ToString();
                CityEntry.IsVisible = !string.IsNullOrWhiteSpace(CityEntry.Text);
                CityLabel.IsVisible = CityEntry.IsVisible;
                StreetEntry.Text = address.GetValue("Street")?.ToString();
                StreetEntry.IsVisible = !string.IsNullOrWhiteSpace(StreetEntry.Text);
                StreetLabel.IsVisible = StreetEntry.IsVisible;
                PhoneEntry.Text = address.GetValue("Phone")?.ToString();
                PhoneEntry.IsVisible = !string.IsNullOrWhiteSpace(PhoneEntry.Text);
                PhoneLabel.IsVisible = PhoneEntry.IsVisible;
                CustomerIdEntry.Text = address.GetValue("CustomerId")?.ToString();
                CustomerIdEntry.IsVisible = !string.IsNullOrWhiteSpace(CustomerIdEntry.Text);
                CustomerIDLabel.IsVisible = CustomerIdEntry.IsVisible;

                var bank = (JArray)address.GetValue("Bank");
                var IBAN = "";
                var BIC = "";

                // TODO: handle multiple bank accounts
                if (bank.Count > 0)
                {
                    var object1 = (JObject)bank.First;
                    IBAN = object1.GetValue("IBAN")?.ToString();
                    BIC = object1.GetValue("BIC")?.ToString();
                }

                var regex = new Regex(".{4}");
                IbanEntry.Text = regex.Replace(IBAN, "$0 ");
                IbanEntry.IsVisible = !string.IsNullOrWhiteSpace(IbanEntry.Text);
                IbanLabel.IsVisible = IbanEntry.IsVisible;
                BicEntry.Text = BIC;
                BicEntry.IsVisible = !string.IsNullOrWhiteSpace(BicEntry.Text);
                BicLabel.IsVisible = BicEntry.IsVisible;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
            ScrollView.IsVisible = true;
        }
    }
}