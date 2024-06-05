using static Docutain_SDK_Example_.NET_MAUI.DocutainPreferences;

namespace Docutain_SDK_Example_.NET_MAUI.Controls;

public partial class Dropdown : ViewCell
{
    public string Key;
    List<Item> _Items;
    public string Header { get; set; }
    public string Description { get; set; }

    public Dropdown(ScanSettings settings, List<Item> items)
    {
        InitDropdown(DocutainPreferences.SettingsKey(settings), items);
    }

    protected void InitDropdown(string key, List<Item> items)
    {
        InitializeComponent();
        Key = key;
        _Items = items;
        Header = AppResources.ResourceManager.GetString(Key);
        Description = AppResources.ResourceManager.GetString(Key + "_Description");
        dropdown.ItemsSource = items;
        dropdown.ItemDisplayBinding = new Binding("Value");
        dropdown.SelectedIndex = items.FindIndex(f => f.Key == DocutainPreferences.GetInteger(Key));
        BindingContext = this;
    }

    private void dropdown_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        DocutainPreferences.Set(Key, (dropdown.SelectedItem as Item).Key);
    }

    public void Reload()
    {
        dropdown.SelectedIndex = _Items.FindIndex(f => f.Key == DocutainPreferences.GetInteger(Key));
    }

}

public class Item
{
    public int Key { get; set; }
    public string Value { get; set; }
}

