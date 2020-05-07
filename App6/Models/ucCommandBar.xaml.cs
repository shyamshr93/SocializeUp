using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
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
    public sealed partial class ucCommandBar : UserControl
    {
        public static SolidColorBrush scbColor { get; set; }
        public static ncColor ncColor { get; set; }
        public ucCommandBar()
        {
            this.InitializeComponent();
            ncColor = new ncColor();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SystemInformation.OperatingSystemVersion.Build > 10586 && SystemInformation.DeviceFamily == "Windows.Desktop")
                abtInk.Visibility = Visibility.Visible;
            else
                abtInk.Visibility = Visibility.Collapsed;
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons") && UtilityData.isbtHardwareBack)
            {
                btback.Visibility = Visibility.Collapsed;
            }
            else
                btback.Visibility = Visibility.Visible;
            ncColor.PropertyChanged += NcColor_PropertyChanged;
            //cbAppbar.Background = UtilityData.isFluentDesign ? MainPage.topAcrylicBrush : MainPage.topSCBrush;
            cbAppbar.Background = MainPage.topSCBrush;

            if (UtilityData.OperatingSystemVersion.Build >= 17763)
            {
                cbAppbar.Margin = new Thickness(0, 8, 0, 0);
                bmiMiniMode.Margin = new Thickness(0, -5, 0, -13);
            }
        }

        private void NcColor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (ncColor.strColor == "Color")
            //    cbAppbar.Background = scbColor;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ncColor.PropertyChanged -= NcColor_PropertyChanged;
        }
    }

    public class ncColor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ncColor()
        {

        }
        private string Color;
        public string strColor
        {
            get { return Color; }
            set
            {
                Color = value;
                OnPropertyChanged("Color");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
