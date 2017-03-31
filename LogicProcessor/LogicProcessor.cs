using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using GooJsonViewer.Models;
using GooRestViewer.Models;
namespace GooJsonClassParser
{
    public class JsonToClassesProcessor
    {
        public static async Task<List<GooClass>> GetJsonClassProp(string JsonContent, bool IsOfListType)
        {
            return await _GetJsonClassProp(JsonContent, "ResponseModel", GetJsonStringContentifArray(JsonContent));
        }

        private static bool GetJsonStringContentifArray(string JsonContent)
        {
            try
            {
                return ((JsonContent ?? "").ToString().StartsWith("["));
            }
            catch (Exception ee)
            {
                return false;
            }
        }




        static async Task<List<GooClass>> _GetJsonClassProp(string JsonContent, string ClassName = "ResponseModel", bool IsOfArrayType = false)
        {
            try
            {
                List<GooClass> Classes = new List<GooClass>();
                GooClass _JsonClass = new GooClass() { Name = ClassName };
                var JObj = new JObject();
                var JArray = new JArray();
                if (IsOfArrayType)
                {
                    JArray = JArray.Parse(JsonContent);
                    _JsonClass.InstanceCount = JArray.Count();
                    JObj = JObject.Parse(JArray[0].ToString());
                }
                else
                {
                    JObj = JObject.Parse(JsonContent);
                    _JsonClass.InstanceCount = 1;
                }
                var Properties = JObj.Properties();
                if (Properties == null || Properties.Count() == 0) { return null; }
                foreach (var prp in Properties)
                {
                    var PrpType = GetDataType(prp);
                    var Type = PrpType.ToString().ToLower();
                    var Name = prp.Name;
                    if (PrpType.Equals(GooJsonDataTypes.Object))
                    {
                        Type = prp.Name;
                    }
                    if (PrpType.Equals(GooJsonDataTypes.List))
                    {
                        Type = prp.Name;
                        if ((prp.Name ?? "").ToLower().Last().Equals('s')) { Name = prp.Name; }
                        else
                            Name = prp.Name + "s";
                    }
                    _JsonClass.Properties.Add(new GooProp() { PropName = Name, PropType = Type, PrType = PrpType });
                }
                Classes.Add(_JsonClass);
                foreach (var ListPrp in _JsonClass.Properties.Where(x => x.IsOfRefenrenceType))
                {
                    //  var ChildClasses = _GetJsonClassProp(JObj.Property(ListPrp.PropName).Value.First.ToString(), ChildClass, ChildClass + (++ChildCount));
                    var ChildClasses = await _GetJsonClassProp((JObj[(ListPrp.PropType)] ?? "").ToString(), ListPrp.PropType, ListPrp.IsOfListType);
                    if (ChildClasses != null || ChildClasses.Count > 0)
                        Classes.AddRange(ChildClasses);

                    //  ChildClass = ChildClass + (++ChildCount);
                }
                return Classes;
            }
            catch (Exception ee)
            {
                return new List<GooClass>();
            }
        }


        public static GooJsonDataTypes GetDataType(JProperty prop)
        {
            try
            {
                if ((string.IsNullOrWhiteSpace(prop.Value.ToString())))
                {
                    return GooJsonDataTypes.STRING;
                }
                int IntResult = 0;
                double DoubleRes = 0.0;
                bool BoolRes = false;
                var ValueChild = prop.Value.Count();
                DateTime DateTimeResult = DateTime.Now;
                if (int.TryParse(prop.Value.ToString(), out IntResult))
                    return GooJsonDataTypes.INT;
                else if (double.TryParse(prop.Value.ToString(), out DoubleRes))
                    return GooJsonDataTypes.Double;
                else if (bool.TryParse(prop.Value.ToString(), out BoolRes))
                    return GooJsonDataTypes.BOOL;
                else if (DateTime.TryParse(prop.Value.ToString(), out DateTimeResult))
                    return GooJsonDataTypes.DateTime;
                else if ((prop.Value ?? "").ToString().StartsWith("["))
                    return GooJsonDataTypes.List;
                else if ((prop.Value ?? "").ToString().StartsWith("{"))
                    return GooJsonDataTypes.Object;
                else
                    return GooJsonDataTypes.STRING;
            }
            catch (Exception)
            {
                return GooJsonDataTypes.STRING;
            }

        }
    }
}
