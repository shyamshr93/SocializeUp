using App6.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Notifications;
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
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WhatsApp : Page
    {
        public int _NewConversationCount = 0;
        private string cssToApply = "";
        public static ncMainPage notifyChange { get; set; }
        private bool isFirstLoaded = false;
        private string styleDisplayNone = "";

        public WhatsApp()
        {
            this.InitializeComponent();
            notifyChange = new ncMainPage();
            notifyChange.PropertyChanged += NotifyChange_PropertyChanged;
            _ucCommandBar.btrefresh.Click += Refresh_Click;
            _ucCommandBar.btback.Click += Back_Click;
            _ucCommandBar.btforward.Click += btforward_Click;
            _ucCommandBar.btsetting.Click += btsetting_Click;
            _ucCommandBar.abtInk.Click += abtInk_Click;
            web.RegisterPropertyChangedCallback(WebView.DocumentTitleProperty, OnDocTitleChanged);
            chromewhatsapp();
            OneTimeSave();
            web.Source = new Uri("https://web.whatsapp.com");
            //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
        }


        private void NotifyChange_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (notifyChange.setSettings == "Home")
                chromewhatsapp();
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
            tbTransparentTileIcon.Text = SecondaryTile.Exists("secWhatsAppId") ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = SecondaryTile.Exists("secWhatsAppId") ? "Unpin from Start" : "Pin to Start";
        }

        private void chromewhatsapp()
        {
            //var rm = new HttpRequestMessage(HttpMethod.Get, new Uri("https://web.whatsapp.com"));
            //rm.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
            //web.NavigateWithHttpRequestMessage(rm);
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

        private async void OneTimeSave()
        {
            isFirstLoaded = true;
            EnableNotification.IsOn = (bool)UtilityClass.ApplicationDataBool("Notification(WhatsApp)", true);
            _ucCommandBar.btsetting.Visibility = Visibility.Visible;
            EnableDarkMode.IsOn = UtilityData.isWhatsAppDark;
            if (!EnableDarkMode.IsOn)
                spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#009688"));

            if (UtilityData.isFluentDesign)
                _ucCommandBar.cbAppbar.Style = Application.Current.Resources["CommandBarRevealStyle"] as Style;

            cssToApply = UtilityData.getCss("WhatsApp");
            style();
            var _cssToApply = await UtilityClass.serverStyle("http://rebrand.ly/oneWhatsApp", "WhatsApp", "DesktopMain");
            if (_cssToApply != "")
            {
                var result = _cssToApply.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone += result[0];
                }
                cssToApply = _cssToApply;
                style();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            prRing.Visibility = Visibility.Visible;
            web.Refresh();
            if (_ucErrorPage != null)
                _ucErrorPage.Visibility = Visibility.Collapsed;
        }

        private void dismisstile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (web.CanGoBack)
            {
                web.GoBack();
            }
        }

        private void OnDocTitleChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (EnableNotification.IsOn)
            {
                if (web.DocumentTitle.Trim().Length > 0)
                {
                    int conversationCountFromTitle = UtilityClass.ConvertToInt(web.DocumentTitle);
                    if (_NewConversationCount != conversationCountFromTitle)
                    {
                        System.Diagnostics.Debug.WriteLine("New conversation count: " + web.DocumentTitle);
                        if (_NewConversationCount == 0)
                        {
                            UtilityClass.Notify("You have new messages from WhatsApp.", "whatsapp.png", "WhatsApp");
                        }
                        _NewConversationCount = conversationCountFromTitle;
                    }
                }
            }
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
            prRing.Visibility = Visibility.Collapsed;
            style();
        }

        private void style()
        {
            if (EnableDarkMode.IsOn)
            {
                UtilityClass.ApplyStyle(web, cssToApply);
            }
            UtilityClass.ApplyStyle(web, styleDisplayNone);
        }

        private void web_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
           // args.Handled = true;
           // UtilityClass.OpenExternalLink(sender, args.Uri);
        }

        private void web_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            IAsyncOperation<bool> b = Windows.System.Launcher.LaunchUriAsync(args.Uri);
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
                web.Navigate(new Uri("http://web.whatsapp.com"));
            if (!args.IsSuccess)
            {
                if (_ucErrorPage == null)
                {
                    this.FindName("_ucErrorPage");
                    _ucErrorPage.btErrorRefresh.Click += Refresh_Click;
                }
                prRing.Visibility = Visibility.Collapsed;
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

        private void EnableDarkMode_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            ApplicationData.Current.LocalSettings.Values["EnableDarkMode(WhatsApp)"] = EnableDarkMode.IsOn;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#171717"));
                    style();

                }
                else
                {
                    spContentSplitView.Background = new SolidColorBrush(UtilityClass.ConvertStringToColor("#009688"));
                    web.Refresh();
                }
            }
            UtilityData.isWhatsAppDark = EnableDarkMode.IsOn;
            if (!isFirstLoaded)
                MainPage.ncSettings.setSettings = "WhatsApp";
            
            _ucCommandBar.cbAppbar.Background = MainPage.topSCBrush;
        }

        private void EnableNotification_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Notification(WhatsApp)"] = EnableNotification.IsOn;
        }

        private void web_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            args.PermissionRequest.Allow();
        }

        private async void btPintoStart_Click(object sender, RoutedEventArgs e)
        {
            bool isPinned = await UtilityClass.PinSecondaryTile("Assets/whatsapp.png", "secWhatsAppId", "WhatsApp", "jplWhatsApp", ucCommandBar.scbColor.Color, (FrameworkElement)sender);

            tbTransparentTileIcon.Text = isPinned ? "\uE77A" : "\uE840";
            tbTransparentTile.Text = isPinned ? "Unpin from Start" : "Pin to Start";
        }

        private void web_UnsupportedUriSchemeIdentified(WebView sender, WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {

        }

        private void web_UnsafeContentWarningDisplaying(WebView sender, object args)
        {

        }

        private async void btRefreshDarkMode_Click(object sender, RoutedEventArgs e)
        {
            btRefreshDarkMode.IsEnabled = false;
            var _cssToApply = await UtilityClass.serverStyle("http://rebrand.ly/oneWhatsApp", "WhatsApp", "DesktopMain");
            if (_cssToApply != "")
            {
                var result = _cssToApply.Split(new[] { '\r', '\n' });
                if (result != null && result[0] != null)
                {
                    styleDisplayNone += result[0];
                }
                cssToApply = _cssToApply;
                style();
            }
            btRefreshDarkMode.IsEnabled = true;
        }
    }
}
