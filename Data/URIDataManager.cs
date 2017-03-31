using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RT;
using GooRestViewer.Models;
using System.Collections.ObjectModel;
namespace GooRestViewer.Data
{
    public static class URIDataManager
    {
        public static int StorageLimit { get { return 10; } set { } }

        public static string Key_SavedURIs { get { return "Json_SavedURIs"; } set { } }

        public static ObservableCollection<MURI> SavedURIs { get; set; }

        public static ObservableCollection<MURI> GetAllSavedURIs()
        {
            try
            {
                var Data = RT.Storage.GetRoamingValue(Key_SavedURIs);
                if (string.IsNullOrWhiteSpace(Data))
                {
                    return SavedURIs;
                }
                var _URIs = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<MURI>>(Data);
                if (_URIs == null || _URIs.Count == 0)
                {
                    return SavedURIs;
                }
                return _URIs;
            }
            catch (Exception)
            {
                return SavedURIs;
            }
        }

        public static bool SaveURI(MURI _URI)
        {
            try
            {
                SavedURIs = GetAllSavedURIs();
                bool _res = false;
                if (SavedURIs == null) { SavedURIs = new ObservableCollection<MURI>(); }
                if (SavedURIs.Count > StorageLimit - 1)
                {
                    SavedURIs = new ObservableCollection<MURI>(SavedURIs.OrderByDescending(x => x.CreatedOn).Take(StorageLimit - 1));
                }
                if (SavedURIs.FirstOrDefault(x => (x.StrURI ?? "").Trim().Equals((_URI.StrURI ?? "").Trim())) == null)
                {
                    _URI.CreatedOn = DateTime.Now;
                    SavedURIs.Add(_URI);
                    _URI.ID = SavedURIs.Count;
                    _res = Storage.SetRoamingValue(Key_SavedURIs, Newtonsoft.Json.JsonConvert.SerializeObject(SavedURIs));
                }
                return _res;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
