using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Studio_Photo_Collage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class BackgroundPageViewModel : ViewModelBase
    {
        private RelayCommand _uploadBtnCommand;

        public RelayCommand UploadBtnCommand
        {
            get { return _uploadBtnCommand; }
            set { _uploadBtnCommand = value; }
        }

        public BackgroundPageViewModel()
        {
            //_uploadBtnCommand = new RelayCommand(()=>Localize.LoadStringResource("ru-Ru"));
        }
    }
}
