using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Services.Store;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace App6.Models
{
    public sealed partial class ucInAppPurchaseHub : UserControl
    {
        private StoreContext storeContext;
        public ucInAppPurchaseHub()
        {
            this.InitializeComponent();
            try
            {
                storeContext = StoreContext.GetDefault();
            }
            catch (Exception)
            {
                
            }
        }

        private async Task PurchaseInAppPurchase(string Product, string strRoaming)
        {
            try
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                {
                    string[] filterList = new string[] { "Durable" };

                    if (UtilityData.addOnCollection == null || UtilityData.addOnCollection.ExtendedError != null)
                        UtilityData.addOnCollection = await storeContext.GetUserCollectionAsync(filterList);

                    var isPurchasedList = UtilityData.addOnCollection.Products.Values.Where(p => p.InAppOfferToken.Equals(Product)).ToList();
                    if (isPurchasedList.Count == 0)
                    {
                        if (UtilityData.addOnsAssociatedStoreProducts == null)
                            UtilityData.addOnsAssociatedStoreProducts = await storeContext.GetAssociatedStoreProductsAsync(filterList);

                        var AddOnList = UtilityData.addOnsAssociatedStoreProducts.Products.Values.Where(p => p.InAppOfferToken.Equals(Product)).ToList();

                        StorePurchaseResult result = await storeContext.RequestPurchaseAsync(AddOnList[0].StoreId);
                        if (result != null)
                        {
                            switch (result.Status)
                            {
                                case StorePurchaseStatus.AlreadyPurchased:
                                    break;
                                case StorePurchaseStatus.Succeeded:
                                    ApplicationData.Current.RoamingSettings.Values[strRoaming] = true;
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
                    if (!UtilityData.AppLicenseInformation.ProductLicenses[Product].IsActive)
                    {
                        try
                        {
                            PurchaseResults results = await CurrentApp.RequestProductPurchaseAsync(Product);
                            if (results.Status == ProductPurchaseStatus.Succeeded)
                            {
                                ApplicationData.Current.RoamingSettings.Values[strRoaming] = UtilityData.AppLicenseInformation.ProductLicenses[Product].IsActive;
                                BlankPage4.ncSettings.setSettings = "IAP";
                                UtilityClass.MessageDialog("Thank You very much for Purchasing. We really appreciate your kind support!", "Thank You very much! :)");
                            }
                        }
                        catch (Exception)
                        {
                            UtilityClass.MessageDialog("An Error Occured, Please Try Again", "Error");
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception)
            {
                UtilityClass.MessageDialog("An Error Occured, Please Try Again", "Error");
            }
        }

        private void btIAPFixLicenseInfo_Click(object sender, RoutedEventArgs e)
        {
            UtilityClass.MessageDialog("If the product or feature you have purchased from Store and it is not working or showing in App, this may help you to fix it. Make sure to restart the app after doing this! If it is still not working after doing this, Contact us and we will help you to resolve the issue.", "Refresh or Fix In-App Purchase Licenses");
        }

        private async void btIAPFixLicense_Click(object sender, RoutedEventArgs e)
        {
            btIAPFixLicense.IsEnabled = false;
            prFixIAPLicense.Visibility = Visibility.Visible;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3))
                    {
                        if (UtilityData.addOnCollection == null || UtilityData.addOnCollection.ExtendedError != null)
                            UtilityData.addOnCollection = await storeContext.GetUserCollectionAsync(new string[] { "Durable" });

                        CheckAddOnLicense(UtilityData.RemoveAds, UtilityData.UpRemoveAds);
                        CheckAddOnLicense(UtilityData.PinIAP, UtilityData.UpPin);
                        CheckAddOnLicense(UtilityData.UnlockTelegram, UtilityData.UpTelegram);
                    }
                    else
                    {
                        if (UtilityData.AppLicenseInformation == null)
                        {
                            await Task.Run(() =>
                            {
                                UtilityData.AppLicenseInformation = CurrentApp.LicenseInformation;
                            });
                        }

                        ApplicationData.Current.RoamingSettings.Values["UpRemoveAds"] = UtilityData.AppLicenseInformation.ProductLicenses["RemoveAds"].IsActive;
                        ApplicationData.Current.RoamingSettings.Values["UpPin"] = UtilityData.AppLicenseInformation.ProductLicenses["PinIAP"].IsActive;
                        ApplicationData.Current.RoamingSettings.Values["UpTelegram"] = UtilityData.AppLicenseInformation.ProductLicenses["UnlockTelegram"].IsActive;
                    }
                    UtilityClass.MessageDialog("In-App License have been Updated, May require restart to see Changes", "Licenses has been Updated");
                }
                else
                {
                    UtilityClass.MessageDialog("Please Check Your Internet Connections", "No Internet Connection");
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                prFixIAPLicense.Visibility = Visibility.Collapsed;
                btIAPFixLicense.IsEnabled = true;
            }
            BlankPage4.ncSettings.setSettings = "IAP";
            License(gdTbRemoveAds, tbRemoveAds, UtilityData.UpRemoveAds, tbRemoveAdsPurchased);
            License(gdTbTelegram, tbTelegram, UtilityData.UpTelegram, tbTelegramPurchased);
            License(gdTbPassword, tbPassword, UtilityData.UpPin, tbPasswordPurchased);
        }

        private void CheckAddOnLicense(string InAppOfferToken, string RoamingLicense)
        {
            if (UtilityData.addOnCollection.ExtendedError == null)
            {
                var isPurchasedList = UtilityData.addOnCollection.Products.Values.Where(p => p.InAppOfferToken.Equals(InAppOfferToken)).ToList();
                //var x = isPurchasedList.Count == 0 ? false : true;
                ApplicationData.Current.RoamingSettings.Values[RoamingLicense] = isPurchasedList.Count == 0 ? false : true;
            }
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
        private async void lvInAppPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvInAppPurchase.SelectedIndex != -1)
            {
                switch (lvInAppPurchase.SelectedIndex)
                {
                    case 0:
                        await PurchaseInAppPurchase(UtilityData.RemoveAds, UtilityData.UpRemoveAds);
                        License(gdTbRemoveAds, tbRemoveAds, UtilityData.UpRemoveAds, tbRemoveAdsPurchased);
                        break;
                    case 1:
                        await PurchaseInAppPurchase(UtilityData.UnlockTelegram, UtilityData.UpTelegram);
                        License(gdTbTelegram, tbTelegram, UtilityData.UpTelegram, tbTelegramPurchased);
                        break;
                    case 2:
                        await PurchaseInAppPurchase(UtilityData.PinIAP, UtilityData.UpPin);
                        License(gdTbPassword, tbPassword, UtilityData.UpPin, tbPasswordPurchased);
                        break;
                }
                lvInAppPurchase.SelectedIndex = -1;
            }
        }

        private void License(Grid gdTextBlock, TextBlock tbTextblock, string strLicense, TextBlock tbPurchased)
        {
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(strLicense))
            {
                if ((bool)ApplicationData.Current.RoamingSettings.Values[strLicense] == true)
                {
                    tbTextblock.Text = "\uE081";

                    // Color #017001
                    gdTextBlock.Background = new SolidColorBrush(Color.FromArgb(255, 1, 112, 1));
                    tbPurchased.Text = "Purchased";
                    return;
                }
            }
            tbTextblock.Text = "\uE785";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            License(gdTbRemoveAds, tbRemoveAds, UtilityData.UpRemoveAds, tbRemoveAdsPurchased);
            License(gdTbTelegram, tbTelegram, UtilityData.UpTelegram, tbTelegramPurchased);
            License(gdTbPassword, tbPassword, UtilityData.UpPin, tbPasswordPurchased);
        }
    }
}
