using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.ViewModels.PopUps;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps.Settings;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
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
        public const string StartPageKey = "StartPage";
        public const string TemplatesPageKey = "TemplatesPage";
        public const string MainPageKey = "MainPage";
        public const string FiltersPageKey = "FiltersPage";
        public const string BackgroundPageKey = "BackgroundPage";
        public const string SettingsPageKey = "SettingsPage";
        public const string RecentPageKey = "RecentPage";

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
            nav.Configure(SettingsPageKey, typeof(SettingsPage));
            nav.Configure(RecentPageKey, typeof(RecentPage));

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
            SimpleIoc.Default.Register<SettingsPageViewModel>();
            SimpleIoc.Default.Register<RecentPageViewModel>();

            // SimpleIoc.Default.Register<SettingsPageViewModel>();

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
        public SettingsPageViewModel SettingsPageInstance => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
        public RecentPageViewModel ReccentPageInstance => ServiceLocator.Current.GetInstance<RecentPageViewModel>();

        // public BackgroundPageViewModel SettingsPageInstance => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
        public static async Task ReloadAll()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        var frame = frameworkElement as Frame;
                        frame.Navigate(frame.SourcePageType);
                    }
                });
            }
        }



        // <summary>
        // The cleanup.
        // </summary>
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
