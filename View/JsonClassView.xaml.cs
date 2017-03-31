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
using Windows.ApplicationModel.Email;
using System.Collections.ObjectModel;
using GooJsonClassParser;
using System.Threading.Tasks;
using GooRestViewer.Helper;
//using Goo;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GooRestViewer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JsonClassView : Page, IPageView
    {
        #region Properties

        public IEnumerable<Controls.ClassView> ClassViews
        {
            get
            {
                try
                {
                    if (grd_Content.Children == null || grd_Content.Children.Count == 0) { return null; }
                    return grd_Content.Children.OfType<Controls.ClassView>();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { }
        }

        public string ClassCount
        {
            get
            {
                try
                {
                    return txt_classcount.Text ?? string.Empty;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    txt_classcount.Text = value;
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        this.RunAsAsync(() => brd_ClassCount.SetVisibility(false));
                    }
                    else
                    {
                        this.RunAsAsync(() => brd_ClassCount.SetVisibility(true));
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion
        public JsonClassView()
        {
            try
            {
                this.InitializeComponent();
                this.NavigationCacheMode = NavigationCacheMode.Required;
                //Model.ContentLoaded -= Model_ContentLoaded;
                //Model.RaiseErrorOccured -= Model_RaiseErrorOccured;
                //Model.ContentLoaded += Model_ContentLoaded;
                //Model.RaiseErrorOccured += Model_RaiseErrorOccured;
            }
            catch (Exception ee) { }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (this.CanExecuteInitiate(e))
                {
                    if (e.Parameter is NavigationParam)
                    {
                        this.CurrentNavigationParam = e.Parameter as NavigationParam;
                        var Data = Convert.ToString(CurrentNavigationParam.StrData ?? "");
                        JsonContent = Data;
                        Initiate();
                    }
                    else
                    {
                        PromptForRetry();
                    }
                }
            }
            catch (Exception ee)
            {
                PromptForRetry();
            }
        }
        public bool CanHandleBackButton()
        {
            try
            {

                if (IsSearchControlVisible())
                {
                    HideSearchControl();
                    return true;
                }

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
                HidePrompt();
                var _clsViews = ClassViews;
                if (_clsViews == null || _clsViews.Count() == 0) { PerformAction(); this.Focus(FocusState.Keyboard); }
                else { ResetSearckKey(); }

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
                Notify("IsBusy");
            }
        }
        #endregion


        #region Rest Generated Class Views based Methods and Properties

        public void LoadToControl()
        {
            try
            {
                HidePrompt();
                grd_Content.Children.Clear();
                foreach (var cls in _Classes)
                {
                    Controls.ClassView _clsView = new Controls.ClassView() { IsOpen = false, JSonViewer = this };
                    grd_Content.Children.Add(_clsView);
                    this.RunAsAsync(() => _clsView.UpdateClassInfo(cls));
                }


                this.RunAsAsync(() => ShowAllClassViewControls());


            }
            catch (Exception ee)
            {

            }
        }

        public void ShowAllClassViewControls(bool UseAnimation = true)
        {
            try
            {
                var _clsViews = ClassViews;
                if (_clsViews == null || _clsViews.Count() == 0) { return; }
                foreach (var cls in _clsViews)
                {
                    if (UseAnimation)
                        this.RunAsAsync(() => cls.IsOpen = true);
                    else
                        this.RunAsAsync(() => cls.SetVisibility(true));
                }

                ClassCount = _clsViews.Count().ToString();

            }
            catch (Exception)
            {

            }
        }

        public void HideAllClassViewControls()
        {
            try
            {
                var _clsViews = ClassViews;
                if (_clsViews == null) { return; }
                foreach (var cls in _clsViews)
                {
                    this.RunAsAsync(() => cls.IsOpen = false);
                }
            }
            catch (Exception)
            {

            }
        }

        public void btn_PropType_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if ((sender as Button).DataContext == null) { return; }
                var Data = (sender as Button).DataContext as GooClass;
                var FirstClass = grd_Content.Children.OfType<Controls.ClassView>().FirstOrDefault(x => (x.ClassInfo.Name ?? "").ToLower().Equals(((sender as Button).Content ?? "").ToString().ToLower()));
                if (FirstClass != null)
                {
                    FirstClass.IsCollapsed = false;
                    this.RunAsAsync(() => scrl_Classes.ChangeView(null, (scrl_Classes.VerticalOffset + FirstClass.ActualHeight), null, false));

                }
            }
            catch (Exception ee)
            {

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
        #endregion

        #region Retry Control Properties and Methods
        private void cntrl_retry_clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsSearchControlVisible())
                {
                    SelectAndFocusSearchTextBox();
                }
                else
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
                ClassCount = "";
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

        #region Searching Properties and Methods
        #region StoryBoard Related Properties and Methods
        public void StartAnimationForSearchControl()
        {
            try
            {
                grd_Search.SetVisibility(true);
                this.RunAsAsync(() => str_Show_Search_Control.Begin());
            }
            catch (Exception)
            {

            }
        }

        public void StartAnimationForHidingSearchControl()
        {
            try
            {

                this.RunAsAsync(() => str_Hide_Search_Control.Begin());
            }
            catch (Exception)
            {

            }
        }
        public void str_Show_Search_Control_Completed(object sender, object e)
        {
            try
            {
                SelectAndFocusSearchTextBox();
            }
            catch (Exception)
            {

            }
        }

        public void str_Hide_Search_Control_Completed(object sender, object e)
        {
            try
            {
                grd_Search.SetVisibility(false);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        public string SearchText
        {
            get
            {
                try
                {
                    return txt_Search_class.Text ?? "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    txt_Search_class.Text = value;
                }
                catch { }
            }
        }


        private void btn_clear_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                ResetSearckKey();
                SelectAndFocusSearchTextBox();
            }
            catch (Exception)
            {

            } // TODO: Add event handler implementation here.
        }
        public void ResetSearckKey(bool UseAnimation = true)
        {
            try
            {
                SearchText = string.Empty;
                ShowAllClassViewControls(UseAnimation);
            }
            catch (Exception)
            {

                //   throw;
            }
        }

        public void SelectAndFocusSearchTextBox()
        {
            try
            {
                txt_Search_class.Focus(FocusState.Keyboard);
                txt_Search_class.SelectAll();
            }
            catch (Exception)
            {

            }
        }
        private void btn_search_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                ShowSearchControl();
            }
            catch (Exception)
            {

            }
        }

        public void ShowSearchControl()
        {
            try
            {
                StartAnimationForSearchControl();
                //SelectAndFocusSearchTextBox();
            }
            catch (Exception)
            {

            }
        }


        public void HideSearchControl()
        {
            try
            {
                StartAnimationForHidingSearchControl();
                this.Focus(FocusState.Keyboard);
                ResetSearckKey(false);

            }
            catch (Exception)
            {

            }
        }

        public bool IsSearchControlVisible()
        {
            try
            {
                return grd_Search.Visibility == Visibility.Visible;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void txt_Search_class_TextChanged(object sender, Windows.UI.Xaml.Controls.TextChangedEventArgs e)
        {
            try
            {
                HidePrompt();
                btn_clear.SetVisibility(!(string.IsNullOrWhiteSpace((sender as TextBox).Text ?? "")));
                if (string.IsNullOrWhiteSpace((sender as TextBox).Text ?? ""))
                {
                    ResetSearckKey();
                    return;
                }

                this.RunAsAsync(() => PerformSearch((sender as TextBox).Text ?? ""));
            }
            catch (Exception)
            {

            }
        }
        public void PerformSearch(string _SearchKey)
        {
            try
            {
                _SearchKey = (_SearchKey ?? "").Trim().ToLower();
                var _clsViews = ClassViews;
                if (_clsViews == null || _clsViews.Count() == 0) { PromptForRetry("Oops! Try resetting the search parameters."); return; }
                var SearchedResults = _clsViews.Where(x => x.ClassInfo != null)
                    .Where(x => (x.ClassInfo.ToString().Trim().ToLower().Contains(_SearchKey)
                    || (x.ClassInfo.Properties.Any(y => (y.ToString().Trim().ToLower().Contains(_SearchKey))))));
                if (SearchedResults == null || SearchedResults.Count() == 0) { PromptForRetry("Oops! Try resetting the search parameters."); return; }
                foreach (var clsview in SearchedResults) { this.RunAsAsync(() => clsview.IsOpen = true); }
                foreach (var clsview in _clsViews.Where(x => !SearchedResults.Contains(x))) { this.RunAsAsync(() => clsview.IsOpen = false); }
                ClassCount = SearchedResults.Count().ToString();

            }
            catch (Exception)
            {
                PromptForRetry("Oops! Try resetting the search parameters.");
            }
        }

        #endregion

        #region Redierction to Class Hirarchies
        private void btn_Class_Hierarchy_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                RedirectToShowClassHierarchy(null);
            }
            catch (Exception)
            {

            }
        }

        public void RedirectToShowClassHierarchy(object Class)
        {
            try
            {

                Frame.Navigate(typeof(View.ClassStructure), new NavigationParam() { ParamObject = Class, ParamObjects = _Classes, NavigationFrom = this.ToString(), });

            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region Rest String To Class Generation Related Methods and Properties

        public async void PerformAction()
        {
            try
            {
                if (_Classes == null)
                    _Classes = new ObservableCollection<GooClass>();

                if (string.IsNullOrWhiteSpace(JsonContent))
                {
                    PromptForRetry();
                    return;
                    //    return new ObservableCollection<GooClass>();
                }

                IsBusy = true;
                App.ShowShellProgress();
                _Classes.Clear();
                var Data = await JsonToClassesProcessor.GetJsonClassProp(JsonContent, false);
                _Classes = new ObservableCollection<GooClass>(Data);
                Notify("HaveClasses");
                if (HaveClasses)
                {
                    this.RunAsAsync(() => LoadToControl());
                }
                else
                {
                    PromptForRetry();
                }
            }
            catch (Exception)
            {
                PromptForRetry();
            }
            App.HideShellProgress();
            IsBusy = false;
        }


        string _JsonContent;
        public string JsonContent
        {
            get { return _JsonContent; }
            set
            {
                _JsonContent = value;
                NotifyAll();
            }

        }

        public bool HaveClasses
        {
            get
            {
                if (_Classes == null)
                {
                    return false;
                }
                return _Classes.Count > 0;
            }
            set { }
        }


        public ObservableCollection<GooClass> _Classes
        {
            get;
            set;
        }


        #endregion

        #region Options Flyout based Menus and Methods

        #region Flyout Based Methods and Properties
        public void ShowSortFlyout()
        {

            try
            {
                fly_Options.ShowAt(btn_options);
            }
            catch (Exception)
            {

            }
        }

        public void HideSortFlyout()
        {

            try
            {
                fly_Options.Hide();
            }
            catch (Exception)
            {

            }
        }


        #endregion
        public void update_expandcollapseContainer()
        {
            try
            {
                btn_ExpandCollapseClasses.Content = ClassViews.Where(x => x.IsCollapsed).Count() > 0 ? "expand all" : "collapse all";
            }
            catch (Exception)
            {

            }
        }

        private void btn_ExpandCollapseClasses_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                HideSortFlyout();
                if (HaveClasses)
                {
                    if (ClassViews.Any(x => x.IsCollapsed))
                    {

                        foreach (var cls in ClassViews.Where(x => x.IsCollapsed))
                        {
                            cls.IsCollapsed = false;
                        }
                    }
                    else
                    {
                        foreach (var cls in ClassViews.Where(x => x.IsExpanded))
                        {
                            cls.IsExpanded = false;
                        }
                    }
                }
                update_expandcollapseContainer();
            }
            catch (Exception)
            {

            }
        }

        private void btn_showClassesMap_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void btn_SendMail_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {

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
                _mail.Subject = "Class Structure";

                em.Attachments.Add(new EmailAttachment() { FileName = "json to class", Data=  });

                // Show the email composer.
                await EmailManager.ShowComposeNewEmailAsync(_mail);
            }
            catch (Exception)
            {

            }
        }
        #endregion

    }

}
