using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class PaintPopUpPageViewModel : ViewModelBase
    {
        private PaintClearEnum? _appBarBtnsEnum;
        public PaintClearEnum? AppBarBtnsEnum {
            get => _appBarBtnsEnum;
            set => Set(ref _appBarBtnsEnum, value); }


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
