using RT;
using RT.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GooRestViewer.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class URIBuilder : Page, IPageView
    {
        #region Properties
        ObservableCollection<QueryParam> _QueryParams = new ObservableCollection<QueryParam>();
        public ObservableCollection<QueryParam> QueryParams
        {
            get { return _QueryParams; }
            set
            {
                _QueryParams = value;
                Notify("QueryParams");
            }
        }

        public string BaseURI
        {
            get
            {
                try
                {
                    return txt_BaseURL.Text ?? "";
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
                    txt_BaseURL.Text = value;
                }
                catch (Exception)
                {

                }
            }
        }

        public int QueryParamsCount
        {
            get
            {
                try
                {
                    return (QueryParams != null) ? QueryParams.Count : 0;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set { }
        }

        public bool HaveQueryParams
        {
            get
            {
                return QueryParamsCount > 0;
            }
            set { }
        }
        #endregion

        public URIBuilder()
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
                if (Get_If_GeneratedURIContainerVisible())
                {
                    HideGeneratedRIContainer();
                    return true;
                }


                return false;
            }
            catch (Exception ee)
            {
                return false;
            }
        }

        #region Retry Control Methods and Properties

        public void PromptForRetry(string str_prompt_message)
        {
            try
            {
                str_prompt_message = string.IsNullOrWhiteSpace(str_prompt_message) ? ErrorMessages.DefaultErrorMessage : str_prompt_message;
                this.RunAsAsync(() => stck_RetryContainer.SetVisibility(true));
                //   this.RunAsAsync(() => cntrl_retry.InvokeAnimation());
                //    txt_errormessage.Text = str_prompt_message;
                this.RunAsAsync(() => cntrl_errorMessage.SetMessage((str_prompt_message ?? "")));
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

        public async void Initiate()
        {
            try
            {
                IsBusy = false;
                if (CurrentNavigationParam == null)
                {
                    PromptForRetry(ErrorMessages.DefaultErrorMessage);
                    return;
                }
                if (!string.IsNullOrWhiteSpace(CurrentNavigationParam.StrData))
                {

                    IsBusy = true;
                    App.ShowShellProgress();
                    await this.RunAsAsync(() => LoadQueryParamsForURL(CurrentNavigationParam.StrData));
                }
                HidePrompt();
                IsBusy = false;
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

        #region Maintaining Focus for the Base Text Box URI
        private void txt_BaseURL_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                // (sender as TextBox).Focus(FocusState.Programmatic);
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region QueryParams Based Methods and Properties
        public void LoadQueryParamsForURL(string URL)
        {
            try
            {
                Uri _ur = new Uri(URL);
                BaseURI = _ur.Scheme + "://" + _ur.Authority + _ur.LocalPath;
                if (!string.IsNullOrWhiteSpace(_ur.Query))
                {
                    if (QueryParams == null) { QueryParams = new ObservableCollection<QueryParam>(); }
                    QueryParams.Clear();
                    var Query = WebUtility.UrlDecode(_ur.Query ?? "");
                    var StrArr = Query.Replace("?", "").Split('&');
                    foreach (var ele in StrArr)
                    {
                        if (!string.IsNullOrWhiteSpace(ele))
                        {
                            // var _Data = ele.Split('=');
                            var SeparatorIndex = ele.IndexOf("=");
                            var ParamKey = ele.Substring(0, SeparatorIndex);
                            var ParamValue = ele.Substring(SeparatorIndex + 1);
                            AddQueryParam(new QueryParam() { Key = (ParamKey ?? ""), Value = (ParamValue ?? "") });
                        }
                    }
                    ShowOrHideQueryParamsCountContainer();
                }
                // Notify("QueryParams");
            }
            catch (Exception)
            {
                PromptForRetry(ErrorMessages.URLParsingFailureMessage);
            }
            App.HideShellProgress();
            IsBusy = false;
        }

        public void ShowOrHideQueryParamsCountContainer()
        {
            try
            {
                txt_QueryParams_Count.Text = QueryParamsCount.ToString();
                brd_QueryParams_Count.SetVisibility(HaveQueryParams);
                brd_NoQueryParams.SetVisibility(!HaveQueryParams);
            }
            catch (Exception)
            {

            }
        }



        private void btn_add_QryParam_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                AddQueryParam(null);
            }
            catch (Exception)
            {

            }
        }
        public void AddQueryParam(QueryParam _Param)
        {
            try
            {
                if (QueryParams == null)
                {
                    QueryParams = new ObservableCollection<QueryParam>();
                }

                if (_Param == null)
                {
                    _Param = new QueryParam() { IndexID = (QueryParams.Count + 1) };
                    QueryParams.Add(_Param);
                    //    QueryParams = new ObservableCollection<QueryParam>(QueryParams.OrderByDescending(x => x.IndexID));
                    ShowOrHideQueryParamsCountContainer();
                    lstview_QueryParams.ScrollIntoView(_Param, ScrollIntoViewAlignment.Default);
                }
                else
                {
                    _Param.IndexID = (QueryParams.Count + 1);
                    QueryParams.Add(_Param);
                }
                // Notify("QueryParams");
            }
            catch (Exception)
            {

            }
        }

        public void RemoveParam(QueryParam _Param)
        {
            try
            {
                if (_Param == null) { return; }
                if (QueryParams.Contains(_Param))
                {
                    QueryParams.Remove(_Param);
                    //  QueryParams = new ObservableCollection<QueryParam>(QueryParams.OrderByDescending(x => x.IndexID));
                    ShowOrHideQueryParamsCountContainer();
                    //  Notify("QueryParams");
                    // this.Focus(FocusState.Keyboard);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btn_rem_QryParam_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                RemoveParam((sender as FrameworkElement).DataContext as QueryParam);
            }
            catch (Exception)
            {

            }
        }

        private void grd_Query_Params_Container_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                //var ele_Text = (sender as FrameworkElement).FindName("txt_QueryKey") as TextBox;
                //var ele_Value = (sender as FrameworkElement).FindName("txt_QueryValue") as TextBox;
                //((sender as FrameworkElement).DataContext as QueryParam).SetElementsFromTemplate(ele_Text, ele_Value);
            }
            catch (Exception)
            {

            }

        }

        #endregion

        #region Command Bar Element Related Methods and Event Handlers
        private void App_btn_URI_Frwrd_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (Get_If_GeneratedURIContainerVisible())
                {
                    ValidateAndRedirectBackToMainPage(txt_Final_Uri.Text ?? "");
                }
                else
                {
                    ShowForGeneratedURIString();
                }
            }
            catch (Exception)
            {

            }
        }

        public void ShowForGeneratedURIString()
        {
            try
            {
                string _strURI = BaseURI;
                string _paramsaperator = "";
                Uri _uri = new Uri(_strURI);
                if (string.IsNullOrWhiteSpace(_uri.Query))
                    _paramsaperator = "?";
                else
                    _paramsaperator = "&";
                if (HaveQueryParams)
                {
                    string _strQueryParams = "";
                    foreach (var _param in QueryParams.Where(x => x.IsValid))
                    {
                        _strQueryParams += _paramsaperator + (_param.Key) + "=" + (_param.Value);
                        _paramsaperator = "&";
                    }
                    if (!string.IsNullOrWhiteSpace(_strQueryParams)) { _strURI = _strURI + _strQueryParams; }
                }
                ShowGeneratedRIContainer(_strURI);
            }
            catch (Exception)
            {
                PromptForRetry(ErrorMessages.URLParsingFailureMessage);
            }
        }

        #endregion

        #region Generated URI String Animation and Methods
        public void ShowGeneratedRIContainer(string URI_text)
        {
            try
            {
                brd_GenetedURI.SetVisibility(true);
                this.RunAsAsync(() => ShowOverLay());
                str_Show_GeneratedURI.Begin();
                //   this.TryHideCommandbar();
                txt_Final_Uri.Text = URI_text;
            }
            catch (Exception)
            {

            }
        }
        public void HideGeneratedRIContainer()
        {
            try
            {
                this.RunAsAsync(() => HideOverLay());
                str_Hide_GeneratedURI.Begin();
                // this.TryShowCommandBar();
            }
            catch (Exception)
            {

            }
        }
        public bool Get_If_GeneratedURIContainerVisible()
        {
            try
            {
                return brd_GenetedURI.GetVisibility();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void str_Hide_GeneratedURI_Completed(object sender, object e)
        {
            try
            {
                brd_GenetedURI.SetVisibility(false);
            }
            catch (Exception)
            {

            }

        }



        private void btn_Url_Builder_Click(object sender, RoutedEventArgs e)
        {

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
                PromptForRetry(ErrorMessages.URLParsingFailureMessage);
            }
        }

        #endregion

        #region KeyDown Elements
        private void txt_QueryKey_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                    ((sender as FrameworkElement).DataContext as QueryParam).SetFocusOnText_Value(this);
            }
            catch (Exception)
            {
                this.Focus(FocusState.Keyboard);
            }
        }

        private void txt_QueryValue_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    var NextQueryParamIndex = QueryParams.IndexOf(((sender as FrameworkElement).DataContext as QueryParam)) + 1;
                    NextQueryParamIndex = (NextQueryParamIndex > QueryParams.Count - 1) ? 0 : NextQueryParamIndex;
                    var NextQueryParam = QueryParams.ElementAt(NextQueryParamIndex);
                    if (NextQueryParam != null)
                    {
                        (NextQueryParam).SetFocusOnText_Key(this);
                        lstview_QueryParams.ScrollIntoView(NextQueryParam);
                    }
                }
            }
            catch (Exception)
            {
                this.Focus(FocusState.Keyboard);
            }
        }

        private void grd_Query_Params_Container_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            try
            {
                var ele_Text = (sender as FrameworkElement).FindName("txt_QueryKey") as TextBox;
                var ele_Value = (sender as FrameworkElement).FindName("txt_QueryValue") as TextBox;
                ((sender as FrameworkElement).DataContext as QueryParam).SetElementsFromTemplate(ele_Text, ele_Value);
            }
            catch (Exception)
            {

            }
        }
        #endregion

    }

    public class QueryParam : RT.EntityModel
    {
        string _Key = "";
        public string Key
        {
            get { return _Key; }
            set
            {
                _Key = value;
                Notify("Key");
                Notify("WarningMessage");
                Notify("HaveWarningMessage");
            }
        }

        string _Value = "";
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                Notify("Value");
                Notify("WarningMessage");
                Notify("HaveWarningMessage");
            }
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    return (!string.IsNullOrWhiteSpace(Key));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set { }
        }

        public int IndexID { get; set; }

        public string WarningMessage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Key))
                {
                    return "please enter a key for the param, otherwise will not be included in the URI";
                }
                if (string.IsNullOrWhiteSpace(Value))
                {
                    return "does'nt have any value for " + (Key ?? "").ToLower();
                }
                return "";
            }
            set
            {

            }
        }

        public bool HaveWarningMessage
        {
            get
            {
                try
                {
                    return !(string.IsNullOrWhiteSpace(WarningMessage));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set { }
        }

        #region Visual Elements and Methods
        public TextBox Txt_KeyElement { get; set; }
        public TextBox Txt_ValueElement { get; set; }

        public void SetElementsFromTemplate(TextBox _txtKey, TextBox _txtValue)
        {
            try
            {
                Txt_KeyElement = _txtKey;
                Txt_ValueElement = _txtValue;
            }
            catch (Exception)
            {

            }
        }

        public void SetFocusOnText_Key(Page _ConsumerPage)
        {
            try
            {
                if (Txt_KeyElement == null)
                {
                    _ConsumerPage.Focus(FocusState.Keyboard);
                    return;
                }

                Txt_KeyElement.Focus(FocusState.Keyboard);
            }
            catch (Exception)
            {

            }
        }

        public void SetFocusOnText_Value(Page _ConsumerPage)
        {
            try
            {
                if (Txt_ValueElement == null)
                {
                    _ConsumerPage.Focus(FocusState.Keyboard);
                    return;
                }

                Txt_ValueElement.Focus(FocusState.Keyboard);
            }
            catch (Exception)
            {

            }
        }


        #endregion
    }



}
