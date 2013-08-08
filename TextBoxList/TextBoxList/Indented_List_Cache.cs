using System;
//using System.Collections.Generic;
using System.Text;
using System.Xml;

using umbraco.BusinessLogic;

namespace TextBoxList
{

    public class DataTypeTextBoxList : umbraco.cms.businesslogic.datatype.DefaultData
    {

        public DataTypeTextBoxList(umbraco.cms.businesslogic.datatype.BaseDataType DataType) : base(DataType) {
        }

        public override System.Xml.XmlNode ToXMl(System.Xml.XmlDocument data)
        {

            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(this.Value.ToString());
            }
            catch (Exception e)
            {
                string initialValue = "<list><item indent=\"0\"/></list>";
                xd.LoadXml(initialValue);
            }

            return data.ImportNode(xd.DocumentElement, true);
        }
    }
}

