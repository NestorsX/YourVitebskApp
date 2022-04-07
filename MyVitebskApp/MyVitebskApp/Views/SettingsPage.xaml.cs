using MyVitebskApp.Models;
using MyVitebskApp.Services;
using MyVitebskApp.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyVitebskApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Preferences.ContainsKey("CurrentAppTheme"))
            {
                ThemePicker.SelectedItem = Preferences.Get("CurrentAppTheme", "Light");
            }

            ThemePicker.ItemsSource = Enum.GetValues(typeof(UITheme));

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries.Count == 0)
            {
                var currentTheme = mergedDictionaries.First().GetType();

                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(LightTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 0;
                }
                else if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(DarkTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 1;
                }

                if (ThemePicker.SelectedItem != null)
                    statusLabel.Text = $"Сейчас используется, {ThemePicker.SelectedItem.ToString()} тема.";
            }
        }
        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                // parsing selected theme value
                if (Enum.TryParse(ThemePicker.SelectedItem.ToString(), out UITheme currentThemeEnum))
                {
                    // setting up theme
                    if (ThemeSwitcherService.SetAppTheme(currentThemeEnum))
                    {
                        // Theme setting successful
                        statusLabel.Text = $"{ThemePicker.SelectedItem.ToString()} установлена";
                        Preferences.Set("CurrentAppTheme", ThemePicker.SelectedItem.ToString());
                    }
                }

            }
        }
    }
}