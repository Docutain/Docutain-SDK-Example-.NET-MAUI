using static Docutain_SDK_Example_.NET_MAUI.DocutainPreferences;

namespace Docutain_SDK_Example_.NET_MAUI.Controls;
public partial class ColorList : ViewCell
{
    private ColorSettings _Settings;
    public string Header { get; set; }
    public string Description { get; set; }

    public ColorList(ColorSettings settings)
    {
        InitColorList(settings);
    }

    protected void InitColorList(ColorSettings settings)
    {
        InitializeComponent();

        _Settings = settings;

        Header = AppResources.ResourceManager.GetString(DocutainPreferences.SettingsKey(settings));
        Description = AppResources.ResourceManager.GetString(DocutainPreferences.SettingsKey(settings) + "_Description");
        Tuple<Color, Color> tuple = DocutainPreferences.Get(settings);
        bt_Light.BackgroundColor = tuple.Item1;
        bt_Dark.BackgroundColor = tuple.Item2;
        BindingContext = this;
    }

    async void bt_Light_Clicked(object sender, EventArgs args)
    {
        var color = await DocutainColorPicker.DocutainColorPicker.PickColor((sender as Button).BackgroundColor);

        if (color == null)
            return;

        (sender as Button).BackgroundColor = color;
        DocutainPreferences.SetColorLight(_Settings, color);
    }

    async void bt_Dark_Clicked(object sender, EventArgs args)
    {
        var color = await DocutainColorPicker.DocutainColorPicker.PickColor((sender as Button).BackgroundColor);

        if (color == null)
            return;

        (sender as Button).BackgroundColor = color;
        DocutainPreferences.SetColorDark(_Settings, color);
    }

    public void Reload()
    {
        Tuple<Color,Color> tuple = DocutainPreferences.Get(_Settings);
        bt_Light.BackgroundColor = tuple.Item1;
        bt_Dark.BackgroundColor = tuple.Item2;
    }
}

