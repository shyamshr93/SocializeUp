using App6.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
    public sealed partial class BlankPage5 : Page
    {
        private Compositor _compositor;

        public static ncMainPage notifyChange { get; set; }
        public BlankPage5()
        {
            this.InitializeComponent();
            notifyChange = new ncMainPage();
            notifyChange.PropertyChanged += NotifyChange_PropertyChanged;
            _ucCommandBar.btrefresh.Click += Refresh_Click;
            _ucCommandBar.btback.Click += Back_Click;
            _ucCommandBar.btforward.Click += btforward_Click;
            _ucCommandBar.abtInk.Click += abtInk_Click;
            _ucCommandBar.btsetting.Click += btsetting_Click;
            OneTimeSave();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void NotifyChange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (notifyChange.setSettings == "Home")
                web.Navigate(new Uri("https://www.reddit.com/login"));
        }

        private void OneTimeSave()
        {
            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            _ucCommandBar.btsetting.Visibility = Visibility.Visible;
        }


        private void LoadSaveData()
        {
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoBack)
            {
                web.GoBack();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            prRing.Visibility = Visibility.Visible;
            web.Refresh();
            if (_ucErrorPage != null)
                _ucErrorPage.Visibility = Visibility.Collapsed;
        }

        private async void abtInk_Click(object sender, RoutedEventArgs e)
        {
            if (_ucInkWeb == null)
                this.FindName("_ucInkWeb");
            _ucInkWeb.Visibility = Visibility.Visible;
            using (InMemoryRandomAccessStream streams = new InMemoryRandomAccessStream())
            {
                await web.CapturePreviewToStreamAsync(streams);
                BitmapSource thumbnailBitmap = await UtilityClass.CreateScaledBitmapFromStreamAsync(web, streams);
                _ucInkWeb.imgInk.Source = thumbnailBitmap;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (UtilityData.isbtHardwareBack)
            {
                if (web.CanGoBack) web.GoBack();
                e.Handled = true;
            }
        }
        private void btforward_Click(object sender, RoutedEventArgs e)
        {
            //string imgUri = await UtilityClass.retrieveWebPhotoLink(web.Source.ToString(), true);
            if (web.CanGoForward)
            {
                web.GoForward();
            }
        }

        private void btsetting_Click(object sender, RoutedEventArgs e)
        {
            if (gdRectVisibility == null)
            {
                this.FindName("gdRectVisibility");
                if (UtilityData.isFluentDesign && spContentSplitView.Background is SolidColorBrush)
                {
                    spContentSplitView.Background = new AcrylicBrush()
                    {
                        FallbackColor = ((SolidColorBrush)spContentSplitView.Background).Color,
                        TintColor = ((SolidColorBrush)spContentSplitView.Background).Color,
                        TintOpacity = 0.6,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    };
                }
            }

            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NoOfDevClick"))
            //{
            //    string strNoOfClick = ApplicationData.Current.LocalSettings.Values["NoOfDevClick"].ToString();

            //    int noOfClick = Convert.ToInt32(strNoOfClick);
            //    if (noOfClick >= 5)
            //    {
            //        this.FindName("btRefreshDarkMode");
            //        btRefreshDarkMode.Visibility = Visibility.Visible;
            //    }
            //}
            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
            if (SecondaryTile.Exists("secRedditId"))
            {
                tbTransparentTileIcon.Text = "\uE77A";
                tbTransparentTile.Text = "Unpin from Start";
            }
            else
            {
                tbTransparentTileIcon.Text = "\uE840";
                tbTransparentTile.Text = "Pin to Start";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSaveData();
        }

        private void web_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            args.StopPageScriptExecution = true;
        }

        private void web_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _ucCommandBar.btforward.IsEnabled = web.CanGoForward;
            _ucCommandBar.iconRotation.Stop();
            prRing.Visibility = Visibility.Collapsed;
            //style();
        }

        private void web_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            switch (UtilityData.LinksSetting)
            {
                case 3:
                    if (_ucMessageDialog == null)
                    {
                        this.FindName("_ucMessageDialog");
                        if (UtilityData.isFluentDesign)
                        {
                            _ucMessageDialog.MainGrid.Background = new AcrylicBrush()
                            {
                                BackgroundSource = AcrylicBackgroundSource.Backdrop,
                                TintOpacity = 0.9,
                                TintColor = (_ucMessageDialog.MainGrid.Background as SolidColorBrush).Color,
                                FallbackColor = (_ucMessageDialog.MainGrid.Background as SolidColorBrush).Color
                            };
                        }
                        _ucMessageDialog.btSameWindow.Click += (s, a) =>
                        {
                            web.Navigate(args.Uri);
                            _ucMessageDialog.Visibility = Visibility.Collapsed;
                            _ucMessageDialog.MainGrid.Opacity = 0;
                        };
                    }
                    _ucMessageDialog.Visibility = Visibility.Visible;
                    try
                    {
                        _ucMessageDialog.MainGrid.Fade((float)1, 200).Start();
                    }
                    catch (Exception)
                    {
                        _ucMessageDialog.MainGrid.Opacity = 1;
                    }
                    _ucMessageDialog.tbLink.Text = args.Uri.ToString();
                    break;
                case 0:
                    IAsyncOperation<bool> b = Launcher.LaunchUriAsync(args.Uri);
                    break;
                case 1:
                    web.Navigate(args.Uri);
                    break;
                case 2:
                    UtilityClass.MultipleInstance(args.Uri);
                    break;
            }
        }

        private void web_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            IAsyncOperation<bool> b = Windows.System.Launcher.LaunchUriAsync(args.Uri);
        }

        private void web_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            // style();
        }

        private void web_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (prRing == null)
                return;
            prRing.Visibility = Visibility.Visible;
            if (_ucCommandBar.iconRotation.GetCurrentState() != Windows.UI.Xaml.Media.Animation.ClockState.Active)
                _ucCommandBar.iconRotation.Begin();
            //style();
        }

        private void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("about:blank"))
                web.Navigate(new Uri("https://www.reddit.com/login"));
            if (!args.IsSuccess)
            {
                if (_ucErrorPage == null)
                {
                    this.FindName("_ucErrorPage");
                    _ucErrorPage.btErrorRefresh.Click += Refresh_Click;
                }
                _ucErrorPage.Visibility = Visibility.Visible;
                _ucErrorPage.tbErrorCode.Text = "Error Code - " + args.WebErrorStatus.ToString();
            }
            else
            {
                if (_ucErrorPage != null) _ucErrorPage.Visibility = Visibility.Collapsed;
            }
            _ucCommandBar.iconRotation.Stop();
            //style();
        }


        private void BtDLPhotoLink_Click(object sender, RoutedEventArgs e)
        {
            if (gdImageDownloadContainer == null)
            {
                this.FindName("gdImageDownloadContainer");

                if (UtilityData.isFluentDesign)
                {
                    gdImageDownloadContainer.Background = new AcrylicBrush()
                    {
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        TintOpacity = 0.2,
                        TintColor = (gdImageDownloadContainer.Background as SolidColorBrush).Color,
                        FallbackColor = (gdImageDownloadContainer.Background as SolidColorBrush).Color
                    };

                    gdPhotoScndContainer.Background = new AcrylicBrush()
                    {
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        TintOpacity = 0.5,
                        TintColor = (gdPhotoScndContainer.Background as SolidColorBrush).Color,
                        FallbackColor = (gdPhotoScndContainer.Background as SolidColorBrush).Color
                    };
                }

                UtilityClass.TranslationAnimation(_compositor, gdPhotoScndContainer, false);
            }

            gdImageDownloadContainer.Visibility = Visibility.Visible;
            gdMediaContainer.Visibility = Visibility.Collapsed;
            spDLMediaProgress.Visibility = Visibility.Collapsed;
            mdePlayer.Source = null;

            txtRedditLink.Text = "";
            if (web.Source.AbsoluteUri.Contains("www.reddit.com/"))
            {
                txtRedditLink.Text = web.Source.AbsoluteUri;
            }

            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
        }



        private async Task manualSavePhoto(string imgUri)
        {
            try
            {
                Uri source = new Uri(imgUri);

                FileSavePicker savePicker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.Downloads };
                if (imgUri.Contains(".mp4"))
                {
                    savePicker.FileTypeChoices.Add("MP4 Format", new List<string>() { ".mp4" });
                }
                else if (imgUri.Contains(".m3u8"))
                {
                    savePicker.FileTypeChoices.Add("M3U8 Format", new List<string>() { ".m3u8" });
                }
                else
                {
                    savePicker.FileTypeChoices.Add("Portable Network Graphics", new List<string>() { ".png" });
                    savePicker.FileTypeChoices.Add("JPG Format", new List<string>() { ".jpg" });
                }
                savePicker.SuggestedFileName = "Untitled";

                StorageFile destinationFile = await savePicker.PickSaveFileAsync();
                if (destinationFile != null)
                {
                    BackgroundDownloader downloader = new BackgroundDownloader();
                    DownloadOperation download = downloader.CreateDownload(source, destinationFile);
                    await download.StartAsync();
                }
            }
            catch (Exception ex)
            {
                //LogException("Download Error", ex);
            }
        }


        private async void TxtRedditLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                spDLMediaProgress.Visibility = Visibility.Visible;
                string imgUri = await UtilityClass.retrieveWebPhotoLink(txtRedditLink.Text, "Reddit");
                if (imgUri != null)
                {
                    imgRedditPhoto.Source = null;
                    gdMediaContainer.Visibility = Visibility.Visible;

                    if (imgUri.Contains(".m3u8") || imgUri.Contains(".mp4"))
                    {
                        imgRedditPhoto.Visibility = Visibility.Collapsed;

                        mdePlayer.Source = new Uri(imgUri);
                        mdePlayer.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        imgRedditPhoto.Visibility = Visibility.Visible;
                        mdePlayer.Visibility = Visibility.Collapsed;
                        imgRedditPhoto.Source = new BitmapImage(new Uri(imgUri));
                    }
                }
                gdLinkErrorDownload.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                gdMediaContainer.Visibility = Visibility.Collapsed;
            }
            finally
            {
                spDLMediaProgress.Visibility = Visibility.Collapsed;
            }
        }

        private void BtCancelDownloadUI_Click(object sender, RoutedEventArgs e)
        {
            if (gdImageDownloadContainer != null)
            {
                gdImageDownloadContainer.Visibility = Visibility.Collapsed;
                gdMediaContainer.Visibility = Visibility.Collapsed;
                mdePlayer.Source = null;
            }
        }

        private async void BtDownloadPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (imgRedditPhoto.Visibility == Visibility.Visible)
            {
                if (imgRedditPhoto != null && imgRedditPhoto.Source != null)
                    await manualSavePhoto(((BitmapImage)imgRedditPhoto.Source).UriSource.ToString());
            }
            else
            {
                if (mdePlayer != null && mdePlayer.Source != null)
                    await manualSavePhoto(((Uri)mdePlayer.Source).ToString());
            }
        }

        private async void BtDownloadRedditPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            await manualSavePhoto(await UtilityClass.retrieveWebPhotoLink(web.Source.AbsoluteUri, "Reddit"));
        }

        private void BtRemoveDownloadPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            //if (gdDownloadPhotoOverlay != null)
            //    gdDownloadPhotoOverlay.Visibility = Visibility.Collapsed;
        }

        private void BtGoDownloadPhoto_Click(object sender, RoutedEventArgs e)
        {
            gdLinkErrorDownload.Visibility = gdMediaContainer.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtMoveXOffset_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            UtilityClass.moveYOffset((Button)sender, -5);
        }

        private void BtMoveXOffset_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            UtilityClass.moveYOffset((Button)sender, 0);
        }

        private async void btPintoStart_Click(object sender, RoutedEventArgs e)
        {
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/reddit.png", "secRedditId", "Reddit", "jplReddit", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }
    }
}
