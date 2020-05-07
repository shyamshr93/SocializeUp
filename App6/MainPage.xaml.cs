using App6;
using App6.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Security.Credentials.UI;
using Windows.Services.Store;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
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
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App6

{
    public sealed partial class MainPage : Page
    {
        public static ncMainPage ncSettings { get; set; }
        public static Brush topAcrylicBrush;
        public static Brush HamburgerAcrylicBrush;
        public static SolidColorBrush topSCBrush;
        public static bool isHideCompactMenu = false;
        private bool isMenuPinned = false;
        private StoreContext storeContext;

        public MainPage()
        {
            this.InitializeComponent();

            ncSettings = new ncMainPage();
            ncSettings.PropertyChanged += NcSettings_PropertyChanged;

            OneTimeSave();

            Changelog();
        }

        private void NcSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (ncSettings.setSettings)
            {
                case "Facebook":
                    if (UtilityData.isFacebookDark) ChangeColor("#1a1a1a"); else ChangeColor(UtilityData.FacebookColor);
                    break;
                case "Twitter":
                    if (UtilityData.isTwitterDark) ChangeColor("#1a1a1a"); else ChangeColor(UtilityData.TwitterColor);
                    break;
                case "Instagram":
                    if (UtilityData.isInstagramDark) ChangeColor("#1a1a1a"); else ChangeColor(UtilityData.InstagramColor);
                    break;
                case "Telegram":
                    if (UtilityData.isTelegramDark) ChangeColor("#1a1a1a"); else ChangeColor(UtilityData.TelegramColor);
                    break;
                case "WhatsApp":
                    if (UtilityData.isWhatsAppDark) ChangeColor("#1a1a1a"); else ChangeColor(UtilityData.WhatsAppColor);
                    break;
                case "ShowCompactOverlay":
                    ShowCompactView(true);
                    break;
                case "HideCompactOverlay":
                    ShowCompactView(false);
                    break;
                case "InstaURL":
                    mylistbox.SelectedIndex = 5;
                    UtilityData.ShareURL = null;
                    break;
                case "PinToStart":
                    FadedNotify((bool)ncSettings.objValue ? "Sucessfully Pinned tile to Start" : "Sucessfully unpinned tile from Start.");
                    break;
                case "msgNotify":
                    FadedNotify(ncSettings.objValue.ToString());
                    break;
            }
            ucCommandBar.ncColor.strColor = "Color";
        }

        private async void Changelog()
        {
            StorageFile recentFile = null;

            StorageFile Assetfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/Changelog.txt"));
            try
            {
                recentFile = await ApplicationData.Current.LocalFolder.GetFileAsync(UtilityData.strChangelog);
            }
            catch (FileNotFoundException)
            {
                await ApplicationData.Current.LocalFolder.CreateFileAsync(UtilityData.strChangelog, CreationCollisionOption.ReplaceExisting);
                recentFile = await ApplicationData.Current.LocalFolder.GetFileAsync(UtilityData.strChangelog);
                await Assetfile.CopyAndReplaceAsync(recentFile);

                UtilityData.newUpdate = true;

                this.FindName("_ucChangelog");
                _ucChangelog.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {

            }
            if (recentFile != null)
            {
                string[] changelog = File.ReadAllLines(recentFile.Path);
                if (changelog[0] != UtilityData.VersionNumber)
                {
                    await Assetfile.CopyAndReplaceAsync(recentFile);

                    UtilityData.newUpdate = true;

                    this.FindName("_ucChangelog");
                    _ucChangelog.Visibility = Visibility.Visible;
                }
            }
            CreateJumplist();
        }

        private void OneTimeSave()
        {
            try
            {
                storeContext = StoreContext.GetDefault();
            }
            catch (Exception)
            {

            }

            isMenuPinned = (bool)UtilityClass.ApplicationDataBool("isMenuPinned", false);

            if (SystemInformation.DeviceFamily == "Windows.Mobile")
                btPinSplitView.Visibility = Visibility.Collapsed;
            //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.167 Safari/537.36");

            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
            {
                UtilityData.isFluentDesign = (bool)UtilityClass.ApplicationDataBool("isFluentDesign", true);
            }
            else
            {
                UtilityData.isFluentDesign = false;
            }
            //UtilityData.isFluentDesign = false;
            try
            {
                if (UtilityData.isFluentDesign)
                {
                    topAcrylicBrush = new AcrylicBrush()
                    {
                        FallbackColor = ((SolidColorBrush)rpWeb.Background).Color,
                        TintColor = ((SolidColorBrush)rpWeb.Background).Color,
                        TintOpacity = 0.7,
                        BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                    };
                    HamburgerAcrylicBrush = new AcrylicBrush()
                    {
                        FallbackColor = ((SolidColorBrush)mySplitsview.PaneBackground).Color,
                        TintColor = ((SolidColorBrush)mySplitsview.PaneBackground).Color,
                        TintOpacity = 0.7,
                        BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                    };
                    // Fluent Button Style
                    bthamburger.Style = Application.Current.Resources["ButtonRevealStyle"] as Style;
                }
            }
            catch (Exception)
            {
                UtilityData.isFluentDesign = false;
            }
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Core.CoreApplicationViewTitleBar") && Microsoft.Toolkit.Uwp.Helpers.SystemInformation.DeviceFamily == "Windows.Desktop")
            {
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.LayoutMetricsChanged += (s, a) =>
                {
                    gdTitleBar.Margin = new Thickness(coreTitleBar.SystemOverlayLeftInset, 0, 0, 0);
                };
                coreTitleBar.IsVisibleChanged += (s, a) => { gdTitleBar.Visibility = coreTitleBar.IsVisible ? Visibility.Visible : Visibility.Collapsed; };
            }

            SavedDarkMode();

            PinnedMenuHelper(isMenuPinned);
        }

        private void SavedDarkMode()
        {
            UtilityData.isFacebookDark = (bool)UtilityClass.ApplicationDataBool("EnableDarkMode(F)", true);
            UtilityData.isTwitterDark = (bool)UtilityClass.ApplicationDataBool("EnableDarkMode(Twitter)", true);
            UtilityData.isInstagramDark = (bool)UtilityClass.ApplicationDataBool("EnableDarkMode(I)", true);
            UtilityData.isTelegramDark = (bool)UtilityClass.ApplicationDataBool("EnableDarkMode(Telegram)", true);
            UtilityData.isWhatsAppDark = (bool)UtilityClass.ApplicationDataBool("EnableDarkMode(WhatsApp)", true);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Core.CoreApplicationViewTitleBar") && Microsoft.Toolkit.Uwp.Helpers.SystemInformation.DeviceFamily == "Windows.Desktop")
            {
                gdTitleBar.Visibility = Visibility.Visible;
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = true;
                Window.Current.SetTitleBar(gdTitleBar);
            }
            else
            {
                gdTitleBar.Visibility = Visibility.Collapsed;
            }

            UtilityData.LinksSetting = (int)UtilityClass.ApplicationDataBool("cbExtLinks", 3);
            isHideCompactMenu = (bool)UtilityClass.ApplicationDataBool("HideCompactMenu", false);

            if (!isMenuPinned)
            {
                mySplitsview.IsPaneOpen = true;
                mySplitsview.IsPaneOpen = false;
            }

            HideCompactMenu();

            HideListView();

            InAppPurchaseLicense();
            try
            {
                if (e.Parameter == null || (string)e.Parameter == "")
                {
                    if (mylistbox.SelectedIndex == -1)
                        pageselection();
                }
                else
                {
                    switch ((string)e.Parameter)
                    {
                        case "jplFacebook":
                            mylistbox.SelectedIndex = 5;
                            break;
                        case "jplTwitter":
                            mylistbox.SelectedIndex = 0;
                            break;
                        case "jplTelegram":
                            mylistbox.SelectedIndex = 2;
                            break;
                        //case "jplWhatsApp":
                        //    mylistbox.SelectedIndex = 3;
                        //    break;
                        case "jplReddit":
                            mylistbox.SelectedIndex = 4;
                            break;
                        case "jplInstagram":
                            mylistbox.SelectedIndex = 1;
                            break;
                        case "jplSplitView":
                            mylistbox.SelectedIndex = 7;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                if (mylistbox.SelectedIndex == -1)
                    pageselection();
            }
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                UtilityData.isbtHardwareBack = (bool)UtilityClass.ApplicationDataBool("bthardwareBack", false);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!isMenuPinned)
                mySplitsview.IsPaneOpen = false;
        }

        private void pageselection()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("cbPageSelection"))
            {
                switch ((int)ApplicationData.Current.LocalSettings.Values["cbPageSelection"])
                {
                    case -1:
                        mylistbox.SelectedIndex = 0;
                        break;
                    case 0:
                        mylistbox.SelectedIndex = 0;
                        break;
                    case 1:
                        mylistbox.SelectedIndex = 1;
                        break;
                    case 2:
                        mylistbox.SelectedIndex = 3;
                        break;
                    case 3:
                        mylistbox.SelectedIndex = 4;
                        break;
                    case 4:
                        mylistbox.SelectedIndex = 5;
                        break;
                    case 5:
                        mylistbox.SelectedIndex = 2;
                        break;
                    case 6:
                        mylistbox.SelectedIndex = 6;
                        break;
                    case 7:
                        mylistbox.SelectedIndex = 7;
                        break;
                }
            }
            else
            {
                mylistbox.SelectedIndex = 0;
            }
        }

        private void HideListView()
        {
            btFaceBook.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(F)", false) ? Visibility.Collapsed : Visibility.Visible;
            btTwitter.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(Tw)", false) ? Visibility.Collapsed : Visibility.Visible;
            btInstagram.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(I)", false) ? Visibility.Collapsed : Visibility.Visible;
            btReddit.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(R)", false) ? Visibility.Collapsed : Visibility.Visible;
            //btGoogle.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(G)", false) ? Visibility.Collapsed : Visibility.Visible;

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Mix)"))
            {
                if ((bool)ApplicationData.Current.LocalSettings.Values["Hide(Mix)"] == true)
                {
                    btMix.Visibility = Visibility.Collapsed;
                }
                else if ((bool)ApplicationData.Current.LocalSettings.Values["Hide(Mix)"] == false)
                {
                    btMix.Visibility = Visibility.Visible;
                }
            }

            if (UtilityData.isFirstAppRun)
            {
                ApplicationData.Current.LocalSettings.Values["Hide(Wh)"] = true;
                btWhatsApp.Visibility = Visibility.Collapsed;
                UtilityData.isFirstAppRun = false;
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Hide(Wh)"))
                {
                    if ((bool)ApplicationData.Current.LocalSettings.Values["Hide(Wh)"] == true)
                    {
                        btWhatsApp.Visibility = Visibility.Collapsed;
                    }
                    else if ((bool)ApplicationData.Current.LocalSettings.Values["Hide(Wh)"] == false)
                    {
                        btWhatsApp.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private async void CreateJumplist()
        {
            try
            {
                if (JumpList.IsSupported())
                {
                    JumpList jumpList = await JumpList.LoadCurrentAsync();

                    if (UtilityData.newUpdate)
                    {
                        jumpList.Items.Clear();
                    }

                    if (jumpList.Items.Count == 0 || jumpList.Items.Count != 6)
                    {
                        jumpList.Items.Clear();


                        JumpListItem Messenger = JumpListItem.CreateWithArguments("jplFacebook", "Facebook");
                        JumpListItem Skype = JumpListItem.CreateWithArguments("jplTwitter", "Twitter");
                        JumpListItem Slack = JumpListItem.CreateWithArguments("jplInstagram", "Instagram");
                        //JumpListItem WhatsApp = JumpListItem.CreateWithArguments("jplWhatsApp", "WhatsApp");
                        JumpListItem WeChat = JumpListItem.CreateWithArguments("jplReddit", "Reddit");
                        JumpListItem jltSplitView = JumpListItem.CreateWithArguments("jplSplitView", "Split View");

                        Messenger.Logo = new Uri("ms-appx:///Assets/fb.png");
                        Skype.Logo = new Uri("ms-appx:///Assets/twitter.png");
                        Slack.Logo = new Uri("ms-appx:///Assets/instagram.png");
                        //WhatsApp.Logo = new Uri("ms-appx:///Assets/whatsapp.png");
                        WeChat.Logo = new Uri("ms-appx:///Assets/reddit.png");
                        jltSplitView.Logo = new Uri("ms-appx:///Assets/splitview.png");

                        Messenger.GroupName = "Services";
                        Skype.GroupName = "Services";
                        WeChat.GroupName = "Services";
                        //WhatsApp.GroupName = "Services";
                        Slack.GroupName = "Services";
                        jltSplitView.GroupName = "Services";

                        jumpList.Items.Add(Messenger);
                        jumpList.Items.Add(Skype);
                        jumpList.Items.Add(Slack);
                        //jumpList.Items.Add(WhatsApp);
                        jumpList.Items.Add(WeChat);
                        jumpList.Items.Add(jltSplitView);

                        await jumpList.SaveAsync();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void vsgMain_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            RemoveAdsLicense();

            if (e.NewState.Name == "vsDesktop")
            {
                HideCompactMenu();
            }
            else
            {
                mySplitsview.DisplayMode = SplitViewDisplayMode.Overlay;
                rpWeb.Margin = new Thickness(50, 0, 0, 0);
                mySplitsview.IsPaneOpen = false;
            }
        }

        private void HideCompactMenu()
        {
            if (isHideCompactMenu)
            {
                mySplitsview.DisplayMode = SplitViewDisplayMode.Overlay;
                mySplitsview.IsPaneOpen = false;
                rpWeb.Margin = new Thickness(50, 0, 0, 0);
                dspLeftPane.Margin = new Thickness(0);
            }
            else
            {
                dspLeftPane.Margin = new Thickness(15, 0, 0, 0);
                if (vsgMain.CurrentState.Name != "vsMobile")
                {
                    PinnedMenuHelper(isMenuPinned);
                    rpWeb.Margin = new Thickness(0, 0, 0, 0);
                }
            }
        }

        private void btMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!isMenuPinned || mySplitsview.DisplayMode != SplitViewDisplayMode.CompactInline)
            {
                if (rectTest == null)
                    this.FindName("rectTest");
                mySplitsview.IsPaneOpen = !mySplitsview.IsPaneOpen;

                // Acrylic Brush
                if (UtilityData.isFluentDesign && mySplitsview.IsPaneOpen)
                {
                    ((AcrylicBrush)HamburgerAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.Backdrop;
                    mySplitsview.PaneBackground = HamburgerAcrylicBrush;
                }
                rectTest.Visibility = mySplitsview.IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                if (UtilityData.isFluentDesign)
                    ((AcrylicBrush)HamburgerAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                if (rectTest != null) rectTest.Visibility = Visibility.Collapsed;
            }
        }

        private void listboxassets(string image, Type source, string name, string color, bool isDarkMode = false, Uri uri = null, string strEllipse = "")
        {
            webIcon.Source = new BitmapImage(new Uri(string.Format("ms-appx:///Assets/{0}.png", image), UriKind.RelativeOrAbsolute));
            if (webName != null)
                webName.Text = name;
            if (isDarkMode) ChangeColor("#1a1a1a"); else ChangeColor(color);
            if (uri != null)
                myFrame.Navigate(source, uri);
            else
                myFrame.Navigate(source);

            if (SystemInformation.DeviceFamily != "Windows.Mobile" && !isHideCompactMenu)
            {
                Ellipse ellipse = (Ellipse)this.FindName(strEllipse);
                if (ellipse != null)
                    ellipse.Visibility = Visibility.Visible;
            }
        }

        public void ChangeColor(string color)
        {
            if (color == "#1a1a1a" && topSCBrush != null && topSCBrush.Color == Color.FromArgb(255, 26, 26, 26))
                return;

            SolidColorBrush scbColor = new SolidColorBrush(UtilityClass.ConvertStringToColor(color));
            if (scbColor != null)
            {
                topSCBrush = scbColor;
                rpWeb.Background = scbColor;
                UtilityClass.titlebar(scbBrush: scbColor);
                ucCommandBar.scbColor = scbColor;

                if (UtilityData.isFluentDesign)
                {
                    ((AcrylicBrush)HamburgerAcrylicBrush).TintColor = scbColor.Color;
                    ((AcrylicBrush)HamburgerAcrylicBrush).FallbackColor = scbColor.Color;
                    ((AcrylicBrush)HamburgerAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    mySplitsview.PaneBackground = HamburgerAcrylicBrush;

                    ((AcrylicBrush)topAcrylicBrush).TintColor = scbColor.Color;
                    ((AcrylicBrush)topAcrylicBrush).FallbackColor = scbColor.Color;
                    ((AcrylicBrush)topAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    //rpWeb.Background = topAcrylicBrush;
                }
                else
                {
                    mySplitsview.PaneBackground = rpWeb.Background;
                }
            }
        }

        private void mySplitsview_PaneClosed(SplitView sender, object args)
        {
            // Acrylic Brush
            if (UtilityData.isFluentDesign)
            {
                ((AcrylicBrush)HamburgerAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                mySplitsview.PaneBackground = HamburgerAcrylicBrush;
            }
            if (rectTest != null) rectTest.Visibility = Visibility.Collapsed;
        }

        private void mylistbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mylistbox.SelectedIndex != -1)
            {
                rpWeb.Visibility = Visibility.Visible;
                lvSecondary.SelectedIndex = -1;
                if (!isMenuPinned)
                    mySplitsview.IsPaneOpen = false;
                switch (mylistbox.SelectedIndex)
                {
                    case 0:
                        listboxassets("twitter", typeof(BlankPage2), "TWITTER", "#138BDE", UtilityData.isTwitterDark, strEllipse: "EllipseTwitterActive");
                        break;
                    case 1:
                        listboxassets("Instagram", typeof(BlankPage7), "INSTAGRAM", UtilityData.InstagramColor, UtilityData.isInstagramDark, UtilityData.ShareURL, strEllipse: "EllipseInstaActive");
                        break;
                    case 2:
                        if (tbTelegramIAPx.Text == "Unlock Telegram" || tbTelegramIAP.Visibility == Visibility.Visible)
                            btTelegramIAP_Tapped();
                        else
                        {
                            listboxassets("tele", typeof(BlankPage10), "TELEGRAM", UtilityData.TelegramColor, UtilityData.isTelegramDark, strEllipse: "EllipseTeleActive");
                        }
                        break;
                    case 3:
                        listboxassets("whatsapp", typeof(WhatsApp), "WHATSAPP", UtilityData.WhatsAppColor, UtilityData.isWhatsAppDark, strEllipse: "EllipseWhatsAppActive");
                        break;
                    case 4:
                        listboxassets("reddit", typeof(BlankPage5), "REDDIT", "#727C80", strEllipse: "EllipseRedditActive");
                        break;
                    case 5:
                        listboxassets("fb", typeof(BlankPage1), "FACEBOOK", "#4267B2", UtilityData.isFacebookDark, strEllipse: "EllipseFBActive");
                        break;
                    case 6:
                        break;
                    case 7:
                        if (SystemInformation.DeviceFamily != "Windows.Mobile" && !isHideCompactMenu)
                        {
                            this.FindName("EllipseSplitViewActive");
                            EllipseSplitViewActive.Visibility = Visibility.Visible;
                        }
                        rpWeb.Visibility = Visibility.Collapsed;
                        myFrame.Navigate(typeof(BlankPage6));
                        break;
                    default:
                        break;
                }
            }

        }

        private void RemoveAdsLicense()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpRemoveAds))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values[UtilityData.UpRemoveAds] == true)
                {
                    if (rpMobileAds != null)
                        rpMobileAds.Visibility = Visibility.Collapsed;

                    if (rpDesktopAds != null)
                        rpDesktopAds.Visibility = Visibility.Collapsed;

                    myFrame.Margin = new Thickness(0, 0, 0, 0);
                    return;
                }
            }
            if (vsgMain.CurrentState.Name == "vsDesktop")
            {
                this.FindName("rpDesktopAds");
                rpDesktopAds.Visibility = Visibility.Visible;

                if (rpMobileAds != null)
                    rpMobileAds.Visibility = Visibility.Collapsed;

                myFrame.Margin = new Thickness(0, 0, 0, 0);
            }
            else if (vsgMain.CurrentState.Name == "vsMobile")
            {
                this.FindName("rpMobileAds");
                rpMobileAds.Visibility = Visibility.Visible;

                if (rpDesktopAds != null)
                    rpDesktopAds.Visibility = Visibility.Collapsed;

                myFrame.Margin = new Thickness(0, 0, 0, 50);
            }
        }

        private void TelegramLicense()
        {
            btTelegram.Visibility = Visibility.Visible;
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpTelegram))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values[UtilityData.UpTelegram] == true)
                {
                    btTelegram.Visibility = (bool)UtilityClass.ApplicationDataBool("Hide(Tele)", false) ? Visibility.Collapsed : Visibility.Visible;
                    tbTelegramIAP.Visibility = Visibility.Collapsed;
                    tbTelegramIAPx.Text = "Telegram";
                    return;
                }
            }
            tbTelegramIAP.Visibility = Visibility.Visible;
            tbTelegramIAPx.Text = "Unlock Telegram";
        }

        private void PinLicense()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpPin))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values[UtilityData.UpPin] == true)
                {
                    if (btPinIAP != null)
                        btPinIAP.Visibility = Visibility.Collapsed;
                    return;
                }
            }
            this.FindName("btPinIAP");
            btPinIAP.Visibility = Visibility.Visible;
        }


        private async Task<StorePurchaseResult> PurchaseAddOn(string InAppOfferToken)
        {
            string[] filterList = new string[] { "Durable" };

            if (UtilityData.addOnCollection == null || UtilityData.addOnCollection.ExtendedError != null)
                UtilityData.addOnCollection = await storeContext.GetUserCollectionAsync(filterList);

            var isPurchasedList = UtilityData.addOnCollection.Products.Values.Where(p => p.InAppOfferToken.Equals(InAppOfferToken)).ToList();
            if (isPurchasedList.Count == 0)
            {
                if (UtilityData.addOnsAssociatedStoreProducts == null)
                    UtilityData.addOnsAssociatedStoreProducts = await storeContext.GetAssociatedStoreProductsAsync(filterList);

                var AddOnList = UtilityData.addOnsAssociatedStoreProducts.Products.Values.Where(p => p.InAppOfferToken.Equals(InAppOfferToken)).ToList();

                StorePurchaseResult result = await storeContext.RequestPurchaseAsync(AddOnList[0].StoreId);
                return result;
            }
            return null;
        }

        private void CheckAddOnLicense(string InAppOfferToken, string RoamingLicense)
        {
            if (UtilityData.addOnCollection.ExtendedError == null)
            {
                var isPurchasedList = UtilityData.addOnCollection.Products.Values.Where(p => p.InAppOfferToken.Equals(InAppOfferToken)).ToList();
                ApplicationData.Current.RoamingSettings.Values[RoamingLicense] = isPurchasedList.Count == 0 ? false : true;
            }
        }

        private async void InAppPurchaseLicense()
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                    {
                        string[] filterList = new string[] { "Durable" };
                        if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpRemoveAds) || !ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpPin)
                                || !ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpTelegram))
                        {
                            if (UtilityData.addOnCollection == null || UtilityData.addOnCollection.ExtendedError != null)
                                UtilityData.addOnCollection = await storeContext.GetUserCollectionAsync(filterList);
                            CheckAddOnLicense(UtilityData.RemoveAds, UtilityData.UpRemoveAds);
                            CheckAddOnLicense(UtilityData.UnlockTelegram, UtilityData.UpTelegram);
                            CheckAddOnLicense(UtilityData.PinIAP, UtilityData.UpPin);
                        }
                    }
                    else
                    {
                        if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpRemoveAds) || !ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpPin)
                            || !ApplicationData.Current.RoamingSettings.Values.ContainsKey(UtilityData.UpTelegram))
                        {
                            if (UtilityData.AppLicenseInformation == null)
                            {
                                await Task.Run(() =>
                                {
                                    UtilityData.AppLicenseInformation = CurrentApp.LicenseInformation;
                                });
                            }
                            ApplicationData.Current.RoamingSettings.Values[UtilityData.UpRemoveAds] = UtilityData.AppLicenseInformation.ProductLicenses[UtilityData.RemoveAds].IsActive;
                            ApplicationData.Current.RoamingSettings.Values[UtilityData.UpPin] = UtilityData.AppLicenseInformation.ProductLicenses[UtilityData.PinIAP].IsActive;
                            ApplicationData.Current.RoamingSettings.Values[UtilityData.UpTelegram] = UtilityData.AppLicenseInformation.ProductLicenses[UtilityData.UnlockTelegram].IsActive;
                        }
                    }
                }
            }
            catch
            {

            }
            PinLicense();
            RemoveAdsLicense();
            TelegramLicense();
        }

        private async void btTelegramIAP_Tapped()
        {
            this.FindName("gdPurchasingAddon");
            gdPurchasingAddon.Visibility = Visibility.Visible;

            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                {
                    StorePurchaseResult result = await PurchaseAddOn(UtilityData.UnlockTelegram);
                    if (result != null)
                    {
                        switch (result.Status)
                        {
                            case StorePurchaseStatus.AlreadyPurchased:
                                TelegramLicense();
                                //listboxassets("telegram", typeof(Telegram), "Telegram", UtilityData.TelegramColor, UtilityData.isTelegramDark);
                                break;
                            case StorePurchaseStatus.Succeeded:
                                ApplicationData.Current.RoamingSettings.Values[UtilityData.UpTelegram] = true;
                                TelegramLicense();
                                //lvMain.SelectedIndex = 0;
                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                                break;
                            case StorePurchaseStatus.NetworkError:
                            case StorePurchaseStatus.ServerError:
                                UtilityClass.MessageDialog("An Error Occured , Please Try Again!", "Error occured while purchasing.");
                                break;
                        }
                    }
                }
                else
                {
                    if (UtilityData.AppLicenseInformation == null)
                        UtilityData.AppLicenseInformation = CurrentApp.LicenseInformation;

                    if (!UtilityData.AppLicenseInformation.ProductLicenses["UnlockTelegram"].IsActive)
                    {
                        try
                        {
                            PurchaseResults results = await CurrentApp.RequestProductPurchaseAsync("UnlockTelegram");
                            if (results.Status == ProductPurchaseStatus.Succeeded)
                            {
                                ApplicationData.Current.RoamingSettings.Values["UpTelegram"] = UtilityData.AppLicenseInformation.ProductLicenses["UnlockTelegram"].IsActive;
                                TelegramLicense();
                                mylistbox.SelectedIndex = 0;

                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                            }
                        }
                        catch (Exception)
                        {
                            UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
                        }
                    }
                    else
                    {
                        TelegramLicense();
                    }
                }
            }
            catch (Exception)
            {
                UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
            }
            finally
            {
                if (gdPurchasingAddon != null)
                    gdPurchasingAddon.Visibility = Visibility.Collapsed;
            }
        }

        private async void btRemoveDesktopAds_Click(object sender, RoutedEventArgs e)
        {
            this.FindName("gdPurchasingAddon");
            gdPurchasingAddon.Visibility = Visibility.Visible;

            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                {
                    StorePurchaseResult result = await PurchaseAddOn(UtilityData.RemoveAds);
                    if (result != null)
                    {
                        switch (result.Status)
                        {
                            case StorePurchaseStatus.AlreadyPurchased:
                                RemoveAdsLicense();
                                break;
                            case StorePurchaseStatus.Succeeded:
                                ApplicationData.Current.RoamingSettings.Values[UtilityData.UpRemoveAds] = true;
                                RemoveAdsLicense();
                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                                break;
                            case StorePurchaseStatus.NetworkError:
                            case StorePurchaseStatus.ServerError:
                                UtilityClass.MessageDialog("An Error Occured , Please Try Again!", "Error occured while purchasing.");
                                break;
                        }
                    }
                }
                else
                {
                    if (UtilityData.AppLicenseInformation == null)
                        UtilityData.AppLicenseInformation = CurrentApp.LicenseInformation;

                    if (!UtilityData.AppLicenseInformation.ProductLicenses["RemoveAds"].IsActive)
                    {
                        try
                        {
                            PurchaseResults results = await CurrentApp.RequestProductPurchaseAsync("RemoveAds");
                            if (results.Status == ProductPurchaseStatus.Succeeded)
                            {
                                ApplicationData.Current.RoamingSettings.Values["UpRemoveAds"] = UtilityData.AppLicenseInformation.ProductLicenses["RemoveAds"].IsActive;
                                RemoveAdsLicense();
                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                            }
                        }
                        catch (Exception)
                        {
                            UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
                        }
                    }
                    else
                    {
                        // The customer already owns this feature.
                    }
                }
            }
            catch (Exception)
            {
                UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
            }
            finally
            {
                if (gdPurchasingAddon != null)
                    gdPurchasingAddon.Visibility = Visibility.Collapsed;
            }
        }

        private void btPinIAP_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage4), "PasswordIAP");
            if (!isMenuPinned)
                mySplitsview.IsPaneOpen = false;
        }

        //private void btwebIcon_Click(object sender, RoutedEventArgs e)
        //{
        //}

        private async void ShowCompactView(bool isEnabled)
        {
            if (isEnabled)
            {
                rpWeb.Visibility = Visibility.Collapsed;
                mySplitsview.CompactPaneLength = 0;
                dspLeftPane.Margin = new Thickness(0);
                mySplitsview.IsPaneOpen = false;
                ViewModePreferences vmPreference = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                vmPreference.CustomSize = new Size(500, 500);
                vmPreference.ViewSizePreference = ViewSizePreference.UseMore;

                bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay, vmPreference);
            }
            else
            {
                rpWeb.Visibility = Visibility.Visible;
                mySplitsview.CompactPaneLength = 50;
                mySplitsview.IsPaneOpen = isMenuPinned;
                if (!isHideCompactMenu)
                {
                    dspLeftPane.Margin = new Thickness(15, 0, 0, 0);
                }
                bool modeSwitched = await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
            }
            bthamburger.Visibility = rpWeb.Visibility;
        }

        private void webIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (btFaceBook.IsSelected) BlankPage1.notifyChange.setSettings = "Home";
            else if (btTwitter.IsSelected) BlankPage2.notifyChange.setSettings = "Home";
            else if (btInstagram.IsSelected) BlankPage7.notifyChange.setSettings = "Home";
            else if (btTelegram.IsSelected) BlankPage10.notifyChange.setSettings = "Home";
            else if (btWhatsApp.IsSelected) WhatsApp.notifyChange.setSettings = "Home";
            else if (btReddit.IsSelected) BlankPage5.notifyChange.setSettings = "Home";
        }

        private void pinSplitView_Click(object sender, RoutedEventArgs e)
        {
            isMenuPinned = !isMenuPinned;
            PinnedMenuHelper(isMenuPinned);
            ApplicationData.Current.LocalSettings.Values["isMenuPinned"] = isMenuPinned;
        }

        private void PinnedMenuHelper(bool isPinned)
        {
            if (isPinned)
            {
                btPinSplitView.Content = "\uE77A";
                ToolTipService.SetToolTip(btPinSplitView, "UnPin this Pane");
                mySplitsview.DisplayMode = SplitViewDisplayMode.CompactInline;
                mySplitsview.IsPaneOpen = true;
                if (rectTest != null) rectTest.Visibility = Visibility.Collapsed;
                if (UtilityData.isFluentDesign) ((AcrylicBrush)HamburgerAcrylicBrush).BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
            }
            else
            {
                btPinSplitView.Content = "\uE840";
                ToolTipService.SetToolTip(btPinSplitView, "Pin this Pane");
                mySplitsview.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                mySplitsview.IsPaneOpen = false;
            }
        }

        private void FadedNotify(string Message)
        {
            if (ucFadeControl == null)
            {
                this.FindName("ucFadeControl");

                if (UtilityData.isFluentDesign)
                {
                    ucFadeControl.Background = new AcrylicBrush()
                    {
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                        TintOpacity = 0.5,
                        TintColor = (ucFadeControl.Background as SolidColorBrush).Color,
                        FallbackColor = (ucFadeControl.Background as SolidColorBrush).Color
                    };
                }
            }
            ucFadeControl.Visibility = Visibility.Visible;
            ucFadeControl.Show(Message, 2000);
        }

        private void OffsetAnimation_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                UIElementCollection stackChild = null;
                var item = (ListViewItem)sender;
                if (item.Content is Grid)
                {
                    var stackPanel = (Grid)item.Content;
                    stackChild = stackPanel.Children;
                }
                else
                {
                    var stackPanel = (StackPanel)item.Content;
                    stackChild = stackPanel.Children;
                }
                UtilityClass.moveXOffset(stackChild[0], 5);
                UtilityClass.moveXOffset(stackChild[1], -5);
            }
            catch (Exception)
            {

            }
        }

        private void OffsetAnimation_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                UIElementCollection stackChild = null;
                var item = (ListViewItem)sender;
                if (item.Content is Grid)
                {
                    var stackPanel = (Grid)item.Content;
                    stackChild = stackPanel.Children;
                }
                else
                {
                    var stackPanel = (StackPanel)item.Content;
                    stackChild = stackPanel.Children;
                }
                UtilityClass.moveXOffset(stackChild[0], 0);
                UtilityClass.moveXOffset(stackChild[1], 0);
            }
            catch (Exception)
            {

            }
        }

        private void lvSecondary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (lvSecondary.SelectedIndex)
            {
                case 0:
                    Frame.Navigate(typeof(BlankPage4), "MoreApps");
                    break;
                case 1:
                    Frame.Navigate(typeof(BlankPage4));
                    break;
                default:
                    break;
            }
            lvSecondary.SelectedIndex = -1;
            if (!isMenuPinned)
                mySplitsview.IsPaneOpen = false;
        }

        private async void BtAdTwitter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var launched = await Launcher.LaunchUriAsync(new Uri("https://twitter.com/define_studio"));
            }
            catch (Exception)
            {

            }
        }
    }
}
