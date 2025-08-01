using System;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using NotAClue.Web.DynamicData;
using NotAClue.Web;
using System.Text;

namespace $rootnamespace$
{
    public partial class Autocomplete_EditField : System.Web.DynamicData.FieldTemplateUserControl
    {
        private new MetaForeignKeyColumn Column
        {
            get { return (MetaForeignKeyColumn)base.Column; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SetUpValidator(RequiredFieldValidator1);

            SetUpValidator(CustomValidator1);
            SetUpValidator(DynamicValidator1);
            if (Column.IsRequired)
            {
                CustomValidator1.Enabled = true;
                CustomValidator1.ErrorMessage = RequiredFieldValidator1.ErrorMessage;

                #region Add Validation script
                var script = new StringBuilder();
                script.Append("function AutocompleteClientValidate(source, arguments)\n");
                script.Append("{\n");
                script.Append("    var autocompleteValue = document.getElementById(\"" + AutocompleteValue.ClientID + "\");\n");
                script.Append("    if (autocompleteValue.value == undefined || autocompleteValue.value == \"\")\n");
                script.Append("        arguments.IsValid = false;\n");
                script.Append("    else\n");
                script.Append("        arguments.IsValid = true;\n");
                script.Append("}\n");
                Page.AddClientScript(AutocompleteValue.ClientID, script.ToString());
                CustomValidator1.EnableClientScript = true;
                #endregion
            }

            var fkColumn = Column as MetaForeignKeyColumn;

            //// dynamically build the context key so the web service knows which table we're talking about
            //autoComplete1.ContextKey = fkColumn.ParentTable.Provider.DataModel.ContextType.FullName + "#" + fkColumn.ParentTable.Name;
            autoComplete1.ContextKey = AutocompleteFilterService.GetContextKey(fkColumn.ParentTable);

            // add JavaScript to create post-back when user selects an item in the list
            var method = "function(source, eventArgs) {\r\n" +
                "var valueField = document.getElementById('" + AutocompleteValue.ClientID + "');\r\n" +
                "valueField.value = eventArgs.get_value();\r\n" +
                "setTimeout('" + Page.ClientScript.GetPostBackEventReference(AutocompleteTextBox, null).Replace("'", "\\'") + "', 0);\r\n" +
                "}";
            autoComplete1.OnClientItemSelected = method;

            // modify behaviorID so it does not clash with other auto complete extenders on the page
            autoComplete1.Animations = autoComplete1.Animations.Replace(autoComplete1.BehaviorID, AutocompleteTextBox.UniqueID);
            autoComplete1.BehaviorID = AutocompleteTextBox.UniqueID;

            // get control parameters
            var uiHintAttribute = Column.Attributes.OfType<UIHintAttribute>().FirstOrDefault();
            if (uiHintAttribute != null && uiHintAttribute.ControlParameters.Count > 0)
            {
                if (uiHintAttribute.ControlParameters.ContainsKey("MinimumPrefixLength"))
                {
                    int minimumPrefixLength = 3;
                    minimumPrefixLength = (int)uiHintAttribute.ControlParameters["MinimumPrefixLength"];
                    autoComplete1.MinimumPrefixLength = minimumPrefixLength;
                }

                if (uiHintAttribute.ControlParameters.ContainsKey("ServiceName"))
                {
                    var servicePath = String.Format(@"~\{0}.asmx", uiHintAttribute.ControlParameters["ServiceName"]);
                    autoComplete1.ServicePath = servicePath;
                }
            }
        }

        public void ClearButton_Click(object sender, EventArgs e)
        {
            // this would probably be better handled using client JavaScirpt
            AutocompleteValue.Value = String.Empty;
            AutocompleteTextBox.Text = String.Empty;
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit)
            {
                string selectedValueString = GetSelectedValueString();
                MetaTable parentTable = Column.ParentTable;
                IQueryable query = parentTable.GetQuery();
                // multi-column PK values are separated by commas
                var singleCall = LinqExpressionHelper.BuildSingleItemQuery(query, parentTable, selectedValueString.Split(','));
                var row = query.Provider.Execute(singleCall);
                string display = parentTable.GetDisplayString(row);

                AutocompleteTextBox.Text = display;
                AutocompleteValue.Value = selectedValueString;
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
            string value = String.IsNullOrEmpty(AutocompleteValue.Value) ? null : AutocompleteValue.Value;
            if (String.IsNullOrEmpty(value))
                value = null;

            ExtractForeignKey(dictionary, value);
        }

        public override Control DataControl
        {
            get { return AutocompleteValue; }
        }
    }
}
