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
using GooJsonViewer.Models;
using Windows.ApplicationModel;
using RT;
using Goo;
using System.Collections.ObjectModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GooRestViewer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassStructure : Page, IPageView
    {
        public ClassStructure()
        {
            this.InitializeComponent();
        }

        #region Properties

        public ObservableCollection<GooClass> Classes
        {
            get
            {
                try
                {
                    return CurrentNavigationParam.ParamObjects as ObservableCollection<GooClass>;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { }
        }


        public GooClass SelectedClass
        {
            get
            {
                try
                {
                    return CurrentNavigationParam.ParamObject as GooClass;
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
            }
            catch (Exception)
            {

            }
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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

        private void btn_retry_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {

                this.RunAsAsync(() => this.Initiate());
            }
            catch (Exception)
            {

            }
        }

        #region Retry Control Methods and Properties
        private void cntrl_retry_clicked(object sender, EventArgs e)
        {
            try
            {

                //   else
                {
                    this.RunAsAsync(() => this.Initiate());
                }
            }
            catch (Exception ee)
            {

            }
        }

        private void cntrl_retry_animation_completed(object sender, EventArgs e)
        {
            try
            {

                this.RunAsAsync(() => cntrl_errorMessage.SetMessage((txt_errormessage.Text ?? "")));
            }
            catch (Exception ee)
            {

            }
        }

        public void PromptForRetry(string str_prompt_message = "Oops! click on the button above to retry")
        {
            try
            {
                this.RunAsAsync(() => stck_RetryContainer.SetVisibility(true));
                this.RunAsAsync(() => cntrl_retry.InvokeAnimation());
                txt_errormessage.Text = str_prompt_message;
                //  ClassCount = "";
            }
            catch (Exception ee)
            {


            }
        }

        public void HidePrompt()
        {
            try
            {
                this.RunAsAsync(() => stck_RetryContainer.SetVisibility(false));
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
                IsBusy = false;
                if (CurrentNavigationParam == null)
                {
                    PromptForRetry();
                    return;
                }
                HidePrompt();


            }
            catch (Exception ee)
            {
                PromptForRetry();
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
            //   foreach(var prp in GetType())
        }

        public void Notify(string Property)
        {
            try
            {
                if (PropertyChanged != null) { PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(Property)); }

            }
            catch (Exception)
            {

            }
            //    Model.Notify(Property);
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
            get
            {
                try
                {
                    return OverlayGrid;
                }
                catch (Exception)
                {
                    return null;

                }
            }
            set { }
        }

        public void ShowOverLay()
        {
            try
            {

                //   stck_progress.SetVisibility(value);
                OverlayGrid.SetVisibility(true);
            }
            catch (Exception ee)
            {

            }
        }

        public void HideOverLay()
        {
            try
            {
                OverlayGrid.SetVisibility(false);
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
                stck_progress.SetVisibility(value);
                OverlayGrid.SetVisibility(value);
                _IsBusy = value;
            }
        }
        #endregion

        #region Classes Heirarchy Methods and Properties




        #endregion


    }
}
