using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class ConfirmDialog : ContentDialog
    {
        private string _allOrThis;
        public string AllOrThis
        {
            get { return _allOrThis; }
            set
            {
                _allOrThis = value.GetLocalized();
            }
        }

        public ConfirmDialog()
        {
            this.InitializeComponent();
            var frame = Window.Current.Content as Frame; ;
            this.RequestedTheme = frame.ActualTheme;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
