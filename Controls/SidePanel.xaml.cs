using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using GooRestViewer.Helper;
using Windows.ApplicationModel.Email;
using System.Threading.Tasks;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GooRestViewer.Controls
{
    public sealed partial class SidePanel : UserControl, IUserControlView
    {
        public SidePanel()
        {
            this.InitializeComponent();
            Initiate();

        }

        #region Properties and Methods of IUserControlView
        public void Initiate()
        {
            try
            {
                txt_Appversion.Text = AppInfo.Version;
            }
            catch (Exception)
            {

            }
        }

        private bool _IsOpen = false;
        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
            set
            {
                try
                {

                    _IsOpen = value;

                    if (value)
                    {
                        this.SetVisibility(value);
                        this.RunAsync(() => Str_TransitionIn.Begin());
                    }
                    else
                    {
                        this.RunAsync(() => Str_TransitionOut.Begin());
                    }
                    RaiseTransitionStarting();
                }

                catch (Exception ee)
                {

                }


            }
        }
        public event EventHandler TransitionInCompleted;

        public void RaiseTransitionInCompleted()
        {
            if (TransitionInCompleted != null)
            {
                TransitionInCompleted(this, new EventArgs());
            }
        }

        public event EventHandler TransitionOutCompleted;

        public void RaiseTransitionOutCompleted()
        {
            if (TransitionOutCompleted != null)
            {
                TransitionOutCompleted(this, new EventArgs());
            }
        }

        public void SetIsOpen(bool _IsOpen)
        {
            IsOpen = _IsOpen;
        }

        public event EventHandler TransitionStarting;

        public void RaiseTransitionStarting()
        {
            if (TransitionStarting != null)
            {
                TransitionStarting(this, new EventArgs());
            }
        }
        private void Transition_InCompleted(object sender, object e)
        {
            // IsOpen = false;
            RaiseTransitionInCompleted();
        }

        private void Transition_OutCompleted(object sender, object e)
        {
            try
            {

                this.SetVisibility(IsOpen);
                RaiseTransitionOutCompleted();
            }
            catch (Exception)
            {

            }
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
            catch (Exception)
            {

            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void URI_Builder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedirectToURIBuilder();
            }
            catch (Exception)
            {

            }
        }

        private void btn_AboutUs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedirectToAboutUs();
            }
            catch (Exception)
            {

            }
        }

        private void btn_WriteUs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowForEmail();
            }
            catch (Exception)
            {

            }
        }
        private void btn_Review_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowForReviewApp();
            }
            catch (Exception)
            {

            }
        }
        private void btn_SavedURIs_click(object sender, RoutedEventArgs e)
        {
            try
            {
                RedirectToSavedURIs();
            }
            catch (Exception)
            {

            }
        }


        public void RedirectToURIBuilder()
        {
            try
            {
                if (MainPage.Current == null) { return; }

                MainPage.Current.RedirectToURIBuilderView();
            }
            catch (Exception)
            {

            }
        }

        public void RedirectToAboutUs()
        {
            try
            {

                if (MainPage.Current == null) { return; }

                MainPage.Current.RedirectToAboutUs();
            }
            catch (Exception)
            {

            }
        }

        public void RedirectToSavedURIs()
        {
            try
            {

                if (MainPage.Current == null) { return; }

                MainPage.Current.RedirectToSavedURIs();
            }
            catch (Exception)
            {

            }
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

        public async Task ShowForReviewApp()
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + AppInfo.AppGUID));
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
