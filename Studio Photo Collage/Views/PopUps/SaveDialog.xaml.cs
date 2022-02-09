using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveDialog : ContentDialog
    {
        public string ProjectName { get; set; }

        private string _whatSave;
        public string WhatSave
        {
            get { return _whatSave; }
            set
            {
                _whatSave = value;
                if (WhatSave == "project")
                    this.SecondaryButtonText = "Save";
                else if (WhatSave == "collage")
                {
                    this.PrimaryButtonText = "Yes";
                    this.SecondaryButtonText = "No";
                }
            }
        }

        public SaveDialog(string projectOrCollage)
        {
            this.InitializeComponent();
            var frame = Window.Current.Content as Frame; ;
            this.RequestedTheme = frame.ActualTheme;
            WhatSave = projectOrCollage;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
