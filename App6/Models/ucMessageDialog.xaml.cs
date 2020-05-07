using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class ucMessageDialog : UserControl
    {
        public ucMessageDialog()
        {
            this.InitializeComponent();
        }

        private void BtCloseDialog_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            MainGrid.Opacity = 0;
        }

        private void BtExternal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IAsyncOperation<bool> d = Launcher.LaunchUriAsync(new Uri(tbLink.Text));
            }
            catch (Exception)
            {

            }
            finally
            {
                this.Visibility = Visibility.Collapsed;
                MainGrid.Opacity = 0;
            }
        }

        private void BtAnotherWindows_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UtilityClass.MultipleInstance(new Uri(tbLink.Text));
            }
            catch (Exception)
            {

            }
            finally
            {
                this.Visibility = Visibility.Collapsed;
                MainGrid.Opacity = 0;
            }
        }
        
    }
}
