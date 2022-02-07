using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class BackgroundPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private ICommand _uploadBtnCommand;
        public ICommand UploadBtnCommand
        {
            get
            {
                if (_uploadBtnCommand == null)
                    _uploadBtnCommand = new RelayCommand(async () => 
                    {
                        var imageBrush = new ImageBrush();
                        StorageFile file = await ImageHelper.OpenFilePicker();

                        if (file != null)
                        {
                            using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                            {
                                try
                                {
                                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                                    WriteableBitmap source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                                    await source.SetSourceAsync(fileStream);
                                    imageBrush.ImageSource = source;
                                    Messenger.Default.Send(imageBrush as Brush);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    });
                return _uploadBtnCommand;
            }
        }


        private ICommand _colorBtnCommand;
        public ICommand ColorBtnCommand
        {
            get
            {
                if (_colorBtnCommand == null)
                    _colorBtnCommand = new RelayCommand<Brush>((parametr) => { Messenger.Default.Send(parametr); });
                return _colorBtnCommand;
            }
        }


        private int _bordersThickness;
        public int BordersThickness 
        { 
            get => _bordersThickness; 
            set { 
                Set(ref _bordersThickness, value);
                Messenger.Default.Send(new Thickness(value)); }
        }

        private int _borderOpacity;
        public int BorderOpacity { get => _borderOpacity;
            set 
            {
                Set(ref _borderOpacity, value);
                Messenger.Default.Send(value / 100.0);
            }
        }


        public List<SolidColorBrush> Colors { get; set; }

        public BackgroundPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Colors = BrushGenerator.GenerateBrushes();
        }

    }
}

