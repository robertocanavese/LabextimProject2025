using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class PickingItemPopup : QuotationController
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
            DynamicDataManager1.RegisterControl(dtvPickingItem);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (! IsPostBack)
            {
                if (Convert.ToInt32(Request.QueryString["ID"]) == -1)
                    dtvPickingItem.DefaultMode = DetailsViewMode.Insert;
            }

        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //Session[MenuType.MenuPickingItems.ToString()] = null;
            ClearAllMenusCache(e.Values["ID_Company"].ToString());
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            {
                //Session[MenuType.MenuPickingItems.ToString()] = null;
                ClearAllMenusCache(e.NewValues["ID_Company"].ToString());
            }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //Session[MenuType.MenuPickingItems.ToString()] = null;
            ClearAllMenusCache(e.Values["ID_Company"].ToString());
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode != DetailsViewMode.ReadOnly)
            {
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            var rowCount = dtvPickingItem.Rows.Count;
            if (rowCount > 0)
            {
                SetDeleteConfirmation(dtvPickingItem.Rows[rowCount - 1]);

                var _lbtDeactivate = (LinkButton) dtvPickingItem.FindControl("lbtDeactivate");
                var _lbtActivate = (LinkButton) dtvPickingItem.FindControl("lbtActivate");

                using (var _qc = new QuotationDataContext())
                {
                    var _pickingItem =
                        _qc.PickingItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvPickingItem.DataKey.Value));
                    if (_pickingItem != null)
                    {
                        _lbtDeactivate.Enabled = _pickingItem.Inserted;
                        _lbtActivate.Enabled = !_pickingItem.Inserted;
                    }
                }
            }

            if (dtvPickingItem.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvPickingItem.Rows[0];

                var _dycCompany = (DynamicControl)_dvr.FindControl("dycCompany");
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).SelectedValue = CurrentCompanyId.ToString();
                // merge aziendale
                //((DropDownList)_dycCompany.Controls[0].Controls[0]).Enabled = false;
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
            ((PickingItem) e.NewObject).Inserted = true;
        }

        protected void dtvPickingItem_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Deactivate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _pickingItem =
                        _qc.PickingItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvPickingItem.DataKey.Value));
                    _pickingItem.Inserted = false;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuPickingItems.ToString()] = null;
                    ClearAllMenusCache(_pickingItem.ID_Company.ToString());
                }
            }
            if (e.CommandName == "Activate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _pickingItem =
                        _qc.PickingItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvPickingItem.DataKey.Value));
                    _pickingItem.Inserted = true;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuPickingItems.ToString()] = null;
                    ClearAllMenusCache(_pickingItem.ID_Company.ToString());
                }
            }
        }

        protected void dtvPickingItem_ItemCreated(object sender, EventArgs e)
        {
            var _lbtDeactivate = (LinkButton) dtvPickingItem.FindControl("lbtDeactivate");
            var _lbtActivate = (LinkButton) dtvPickingItem.FindControl("lbtActivate");

            using (var _qc = new QuotationDataContext())
            {
                var _pickingItem =
                    _qc.PickingItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvPickingItem.DataKey.Value));
                if (_pickingItem != null)
                {
                    if (_pickingItem.Inserted)
                    {
                        _lbtDeactivate.OnClientClick =
                            @"return confirm('Sei sicuro di voler disattivare definitivamente questa voce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo o di un gruppo voci già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
                    }
                    if (!_pickingItem.Inserted)
                    {
                        _lbtActivate.OnClientClick =
                            @"return confirm('Sei sicuro di voler riattivare questa voce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
                    }
                }
            }
        }

        protected void dtvPickingItem_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (Convert.ToDecimal(e.NewValues["Cost"]) != Convert.ToDecimal(e.OldValues["Cost"]))
                e.NewValues["Date"] = DateTime.Today;
        }
    }
}