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
    public partial class DeliveryTripPopup : ProductionOrderController
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
            DynamicDataManager1.RegisterControl(dtvDeliveryTrip);
            DynamicDataManager1.RegisterControl(grdDeliveryTripDetails);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblItemNo.Text = DTIdParameter == -1 ? " [Nuovo]" : " No " + DTIdParameter;
            if (DTIdParameter == -1)
            {
                //DeliveryTrip existing = new QuotationDataContext().DeliveryTrips.FirstOrDefault(p => p.ID == DTIdParameter && p.ID_Company == CurrentCompanyId);
                // merge aziendale
                DeliveryTrip existing = new QuotationDataContext().DeliveryTrips.FirstOrDefault(p => p.ID == DTIdParameter);
                if (existing != null)
                {
                    Response.Redirect(string.Format("{0}&DTid={1}", this.Request.Url.ToString(), existing.ID), true);
                }

                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvDeliveryTrip.ChangeMode(DetailsViewMode.Insert);
                lblDetails.Visible = false;
                grdDeliveryTripDetails.Visible = false;
            }
            else
            {
                dtvDeliveryTrip.AutoGenerateInsertButton = false;
                if (!IsPostBack)
                {
                    BindGrids();
                }
            }


        }

        public void BindGrids()
        {
            grdDeliveryTripDetails.DataBind();
        }


        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //    //Session[MenuType.MenuDeliveryTrips.ToString()] = null;
            //    Cache.Remove(MenuType.MenuDeliveryTrips.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            //    if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            //    {
            //        //Session[MenuType.MenuDeliveryTrips.ToString()] = null;
            //        Cache.Remove(MenuType.MenuDeliveryTrips.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //    //Session[MenuType.MenuDeliveryTrips.ToString()] = null;
            //    Cache.Remove(MenuType.MenuDeliveryTrips.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
            dtvDeliveryTrip.Visible = false;
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

            if (dtvDeliveryTrip.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvDeliveryTrip.Rows[0];

                var _dycCompany = (DynamicControl)_dvr.FindControl("dycCompany");
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).SelectedValue = CurrentCompanyId.ToString();
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).Enabled = false;

                if (POCustomerIdParameter != -1)
                {
                    var _dyc = (DynamicControl)_dvr.FindControl("dycCustomer");
                    ((DropDownList)_dyc.Controls[0].Controls[0]).Enabled = false;

                    if (POCustomerIdParameter == 0)
                    {

                        ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue =
                            new QuotationDataContext().DeliveryTrips.SingleOrDefault(q => q.ID == POQuotationIdParameter)
                                .CustomerCode.Value.ToString();
                    }
                    else
                    {
                        ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = POCustomerIdParameter.ToString();
                    }
                }


                var _dyc3 = (DynamicControl)_dvr.FindControl("dycStartDate");
                ((TextBox)_dyc3.Controls[0].Controls[0]).Text = DateTime.Today.ToString("dd/MM/yyyy");

                //var _dyc0 = (DynamicControl)_dvr.FindControl("dycStatuse");
                //((DropDownList)_dyc0.Controls[0].Controls[0]).SelectedValue = 0.ToString();
                //((DropDownList)_dyc0.Controls[0].Controls[0]).Enabled = false;

            }
            else
            {
                dtvDeliveryTrip.DataBind();

                if (dtvDeliveryTrip.CurrentMode == DetailsViewMode.Edit)
                {
                    var _dvr = dtvDeliveryTrip.Rows[0];
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
            ((DeliveryTrip)e.NewObject).Status = 0;
            ((DeliveryTrip)e.NewObject).MacroRef = 411;
            
        }

        protected void dtvDeliveryTrip_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            
        }

        protected void dtvDeliveryTrip_ItemCreated(object sender, EventArgs e)
        {

        }

        protected void ldsDeliveryTripDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsDeliveryTripDetails.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            ldsDeliveryTripDetails.OrderByParameters.Clear();
            ldsDeliveryTripDetails.AutoGenerateOrderByClause = false;
            e.Result = _qc.DeliveryTripDetails.OrderBy(pi => pi.InsertDate);

        }

        protected void grdDeliveryTripDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDeliveryTripDetails.PageIndex = e.NewPageIndex;
        }

        protected void DetailsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvDeliveryTrip.CurrentMode != DetailsViewMode.Insert)
            {
                var table = DetailsDataSource.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.DeliveryTrips.Where(po => po.ID == DTIdParameter);
            }
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            //using (QuotationDataContext db = new QuotationDataContext())
            //{
            //    var _newQuotation =
            //        db.Quotations.SingleOrDefault(q => q.ID == ((DeliveryTrip)e.Result).ID_Quotation);
            //    //if (_newQuotation.Q3 == -1 && _newQuotation.Q4 == -1 && _newQuotation.Q5 == -1)
            //    if (((DeliveryTrip)e.Result).Price != null)
            //    {
            //        _newQuotation.Subject = ((DeliveryTrip)e.Result).Description + " (Automatico da OdP " +
            //                                ((DeliveryTrip)e.Result).Number + " [" +
            //                                ((DeliveryTrip)e.Result).ID + "])";
            //    }
            //    else
            //    {
            //        DeliveryTripService.SyncroniseQuotationSubject(db, (DeliveryTrip)e.Result);
            //        DeliveryTripService.DeleteDeliveryTripSchedule(db, ((DeliveryTrip)e.Result));
            //        if (((DeliveryTrip)e.Result).Status == 1)
            //        {
            //            DeliveryTripService.CreateDeliveryTripSchedule(db, ((DeliveryTrip)e.Result), Global.CurrentSchedulingType);
            //        }
            //    }
            //    db.SubmitChanges();
            //}
            Response.Redirect("DeliveryTripPopup.aspx?" + DTIdKey + "=" + ((DeliveryTrip)e.Result).ID);
        }

        protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            //if (((DeliveryTrip)e.NewObject).ID_Quotation == null) // allora crea preventivo fittizio
            //{
            //    ((DeliveryTrip)e.NewObject).ID_Quotation = CreateDummyQuotation((DeliveryTrip)e.NewObject);
            //}
            //else // allora modifica preventivo fittizio, NO IL PREVENTIVO FITTIZIO SI MODIFICA A MANO E DIRETTAMENTE!
            //{
            //    ((DeliveryTrip)e.NewObject).ID_Quotation = UpdateDummyQuotation((DeliveryTrip)e.NewObject);

            //    using (QuotationDataContext db = new QuotationDataContext())
            //    {
            //        DeliveryTripService.SyncroniseQuotationSubject(db, (DeliveryTrip)e.NewObject);

            //        if (((DeliveryTrip)e.NewObject).Quantity != ((DeliveryTrip)e.OriginalObject).Quantity ||
            //            ((DeliveryTrip)e.NewObject).DeliveryDate != ((DeliveryTrip)e.OriginalObject).DeliveryDate)
            //        {
            //            DeliveryTripService.DeleteDeliveryTripSchedule(db, ((DeliveryTrip)e.NewObject));
            //            if (((DeliveryTrip)e.NewObject).Status == 1)
            //            {
            //                DeliveryTripService.CreateDeliveryTripSchedule(db, ((DeliveryTrip)e.NewObject), Global.CurrentSchedulingType);
            //            }
            //        }
            //        if (((DeliveryTrip)e.NewObject).Status == 3)
            //        {
            //            DeliveryTripService.CloseDeliveryTripSchedule(db, ((DeliveryTrip)e.NewObject));
            //        }
            //        if (((DeliveryTrip)e.OriginalObject).Status == 3 && ((DeliveryTrip)e.NewObject).Status == 1)
            //        {
            //            DeliveryTripService.DeleteDeliveryTripSchedule(db, ((DeliveryTrip)e.NewObject), true);
            //            DeliveryTripService.CreateDeliveryTripSchedule(db, ((DeliveryTrip)e.NewObject), Global.CurrentSchedulingType);
            //        }
            //        db.SubmitChanges();
            //    }
            //}
            //grdProductionMPS.DataBind();
            BindGrids();



        }

        protected void dtvDeliveryTrip_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
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

        protected void dtvDeliveryTrip_ItemInserting(object sender, DetailsViewInsertEventArgs e)
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

        protected void grdDeliveryTripDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    var _bound = (DeliveryTripDetail)e.Row.DataItem;
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

        protected void grdDeliveryTripDetails_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            //using (var _ctx = new QuotationDataContext())
            //{
            //    var _curItem =
            //        _ctx.DeliveryTripDetails.SingleOrDefault(pod => pod.ID == Convert.ToInt32(e.Keys["ID"]));
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


        protected void lbtShowHideDet_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = !pnlDetails.Visible;
        }

        protected void hypEdit_Click(object sender, ImageClickEventArgs e)
        {
            EditMode = !EditMode;
            BindGrids();
        }
   

    }
}