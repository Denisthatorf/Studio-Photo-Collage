using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using Lumia.Imaging.Compositing;
using Lumia.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Models;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
{
    public class FiltersPageViewModel : ObservableObject
    {
        private ICommand applyEffectCommand;
        private ObservableCollection<ImageSourceAndEffect> effects;
        private List<Type> applyingEffects;
        private WriteableBitmap clearImageSource;
        private bool isChekedApplyToAllCheckBox;

        public ObservableCollection<ImageSourceAndEffect> Effects
        {
            get => effects;
            set => SetProperty(ref effects, value);
        }
        public List<Type> ApplyingEffects
        {
            get => applyingEffects;
            set => SetProperty(ref applyingEffects, value);
        }
        public ICommand ApplyEffectCommand => applyEffectCommand ??
            (applyEffectCommand = new RelayCommand<ImageSourceAndEffect>((p) => SetEffect(p)));

        public bool IsChekedApplyToAllCheckBox 
        {
            get => isChekedApplyToAllCheckBox; 
            set
            {
                isChekedApplyToAllCheckBox = value;
                var applyEffectsList = new List<Type>();

                if (value == true)
                {
                    applyEffectsList.AddRange(ApplyingEffects);
                    WeakReferenceMessenger.Default.Send(new ApplyEffectsToAllMessage(applyEffectsList));
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new ApplyEffectsToAllMessage(applyEffectsList));
                    if (clearImageSource != null)
                    {
                        applyEffectsList = new List<Type>();
                        applyEffectsList.AddRange(ApplyingEffects);
                        WeakReferenceMessenger.Default.Send(new ApplyEffectsMessage(applyEffectsList));
                    }
                }
            }
        }

        public FiltersPageViewModel()
        {
            effects = new ObservableCollection<ImageSourceAndEffect>();
            applyingEffects = new List<Type>();
            WeakReferenceMessenger.Default.Register<ChangeSelectedImageMessage>(this, async (r, m) =>
            {
                Effects.Clear();

                ApplyingEffects.Clear();

                if(m.Value != null)
                {
                    ApplyingEffects.AddRange(m.Value.EffectsTypes);
                    clearImageSource = await ImageHelper.FromBase64(m.Value.ImageBase64Clear) as WriteableBitmap;
                    SetImageWithFiltersAsync();
                }
            });
        }

        private async void SetImageWithFiltersAsync()
        {
            //var resultList = new ObservableCollection<ImageSourceAndEffect>();

            var effects = new List<IImageConsumer>()
             {
             new AntiqueEffect(),
             new Lumia.Imaging.Compositing.ChromaKeyEffect(),
             new Lumia.Imaging.Adjustments.BlurEffect(50),
             new Lumia.Imaging.Adjustments.AutoLevelsEffect(),
             new Lumia.Imaging.Adjustments.ColorAdjustEffect(0,0,1),
             new Lumia.Imaging.Adjustments.ColorAdjustEffect(0,1,0),
             new Lumia.Imaging.Adjustments.ColorAdjustEffect(1 ,0,1),
             new Lumia.Imaging.Adjustments.GrayscaleEffect(),
             new Lumia.Imaging.Adjustments.NoiseEffect(),
             new Lumia.Imaging.Artistic.AntiqueEffect(),
             new Lumia.Imaging.Artistic.CartoonEffect(),
             new Lumia.Imaging.Artistic.GrayscaleNegativeEffect(),
             new Lumia.Imaging.Artistic.LomoEffect(),
             new Lumia.Imaging.Artistic.MagicPenEffect(),
             new Lumia.Imaging.Artistic.MilkyEffect(),
             new Lumia.Imaging.Artistic.MoonlightEffect(),
             new Lumia.Imaging.Artistic.OilyEffect(),
             new Lumia.Imaging.Artistic.PaintEffect(),
             new Lumia.Imaging.Artistic.PosterizeEffect(),
             new Lumia.Imaging.Artistic.SepiaEffect(),
             new Lumia.Imaging.Artistic.SketchEffect(),
             new Lumia.Imaging.Artistic.SolarizeEffect(),
             new Lumia.Imaging.Artistic.WatercolorEffect()
             };

            var wrBitmap = clearImageSource;

            foreach (var effect in effects)
             {
                 if (wrBitmap != null && effect != null)
                 {
                    var writeableBitmap = await LumiaHelper.SetEffectToWritableBitmap(wrBitmap, effect);

                    bool isActive = ApplyingEffects.Contains(effect.GetType());
                    var imageSourceAndEffect = new ImageSourceAndEffect(writeableBitmap, effect.GetType(), isActive);
                    Effects.Add(imageSourceAndEffect);
                 }
             }
        }

        private void SetEffect(ImageSourceAndEffect effect)
        {
            if (ApplyingEffects.Contains(effect.EffecteType))
            {
                ApplyingEffects.Remove(effect.EffecteType);
            }
            else
            {
                ApplyingEffects.Add(effect.EffecteType);
            }

            var applyEffectsList = new List<Type>();
            applyEffectsList.AddRange(ApplyingEffects);

           if(IsChekedApplyToAllCheckBox == false)
           {
                WeakReferenceMessenger.Default.Send(new ApplyEffectsMessage(applyEffectsList));
           }
            else
            {
                WeakReferenceMessenger.Default.Send(new ApplyEffectsToAllMessage(applyEffectsList));
            }
        }
    }
}
