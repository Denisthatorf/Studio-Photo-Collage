using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System.Threading.Tasks;
using Studio_Photo_Collage.Infrastructure.Messages;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
{
    public class BackgroundPageViewModel : ObservableRecipient
    {
        private ICommand uploadBtnCommand;
        private ICommand colorBtnCommand;
        private int bordersThickness;
        private int borderOpacity;

        public ICommand UploadBtnCommand
        {
            get
            {
                if (uploadBtnCommand == null)
                {
                    uploadBtnCommand = new RelayCommand(() =>
                    {
                        UploadBackgroundFromPhotoBtnClickMethod();
                    });
                }

                return uploadBtnCommand;
            }
        }

        public ICommand ColorBtnCommand
        {
            get
            {
                if (colorBtnCommand == null)
                {
                    colorBtnCommand = new RelayCommand<SolidColorBrush>((parametr) => Messenger.Send(parametr));
                }

                return colorBtnCommand;
            }
        }

        public int BordersThickness
        {
            get => bordersThickness;
            set
            {
                SetProperty(ref bordersThickness, value);
                Messenger.Send(new BorderThicknessChangedMessage(value));
            }
        }

        public int BorderOpacity
        {
            get => borderOpacity;
            set
            {
                SetProperty(ref borderOpacity, value);
                Messenger.Send(new BackgroundOpacityChangedMessage(value / 100.0));
            }
        }

        public List<SolidColorBrush> Colors { get; set; }

        public BackgroundPageViewModel()
        {
            Colors = BrushGenerator.GenerateBrushes();
        }

        private static async void UploadBackgroundFromPhotoBtnClickMethod()
        {
            var file = await ImageHelper.OpenFilePicker();
            if (file != null)
            {
                using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    try
                    {
                        var decoder = await BitmapDecoder.CreateAsync(fileStream);
                        var source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                        await source.SetSourceAsync(fileStream);

                        var imgBrush = new ImageBrush();
                        imgBrush.ImageSource = source;

                        //WeakReferenceMessenger.Default.Send(imgBrush);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}

