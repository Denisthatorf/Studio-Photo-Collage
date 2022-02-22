using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Services;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.PopUpsViewModels
{
    public class SettingsDialogViewModel : ObservableObject
    {
        private readonly SettingServise settingServise;

        private CultureInfo languageComBox_SelectedItm;
        private ElementTheme themeComBox_SelectedItem;
        private ElementTheme themeOfSettings;
        private ICommand changeMainColorCommand;
        //private Color selectedColor;

        public ICommand ChangeMainColorCommand
        {
            get
            {
                if (changeMainColorCommand == null)
                {
                    changeMainColorCommand = new RelayCommand<Color>(async (parametr) =>
                    {
                        await settingServise.SetCutomColorAsync(parametr);
                    });
                }

                return changeMainColorCommand;
            }
        }
        public List<SolidColorBrush> Brushes { get; }

        public CultureInfo LanguageComBox_SelectedItm
        {
            get { return languageComBox_SelectedItm; }
            set
            {
                SetProperty(ref languageComBox_SelectedItm, value);
                settingServise.SetLanguage(languageComBox_SelectedItm);
            }
        }
        public ElementTheme ThemeComBox_SelectedItem
        {
            get { return themeComBox_SelectedItem; }
            set
            {
                SetProperty(ref themeComBox_SelectedItem, value);
                ChangeTheme(themeComBox_SelectedItem);

                settingServise.Theme = value;
                ThemeOfSettings = value;
            }
        }
        public ElementTheme ThemeOfSettings
        {
            get => themeOfSettings;
            set => SetProperty(ref themeOfSettings, value);
        }
        public string VersionDescription { get; set; }

        //public Color SelectedColor 
        //{ 
        //    get => selectedColor;
        //    set => SetProperty(ref selectedColor, value);
        //}

        public SettingsDialogViewModel(SettingServise settingServise)
        {
            this.settingServise = settingServise;

            Brushes = ColorGenerator.FillSettingByBrush();

            languageComBox_SelectedItm = settingServise.Language;
            themeComBox_SelectedItem = settingServise.Theme;
            themeOfSettings = settingServise.Theme;
            VersionDescription = GetVersionDescription();
        }

        public async void ChangeTheme(ElementTheme newtheme)
        {
            await settingServise.SetRequestedThemeAsync(newtheme);
        }

        private static string GetVersionDescription()
        {
            //var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
