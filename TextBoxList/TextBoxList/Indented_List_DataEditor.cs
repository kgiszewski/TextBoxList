using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Collections.Generic;
using System.Xml;

using umbraco.interfaces;
using umbraco.NodeFactory;
using umbraco.BusinessLogic;

namespace TextBoxList
{
    /// <summary>
    /// This class is used for the actual datatype dataeditor, i.e. the control you will get in the content section of umbraco. 
    /// </summary>
    public class TextBoxList_DataEditor : System.Web.UI.UpdatePanel, umbraco.interfaces.IDataEditor
    {
        private umbraco.interfaces.IData savedData;
        private TextBoxListOptions options;
        private XmlDocument xmlData = new XmlDocument();
        private string pluginPath = "/umbraco/plugins/TextBoxList";

        private TextBox saveBox;

        private HtmlGenericControl divWrapper;

        private HtmlGenericControl textStringRow;
        private HtmlGenericControl textStringRowField;
        private HtmlGenericControl indent;
        private HtmlGenericControl unindent;
        private HtmlGenericControl indentImage;
        private HtmlGenericControl unindentImage;
        private HtmlGenericControl addRow;
        private HtmlGenericControl deleteRow;
        private HtmlGenericControl addRowImage;
        private HtmlGenericControl deleteRowImage;

        private HtmlGenericControl sort;
        private HtmlGenericControl sortImage;
        private TextBox listItem;
        private Literal ltrlControls;

        public string currentData = "";

        public TextBoxList_DataEditor(umbraco.interfaces.IData Data, TextBoxListOptions Configuration)
        {
            //load the prevalues
            options = Configuration;

            //ini the savedData object
            savedData = Data;

            //setupChildControls();
        }

        public virtual bool TreatAsRichTextEditor
        {
            get { return false; }
        }

        public bool ShowLabel
        {
            get { return true; }
        }

        public Control Editor { get { return this; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string css = string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\" />", pluginPath + "/TextBoxList.css");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(TextBoxList_DataEditor), "TextBoxListCSS", css, false);

            string js = string.Format("<script src=\"{0}\" ></script>", pluginPath + "/TextBoxList.js");
            ScriptManager.RegisterClientScriptBlock(Page, typeof(TextBoxList_DataEditor), "TextBoxListJS", js, false);


            saveBox = new TextBox();
            saveBox.CssClass = "TextBoxListSettingsSaveBox";
            ContentTemplateContainer.Controls.Add(saveBox);
            
        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            
           
        }
        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.EnsureChildControls();

        }
        private void setupChildControls() {

            if (Page.IsPostBack) {
                if (currentData == "") {
                    currentData = saveBox.Text;
                }
            }
            else {
                currentData = savedData.Value.ToString();
            }
            
            ltrlControls = new Literal();
            string controlsText = "";
            controlsText += "<a class=\"import-button\" href=\"javascript:UmbClientMgr.openModalWindow('plugins/TextBoxList/ImportList.aspx?', 'Edit Spreadsheet Header', true, 500, 600,'','','', function(returnValue){updateList('" + this.ClientID + "', returnValue)} );\">Import</a>";
            controlsText += "<a class=\"clear-button\" href=\"#clear\">Clear</a>";
            ltrlControls.Text = controlsText;
            ContentTemplateContainer.Controls.Add(ltrlControls);


            try {
                xmlData.LoadXml(currentData);
            }
            catch (Exception e2) {
                xmlData.LoadXml("<list><item indent='0'/></list>");
            }


            XmlNodeList items = xmlData.SelectNodes("list/item");

            divWrapper = new HtmlGenericControl();
            divWrapper.TagName = "div";
            divWrapper.Attributes["limit"] = options.indentedLimit;
            divWrapper.Attributes["class"] = "TextBoxListWrapper";

            textStringRow = new HtmlGenericControl();
            textStringRow.TagName = "ul";
            textStringRow.Attributes["class"] = "textstring-row-set";
            divWrapper.Controls.Add(textStringRow);

            foreach (XmlNode item in items) {

                textStringRowField = new HtmlGenericControl();
                textStringRowField.TagName = "li";
                textStringRowField.Attributes["class"] = "textstring-row-field";
                textStringRowField.Attributes["indent"] = item.Attributes["indent"].Value.ToString();
                textStringRow.Controls.Add(textStringRowField);

                unindent = new HtmlGenericControl();
                unindent.TagName = "a";
                unindent.Attributes["class"] = "unindent_row";
                unindent.Attributes["title"] = "unindent this row";
                unindent.Attributes["href"] = "#unindent";
                textStringRowField.Controls.Add(unindent);

                unindentImage = new HtmlGenericControl();
                unindentImage.TagName = "img";
                unindentImage.Attributes["src"] = pluginPath + "/images/unIndentRow.png";
                unindent.Controls.Add(unindentImage);

                indent = new HtmlGenericControl();
                indent.TagName = "a";
                indent.Attributes["class"] = "indent_row";
                indent.Attributes["title"] = "indent this row";
                indent.Attributes["href"] = "#indent";
                textStringRowField.Controls.Add(indent);

                indentImage = new HtmlGenericControl();
                indentImage.TagName = "img";
                indentImage.Attributes["src"] = pluginPath + "/images/indentRow.png";
                indent.Controls.Add(indentImage);

                listItem = new TextBox();
                listItem.CssClass = "umbEditorTextField";
                textStringRowField.Controls.Add(listItem);
                listItem.Text = HttpUtility.HtmlDecode(item.InnerText);

                addRow = new HtmlGenericControl();
                addRow.TagName = "a";
                addRow.Attributes["class"] = "add_row";
                addRow.Attributes["title"] = "add a new row";
                addRow.Attributes["href"] = "#add";
                textStringRowField.Controls.Add(addRow);

                addRowImage = new HtmlGenericControl();
                addRowImage.TagName = "img";
                addRowImage.Attributes["src"] = pluginPath + "/images/plus-button.png";
                addRow.Controls.Add(addRowImage);

                deleteRow = new HtmlGenericControl();
                deleteRow.TagName = "a";
                deleteRow.Attributes["class"] = "delete_row";
                deleteRow.Attributes["title"] = "delete this row";
                deleteRow.Attributes["href"] = "#remove";
                textStringRowField.Controls.Add(deleteRow);

                deleteRowImage = new HtmlGenericControl();
                deleteRowImage.TagName = "img";
                deleteRowImage.Attributes["src"] = pluginPath + "/images/minus-button.png";
                deleteRow.Controls.Add(deleteRowImage);

                sort = new HtmlGenericControl();
                sort.TagName = "a";
                sort.Attributes["class"] = "textstring-row-sort-a";
                sort.Attributes["title"] = "Re-order this row";
                deleteRow.Attributes["href"] = "#sort";
                textStringRowField.Controls.Add(sort);

                sortImage = new HtmlGenericControl();
                sortImage.TagName = "img";
                sortImage.Attributes["class"] = "textstring-row-sort-image";
                sortImage.Attributes["src"] = pluginPath + "/images/sort.png";
                sort.Controls.Add(sortImage);

            }
            ContentTemplateContainer.Controls.Add(divWrapper);


        }


        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);

            setupChildControls();
        }

        

        public void Save()
        {
            savedData.Value = saveBox.Text;

        }

    }
}