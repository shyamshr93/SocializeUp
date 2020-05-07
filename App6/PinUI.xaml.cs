using Microsoft.Services.Store.Engagement;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PinUI : Page
    {
        public PinUI()
        {
            this.InitializeComponent();
        }
        object parameter;
        private string pinString = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            parameter = e.Parameter;
            UtilityClass.titlebar("#0C0C0E");
            pin();
        }

        public async void pin()
        {
            try
            {
                StorageFile nameFile = await ApplicationData.Current.LocalFolder.GetFileAsync("name.txt");
                string nameString = await FileIO.ReadTextAsync(nameFile);
                if (string.IsNullOrWhiteSpace(nameString) || string.IsNullOrEmpty(nameString))
                {
                    tname.Text = "Welcome Back!";
                }
                else
                {
                    tname.Text = nameString;
                }
            }
            catch (Exception)
            {
            }
            pinString = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
        }

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Password == pinString)
                {
                    this.Frame.Navigate(typeof(MainPage), parameter);
                }
                else
                {
                    UtilityClass.MessageDialog("Password is Incorrect, Please Try Again! ", "Wrong Password");
                }
            }
            catch
            {

            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("isPassImage"))
                {
                    if ((bool)ApplicationData.Current.LocalSettings.Values["isPassImage"])
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFile imgFile = await folder.GetFileAsync("Image");
                        BitmapImage bitmapImage = new BitmapImage();
                        using (IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await bitmapImage.SetSourceAsync(stream);
                        }
                        imgProfile.Source = bitmapImage;
                    }
                    else
                    {
                        imgProfile.Source = new BitmapImage(new Uri("ms-appx:///Assets/account.png"));
                    }
                }
                else
                {
                    imgProfile.Source = new BitmapImage(new Uri("ms-appx:///Assets/account.png"));
                }
            }
            catch
            {
                imgProfile.Source = new BitmapImage(new Uri("ms-appx:///Assets/account.png"));
            }
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("isPassCoverImage"))
                {
                    if ((bool)ApplicationData.Current.LocalSettings.Values["isPassCoverImage"])
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFile imgFile = await folder.GetFileAsync("CoverImage");
                        BitmapImage bitmapImage = new BitmapImage();
                        using (IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await bitmapImage.SetSourceAsync(stream);
                        }
                        CoverImageBrush.ImageSource = bitmapImage;
                    }
                    else
                    {
                        CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10purple.jpg"));
                    }
                }
                else
                {
                    CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10purple.jpg"));
                }
            }
            catch
            {
                CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10purple.jpg"));
            }
            try
            {
                if (StoreServicesFeedbackLauncher.IsSupported())
                {
                    this.FindName("imgBlur");
                    imgBlur.Visibility = Visibility.Visible;
                    await imgProfile.Blur(10, 0, 0).StartAsync();
                }
                else
                {
                    if (imgBlur != null)
                        imgBlur.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                if (imgBlur != null)
                    imgBlur.Visibility = Visibility.Collapsed;
            }
        }

        private void tb1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tb1.Password))
            {
                bt1.IsEnabled = true;
                if (tb1.Password == pinString)
                {
                    bt1_Click(null, null);
                }
            }
            else
                bt1.IsEnabled = false;
        }

        private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                    bt1_Click(null, null);
                e.Handled = true;
            }
            catch (Exception)
            {

            }
        }
    }
}
