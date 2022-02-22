using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class PaintPopUpPageViewModel : ObservableObject
    {
        private PaintClearEnum? _appBarBtnsEnum;
        public PaintClearEnum? AppBarBtnsEnum
        {
            get => _appBarBtnsEnum;
            set => SetProperty(ref _appBarBtnsEnum, value);
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
