using App6.Models;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.Core;
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
    public sealed partial class BlankPage7 : Page
    {
        public static ncMainPage notifyChange { get; set; }
        string DesktopCss = "";
        string styleDisplayNone = "";
        string lightModeCSS = "";
        private bool isFirstLoaded = false;
        private Compositor _compositor;
        public BlankPage7()
        {
            this.InitializeComponent();
            notifyChange = new ncMainPage();
            notifyChange.PropertyChanged += NotifyChange_PropertyChanged;
            _ucCommandBar.btrefresh.Click += Refresh_Click;
            _ucCommandBar.btback.Click += Back_Click;
            _ucCommandBar.btforward.Click += btforward_Click;
            _ucCommandBar.btsetting.Click += btsetting_Click;
            _ucCommandBar.abtInk.Click += abtInk_Click;
            OneTimeSave();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        private void NotifyChange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (notifyChange.setSettings == "Home")
                web.Navigate(new Uri("https://www.instagram.com/accounts/login/"));
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
            if (e.Parameter != null)
            {
                try
                {
                    Uri url = (Uri)e.Parameter;
                    web.Navigate(url);
                }
                catch (Exception)
                {

                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4023.0 Safari/537.36 Edg/81.0.396.0");
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (UtilityData.isbtHardwareBack)
            {
                if (web.CanGoBack) web.GoBack();
                e.Handled = true;
            }
        }
        private void CompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            _ucCommandBar.Visibility = Visibility.Visible;
            MainPage.ncSettings.setSettings = "HideCompactOverlay";
        }

        private void btCompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            _ucCommandBar.Visibility = Visibility.Collapsed;
            MainPage.ncSettings.setSettings = "ShowCompactOverlay";
        }

        private async void OneTimeSave()
        {
            isFirstLoaded = true;
            try
            {
                if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
                {
                    _ucCommandBar.btCompactOverlay.Visibility = Visibility.Visible;
                    _ucCommandBar.btCompactOverlay.Click += btCompactOverlay_Click;
                }
            }
            catch (Exception)
            {

            }
            _ucCommandBar.btsetting.Visibility = Visibility.Visible;

            chbGoToTop.IsChecked = (bool)UtilityClass.ApplicationDataBool("isGoToTop(Insta)", false);

            EnableDarkMode.IsOn = UtilityData.isInstagramDark;
            if (!EnableDarkMode.IsOn)
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#8D3EBB"));

            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            DesktopCss = UtilityData.getCss("Insta");
            style();
            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/InstaMain", "Instagram", "InstaMain");
            if (_DesktopCss != "")
            {
                var result = _DesktopCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone = result[0];
                }
                DesktopCss = _DesktopCss;
                style();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            prRing.Visibility = Visibility.Visible;
            //web.Navigate(new Uri("https://www.instagram.com"));
            web.Refresh();
            if (_ucErrorPage != null)
                _ucErrorPage.Visibility = Visibility.Collapsed;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoBack)
            {
                web.GoBack();
            }
        }

        private void btStartDownload(object sender, RoutedEventArgs e)
        {
            _ucDownloadUI.btDownload.Click -= btStartDownload;
        }

        private void TsDownload_Toggled(object sender, RoutedEventArgs e)
        {
            if (!_ucDownloadUI.tsDownload.IsOn)
            {
                _ucDownloadUI.tsDownload.Toggled -= TsDownload_Toggled;
            }
        }

        private async void web_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            this.FindName("_ucDownloadUI");
            _ucDownloadUI.btDownload.Click += btStartDownload;
            _ucDownloadUI.tsDownload.Toggled += TsDownload_Toggled;
            if (_ucDownloadUI.tsDownload.IsOn)
            {
                ContentDialog cntExisitingDownload = new ContentDialog
                {
                    Content = "A File is Downloading Currently, let it finish and then try again",
                    Title = "Another file is Downloading",
                    PrimaryButtonText = "Ok",
                    SecondaryButtonText = "Check Download"
                };
                cntExisitingDownload.SecondaryButtonClick += (s, a) =>
                {
                    cntExisitingDownload.Hide();
                    _ucDownloadUI.Visibility = Visibility.Visible;
                };
                await cntExisitingDownload.ShowAsync();
                return;
            }
            else
            {
                _ucDownloadUI.txbDownloadLink.Text = args.Uri.ToString();
                _ucDownloadUI.txbFileName.Text = await UtilityClass.GetFileName(args.Uri);
                _ucDownloadUI.Visibility = Visibility.Visible;
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

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NoOfDevClick"))
            {
                string strNoOfClick = ApplicationData.Current.LocalSettings.Values["NoOfDevClick"].ToString();

                int noOfClick = Convert.ToInt32(strNoOfClick);
                if (noOfClick >= 5)
                {
                    this.FindName("btRefreshDarkMode");
                    btRefreshDarkMode.Visibility = Visibility.Visible;
                }
            }
            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
            tbTransparentTileIcon.Text = SecondaryTile.Exists("secInstagramId") ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = SecondaryTile.Exists("secInstagramId") ? "Unpin from Start" : "Pin to Start";
        }

        private void btforward_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoForward)
            {
                web.GoForward();
            }
        }
        private void style()
        {
            UtilityClass.ApplyStyle(web, styleDisplayNone);
            if (EnableDarkMode.IsOn)
            {
                UtilityClass.ApplyStyle(web, DesktopCss);
            }
            else
            {
                UtilityClass.ApplyStyle(web, lightModeCSS);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isFirstLoaded = false;
            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey("instaUserAgent"))
            //{
            //    if((int)ApplicationData.Current.LocalSettings.Values["instaUserAgent"] == 1)
            //    {
            //        UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1");
            //    }
            //}
            //LoadSaveData();
        }

        private void EnableDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            ApplicationData.Current.LocalSettings.Values["EnableDarkMode(I)"] = EnableDarkMode.IsOn;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#1e1e1e"));
                    style();
                }
                else
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#8D3EBB"));
                    web.Refresh();
                }
            }
            UtilityData.isInstagramDark = EnableDarkMode.IsOn;
            if (!isFirstLoaded)
                MainPage.ncSettings.setSettings = "Instagram";
           
            _ucCommandBar.cbAppbar.Background = MainPage.topSCBrush;
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
            style();
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

        private void web_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            style();
            if (args.Uri.AbsoluteUri.Contains("www.instagram.com/p/"))
            {
                if (gdDownloadPhotoOverlay == null)
                {
                    this.FindName("gdDownloadPhotoOverlay");
                    UtilityClass.TranslationAnimation(_compositor, gdDownloadPhotoOverlay, false);
                }
                gdDownloadPhotoOverlay.Visibility = Visibility.Visible;
            }
            else
            {
                if (gdDownloadPhotoOverlay != null) gdDownloadPhotoOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void web_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (prRing == null)
                return;
            prRing.Visibility = Visibility.Visible;
            if (_ucCommandBar.iconRotation.GetCurrentState() != Windows.UI.Xaml.Media.Animation.ClockState.Active)
                _ucCommandBar.iconRotation.Begin();
            style();
        }

        private void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)

        {
            if (args.Uri.AbsoluteUri.Contains("about:blank"))
                web.Navigate(new Uri("https://www.instagram.com/accounts/login/"));
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
            style();
        }

        private async void btPintoStart_Click(object sender, RoutedEventArgs e)
        {
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/instagram.png", "secInstagramId", "Instagram", "jplInstagram", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }

        private async void btRefreshDarkMode_Click(object sender, RoutedEventArgs e)
        {
            btRefreshDarkMode.IsEnabled = false;
            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/InstaMain", "Instagram", "InstaMain");
            if (_DesktopCss != "")
            {
                var result = _DesktopCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone = result[0];
                }
                DesktopCss = _DesktopCss;
                style();
                btRefreshDarkMode.IsEnabled = true;
            }
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
            txtInstaLink.Text = "";
            if (web.Source.AbsoluteUri.Contains("www.instagram.com/p/"))
            {
                txtInstaLink.Text = web.Source.AbsoluteUri;
            }

            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
        }



        private async Task saveInstaPhoto(string imgUri)
        {
            try
            {
                Uri source = new Uri(imgUri);

                FileSavePicker savePicker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.Downloads };
                if (imgUri.Contains(".mp4"))
                {
                    savePicker.FileTypeChoices.Add("MP4 Format", new List<string>() { ".mp4" });
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


        private async void TxtInstaLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                spDLMediaProgress.Visibility = Visibility.Visible;
                string imgUri = await UtilityClass.retrieveWebPhotoLink(txtInstaLink.Text, "Insta");
                if (imgUri != null)
                {
                    imgInstaPhoto.Source = null;
                    gdMediaContainer.Visibility = Visibility.Visible;

                    if (imgUri.Contains(".mp4"))
                    {
                        imgInstaPhoto.Visibility = Visibility.Collapsed;
                        
                        mdePlayer.Source = new Uri(imgUri);
                        mdePlayer.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        imgInstaPhoto.Visibility = Visibility.Visible;
                        mdePlayer.Visibility = Visibility.Collapsed;
                        imgInstaPhoto.Source = new BitmapImage(new Uri(imgUri));
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
            if (imgInstaPhoto.Visibility == Visibility.Visible)
            {
                if (imgInstaPhoto != null && imgInstaPhoto.Source != null)
                    await saveInstaPhoto(((BitmapImage)imgInstaPhoto.Source).UriSource.ToString());
            }
            else
            {
                if (mdePlayer != null && mdePlayer.Source != null)
                    await saveInstaPhoto(((Uri)mdePlayer.Source).ToString());
            }
        }

        private async void BtDownloadInstaPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            await saveInstaPhoto(await UtilityClass.retrieveWebPhotoLink(web.Source.AbsoluteUri, "Insta"));
        }

        private void BtRemoveDownloadPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            if (gdDownloadPhotoOverlay != null)
                gdDownloadPhotoOverlay.Visibility = Visibility.Collapsed;
        }

        private void BtGoDownloadPhoto_Click(object sender, RoutedEventArgs e)
        {
            gdLinkErrorDownload.Visibility = gdMediaContainer.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ChbGoToTop_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["isGoToTop(Insta)"] = chbGoToTop.IsChecked;
        }

        private async void btGotoTop_Click(object sender, RoutedEventArgs e)
        {
            string ScrollToTopString = @"window.scrollTo(0,0);";
            await web.InvokeScriptAsync("eval", new string[] { ScrollToTopString });
        }


        private async void BtGotoTop_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid uIElement = (Grid)((Button)sender).Content;
            try
            {
                await uIElement.Scale(scaleX: (float)1.25, scaleY: (float)1.25, centerX: 20, centerY: 20, duration: 400).StartAsync();

            }
            catch (Exception)
            {

            }
        }

        private async void BtGotoTop_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Grid uIElement = (Grid)((Button)sender).Content;
            try
            {
                await uIElement.Scale(scaleX: 1, scaleY: 1, centerX: 20, centerY: 20, duration: 400).StartAsync();

            }
            catch (Exception)
            {

            }
        }

        private void BtMoveXOffset_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            UtilityClass.moveYOffset((Button)sender, -5);
        }

        private void BtMoveXOffset_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            UtilityClass.moveYOffset((Button)sender, 0);
        }

        //private void cbUserAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ApplicationData.Current.LocalSettings.Values["instaUserAgentIndex"] = cbUserAgent.SelectedIndex;
        //    switch (cbUserAgent.SelectedIndex)
        //    {
        //        case 0:
        //            UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299");
        //            break;
        //        case 1:
        //            UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1");
        //            break;

        //    }
        //    web.Refresh();
        //}

        //private void Page_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    if (SystemInformation.DeviceFamily != "Windows.Mobile" && ApplicationData.Current.LocalSettings.Values.ContainsKey("instaUserAgentIndex"))
        //    {
        //        if (cbUserAgent.SelectedIndex == 1)
        //        {
        //            UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299");
        //        }
        //    }
        //}
    }
}
