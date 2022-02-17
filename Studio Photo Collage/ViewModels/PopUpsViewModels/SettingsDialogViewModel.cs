using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Services;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.PopUpsViewModels
{
    public class SettingsDialogViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        private CultureInfo languageComBox_SelectedItm;
        private ElementTheme themeComBox_SelectedItem;
        private ElementTheme themeOfSettings;

        private ICommand changeMainColorCommand;

        public CultureInfo LanguageComBox_SelectedItm
        {
            get { return languageComBox_SelectedItm; }
            set
            {
                SetProperty(ref languageComBox_SelectedItm, value);
                Thread.CurrentThread.CurrentCulture = value;
                ApplicationLanguages.PrimaryLanguageOverride = value?.ToString();
                //ResourceContext.GetForCurrentView().Reset();
                //ResourceContext.GetForViewIndependentUse().Reset();
                //ViewModelLocator.ReloadCurrentPage();
            }
        }
        public ElementTheme ThemeComBox_SelectedItem
        {
            get { return themeComBox_SelectedItem; }
            set
            {
                SetProperty(ref themeComBox_SelectedItem, value);
                ChangeTheme(themeComBox_SelectedItem);
            }
        }
        
        public ICommand ChangeMainColorCommand
        {
            get
            {
                if (changeMainColorCommand == null)
                {
                    changeMainColorCommand = new RelayCommand<Color>((parametr) =>
                    {
                        var brush = (SolidColorBrush)App.Current.Resources["CustomBrush"];
                        brush.Color = parametr;
                        navigationService.Navigate(navigationService.CurrentPageType, DateTime.Now.Ticks);
                    });
                }

                return changeMainColorCommand;
            }
        }

     
        public string VersionDescription { get; set; }

        public List<Brush> Colors { get; }

        public ElementTheme ThemeOfSettings
        {
            get => themeOfSettings;
            set => SetProperty(ref themeOfSettings, value);
        }

        public SettingsDialogViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            var frame = Window.Current.Content as Frame;
            Colors = FillByBrush();
            languageComBox_SelectedItm = GetStartLanguage();
            themeComBox_SelectedItem = frame.RequestedTheme;
            VersionDescription = GetVersionDescription();

        }

        public async void ChangeTheme(ElementTheme newtheme)
        {
            await ThemeSelectorService.SetThemeAsync(newtheme);
            ThemeOfSettings = newtheme;
        }

        private static string GetVersionDescription()
        {
            //var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private CultureInfo GetStartLanguage()
        {
            var selectedLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            var conv = new CultureInfoToFullStringNameConverter();
            var culture = (CultureInfo)conv.ConvertBack(selectedLanguage, null, null, null);
            return culture;
        }

        private List<Brush> FillByBrush()
        {
            var Colors = new List<Brush>
            {
                BrushHelper.GetBrushFromHexOrStrImgBase64("FFBA00"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("F76304"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("DB3800"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("D23135"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("E9091E"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("C40051"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("E4008D"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("C336B5"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("891099"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("754CAB"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("8F8DD9"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("6B69D7"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("0063B3"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("0079D8"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("009ABD"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("00B8C4"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("00B395"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("008675"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("078A3C"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("505C6B"),
                BrushHelper.GetBrushFromHexOrStrImgBase64("7F745F")
            };

            return Colors;
        }
    }
}
