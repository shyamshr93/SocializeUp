using App6.Models;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Imaging;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App6
{
    public class UtilityClass
    {
        // Title Bar
        private static void setTitleBarColor(Color HexColor)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titlebar = ApplicationView.GetForCurrentView().TitleBar;

                if (titlebar != null && HexColor != null)
                {
                    titlebar.BackgroundColor = HexColor;
                    titlebar.ButtonBackgroundColor = Colors.Transparent;
                    titlebar.InactiveBackgroundColor = HexColor;
                    titlebar.ButtonInactiveBackgroundColor = HexColor;
                    //titlebar.ButtonForegroundColor = UtilityData.isFluentDesign ? Colors.White : Colors.Black;

                }
            }
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null && HexColor != null)
                {
                    statusBar.BackgroundColor = HexColor;
                    statusBar.BackgroundOpacity = 1;
                }
            }
        }

        public static void titlebar(SolidColorBrush scbBrush)
        {
            if (scbBrush != null)
                setTitleBarColor(scbBrush.Color);
        }

        public static void titlebar(string hex)
        {
            if (hex != null && hex != "")
                setTitleBarColor(ConvertStringToColor(hex));
        }

        public static async Task<BitmapSource> CreateScaledBitmapFromStreamAsync(WebView web, IRandomAccessStream source)
        {
            int height = Convert.ToInt32(web.ActualHeight);
            int width = Convert.ToInt32(web.ActualWidth);
            WriteableBitmap bitmap = new WriteableBitmap(width, height);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(source);
            BitmapTransform transform = new BitmapTransform();
            transform.ScaledHeight = (uint)height;
            transform.ScaledWidth = (uint)width;
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);
            pixelData.DetachPixelData().CopyTo(bitmap.PixelBuffer);
            var savefile = await ApplicationData.Current.LocalFolder.CreateFileAsync("inkSample", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await savefile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                Stream pixelStream = bitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }
            return bitmap;
        }

        //public static async void OpenExternalLink(WebView web, Uri Uri)
        //{
        //    switch (UtilityData.LinksSetting)
        //    {
        //        case 3:
        //            var dialog = MessageDialogBox("Open Links in", "Choose", "In Same Window", "External", "New Instance");
        //            var res = await dialog.ShowAsync();
        //            switch ((int)res.Id)
        //            {
        //                case 0:
        //                    web.Navigate(Uri);
        //                    break;
        //                case 1:
        //                    IAsyncOperation<bool> d = Launcher.LaunchUriAsync(Uri);
        //                    break;
        //                case 2:
        //                    MultipleInstance(Uri);
        //                    break;

        //            }
        //            break;
        //        case 0:
        //            IAsyncOperation<bool> b = Launcher.LaunchUriAsync(Uri);
        //            break;
        //        case 1:
        //            web.Navigate(Uri);
        //            break;
        //        case 2:
        //            MultipleInstance(Uri);
        //            break;
        //    }
        //}

        public static async void ApplyStyle(WebView web, string style)
        {
            style = style.Replace("\r", string.Empty).Replace("\n", string.Empty);
            try
            {
                await web.InvokeScriptAsync("eval", new[] { "javascript:function addStyleString(str) { var node = document.createElement('style'); node.innerHTML = " +
                "str; document.body.appendChild(node); } addStyleString('" + style + "');" });
            }
            catch (Exception)
            {
            }
        }


        public static async Task<bool> PinSecondaryTile(string imageUri, string tileId, string DisplayName, string argument, Color color, FrameworkElement sender)
        {
            if (SecondaryTile.Exists(tileId))
            {
                SecondaryTile tile = (await SecondaryTile.FindAllAsync()).FirstOrDefault((t) => t.TileId == tileId);
                if (tile != null) await tile.RequestDeleteAsync();
                MainPage.ncSettings.objValue = false;
                MainPage.ncSettings.setSettings = "PinToStart";
                return false;
            }
            Uri logo = new Uri("ms-appx:///" + imageUri);
            SecondaryTile secTile = new SecondaryTile(tileId, DisplayName,
                                                        argument, logo, TileSize.Square150x150);
            secTile.VisualElements.BackgroundColor = color;
            secTile.VisualElements.ForegroundText = ForegroundText.Light;
            secTile.VisualElements.ShowNameOnSquare150x150Logo = true;

            bool isPinned = await secTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Placement.Below);
            if (isPinned)
            {
                MainPage.ncSettings.objValue = isPinned;
                MainPage.ncSettings.setSettings = "PinToStart";
            }
            return isPinned;
        }

        public static Windows.UI.Color ConvertStringToColor(String hex)
        {
            hex = hex.Replace("#", "");
            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            int start = 0;
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Windows.UI.Color.FromArgb(a, r, g, b);
        }

        public static async void MultipleInstance(Uri targeturiWeb)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                Grid grid = new Grid();
                grid.Children.Clear();

                WebView web = new WebView();
                ProgressRing pr = new ProgressRing
                {
                    Height = 50,
                    Width = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsActive = true,
                    Visibility = Visibility.Visible,
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Crimson)
                };
                grid.Children.Add(web);
                grid.Children.Add(pr);
                web.Navigate(targeturiWeb);
                web.ContentLoading += (s, a) => { pr.Visibility = Visibility.Visible; };
                web.LoadCompleted += (s, a) => { pr.Visibility = Visibility.Collapsed; };
                Window.Current.Content = grid;
                Window.Current.Activate();

                newViewId = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Id;
            }
             );
            bool viewShown = await Windows.UI.ViewManagement.ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);

        }

        public static void TranslationAnimation(Compositor _compositor, UIElement _element, bool isExit)
        {
            try
            {

                if (!isExit)
                {
                    var topBorderOffsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    topBorderOffsetAnimation.Duration = TimeSpan.FromSeconds(0.4);
                    topBorderOffsetAnimation.Target = "Translation.Y";
                    topBorderOffsetAnimation.InsertKeyFrame(0, 100.0f);
                    topBorderOffsetAnimation.InsertKeyFrame(1, 0);

                    ElementCompositionPreview.SetIsTranslationEnabled(_element, true);
                    ElementCompositionPreview.GetElementVisual(_element);
                    ElementCompositionPreview.SetImplicitShowAnimation(_element, topBorderOffsetAnimation);
                }
                else
                {
                    var mainContentExitAnimation = _compositor.CreateScalarKeyFrameAnimation();
                    mainContentExitAnimation.Target = "Translation.Y";
                    mainContentExitAnimation.InsertKeyFrame(1, 30);
                    mainContentExitAnimation.Duration = TimeSpan.FromSeconds(0.4);

                    ElementCompositionPreview.SetIsTranslationEnabled(_element, true);
                    ElementCompositionPreview.SetImplicitHideAnimation(_element, mainContentExitAnimation);
                }
            }
            catch (Exception)
            {

            }
        }

        public static void FadeAnimation(Compositor _compositor, UIElement _element, bool fadein)
        {
            try
            {
                if (fadein)
                {
                    // Add an opacity animation that will play when the page exits the scene
                    var fadeIn = _compositor.CreateScalarKeyFrameAnimation();
                    fadeIn.Target = "Opacity";
                    fadeIn.InsertKeyFrame(0, 1);
                    fadeIn.Duration = TimeSpan.FromSeconds(0.4);

                    // Set Z index to force this page to the top during the hide animation
                    Canvas.SetZIndex(_element, 1);
                    ElementCompositionPreview.GetElementVisual(_element);
                    ElementCompositionPreview.SetImplicitShowAnimation(_element, fadeIn);
                }
                else
                {
                    var fadeOut = _compositor.CreateScalarKeyFrameAnimation();
                    fadeOut.Target = "Opacity";
                    fadeOut.InsertKeyFrame(1, 0);
                    fadeOut.Duration = TimeSpan.FromSeconds(0.4);

                    // Set Z index to force this page to the top during the hide animation
                    Canvas.SetZIndex(_element, 1);
                    ElementCompositionPreview.GetElementVisual(_element);
                    ElementCompositionPreview.SetImplicitHideAnimation(_element, fadeOut);
                }
            }
            catch (Exception)
            {

            }
        }


        public static void Notify(string notificationText, string image, string secondLine = "")
        {
            try
            {
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText02;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("src", string.Format("ms-appx:///Assets/{0}", image));
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(notificationText));
                toastTextElements[1].AppendChild(toastXml.CreateTextNode(secondLine));
                ToastNotification toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            catch (Exception)
            {

            }
        }


        public static async Task<string> serverStyle(string _uri, string flName, string destName)
        {
            string cssToApply = "", bckcssToApply = "";
            StorageFile destinationFile = null, _destfile = null;

            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Style", CreationCollisionOption.OpenIfExists);
            var flOneDrive = await folder.CreateFolderAsync(flName, CreationCollisionOption.OpenIfExists);
            try
            {
                _destfile = await flOneDrive.GetFileAsync(destName);
            }
            catch
            {
            }

            if (_destfile != null)
                bckcssToApply = await FileIO.ReadTextAsync(_destfile);

            destinationFile = await flOneDrive.CreateFileAsync(destName, CreationCollisionOption.ReplaceExisting);
            try
            {
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation DownloadFile = downloader.CreateDownload(new Uri(_uri), destinationFile);
                await DownloadFile.StartAsync();
                //cssToApply = await FileIO.ReadTextAsync(destinationFile);
                //cssToApply = cssToApply.Replace("\r", string.Empty).Replace("\n", string.Empty);
            }
            catch (Exception)
            {
                destinationFile = await flOneDrive.GetFileAsync(destName);
            }
            if (destinationFile != null)
            {
                if (!cssToApply.Contains("<!doctype html>"))
                {
                    cssToApply = await FileIO.ReadTextAsync(destinationFile);
                }
                else
                {
                    cssToApply = bckcssToApply;
                }
                await FileIO.WriteTextAsync(destinationFile, "");
                //cssToApply = cssToApply.Replace("\r", string.Empty).Replace("\n", string.Empty);
            }
            return cssToApply;
        }

        private void NotifyTile(string notificationText, string image)
        {
            try
            {

                TileTemplateType tileTemplate = TileTemplateType.TileSquare150x150PeekImageAndText04;
                XmlDocument tileXml = TileUpdateManager.GetTemplateContent(tileTemplate);
                XmlNodeList tileTextElements = tileXml.GetElementsByTagName("text");
                tileTextElements[0].AppendChild(tileXml.CreateTextNode(notificationText));
                XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");
                ((XmlElement)tileImageAttributes[0]).SetAttribute("src", image);
                TileNotification tile = new TileNotification(tileXml);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tile);
            }
            catch (Exception)
            {

            }

        }

        public static ContentDialog NewMessageDialogBox(string content, string title, string First, string Second)
        {
            ContentDialog cntMessage = new ContentDialog
            {
                Title = title,
                PrimaryButtonText = First,
                Content = content,
                SecondaryButtonText = Second
            };
            return cntMessage;
        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        public static int ConvertToInt(String input)
        {
            String inputCleaned = System.Text.RegularExpressions.Regex.Replace(input, "[^0-9]", "");
            int value = 0;
            if (int.TryParse(inputCleaned, out value))
            {
                return value;
            }

            return 0;
        }

        public static MessageDialog MessageDialogBox(string content, string title, string First, string Second, string Third)
        {
            var dialog = new MessageDialog(content, title);

            dialog.Commands.Add(new UICommand(First) { Id = 0 });
            dialog.Commands.Add(new UICommand(Second) { Id = 1 });
            if (Third != null)
            {
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    dialog.Commands.Add(new UICommand(Third) { Id = 2 });
                }
            }

            dialog.DefaultCommandIndex = 0;
            return dialog;
        }

        public static object ApplicationDataBool(string strName, object nullObj)
        {
            object obj = new object();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(strName))
            {
                obj = ApplicationData.Current.LocalSettings.Values[strName];
            }
            else
            {
                obj = null;
                obj = nullObj;
            }
            return obj;
        }

        public async static void MessageDialog(string Content, string Title, string PrimaryText = "Cancel")
        {
            ContentDialog cnt = new ContentDialog
            {
                Title = Title,
                Content = Content,
                PrimaryButtonText = PrimaryText
            };
            await cnt.ShowAsync();
        }


        public static async Task<string> retrieveWebPhotoLink(string uri, string service)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();

                var handler = new HttpClientHandler { AllowAutoRedirect = true };
                var client = new HttpClient(handler);
                var response = await client.GetAsync(new Uri(uri));
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();
                string email = html;
                MatchCollection ms = Regex.Matches(html, @"(www.+|http.+)([\s]|$)");
                string testMatch = "";

                foreach (Match item in ms)
                {
                    if (service == "Insta")
                    {
                        if (html.Contains("video_url"))
                        {
                            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            foreach (Match m in linkParser.Matches(html))
                            {
                                if (m.Value.Contains(".mp4"))
                                {
                                    testMatch = m.Value;
                                    testMatch = testMatch.Replace("\"" + " />", " ");
                                    break;
                                }
                            }
                            break;
                        }
                        else if (item.Value.Contains("instagram.fdel8-1.fna.fbcdn.net"))
                        {
                            testMatch = item.Value;
                            testMatch = testMatch.Replace("\"" + " />", " ");
                            break;
                        }
                    }
                    else if (service == "Twitter")
                    {
                        if (item.Value.Contains("pbs.twimg.com/media/"))
                        {
                            testMatch = item.Value;
                            testMatch = testMatch.Replace("small\"" + "/>", "large");
                            break;
                        }
                    }
                    else if (service == "Reddit")
                    {
                        if (item.Value.Contains("https://i.redd.it"))
                        {
                            foreach (Match m in Regex.Matches(item.Value, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
                            {
                                testMatch = m.Value;
                                break;
                            }
                            break;
                        }
                        else if (item.Value.Contains("https://v.redd.it"))
                        {
                            foreach (Match m in Regex.Matches(item.Value, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
                            {
                                if (m.Value.Contains("v.redd.it"))
                                {
                                    testMatch = m.Value;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                return testMatch = testMatch.Replace("\">", " ");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static async Task<string> GetFileName(Uri requestUri)
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            var headers = httpClient.DefaultRequestHeaders;

            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                return System.IO.Path.GetFileName(Uri.UnescapeDataString(requestUri.AbsolutePath.ToString()).Replace("/", "\\"));
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                return System.IO.Path.GetFileName(Uri.UnescapeDataString(requestUri.AbsolutePath.ToString()).Replace("/", "\\"));
            }

            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                string fileName = httpResponse.Content.Headers.ContentDisposition.FileName;
                if (!string.IsNullOrEmpty(fileName) || !string.IsNullOrWhiteSpace(fileName))
                    return Uri.UnescapeDataString(fileName.Replace("/", "\\"));
                else
                    return System.IO.Path.GetFileName(Uri.UnescapeDataString(requestUri.AbsolutePath.ToString()).Replace("/", "\\"));
            }
            catch (Exception ex)
            {
                return System.IO.Path.GetFileName(Uri.UnescapeDataString(requestUri.AbsolutePath.ToString()).Replace("/", "\\"));
            }
        }


        public static async void moveXOffset(UIElement uIElement, float X)
        {
            try
            {
                await uIElement.Offset(offsetX: X, duration: 200).StartAsync();

            }
            catch (Exception)
            {

            }
        }

        public static async void moveYOffset(UIElement uIElement, float Y)
        {
            try
            {
                await uIElement.Offset(offsetY: Y, duration: 200).StartAsync();

            }
            catch (Exception)
            {

            }
        }


    }
    public class ncMainPage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ncMainPage()
        {

        }
        private string Settings;
        private object _objValue;
        public object objValue
        {
            get { return _objValue; }
            set
            {
                _objValue = value;
                //OnPropertyChanged("_objValue");
            }
        }
        public string setSettings
        {
            get { return Settings; }
            set
            {
                Settings = value;
                OnPropertyChanged("Settings");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
