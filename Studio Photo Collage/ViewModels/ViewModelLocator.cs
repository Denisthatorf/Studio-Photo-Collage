using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.ViewModels.PopUps;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;
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
       // private static ViewModelLocator _current;
      //  public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        public const string StartPageKey = "StartPage";
        public const string TemplatesPageKey = "TemplatesPage";
        public const string MainPageKey = "MainPage";
        public const string FiltersPageKey = "FiltersPage";
        public const string BackgroundPageKey = "BackgroundPage";

       // public const string SettingsPageKey = "SettingsPage";
        public const string SettingstDialogKey = "SettingsContentDialog";

        public const string RecentPageKey = "RecentPage";
        public const string FramesPadeKey = "FramesPage";
        public const string TemplatesSidePanelPageKey = "TemplatesSidePagePage";

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        /// 

        private static NavigationService nav = new NavigationService();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            nav.Configure(StartPageKey, typeof(StartPage));
            nav.Configure(TemplatesPageKey, typeof(TemplatesPage));
            nav.Configure(MainPageKey, typeof(MainPage));
            nav.Configure(FiltersPageKey, typeof(FiltersPage));
            nav.Configure(BackgroundPageKey, typeof(BackgroundPage));

          //  nav.Configure(SettingsPageKey, typeof(SettingsPage));
            nav.Configure(SettingstDialogKey, typeof(SettingsDialog));

            nav.Configure(RecentPageKey, typeof(ResentsPage));
            nav.Configure(FramesPadeKey, typeof(FramesPage));
           // nav.Configure(TemplatesSidePanelPageKey, typeof(TemplatesSidePanelPage));

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

           // SimpleIoc.Default.Register<SettingsPageViewModel>();
            SimpleIoc.Default.Register<SettingsDialogViewModel>();

            SimpleIoc.Default.Register<RecentPageViewModel>();
            SimpleIoc.Default.Register<FramesPageViewModel>();
            SimpleIoc.Default.Register<TemplatesSidePanelPageViewModel>();
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

        //public SettingsPageViewModel SettingsPageInstance => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
        public SettingsDialogViewModel SettingsDialogInstance => ServiceLocator.Current.GetInstance<SettingsDialogViewModel>();

        public RecentPageViewModel RecentPageInstance => ServiceLocator.Current.GetInstance<RecentPageViewModel>();
        public FramesPageViewModel FramesPageInstanse => ServiceLocator.Current.GetInstance<FramesPageViewModel>();
        public TemplatesSidePanelPageViewModel TemplatesSidePanelPageInstance => ServiceLocator.Current.GetInstance<TemplatesSidePanelPageViewModel>();


        // public BackgroundPageViewModel SettingsPageInstance => ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
        public static async Task ReloadCurrentPage()
        {
           /* foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                    navigation.NavigateTo(navigation.CurrentPageKey);

                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame?.Navigate(typeof(MainPage));
                });
            }*/
        }
        public static void GoBack()
        {
             var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
             navigation.GoBack();
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
