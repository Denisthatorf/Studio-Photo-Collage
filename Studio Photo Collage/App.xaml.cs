using CommonServiceLocator;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.ViewModels;
using Studio_Photo_Collage.Views;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Studio_Photo_Collage
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // customize Title Bar
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (Application.Current.Resources["MainBackgroundColor"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = (Application.Current.Resources["AppBarItemForegroundThemeBrush"] as SolidColorBrush).Color;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.BackRequested += (object s, BackRequestedEventArgs ev) => ViewModelLocator.GoBack();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(StartPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }


            var argumentOfSecondaryBtn = e.Arguments;
            SetProjectFromSecondaryTile(argumentOfSecondaryBtn);

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async void SetProjectFromSecondaryTile(string argument)
        {
            if (string.IsNullOrEmpty(argument))
                return;

            var jsonStr = await JsonHelper.DeserializeFileAsync("projects.json");
            var projectList = await JsonHelper.ToObjectAsync<List<Project>>(jsonStr);

            if (projectList != null)
            {
                foreach (var proj in projectList)
                {
                    var hashCode = proj.GetHashCode().ToString();
                    if (hashCode == argument)
                    {
                        var navigation = ServiceLocator.Current.GetInstance<INavigationService>();

                        if(ViewModelLocator.GetStringCurrentPage() != "MainPage")
                            navigation.NavigateTo("MainPage");
                        Messenger.Default.Send(proj);

                        break;
                    }

                }
            }
        }

    }
}
