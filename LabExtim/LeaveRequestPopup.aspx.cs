using System;
using System.Linq;
using System.Data.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using DLLabExtim;
using CMLabExtim.WODClasses;
using UILabExtim;

namespace LabExtim
{
    public partial class LeaveRequestPopup : ProductionOrderController
    {


        public bool EditMode
        {
            get
            {
                if (ViewState["EditMode"] == null)
                {
                    ViewState["EditMode"] = false;
                }
                return Convert.ToBoolean(ViewState["EditMode"]);
            }
            set { ViewState["EditMode"] = value; }

        }

         
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(dtvLeaveRequest);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblItemNo.Text = LRIdParameter == -1 ? " [Nuovo]" : " No " + LRIdParameter;
            if (LRIdParameter == -1)
            {
                //LeaveRequest existing = new QuotationDataContext().LeaveRequests.FirstOrDefault(p => p.ID == DTIdParameter && p.ID_Company == CurrentCompanyId);
                // merge aziendale
                LeaveRequest existing = new QuotationDataContext().LeaveRequests.FirstOrDefault(p => p.ID == LRIdParameter);
                if (existing != null)
                {
                    Response.Redirect(string.Format("{0}&LRid={1}", this.Request.Url.ToString(), existing.ID), true);
                }

                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvLeaveRequest.ChangeMode(DetailsViewMode.Insert);
            }
            else
            {
                dtvLeaveRequest.AutoGenerateInsertButton = false;
            }


        }


        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //    //Session[MenuType.MenuLeaveRequests.ToString()] = null;
            //    Cache.Remove(MenuType.MenuLeaveRequests.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            //    if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            //    {
            //        //Session[MenuType.MenuLeaveRequests.ToString()] = null;
            //        Cache.Remove(MenuType.MenuLeaveRequests.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //    //Session[MenuType.MenuLeaveRequests.ToString()] = null;
            //    Cache.Remove(MenuType.MenuLeaveRequests.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
            dtvLeaveRequest.Visible = false;
            lblItemNo.Visible = false;
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

