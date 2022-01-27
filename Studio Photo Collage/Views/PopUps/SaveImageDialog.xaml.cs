using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveImageDialog : ContentDialog
    {


        public SaveImageDialog()
        {
            this.InitializeComponent();
        }

        private void CloseBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {

        }
    }
}
