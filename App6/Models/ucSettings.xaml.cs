using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace App6.Models
{
    public sealed partial class ucSettings : UserControl
    {
        public static ncMainPage ncSettings { get; set; }
        public ucSettings()
        {
            this.InitializeComponent();
            ncSettings = new ncMainPage();
            ncSettings.PropertyChanged += NcSettings_PropertyChanged;
            OneTimeSave();
            TelegramLicense();
        }

        private void NcSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ncSettings.setSettings == "IAP")
            {
                TelegramLicense();
            }
        }

        private void TelegramLicense()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("UpTelegram"))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values["UpTelegram"] == true)
                {
                    cbtTelegram.Visibility = Visibility.Visible;
                    return;
                }
            }

            cbtTelegram.Visibility = Visibility.Collapsed;
        }

        private void OneTimeSave()
        {
            if (SystemInformation.DeviceFamily == "Windows.Mobile" || UtilityData.isFirstAppRun)
            {
                cbtWhatsApp.Visibility = Visibility.Collapsed;
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Tw)"))
            {
                HideTwitter.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(Tw)"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(I)"))
            {
                HideInstagram.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(I)"];
            }
            
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Mix)"))
            {
                HideMix.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(Mix)"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Wh)"))
            {
                HideWhatsApp.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(Wh)"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(R)"))
            {
                HideReddit.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(R)"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("cbPageSelection"))
            {
                cbPageSelection.SelectedIndex = (int)ApplicationData.Current.LocalSettings.Values["cbPageSelection"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("cbExtLinks"))
            {
                cbOpenExtLinks.SelectedIndex = (int)ApplicationData.Current.LocalSettings.Values["cbExtLinks"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(F)"))
            {
                HideFacebook.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(F)"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Tele)"))
            {
                HideTelegram.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["Hide(Tele)"];
            }
            if (cbPageSelection.SelectedIndex == -1)
            {
                cbPageSelection.SelectedIndex = 0;
            }
            if (cbOpenExtLinks.SelectedIndex == -1)
            {
                cbOpenExtLinks.SelectedIndex = 0;
            }
        }

        private void HideFacebook_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(F)"] = HideFacebook.IsOn;
        }

        private void HideWhatsApp_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(Wh)"] = HideWhatsApp.IsOn;
        }

        private void HideTwitter_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(Tw)"] = HideTwitter.IsOn;
        }

        private void HideMix_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(Mix)"] = HideMix.IsOn;
        }

        private void HideInstagram_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(I)"] = HideInstagram.IsOn;
        }

        private void HideReddit_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(R)"] = HideReddit.IsOn;
        }
        
        private void cbOpenExtLink_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["cbExtLinks"] = cbOpenExtLinks.SelectedIndex;
        }
        private void cbPageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["cbPageSelection"] = cbPageSelection.SelectedIndex;
        }

        private void HideTelegram_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Hide(Tele)"] = HideTelegram.IsOn;
        }
    }
}
