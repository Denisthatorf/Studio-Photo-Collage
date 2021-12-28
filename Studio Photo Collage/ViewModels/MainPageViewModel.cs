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
using Windows.UI.Xaml.Controls;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Views.PopUps;
using Studio_Photo_Collage.Views.PopUps.Settings;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public RelayCommand SettingsCommand { get; private set; }

        private bool _isSettingOpen = false;
        public bool IsSettingsOpen { get => _isSettingOpen; set => Set(ref _isSettingOpen, value); }

        private Frame _settingsFrame;

        public Frame SettingsFrame
        {
            get { return _settingsFrame; }
            set { Set(ref _settingsFrame, value); }
        }


        public MainPageViewModel()
        {
            _settingsFrame = new Frame();
            _settingsFrame.Navigate(typeof(SettingsPage));

            SettingsCommand = new RelayCommand(()=>IsSettingsOpen=!IsSettingsOpen);
        }
    }
}
