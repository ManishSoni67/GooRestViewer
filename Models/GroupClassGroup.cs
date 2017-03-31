using GooJsonViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooRestViewer.Models
{
    public class GroupClassGroup : List<GooClass>
    {

        public string Key { get; set; }

        public GroupClassGroup(string _Key)
        {
            Key = _Key;
        }


        public static ObservableCollection<GroupClassGroup> CreateGroups(List<GooClass> _Classes)
        {
            try
            {
                ObservableCollection<GroupClassGroup> Grouping = new ObservableCollection<GroupClassGroup>();
                var grp = _Classes.GroupBy(x => x.Name);
                foreach (var a in grp)
                {
                    GroupClassGroup _GRP = new GroupClassGroup(a.Key);
                    foreach (var items in a)
                    {
                        _GRP.Add(items);
                    }
                    Grouping.Add(_GRP);
                }
                return Grouping;
            }
            catch (Exception e)
            {
                return new ObservableCollection<GroupClassGroup>();
            }
        }

    }
}
