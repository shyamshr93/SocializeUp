using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace App6.Models
{
    public sealed partial class ucInkWeb : UserControl
    {
        Symbol CalligraphyPen = (Symbol)0xEDFB;
        Symbol TouchWriting = (Symbol)0xED5F;
        private Compositor _compositor;
        private StorageFile sharefile;
        public ucInkWeb()
        {
            this.InitializeComponent();

            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            // Enter Animation
            UtilityClass.FadeAnimation(_compositor, this, true);
            UtilityClass.FadeAnimation(_compositor, this, false);
            UtilityClass.TranslationAnimation(_compositor, gdMain, false);
            UtilityClass.TranslationAnimation(_compositor, gdMain, true);

            OneTimeSave();
        }

        private async void OneTimeSave()
        {
            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Touch;
            try
            {
                await gdBlur.Blur(5, 0, 0).StartAsync();
            }
            catch (Exception)
            {

            }
        }

        private void Toggle_Custom(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                inkCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                inkCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }
        }

        private async void btSave_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Portable Network Graphics", new List<string>() { ".png" });
            savePicker.SuggestedFileName = "Untitled";
            StorageFile saveFile = await savePicker.PickSaveFileAsync();
            if (saveFile != null)
            {
                await Save_InkedImagetoFile(saveFile);
            }
        }

        private async Task Save_InkedImagetoFile(StorageFile saveFile)
        {
            try
            {
                sharefile = await ApplicationData.Current.LocalFolder.CreateFileAsync("imgshare.png", CreationCollisionOption.ReplaceExisting);
                CachedFileManager.DeferUpdates(sharefile);
                using (var outStream = await sharefile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync("inkSample");
                    CanvasDevice device = CanvasDevice.GetSharedDevice();
                    var image = await CanvasBitmap.LoadAsync(device, file.Path);
                    using (var renderTarget = new CanvasRenderTarget(device, (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, image.Dpi))
                    {
                        using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
                        {
                            ds.Clear(Colors.White);
                            ds.DrawImage(image, new Rect(0, 0, (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight));
                            ds.DrawInk(inkCanvas.InkPresenter.StrokeContainer.GetStrokes());
                        }
                        await renderTarget.SaveAsync(outStream, CanvasBitmapFileFormat.Png);
                    }
                    image.Dispose();
                    Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(sharefile);
                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        if (saveFile != null)
                        {
                            await sharefile.CopyAndReplaceAsync(saveFile);
                            MainPage.ncSettings.objValue = "Photo has been saved successfully!";
                            MainPage.ncSettings.setSettings = "msgNotify";
                            inkCanvas.InkPresenter.StrokeContainer.Clear();
                            await Task.Delay(new TimeSpan(0, 0, 1));
                            this.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception)
            {
                UtilityClass.MessageDialog("An error occured while saving photo, Please try again", "An error occured");
            }
        }

        private void btCopy_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.CopySelectedToClipboard();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void inkToolbar_Loaded(object sender, RoutedEventArgs e)
        {
            var attr = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            attr.Color = Colors.Red;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(attr);
        }

        private async void btShare_Click(object sender, RoutedEventArgs e)
        {
            await Save_InkedImagetoFile(null);
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();
            //dataTransferManager.DataRequested -= DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                DataRequest request = args.Request;
                request.Data.Properties.Title = "Socialize Up";
                List<IStorageItem> lstItems = new List<IStorageItem>();
                lstItems.Add(sharefile);
                request.Data.SetStorageItems(lstItems);
                RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(sharefile);
                request.Data.Properties.Thumbnail = imageStreamRef;
                request.Data.SetBitmap(imageStreamRef);
                //sender.DataRequested -= DataTransferManager_DataRequested;
            }
            catch (Exception)
            {

            }
        }
    }

    public class CalligraphicPen : InkToolbarCustomPen
    {
        public CalligraphicPen()
        {
        }

        protected override InkDrawingAttributes CreateInkDrawingAttributesCore(Brush brush, double strokeWidth)
        {

            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.PenTip = PenTipShape.Circle;
            inkDrawingAttributes.IgnorePressure = false;
            SolidColorBrush solidColorBrush = (SolidColorBrush)brush;

            if (solidColorBrush != null)
            {
                inkDrawingAttributes.Color = solidColorBrush.Color;
            }

            inkDrawingAttributes.Size = new Size(strokeWidth, 2.0f * strokeWidth);
            inkDrawingAttributes.PenTipTransform = System.Numerics.Matrix3x2.CreateRotation(45.0f);

            return inkDrawingAttributes;
        }
    }
}
