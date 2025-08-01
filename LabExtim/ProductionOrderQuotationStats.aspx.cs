using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrderQuotationStats : ProductionOrderController
    {
        public bool PrintableLayout
        {
            get { return Convert.ToBoolean(ViewState["PrintableLayout"]); }
            set { ViewState["PrintableLayout"] = value; }
        }

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
            //DynamicDataManager1.RegisterControl(dtvProductionOrder);
            //DynamicDataManager1.RegisterControl(grdProductionOrderDetails);
            //DynamicDataManager1.RegisterControl(grdQuotationDetails);
            //DynamicDataManager1.RegisterControl(grdPhasesDetails);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PrintableLayout = false;
                lbtNote.Attributes.Add("onclick",
                    "javascript:OpenBigItem3('PODetailsNotes.aspx?" + POIdKey + "=" + POIdParameter + "');return false;");
                dtvProductionOrder.ChangeMode(DetailsViewMode.ReadOnly);
                BindGridViews();

                using (var _ctx = new QuotationDataContext())
                {
                    VW_QUOPORCostsPrices_select stats = _ctx.VW_QUOPORCostsPrices_selects.FirstOrDefault(d => d.ID == POIdParameter);
                    if (stats != null)
                    {
                        lblQUOTotCostv.Text = stats.QUOTotCost.GetValueOrDefault().ToString("N2");
                        lblPORTotCostv.Text = stats.PORTotCost.GetValueOrDefault().ToString("N2");
                        lblPORTotHistoricalCostv.Text = stats.PORTotHistoricalCost.GetValueOrDefault().ToString("N2");
                        lblProvvTotValuev.Text = stats.ProvvTotValue.GetValueOrDefault().ToString("N2");
                        lblTotCostsv.Text = stats.TotCosts.GetValueOrDefault().ToString("N2");
                        lblFATTotValuev.Text = stats.FATTotValue.GetValueOrDefault().ToString("N2");
                        lblSavingv.Text = stats.Saving.GetValueOrDefault().ToString("N2");
                        lblPercentageSavingv.Text = stats.PercentageSaving.GetValueOrDefault().ToString("P2");
                  
                    
                    }
                }

            }
            using (var _ctx = new QuotationDataContext())
            {
                ProductionOrder current = _ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter);
                lblItemNo.Text = string.Format(" Id {0} - Numero {1} - ({2}) del Cliente {3}", current.ID, current.Number, current.Description, current.Customer.Name);

                //lblItemNo.Text = string.Format("No {0} ({1}) del Cliente {2}",
                //    POIdParameter,
                //    _ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter).Description,
                //    _ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter).Customer.Name);
                //if (!IsPostBack)
            }
        }

        protected void BindGridViews()
        {
            using (var _ctx = new QuotationDataContext())
            {
                var _curQuotationId =
                    Convert.ToInt32(_ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter).ID_Quotation);
                IEnumerable<StatGroupedDetail> _details = _ctx.QuotationDetails.Where(
                    qd => qd.ID_Quotation == _curQuotationId).OrderBy(qd => qd.Position).
                    Select(qd => new StatGroupedDetail
                    {
                        TypeDescription = qd.Type.Description,
                        ItemTypeDescription = qd.ItemType.Description,
                        ItemDescription = qd.ItemTypeDescription,
                        UnitDescription = qd.Unit.Description,
                        //Quantity = qd.Quantity,
                        Quantity = Convert.ToSingle(qd.Quantity) *
                                   (qd.Multiply
                                       ? Convert.ToSingle(
                                           _ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter).Quantity) /
                                         Convert.ToSingle(qd.Quotation.Q1)
                                       : 1f),
                        Cost = Convert.ToSingle(qd.Cost) * qd.Quantity *
                               (qd.Multiply
                                   ? Convert.ToSingle(
                                       _ctx.ProductionOrders.SingleOrDefault(po => po.ID == POIdParameter).Quantity) /
                                     Convert.ToSingle(qd.Quotation.Q1)
                                   : 1f),
                        SupplierName = qd.Supplier.Name
                    });
                grdQuotationDetails.DataSource = _details;
                grdQuotationDetails.Columns[5].FooterText = _details.Sum(d => d.Cost).Value.ToString("N2");
                grdQuotationDetails.Columns[6].FooterText = _details.Sum(d => d.HistoricalCost).Value.ToString("N2");
                grdQuotationDetails.DataBind();
            }
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            lblItemNo.Text = " No " + Request.QueryString["ID"];
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            dtvProductionOrder.DataBind();
            if (dtvProductionOrder.CurrentMode == DetailsViewMode.Edit)
            {
                var _dvr = dtvProductionOrder.Rows[0];
                var _dyc = (DynamicControl)_dvr.FindControl("dycCustomer");
                ((DropDownList)_dyc.Controls[0].Controls[0]).Enabled = false;
            }
            var _dvr1 = dtvProductionOrder.Rows[0];
            var _dyc1 = (DynamicControl)_dvr1.FindControl("dycNoteFromProduction");
            ((TextBox)_dyc1.Controls[0].Controls[0]).Text = ProductionOrderService.GetNoteFromProduction(POIdParameter);
            var _dyc2 = (DynamicControl)_dvr1.FindControl("dycAccountNoteFromProduction");
            ((TextBox)_dyc2.Controls[0].Controls[0]).Text = ProductionOrderService.GetAccountNotesFromProduction(POIdParameter);

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

        //protected void dtvProductionOrder_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        //{
        //    //if (e.CommandName == "Deactivate")
        //    //{
        //    //    using (QuotationDataContext _qc = new QuotationDataContext())
        //    //    {
        //    //        ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
        //    //        _ProductionOrder.Inserted = false;
        //    //        _qc.SubmitChanges();
        //    //        //Session[MenuType.MenuProductionOrders.ToString()] = null;
        //    //        Cache.Remove(MenuType.MenuProductionOrders.ToString());
        //    //        Cache.Remove(MenuType.MenuOperations.ToString());
        //    //    }
        //    //}
        //    //if (e.CommandName == "Activate")
        //    //{
        //    //    using (QuotationDataContext _qc = new QuotationDataContext())
        //    //    {
        //    //        ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
        //    //        _ProductionOrder.Inserted = true;
        //    //        _qc.SubmitChanges();
        //    //        //Session[MenuType.MenuProductionOrders.ToString()] = null;
        //    //        Cache.Remove(MenuType.MenuProductionOrders.ToString());
        //    //        Cache.Remove(MenuType.MenuOperations.ToString());
        //    //    }
        //    //}
        //}

        //protected void dtvProductionOrder_ItemCreated(object sender, EventArgs e)
        //{
        //    //LinkButton _lbtDeactivate = (LinkButton)dtvProductionOrder.FindControl("lbtDeactivate");
        //    //LinkButton _lbtActivate = (LinkButton)dtvProductionOrder.FindControl("lbtActivate");

        //    //using (QuotationDataContext _qc = new QuotationDataContext())
        //    //{
        //    //    ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
        //    //    if (_ProductionOrder != null)
        //    //    {
        //    //        if (_ProductionOrder.Inserted)
        //    //        {
        //    //            _lbtDeactivate.OnClientClick = @"return confirm('Sei sicuro di voler disattivare definitivamente questa voce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo o di un gruppo voci già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
        //    //        }
        //    //        if (!_ProductionOrder.Inserted)
        //    //        {
        //    //            _lbtActivate.OnClientClick = @"return confirm('Sei sicuro di voler riattivare questa voce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
        //    //        }
        //    //    }
        //    //}
        //}

        //protected void ldsProductionOrderDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    using (var _ctx = new QuotationDataContext())
        //    {
        //        var _groupedDetails = new List<StatGroupedDetail>();

        //        var _rPhase =
        //            _ctx.prc_LAB_MGet_LAB_PODetailsGroupedByPhaseAndPOID(POIdParameter).ToList();
        //        var _rRawMat =
        //            _ctx.prc_LAB_MGet_LAB_PODetailsGroupedByRawMaterialAndPOID(POIdParameter).ToList();
        //        var _rSecMat =
        //            _ctx.prc_LAB_MGet_LAB_PODetailsGroupedBySecMaterialAndPOID(POIdParameter).ToList();

        //        if (_rPhase.Count > 0)
        //            _groupedDetails.AddRange(_rPhase);
        //        if (_rRawMat.Count > 0)
        //            _groupedDetails.AddRange(_rRawMat);
        //        if (_rSecMat.Count > 0)
        //            _groupedDetails.AddRange(_rSecMat);

        //        e.Result = _groupedDetails.OrderBy(gd => gd.Order);
        //        grdProductionOrderDetails.Columns[5].FooterText = _groupedDetails.Sum(gd => gd.Cost)
        //            .Value.ToString("N2");
        //        grdProductionOrderDetails.Columns[6].FooterText =
        //            _groupedDetails.Sum(gd => gd.HistoricalCost).Value.ToString("N2");
        //    }
        //}

        //protected void grdProductionOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdProductionOrderDetails.PageIndex = e.NewPageIndex;
        //    grdProductionOrderDetails.DataBind();
        //}

        protected void ldsProductionOrderDetails_MV_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (var _ctx = new QuotationDataContext())
            {
                var _groupedDetails = new List<StatGroupedDetail>();

                var _rPhase =
                    _ctx.prc_LAB_MGet_LAB_PODetailsGroupedByPhaseAndPOID_MV(POIdParameter).ToList();
                var _rRawMat =
                    _ctx.prc_LAB_MGet_LAB_PODetailsGroupedByRawMaterialAndPOID_MV(POIdParameter).ToList();
                var _rSecMat =
                    _ctx.prc_LAB_MGet_LAB_PODetailsGroupedBySecMaterialAndPOID_MV(POIdParameter).ToList();

                if (_rPhase.Count > 0)
                    _groupedDetails.AddRange(_rPhase);
                if (_rRawMat.Count > 0)
                    _groupedDetails.AddRange(_rRawMat);
                if (_rSecMat.Count > 0)
                    _groupedDetails.AddRange(_rSecMat);

                e.Result = _groupedDetails.OrderBy(gd => gd.Order);
                grdProductionOrderDetails_MV.Columns[5].FooterText = _groupedDetails.Sum(gd => gd.Cost)
                    .Value.ToString("N2");
                grdProductionOrderDetails_MV.Columns[6].FooterText =
                    _groupedDetails.Sum(gd => gd.HistoricalCost).Value.ToString("N2");
            }
        }

        protected void grdProductionOrderDetails_MV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderDetails_MV.PageIndex = e.NewPageIndex;
            grdProductionOrderDetails_MV.DataBind();
        }

        protected void DetailsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvProductionOrder.CurrentMode != DetailsViewMode.Insert)
            {
                var table = DetailsDataSource.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.ProductionOrders.Where(po => po.ID == POIdParameter);
            }
        }

        protected void lbtPrintProductionOrder_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            PrintProductionOrder(_qc);
        }

        protected void lbtCloneProductionOrder_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            CloneProductionOrder(_qc, Global.CurrentSchedulingType);
        }

        //protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        //{
        //    if (((ProductionOrder)e.NewObject).ID_Quotation == null) // allora crea preventivo fittizio
        //    {
        //        ((ProductionOrder)e.NewObject).ID_Quotation = CreateDummyQuotation((ProductionOrder)e.NewObject);
        //    }
        //    else // allora modifica preventivo fittizio
        //    {
        //        ((ProductionOrder)e.NewObject).ID_Quotation = UpdateDummyQuotation((ProductionOrder)e.NewObject);
        //    }
        //}

        //protected void dtvProductionOrder_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        //{
        //    if (e.NewValues["ID_Customer"] == null)
        //    {
        //        ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CustomerIsMandatory);
        //        e.Cancel = true;
        //    }
        //    if (e.NewValues["Quantity"] == null)
        //    {
        //        ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
        //        e.Cancel = true;
        //    }
        //    //if (e.NewValues["ID_Quotation"] == null && e.NewValues["Price"] == null)
        //    //if (e.OldValues["ID_Quotation"] == null && e.NewValues["Price"] == null)
        //    //{
        //    //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.PriceIsMandatoryForAuto);
        //    //    e.Cancel = true;
        //    //}
        //}

        protected void grdQuotationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdQuotationDetails.PageIndex = e.NewPageIndex;
            grdQuotationDetails.DataBind();
        }

        protected void ldsQuotationDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
        }

        protected void ldsPhasesDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (var _ctx = new QuotationDataContext())
            {
                var _details =
                    _ctx.prc_LAB_MGet_LAB_PODetailsOnlyPhasesByPOID(POIdParameter).ToList();
                e.Result = _details;
                grdPhasesDetails.Columns[3].FooterText = _details.Sum(gd => gd.ProductionTime).Value.ToString("N2");
                grdPhasesDetails.Columns[4].FooterText = _details.Sum(gd => gd.ProductionCost).Value.ToString("N2");
                grdPhasesDetails.Columns[5].FooterText = _details.Sum(gd => gd.OkCopiesCount).Value.ToString("N0");
                grdPhasesDetails.Columns[6].FooterText = _details.Sum(gd => gd.KoCopiesCount).Value.ToString("N0");
                grdPhasesDetails.Columns[7].FooterText = _details.Sum(gd => gd.ProducedQuantity).Value.ToString("N0");
            }
        }

        protected void grdPhasesDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPhasesDetails.PageIndex = e.NewPageIndex;
            grdPhasesDetails.DataBind();
        }

        protected void lbtPrintPOFinalCost_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            PrintPOFinalCost(_qc);
        }

        protected void ldsDNDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (var _ctx = new QuotationDataContext())
            {
                var _details =
                    _ctx.prc_LAB_MGet_LAB_DNDetailsByPOID(POIdParameter)
                        .Select(d => new prc_LAB_MGet_LAB_DNDetailsByPOIDResult
                        {
                            mm_anno = d.mm_anno,
                            mm_codart = d.mm_codart,
                            mm_datfin = d.mm_datfin,
                            mm_descr = d.mm_descr,
                            mm_desint = d.mm_desint,
                            mm_lotto = d.mm_lotto,
                            mm_numdoc = d.mm_numdoc,
                            mm_prezzo = (d.mm_valore.Value != 0m ? d.mm_valore.Value / d.mm_quant : 0m),
                            mm_quant = d.mm_quant,
                            mm_riga = d.mm_riga,
                            mm_serie = d.mm_serie,
                            mm_tipork = d.mm_tipork,
                            mm_ump = d.mm_ump,
                            mm_unmis = d.mm_unmis,
                            mm_valore = d.mm_valore,
                            mm_vprovv = d.mm_vprovv
                        }
                        ).ToList();
                e.Result = _details;
                grdDNDetails.Columns[9].FooterText = _details.Sum(gd => gd.mm_valore).GetValueOrDefault().ToString("N2");
                grdDNDetails.Columns[10].FooterText =
                    _details.Sum(gd => gd.mm_vprovv.GetValueOrDefault(0m)).ToString("N2");
            }
        }

        protected void grdDNDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDNDetails.PageIndex = e.NewPageIndex;
            grdDNDetails.DataBind();
        }

        protected void lbtOptimizeLayout_Click(object sender, EventArgs e)
        {
            PrintableLayout = !PrintableLayout;
            if (PrintableLayout)
            {
                divDetails.Visible = false;
                tblStats.Visible = false;
                tdDetails.Width = "0px";
                grdQuotationDetails.PageSize = 64000;
                grdDNDetails.PageSize = 64000;
                grdPhasesDetails.PageSize = 64000;
                //grdProductionOrderDetails.PageSize = 64000;
                grdProductionOrderDetails_MV.PageSize = 64000;
            }
            else
            {
                divDetails.Visible = true;
                tblStats.Visible = true;
                tdDetails.Width = "";

                grdQuotationDetails.PageSize = 10;
                grdDNDetails.PageSize = 10;
                grdPhasesDetails.PageSize = 10;
                //grdProductionOrderDetails.PageSize = 10;
                grdProductionOrderDetails_MV.PageSize = 10;
            }

            grdQuotationDetails.PageIndex = 0;
            grdDNDetails.PageIndex = 0;
            grdPhasesDetails.PageIndex = 0;
            //grdProductionOrderDetails.PageIndex = 0;
            grdProductionOrderDetails_MV.PageIndex = 0;


            BindGridViews();
        }

    }
}