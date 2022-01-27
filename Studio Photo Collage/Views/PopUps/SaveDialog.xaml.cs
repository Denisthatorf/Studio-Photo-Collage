using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class SaveDialog : ContentDialog
    {
        public string ProjectName { get; set; }

        private string _whatSave;
        public string WhatSave
        {
            get { return _whatSave; }
            set 
            { 
                _whatSave = value;
                if (WhatSave == "project")
                    this.SecondaryButtonText = "Save";
                else if (WhatSave == "collage")
                {
                    this.PrimaryButtonText = "Yes";
                    this.SecondaryButtonText = "No";
                }
            }
        }

        public SaveDialog(string projectOrCollages)
        {
            this.InitializeComponent();
            WhatSave = projectOrCollages;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
