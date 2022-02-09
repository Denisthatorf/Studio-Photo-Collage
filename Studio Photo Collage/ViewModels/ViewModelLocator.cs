using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.ViewModels.PopUps;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Threading.Tasks;
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
        public const string TransformPageKey = "TransformPage";
        public const string RecentPageKey = "RecentPage";
        public const string FramesPadeKey = "FramesPage";
        public const string TemplatesSidePanelPageKey = "TemplatesSidePagePage";

        public const string PaintPopUpPageKey = "PaintPopUpPage";
        public const string SettingstDialogKey = "SettingsContentDialog";

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
            nav.Configure(TransformPageKey, typeof(TransformPage));
            nav.Configure(RecentPageKey, typeof(ResentsPage));
            nav.Configure(FramesPadeKey, typeof(FramesPage));

            nav.Configure(PaintPopUpPageKey, typeof(PaintPopUpPage));
            nav.Configure(SettingstDialogKey, typeof(SettingsDialog));
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
            SimpleIoc.Default.Register<TransformPageViewModel>();
            SimpleIoc.Default.Register<RecentPageViewModel>();
            SimpleIoc.Default.Register<FramesPageViewModel>();
            SimpleIoc.Default.Register<TemplatesSidePanelPageViewModel>();

            SimpleIoc.Default.Register<PaintPopUpPageViewModel>();
            SimpleIoc.Default.Register<SettingsDialogViewModel>();

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
        public TransformPageViewModel TransformPageInstance => ServiceLocator.Current.GetInstance<TransformPageViewModel>();
        public RecentPageViewModel RecentPageInstance => ServiceLocator.Current.GetInstance<RecentPageViewModel>();
        public FramesPageViewModel FramesPageInstanse => ServiceLocator.Current.GetInstance<FramesPageViewModel>();
    //    public TemplatesSidePanelPageViewModel TemplatesSidePanelPageInstance => ServiceLocator.Current.GetInstance<TemplatesSidePanelPageViewModel>();

        public SettingsDialogViewModel SettingsDialogInstance => ServiceLocator.Current.GetInstance<SettingsDialogViewModel>();
        public PaintPopUpPageViewModel PaintPopUppageInstance => ServiceLocator.Current.GetInstance<PaintPopUpPageViewModel>();


        public static void ReloadCurrentPage()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();

            Frame rootFrame = Window.Current.Content as Frame;
            var strContent = rootFrame.Content.ToString();
            var arr = strContent.Split('.');

            //rootFrame.Navigate(rootFrame.CurrentSourcePageType, "reload");
            navigation.NavigateTo(GetStringCurrentPage(), "reload");
        }
        public static string GetStringCurrentPage()
        {
            var pageFullType = (Window.Current.Content as Frame).Content.ToString();
            var arr = pageFullType.Split('.');
            var currentPage = arr[arr.Length - 1];
            return currentPage;
        }
        public static void GoBack()
        {
            var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            var currentPage = GetStringCurrentPage();
            if (currentPage == "MainPage")
            {
                var mainPage =ServiceLocator.Current.GetInstance<MainPageViewModel>();
               mainPage.GoBack();
            }
            else if(currentPage == "TemplatesPage")
            {
                navigation.NavigateTo("StartPage");
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
