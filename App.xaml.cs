using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using RT;
using Windows.UI;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace GooRestViewer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        #region Static Properties
        public static Frame RootFrame
        {
            get
            {
                try
                {
                    return (Window.Current.Content as Frame);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { }
        }
        #endregion

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            AttachBackPressKeyEvents();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            SetShellBarProp();
            Window.Current.Activate();
        }

        protected override async void OnFileActivated(FileActivatedEventArgs args)
        {
            // TODO: Handle file activation

            try
            {

                var data = args;
                string _Data = "";
                foreach (var file in args.Files)
                {

                    if (file is StorageFile)
                    {
                        var FL = file as StorageFile;
                        var FileName = file.Name;
                        var FilePath = file.Path;
                        var Stream = await FL.OpenStreamForReadAsync();
                        if (Stream != null)
                        {
                            using (var str = new StreamReader(Stream))
                            {
                                var Data = await str.ReadToEndAsync();
                                _Data = Data;
                                break;

                            }
                        }
                    }

                }

                Frame rootFrame = Window.Current.Content as Frame;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    // TODO: change this value to a cache size that is appropriate for your application
                    rootFrame.CacheSize = 1;

                    if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        // TODO: Load state from previously suspended application
                    }

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (rootFrame.Content == null)
                {
                    // Removes the turnstile navigation for startup.
                    if (rootFrame.ContentTransitions != null)
                    {
                        this.transitions = new TransitionCollection();
                        foreach (var c in rootFrame.ContentTransitions)
                        {
                            this.transitions.Add(c);
                        }
                    }

                    rootFrame.ContentTransitions = null;
                    rootFrame.Navigated += this.RootFrame_FirstNavigated;

                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    if (!rootFrame.Navigate(typeof(MainPage), new NavigationParam() { ParamObject = null, ParamObjects = null, NavigationFrom = "OnFileActivation", StrData = _Data }))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                //if (!string.IsNullOrWhiteSpace(_Data))
                //{
                //    var frame = Window.Current.Content as Frame;
                //    //  frame.Navigate(typeof(MainPage), _Data);
                //}
                if (rootFrame.Content is MainPage)
                    (rootFrame.Content as MainPage).SetForFileActivation(_Data);

                SetShellBarProp();
                Window.Current.Activate();
            }
            catch (Exception)
            {

            }

            // The number of files received is args.Files.Size
            // The first file is args.Files[0].Name
        }
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            try
            {
                var rootFrame = sender as Frame;
                //    rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
                rootFrame.Navigated -= this.RootFrame_FirstNavigated;
                SetShellBarProp();
            }
            catch (Exception ee) { }
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

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        void DetachBackPressKeyEvents()
        {
            try
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            }
            catch (Exception ee)
            {

            }
        }
        void AttachBackPressKeyEvents()
        {
            try
            {
                DetachBackPressKeyEvents();
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            }
            catch (Exception ee)
            {

            }
        }
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            try
            {
                if (((Window.Current.Content as Frame).Content is IPageView))
                {
                    if (((Window.Current.Content as Frame).Content as IPageView).CanHandleBackButton())
                    {
                        e.Handled = true;
                        return;
                    }
                }

                if ((Window.Current.Content as Frame).CanGoBack)
                {
                    (Window.Current.Content as Frame).GoBack();
                    e.Handled = true;
                    return;
                }
                else
                {
                    Application.Current.Exit();
                    e.Handled = true;
                    return;
                }


            }
            catch (Exception ee)
            {

            }
        }

        public static async Task ShowShellProgress(string strMessage = "loading please wait....")
        {
            try
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusBar.ProgressIndicator.Text = strMessage;
                //Show the status bar
                //  await statusBar.ShowAsync();
                await statusBar.ProgressIndicator.ShowAsync();
            }
            catch (Exception ee)
            {

            }
        }
        public static async Task HideShellProgress()
        {
            try
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                // statusBar.ProgressIndicator.Text = strMessage;
                //Show the status bar
                //  await statusBar.HideAsync();
                await statusBar.ProgressIndicator.HideAsync();
            }
            catch (Exception ee)
            {

            }
        }

        public static async Task SetShellBarProp()
        {
            try
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                if (statusBar == null)
                {
                    return;
                }

                statusBar.BackgroundColor = new SolidColorBrush(Colors.White).Color;
                statusBar.BackgroundOpacity = 1;
                statusBar.ForegroundColor = new SolidColorBrush(Color.FromArgb(255, 75, 75, 75)).Color;
            }
            catch (Exception ee)
            {

            }
        }
    }
}