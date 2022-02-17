using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveDialog : ContentDialog
    {

        private string projectName;
        public string ProjectName { get => projectName; 
            set
            {
               projectName = value;

                if (!string.IsNullOrEmpty(projectName))
                {
                    this.IsPrimaryButtonEnabled = true;
                }
                else
                {
                    this.IsPrimaryButtonEnabled = false;
                }
            } 
        }

        public SaveDialog()
        {
            this.InitializeComponent();

            this.IsPrimaryButtonEnabled = false;

            var frame = Window.Current.Content as Frame; ;
            this.RequestedTheme = frame.ActualTheme;
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
