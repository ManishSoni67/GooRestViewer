using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace GooRestViewer.Helper
{
    public static class AppInfo
    {

        static Windows.ApplicationModel.Resources.ResourceLoader CurrentResourceLoader
        {
            get
            {
                try
                {
                    return Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("AppResources");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set { }
        }

        public static string AppName
        {
            get
            {
                try
                {
                    if (CurrentResourceLoader == null) { return ""; }
                    return CurrentResourceLoader.GetString("AppName");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }

        public static string EmailAddress
        {
            get
            {
                try
                {
                    if (CurrentResourceLoader == null) { return ""; }
                    return CurrentResourceLoader.GetString("Email");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }

        public static string Version
        {
            get
            {
                try
                {
                    return Package.Current.Id.Version.Major + "." + Package.Current.Id.Version.Minor + "." + Package.Current.Id.Version.Build + "." + Package.Current.Id.Version.Revision;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }

        public static string AppGUID
        {
            get
            {
                try
                {
                    if (CurrentResourceLoader == null) { return ""; }
                    return CurrentResourceLoader.GetString("AppGUID");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }


        public static string AppDescription
        {
            get
            {
                try
                {
                    if (CurrentResourceLoader == null) { return ""; }
                    return CurrentResourceLoader.GetString("AppDescription");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }
        public static string AppMarketPlaceLink
        {
            get
            {
                try
                {
                    return "https://www.windowsphone.com/en-us/store/app/rest-viewer/" + AppGUID;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }

    }
}
