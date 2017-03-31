using GooRestViewer.Helper;
using RT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GooRestViewer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutUs : Page, IPageView
    {
        public AboutUs()
        {
            this.InitializeComponent();
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
                if (this.CanExecuteInitiate(e))
                {
                    if (e.Parameter is NavigationParam)
                    {
                        this.CurrentNavigationParam = e.Parameter as NavigationParam;
                    }
                    Initiate();
                }
                this.RunAsAsync(() => str_about_intro.Begin());



            }
            catch (Exception)
            {

            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {

                this.RunAsAsync(() => str_about_intro.Stop());
                DettachDataTransferInfoEvents();
                App.HideShellProgress();
            }
            catch (Exception)
            {

            }
            base.OnNavigatedFrom(e);
        }

        public bool CanHandleBackButton()
        {
            try
            {



                return false;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

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
                txt_Appversion.Text = AppInfo.Version;
                hyp_Email.Content = AppInfo.EmailAddress;
            }
            catch (Exception ee)
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
                //ShowError(Error);
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

                OverlayGrid.SetVisibility(true);
            }
            catch (Exception)
            {

            }
        }

        public void HideOverLay()
        {
            try
            {
                OverlayGrid.SetVisibility(false);
            }
            catch (Exception)
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

        private void hyp_Email_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                ShowForEmail();
            }
            catch (Exception) { }
        }
        public async Task ShowForEmail()
        {
            try
            {
                var _mail = new EmailMessage();
                _mail.To.Add(new EmailRecipient(AppInfo.EmailAddress));
                _mail.Subject = "Feedback: " + RT.DeviceInfo.FullDeviceName + " | " + AppInfo.Version;
                //_mail.Body = "Your email body...";
                // You can add an attachment that way.
                //em.Attachments.Add(new EmailAttachment(...);

                // Show the email composer.
                await EmailManager.ShowComposeNewEmailAsync(_mail);
            }
            catch (Exception)
            {

            }
        }

        #region Sharing App Info
        public DataTransferManager _DataTransferManager { get; set; }
        private void btn_share_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                InitiateShareAppInfo();
            }
            catch (Exception)
            {

            }
        }
        public void InitiateShareAppInfo()
        {
            try
            {
                App.ShowShellProgress();
                _DataTransferManager = DataTransferManager.GetForCurrentView();
                AttachDataTransferInfoEvents();
                DataTransferManager.ShowShareUI();
            }
            catch (Exception)
            {

            }
        }

        public void AttachDataTransferInfoEvents()
        {
            try
            {
                DettachDataTransferInfoEvents();
                _DataTransferManager.DataRequested += _DataTransferManager_DataRequested;
            }
            catch (Exception)
            {

            }
        }

        public void DettachDataTransferInfoEvents()
        {
            try
            {

                _DataTransferManager.DataRequested -= _DataTransferManager_DataRequested;
            }
            catch (Exception)
            {

            }
        }


        void _DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                args.Request.Data.Properties.Title = AppInfo.AppName;
                args.Request.Data.Properties.Description = AppInfo.AppDescription;
                args.Request.Data.SetApplicationLink(new Uri(AppInfo.AppMarketPlaceLink));
                args.Request.Data.SetWebLink(new Uri(AppInfo.AppMarketPlaceLink));
                App.HideShellProgress();
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}
