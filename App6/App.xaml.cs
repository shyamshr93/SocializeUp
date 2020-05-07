using App6.Models;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Security.Credentials.UI;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App6
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            if(SystemInformation.DeviceFamily != "Windows.Mobile")
            {
                //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Windows Phone 10.0; Android 4.2.1; Microsoft; Lumia 950) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Mobile Safari/537.36 Edge/14.14263");
                //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
            }
            //UserAgentHelper.SetDefaultUserAgent("Mozilla/5.0 (Linux; Android 8.0.0; Pixel 2 XL Build/OPD1.170816.004) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4023.0 Mobile Safari/537.36");
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }


        protected override async void OnActivated(IActivatedEventArgs e)
        {
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                if (args != null)
                {
                    if (args["action"] == "openFolder")
                    {
                        if (StorageApplicationPermissions.FutureAccessList.ContainsItem("DownloadFolderToken"))
                        {
                            StorageFolder folders = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("DownloadFolderToken");
                            try
                            {
                                await Launcher.LaunchFolderAsync(folders);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            await Launcher.LaunchFolderAsync(KnownFolders.SavedPictures);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        Frame rootFrame;
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            UnhandledException += App_UnhandledException;
            rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.Kind == ActivationKind.Launch && e.Arguments != null && e.Arguments != "")
                {
                    //PinPageHelper(e);
                }
            }
            if (rootFrame.Content == null)
            {
                PinPageHelper(e);
            }
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async void PinPageHelper(LaunchActivatedEventArgs e)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("cbExtLinks"))
            {
                UtilityData.isFirstAppRun = true;
                rootFrame.Navigate(typeof(OOBE), e.Arguments);
                return;
            }
            bool isWindows = false;
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("WindowsHello"))
                {
                    if ((bool)ApplicationData.Current.LocalSettings.Values["WindowsHello"] == true)
                    {
                        var result = await UserConsentVerifier.CheckAvailabilityAsync();
                        if (result == UserConsentVerifierAvailability.Available)
                        {
                            var verifiedResult = await UserConsentVerifier.RequestVerificationAsync("Just checking that you are really you :)");

                            if (verifiedResult == UserConsentVerificationResult.Verified)
                            {
                                rootFrame.Navigate(typeof(MainPage), e.Arguments);
                                isWindows = true;
                            }
                            else if (verifiedResult == UserConsentVerificationResult.Canceled)
                            {
                                Application.Current.Exit();
                            }
                        }
                    }
                }
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("defaultPin") && !isWindows)
                {
                    if ((bool)ApplicationData.Current.LocalSettings.Values["defaultPin"] == true)
                    {
                        string pinString = "";
                        try
                        {
                            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Password"))
                            {
                                StorageFile sampleFile = await ApplicationData.Current.LocalFolder.GetFileAsync("data.txt");
                                ApplicationData.Current.LocalSettings.Values["Password"] = await FileIO.ReadTextAsync(sampleFile);
                            }
                            pinString = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
                        }
                        catch (Exception)
                        {

                        }
                        if (pinString != "")
                            rootFrame.Navigate(typeof(PinUI), e.Arguments);
                        else
                            rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
                    else
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);

                }
                else
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            catch (Exception)
            {
                rootFrame.Navigate(typeof(MainPage), e?.Arguments);
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        
    }
}
