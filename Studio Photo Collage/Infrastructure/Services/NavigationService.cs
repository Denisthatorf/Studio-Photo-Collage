using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Studio_Photo_Collage.Views;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.ViewModels;

namespace Studio_Photo_Collage.Infrastructure.Services
{
    public sealed class NavigationService : INavigationService
    {
        public Type CurrentPageType
        {
            get
            {
                var frame = (Frame)Window.Current.Content;
                var page = frame.Content;
                return page.GetType();
            }
        }

        // alternative to void Navigate(Type t)
        //public void Navigate(Type sourcePage) {
        //    Navigate (sourcePage, null);
        //}
        // or with paramterless
        //public void Navigate(Type sourcePage, object paramter = null) {
        //    Navigate (sourcePage, parameter);
        //}
        public void Navigate(Type sourcePage)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage);
        }

        public void Navigate(Type sourcePage, object parameter)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(sourcePage, parameter);
        }

        public void Navigate(string sourcePage)
        {
            Navigate(Type.GetType(sourcePage));
        }

        public void Navigate(string sourcePage, object parameter)
        {
            Navigate(Type.GetType(sourcePage), parameter);
        }

        /// <summary>
        /// Navigates to the most recent item in forward navigation history, if a Frame manages its own navigation history.
        /// </summary>
        public void GoForward()
        {
            // Frame.CanGoForward()?
            Go(true);
        }
        public string GetStringCurrentPage
        {
            get
            {
                var pageFullType = (Window.Current.Content as Frame).Content.ToString();
                var arr = pageFullType.Split('.');
                var currentPage = arr[arr.Length - 1];
                return currentPage;
            }
        }

        /// <summary>
        /// Navigates to the most recent item in back navigation history, if a Frame manages its own navigation history.
        /// </summary>
        public void GoBack()
        {
            var previousPage = GetStringCurrentPage;
            var rootFrame = (Frame)Window.Current.Content;
            string currentPage = string.Empty;
            do
            {
                rootFrame.GoBack();
                currentPage = GetStringCurrentPage;

            } while (previousPage == currentPage);
        }
        /*public static void GoBack()
        {
            var navigation = Ioc.Default.GetService<INavigationService>();
            var currentPage = GetStringCurrentPage();
            if (currentPage == "MainPage")
            {
                var mainPage = Ioc.Default.GetService<MainPageViewModel>();
                //mainPage.GoBack();
            }
            else if (currentPage == "TemplatesPage")
            {
                navigation.NavigateTo("StartPage");
            }
        }*/

        private static void Go(bool isForward)
        {
            var frame = (Frame)Window.Current.Content;
            if (isForward)
            {
                frame.GoForward();
            }
            else
            {
                frame.GoBack();
            }
        }
    }
}
