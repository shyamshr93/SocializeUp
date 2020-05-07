using App6.Models;
using Microsoft.Services.Store.Engagement;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
    public sealed partial class setpin : Page
    {
        bool isPassword = false;
        StorageFile nameFile;
        StorageFile tempImage = null;
        StorageFile tempCoverImage = null;
        private Color titleBarColor;

        public setpin()
        {
            this.InitializeComponent();
            abc();
            btSelectimage.Visibility = Visibility.Visible;
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            UtilityClass.titlebar(scbBrush: new SolidColorBrush(titleBarColor));
        }

        public async void abc()
        {
            try
            {
                string pinString = "";
                nameFile = await ApplicationData.Current.LocalFolder.GetFileAsync("name.txt");
                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Password"))
                {
                    try
                    {
                        StorageFile sampleFile = await ApplicationData.Current.LocalFolder.GetFileAsync("data.txt");
                        ApplicationData.Current.LocalSettings.Values["Password"] = await FileIO.ReadTextAsync(sampleFile);
                    }
                    catch (Exception)
                    {
                        isPassword = false;
                    }
                }
                else
                {
                    pinString = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
                }
                isPassword = pinString != "" ? true : false;
            }
            catch (FileNotFoundException)
            {
                nameFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("name.txt");
                isPassword = false;
            }
            if (isPassword)
            {
                spSetPassword.Visibility = Visibility.Collapsed;
                btremovepin.Visibility = Visibility.Visible;
                spResetPassword.Visibility = Visibility.Visible;
            }
            else
            {
                spResetPassword.Visibility = Visibility.Collapsed;
                btremovepin.Visibility = Visibility.Collapsed;
                spSetPassword.Visibility = Visibility.Visible;
            }
        }

        private async void btset_Click(object sender, RoutedEventArgs e)
        {
            if (!isPassword)
            {
                if (tbset1.Password == tbset2.Password)
                {
                    if (string.IsNullOrEmpty(tbset1.Password) || string.IsNullOrWhiteSpace(tbset1.Password))
                    {
                        UtilityClass.MessageDialog("Password cannot be empty or white space, Try again!", "Failed to Set Password");
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values["Password"] = tbset1.Password;
                        await FileIO.WriteTextAsync(nameFile, tbname1.Text);

                        if (tempImage != null)
                        {
                            StorageFolder folder = ApplicationData.Current.LocalFolder;
                            StorageFile newFile = await tempImage.CopyAsync(folder, "Image", NameCollisionOption.ReplaceExisting);
                            ApplicationData.Current.LocalSettings.Values["isPassImage"] = true;
                        }
                        else
                        {
                            ApplicationData.Current.LocalSettings.Values["isPassImage"] = false;
                        }
                        if (tempCoverImage != null)
                        {
                            StorageFolder folder = ApplicationData.Current.LocalFolder;
                            StorageFile newFile = await tempCoverImage.CopyAsync(folder, "CoverImage", NameCollisionOption.ReplaceExisting);
                            ApplicationData.Current.LocalSettings.Values["isPassCoverImage"] = true;
                        }
                        else
                        {
                            ApplicationData.Current.LocalSettings.Values["isPassCoverImage"] = false;
                        }

                        UtilityClass.MessageDialog("Password has been Set Successfully!", "Password Set Successfully.");
                        Frame.GoBack();
                        spSetPassword.Visibility = Visibility.Collapsed;
                        spResetPassword.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    UtilityClass.MessageDialog("Password Didn't Match, Try Again!", "Wrong Password");
                }
            }
            else
            {
                RemoveorResetPass(null);
            }
        }

        private async void RemoveorResetPass(string Pass)
        {
            string pinString = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
            if (Pass == null)
            {
                if (string.IsNullOrEmpty(tbreset2.Password) || string.IsNullOrWhiteSpace(tbreset3.Password))
                {
                    UtilityClass.MessageDialog("Password cannot be empty or white space, Try again!", "Failed to Set Password");
                }
                else if (tbreset1.Password == pinString && tbreset2.Password == tbreset3.Password)
                {
                    ApplicationData.Current.LocalSettings.Values["Password"] = tbreset3.Password;
                    if (tempImage != null)
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFile newFile = await tempImage.CopyAsync(folder, "Image", NameCollisionOption.ReplaceExisting);
                        ApplicationData.Current.LocalSettings.Values["isPassImage"] = true;
                    }
                    if (tempCoverImage != null)
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFile newFile = await tempCoverImage.CopyAsync(folder, "CoverImage", NameCollisionOption.ReplaceExisting);
                        ApplicationData.Current.LocalSettings.Values["isPassCoverImage"] = true;
                    }
                    Frame.GoBack();
                    UtilityClass.MessageDialog("Password has been Changed Successfully!", "Password Changed");
                }
                else
                {
                    UtilityClass.MessageDialog("Password Didn't Match, Try Again!", "Wrong Password");
                }
            }
            else if (Pass == pinString)
            {
                ApplicationData.Current.LocalSettings.Values["Password"] = "";
                ApplicationData.Current.LocalSettings.Values["isPassImage"] = false;
                ApplicationData.Current.LocalSettings.Values["isPassCoverImage"] = false;
                Frame.GoBack();
                UtilityClass.MessageDialog("Password has been removed!", "Password removed successfully");
            }
            else
            {
                UtilityClass.MessageDialog("You have entered Wrong Password, Please Try Again", "Wrong Password");
            }
        }

        private void btcancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBarColor = (Color)titleBar.BackgroundColor;
            UtilityClass.titlebar("#0C0C0E");

            tempImage = null;
            tempCoverImage = null;
            visualSetters(vsgMain.CurrentState.Name);
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
            catch (Exception ex)
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
                        CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10Purple.jpg"));
                    }
                }
                else
                {
                    CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10Purple.jpg"));
                }
            }
            catch (Exception ex)
            {
                CoverImageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/win10Purple.jpg"));
            }


            try
            {
                if (UtilityData.isFluentDesign)
                {
                    spPassCont.Background = new AcrylicBrush()
                    {
                        FallbackColor = ((SolidColorBrush)spPassCont.Background).Color,
                        TintColor = ((SolidColorBrush)spPassCont.Background).Color,
                        TintOpacity = 0.9,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    };

                }
            }
            catch (Exception)
            {
            }
        }

        private async void btremovepin_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string pinString = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
            var cntRemovePin = new ContentDialog
            {
                Title = "Do you really want to remove the Password ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
            };
            StackPanel sp = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Vertical
            };
            TextBlock tb = new TextBlock
            {
                Text = "Please Enter your Current Password",
                Margin = new Thickness(0, 10, 0, 0)
            };
            PasswordBox txb = new PasswordBox
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                PlaceholderText = "Your Current Password",
                Margin = new Thickness(0, 5, 0, 0)
            };
            sp.Children.Add(tb);
            sp.Children.Add(txb);
            cntRemovePin.Content = sp;
            cntRemovePin.PrimaryButtonClick += (s, a) =>
            {
                cntRemovePin.Hide();
                if (pinString != null && txb.Password == pinString)
                {
                    RemoveorResetPass(pinString);
                }
                else
                {
                    RemoveorResetPass("NoPass123");
                }
            };
            await cntRemovePin.ShowAsync();
        }

        private async void saveImage(StorageFile storageFile, bool isCoverPhoto)
        {
            try
            {
                if (storageFile != null)
                {
                    var contentType = storageFile.ContentType;

                    if (!isCoverPhoto)
                    {
                        if (contentType.Contains("image/"))
                        {
                            tempImage = storageFile;
                            var bitmapImage = new BitmapImage();
                            using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                            {
                                bitmapImage.SetSource(stream);
                            }
                            imgProfile.Source = bitmapImage;
                        }
                        else
                        {
                            tempImage = null;
                            UtilityClass.MessageDialog("Sorry, Only Image is supported. Try Again!", "File not supported");
                        }
                    }
                    else
                    {
                        if (contentType.Contains("image/"))
                        {
                            tempCoverImage = storageFile;
                            var bitmapImage = new BitmapImage();
                            using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                            {
                                bitmapImage.SetSource(stream);
                            }
                            CoverImageBrush.ImageSource = bitmapImage;
                        }
                        else
                        {
                            tempCoverImage = null;
                            UtilityClass.MessageDialog("Sorry, Only Image is supported. Try Again!", "File not supported");
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void tbset2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbset1.Password != null && tbset1.Password != "" && tbset2.Password != null && tbset2.Password != "")
                btset.IsEnabled = true;
            else
                btset.IsEnabled = false;
        }

        private void tbreset3_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbreset1.Password != null && tbreset1.Password != "" && tbreset2.Password != null && tbreset2.Password != "" && tbreset3.Password != null && tbreset3.Password != "")
                btset.IsEnabled = true;
            else
                btset.IsEnabled = false;
        }

        //private async Task<StorageFile> PickPhotofromUserAsync()
        //{
        //    try
        //    {
        //        FileOpenPicker filePicker = new FileOpenPicker();
        //        filePicker.FileTypeFilter.Add(".png");
        //        filePicker.FileTypeFilter.Add(".jpeg");
        //        filePicker.FileTypeFilter.Add(".jpg");
        //        filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //        return await filePicker.PickSingleFileAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        private async void btSelectimage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add("*");
                filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                StorageFile pickedimage = await filePicker.PickSingleFileAsync();
                if (pickedimage != null)
                {
                    saveImage(pickedimage, false);
                }
                else
                {
                    btSelectimage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
            }
        }

        private void gdImage_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "Drop Here";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void gdImage_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.DataView.Contains(StandardDataFormats.StorageItems))
                {
                    var items = await e.DataView.GetStorageItemsAsync();
                    if (items.Any())
                    {
                        var storageFile = items[0] as StorageFile;
                        saveImage(storageFile, false);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    btset_Click(sender, e);
                    e.Handled = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private async void btChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add("*");
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            StorageFile pickedimage = await filePicker.PickSingleFileAsync();
            if (pickedimage != null)
            {
                saveImage(pickedimage, true);
            }
            else
            {
                //btSelectimage.Visibility = Visibility.Visible;
            }
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            //try
            //{
            //    if (e.DataView.Contains(StandardDataFormats.StorageItems))
            //    {
            //        var items = await e.DataView.GetStorageItemsAsync();
            //        if (items.Any())
            //        {
            //            var storageFile = items[0] as StorageFile;
            //            saveImage(storageFile, true);
            //        }
            //    }
            //}
            //catch (Exception)
            //{

            //}
        }

        private void VsgMain_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            visualSetters(e.NewState.Name);
        }

        private void visualSetters(string setter)
        {
            if (setter == "vsMobile")
            {
                try
                {
                    gdImgHeader.Scale((float)0.7, (float)0.7, duration: 500).Start();
                    gdImgHeader.Offset((float)0, (float)-15, duration: 500).Start();
                    spHeader.Offset((float)-50, (float)-15, duration: 500).Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    gdImgHeader.Scale((float)1, (float)1, duration: 500).Start();
                    gdImgHeader.Offset((float)0, (float)0, duration: 500).Start();
                    spHeader.Offset((float)0, (float)0, duration: 500).Start();
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
