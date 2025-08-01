using System;
using System.Web.DynamicData;
using System.Linq;
using System.Web.UI;
using System.Collections;
using NotAClue.Web.DynamicData;
using NotAClue.Web;

namespace $rootnamespace$
{
    public partial class Autocomplete_Filter : System.Web.DynamicData.QueryableFilterUserControl
    {
        private new MetaForeignKeyColumn Column
        {
            get { return (MetaForeignKeyColumn)base.Column; }
        }

        public override Control FilterControl
        {
            get { return AutocompleteTextBox; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
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

            // modify behaviorID so it does not clash with other autocomplete extenders on the page
            autoComplete1.Animations = autoComplete1.Animations.Replace(autoComplete1.BehaviorID, AutocompleteTextBox.UniqueID);
            autoComplete1.BehaviorID = AutocompleteTextBox.UniqueID;

            if (!Page.IsPostBack && !String.IsNullOrEmpty(DefaultValue))
            {
                // set the initial value of the filter if it's present in the request URL

                MetaTable parentTable = fkColumn.ParentTable;
                IQueryable query = parentTable.GetQuery();
                // multi-column PK values are separated by commas
                var singleCall = LinqExpressionHelper.BuildSingleItemQuery(query, parentTable, DefaultValue.Split(','));
                var row = query.Provider.Execute(singleCall);
                string display = parentTable.GetDisplayString(row);

                AutocompleteTextBox.Text = display;
                AutocompleteValue.Value = DefaultValue;
            }

            AutocompleteTextBox.ToolTip = this.AppRelativeVirtualPath.GetFileNameTitle();
}

        public void ClearButton_Click(object sender, EventArgs e)
        {
            // this would probably be better handled using client JavaScirpt
            AutocompleteValue.Value = String.Empty;
            AutocompleteTextBox.Text = String.Empty;
            OnFilterChanged();
        }

        protected void AutocompleteValue_ValueChanged(object sender, EventArgs e)
        {
            OnFilterChanged();
        }

        public override IQueryable GetQueryable(IQueryable source)
        {
            string selectedValue = String.IsNullOrEmpty(AutocompleteValue.Value) ? null : AutocompleteValue.Value;
            if (String.IsNullOrEmpty(selectedValue))
                return source;

            IDictionary dict = new Hashtable();
            Column.ExtractForeignKey(dict, selectedValue);
            foreach (DictionaryEntry entry in dict)
            {
                string key = (string)entry.Key;
                if (DefaultValues != null)
                    DefaultValues[key] = entry.Value;

                source = ApplyEqualityFilter(source, Column.GetFilterExpression(key), entry.Value);
            }
            return source;
        }
    }
}
