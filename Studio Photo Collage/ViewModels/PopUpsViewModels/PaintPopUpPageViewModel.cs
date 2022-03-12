using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Messages;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class PaintPopUpPageViewModel : ObservableObject
    {
        private int hexValue;
        private int brushSize;
        private PaintClearEnum? _appBarBtnsEnum;
        public PaintClearEnum? AppBarBtnsEnum
        {
            get => _appBarBtnsEnum;
            set => SetProperty(ref _appBarBtnsEnum, value);
        }
        public int HexValue
        {
            get => hexValue;
            set
            {
                hexValue = value;
                var color = ColorHelper.FromHsl(value, 1, 0.5);
                WeakReferenceMessenger.Default.Send(new PainColorChangedMessage(color));
            }
        }
        public int BrushSize
        {
            get => brushSize; 
            set
            {
                brushSize = value;
                WeakReferenceMessenger.Default.Send(new BrushSizeMessage(value));
            }
        }

        public PaintPopUpPageViewModel()
        {
            _appBarBtnsEnum = null;
        }
    }
    public enum PaintClearEnum
    {
        Pen,
        Rubber
    }
}
