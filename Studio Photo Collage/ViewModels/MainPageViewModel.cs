using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Views.PopUps;
using Studio_Photo_Collage.Views.PopUps.Settings;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Globalization;
using Windows.ApplicationModel.Core;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Core;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                var setvalue = new Task(async () =>
                await Window.Current.Content.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,

                () =>
                {
                    Set(ref _selectedItem, value);
                    var item = _selectedItem as NavigationViewItem;
                    if (item != null)
                    {
                        var tag = item.Tag.ToString();
                        if (tag == "Settings")
                            IsSettingsOpen = true;
                        else
                            IsSettingsOpen = false;
                    }
                }

                ));
                setvalue.RunSynchronously();

            }
        }


        private bool _isSettingOpen = false;
        public bool IsSettingsOpen { 
            get => _isSettingOpen; 
            set => Set(ref _isSettingOpen, value);}


        //  private Frame _settingsFrame;
        public Frame SettingsFrame { get; }
        public Frame PaintFrame { get; }

        private RelayCommand _backButtonCommand;

        public MainPageViewModel(INavigationService _navigationService) 
        {
            SettingsFrame = new Frame();
            SettingsFrame.Navigate(typeof(SettingsPage));

            PaintFrame = new Frame();
            PaintFrame.Navigate(typeof(PaintPopUpPage));

            NavigationService = _navigationService;
            _backButtonCommand = new RelayCommand(() => NavigationService.NavigateTo("TemplatesPage"));
        }

        public RelayCommand BackButtonCommand { get => _backButtonCommand; set => Set(ref _backButtonCommand, value); }

        
    }
}
