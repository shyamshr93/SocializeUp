using App6.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OOBE : Page
    {
        public OOBE()
        {
            this.InitializeComponent();
            UtilityData.isFirstAppRun = true;
        }

        private void btGetStarted_Click(object sender, RoutedEventArgs e)
        {
            gdFrontWelcome.Visibility = Visibility.Collapsed;
            try
            {
                gdSettings.Fade((float)1, 500).Start();
                gdCircle1.Offset(offsetX: 20, duration: 700, easingType: EasingType.Back).Start();
                gdSettings.Offset(offsetX: 20, duration: 700).Start();
            }
            catch (Exception)
            {
                gdSettings.Opacity = 1;
            }

            btGetStarted.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UtilityClass.titlebar("#0C0C0E");
            HideCompactMenu.IsOn = (bool)UtilityClass.ApplicationDataBool("HideCompactMenu", false);
            try
            {
                UtilityClass.moveXOffset(gdSettings, -20);
            }
            catch (Exception)
            {

            }
        }

        object args = null;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            args = e.Parameter;
        }

        private void btLetsGo_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), args);
        }

        private void HideCompactMenu_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["HideCompactMenu"] = HideCompactMenu.IsOn;
        }
    }
}
