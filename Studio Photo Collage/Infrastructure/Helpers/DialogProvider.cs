using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
   /* public static class DialogProvider 
    {
        public static List<ContentDialog> AllContentDialogs { get; set; } = new List<ContentDialog>();

        private static SettingsDialog _settingsContentDialog;
        static public SettingsDialog SettingsContentDialog
        {
            get
            {
                if (_settingsContentDialog == null)
                {
                    _settingsContentDialog = new SettingsDialog();
                    AllContentDialogs.Add(_settingsContentDialog);
                }
                return _settingsContentDialog;
            }
        }

        private static ConfirmDialog _confirmDialog;
        public static ConfirmDialog ConfirmContentDialog
        {
            get
            {
                if (_confirmDialog == null)
                {
                    _confirmDialog = new ConfirmDialog();
                    AllContentDialogs.Add(_confirmDialog);
                }
                return _confirmDialog;
            }
        }

        private static SaveDialog _saveDialog;
        public static SaveDialog SaveContentDialog
        {
            get 
            {
                if (_saveDialog == null)
                {
                    _saveDialog = new SaveDialog("project");
                    AllContentDialogs.Add(_saveDialog);
                }
                return _saveDialog; 
            }
        }

        public static void ShowSettingsDialogAsync()
        {
            var dialog = new SettingsDialog();
           // await dialog.ShowAsync();
        }

        *//*public static async Task<bool> ShowConfirmDialogAsync(string _allOrThisCollages)
        {
            ConfirmContentDialog.AllOrThis = _allOrThisCollages;
            var result = await ConfirmContentDialog.ShowAsync();
            return ContentDialogResultToBool(result);
            
        }

      

        public static async Task<bool> ShowSaveDialogAsync(string _whatSave)
        {
            SaveContentDialog.WhatSave = _whatSave;
            var result = await

        }*//*

        static bool ContentDialogResultToBool(ContentDialogResult result)
        {
            if (result.ToString() == "Primary")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }*/
}