            if (dtvLeaveRequest.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvLeaveRequest.Rows[0];

                var _dycCompany = (DynamicControl)_dvr.FindControl("dycCompany");
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).SelectedValue = CurrentCompanyId.ToString();
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).Enabled = false;

                //if (POCustomerIdParameter != -1)
                //{
                //    var _dyc = (DynamicControl)_dvr.FindControl("dycCustomer");
                //    ((DropDownList)_dyc.Controls[0].Controls[0]).Enabled = false;

                //    if (POCustomerIdParameter == 0)
                //    {

                //        ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue =
                //            new QuotationDataContext().LeaveRequests.SingleOrDefault(q => q.ID == POQuotationIdParameter)
                //                .CustomerCode.Value.ToString();
                //    }
                //    else
                //    {
                //        ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = POCustomerIdParameter.ToString();
                //    }
                //}


                var _dyc3 = (DynamicControl)_dvr.FindControl("dycStartDate");
                ((TextBox)_dyc3.Controls[0].Controls[0]).Text = DateTime.Today.ToString("dd/MM/yyyy");

                //var _dyc0 = (DynamicControl)_dvr.FindControl("dycStatuse");
                //((DropDownList)_dyc0.Controls[0].Controls[0]).SelectedValue = 0.ToString();
                //((DropDownList)_dyc0.Controls[0].Controls[0]).Enabled = false;

            }
            else
            {
                dtvLeaveRequest.DataBind();

                if (dtvLeaveRequest.CurrentMode == DetailsViewMode.Edit)
                {
                    var _dvr = dtvLeaveRequest.Rows[0];
                    var _dyc = (DynamicControl)_dvr.FindControl("dycCustomer");
                    ((DropDownList)_dyc.Controls[0].Controls[0]).Enabled = false;
                }

            }
        }

        private void SetDeleteConfirmation(TableRow row)
        {
            foreach (Control c in row.Cells[0].Controls)
            {
                if (c is LinkButton)
                {
                    var btn = (LinkButton)c;
                    if (btn.CommandName == DataControlCommands.DeleteCommandName)
                    {
                        btn.OnClientClick = "return confirm('Sei sicuro di voler eliminare questa voce?');";
                    }
                }
            }
        }

        protected void DetailsDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            ((LeaveRequest)e.NewObject).Status = 0;
            //((LeaveRequest)e.NewObject).MacroRef = 411;
            
        }

        protected void dtvLeaveRequest_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            
        }

        protected void dtvLeaveRequest_ItemCreated(object sender, EventArgs e)
        {

        }

        protected void ldsLeaveRequestDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //var table = ldsLeaveRequestDetails.GetTable();
            //var _qc = (QuotationDataContext)table.CreateContext();

            //ldsLeaveRequestDetails.OrderByParameters.Clear();
            //ldsLeaveRequestDetails.AutoGenerateOrderByClause = false;
            //e.Result = _qc.LeaveRequestDetails.OrderBy(pi => pi.InsertDate);

        }

        protected void DetailsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvLeaveRequest.CurrentMode != DetailsViewMode.Insert)
            {
                var table = DetailsDataSource.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.LeaveRequests.Where(po => po.ID == DTIdParameter);
            }
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            //using (QuotationDataContext db = new QuotationDataContext())
            //{
            //    var _newQuotation =
            //        db.Quotations.SingleOrDefault(q => q.ID == ((LeaveRequest)e.Result).ID_Quotation);
            //    //if (_newQuotation.Q3 == -1 && _newQuotation.Q4 == -1 && _newQuotation.Q5 == -1)
            //    if (((LeaveRequest)e.Result).Price != null)
            //    {
            //        _newQuotation.Subject = ((LeaveRequest)e.Result).Description + " (Automatico da OdP " +
            //                                ((LeaveRequest)e.Result).Number + " [" +
            //                                ((LeaveRequest)e.Result).ID + "])";
            //    }
            //    else
            //    {
            //        LeaveRequestService.SyncroniseQuotationSubject(db, (LeaveRequest)e.Result);
            //        LeaveRequestService.DeleteLeaveRequestSchedule(db, ((LeaveRequest)e.Result));
            //        if (((LeaveRequest)e.Result).Status == 1)
            //        {
            //            LeaveRequestService.CreateLeaveRequestSchedule(db, ((LeaveRequest)e.Result), Global.CurrentSchedulingType);
            //        }
            //    }
            //    db.SubmitChanges();
            //}
            Response.Redirect("LeaveRequestPopup.aspx?" + DTIdKey + "=" + ((LeaveRequest)e.Result).ID);
        }

        protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            //if (((LeaveRequest)e.NewObject).ID_Quotation == null) // allora crea preventivo fittizio
            //{
            //    ((LeaveRequest)e.NewObject).ID_Quotation = CreateDummyQuotation((LeaveRequest)e.NewObject);
            //}
            //else // allora modifica preventivo fittizio, NO IL PREVENTIVO FITTIZIO SI MODIFICA A MANO E DIRETTAMENTE!
            //{
            //    ((LeaveRequest)e.NewObject).ID_Quotation = UpdateDummyQuotation((LeaveRequest)e.NewObject);

            //    using (QuotationDataContext db = new QuotationDataContext())
            //    {
            //        LeaveRequestService.SyncroniseQuotationSubject(db, (LeaveRequest)e.NewObject);

            //        if (((LeaveRequest)e.NewObject).Quantity != ((LeaveRequest)e.OriginalObject).Quantity ||
            //            ((LeaveRequest)e.NewObject).DeliveryDate != ((LeaveRequest)e.OriginalObject).DeliveryDate)
            //        {
            //            LeaveRequestService.DeleteLeaveRequestSchedule(db, ((LeaveRequest)e.NewObject));
            //            if (((LeaveRequest)e.NewObject).Status == 1)
            //            {
            //                LeaveRequestService.CreateLeaveRequestSchedule(db, ((LeaveRequest)e.NewObject), Global.CurrentSchedulingType);
            //            }
            //        }
            //        if (((LeaveRequest)e.NewObject).Status == 3)
            //        {
            //            LeaveRequestService.CloseLeaveRequestSchedule(db, ((LeaveRequest)e.NewObject));
            //        }
            //        if (((LeaveRequest)e.OriginalObject).Status == 3 && ((LeaveRequest)e.NewObject).Status == 1)
            //        {
            //            LeaveRequestService.DeleteLeaveRequestSchedule(db, ((LeaveRequest)e.NewObject), true);
            //            LeaveRequestService.CreateLeaveRequestSchedule(db, ((LeaveRequest)e.NewObject), Global.CurrentSchedulingType);
            //        }
            //        db.SubmitChanges();
            //    }
            //}
            //grdProductionMPS.DataBind();


        }

        protected void dtvLeaveRequest_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            //if (e.NewValues["ID_Customer"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CustomerIsMandatory);
            //    e.Cancel = true;
            //}
            //if (e.NewValues["Quantity"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
            //    e.Cancel = true;
            //}
            ////if (e.NewValues["ID_Quotation"] == null && e.NewValues["Price"] == null)
            ////{
            ////    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.PriceIsMandatoryForAuto);
            ////    e.Cancel = true;
            ////}
            //if (e.NewValues["DeliveryDate"] == null && e.NewValues["DeliveryDate"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsMandatory);
            //    e.Cancel = true;
            //}

        }

        protected void dtvLeaveRequest_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //if (e.Values["ID_Customer"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CustomerIsMandatory);
            //    e.Cancel = true;
            //}
            //if (e.Values["Quantity"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
            //    e.Cancel = true;
            //}
            ////if (e.Values["ID_Quotation"] == null && e.Values["Price"] == null)
            ////{
            ////    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.PriceIsMandatoryForAuto);
            ////    e.Cancel = true;
            ////}
            //if (e.Values["DeliveryDate"] == null && e.Values["DeliveryDate"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsMandatory);
            //    e.Cancel = true;
            //}
            //else
            //{
            //    DateTime deliveryDate = DateTime.Parse(e.Values["DeliveryDate"].ToString());
            //    if (deliveryDate < DateTime.Today)
            //    {
            //        ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsInvalid);
            //        e.Cancel = true;
            //    }
            //}
        }

        protected void grdLeaveRequestDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    var _bound = (LeaveRequestDetail)e.Row.DataItem;
            //    e.Row.FindControl("dycOwner").Visible = (_bound.FreeTypeCode == null);
            //    e.Row.FindControl("dycPhase").Visible = (_bound.FreeTypeCode == null);
            //    e.Row.FindControl("dycProductionTime").Visible = (_bound.FreeTypeCode == null);

            //    e.Row.FindControl("dycRawMaterial").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterial").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycRawMaterial").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.RFlag == null);

            //    e.Row.FindControl("dycUMRawMaterial").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycUMRawMaterial").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycUMRawMaterial").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.RFlag == null);

            //    e.Row.FindControl("dycRawMaterialQuantity").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterialQuantity").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycRawMaterialQuantity").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.RFlag == null);

            //    e.Row.FindControl("dycSupplier").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycSupplier").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycSupplier").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.RFlag == null);

            //    e.Row.FindControl("dycRawMaterialSup").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterialSup").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycRawMaterialSup").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycSupplierSup").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycSupplierSup").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycSupplierSup").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycUMUser").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycUMUser").Controls[0].Controls[0] is DropDownList)
            //        ((DropDownList)e.Row.FindControl("dycUMUser").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycRawMaterialX").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterialX").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycRawMaterialX").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycOkCopiesCount").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycOkCopiesCount").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycOkCopiesCount").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycKoCopiesCount").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycKoCopiesCount").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycKoCopiesCount").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycRawMaterialY").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterialY").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycRawMaterialY").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycRawMaterialZ").Visible = (_bound.FreeTypeCode == null);
            //    if (e.Row.FindControl("dycRawMaterialZ").Controls[0].Controls[0] is TextBox)
            //        ((TextBox)e.Row.FindControl("dycRawMaterialZ").Controls[0].Controls[0]).Visible =
            //            (_bound.FreeTypeCode == null && _bound.SFlag == null);

            //    e.Row.FindControl("dycDirectSupply").Visible = (_bound.FreeTypeCode == null);

            //    e.Row.FindControl("dycFreeType").Visible = (_bound.FreeTypeCode != null);
            //    e.Row.FindControl("dycItemFreeType").Visible = (_bound.FreeTypeCode != null);
            //    e.Row.FindControl("dycFreeItemDescription").Visible = (_bound.FreeTypeCode != null);
            //}
        }

        protected void lbtShowHide_Click(object sender, EventArgs e)
        {
            tblTestata.Visible = !tblTestata.Visible;
        }

        protected void grdLeaveRequestDetails_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //using (var _ctx = new QuotationDataContext())
            //{
            //    var _curItem =
            //        _ctx.LeaveRequestDetails.SingleOrDefault(pod => pod.ID == Convert.ToInt32(e.Keys["ID"]));
            //    _curItem.HistoricalCostPhase = _curItem.CostCalcPhase;
            //    if (_curItem.RFlag == null)
            //        _curItem.HistoricalCostRawM = _curItem.CostCalcRawM;
            //    if (_curItem.SFlag == null)
            //        _curItem.HistoricalCostSupM = _curItem.CostCalcSupM;
            //    if (_curItem.FreeTypeCode != null)
            //        _curItem.ID_Phase = null;
            //    _ctx.SubmitChanges();
            //}
        }

        protected void DetailsDataSource_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {

        }


        protected void hypEdit_Click(object sender, ImageClickEventArgs e)
        {
            EditMode = !EditMode;
        }
   

    }
}