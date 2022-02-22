using System;

namespace Studio_Photo_Collage.Infrastructure.Services
{
    public interface INavigationService
    {
        Type CurrentPageType { get; }
        void Navigate(Type sourcePage);
        void Navigate(Type sourcePage, object parameter);
        void Navigate(string sourcePage);
        void Navigate(string sourcePage, object parameter);
        void GoBack();
        void GoForward();
    }
}
