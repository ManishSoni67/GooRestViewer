using RT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GooRestViewer.Models
{
    [DataContract]
    public class MURI : EntityModel
    {
        [DataMember]
        public string StrURI { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [IgnoreDataMember]
        public string strCreatedOn
        {
            get
            {
                try
                {
                    return CreatedOn.ToString();
                }
                catch (Exception)
                {
                    return "";
                }
            }
            set { }
        }

        private int _ID;
        [IgnoreDataMember]
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                Notify("ID");
                Notify("strID");
            }
        }

        [IgnoreDataMember]
        public string strID { get { return "#" + ID; } set { } }

    }
}
