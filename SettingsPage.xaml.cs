using Docutain.SDK.MAUI;
using Docutain_SDK_Example_.NET_MAUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Docutain_SDK_Example_.NET_MAUI;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        InitSettings();
    }

    private void InitSettings()
    {
        ScanSettingSection.Add(new SwitchList(DocutainPreferences.ScanSettings.AllowCaptureModeSetting));
        ScanSettingSection.Add(new SwitchList(DocutainPreferences.ScanSettings.AutoCapture));
        ScanSettingSection.Add(new SwitchList(DocutainPreferences.ScanSettings.AutoCrop));
        ScanSettingSection.Add(new SwitchList(DocutainPreferences.ScanSettings.MultiPage));
        ScanSettingSection.Add(new SwitchList(DocutainPreferences.ScanSettings.PreCaptureFocus));

        List<Item> items =
        [
            new Item() { Key = (int)ScanFilter.Auto, Value = "Auto" },
            new Item() { Key = (int)ScanFilter.Gray, Value = "Gray" },
            new Item() { Key = (int)ScanFilter.BlackWhite, Value = "Black & White" },
            new Item() { Key = (int)ScanFilter.Original, Value = "Original" },
            new Item() { Key = (int)ScanFilter.Text, Value = "Text" },
            new Item() { Key = (int)ScanFilter.Auto2, Value = "Auto 2" },
            new Item() { Key = (int)ScanFilter.Illustration, Value = "Illustration" },
        ];

        Dropdown dropdown = new Dropdown(DocutainPreferences.ScanSettings.DefaultScanFilter, items);
        ScanSettingSection.Add(dropdown);

        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.AllowPageFilter));
        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.AllowPageRotation));
        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.AllowPageArrangement));
        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.AllowPageCropping));
        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.PageArrangementShowDeleteButton));
        EditSettingSection.Add(new SwitchList(DocutainPreferences.EditSettings.PageArrangementShowPageNumber));

        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorPrimary));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorSecondary));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorOnSecondary));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorScanButtonsLayoutBackground));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorScanButtonsForeground));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorScanPolygon));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorBottomBarBackground));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorBottomBarForeground));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorTopBarBackground));
        ColorSettingSection.Add(new ColorList(DocutainPreferences.ColorSettings.ColorTopBarForeground));    
    }
 
    private void bt_Reset_Clicked(object sender, EventArgs e)
    {
        DocutainPreferences.Reset();
        for(int i= 0; i < (int)DocutainPreferences.ScanSettings.DefaultScanFilter; i++)
        {
            ((SwitchList)ScanSettingSection[i]).Reload();
        }

        ((Dropdown)ScanSettingSection[(int)DocutainPreferences.ScanSettings.DefaultScanFilter]).Reload();

        foreach (SwitchList item in EditSettingSection)
        {
            item.Reload();
        }

        foreach (ColorList item in ColorSettingSection)
        {
            item.Reload();
        }
    }
}
