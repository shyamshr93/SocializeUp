using App6.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
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
    public sealed partial class BlankPage10 : Page
    {
        string TeleCss = "";
        private string cssToApply = "";
        public static ncMainPage notifyChange { get; set; }
        private bool isFirstLoaded = false;
        public BlankPage10()
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
        }

        private void NotifyChange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (notifyChange.setSettings == "Home")
                web.Navigate(new Uri("https://web.telegram.org"));
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
            // EnableNotification.IsOn = (bool)Utility.ApplicationDataBool("Notification(Telegram)", true);
            _ucCommandBar.btsetting.Visibility = Visibility.Visible;
            EnableDarkMode.IsOn = UtilityData.isTelegramDark;
            if (!EnableDarkMode.IsOn)
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#5682A3"));

            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            TeleCss = UtilityData.getCss("Tele");
            style();
            var _TeleCss = await UtilityClass.serverStyle("https://rebrand.ly/TeleMain", "Telegram", "TeleMain");
            if (_TeleCss != "")
            {
                TeleCss = _TeleCss;
                style();
            }
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


        private void style()
        {
            cssToApply = "";
            if (EnableDarkMode.IsOn && web.Source.AbsoluteUri.Contains("https://web.telegram.org/#/login"))
            {
                // background-color #292929
                //cssToApply += "html {background: #1e1e1e}";
                cssToApply += ".login_head_bg {background: #292929}";
                cssToApply += ".login_footer_wrap {display: none}";
            }
            else if (web.Source.AbsoluteUri.Contains("https://web.telegram.org/#/login"))
            {
                cssToApply += ".login_footer_wrap {display: none}";
            }
            if (EnableDarkMode.IsOn && web.Source.AbsoluteUri.Contains("https://web.telegram.org/#/im"))
            {
                cssToApply += TeleCss;
            }
            if (web.Source.AbsoluteUri.Contains("https://web.telegram.org/"))
            {
                cssToApply += ".tg_head_split, .im_page_wrap {max-width: none}";
            }
            UtilityClass.ApplyStyle(web, cssToApply);
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
            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
            tbTransparentTileIcon.Text = SecondaryTile.Exists("secTelegramId") ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = SecondaryTile.Exists("secTelegramId") ? "Unpin from Start" : "Pin to Start";
        }

        private void EnableDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["EnableDarkMode(Telegram)"] = EnableDarkMode.IsOn;
            if (EnableDarkMode.IsOn == true)
            {
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#171717"));
                style();
            }
            else
            {
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#5682A3"));
                web.Refresh();
            }
            UtilityData.isTelegramDark = EnableDarkMode.IsOn;
            if (!isFirstLoaded)
                MainPage.ncSettings.setSettings = "Telegram";

            _ucCommandBar.cbAppbar.Background = MainPage.topSCBrush;
        }

        private void btforward_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoForward)
            {
                web.GoForward();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isFirstLoaded = false;
            //LoadSaveData();
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
                web.Navigate(new Uri("https://web.telegram.org"));
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
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/tele.png", "secTelegramId", "Telegram", "jplTelegram", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }
    }
}
