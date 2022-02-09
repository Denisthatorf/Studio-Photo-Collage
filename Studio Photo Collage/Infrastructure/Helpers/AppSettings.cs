using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public class AppSettings : INotifyPropertyChanged
    {
        public bool ShowTimeSheetSetting
        {
            get
            {
                return ReadSettings(nameof(ShowTimeSheetSetting), true);
            }
            set
            {
                SaveSettings(nameof(ShowTimeSheetSetting), value);
                NotifyPropertyChanged();
            }
        }
        public bool UsePrimaryLanguageOverride
        {
            get => ReadSettings(nameof(UsePrimaryLanguageOverride), false);
            set
            {
                SaveSettings(nameof(UsePrimaryLanguageOverride), value);
                NotifyPropertyChanged();
            }
        }
        public string PrimaryLanguageOverride
        {
            get => ReadSettings(nameof(PrimaryLanguageOverride), "en-US");
            set
            {
                SaveSettings(nameof(PrimaryLanguageOverride), value);
                NotifyPropertyChanged();
            }
        }

        public ApplicationDataContainer LocalSettings { get; set; }

        public AppSettings()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        private void SaveSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        private T ReadSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }
            if (null != defaultValue)
            {
                return defaultValue;
            }
            return default(T);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
