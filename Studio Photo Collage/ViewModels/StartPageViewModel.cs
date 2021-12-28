using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand ImageClickCommand { get; private set; }

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            ImageClickCommand = new RelayCommand(() => _navigationService.NavigateTo("MainPage")) ;
        }
    }
}
