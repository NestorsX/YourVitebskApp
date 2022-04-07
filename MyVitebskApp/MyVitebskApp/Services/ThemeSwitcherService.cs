using MyVitebskApp.Models;
using MyVitebskApp.Themes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyVitebskApp.Services
{
    public static class ThemeSwitcherService
    {
        public static bool SetAppTheme(UITheme selectedTheme)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (selectedTheme)
                {
                    case UITheme.Dark:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case UITheme.Light:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                    default:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }

                return true;
            }

            return false;
        }
    }
}
