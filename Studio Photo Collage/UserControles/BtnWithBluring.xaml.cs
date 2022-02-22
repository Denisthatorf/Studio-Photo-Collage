using System.Windows.Input;
using Studio_Photo_Collage.Models;
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

        public ICommand RemoveProjectCommand
        {
            get { return (ICommand)GetValue(RemoveProjectCommandProperty); }
            set { SetValue(RemoveProjectCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveProjectCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveProjectCommandProperty =
            DependencyProperty.Register("RemoveProjectCommand", typeof(ICommand), typeof(BtnWithBluring), new PropertyMetadata(null));

        public string DateOfProject => Project.DateOfLastEditing.ToString("MM/dd/yy");

        public BtnWithBluring()
        {
            this.InitializeComponent();
        }
    }
}
