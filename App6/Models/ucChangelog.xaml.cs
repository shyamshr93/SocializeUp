using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace App6.Models
{
    public sealed partial class ucChangelog : UserControl
    {
        public ucChangelog()
        {
            this.InitializeComponent();
            Changelog();
        }

        private async void Changelog()
        {
            try
            {
                await gdBlur.Blur(10, 0, 0).StartAsync();
            }
            catch (Exception)
            {

            }
            try
            {
                tbChangelog.Text = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/Changelog.txt")));
            }
            catch (Exception)
            {

            }
            if (UtilityData.isFluentDesign)
            {
                //SetAcrylicBackground(gdBlur, 0.4);
                SetAcrylicBackground(gdMain, 0.8);
                gdTopPanel.Background = gdMain.Background;
                btClose.Style = Application.Current.Resources["ButtonRevealStyle"] as Style;
            }
        }

        private void SetAcrylicBackground(Panel panel, double opacity)
        {
            if (UtilityData.isFluentDesign)
            {
                panel.Background = new AcrylicBrush()
                {
                    FallbackColor = ((SolidColorBrush)panel.Background).Color,
                    TintColor = ((SolidColorBrush)panel.Background).Color,
                    TintOpacity = opacity,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop
                };
            }
        }

        private async void btClose_Click(object sender, RoutedEventArgs e)
        {
            await this.Fade(0, 1000, 0).StartAsync();
            this.Visibility = Visibility.Collapsed;
        }


        private async void btAboutSupport_Click(object sender, RoutedEventArgs e)
        {
            Button btsender = sender as Button;
            try
            {
                Uri uri = null;
                switch (btsender.Tag.ToString())
                {
                    case "mail":
                        uri = new Uri("mailto:definestudio@outlook.com?Body=Sent%20from%20Socialize%20Up");
                        break;
                    case "rating":
                        uri = new Uri("ms-windows-store:REVIEW?PFN=5913DefineStudio.SocializeUp_jj4r3mnwe2ey2");
                        break;
                    case "twitter":
                        uri = new Uri("https://twitter.com/define_studio");
                        break;
                    case "site":
                        uri = new Uri("http://www.definestudio.in/");
                        break;
                }
                var launched = await Launcher.LaunchUriAsync(uri);
            }
            catch (Exception)
            {

            }
        }
        
    }
}
