using App6.Models;
using MainSCLibrary;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.Phone.UI.Input;
using Windows.Services.Store;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App6
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage4 : Page
    {
        private ObservableCollection<SettingsCategoryClass> obSettingsList = new ObservableCollection<SettingsCategoryClass>();

        private Brush HeaderBrush;
        private Compositor _compositor;
        public static ncMainPage ncSettings { get; set; }
        private Color titleBarColor;
        private StoreContext storeContext;
        public BlankPage4()
        {
            this.InitializeComponent();
            ncSettings = new ncMainPage();
            ncSettings.PropertyChanged += NcSettings_PropertyChanged;
            OneTimeSave();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            UtilityClass.TranslationAnimation(_compositor, gdGeneralSettings, false);
            UtilityClass.TranslationAnimation(_compositor, gdAddOnsSettings, false);
        }

        private void NcSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ncSettings.setSettings == "IAP")
            {
                ucSettings.ncSettings.setSettings = "IAP";
                PinLicense();
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            lvSettingsCategory.SelectedIndex = -1;
            if (vsgMain.CurrentState.Name == vsMobile.Name && !gdColumn0.Width.IsStar)
            {
                gdColumn0.Width = new GridLength(1, GridUnitType.Star);
                gdColumn1.Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                if (this.Frame.CanGoBack)
                    Frame.GoBack();
            }
            e.Handled = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            if (ApiInformation.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= AboutPage_BackRequested;
            }
            UtilityClass.titlebar(scbBrush: new SolidColorBrush(titleBarColor));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            if (ApiInformation.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += AboutPage_BackRequested;
            }

            try
            {
                if (lvSettingsCategory.Items.Count <= 0)
                    obSettingsList = SettingsCategoryClass.getSettingsCategories();

                lvSettingsCategory.ItemsSource = obSettingsList;
                if (e.Parameter != null && (string)e.Parameter == "in-apps")
                {
                    lvSettingsCategory.SelectedIndex = 2;
                    if (vsgMain.CurrentState.Name == vsMobile.Name)
                    {
                        gdColumn1.Width = new GridLength(1, GridUnitType.Star);
                        gdColumn0.Width = new GridLength(0, GridUnitType.Pixel);

                    }
                    //btRemoveAds_Click(null, null);
                }
                else if (vsgMain.CurrentState.Name != vsMobile.Name)
                {
                    lvSettingsCategory.SelectedIndex = 0;
                }
                else
                {
                    gdColumn0.Width = new GridLength(1, GridUnitType.Star);
                    gdColumn1.Width = new GridLength(0, GridUnitType.Pixel);

                    if (!UtilityData.isFluentDesign)
                    {
                        gdGeneralSettings.Padding = new Thickness(0, 90, 0, 0);
                        gdHeaderOverlay.Visibility = Visibility.Collapsed;
                    }
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (e.Parameter != null && e.Parameter is string)
                {
                    switch ((string)e.Parameter)
                    {
                        case "MoreApps":
                            lvSettingsCategory.SelectedIndex = 4;
                            break;
                        case "PasswordIAP":
                            btpinIAP_Click(null, null);
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
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
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
            {
                tgsFluentDesign.Visibility = Visibility.Visible;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("isFluentDesign"))
                {
                    tgsFluentDesign.IsOn = (bool)ApplicationData.Current.LocalSettings.Values["isFluentDesign"];
                }
                else
                {
                    tgsFluentDesign.IsOn = true;
                }
            }
            else
            {
                tgsFluentDesign.Visibility = Visibility.Collapsed;
            }
            prClearCache.Visibility = Visibility.Collapsed;
            HideCompactMenu.IsOn = (bool)UtilityClass.ApplicationDataBool("HideCompactMenu", false);
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("defaultPin"))
            {
                defaultPin.IsChecked = (bool)ApplicationData.Current.LocalSettings.Values["defaultPin"];
            }
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("WindowsHello"))
            {
                WindowsHelloPin.IsChecked = (bool)ApplicationData.Current.LocalSettings.Values["WindowsHello"];
            }
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                chbbthardwareBack.IsChecked = (bool)UtilityClass.ApplicationDataBool("bthardwareBack", false);
                chbbthardwareBack.Visibility = Visibility.Visible;
            }
            else
                chbbthardwareBack.Visibility = Visibility.Collapsed;

        }

        private void tgsFluentDesign_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["isFluentDesign"] = tgsFluentDesign.IsOn;
            //UtilityData.isFluentDesign = tgsFluentDesign.IsOn;
            //AboutPage.ncSettings.setSettings = "FluentDesign";
        }

        private void AboutPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            lvSettingsCategory.SelectedIndex = -1;
            if (vsgMain.CurrentState.Name == vsMobile.Name && !gdColumn0.Width.IsStar)
            {
                gdColumn0.Width = new GridLength(1, GridUnitType.Star);
                gdColumn1.Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                if (this.Frame.CanGoBack)
                    Frame.GoBack();
            }
            e.Handled = true;
        }

        private void savedsettings()
        {
            try
            {
                UISettings uiColor = new UISettings();
                gdRectfillColor.Background = new SolidColorBrush(uiColor.GetColorValue(UIColorType.AccentDark2));
                MainGrid.Background = new SolidColorBrush(uiColor.GetColorValue(UIColorType.AccentDark3));
            }
            catch (Exception)
            {
                gdRectfillColor.Background = UtilityData.ScrollBrush;
            }
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBarColor = (Color)titleBar.BackgroundColor;
            UtilityClass.titlebar(scbBrush: (SolidColorBrush)gdRectfillColor.Background);

            _ucInAppPurchase.btIAPFixLicense.Background = gdRectfillColor.Background;

            if (UtilityData.isFluentDesign)
            {
                HeaderBrush = new AcrylicBrush
                {
                    FallbackColor = ((SolidColorBrush)gdRectfillColor.Background).Color,
                    TintColor = ((SolidColorBrush)gdRectfillColor.Background).Color,
                    TintOpacity = 0.5,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop
                };
                //SetAcrylicBackground(rpMain, 0.8);
                SetAcrylicBackground(gdRectfillColor, 0.7);
                SetAcrylicBackground(MainGrid, 0.6);
                _ucInAppPurchase.btIAPFixLicense.Background = gdRectfillColor.Background;
            }
            else
            {
                HeaderBrush = gdRectfillColor.Background;
            }
            if (vsgMain.CurrentState != null && vsgMain.CurrentState.Name == vsMobile.Name)
                gdHeader.Background = HeaderBrush;
        }


        private void SetAcrylicBackground(Panel panel, double opacity)
        {
            if (UtilityData.isFluentDesign)
            {
                panel.Background = new AcrylicBrush()
                {
                    FallbackColor = ((SolidColorBrush)panel.Background).Color,
                    TintColor = ((SolidColorBrush)panel.Background).Color,
                    TintOpacity = opacity,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                };
            }
        }

        private void PinLicense()
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey("UpPin"))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values["UpPin"] == true)
                {
                    btpinIAP.Visibility = Visibility.Collapsed;
                    btsetpin.Visibility = Visibility.Visible;
                    spPinSelection.Visibility = Visibility.Visible;

                    if (defaultPin.IsChecked == false && WindowsHelloPin.IsChecked == false)
                    {
                        defaultPin.IsChecked = true;
                    }

                    if (defaultPin.IsChecked == true && WindowsHelloPin.IsChecked == false)
                    {
                        btsetpin.Visibility = Visibility.Visible;
                    }
                    else if (defaultPin.IsChecked == false && WindowsHelloPin.IsChecked == true)
                    {
                        btsetpin.Visibility = Visibility.Collapsed;
                    }
                    return;
                }
            }

            spPinSelection.Visibility = Visibility.Collapsed;
            btsetpin.Visibility = Visibility.Collapsed;
            btpinIAP.Visibility = Visibility.Visible;
        }

        private async void btAboutSupport_Click(object sender, RoutedEventArgs e)
        {
            Button btsender = sender as Button;
            try
            {
                Uri uri = null;
                switch (btsender.Tag.ToString())
                {
                    case "mail":
                        uri = new Uri("mailto:definestudio@outlook.com?Body=Sent%20from%20Socialize%20Up");
                        break;
                    case "rating":
                        uri = new Uri("ms-windows-store:REVIEW?PFN=5913DefineStudio.SocializeUp_jj4r3mnwe2ey2");
                        break;
                    case "twitter":
                        uri = new Uri("https://twitter.com/define_studio");
                        break;
                    case "privacy":
                        uri = new Uri("http://www.definestudio.in/p/privacy-policy-information-collection.html");
                        break;
                    case "site":
                        uri = new Uri("http://www.definestudio.in/");
                        break;
                }
                var launched = await Launcher.LaunchUriAsync(uri);
            }
            catch (Exception)
            {

            }
        }


        private async void btClearCache_Click(object sender, RoutedEventArgs e)
        {
            prClearCache.Visibility = Visibility.Visible;
            btClearCache.IsEnabled = false;
            await WebView.ClearTemporaryWebDataAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
            prClearCache.Visibility = Visibility.Collapsed;
            btClearCache.IsEnabled = true;
            UtilityClass.MessageDialog("Cache and Temporary Files have been Cleaned, May require restart.", "Cache and Temporary Files Cleaned");
        }

        private void btsetpin_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(setpin));
        }

        private async void btpinIAP_Click(object sender, RoutedEventArgs e)
        {
            this.FindName("gdPurchasingAddon");
            gdPurchasingAddon.Visibility = Visibility.Visible;

            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                {
                    string[] filterList = new string[] { "Durable" };

                    if (UtilityData.addOnCollection == null || UtilityData.addOnCollection.ExtendedError != null)
                        UtilityData.addOnCollection = await storeContext.GetUserCollectionAsync(filterList);

                    var isPurchasedList = UtilityData.addOnCollection.Products.Values.Where(p => p.InAppOfferToken.Equals(UtilityData.PinIAP)).ToList();
                    if (isPurchasedList.Count == 0)
                    {
                        if (UtilityData.addOnsAssociatedStoreProducts == null)
                            UtilityData.addOnsAssociatedStoreProducts = await storeContext.GetAssociatedStoreProductsAsync(filterList);

                        var AddOnList = UtilityData.addOnsAssociatedStoreProducts.Products.Values.Where(p => p.InAppOfferToken.Equals(UtilityData.PinIAP)).ToList();

                        StorePurchaseResult result = await storeContext.RequestPurchaseAsync(AddOnList[0].StoreId);
                        if (result != null)
                        {
                            switch (result.Status)
                            {
                                case StorePurchaseStatus.AlreadyPurchased:
                                    PinLicense();
                                    break;
                                case StorePurchaseStatus.Succeeded:
                                    ApplicationData.Current.RoamingSettings.Values[UtilityData.UpPin] = true;
                                    PinLicense();
                                    UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                                    break;
                                case StorePurchaseStatus.NetworkError:
                                case StorePurchaseStatus.ServerError:
                                    UtilityClass.MessageDialog("An Error Occured , Please Try Again!", "Error occured while purchasing.");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (UtilityData.AppLicenseInformation == null)
                        UtilityData.AppLicenseInformation = CurrentApp.LicenseInformation;
                    if (!UtilityData.AppLicenseInformation.ProductLicenses["PinIAP"].IsActive)
                    {
                        try
                        {
                            PurchaseResults results = await CurrentApp.RequestProductPurchaseAsync("PinIAP");
                            if (results.Status == ProductPurchaseStatus.Succeeded)
                            {
                                ApplicationData.Current.RoamingSettings.Values["UpPin"] = UtilityData.AppLicenseInformation.ProductLicenses["PinIAP"].IsActive;
                                PinLicense();
                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                            }
                        }
                        catch
                        {
                            UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception)
            {
                UtilityClass.MessageDialog("Please Check your Internet Connection and then try again", "No Internet Connection.");
            }
            finally
            {
                if (gdPurchasingAddon != null) gdPurchasingAddon.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Core.CoreApplicationViewTitleBar") && Microsoft.Toolkit.Uwp.Helpers.SystemInformation.DeviceFamily == "Windows.Desktop")
            {
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

                gdTitleBar.Visibility = Visibility.Visible;
                if (gdTitleBar.Visibility == Visibility.Visible)
                    Window.Current.SetTitleBar(gdTitleBar);
            }
            savedsettings();
            PinLicense();
        }

        private void bthelp_Click(object sender, RoutedEventArgs e)
        {
            UtilityClass.MessageDialog("You can Unlock the Password and Windows Hello Setting to secure your app by setting your own Password or Windows Hello so that no one can use your app Without your Permission/Password.", "Unlock Custom Password and Windows Hello");
        }

        private async void lvDSApps_ItemClick(object sender, ItemClickEventArgs e)
        {
            var obj = (DefineStudioApps)e.ClickedItem;
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productId=" + obj.ProductId));
        }

        private void defaultPin_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["defaultPin"] = defaultPin.IsChecked;
            ApplicationData.Current.LocalSettings.Values["WindowsHello"] = WindowsHelloPin.IsChecked;
            if (defaultPin.IsChecked == true && WindowsHelloPin.IsChecked == false)
            {
                btsetpin.Visibility = Visibility.Visible;
            }
            else if (defaultPin.IsChecked == false && WindowsHelloPin.IsChecked == true)
            {
                btsetpin.Visibility = Visibility.Collapsed;
            }
        }

        private void HideCompactMenu_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["HideCompactMenu"] = HideCompactMenu.IsOn;
        }

        private void chbbthardwareBack_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["bthardwareBack"] = chbbthardwareBack.IsChecked;
            UtilityData.isbtHardwareBack = (bool)chbbthardwareBack.IsChecked;
        }

        private void tbVersionNumber_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("NoOfDevClick"))
            {
                string strNoOfClick = ApplicationData.Current.LocalSettings.Values["NoOfDevClick"].ToString();

                int noOfClick = Convert.ToInt32(strNoOfClick);

                if (noOfClick < 5)
                {
                    ApplicationData.Current.LocalSettings.Values["NoOfDevClick"] = noOfClick + 1;

                    if (noOfClick + 1 == 5)
                    {
                        UtilityClass.MessageDialog("Congratulation! You have unlocked a new hidden feature, Find it in Services Settings Pane", "Congratulations! You've done it!");
                    }
                }
                else
                {
                    UtilityClass.MessageDialog("You have already unlocked a new hidden feature, Find it in Services Settings Pane", "You did it already!");
                }
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["NoOfDevClick"] = 1;
            }
        }

        private async void lvSettingsCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hideAllSettingsCategory();

            visualSetters();

            switch (lvSettingsCategory.SelectedIndex)
            {
                case 0:
                    if (gdGeneralSettings == null)
                        this.FindName("gdGeneralSettings");
                    gdGeneralSettings.Visibility = Visibility.Visible;
                    break;
                case 1:
                    if (gdAboutSettings == null)
                    {
                        this.FindName("gdAboutSettings");
                        tbVersionNumber.Content = UtilityData.VersionNumber;
                        UtilityClass.TranslationAnimation(_compositor, gdAboutSettings, false);
                    }
                    gdAboutSettings.Visibility = Visibility.Visible;
                    break;
                case 2:
                    if (gdAddOnsSettings == null)
                        this.FindName("gdAddOnsSettings");
                    gdAddOnsSettings.Visibility = Visibility.Visible;
                    break;
                case 3:
                    if (gdChangelogSettings == null)
                    {
                        this.FindName("gdChangelogSettings");
                        tbChangelog.Text = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/Changelog.txt")));
                        UtilityClass.TranslationAnimation(_compositor, gdChangelogSettings, false);
                    }
                    gdChangelogSettings.Visibility = Visibility.Visible;
                    break;
                case 4:
                    if (gdOurAppsSettings == null)
                    {
                        this.FindName("gdOurAppsSettings");
                        UtilityClass.TranslationAnimation(_compositor, gdOurAppsSettings, false);
                        lvDSApps.ItemsSource = DefineStudioApps.getAppsData("Socialize Up").OrderByDescending(p => p.AppName).Reverse();
                    }
                    gdOurAppsSettings.Visibility = Visibility.Visible;
                    break;
            }
            if (lvSettingsCategory.SelectedIndex >= 0 && lvSettingsCategory.SelectedIndex <= 6)
                tbSettingsHeader.Text = obSettingsList[lvSettingsCategory.SelectedIndex].title;


        }

        private void hideAllSettingsCategory()
        {
            if (gdGeneralSettings != null)
                gdGeneralSettings.Visibility = Visibility.Collapsed;

            if (gdAboutSettings != null)
                gdAboutSettings.Visibility = Visibility.Collapsed;

            if (gdAddOnsSettings != null)
                gdAddOnsSettings.Visibility = Visibility.Collapsed;

            if (gdChangelogSettings != null)
                gdChangelogSettings.Visibility = Visibility.Collapsed;

            if (gdOurAppsSettings != null)
                gdOurAppsSettings.Visibility = Visibility.Collapsed;
        }

        private void vsgMain_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (lvSettingsCategory.SelectedIndex >= 0)
            {
                visualSetters();
            }
            else
            {
                lvSettingsCategory.SelectedIndex = 0;
            }
        }

        private void visualSetters()
        {
            if (vsgMain.CurrentState.Name == vsMobile.Name)
            {
                gdColumn0.Width = new GridLength(0, GridUnitType.Pixel);
                gdColumn1.Width = new GridLength(1, GridUnitType.Star);
                gdHeader.Background = HeaderBrush;

                if (!UtilityData.isFluentDesign)
                {
                    gdGeneralSettings.Padding = new Thickness(0, 90, 0, 0);
                    gdHeaderOverlay.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                gdHeader.Background = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void stackOffSet_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var stackPanel = (Grid)sender;
                var stackChild = stackPanel.Children;
                UtilityClass.moveXOffset(stackChild[0], 5);
                UtilityClass.moveXOffset(stackChild[1], -5);
            }
            catch (Exception)
            {

            }
        }

        private void stackOffSet_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var stackPanel = (Grid)sender;
                var stackChild = stackPanel.Children;
                UtilityClass.moveXOffset(stackChild[0], 0);
                UtilityClass.moveXOffset(stackChild[1], 0);
            }
            catch (Exception)
            {

            }
        }

        private void SpDSApps_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var stackPanel = (StackPanel)sender;
                stackPanel.Children[0].Scale((float)1, (float)1, (float)50.0, (float)50.0).Start();
            }
            catch (Exception)
            {

            }
        }
        private void SpDSApps_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                var stackPanel = (StackPanel)sender;
                stackPanel.Children[0].Scale((float)1.2, (float)1.2, (float)50.0, (float)50.0).Start();
                //UtilityClass.moveXOffset(stackPanel, 5);
            }
            catch (Exception)
            {

            }
        }
        private async void BtDownloadApp_Click(object sender, RoutedEventArgs e)
        {
            var obj = (DefineStudioApps)((Button)sender).DataContext;
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productId=" + obj.ProductId));
        }

        private void BtOffset_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var stack = (StackPanel)((Button)sender).Content;
            UtilityClass.moveXOffset(stack.Children[0], 3);
            UtilityClass.moveXOffset(stack.Children[1], -3);
        }

        private void BtOffset__PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var stack = (StackPanel)((Button)sender).Content;
            UtilityClass.moveXOffset(stack.Children[0], 0);
            UtilityClass.moveXOffset(stack.Children[1], 0);
        }
    }

    public class SettingsCategoryClass
    {
        public string icon { get; set; }
        public string title { get; set; }

        private static ObservableCollection<SettingsCategoryClass> obCategories;

        public SettingsCategoryClass()
        {

        }

        public static ObservableCollection<SettingsCategoryClass> getSettingsCategories()
        {
            obCategories = new ObservableCollection<SettingsCategoryClass>();
            obCategories.Add(new SettingsCategoryClass { icon = "\uE81E", title = "General" });
            obCategories.Add(new SettingsCategoryClass { icon = "\uE946", title = "About" });
            obCategories.Add(new SettingsCategoryClass { icon = "\uE14D", title = "In-App Purchase" });
            obCategories.Add(new SettingsCategoryClass { icon = "\uE777", title = "Changelog" });
            obCategories.Add(new SettingsCategoryClass { icon = "\uE71D", title = "Our Apps" });
            return obCategories;
        }
    }
}
