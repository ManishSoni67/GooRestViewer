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
using GooRestViewer.Models;
using System.Collections.ObjectModel;
using RT.Utilities;
using System.Runtime.Serialization.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GooRestViewer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SavedURIs : Page, IPageView
    {
        #region Properties
        public string Key_SavedURIs { get { return "Json_SavedURIs"; } set { } }

        ObservableCollection<MURI> _URIs = new ObservableCollection<MURI>();
        public ObservableCollection<MURI> URIs
        {
            get { return _URIs; }
            set
            {
                _URIs = value;
                Notify("URIs");
            }
        }

        public int URICount
        {
            get
            {
                try
                {
                    if (URIs == null) { return 0; }
                    return URIs.Count;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set { }
        }

        public bool HaveURIs
        {
            get
            {
                try
                {
                    return URICount > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set { }
        }
        #endregion

        public SavedURIs()
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
                    IsAlphabeticallySortChecked = CanSortForAlphabetically;
                    IsDateSortChecked = CanSortForDate;
                    UpdateForegroundForSortButton(btn_SortrecentDate, CanSortForDate);
                    UpdateForegroundForSortButton(btn_SortAplabetically, CanSortForAlphabetically);
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
            catch (Exception)
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
                IsBusy = true; App.ShowShellProgress();
                this.RunAsAsync(() => LoadAllSavedURIs());
            }
            catch (Exception)
            {
                PromptForRetry(ErrorMessages.DefaultErrorMessage);
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

        #region Retry Control Properties and Methods
        private void cntrl_retry_clicked(object sender, EventArgs e)
        {
            try
            {

                this.RunAsAsync(() => this.Initiate());

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
                IsBusy = false;
                App.HideShellProgress();
                this.RunAsAsync(() => stck_RetryContainer.SetVisibility(true));
                this.RunAsAsync(() => cntrl_retry.InvokeAnimation());
                txt_errormessage.Text = str_prompt_message;
            }
            catch (Exception ee)
            {


            }
        }

        public void HidePrompt()
        {
            try
            {
                App.HideShellProgress();
                this.RunAsAsync(() => stck_RetryContainer.SetVisibility(false));
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Roaming Data For SavedURIs
        public void LoadAllSavedURIs()
        {
            try
            {
                if (URIs == null) { URIs = new ObservableCollection<MURI>(); }
                URIs.Clear();
                var Data = GooRestViewer.Data.URIDataManager.GetAllSavedURIs();
                if (Data == null || Data.Count == 0)
                {
                    PromptForRetry(ErrorMessages.NoSaveURIsFound);
                    return;
                }
                if (CanSortForAlphabetically)
                {
                    Data = new ObservableCollection<MURI>(Data.OrderBy(x => x.StrURI));
                }
                if (CanSortForDate)
                {
                    Data = new ObservableCollection<MURI>(Data.OrderByDescending(x => x.CreatedOn));
                }
                URIs = Data;
                foreach (var uri in URIs)
                {
                    uri.ID = URIs.IndexOf(uri) + 1;
                }
                brd_URIsCount.SetVisibility(HaveURIs);
                txt_URIscount.Text = URICount.ToString();
                HidePrompt();

            }
            catch (Exception)
            {
                PromptForRetry(ErrorMessages.DefaultErrorMessage);
            }
            IsBusy = false;
        }


        #endregion

        #region Sort and Refresh Methods and Properties
        public bool CanSortForDate
        {
            get
            {
                try
                {
                    var Data = RT.Storage.GetRoamingValue("Json_CanSortForDate");
                    if (string.IsNullOrWhiteSpace(Data))
                    {
                        return false;
                    }
                    return bool.Parse(Data);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    RT.Storage.SetRoamingValue("Json_CanSortForDate", value.ToString());
                }
                catch (Exception)
                {

                }
            }
        }
        public bool CanSortForAlphabetically
        {
            get
            {
                try
                {
                    var Data = RT.Storage.GetRoamingValue("Json_CanSortForAlphabetically");
                    if (string.IsNullOrWhiteSpace(Data))
                    {
                        return false;
                    }
                    return bool.Parse(Data);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    RT.Storage.SetRoamingValue("Json_CanSortForAlphabetically", value.ToString());
                }
                catch (Exception)
                {

                }
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

        private void btn_sortby_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                UpdateForegroundForSortButton(btn_SortrecentDate, CanSortForDate);
                UpdateForegroundForSortButton(btn_SortAplabetically, CanSortForAlphabetically);
            }
            catch (Exception)
            {

            }
        }

        public bool IsDateSortChecked { get; set; }
        private void btn_SortrecentDate_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                IsDateSortChecked = !IsDateSortChecked;
                IsAlphabeticallySortChecked = false;
                UpdateForegroundForSortButton(sender as Button, IsDateSortChecked);
                UpdateForegroundForSortButton(btn_SortAplabetically, IsAlphabeticallySortChecked);
            }
            catch (Exception)
            {

            }
        }
        public bool IsAlphabeticallySortChecked { get; set; }
        private void btn_SortAplabetically_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                IsAlphabeticallySortChecked = !IsAlphabeticallySortChecked;
                IsDateSortChecked = false;
                UpdateForegroundForSortButton(sender as Button, IsAlphabeticallySortChecked);
                UpdateForegroundForSortButton(btn_SortrecentDate, IsDateSortChecked);
            }
            catch (Exception)
            {

            }
        }

        private void btn_Filter_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                HideSortFlyout();
                CanSortForDate = IsDateSortChecked;
                CanSortForAlphabetically = IsAlphabeticallySortChecked;
                this.RunAsAsync(() => this.Initiate());
            }
            catch (Exception)
            {

            }
        }
        public void UpdateForegroundForSortButton(Button _Button, bool CanSort)
        {
            try
            {
                var _Color = (CanSort) ? App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush : App.Current.Resources["Gray_ForegroundBrush"] as SolidColorBrush;
                _Button.Foreground = _Color;

            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Flyout Based Methods and Properties
        public void ShowSortFlyout()
        {

            try
            {
                fly_Sort.ShowAt(btn_Filter);
            }
            catch (Exception)
            {

            }
        }

        public void HideSortFlyout()
        {

            try
            {
                fly_Sort.Hide();
            }
            catch (Exception)
            {

            }
        }


        #endregion

        #region Make Request to the Web for the URL
        private void btn_makerequest_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                ValidateAndRedirectBackToMainPage(((sender as Button).DataContext as MURI).StrURI);
            }
            catch (Exception)
            {

            }
        }
        public void ValidateAndRedirectBackToMainPage(string _FinalURI)
        {
            try
            {
                Uri _UR = new Uri(_FinalURI);
                MainPage.Redirect(true, _UR.ToString());
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
