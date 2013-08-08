using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Xml;

using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.DataLayer;
using umbraco.interfaces;

using System.Web.Script.Serialization;

namespace TextBoxList
{
    /// <summary>
    /// This class is used to setup the datatype settings. 
    /// On save it will store these values (using the datalayer) in the database
    /// </summary>
    public class DataType_PrevalueEditor : System.Web.UI.UpdatePanel, IDataPrevalue
    {
        // referenced datatype
        private umbraco.cms.businesslogic.datatype.BaseDataType _datatype;

        private TextBox saveBox;
        private TextBox indLimitBox;
        
       

        private JavaScriptSerializer jsonSerializer;
        private TextBoxListOptions savedOptions;


        public DataType_PrevalueEditor(umbraco.cms.businesslogic.datatype.BaseDataType DataType)
        {
            //Log.Add(LogTypes.Debug, 0, "Prevalue Constructor");
            _datatype = DataType;
            jsonSerializer = new JavaScriptSerializer();
            savedOptions = Configuration;
            if (savedOptions == null)
                savedOptions = new TextBoxListOptions();
        }

        public Control Editor
        {
            get
            {
                return this;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //Log.Add(LogTypes.Debug, 0, "Prevalue ini");

            saveBox = new TextBox();
            saveBox.CssClass = "indentedListSaveBox";
            ContentTemplateContainer.Controls.Add(saveBox);

            Label indLimitLabel = new Label();
            indLimitLabel.Text = "Maximum Indent Level";
            ContentTemplateContainer.Controls.Add(indLimitLabel);

            indLimitBox = new TextBox();
            indLimitBox.CssClass = "indLimitBox";
            ContentTemplateContainer.Controls.Add(indLimitBox);

            string css = string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", "/umbraco/plugins/TextBoxList/TextBoxListPrevalue.css");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(TextBoxList_DataEditor), "TextBoxListPrevalueCSS", css, false);

            string js = string.Format("<script src=\"{0}\" ></script>", "/umbraco/plugins/TextBoxList/TextBoxListPrevalue.js");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(TextBoxList_DataEditor), "TextBoxListPrevalueJS", js, false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //string csvList = String.Join(",", savedOptions.tabNames);

            //Log.Add(LogTypes.Debug, 0, "!!!!!");

            indLimitBox.Text = savedOptions.indentedLimit;
            
        }

        public void Save()
        {
            //Log.Add(LogTypes.Debug, 0, "Prevalue saving");
            _datatype.DBType = (umbraco.cms.businesslogic.datatype.DBTypes)Enum.Parse(typeof(umbraco.cms.businesslogic.datatype.DBTypes), DBTypes.Ntext.ToString(), true);

            SqlHelper.ExecuteNonQuery("delete from cmsDataTypePreValues where datatypenodeid = @dtdefid", SqlHelper.CreateParameter("@dtdefid", _datatype.DataTypeDefinitionId));
            SqlHelper.ExecuteNonQuery("insert into cmsDataTypePreValues (datatypenodeid,[value],sortorder,alias) values (@dtdefid,@value,0,'')", SqlHelper.CreateParameter("@dtdefid", _datatype.DataTypeDefinitionId), SqlHelper.CreateParameter("@value", saveBox.Text));
        }

        public TextBoxListOptions Configuration
        {
            get
            {
                try
                {
                    object conf = SqlHelper.ExecuteScalar<object>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid", SqlHelper.CreateParameter("@datatypenodeid", _datatype.DataTypeDefinitionId));
                    return jsonSerializer.Deserialize<TextBoxListOptions>(conf.ToString());
                }
                catch (Exception e)
                {
                    return new TextBoxListOptions();
                }
            }
        }

        public static ISqlHelper SqlHelper
        {
            get
            {
                return Application.SqlHelper;
            }
        }
    }
}