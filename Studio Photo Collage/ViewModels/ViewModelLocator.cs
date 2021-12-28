using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary> 
    /// 
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        public const string StartPageKey = "StartPage";
        public const string TemplatesPageKey = "TemplatesPage";
        public const string MainPageKey = "MainPage";
        public const string FiltersPageKey = "FiltersPage";
        public const string BackgroundPageKey = "BackgroundPage";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var nav = new NavigationService();
            nav.Configure(StartPageKey, typeof(StartPage));
            nav.Configure(TemplatesPageKey, typeof(TemplatesPage));
            nav.Configure(MainPageKey, typeof(MainPage));
            nav.Configure(FiltersPageKey, typeof(FiltersPage));
            nav.Configure(BackgroundPageKey, typeof(BackgroundPage));

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            //Register your services used here
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<StartPageViewModel>();
            SimpleIoc.Default.Register<TemplatesPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();

            SimpleIoc.Default.Register<FiltersPageViewModel>();
            SimpleIoc.Default.Register<BackgroundPageViewModel>();

        }


        // <summary>
        // Gets the StartPage view model.
        // </summary>
        // <value>
        // The StartPage view model.
        // </value>
        public StartPageViewModel StartPageInstance => ServiceLocator.Current.GetInstance<StartPageViewModel>();
        public TemplatesPageViewModel TemplatesPageInstance => ServiceLocator.Current.GetInstance<TemplatesPageViewModel>();
        public MainPageViewModel MainPageInstance => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        public FiltersPageViewModel FiltersPageInstance => ServiceLocator.Current.GetInstance<FiltersPageViewModel>();
        public BackgroundPageViewModel BackgroundPageInstance => ServiceLocator.Current.GetInstance<BackgroundPageViewModel>();


        // <summary>
        // The cleanup.
        // </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
