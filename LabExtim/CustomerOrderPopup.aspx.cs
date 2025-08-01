using System;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class CustomerOrderPopup : QuotationController
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
            DynamicDataManager1.RegisterControl(dtvCustomerOrder);
            DynamicDataManager1.RegisterControl(grdProductionOrders);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblItemNo.Text = Request.QueryString["ID"] == "-1" ? " [Nuovo]" : " No " + Request.QueryString["ID"];
            ;
            if (Request.QueryString["ID"] == "-1")
            {
                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvCustomerOrder.ChangeMode(DetailsViewMode.Insert);
            }
        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //    //Session[MenuType.MenuCustomerOrders.ToString()] = null;
            //    Cache.Remove(MenuType.MenuCustomerOrders.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            //    if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            //    {
            //        //Session[MenuType.MenuCustomerOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuCustomerOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //    //Session[MenuType.MenuCustomerOrders.ToString()] = null;
            //    Cache.Remove(MenuType.MenuCustomerOrders.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode == DetailsViewMode.Insert)
            {
                lblItemNo.Text = " [Nuovo]";
            }
            else
            {
                lblItemNo.Text = " No " + Request.QueryString["ID"];
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            //int rowCount = dtvCustomerOrder.Rows.Count;
            //if (rowCount > 0)
            //{
            //    SetDeleteConfirmation(dtvCustomerOrder.Rows[rowCount - 1]);

            //    LinkButton _lbtDeactivate = (LinkButton)dtvCustomerOrder.FindControl("lbtDeactivate");
            //    LinkButton _lbtActivate = (LinkButton)dtvCustomerOrder.FindControl("lbtActivate");

            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        CustomerOrder _CustomerOrder = _qc.CustomerOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvCustomerOrder.DataKey.Value));
            //        if (_CustomerOrder != null)
            //        {
            //            _lbtDeactivate.Enabled = _CustomerOrder.Inserted;
            //            _lbtActivate.Enabled = !_CustomerOrder.Inserted;
            //        }
            //    }

            //}

            var _dvr = dtvCustomerOrder.Rows[0];

            if (dtvCustomerOrder.CurrentMode == DetailsViewMode.Insert)
            {
                var _dyc0 = (DynamicControl) _dvr.FindControl("dycStatuse");
                ((DropDownList) _dyc0.Controls[0].Controls[0]).SelectedValue = 4.ToString();
                ((DropDownList) _dyc0.Controls[0].Controls[0]).Enabled = false;

                var _dycCustomer = (DynamicControl) _dvr.FindControl("dycCustomer");
                var _curCustomerCode = ((DropDownList) _dycCustomer.Controls[0].Controls[0]).SelectedValue;
                if (_curCustomerCode != string.Empty)
                {
                    var _curContext =
                        (QuotationDataContext) dtvCustomerOrder.FindMetaTable().CreateContext();

                    var _dyc1 = (DynamicControl) _dvr.FindControl("dycQuotation");
                    ((DropDownList) _dyc1.Controls[0].Controls[0]).Items.Clear();
                    ((DropDownList) _dyc1.Controls[0].Controls[0]).Items.Add(new ListItem("[Non selezionato]", ""));
                    var _list1 =
                        _curContext.Quotations.Where(q => q.CustomerCode == Int32.Parse(_curCustomerCode))
                            .Select(q => new ListItem(q.Subject, q.ID.ToString()))
                            .ToArray();
                    ((DropDownList) _dyc1.Controls[0].Controls[0]).Items.AddRange(_list1);
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
            //((CustomerOrder)e.NewObject).Inserted = true;
        }

        protected void dtvCustomerOrder_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            //if (e.CommandName == "Deactivate")
            //{
            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        CustomerOrder _CustomerOrder = _qc.CustomerOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvCustomerOrder.DataKey.Value));
            //        _CustomerOrder.Inserted = false;
            //        _qc.SubmitChanges();
            //        //Session[MenuType.MenuCustomerOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuCustomerOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
            //}
            //if (e.CommandName == "Activate")
            //{
            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        CustomerOrder _CustomerOrder = _qc.CustomerOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvCustomerOrder.DataKey.Value));
            //        _CustomerOrder.Inserted = true;
            //        _qc.SubmitChanges();
            //        //Session[MenuType.MenuCustomerOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuCustomerOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
            //}
        }

        protected void dtvCustomerOrder_ItemCreated(object sender, EventArgs e)
        {
            //LinkButton _lbtDeactivate = (LinkButton)dtvCustomerOrder.FindControl("lbtDeactivate");
            //LinkButton _lbtActivate = (LinkButton)dtvCustomerOrder.FindControl("lbtActivate");

            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{
            //    CustomerOrder _CustomerOrder = _qc.CustomerOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvCustomerOrder.DataKey.Value));
            //    if (_CustomerOrder != null)
            //    {
            //        if (_CustomerOrder.Inserted)
            //        {
            //            _lbtDeactivate.OnClientClick = @"return confirm('Sei sicuro di voler disattivare definitivamente questa voce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo o di un gruppo voci già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
            //        }
            //        if (!_CustomerOrder.Inserted)
            //        {
            //            _lbtActivate.OnClientClick = @"return confirm('Sei sicuro di voler riattivare questa voce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
            //        }
            //    }
            //}
        }

        protected void ldsProductionOrders_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //MetaTable table = ldsProductionOrders.GetTable();
            //QuotationDataContext _qc = (QuotationDataContext)table.CreateContext();
            //switch (ddlOrderBy.SelectedValue)
            //{
            //    //case (""):
            //    //    GridDataSource.OrderByParameters.Clear();
            //    //    GridDataSource.AutoGenerateOrderByClause = false;
            //    //    e.Result = _qc.CustomerOrders.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
            //    //    break;
            //    case ("StatusName"):
            //        ldsProductionOrders.OrderByParameters.Clear();
            //        ldsProductionOrders.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.CustomerOrders.OrderBy(qt => qt.Statuse.Description);
            //        break;
            //    case ("CustomerName"):
            //        ldsProductionOrders.OrderByParameters.Clear();
            //        ldsProductionOrders.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.CustomerOrders.OrderBy(qt => qt.Customer.Name);
            //        break;
            //    default:
            //        break;

            //}
        }

        protected void grdProductionOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrders.PageIndex = e.NewPageIndex;
        }
    }
}