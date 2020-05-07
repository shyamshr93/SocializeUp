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
using App6.Models;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;
using Microsoft.Toolkit.Uwp.UI.Animations;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        private UtilityClass Utility = new UtilityClass();
        public static ncMainPage notifyChange { get; set; }
        string DesktopCss = "", MobileCss = "";
        private bool isFirstLoaded = false;
        private DispatcherTimer darkModeTimer;
        private string styleDisplayNone = "";

        public BlankPage1()
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
            if (notifyChange.setSettings == "Home" && cbLiteItems.Visibility == Visibility.Visible)
                cbLiteItemsSelection();
            else
                web.Navigate(new Uri("https://m.facebook.com"));
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

        private void CompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            _ucCommandBar.Visibility = Visibility.Visible;
            MainPage.ncSettings.setSettings = "HideCompactOverlay";
        }

        private void btCompactOverlay_Click(object sender, RoutedEventArgs e)
        {
            switch (cbLiteItems.SelectedIndex)
            {
                case 0:
                    btHideCompactOverlay.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case 1:
                    btHideCompactOverlay.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
            }
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
            cbLiteItems.Visibility = _ucCommandBar.btforward.Visibility;

            chbGoToTop.IsChecked = (bool)UtilityClass.ApplicationDataBool("isGoToTop(FB)", false);
            if (cbLiteItems.Visibility == Visibility.Visible)
            {
                cbLiteItems.SelectedIndex = (int)UtilityClass.ApplicationDataBool("(FB)cbLiteItems", 0);
                cbLiteItemsSelection();
            }
            else
            {
                web.Source = new Uri("https://m.facebook.com");
            }

            EnableDarkMode.IsOn = UtilityData.isFacebookDark;
            if (!EnableDarkMode.IsOn)
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#4267B2"));

            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            DesktopCss = UtilityData.getCss("DFb");
            MobileCss = UtilityData.getCss("MFb");
            style();

            StartDarkModeDispatcher();

            DownloadMobileCss();

            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/fbDesktopMain", "Facebook", "DesktopMain");
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


            //if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TotalUsage(FB)"))
            //{
            //    TotalUsageTime = (int)ApplicationData.Current.LocalSettings.Values["TotalUsage(FB)"];
            //}
            //UsageTimerStart();
        }


        private void StartDarkModeDispatcher()
        {
            if (EnableDarkMode.IsOn)
            {
                darkModeTimer = new DispatcherTimer();
                darkModeTimer.Tick += (s, a) =>
                {
                    style();
                };
                darkModeTimer.Interval = new TimeSpan(0, 0, 7);
            }
        }


        //private void UsageTimerStart()
        //{
        //    dispatcherTimer = new DispatcherTimer();
        //    dispatcherTimer.Tick += dispatcherTimer_Tick;
        //    dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        //    dispatcherTimer.Start();
        //}

        //int TotalUsageTime = 1;
        //private DispatcherTimer dispatcherTimer;
        //int CurrentUsageTime = 1;
        //bool isReminded = false;

        //private async void dispatcherTimer_Tick(object sender, object e)
        //{
        //    TotalUsageTime++;
        //    CurrentUsageTime++;
        //    ApplicationData.Current.LocalSettings.Values["TotalUsage(FB)"] = TotalUsageTime;

        //    if (gdRectVisibility != null && ContentSplitView.IsPaneOpen)
        //    {
        //        string strUsageName = "Current Usage: ";
        //        if (CurrentUsageTime > 60)
        //        {
        //            tbCurrentUsage.Text = strUsageName + (CurrentUsageTime / 60).ToString() + " mins " + (CurrentUsageTime % 60) + " secs";
        //        }
        //        else
        //        {
        //            tbCurrentUsage.Text = strUsageName + CurrentUsageTime.ToString() + " Seconds";
        //        }
        //    }

        //    //if (isReminded && CurrentUsageTime > 200)
        //    //{
        //    //    isReminded = false;
        //    //    await new MessageDialog("Time's up").ShowAsync();
        //    //    isReminded = false;
        //    //}

        //}

        private async void DownloadMobileCss()
        {
            var _MobileCss = await UtilityClass.serverStyle("https://rebrand.ly/fbMobileMain", "Facebook", "MobileMain");
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

        private void LoadSettings()
        {
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
            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
            tbTransparentTileIcon.Text = SecondaryTile.Exists("secFBId") ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = SecondaryTile.Exists("secFBId") ? "Unpin from Start" : "Pin to Start";

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
            //pbTotalTime.Value = TotalUsageTime / 3600;

            //if (CurrentUsageTime > 60)
            //{
            //    tbCurrentUsage.Text = "Current Usage: " + (CurrentUsageTime / 60).ToString() + " mins " + (CurrentUsageTime % 60) + " secs";
            //}
            //else
            //{
            //    tbCurrentUsage.Text = "Current Usage: " + CurrentUsageTime.ToString() + " Seconds";
            //}

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
            LoadSettings();
            if (EnableDarkMode.IsOn && darkModeTimer != null) darkModeTimer.Start();
        }

        private void EnableDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            ApplicationData.Current.LocalSettings.Values["EnableDarkMode(F)"] = EnableDarkMode.IsOn;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#171717"));
                    style();
                }
                else
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#4267B2"));
                    web.Refresh();
                }
            }
            UtilityData.isFacebookDark = EnableDarkMode.IsOn;
            if (!isFirstLoaded || ucCommandBar.scbColor == null)
                MainPage.ncSettings.setSettings = "Facebook";

            _ucCommandBar.Background = ucCommandBar.scbColor;
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

        private void style()
        {
            string displaynone = ".loggedout_menubar_container, #pageFooter, #pagelet_ego_pane, ._26z1 { display: none; }";
            if (web.Source.AbsoluteUri.Contains("m.facebook.com"))
            {
                displaynone += "#header {position: fixed; width:100%; z-index: 12; top: 0px;} #root {padding-top: 44px;} .item.more {position:fixed; bottom: 0px; text-align: center !important;} .flyout {max-height:15px; overflow-y:scroll;}";
                displaynone += "._43mg, ._5t3b, .touch ._qw9 {display: none;}";
}
            styleDisplayNone += displaynone;
            UtilityClass.ApplyStyle(web, styleDisplayNone);

            if (EnableDarkMode.IsOn)
            {
                if (web.Source.AbsoluteUri.Contains("m.facebook.com"))
                {
                    UtilityClass.ApplyStyle(web, MobileCss);
                }
                else if (web.Source.AbsoluteUri.Contains("www.facebook.com/login"))
                {
                    string cssToApply = "";
                    cssToApply += ".loggedout_menubar_container, #pageFooter { display: none; }";
                    cssToApply += "._53jh {background: #292929}";
                    cssToApply += "body, ._4-u8, ._4-u5 {background: #171717}";
                    cssToApply += "._4-u2,._53jh {border-color: #171717;}";
                    cssToApply += "body {color : white}";
                    UtilityClass.ApplyStyle(web, cssToApply);
                }
                else
                {
                    UtilityClass.ApplyStyle(web, DesktopCss);
                }
            }
        }

        private void web_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            args.StopPageScriptExecution = true;
        }

        private void web_LoadCompleted(object sender, NavigationEventArgs e)
        {
            _ucCommandBar.btforward.IsEnabled = web.CanGoForward;
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

        private void cbLiteItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["(FB)cbLiteItems"] = cbLiteItems.SelectedIndex;
            cbLiteItemsSelection();
        }

        private void cbLiteItemsSelection()
        {
            if (cbLiteItems != null)
            {
                switch (cbLiteItems.SelectedIndex)
                {
                    case 0:
                        web.Navigate(new Uri("https://www.facebook.com/login"));
                        break;
                    case 1:
                        web.Navigate(new Uri("https://m.facebook.com/"));
                        break;
                }
            }
        }

        private void web_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.Uri.AbsoluteUri.Contains("about:blank"))
                web.Navigate(new Uri("https://facebook.com/login"));

            if (args.Uri.AbsoluteUri.Contains("https://www.facebook.com/?stype=lo"))
            {
                web.Navigate(new Uri("https://facebook.com/login"));
            }

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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (darkModeTimer != null) darkModeTimer.Stop();
        }

        private async void btRefreshDarkMode_Click(object sender, RoutedEventArgs e)
        {
            btRefreshDarkMode.IsEnabled = false;
            DownloadMobileCss();

            var _DesktopCss = await UtilityClass.serverStyle("https://rebrand.ly/fbDesktopMain", "Facebook", "DesktopMain");
            if (_DesktopCss != "")
            {
                var result = _DesktopCss.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone += result[0];
                }
                DesktopCss = _DesktopCss;
                style();
                btRefreshDarkMode.IsEnabled = true;
            }
        }

        private async void btPintoStart_Click(object sender, RoutedEventArgs e)
        {
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/fb.png", "secFBId", "Facebook", "jplFacebook", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }
        
        private void ChbGoToTop_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["isGoToTop(FB)"] = chbGoToTop.IsChecked;
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
    }
}
