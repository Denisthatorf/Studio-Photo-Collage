using GalaSoft.MvvmLight;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class PaintPopUpPageViewModel : ViewModelBase
    {
        private PaintClearEnum? _appBarBtnsEnum;
        public PaintClearEnum? AppBarBtnsEnum
        {
            get => _appBarBtnsEnum;
            set => Set(ref _appBarBtnsEnum, value);
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
