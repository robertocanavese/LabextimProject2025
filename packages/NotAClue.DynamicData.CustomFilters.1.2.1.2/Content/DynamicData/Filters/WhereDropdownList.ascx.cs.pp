using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using NotAClue.Web;

namespace $rootnamespace$
{
    public partial class WhereDropdownListFilter : System.Web.DynamicData.QueryableFilterUserControl
    {
        private const string NullValueString = "[null]";

        public override Control FilterControl
        {
            get { return DropDownList1; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Column.IsRequired)
                    DropDownList1.Items.Add(new ListItem(String.Format("[No '{0}']", Column.Name), NullValueString));

                PopulateListControl(DropDownList1);
                // Set the initial value if there is one
                string initialValue = DefaultValue;
                if (!String.IsNullOrEmpty(initialValue))
                    DropDownList1.SelectedValue = initialValue;
            }

            DropDownList1.ToolTip = this.AppRelativeVirtualPath.GetFileNameTitle();
        }

        protected new void PopulateListControl(ListControl listControl)
        {
            var query = Column.Table.GetQuery();

            // row
            var entityParam = Expression.Parameter(Column.Table.EntityType, "row");
            // row => row.DataSourceID
            var columnLambda = Expression.Lambda(Expression.Property(entityParam, Column.EntityTypeProperty), entityParam);
            // Items.Select(row => row.DataSourceID)
            var selectCall = Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, columnLambda.Body.Type }, query.Expression, columnLambda);
            // Items.Select(row => row.DataSourceID).Distinct
            var distinctCall = Expression.Call(typeof(Queryable), "Distinct", new Type[] { Column.EntityTypeProperty.PropertyType }, selectCall);

            var result = query.Provider.CreateQuery(distinctCall);

            var r = new List<ListItem>();
            foreach (var item in result)
            {
                if (item != null && !String.IsNullOrEmpty(item.ToString()))
                    r.Add(new ListItem(item.ToString()));
            }

            listControl.Items.AddRange(r.OrderBy(s => s.Text).ToArray());
        }

        public override IQueryable GetQueryable(IQueryable source)
        {
            string selectedValue = DropDownList1.SelectedValue;
            if (String.IsNullOrEmpty(selectedValue))
                return source;

            if (selectedValue == NullValueString)
                return ApplyEqualityFilter(source, Column.Name, null);

            return ApplyEqualityFilter(source, Column.Name, selectedValue);
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnFilterChanged();
        }
    }
}
