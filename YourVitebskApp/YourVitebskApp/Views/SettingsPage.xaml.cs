using YourVitebskApp.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YourVitebskApp.Models;
using YourVitebskApp.Services;

namespace YourVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            if (!Preferences.ContainsKey("CurrentAppTheme"))
            {
                Preferences.Set("CurrentAppTheme", "Default");
            }

            string currentTheme = Preferences.Get("CurrentAppTheme", "Default");
            switch (currentTheme)
            {
                case "Default":
                    UseDeviceThemeSettings = true;
                    break;
                case "Dark":
                    UseDarkMode = true;
                    break;
                case "Light":
                    UseLightMode = true;
                    break;
                default:
                    UseDeviceThemeSettings = true;
                    break;
            }

            BindingContext = this;
        }

        private bool _useDarkMode;
        private bool _useLightMode;
        private bool _useDeviceThemeSettings;

        public bool UseDarkMode
        {
            get
            {
                return _useDarkMode;
            }
            set
            {
                _useDarkMode = value;
                if (_useDarkMode)
                {
                    UseLightMode = UseDeviceThemeSettings = false;
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    Preferences.Set("CurrentAppTheme", "Dark");
                }

            }
        }

        public bool UseLightMode
        {
            get
            {
                return _useLightMode;
            }
            set
            {
                _useLightMode = value;
                if (_useLightMode)
                {
                    UseDarkMode = UseDeviceThemeSettings = false;
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    Preferences.Set("CurrentAppTheme", "Light");
                }
            }
        }

        public bool UseDeviceThemeSettings
        {
            get
            {
                return _useDeviceThemeSettings;
            }
            set
            {
                _useDeviceThemeSettings =  value;
                if (_useDeviceThemeSettings)
                {
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    Preferences.Set("CurrentAppTheme", "Default");
                }
            }

        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (Preferences.ContainsKey("CurrentAppTheme"))
        //    {
        //        ThemePicker.SelectedItem = Preferences.Get("CurrentAppTheme", "Light");
        //    }

        //    ThemePicker.ItemsSource = Enum.GetValues(typeof(UITheme));

        //    ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        //    if (mergedDictionaries.Count == 0)
        //    {
        //        var currentTheme = mergedDictionaries.First().GetType();

        //        if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(LightTheme).FullName))
        //        {
        //            ThemePicker.SelectedIndex = 0;
        //        }
        //        else if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(DarkTheme).FullName))
        //        {
        //            ThemePicker.SelectedIndex = 1;
        //        }

        //        if (ThemePicker.SelectedItem != null)
        //            statusLabel.Text = $"Сейчас используется, {ThemePicker.SelectedItem.ToString()} тема.";
        //    }
        //}
        //void OnPickerSelectionChanged(object sender, EventArgs e)
        //{
        //    ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        //    if (mergedDictionaries != null)
        //    {
        //        mergedDictionaries.Clear();

        //        // parsing selected theme value
        //        if (Enum.TryParse(ThemePicker.SelectedItem.ToString(), out UITheme currentThemeEnum))
        //        {
        //            // setting up theme
        //            if (ThemeSwitcherService.SetAppTheme(currentThemeEnum))
        //            {
        //                // Theme setting successful
        //                statusLabel.Text = $"{ThemePicker.SelectedItem.ToString()} установлена";
        //                Preferences.Set("CurrentAppTheme", ThemePicker.SelectedItem.ToString());
        //            }
        //        }

        //    }
        //}
    }
}