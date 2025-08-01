using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationTemplatePopup : QuotationController
    {
        //protected MetaTable table;


        //protected readonly static string QuotationKey = "P0";

        //public string QuotationParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[QuotationKey];
        //        return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
        //    }
        //}

        //protected readonly static string MenuKey = "P1";

        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
            DynamicDataManager1.RegisterControl(dtvQuotationTemplate);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (! IsPostBack)
            {
                if (Convert.ToInt32(Request.QueryString["ID"]) == -1)
                    dtvQuotationTemplate.DefaultMode = DetailsViewMode.Insert;
            }

        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //Session[MenuType.MenuQuotationTemplates.ToString()] = null;
            ClearAllMenusCache("-1");
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            {
                //Session[MenuType.MenuQuotationTemplates.ToString()] = null;
                ClearAllMenusCache("-1");
            }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //Session[MenuType.MenuQuotationTemplates.ToString()] = null;
            ClearAllMenusCache("-1");
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode != DetailsViewMode.ReadOnly)
            {
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            var rowCount = dtvQuotationTemplate.Rows.Count;
            if (rowCount > 0)
            {
                SetDeleteConfirmation(dtvQuotationTemplate.Rows[rowCount - 1]);

                var _lbtDeactivate = (LinkButton) dtvQuotationTemplate.FindControl("lbtDeactivate");
                var _lbtActivate = (LinkButton) dtvQuotationTemplate.FindControl("lbtActivate");

                using (var _qc = new QuotationDataContext())
                {
                    var _QuotationTemplate =
                        _qc.QuotationTemplates.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvQuotationTemplate.DataKey.Value));
                    if (_QuotationTemplate != null)
                    {
                        _lbtDeactivate.Enabled = _QuotationTemplate.Inserted;
                        _lbtActivate.Enabled = !_QuotationTemplate.Inserted;
                    }
                }
            }
        }

        private void SetDeleteConfirmation(TableRow row)
        {
            foreach (Control c in row.Cells[0].Controls)
            {
                if (c is LinkButton)
                {
                    var btn = (LinkButton) c;
                    if (btn.CommandName == DataControlCommands.DeleteCommandName)
                    {
                        btn.OnClientClick = "return confirm('Sei sicuro di voler eliminare questa voce?');";
                    }
                }
            }
        }

        protected void DetailsDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            ((QuotationTemplate) e.NewObject).Inserted = true;
        }

        protected void dtvQuotationTemplate_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Deactivate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _QuotationTemplate =
                        _qc.QuotationTemplates.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvQuotationTemplate.DataKey.Value));
                    _QuotationTemplate.Inserted = false;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuQuotationTemplates.ToString()] = null;
                    ClearAllMenusCache("-1");
                }
            }
            if (e.CommandName == "Activate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _QuotationTemplate =
                        _qc.QuotationTemplates.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvQuotationTemplate.DataKey.Value));
                    _QuotationTemplate.Inserted = true;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuQuotationTemplates.ToString()] = null;
                    ClearAllMenusCache("-1");
                }
            }
        }

        protected void dtvQuotationTemplate_ItemCreated(object sender, EventArgs e)
        {
            var _lbtDeactivate = (LinkButton) dtvQuotationTemplate.FindControl("lbtDeactivate");
            var _lbtActivate = (LinkButton) dtvQuotationTemplate.FindControl("lbtActivate");

            using (var _qc = new QuotationDataContext())
            {
                var _QuotationTemplate =
                    _qc.QuotationTemplates.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvQuotationTemplate.DataKey.Value));
                if (_QuotationTemplate != null)
                {
                    if (_QuotationTemplate.Inserted)
                    {
                        _lbtDeactivate.OnClientClick =
                            @"return confirm('Sei sicuro di voler disattivare definitivamente questa voce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo o di un gruppo voci già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
                    }
                    if (!_QuotationTemplate.Inserted)
                    {
                        _lbtActivate.OnClientClick =
                            @"return confirm('Sei sicuro di voler riattivare questa voce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
                    }
                }
            }
        }

        protected void dtvQuotationTemplate_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (Convert.ToDecimal(e.NewValues["Cost"]) != Convert.ToDecimal(e.OldValues["Cost"]))
                e.NewValues["Date"] = DateTime.Today;
        }
    }
}