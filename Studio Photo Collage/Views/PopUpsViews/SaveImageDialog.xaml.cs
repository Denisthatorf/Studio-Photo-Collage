using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveImageDialog : ContentDialog
    {
        private string nameOfImg;
        public string NameOfImg { get => nameOfImg; 
            set 
            { 
                nameOfImg = value;
                if (!string.IsNullOrEmpty(nameOfImg))
                {
                    this.IsPrimaryButtonEnabled = true;
                }
                else
                {
                    this.IsPrimaryButtonEnabled = false;
                }
            } 
        }
        public string Format => FormatCBox.SelectedItem.ToString();
        public string Quality => QualityCBox.SelectedItem.ToString();

        public SaveImageDialog()
        {
            this.InitializeComponent();

            this.IsPrimaryButtonEnabled = false;

            var frame = Window.Current.Content as Frame; ;
            this.RequestedTheme = frame.ActualTheme;
        }

        private void CloseBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
