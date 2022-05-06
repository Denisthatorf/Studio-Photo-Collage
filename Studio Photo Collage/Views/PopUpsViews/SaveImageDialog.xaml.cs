using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveImageDialog : ContentDialog
    {
        private string nameOfImg;
        public string NameOfImg
        {
            get => nameOfImg;
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

        public StorageFolder Folder;
        public ContentDialogResult Result;
        public SaveImageDialog()
        {
            this.InitializeComponent();

            this.IsPrimaryButtonEnabled = false;

            var frame = Window.Current.Content as Frame; ;
            this.RequestedTheme = frame.ActualTheme;
        }

        private void ContentDialogPrimaryButtonClick(object sender, RoutedEventArgs e)
        {
            Result = ContentDialogResult.Primary;
            this.Hide();
        }

        private async void ContentDialogSecondaryButtonClick(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                Folder = folder;

                Result = ContentDialogResult.Secondary;
                this.Hide();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = ContentDialogResult.None;
            this.Hide();
        }
    }
}
