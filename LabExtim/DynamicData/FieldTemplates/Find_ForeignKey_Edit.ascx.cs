using System;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class Find_ForeignKey_EditField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return DropDownList1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DropDownList1.Items.Count == 0)
            {
                //if (!Column.IsRequired)
                //{
                    DropDownList1.Items.Add(new ListItem("[Non selezionato]", ""));
                //}

                // get the data context from the current table
                var DBContext = (DataContext)Table.CreateContext();

                // get the primary key column name in the FK table
                var PKColumn = ForeignKeyColumn.ParentTable.PrimaryKeyColumns[0].Name;

                // get the display column name
                var DisplayColumn = ForeignKeyColumn.ParentTable.DisplayColumn.Name;

                // not sure if this can ever happen, but if undefined, use first text column
                if (DisplayColumn.Length == 0)
                {
                    DisplayColumn =
                        (from a in ForeignKeyColumn.ParentTable.Columns where a.IsString select a.Name).FirstOrDefault();
                }

                // if all else fails, use the PK as the display value
                if (DisplayColumn == null || DisplayColumn.Length == 0)
                {
                    DisplayColumn = PKColumn;
                }

                // get a list of columns in the FK table
                var ColumnNames = from c in ForeignKeyColumn.ParentTable.Columns select c.Name;

                // build a where clause and orderby for the FK table

                //string WhereClause = ""; 
                //// PageContext.CustomerId is an external dependency, but this is required for inserts (can't pull from parent record)
                //if (ColumnNames.Contains("CustomerId") && Page is MyNamespace.MyBasePage)
                //{
                //    int CustomerId = ((MyNamespace.MyBasePage)Page).PageContext.CustomerId;
                //    WhereClause += (WhereClause.Length > 0 ? " AND " : "") + " CustomerId = " + CustomerId.ToString() + " ";
                //}
                //if (ColumnNames.Contains("Active"))
                //{
                //    WhereClause += (WhereClause.Length > 0 ? " AND " : "") + " Active = 1 ";
                //}
                //if (ColumnNames.Contains("Deleted"))
                //{
                //    WhereClause += (WhereClause.Length > 0 ? " AND " : "") + " Deleted = 0 ";
                //}
                //WhereClause = (WhereClause.Length > 0 ? " WHERE " : "") + WhereClause;

                var OrderBy = "";
                // modificato da Canny
                //if (ColumnNames.Contains("OrderBy"))
                //{
                //    OrderBy = "OrderBy,";
                //}
                if (ColumnNames.Contains("Order"))
                {
                    OrderBy = "[Order],";
                }

                //// build the full SQL for the FK query, cast everything to nvarchar for guid compat, etc.
                //string Sql = "SELECT cast(" + PKColumn + " as nvarchar) as id,cast(" + DisplayColumn + " as nvarchar) as name FROM " +
                //    ForeignKeyColumn.ParentTable.Name + WhereClause + " ORDER BY " + OrderBy + DisplayColumn + "," + PKColumn;

                // build the full SQL for the FK query, cast everything to nvarchar for guid compat, etc.
                //string Sql = "SELECT cast(" + PKColumn + " as nvarchar) as id,cast(" + DisplayColumn + " as nvarchar) as name FROM " +
                //    ForeignKeyColumn.ParentTable.Name + " ORDER BY " + OrderBy + DisplayColumn + "," + PKColumn;

                var Sql = "SELECT cast(" + PKColumn + " as nvarchar) as id,cast(" + DisplayColumn +
                          " as nvarchar(255)) as name FROM " +
                          ForeignKeyColumn.ParentTable.Name;

                if (ForeignKeyColumn.ParentTable.Name.Contains("Statuse") && Table.Name.Contains("CustomerOrders"))
                {
                    Sql += " WHERE StatusType = 2";
                }
                if (ForeignKeyColumn.ParentTable.Name.Contains("Statuse") && Table.Name.Contains("ProductionOrders"))
                {
                    Sql += " WHERE StatusType = 1";
                }
                if (ForeignKeyColumn.ParentTable.Name.Contains("Statuse") && Table.Name.Contains("ProductionMPs"))
                {
                    Sql += " WHERE StatusType = 3";
                }
                if (ForeignKeyColumn.ParentTable.Name.Contains("Quotation") && Table.Name.Contains("ProductionOrders"))
                {
                    Sql += " WHERE date > dateadd(YY, -1, getdate())";
                }
                if (Column.Name == "PickingItem" && Table.Name.Contains("ProductionOrderDetails"))
                {
                    Sql += " WHERE TypeCode = 31";
                    // merge aziendale
                    //if (!string.IsNullOrEmpty(Session["CurrentCompanyId"].ToString()))
                    //    Sql += string.Format(" AND ID_Company = {0}", Session["CurrentCompanyId"]);
                }
                //if (Column.Name == "PickingItem" && Table.Name.Contains("ProductionOrderDetails"))
                //{
                //    Sql += " WHERE TypeCode <> 31";
                //}

                if (Column.Name == "Customer" && (Table.Name.Contains("TempQuotations") || Table.Name.Contains("Find_Quotations")))
                {
                    Sql += " WHERE Name not like '**%'";
                    // merge aziendale
                    Sql += " AND code between 1 and 199999999";

                }

                if (Column.Name == "Type" || Column.Name == "ItemType")
                {
                    if (Table.Name.Contains("QuotationTemplates"))
                        Sql += " WHERE Category = 'T'";
                    else
                        Sql += " WHERE Category = 'I'";
                }

                Sql += " ORDER BY " + OrderBy + DisplayColumn + "," + PKColumn;

                foreach (var item in DBContext.ExecuteQuery<idname>(Sql))
                {
                    DropDownList1.Items.Add(new ListItem(item.name, item.id));
                }

                //PopulateListControl(DropDownList1);
                //Helper.BindTooltip(DropDownList1);
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (Mode == DataBoundControlMode.Edit)
            {
                var foreignkey = ForeignKeyColumn.GetForeignKeyString(Row);
                var item = DropDownList1.Items.FindByValue(foreignkey);
                if (item != null)
                {
                    DropDownList1.SelectedValue = foreignkey;
                    //DropDownList1.Enabled = false;
                }
            }
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
            var val = DropDownList1.SelectedValue;
            if (val == String.Empty)
                val = null;

            ExtractForeignKey(dictionary, val);
        }

        protected class idname
        {
            private idname()
            {
            }

            public string id { get; set; }
            public string name { get; set; }
        }
    }
}