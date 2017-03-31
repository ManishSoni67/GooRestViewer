using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooRestViewer.Models
{

    public class GooProp
    {
        public GooProp() { }

        public string PropName { get; set; }
        public string PropType { get; set; }
        public GooJsonDataTypes PrType { get; set; }
        public bool IsOfRefenrenceType
        {
            get
            {
                try
                {
                    return (PrType.Equals(GooJsonDataTypes.Object) || PrType.Equals(GooJsonDataTypes.List));
                }
                catch (Exception ee)
                {
                    return false;
                }
            }
            set { }
        }

        public bool IsOfListType
        {
            get
            {
                try
                {
                    return PrType.Equals(GooJsonDataTypes.List);
                }
                catch (Exception ee)
                {
                    return false;
                }
            }
            set { }
        }
        public bool IsOfObjectType
        {
            get
            {
                try
                {
                    return PrType.Equals(GooJsonDataTypes.Object);
                }
                catch (Exception ee)
                {
                    return false;
                }
            }
            set { }
        }

        public override string ToString()
        {
            return PropName ?? "";
        }

    }
}
