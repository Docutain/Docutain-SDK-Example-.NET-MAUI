using Docutain.SDK.MAUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Docutain_SDK_Example_.NET_MAUI;

static public class DocutainPreferences
{
    static List<Tuple<string, bool>> DefaultBool = new List<Tuple<string, bool>>()
    {
        Tuple.Create(ScanSettings.AllowCaptureModeSetting.ToString(),false),
        Tuple.Create(ScanSettings.AutoCapture.ToString(),true),
        Tuple.Create(ScanSettings.AutoCrop.ToString(),true),
        Tuple.Create(ScanSettings.MultiPage.ToString(),true),
        Tuple.Create(ScanSettings.PreCaptureFocus.ToString(),true),

        Tuple.Create(EditSettings.AllowPageFilter.ToString(),true),
        Tuple.Create(EditSettings.AllowPageRotation.ToString(),true),
        Tuple.Create(EditSettings.AllowPageArrangement.ToString(),true),
        Tuple.Create(EditSettings.AllowPageCropping.ToString(),true),
        Tuple.Create(EditSettings.PageArrangementShowDeleteButton.ToString(),false),
        Tuple.Create(EditSettings.PageArrangementShowPageNumber.ToString(),true)
    };

    static List<Tuple<string, int>> DefaultInteger = new List<Tuple<string, int>>()
    {
        Tuple.Create(ScanSettings.DefaultScanFilter.ToString(),(int)ScanFilter.Illustration)
    };

    static List<Tuple<string, Tuple<Color, Color>>> DefaultColor = new List<Tuple<string, Tuple<Color, Color>>>()
    {
        Tuple.Create(ColorSettings.ColorPrimary.ToString(), Tuple.Create(Color.FromArgb("#4CAF50"), Color.FromArgb("#4CAF50"))),
        Tuple.Create(ColorSettings.ColorSecondary.ToString(),Tuple.Create(Color.FromArgb("#4CAF50"), Color.FromArgb("#4CAF50"))),
        Tuple.Create(ColorSettings.ColorOnSecondary.ToString(),Tuple.Create(Colors.White, Colors.Black)),
        Tuple.Create(ColorSettings.ColorScanButtonsLayoutBackground.ToString(),Tuple.Create(Color.FromArgb("#121212"), Color.FromArgb("#121212"))),
        Tuple.Create(ColorSettings.ColorScanButtonsForeground.ToString(),Tuple.Create(Colors.White, Colors.White)),
        Tuple.Create(ColorSettings.ColorScanPolygon.ToString(),Tuple.Create(Color.FromArgb("#4CAF50"), Color.FromArgb("#4CAF50"))),
        Tuple.Create(ColorSettings.ColorBottomBarBackground.ToString(),Tuple.Create(Colors.White, Color.FromArgb("#ff212121"))),
        Tuple.Create(ColorSettings.ColorBottomBarForeground.ToString(),Tuple.Create(Color.FromArgb("#323232"), Color.FromArgb("#ffbebebe"))),
        Tuple.Create(ColorSettings.ColorTopBarBackground.ToString(),Tuple.Create(Color.FromArgb("#4CAF50"), Color.FromArgb("#2a2a2a"))),
        Tuple.Create(ColorSettings.ColorTopBarForeground.ToString(),Tuple.Create(Colors.White, Color.FromArgb("#DEffffff")))
    };

    public enum ScanSettings { AllowCaptureModeSetting, AutoCapture, AutoCrop, MultiPage, PreCaptureFocus, DefaultScanFilter }
    public enum EditSettings { AllowPageFilter, AllowPageRotation, AllowPageArrangement, AllowPageCropping, PageArrangementShowDeleteButton, PageArrangementShowPageNumber }
    public enum ColorSettings
    {
        ColorPrimary, ColorSecondary, ColorOnSecondary, ColorScanButtonsLayoutBackground, ColorScanButtonsForeground, 
        ColorScanPolygon, ColorBottomBarBackground, ColorBottomBarForeground, ColorTopBarBackground, ColorTopBarForeground 
    }

    public static string SettingsKey(ScanSettings settings)
    {
        return settings.ToString();
    }

    public static string SettingsKey(EditSettings settings)
    {
        return settings.ToString();
    }

    public static string SettingsKey(ColorSettings settings)
    {
        return settings.ToString();
    }

    public static void Reset()
    {
        foreach (Tuple<string, int> Item in DefaultInteger)
        {
            Preferences.Set(Item.Item1, Item.Item2);
        }

        foreach (Tuple<string, bool> Item in DefaultBool)
        {
            Preferences.Set(Item.Item1, Item.Item2);
        }

        foreach (Tuple<string, Tuple<Color, Color>> Item in DefaultColor)
        {
            Preferences.Set(Item.Item1 + "_Light", Item.Item2.Item1.ToHex());
            Preferences.Set(Item.Item1 + "_Dark", Item.Item2.Item2.ToHex());
        }
    }

    public static void Set(string key, bool value)
    {
        Preferences.Set(key, value);
    }

    public static void Set(string key, int value)
    {
        Preferences.Set(key, value);
    }

    public static void SetColorLight(ColorSettings settings, Color color)
    {
        Preferences.Set(settings.ToString() + "_Light", color.ToHex());
    }

    public static void SetColorDark(ColorSettings settings, Color color)
    {
        Preferences.Set(settings.ToString() + "_Dark", color.ToHex());
    }

    public static int GetInteger(string key)
    {
        int defaultValue = DefaultInteger.FirstOrDefault(x => x.Item1 == key)?.Item2 ?? 0;
        return Preferences.Get(key, defaultValue);
    }
    public static int GetInteger(ScanSettings settings)
    {
        int defaultValue = DefaultInteger.FirstOrDefault(x => x.Item1 == settings.ToString())?.Item2 ?? 0;
        return Preferences.Get(settings.ToString(), defaultValue);
    }

    public static bool Get(string key)
    {
        var defaultValue = DefaultBool.FirstOrDefault(x => x.Item1 == key)?.Item2 ?? false;
        return Preferences.Get(key, defaultValue);
    }

    public static bool Get(EditSettings settings)
    {
        var defaultValue = DefaultBool.FirstOrDefault(x => x.Item1 == settings.ToString())?.Item2 ?? false;
        return Preferences.Get(settings.ToString(), defaultValue);
    }

    public static bool Get(ScanSettings settings)
    {
        var defaultValue = DefaultBool.FirstOrDefault(x => x.Item1 == settings.ToString())?.Item2 ?? false;
        return Preferences.Get(settings.ToString(), defaultValue);
    }

    public static Tuple<Color, Color> Get(ColorSettings settings)
    {
        var defaultValue = DefaultColor.FirstOrDefault(x => x.Item1 == settings.ToString());
        Color cLight = Color.FromArgb(Preferences.Get(settings.ToString() + "_Light", defaultValue.Item2.Item1.ToHex()));
        Color cDark = Color.FromArgb(Preferences.Get(settings.ToString() + "_Dark", defaultValue.Item2.Item2.ToHex()));
        return Tuple.Create(cLight, cDark);
    }
}

