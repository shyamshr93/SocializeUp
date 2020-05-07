using App6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.Storage.Pickers;
using Windows.Networking.BackgroundTransfer;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage2 : Page
    {
        string MobileCss = "";
        string DesktopCss = "";
        public static ncMainPage notifyChange { get; set; }
        private bool isFirstLoaded = false;
        private string styleDisplayNone = "";
        private Compositor _compositor;

        public BlankPage2()
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

        private void NotifyChange_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (notifyChange.setSettings == "Home" && cbLiteItems.Visibility == Visibility.Visible)
                cbLiteItemsSelection();
            else
                web.Navigate(new Uri("https://twitter.com"));
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
            cbLiteItems.Visibility = _ucCommandBar.btforward.Visibility;
            if (cbLiteItems.Visibility == Visibility.Visible)
            {
                cbLiteItems.SelectedIndex = (int)UtilityClass.ApplicationDataBool("(Twit)cbLiteItems", 1);
                cbLiteItemsSelection();
            }
            else
            {
                web.Source = new Uri("https://twitter.com/login");
            }

            chbGoToTop.IsChecked = (bool)UtilityClass.ApplicationDataBool("isGoToTop(Twit)", false);

            EnableDarkMode.IsOn = UtilityData.isTwitterDark;
            if (!EnableDarkMode.IsOn)
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#138BDE"));

            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            DesktopCss = UtilityData.getCss("DTwit");
            MobileCss = UtilityData.getCss("MTwit");
            style();
            DownloadMobileCss();
            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/TwitDesktopMain", "Twitter", "DesktopMain");
            if (_DesktopCss != "")
            {
                var result = _DesktopCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone += result[0];
                }
                DesktopCss = _DesktopCss;
                style();
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

        private void btCompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            if (cbLiteItems.SelectedIndex == 0)
            {
                btHideCompactOverlay.Margin = new Thickness(150, 0, 0, 0);
                btHideCompactOverlay.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                btHideCompactOverlay.Margin = new Thickness(5);
                btHideCompactOverlay.VerticalAlignment = VerticalAlignment.Bottom;
            }
            _ucCommandBar.Visibility = Visibility.Collapsed;
            MainPage.ncSettings.setSettings = "ShowCompactOverlay";
        }

        private void CompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            _ucCommandBar.Visibility = Visibility.Visible;
            MainPage.ncSettings.setSettings = "HideCompactOverlay";

        }

        private async void DownloadMobileCss()
        {
            var _MobileCss = await UtilityClass.serverStyle("https://rebrand.ly/TwitMobileMain", "Twitter", "MobileMain");
            if (_MobileCss != "")
            {
                var result = _MobileCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone = result[0];
                }
                MobileCss = _MobileCss;
                style();
            }
        }

        private void LoadSaveData()
        {
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            prRing.Visibility = Visibility.Visible;
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
            tbTransparentTileIcon.Text = SecondaryTile.Exists("secTwitterId") ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = SecondaryTile.Exists("secTwitterId") ? "Unpin from Start" : "Pin to Start";
        }

        private void btforward_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoForward)
            {
                web.GoForward();
            }
        }

        private void EnableTweetDeck_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            //ApplicationData.Current.LocalSettings.Values["Tweetdeck"] = EnableTweetDeck.IsOn;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    EnableDarkMode.IsOn = false;
                    web.Source = new Uri("https://tweetdeck.twitter.com");
                }
                else
                {
                    web.Source = new Uri("https://twitter.com/");
                }
            }
        }

        private void mbTwitter_Click(object sender, RoutedEventArgs e)
        {
            Uri targeturi = new Uri("http://mobile.twitter.com");

            web.Navigate(targeturi);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isFirstLoaded = false;
            LoadSaveData();
        }

        private async void EnableDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            ApplicationData.Current.LocalSettings.Values["EnableDarkMode(Twitter)"] = EnableDarkMode.IsOn;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#171717"));
                    web.DefaultBackgroundColor = (spContentSplitView.Background as SolidColorBrush).Color;
                    style();
                }
                else
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#138BDE"));
                    web.DefaultBackgroundColor = Colors.White;
                    web.Refresh();
                }
                UtilityData.isTwitterDark = EnableDarkMode.IsOn;
                if (!isFirstLoaded)
                    MainPage.ncSettings.setSettings = "Twitter";
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                       () =>
                       {
                           _ucCommandBar.Background = ucCommandBar.scbColor;
                       });
                //if (UtilityData.isFluentDesign)
                //{
                //    ContentSplitView.PaneBackground = new AcrylicBrush()
                //    {
                //        FallbackColor = ((SolidColorBrush)ContentSplitView.PaneBackground).Color,
                //        TintColor = ((SolidColorBrush)ContentSplitView.PaneBackground).Color,
                //        TintOpacity = 0.6,
                //        BackgroundSource = AcrylicBackgroundSource.Backdrop
                //    };
                //}
                //else
                //{
                //    _ucCommandBar.cbAppbar.Background = MainPage.topSCBrush;
                //}
                _ucCommandBar.cbAppbar.Background = MainPage.topSCBrush;
            }
        }

        private void style()
        {
            if (EnableDarkMode.IsOn && cbLiteItems.SelectedIndex != 2)
            {
                if (web.Source.ToString().StartsWith("https://mobile.twitter.com/"))
                {
                    UtilityClass.ApplyStyle(web, MobileCss);
                }
                else
                {
                    //DesktopCss += ".tweet js-stream-tweet js-actionable-tweet js-profile-popup-actionable original-tweet js-original-tweet promoted-tweet pixel-promoted-tweet has-cards has-content presented scribed {display: none;}";
                    UtilityClass.ApplyStyle(web, DesktopCss);
                }
            }
            else
            {
                String displaynone = "";
                switch (cbLiteItems.SelectedIndex)
                {
                    case 0:
                        displaynone = ".Footer .Footer-adsModule, .login-responsive .mobile, .global-nav--newLoggedOut #global-actions>li>a  {display: none;}";
                        UtilityClass.ApplyStyle(web, displaynone);
                        break;
                    case 1:
                        UtilityClass.ApplyStyle(web, displaynone);
                        break;

                }
                UtilityClass.ApplyStyle(web, styleDisplayNone);
            }
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

        private void web_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            style();
            if (args.Uri.AbsoluteUri.Contains("mobile.twitter") && args.Uri.AbsoluteUri.Contains("photo"))
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
            if (args.Uri.AbsoluteUri.Contains("instagram.com/") && args.Uri.Host.Contains("instagram.com"))
            {
                UtilityData.ShareURL = args.Uri;
                MainPage.ncSettings.setSettings = "InstaURL";
                web.GoBack();
                args.Cancel = true;
            }
            prRing.Visibility = Visibility.Visible;
            if (_ucCommandBar.iconRotation.GetCurrentState() != Windows.UI.Xaml.Media.Animation.ClockState.Active)
                _ucCommandBar.iconRotation.Begin();
            style();
        }

        private void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("about:blank"))
                web.Navigate(new Uri("https://twitter.com/login"));
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

        private void cbLiteItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["(Twit)cbLiteItems"] = cbLiteItems.SelectedIndex;
            cbLiteItemsSelection();
        }

        private void cbLiteItemsSelection()
        {
            if (cbLiteItems != null)
            {
                switch (cbLiteItems.SelectedIndex)
                {
                    case 0:
                        web.Navigate(new Uri("https://www.twitter.com/"));
                        break;
                    case 1:
                        web.Navigate(new Uri("https://mobile.twitter.com/"));
                        break;
                    case 2:
                        web.Navigate(new Uri("https://tweetdeck.twitter.com"));
                        break;
                }
            }
        }

        private async void btGotoTop_Click(object sender, RoutedEventArgs e)
        {
            string ScrollToTopString = @"window.scrollTo(0,0);";
            await web.InvokeScriptAsync("eval", new string[] { ScrollToTopString });
        }

        private async void btPintoStart_Click(object sender, RoutedEventArgs e)
        {
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/twitter.png", "secTwitterId", "Twitter", "jplTwitter", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }

        private async void btRefreshDarkMode_Click(object sender, RoutedEventArgs e)
        {
            btRefreshDarkMode.IsEnabled = false;
            DownloadMobileCss();
            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/TwitDesktopMain", "Twitter", "DesktopMain");
            if (_DesktopCss != "")
            {
                var result = _DesktopCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone += result[0];
                }
                DesktopCss = _DesktopCss;
                style();
            }
            btRefreshDarkMode.IsEnabled = true;
        }

        private async Task saveTwitPhoto(string imgUri)
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

        private async void BtDownloadPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            await saveTwitPhoto(await UtilityClass.retrieveWebPhotoLink(web.Source.AbsoluteUri, "Twitter"));
        }

        private void BtRemoveDownloadPhotoOverlay_Click(object sender, RoutedEventArgs e)
        {
            if (gdDownloadPhotoOverlay != null)
                gdDownloadPhotoOverlay.Visibility = Visibility.Collapsed;
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

            txtTwitPostLink.Text = "";
            if (web.Source.AbsoluteUri.Contains("status"))
            {
                txtTwitPostLink.Text = web.Source.AbsoluteUri;
            }

            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
        }


        private void BtCancelDownloadUI_Click(object sender, RoutedEventArgs e)
        {
            if (gdImageDownloadContainer != null)
            {
                gdImageDownloadContainer.Visibility = Visibility.Collapsed;
                gdMediaContainer.Visibility = Visibility.Collapsed;
            }

        }

        private async void BtDownloadPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (imgTwitPhoto.Visibility == Visibility.Visible)
            {
                if (imgTwitPhoto != null && imgTwitPhoto.Source != null)
                    await saveTwitPhoto(((BitmapImage)imgTwitPhoto.Source).UriSource.ToString());
            }
            else
            {
                if (mdePlayer != null && mdePlayer.Source != null)
                    await saveTwitPhoto(((Uri)mdePlayer.Source).ToString());
            }
        }

        private async void TxtTwitPostLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                spDLMediaProgress.Visibility = Visibility.Visible;
                string imgUri = await UtilityClass.retrieveWebPhotoLink(txtTwitPostLink.Text, "Twitter");
                if (imgUri != null)
                {
                    imgTwitPhoto.Source = null;
                    gdMediaContainer.Visibility = Visibility.Visible;

                    if (imgUri.Contains(".mp4"))
                    {
                        imgTwitPhoto.Visibility = Visibility.Collapsed;

                        mdePlayer.Source = new Uri(imgUri);
                        mdePlayer.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        imgTwitPhoto.Visibility = Visibility.Visible;
                        mdePlayer.Visibility = Visibility.Collapsed;
                        imgTwitPhoto.Source = new BitmapImage(new Uri(imgUri));
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

        private void ChbGoToTop_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["isGoToTop(Twit)"] = chbGoToTop.IsChecked;
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
    }
}
