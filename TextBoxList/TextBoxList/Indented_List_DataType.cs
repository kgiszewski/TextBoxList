using System;
//using System.Collections.Generic;
using System.Text;
using System.Xml;

using umbraco.cms.businesslogic.datatype;
using umbraco.BusinessLogic;

namespace TextBoxList
{

    public class TextBoxListDataType : umbraco.cms.businesslogic.datatype.BaseDataType, umbraco.interfaces.IDataType
    {
        private umbraco.interfaces.IDataEditor _Editor;
        private umbraco.interfaces.IData _baseData;
        private DataType_PrevalueEditor _prevalueeditor;

        // Instance of the Datatype
        public override umbraco.interfaces.IDataEditor DataEditor
        {
            get
            {
                if (_Editor == null)
                    _Editor = new TextBoxList_DataEditor(Data, ((DataType_PrevalueEditor)PrevalueEditor).Configuration);
                return _Editor;
            }
        }

        //this is what the cache will use when getting the data
        public override umbraco.interfaces.IData Data
        {
            get
            {
                if (_baseData == null)
                    _baseData = new DataTypeTextBoxList(this);
                return _baseData;
            }
        }

        /// <summary>
        /// Gets the datatype unique id.
        /// </summary>
        /// <value>The id.</value>
        public override Guid Id
        {
            get
            {
                return new Guid("{04D5DD09-C680-42CA-9726-364E66D8B439}");
            }
        }

        /// <summary>
        /// Gets the datatype unique id.
        /// </summary>
        /// <value>The id.</value>
        public override string DataTypeName
        {
            get
            {
                return "TextBox List";
            }
        }

        /// <summary>
        /// Gets the prevalue editor.
        /// </summary>
        /// <value>The prevalue editor.</value>
        public override umbraco.interfaces.IDataPrevalue PrevalueEditor
        {
            get
            {
                //Log.Add(LogTypes.Debug, 0, "Deciding to get prevalue editor");   
                if (_prevalueeditor == null)
                {
                    //Log.Add(LogTypes.Debug, 0, "getting prevalue editor");   
                    _prevalueeditor = new DataType_PrevalueEditor(this);
                }
                return _prevalueeditor;
            }
        }
    }
}