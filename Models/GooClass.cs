using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;
using GooJsonClassParser;
using GooRestViewer;
using System.ComponentModel;
using GooRestViewer.Models;
namespace GooJsonViewer.Models
{

    public class GooClass
    {
        public GooClass()
        {
            Properties = new List<GooProp>();
        }

        public string Name { get; set; }
        public List<GooProp> Properties { get; set; }
        public int InstanceCount { get; set; }
        public bool HaveInstaces
        {
            get
            {
                return InstanceCount > 0;
            }
            set { }
        }
        public override string ToString()
        {
            return Name ?? "";
        }



    }

}
