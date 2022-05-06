using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Models;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
{
    public class BackgroundPageViewModel : ObservableRecipient
    {
        private ICommand uploadBtnCommand;
        private ICommand colorBtnCommand;
        private int bordersThickness;
        private int borderOpacity;
        private int selectedColorIndex;

        public ICommand UploadBtnCommand
        {
            get
            {
                if (uploadBtnCommand == null)
                {
                    uploadBtnCommand = new RelayCommand(() =>
                    {
                        UploadBackgroundFromPhotoBtnClickMethod();
                        SelectedColorIndex = -1;
                    });
                }

                return uploadBtnCommand;
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

        public int SelectedColorIndex
        {
            get => selectedColorIndex;
            set
            {
                SetProperty(ref selectedColorIndex, value);
                if(value > 0)
                {
                    Messenger.Send(Colors[value]);
                }
            }
        }
        public BackgroundPageViewModel()
        {
            Colors = ColorGenerator.GenerateBrushes();

            Messenger.Register<Project>(this, (r, m) =>
            {
                BorderOpacity = (int)(m.BorderOpacity * 100);
                BordersThickness = (int)m.BorderThickness;

                SelectedColorIndex = Colors
                    .IndexOf(Colors
                    .Where(x => x.Color == m.Background.ToColor())
                    .FirstOrDefault());
            });
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

                        WeakReferenceMessenger.Default.Send(imgBrush);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}

