using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace App6.Models
{
    public sealed partial class ucDownloadUI : UserControl
    {
        private DownloadOperation DownloadFile;
        private CancellationTokenSource cancellationToken;
        private StorageFolder folder = KnownFolders.SavedPictures;
        private UtilityClass Utility = new UtilityClass();
        private StorageFolder tempFolder = ApplicationData.Current.LocalFolder;
        private StorageFile tempDestinationFile;
        public ucDownloadUI()
        {
            this.InitializeComponent();
            OneTimeSave();
        }

        private async void OneTimeSave()
        {
            try
            {
                await gdBlur.Blur(5, 0, 0).StartAsync();
            }
            catch (Exception)
            {

            }
            setAcrylicBrush(spMainDownloadUI);
        }

        private void setAcrylicBrush(Panel panel)
        {
            if (UtilityData.isFluentDesign)
            {
                panel.Background = new AcrylicBrush()
                {
                    FallbackColor = ((SolidColorBrush)panel.Background).Color,
                    TintColor = ((SolidColorBrush)panel.Background).Color,
                    TintOpacity = 0.8,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop
                };
            }
        }

        private async void btLocation_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker { SuggestedStartLocation = PickerLocationId.Downloads };
            picker.FileTypeFilter.Add("*");
            StorageFolder _folder = await picker.PickSingleFolderAsync();
            if (_folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("DownloadFolderToken", _folder);
                folder = _folder;
                txbLocation.Text = _folder.Path;
            }
        }

        private void txbFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private async void txbLocation_Loaded(object sender, RoutedEventArgs e)
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("DownloadFolderToken"))
            {
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("DownloadFolderToken");
                txbLocation.Text = folder.Path;
            }
            else
            {
                txbLocation.Text = KnownFolders.SavedPictures.Path;
                folder = KnownFolders.SavedPictures;
            }
        }

        private async void btStartDownload(object sender, RoutedEventArgs e)
        {
            tempDestinationFile = await tempFolder.CreateFileAsync("tempDownload", CreationCollisionOption.ReplaceExisting);
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadFile = downloader.CreateDownload(new Uri(txbDownloadLink.Text), tempDestinationFile);
            Progress<DownloadOperation> progress = new Progress<DownloadOperation>(progressChangedAsync);
            cancellationToken = new CancellationTokenSource();
            try
            {
                txbFileName.Visibility = Visibility.Collapsed;
                tbFileName.Visibility = Visibility.Visible;
                tbSize.Text = "Initializing...";
                btPause.IsEnabled = false;
                tsDownload.IsOn = true;
                tbFileName.Text = txbFileName.Text;
                await DownloadFile.StartAsync().AsTask(cancellationToken.Token, progress);
            }
            catch (TaskCanceledException)
            {
                CleanUpDownloadLeftOver();
            }
            catch (Exception)
            {
                CleanUpDownloadLeftOver();
                UtilityClass.MessageDialog("Error Occured while downloading file, Please Try again", "Error");
            }
        }

        private async void btDownloadUICancel(object sender, RoutedEventArgs e)
        {
            if (tsDownload.IsOn)
            {
                var cntDialog = UtilityClass.NewMessageDialogBox("Cancel Download? All Progress will be lost.", "Do you want to Cancel the Download", "Yes", "No");
                cntDialog.PrimaryButtonClick += (s, a) =>
                {
                    cancellationToken.Cancel();
                    // DownloadFile?.ResultFile.DeleteAsync();
                    CleanUpDownloadLeftOver();
                    cntDialog.Hide();
                };
                cntDialog.SecondaryButtonClick += (s, a) =>
                {
                    cntDialog.Hide();
                };
                await cntDialog.ShowAsync();
            }
            else
            {
                CleanUpDownloadLeftOver();
            }
        }

        private async void CleanUpDownloadLeftOver(bool isDownloadSuccessfull = false)
        {
            if (!isDownloadSuccessfull)
            {
                this.Visibility = Visibility.Collapsed;
                tsDownload.IsOn = false;
                tbFileName.Visibility = Visibility.Collapsed;
                tbFileName.Text = "";
                tbSize.Text = "";
                pbProgress.Value = 0;
                txbFileName.Visibility = Visibility.Visible;
                spMainDownloadUI.Visibility = Visibility.Visible;
            }
            else
            {
                CleanUpDownloadLeftOver();
                var file = await folder.CreateFileAsync(txbFileName.Text, CreationCollisionOption.GenerateUniqueName);
                await tempDestinationFile.CopyAndReplaceAsync(file);
                ToastContent content = GenerateToastContent(file.DisplayName + file.FileType + " has been downloaded.");
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
            }
        }

        public static ToastContent GenerateToastContent(string adaptiveText)
        {
            return new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = adaptiveText
                            },
                            new AdaptiveText()
                            {
                                Text = "Successfully Downloaded File.."
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = "ms-appx:///Assets/notifydownload.png",
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                            {
                                new ToastButton("Open Folder", "action=openFolder&userId=49183")
                                {
                                    ActivationType = ToastActivationType.Foreground
                                },
                                new ToastButton("Dismiss", "action=dismiss&userId=49183")
                                {
                                    ActivationType = ToastActivationType.Foreground
                                }
                            }
                },
                Launch = "action=viewFriendRequest&userId=49183"
            };
        }

        private void progressChangedAsync(DownloadOperation downloadOperation)
        {
            string TotalFileSize = "";
            int progress = (int)(100 * ((double)downloadOperation.Progress.BytesReceived / (double)downloadOperation.Progress.TotalBytesToReceive));
            if (downloadOperation.Progress.TotalBytesToReceive / 1024 / 1024 > 0)
            {
                TotalFileSize = downloadOperation.Progress.TotalBytesToReceive / 1024 / 1024 + " Mb";
            }
            else
            {
                TotalFileSize = "Unknown size";
            }
            tbSize.Text = String.Format("{0} Mb of " + TotalFileSize + " Downloaded..", (downloadOperation.Progress.BytesReceived / 1024) / 1024);
            if (progress > 0)
                pbProgress.Value = progress;
            btPause.IsEnabled = true;
            switch (downloadOperation.Progress.Status)
            {
                case BackgroundTransferStatus.Running:
                    {
                        pbProgress.Visibility = Visibility.Visible;
                        btPause.Content = "\uE103";
                        break;
                    }
                case BackgroundTransferStatus.PausedByApplication:
                    {
                        pbProgress.Visibility = Visibility.Collapsed;
                        tbSize.Text = "Downloading Paused";
                        btPause.Content = "\uE102";
                        break;
                    }
                case BackgroundTransferStatus.PausedCostedNetwork:
                    {

                        break;
                    }
                case BackgroundTransferStatus.PausedNoNetwork:
                    {

                        break;
                    }
                case BackgroundTransferStatus.Error:
                    {
                        downloadOperation = null;
                        CleanUpDownloadLeftOver();
                        UtilityClass.MessageDialog("Error Occured while download file, try changing the file name", "Error");
                        break;
                    }
            }
            if (progress >= 100 && downloadOperation != null)
            {
                downloadOperation = null;
                CleanUpDownloadLeftOver(true);
            }
        }

        private void BtPauseDownload_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadFile.Progress.Status != BackgroundTransferStatus.PausedByApplication)
            {
                DownloadFile.Pause();
                tbSize.Text = "Resuming";
                btPause.Content = "\uE103";

            }
            else
            {
                DownloadFile.Resume();
                tbSize.Text = "Downloading Paused";
                btPause.Content = "\uE102";
            }
        }

        private void btHide_Click(object sender, RoutedEventArgs e)
        {
            this.FindName("gdCompactDownloadUI");
            spMainDownloadUI.Visibility = Visibility.Collapsed;
        }
    }
}
