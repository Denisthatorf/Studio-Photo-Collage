using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        #region Save Commands
        private ICommand _saveImageCommand;
        public ICommand SaveImageCommand
        {
            get
            {
                if (_saveImageCommand == null)
                    _saveImageCommand = new RelayCommand<object>((parametr) => { });
                return _saveImageCommand;
            }
        }

        private ICommand _saveProjectCommand;
        public ICommand SaveProjectCommand
        {
            get
            {
                if (_saveProjectCommand == null)
                    _saveProjectCommand = new RelayCommand<object>((parametr) => { });
                return _saveProjectCommand;
            }
        }
        #endregion

        private BtnNameEnum? _checkBoxesEnum = null;
        public BtnNameEnum? CheckBoxesEnum
        {
            get
            {
                return _checkBoxesEnum;
            }
            set
            {
                if (_checkBoxesEnum != value)
                {
                    Set(ref _checkBoxesEnum, value);
                    if (value == BtnNameEnum.Settings)
                    {
                        _ = ShowSettingDialog();
                    }
                }
                else
                    Set(ref _checkBoxesEnum, null);
            }
        }

        private Project _project;
        public Project CurrentProject { get => _project; set => Set(ref _project, value); }


        //  public Frame SettingsFrame { get; }
        public Frame PaintFrame { get; }
         

        public MainPageViewModel(INavigationService _navigationService)
        {
            // SettingsFrame = new Frame();
            // SettingsFrame.Navigate(typeof(SettingsPage));

            PaintFrame = new Frame();
            PaintFrame.Navigate(typeof(PaintPopUpPage));

            NavigationService = _navigationService;

            Messenger.Default.Register<Project>(this, (parameter) => CurrentProject = parameter);
            Messenger.Default.Register<string>(this, (parametr) => { CurrentProject.ProjectName = parametr; SaveProject();});
        }

        private async Task ShowSettingDialog()
        {
            var dialog = new SettingsDialog();
            await dialog.ShowAsync();
        }
        private async Task SaveProject()
        {
            var str = await JsonHelper.DeserializeFileAsync("projects.json");
            var projects = await JsonHelper.ToObjectAsync<ObservableCollection<Project>>(str);
            projects.Add(CurrentProject);
            str = await JsonHelper.StringifyAsync(projects);
            await JsonHelper.WriteToFile("projects.json",str);
        }
    }
}
