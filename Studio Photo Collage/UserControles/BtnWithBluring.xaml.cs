using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using Studio_Photo_Collage.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class BtnWithBluring : UserControl
    {
        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Project.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(BtnWithBluring), new PropertyMetadata(null));

        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MyCommandProperty); }
            set { SetValue(MyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCommandProperty =
            DependencyProperty.Register("MyCommand", typeof(ICommand), typeof(BtnWithBluring), new PropertyMetadata(null));

        public ICommand TemplateClickCommand { get; private set; }

        public BtnWithBluring()
        {
            this.InitializeComponent();

            TemplateClickCommand = new RelayCommand<Project>((parameter) =>
            {
               // var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
               // navigation.NavigateTo("MainPage");
                //Messenger.Default.Send(parameter);
            });
        }
    }
}
