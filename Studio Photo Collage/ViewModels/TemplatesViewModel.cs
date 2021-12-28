using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand TemplateClickCommand { get; private set; }

        public TemplatesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            TemplateClickCommand = new RelayCommand(()=> _navigationService.NavigateTo("MainPage"));
        }
    }
}
