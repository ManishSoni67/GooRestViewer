using RT;
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
using System.Reflection;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GooRestViewer.Controls
{
    public sealed partial class CSStructure : UserControl, IPageView, IUserControlView
    {
        public CSStructure()
        {
            this.InitializeComponent();
        }



        #region Properties and Methods of goo.IpageView

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
                // IsOpen = false;
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

            }
            catch (Exception ee)
            {

            }
        }

        public void NotifyAll()
        {
            try
            {
                foreach (var prp in this.GetType().GetRuntimeProperties())
                {
                    Notify(prp.Name);
                }
            }
            catch (Exception ee)
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
            catch (Exception ee)
            {

            }
        }

        NavigationEventArgs IPageView.CurrentOnNavigatedToEventArg
        {
            get;
            set;
        }

        NavigationParam IPageView.CurrentNavigationParam
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
                Notify("IsBusy");
            }
        }


        bool _IsCollapsed = true;
        public bool IsCollapsed
        {
            get
            {
                return _IsCollapsed;
            }
            set
            {
                _IsCollapsed = value;
                if (value)
                {
                    //   this.RunAsAsync(() => stck_Prop_options.Visibility = Visibility.Visible);
                    //  this.RunAsAsync(() => lst_Props.Visibility = Visibility.Collapsed);
                    this.RunAsAsync(() => str_ContentCollapse.Begin());
                }
                else
                {

                    // this.RunAsAsync(() => stck_Prop_options.Visibility = Visibility.Collapsed);
                    // this.RunAsAsync(() => lst_Props.Visibility = Visibility.Visible);
                    this.RunAsAsync(() => str_ContentExpand.Begin());
                }
                Notify("IsCollapsed");
                Notify("IsExpanded");
            }
        }
        public bool IsExpanded { get { return !IsCollapsed; } set { IsCollapsed = !value; } }
        #endregion

        #region Properties and Methods of IUserControlView
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

        #endregion


        private void Btn_CollapseExpand(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                IsCollapsed = !IsCollapsed;
            }
            catch (Exception ee)
            {

            }
        }

        private void str_ContentExpand_Completed(object sender, object e)
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }

        private void str_ContentCollapse_Completed(object sender, object e)
        {
            try
            {

            }
            catch (Exception ee)
            {

            }
        }
        private void btn_PropType_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                //   if (JSonViewer != null) { JSonViewer.btn_PropType_Click(sender, e); }
            }
            catch (Exception ee)
            {

            }
        }

        private void btn_Class_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }
    }
}
