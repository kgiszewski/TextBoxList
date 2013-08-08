using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic.datatype;
using umbraco;
using umbraco.NodeFactory;

using System.Data.OleDb;
using System.Data;
using System.IO;

namespace TextBoxList {
    public partial class ImportList : umbraco.BasePages.UmbracoEnsuredPage {
        public string umbracoValue;
  
        public object value {
            get {
                return umbracoValue;
            }
            set {
                umbracoValue = value.ToString();
            }
        }




    }
}