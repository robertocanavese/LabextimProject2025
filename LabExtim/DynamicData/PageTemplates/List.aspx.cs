using System;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using CMLabExtim;
using UILabExtim;

namespace LabExtim.DynamicData.PageTemplates
{
    public partial class List : PageTemplateBaseController //System.Web.UI.Page
    {
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(GridView1, true /*setSelectionFromUrl*/);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            table = GridDataSource.GetTable();

            Title = table.DisplayName;

            InsertHyperLink.NavigateUrl = table.GetActionPath(PageAction.Insert);

            // Disable various options if the table is readonly
            if (table.IsReadOnly)
            {
                GridView1.Columns[0].Visible = false;
                InsertHyperLink.Visible = false;
            }

         }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
        }

        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                Log.Write("Errore nella cancellazione di una chiave esterna", e.Exception);
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.UpdateFailed);
                e.ExceptionHandled = true;
            }
        }
    }
}