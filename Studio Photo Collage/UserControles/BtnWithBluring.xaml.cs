using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Models;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                var navigation = ServiceLocator.Current.GetInstance<INavigationService>();
                navigation.NavigateTo("MainPage");
                Messenger.Default.Send(parameter);
            });
        }
    }
}
