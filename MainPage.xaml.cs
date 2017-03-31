using GooJsonViewer.Models;
using GooRestViewer.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RT;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using RT.Utilities;
using GooRestViewer.Models;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace GooRestViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IPageView
    {

        #region Static Properties and Methods
        public static MainPage Current { get; private set; }

        public static void Redirect(bool _CanRefresh, string _URL)
        {
            try
            {
                if (App.RootFrame == null) { return; }
                var BackStacks = App.RootFrame.BackStack.ToList();
                while (BackStacks.FirstOrDefault(x => x.SourcePageType.Equals(typeof(MainPage))) == null)
                {
                    App.RootFrame.BackStack.Remove(App.RootFrame.BackStack.FirstOrDefault());
                }

                if (App.RootFrame.CanGoBack)
                {
                    App.RootFrame.GoBack();
                }
                if (_CanRefresh)
                {
                    if (Current != null)
                    {
                        Current.Initiate();
                        Current.Url = _URL;
                        if (Current.GetIfSideMenuOpen())
                        {
                            Current.CloseSideMenu();
                        }
                        Current.InvokeWebRequest(_URL);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //Model.ContentLoaded += Model_ContentLoaded;
            //Model.RaiseErrorOccured += Model_RaiseErrorOccured;
        }



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // ShowError((e.Parameter ?? "").ToString());
                if (this.CanExecuteInitiate(e))
                {
                    Initiate();
                    if (e.Parameter is NavigationParam)
                    {
                        this.CurrentNavigationParam = e.Parameter as NavigationParam;
                        if (!string.IsNullOrWhiteSpace((CurrentNavigationParam.StrData ?? "").ToString()))
                        {
                            SetForFileActivation((CurrentNavigationParam.StrData ?? "").ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                //   Model.IsBusy = false;
            }
            catch (Exception ee)
            {

            }
            base.OnNavigatedFrom(e);
        }

        public bool CanHandleBackButton()
        {
            try
            {
                if (((app_Fullscreen.Content as Image).Source as BitmapImage).UriSource.ToString().Equals("ms-appx:/Icons/Minimize.png"))
                {
                    MinimizeContents();
                    return true;
                }


                if (GetIfSideMenuOpen())
                {
                    CloseSideMenu();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Command Bar UIElement Events
        private void App_btn_Classes_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                //IsBusy = true;
                if (GetIfSideMenuOpen())
                {
                    this.RunAsAsync(() => CloseSideMenu());
                }
                RedirectToJsonClassView();

            }
            catch (Exception)
            {

            }
            // Uri PageUri = new Uri("/Views/JsonClassView.xaml?Data=" + Model.JsonContent, UriKind.Relative);


            //NavigationService.Navigate(PageUri);
            // TODO: Add event handler implementation here.
        }
        public void RedirectToJsonClassView()
        {
            try
            {
                this.Frame.Navigate(typeof(JsonClassView), new NavigationParam() { ParamObject = null, ParamObjects = null, NavigationFrom = this.ToString(), StrData = JsonContent });
            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region Scoll Viewer View Changing related methods and Properties
        //double LastScrollViewerVerticleOffset;
        //double DeltaValue;

        //bool IsPossitive;
        //bool IsNegative;
        private void scr_content_ViewChanged(object sender, Windows.UI.Xaml.Controls.ScrollViewerViewChangedEventArgs e)
        {
            //try
            //{
            //    var ScrollView = sender as ScrollViewer;
            //    var _DeltaValue = ScrollView.VerticalOffset - LastScrollViewerVerticleOffset;
            //    //if (DeltaValue.Equals())
            //    if (_DeltaValue == 0 || (_DeltaValue + DeltaValue) == 0)
            //    {
            //        return;
            //    }
            //    if (_DeltaValue > 0)
            //    {
            //        //Possitive
            //        //if (IsNegative) { return; }
            //        IsPossitive = true; IsNegative = !IsPossitive;
            //        if (IsPossitive)
            //        {
            //            InvokeAnimationForHidingStringOperations();
            //        }
            //        // return;
            //    }
            //    else if (_DeltaValue < 0)
            //    {
            //        //Negative
            //        //     if (IsPossitive) { return; }
            //        IsPossitive = false; IsNegative = !IsPossitive;
            //        if (IsNegative)
            //        {
            //            InvokeAnimationForStringOperations();
            //        }
            //        //  return;
            //    }
            //    LastScrollViewerVerticleOffset = ScrollView.VerticalOffset;
            //    DeltaValue = ScrollView.VerticalOffset - LastScrollViewerVerticleOffset;
            //    Logger.CreateLog(DeltaValue.ToString());
            //    Logger.CreateLog(LastScrollViewerVerticleOffset.ToString());
            //}
            //catch (Exception ee)
            //{

            //}
        }
        #endregion


        #region Maximize and Minimize related properties and Methods
        private void app_Fullscreen_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if ((((sender as Button).Content as Image).Source as BitmapImage).UriSource.ToString().Equals("ms-appx:/Icons/FullScreen.png"))
                {
                    MaximizeContents();
                }
                else
                {
                    MinimizeContents();
                }
            }
            catch (Exception)
            {

            }
        }

        public void MaximizeContents()
        {
            try
            {
                this.RunAsAsync(() => stck_editoptions.Visibility = Visibility.Collapsed);
                this.RunAsAsync(() => str_ContentFullScreen.Begin());
                this.RunAsAsync(() => (app_Fullscreen.Content as Image).Source = new BitmapImage(new Uri((app_Fullscreen.Content as Image).BaseUri, "/Icons/Minimize.png")));


            }
            catch (Exception)
            {

            }
        }
        public void MinimizeContents()
        {
            try
            {
                this.RunAsAsync(() => stck_editoptions.Visibility = Visibility.Visible);
                this.RunAsAsync(() => str_ContentMinimizescreen.Begin());
                this.RunAsAsync(() => (app_Fullscreen.Content as Image).Source = new BitmapImage(new Uri((app_Fullscreen.Content as Image).BaseUri, "/Icons/FullScreen.png")));


            }
            catch (Exception)
            {

            }
        }
        #endregion


        #region Properties and Methods related to IPageView Interface

        public bool HaveNetwork
        {
            get
            {
                return RT.NetworkInfo.IsNetworkAvailable;
            }
            set
            {

            }
        }

        public string Status
        {
            get;
            set;
        }

        public bool CanDoAnlytics
        {
            get;
            set;
        }

        public void Initiate()
        {
            try
            {
                Current = this;
#if DEBUG
                Url = "http://data.in.bookmyshow.com/getToken2.aspx?f=json&ac=MOBWP82&fl=|UDID=tdTXjQHsrJSpsLBYI+caZIs/w1I=|PUSHTOKEN=http://am3.notify.live.net/throttledthirdparty/01.00/AQGWuxJeJf3qSI7OLnuLhMC7AgAAAAADAQAAAAQUZm52OkJCMjg1QTg1QkZDMkUxREQFBkVVV0UwMQ|FL=MOBAPP_IOSAND3|DEVICE=Microsoft-XDeviceEmulator|&av=13&sv=0&pt=WIN";
#endif
                this.TryHideCommandbar();

            }
            catch (Exception)
            {

            }
        }

        public void DoAnalytics()
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }

        public void UpdateError(string Error)
        {
            try
            {
                ShowError(Error);
            }
            catch (Exception ee)
            {

            }
        }

        public void NotifyAll()
        {
            IPageViewManager.NotifyAll(this);
        }

        public void Notify(string Property)
        {
            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(Property));
                }
            }
            catch
            {

            }
        }

        public NavigationEventArgs CurrentOnNavigatedToEventArg
        {
            get;
            set;
        }

        public NavigationParam CurrentNavigationParam
        {
            get;
            set;
        }

        public UIElement OverlayElement
        {
            get;
            set;
        }

        public void ShowOverLay()
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }

        public void HideOverLay()
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OverlayGrid.SetVisibility(value);
                stck_progress.SetVisibility(value);
                Notify("IsBusy");
            }
        }
        #endregion

        #region Rest Output Parser Related Methods and Properties
        public string Url
        {
            get { return txt_WebUrl.Text ?? ""; }
            set
            {
                txt_WebUrl.Text = value;
                Notify("Url");
            }
        }
        public bool IsUrlValid
        {
            get
            {
                try
                {
                    return true;
                    //  Uri ResultUri = new Uri(Url, UriKind.RelativeOrAbsolute);
                    // return Uri.TryCreate(Url, UriKind.Absolute, out ResultUri) && Uri.IsWellFormedUriString(Url, UriKind.RelativeOrAbsolute);
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            set { }
        }
        int _Progress;
        public int Progress
        {
            get { return _Progress; }
            set
            {
                _Progress = value;
                Notify("Progress");
            }
        }
        string _JsonContent;
        public string JsonContent
        {
            get { return _JsonContent; }
            set
            {
                _JsonContent = value;
                Notify("JsonContent");
            }
        }
        public string JsonContentstrLength
        {
            get
            {
                var Length = (JsonContent ?? "").Length;
                if (Length > 2000)
                {
                    return "2000+";
                }
                return Length.ToString();
            }
            set { }
        }

        public async Task InvokeWebRequest(string _Url)
        {

            if (this.IsUrlValid && !(string.IsNullOrWhiteSpace(_Url)))
            {
                try
                {
                    JsonContent = "";
                    //   IsBusy = true;
                    IsBusy = true;
                    var Data = await RT.ExWebRequest.GetWebRequest(new Uri(_Url, UriKind.RelativeOrAbsolute), this);
                    if (Data.HasError)
                    {
                        // HasError = true;
                        Status = Data.ErrorMessage;
                        //  RaiseRaiseErrorOccured();
                        ShowError(Status);
                        HideContentControl();
                    }
                    else
                    {
                        // HasError = false;
                        JsonContent = WebUtility.HtmlDecode(WebUtility.UrlDecode(Data.Feedback));
                        ShowContentControl();
                        this.RunAsAsync(() => GooRestViewer.Data.URIDataManager.SaveURI(new MURI() { StrURI = _Url }));
                    }
                }
                catch (Exception)
                {

                    Status = RT.Utilities.ErrorMessages.DefaultErrorMessage;
                    //  RaiseRaiseErrorOccured();
                    ShowError(Status);
                    HideContentControl();
                }

                //NotifyAll();
                IsBusy = false;

            }

            else
            {
                IsBusy = false;
                // HasError = true;
                Status = (!this.IsUrlValid) ? ErrorMessages.InvalidURLMessage : ErrorMessages.EmptyURLMessage;
                ShowError(Status);
                HideContentControl();
            }
        }



        #endregion

        #region Rest Output View Manipulation

        private async void btn_MakeWebRequest_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {

                HideContentControl();
                IsBusy = true;
                await InvokeWebRequest(Url);
            }
            catch (Exception ee)
            {
                IsBusy = false;
            }
        }
        public void SetForFileActivation(string Data)
        {
            try
            {
                //Model.HasError = false;
                JsonContent = WebUtility.HtmlDecode(Data);
                //  Model.NotifyAll();
                // Model.RaiseContentLoaded();
                ShowContentControl();
                MaximizeContents();
            }
            catch (Exception)
            {
                HideContentControl();
                ShowError(null);
            }
        }
        private void Str_brd_JsonContentcountTransitionIn_Completed(object sender, object e)
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }

        private void Str_brd_JsonContentcountTransitionOut_Completed(object sender, object e)
        {
            try
            {

                this.RunAsAsync(() => grd_options.Visibility = Visibility.Collapsed);
            }
            catch (Exception ee)
            {

            }
        }
        public void ShowContentControl()
        {
            try
            {
                this.TryShowCommandBar();
                scr_content.SetVisibility(true);
                this.RunAsAsync(() => InvokeAnimationForStringOperations());
                txt_StringLength.Text = JsonContentstrLength;
            }
            catch (Exception ee)
            {

            }
        }
        public void HideContentControl()
        {
            try
            {
                this.TryHideCommandbar();
                scr_content.SetVisibility(false);
                grd_options.SetVisibility(false);
            }
            catch (Exception ee)
            {

            }
        }

        public void InvokeAnimationForStringOperations()
        {
            try
            {
                this.RunAsAsync(() => grd_options.Visibility = Visibility.Visible);
                this.RunAsAsync(() => Str_brd_JsonContentcountTransitionIn.Begin());
            }
            catch (Exception ee)
            {

            }
        }
        public void InvokeAnimationForHidingStringOperations()
        {
            try
            {
                this.RunAsAsync(() => Str_brd_JsonContentcountTransitionOut.Begin());
            }
            catch (Exception ee)
            {

            }
        }

        public void ShowError(string _Message)
        {
            try
            {
                _Message = string.IsNullOrWhiteSpace(_Message) ? RT.Utilities.ErrorMessages.DefaultErrorMessage : _Message;

                cntrl_errorMessage.SetMessage(_Message);
            }
            catch (Exception ee)
            {

            }
        }
        public void HideError()
        {
            try
            {
                cntrl_errorMessage.IsOpen = false;
            }
            catch (Exception ee)
            {

            }
        }

        #endregion

        #region SidePanel View Based Properties and Methods
        public bool GetIfSideMenuOpen()
        {

            return Grd_Navigation_Drawer.GetVisibility();
        }

        public void OpenSideMenu()
        {
            try
            {
                Grd_Navigation_Drawer.SetVisibility(true);
                this.RunAsAsync(() => cntr_SidePanel.SetIsOpen(true));
            }
            catch (Exception)
            {

            }
        }
        public void CloseSideMenu()
        {
            try
            {


                this.RunAsAsync(() => cntr_SidePanel.SetIsOpen(false));
            }
            catch (Exception)
            {

            }
        }


        private void cntr_SidePanel_TransitionOutCompleted(object sender, System.EventArgs e)
        {
            try
            {
                Grd_Navigation_Drawer.SetVisibility(false);
            }
            catch (Exception)
            {

            }
        }

        private void btn_Menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenSideMenu();
            }
            catch (Exception)
            {

            }
        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                if (GetIfSideMenuOpen()) { CloseSideMenu(); }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Redirection To Uri Builder
        private void btn_Url_Builder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedirectToURIBuilderView();
            }
            catch (Exception) { }
        }
        private void btn_Saved_URIs_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {

                RedirectToSavedURIs();
            }
            catch (Exception)
            {

            }
        }
        public void RedirectToURIBuilderView()
        {
            try
            {
                this.Frame.Navigate(typeof(URIBuilder), new NavigationParam() { ParamObject = null, ParamObjects = null, NavigationFrom = this.ToString(), StrData = (this.Url) });
            }
            catch (Exception)
            {

            }
        }

        public void RedirectToAboutUs()
        {
            try
            {
                this.Frame.Navigate(typeof(AboutUs), new NavigationParam() { ParamObject = null, ParamObjects = null, NavigationFrom = this.ToString(), StrData = "" });
            }
            catch (Exception)
            {

            }
        }
        public void RedirectToSavedURIs()
        {
            try
            {
                this.Frame.Navigate(typeof(SavedURIs), new NavigationParam() { ParamObject = null, ParamObjects = null, NavigationFrom = this.ToString(), StrData = "" });
            }
            catch (Exception)
            {

            }
        }
        #endregion

    }
}
