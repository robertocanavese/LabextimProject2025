using System;
using System.Linq;
using System.Data.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using DLLabExtim;
using CMLabExtim;
using CMLabExtim.WODClasses;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrderPopup : ProductionOrderController
    {

        public Query_OperationCount CurProducedQuantity;
        public DHL CurOrderOnBOBST1;

        public OdPBag CurOrderOnBOBST { get { return ViewState["CurOrderOnBOBST"] as OdPBag; } set { ViewState["CurOrderOnBOBST"] = value; } }
        public OdPBag HistoricalDataFromBOBST { get { return ViewState["CurOrderOnBOBST"] as OdPBag; } set { ViewState["CurOrderOnBOBST"] = value; } }
        public OdPBag CurOrderOnEcoSystem { get { return ViewState["CurOrderOnBOBST"] as OdPBag; } set { ViewState["CurOrderOnBOBST"] = value; } }
        public OdPBag HistoricalDataFromEcoSystem { get { return ViewState["HistoricalDataFromEcoSystem"] as OdPBag; } set { ViewState["HistoricalDataFromEcoSystem"] = value; } }
        public OdPBag CurOrderOnSilkFoil { get { return ViewState["CurOrderOnSilkFoil"] as OdPBag; } set { ViewState["CurOrderOnSilkFoil"] = value; } }
        public OdPBag HistoricalDataFromSilkFoil { get { return ViewState["HistoricalDataFromSilkFoil"] as OdPBag; } set { ViewState["HistoricalDataFromSilkFoil"] = value; } }
        public OdPBag CurOrderOnEuroProgetti { get { return ViewState["CurOrderOnEuroProgetti"] as OdPBag; } set { ViewState["CurOrderOnEuroProgetti"] = value; } }
        public OdPBag HistoricalDataFromEuroProgetti { get { return ViewState["HistoricalDataFromEuroProgetti"] as OdPBag; } set { ViewState["HistoricalDataFromEuroProgetti"] = value; } }
        public OdPBag CurOrderOnZechini { get { return ViewState["CurOrderOnZechini"] as OdPBag; } set { ViewState["CurOrderOnZechini"] = value; } }
        public OdPBag HistoricalDataFromZechini { get { return ViewState["HistoricalDataFromZechini"] as OdPBag; } set { ViewState["HistoricalDataFromZechini"] = value; } }

        public int SelectedRowId { get { return Convert.ToInt32(ViewState["SelectedRowId"]); } set { ViewState["SelectedRowId"] = value; } }

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
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
            DynamicDataManager1.RegisterControl(dtvProductionOrder);
            DynamicDataManager1.RegisterControl(grdProductionOrderDetails);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblItemNo.Text = POIdParameter == -1 ? " [Nuovo]" : " No " + POIdParameter;
            if (POIdParameter == -1)
            {
                ProductionOrder existing = new QuotationDataContext().ProductionOrders.FirstOrDefault(p => p.Quotation.ID == POQuotationIdParameter && p.ID_Company == CurrentCompanyId);
                if (existing != null)
                {
                    Response.Redirect(string.Format("{0}&POid={1}", this.Request.Url.ToString(), existing.ID), true);
                }

                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvProductionOrder.ChangeMode(DetailsViewMode.Insert);
                lblDetails.Visible = false;
                grdProductionOrderDetails.Visible = false;
                //grdDeliveryTrips.Visible = false;
                //lbtCloneProductionOrder.Enabled = false;
                lbtPrintProductionOrder.Enabled = false;

                //TransferAvailableData();
            }
            else
            {
                dtvProductionOrder.AutoGenerateInsertButton = false;
                if (!IsPostBack)
                {
                    using (QuotationDataContext db = new QuotationDataContext())
                    {
                        ProductionOrder existing = db.ProductionOrders.FirstOrDefault(p => p.ID == POIdParameter);
                        lblItemNo.Text = string.Format(" Id {0} - Numero {1}", existing.ID, existing.Number);

                        CurOrderOnBOBST = GetRealTimeDataFromBOBST();
                        //HistoricalDataFromBOBST = GetHistoricalDataFromBOBST(POIdParameter, Convert.ToInt32(curItem.Quantity));
                        HistoricalDataFromBOBST = GetHistoricalDataFromBOBST(POIdParameter, Convert.ToInt32(existing.Quantity));
                        CurOrderOnEcoSystem = GetRealTimeDataFromEcoSystem(db);
                        HistoricalDataFromEcoSystem = GetHistoricalDataFromEcoSystem(POIdParameter, db);
                        CurOrderOnSilkFoil = GetRealTimeDataFromSilkFoil(db);
                        HistoricalDataFromSilkFoil = GetHistoricalDataFromSilkFoil(POIdParameter, db);
                        CurOrderOnEuroProgetti = GetRealTimeDataFromEuroProgetti(db);
                        HistoricalDataFromEuroProgetti = GetHistoricalDataFromEuroProgetti(POIdParameter, db);
                        CurOrderOnZechini = GetRealTimeDataFromZechini(db);
                        HistoricalDataFromZechini = GetHistoricalDataFromZechini(POIdParameter, db);

                        BindGrids();
                    }
                }
            }


        }

        public void BindGrids()
        {

            List<VW_ProductionExtMPS_GroupedByPhase_Lite> data = null;
            using (QuotationDataContext db = new QuotationDataContext())
            {

                //data = db.VW_ProductionExtMPS_GroupedByPhases.Where(mp => mp.IDProductionOrder == POIdParameter && new int?[] { 11, 12 }.Contains(mp.Status)).OrderByDescending(mp => mp.Status).ThenBy(mp => Convert.ToInt32(mp.Order)).ThenBy(mp => mp.ProdStart).ToList();
                data = db.VW_ProductionExtMPS_GroupedByPhase_Lites.Where(mp => mp.IDProductionOrder == POIdParameter && new int?[] { 11, 12 }.Contains(mp.Status)).OrderByDescending(mp => mp.Status).ThenBy(mp => Convert.ToInt32(mp.Order)).ThenBy(mp => mp.ProdStart).ToList();


                if (data != null)
                    foreach (VW_ProductionExtMPS_GroupedByPhase_Lite curItem in data)
                    {

                        if (curItem.IDProductionMachine == 30 || curItem.IDProductionMachine == 31)
                        {
                            //CurOrderOnBOBST = GetRealTimeDataFromBOBST();
                            //HistoricalDataFromBOBST = GetHistoricalDataFromBOBST(POIdParameter, Convert.ToInt32(curItem.Quantity));
                            if (HistoricalDataFromBOBST != null)
                            {
                                curItem.DatiMacchinaCopieRichieste = HistoricalDataFromBOBST.CopieRichieste;
                                curItem.DatiMacchinaCopieLavorate = HistoricalDataFromBOBST.CopieLavorate;
                            }
                        }
                        if (curItem.IDProductionMachine == 19)
                        {
                            //CurOrderOnEcoSystem = GetRealTimeDataFromEcoSystem(db);
                            //HistoricalDataFromEcoSystem = GetHistoricalDataFromEcoSystem(POIdParameter, db);
                            if (HistoricalDataFromEcoSystem != null)
                            {
                                curItem.DatiMacchinaCopieRichieste = HistoricalDataFromEcoSystem.CopieRichieste;
                                curItem.DatiMacchinaCopieLavorate = HistoricalDataFromEcoSystem.CopieLavorate;
                            }
                        }
                        if (curItem.IDProductionMachine == 104)
                        {
                            //CurOrderOnSilkFoil = GetRealTimeDataFromSilkFoil(db);
                            //HistoricalDataFromSilkFoil = GetHistoricalDataFromSilkFoil(POIdParameter, db);
                            if (HistoricalDataFromSilkFoil != null)
                            {
                                curItem.DatiMacchinaCopieRichieste = HistoricalDataFromSilkFoil.CopieRichieste;
                                curItem.DatiMacchinaCopieLavorate = HistoricalDataFromSilkFoil.CopieLavorate;
                            }
                        }
                        if (curItem.IDProductionMachine == 77)
                        {
                            //CurOrderOnEuroProgetti = GetRealTimeDataFromEuroProgetti(db);
                            //HistoricalDataFromEuroProgetti = GetHistoricalDataFromEuroProgetti(POIdParameter, db);
                            if (HistoricalDataFromEuroProgetti != null)
                            {
                                curItem.DatiMacchinaCopieRichieste = HistoricalDataFromEuroProgetti.CopieRichieste;
                                curItem.DatiMacchinaCopieLavorate = HistoricalDataFromEuroProgetti.CopieLavorate;
                            }
                        }
                        if (curItem.IDProductionMachine == 107)
                        {
                            //CurOrderOnZechini = GetRealTimeDataFromZechini(db);
                            //HistoricalDataFromZechini = GetHistoricalDataFromZechini(POIdParameter, db);
                            if (HistoricalDataFromZechini != null)
                            {
                                curItem.DatiMacchinaCopieRichieste = HistoricalDataFromZechini.CopieRichieste;
                                curItem.DatiMacchinaCopieLavorate = HistoricalDataFromZechini.CopieLavorate;
                            }
                        }
                    }
            }

            grdVW_ProductionExtMPS_GroupedByPhasesNew.DataSource = data;

            grdVW_ProductionExtMPS_GroupedByPhasesNew.DataBind();
            //grdProductionMPS.DataBind();
            //grdDeliveryTrips.DataBind();

        }





        //protected void TransferAvailableData()
        //{


        //    List<ProductionOrder> _newProductionOrders = new List<ProductionOrder>();
        //    ProductionOrder _newProductionOrder = new ProductionOrder();
        //    if (POCustomerIdParameter != -1)
        //    {
        //        _newProductionOrder.ID_CustomerOrder = POCustomerIdParameter;
        //    }
        //    if (POQuotationIdParameter != -1)
        //    {
        //        _newProductionOrder.ID_Quotation = POQuotationIdParameter;
        //    }
        //    if (POQuantityParameter != -1)
        //    {
        //        _newProductionOrder.Quantity = POQuantityParameter;
        //    }
        //    if (POStartDateParameter != DateTime.Now.Date)
        //    {
        //        _newProductionOrder.StartDate = POStartDateParameter;
        //    }
        //    _newProductionOrders.Add(_newProductionOrder);
        //    dtvProductionOrder.DataSourceID = string.Empty;
        //    dtvProductionOrder.DataSource = _newProductionOrders;

        //}

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //    //Session[MenuType.MenuProductionOrders.ToString()] = null;
            //    Cache.Remove(MenuType.MenuProductionOrders.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            //    if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            //    {
            //        //Session[MenuType.MenuProductionOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuProductionOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //    //Session[MenuType.MenuProductionOrders.ToString()] = null;
            //    Cache.Remove(MenuType.MenuProductionOrders.ToString());
            //    Cache.Remove(MenuType.MenuOperations.ToString());
            dtvProductionOrder.Visible = false;
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
                lblItemNo.Text = " No " + POIdParameter;
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {

            if (dtvProductionOrder.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvProductionOrder.Rows[0];

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
                            new QuotationDataContext().Quotations.SingleOrDefault(q => q.ID == POQuotationIdParameter)
                                .CustomerCode.Value.ToString();
                    }
                    else
                    {
                        ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = POCustomerIdParameter.ToString();
                    }
                }
                if (POCustomerOrderIdParameter != -1)
                {
                    //DynamicControl _dyc = (DynamicControl)_dvr.FindControl("dycCustomerOrder_ID");
                    //((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = POQuotationIdParameter.ToString();
                }
                if (POQuotationIdParameter != -1)
                {
                    //var _dyc = (DynamicControl)_dvr.FindControl("dycQuotation");
                    //((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = POQuotationIdParameter.ToString();
                    //((DropDownList)_dyc.Controls[0].Controls[0]).Enabled = false;

                    var _dyc = (DynamicControl)_dvr.FindControl("dycIDQuotation");
                    ((TextBox)_dyc.Controls[0].Controls[0]).Text = POQuotationIdParameter.ToString();
                    //((Literal)_dyc.Controls[0].Controls[0]).Enabled = false;

                    using (var _context = new QuotationDataContext())
                    {
                        var _dyc1 = (DynamicControl)_dvr.FindControl("dycNote");
                        if (((TextBox)_dyc1.Controls[0].Controls[0]).Text == string.Empty)
                        {
                            //using (var _context = new QuotationDataContext())
                            //{   
                            var _newQuotation =
                                _context.Quotations.SingleOrDefault(q => q.ID == POQuotationIdParameter);
                            ((TextBox)_dyc1.Controls[0].Controls[0]).Text = _newQuotation.Note;
                            //}
                        }

                        var _dyc5 = (DynamicControl)_dvr.FindControl("dycNote1");
                        if (((TextBox)_dyc1.Controls[0].Controls[0]).Text == string.Empty)
                        {
                            //using (var _context = new QuotationDataContext())
                            //{   
                            var _newQuotation =
                                _context.Quotations.SingleOrDefault(q => q.ID == POQuotationIdParameter);
                            ((TextBox)_dyc5.Controls[0].Controls[0]).Text = _newQuotation.Note1;
                            //}
                        }

                        var _dyc2 = (DynamicControl)_dvr.FindControl("dycContractor");
                        {
                            //using (var _context = new QuotationDataContext())
                            //{
                            var _newQuotation =
                                _context.Quotations.SingleOrDefault(q => q.ID == POQuotationIdParameter);
                            //((DropDownList)_dyc2.Controls[0].Controls[0]).SelectedValue =
                            //    _newQuotation.ID_Owner.ToString();
                            ((DropDownList)_dyc2.Controls[0].Controls[0]).SelectedValue =
                                WebUser.Employee.ID.ToString();
                            //}
                        }

                        var _dyc4 = (DynamicControl)_dvr.FindControl("dycManager");
                        {
                            //using (var _context = new QuotationDataContext())
                            //{
                            var _newQuotation =
                                _context.Quotations.SingleOrDefault(q => q.ID == POQuotationIdParameter);
                            ((DropDownList)_dyc4.Controls[0].Controls[0]).SelectedValue =
                                _newQuotation.ID_Manager.ToString();
                            ((DropDownList)_dyc4.Controls[0].Controls[0]).Enabled = false;
                            //}
                        }
                    }

                }
                if (PONameParameter != string.Empty)
                {
                    var _dyc = (DynamicControl)_dvr.FindControl("dycDescription");
                    ((TextBox)_dyc.Controls[0].Controls[0]).Text = PONameParameter;
                    //((TextBox)_dyc.Controls[0].Controls[0]).Enabled = false;
                }
                if (POQuantityParameter != -1)
                {
                    var _dyc = (DynamicControl)_dvr.FindControl("dycQuantity");
                    ((TextBox)_dyc.Controls[0].Controls[0]).Text = POQuantityParameter.ToString();
                    //((TextBox)_dyc.Controls[0].Controls[0]).Enabled = false;
                }
                //if (POStartDateParameter != DateTime.Now.Date)
                //{
                var _dyc3 = (DynamicControl)_dvr.FindControl("dycStartDate");
                ((TextBox)_dyc3.Controls[0].Controls[0]).Text = DateTime.Today.ToString("dd/MM/yyyy"); //POStartDateParameter.ToString();
                //}

                var _dyc0 = (DynamicControl)_dvr.FindControl("dycStatuse");
                ((DropDownList)_dyc0.Controls[0].Controls[0]).SelectedValue = 1.ToString();
                //((DropDownList)_dyc0.Controls[0].Controls[0]).Enabled = false;

                //var _dycCustomer = (DynamicControl)_dvr.FindControl("dycCustomer");
                //var _curCustomerCode = ((DropDownList)_dycCustomer.Controls[0].Controls[0]).SelectedValue;
                ////var _curCustomerCode = ((HiddenField)_dycCustomer.Controls[0].Controls[0]).Value;
                //if (_curCustomerCode != string.Empty)
                //{
                //    var _curContext =
                //        (QuotationDataContext)dtvProductionOrder.FindMetaTable().CreateContext();

                //    if (POQuotationIdParameter == -1)
                //    {
                //        var _dyc1 = (DynamicControl)_dvr.FindControl("dycQuotation");
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.Clear();
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.Add(new ListItem("[Non selezionato]", ""));

                //        var _list1 = _curContext.Quotations
                //            .Where(
                //                q =>
                //                    q.CustomerCode == Int32.Parse(_curCustomerCode) && q.Draft == false &&
                //                    q.ProductionOrders.Count == 0)
                //            .Select(q => new ListItem(q.Subject, q.ID.ToString())).ToArray();
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.AddRange(_list1);
                //    }
                //    var _dyc2 = (DynamicControl)_dvr.FindControl("dycCustomerOrder");
                //    ((DropDownList)_dyc2.Controls[0].Controls[0]).Items.Clear();
                //    ((DropDownList)_dyc2.Controls[0].Controls[0]).Items.Add(new ListItem("[Non selezionato]", ""));
                //    var _list2 =
                //        _curContext.CustomerOrders.Where(q => q.CustomerCode == Int32.Parse(_curCustomerCode))
                //            .Select(q => new ListItem(q.CustomerOrderCode, q.ID.ToString()))
                //            .ToArray();
                //    ((DropDownList)_dyc2.Controls[0].Controls[0]).Items.AddRange(_list2);
                //}
                //else
                //{
                //    //Nicola il 22/04/2015
                //    //filtra caricamento elenco preventivi sugli ultimi 6 mesi
                //    var _curContext =
                //        (QuotationDataContext)dtvProductionOrder.FindMetaTable().CreateContext();

                //    if (POQuotationIdParameter == -1)
                //    {
                //        var _dyc1 = (DynamicControl)_dvr.FindControl("dycQuotation");
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.Clear();
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.Add(new ListItem("[Non selezionato]", ""));
                //        var _list1 = _curContext.Quotations
                //            .Where(
                //                q =>
                //                    q.Draft == false &&
                //                    q.ProductionOrders.Count == 0 && q.Date > DateTime.Now.AddDays(-120))
                //            .Select(q => new ListItem(q.Subject, q.ID.ToString())).ToArray();
                //        ((DropDownList)_dyc1.Controls[0].Controls[0]).Items.AddRange(_list1);
                //    }
                //}

                var _dycDate = (DynamicControl)_dvr.FindControl("dycDeliveryDate");
                ((TextBox)_dycDate.Controls[0].Controls[0]).Text = "prova";

                var _dyc9 = (DynamicControl)_dvr.FindControl("dycNoteFromProduction");
                ((TextBox)_dyc9.Controls[0].Controls[0]).Text = ProductionOrderService.GetNoteFromPreviousProduction(POQuotationIdParameter);

            }
            else
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
            //if (((ProductionOrder)e.NewObject).CustomerOrder == null)
            //{
            //    CustomerOrder _newCustomerOrder = new CustomerOrder();
            //    _newCustomerOrder.Customer = _currentQuotation.Customer;
            //    _newCustomerOrder.CustomerOrderCode = "Automatico da Preventivo " + _currentQuotation.Subject;
            //    _newCustomerOrder.Quotation = _currentQuotation;
            //    _newCustomerOrder.OrderDate = DateTime.Now.Date;
            //    _newCustomerOrder.Status = 1;
            //    _newCustomerOrder.Quantity = 0;

            //}

            if (((ProductionOrder)e.NewObject).ID_Quotation == null) // allora crea preventivo fittizio
            {
                ((ProductionOrder)e.NewObject).ID_Quotation = CreateDummyQuotation((ProductionOrder)e.NewObject);
            }

            //((ProductionOrder)e.NewObject).Inserted = true;
            //((ProductionOrder)e.NewObject).Status = 6;
        }

        protected void dtvProductionOrder_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            //if (e.CommandName == "Deactivate")
            //{
            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
            //        _ProductionOrder.Inserted = false;
            //        _qc.SubmitChanges();
            //        //Session[MenuType.MenuProductionOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuProductionOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
            //}
            //if (e.CommandName == "Activate")
            //{
            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
            //        _ProductionOrder.Inserted = true;
            //        _qc.SubmitChanges();
            //        //Session[MenuType.MenuProductionOrders.ToString()] = null;
            //        Cache.Remove(MenuType.MenuProductionOrders.ToString());
            //        Cache.Remove(MenuType.MenuOperations.ToString());
            //    }
            //}
        }

        protected void dtvProductionOrder_ItemCreated(object sender, EventArgs e)
        {
            //LinkButton _lbtDeactivate = (LinkButton)dtvProductionOrder.FindControl("lbtDeactivate");
            //LinkButton _lbtActivate = (LinkButton)dtvProductionOrder.FindControl("lbtActivate");

            using (QuotationDataContext _qc = new QuotationDataContext())
            {
                ProductionOrder _ProductionOrder = _qc.ProductionOrders.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvProductionOrder.DataKey.Value));
                //if (_ProductionOrder != null)
                //{
                //    if (_ProductionOrder.Inserted)
                //    {
                //        _lbtDeactivate.OnClientClick = @"return confirm('Sei sicuro di voler disattivare definitivamente questa voce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo o di un gruppo voci già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
                //    }
                //    if (!_ProductionOrder.Inserted)
                //    {
                //        _lbtActivate.OnClientClick = @"return confirm('Sei sicuro di voler riattivare questa voce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
                //    }
                //}
            }

        }

        protected void ldsProductionOrderDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsProductionOrderDetails.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            ldsProductionOrderDetails.OrderByParameters.Clear();
            ldsProductionOrderDetails.AutoGenerateOrderByClause = false;
            e.Result = _qc.ProductionOrderDetails.OrderBy(pi => pi.MacroRef == null ? pi.ID * 1e6 - 1 : pi.MacroRef * 1e6);

            //switch (ddlOrderBy.SelectedValue)
            //{
            //    //case (""):
            //    //    GridDataSource.OrderByParameters.Clear();
            //    //    GridDataSource.AutoGenerateOrderByClause = false;
            //    //    e.Result = _qc.ProductionOrders.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
            //    //    break;
            //    case ("StatusName"):
            //        ldsProductionOrderDetails.OrderByParameters.Clear();
            //        ldsProductionOrderDetails.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.ProductionOrders.OrderBy(qt => qt.Statuse.Description);
            //        break;
            //    case ("CustomerName"):
            //        ldsProductionOrderDetails.OrderByParameters.Clear();
            //        ldsProductionOrderDetails.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.ProductionOrders.OrderBy(qt => qt.Customer.Name);
            //        break;
            //    default:
            //        break;

            //}
        }

        protected void grdProductionOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderDetails.PageIndex = e.NewPageIndex;
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

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                var _newQuotation =
                    db.Quotations.SingleOrDefault(q => q.ID == ((ProductionOrder)e.Result).ID_Quotation);
                //if (_newQuotation.Q3 == -1 && _newQuotation.Q4 == -1 && _newQuotation.Q5 == -1)
                if (((ProductionOrder)e.Result).Price != null)
                {
                    _newQuotation.Subject = ((ProductionOrder)e.Result).Description + " (Automatico da OdP " +
                                            ((ProductionOrder)e.Result).Number + " [" +
                                            ((ProductionOrder)e.Result).ID + "])";
                }
                else
                {
                    ProductionOrderService.SyncroniseQuotationSubject(db, (ProductionOrder)e.Result);
                    ProductionOrderService.DeleteProductionOrderSchedule(db, ((ProductionOrder)e.Result));
                    if (((ProductionOrder)e.Result).Status == 1 || ((ProductionOrder)e.Result).Status == 9)
                    {
                        ProductionOrderService.CreateProductionOrderSchedule(db, ((ProductionOrder)e.Result), Global.CurrentSchedulingType);
                    }
                }
                QUOPORCostsPrice _toSaveQUOPORCostsPrice = db.QUOPORCostsPrices.FirstOrDefault(po => po.ID == ((ProductionOrder)e.Result).ID);
                if (_toSaveQUOPORCostsPrice != null)
                {
                    _toSaveQUOPORCostsPrice.AccountNote = ((ProductionOrder)e.Result).AccountNote;
                    _toSaveQUOPORCostsPrice.NonConformityCode = ((ProductionOrder)e.Result).NonConformityCode;
                }
                EcoSystemGateway.UpdateOdPPlasticCoatingData(db, ((ProductionOrder)e.Result).ID);
                db.SubmitChanges();
            }
            Response.Redirect("ProductionOrderPopup.aspx?" + POIdKey + "=" + ((ProductionOrder)e.Result).ID);
        }

        protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
            if (((ProductionOrder)e.NewObject).ID_Quotation == null) // allora crea preventivo fittizio
            {
                ((ProductionOrder)e.NewObject).ID_Quotation = CreateDummyQuotation((ProductionOrder)e.NewObject);
            }
            else // allora modifica preventivo fittizio, NO IL PREVENTIVO FITTIZIO SI MODIFICA A MANO E DIRETTAMENTE!
            {
                ((ProductionOrder)e.NewObject).ID_Quotation = UpdateDummyQuotation((ProductionOrder)e.NewObject);

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ProductionOrderService.SyncroniseQuotationSubject(db, (ProductionOrder)e.NewObject);

                    if (((ProductionOrder)e.NewObject).Quantity != ((ProductionOrder)e.OriginalObject).Quantity ||
                        ((ProductionOrder)e.NewObject).DeliveryDate != ((ProductionOrder)e.OriginalObject).DeliveryDate)
                    {
                        ProductionOrderService.DeleteProductionOrderSchedule(db, ((ProductionOrder)e.NewObject));
                        if (((ProductionOrder)e.NewObject).Status == 1 || ((ProductionOrder)e.NewObject).Status == 9)
                        {
                            ProductionOrderService.CreateProductionOrderSchedule(db, ((ProductionOrder)e.NewObject), Global.CurrentSchedulingType);
                        }
                    }
                    if (((ProductionOrder)e.NewObject).Status == 3)
                    {
                        ProductionOrderService.CloseProductionOrderSchedule(db, ((ProductionOrder)e.NewObject).ID);
                    }
                    if (((ProductionOrder)e.OriginalObject).Status == 3 && (((ProductionOrder)e.NewObject).Status == 1 || ((ProductionOrder)e.NewObject).Status == 9))
                    {
                        ProductionOrderService.DeleteProductionOrderSchedule(db, ((ProductionOrder)e.NewObject), false);
                        ProductionOrderService.CreateProductionOrderSchedule(db, ((ProductionOrder)e.NewObject), Global.CurrentSchedulingType);
                    }
                    QUOPORCostsPrice _toSaveQUOPORCostsPrice = db.QUOPORCostsPrices.FirstOrDefault(po => po.ID == ((ProductionOrder)e.NewObject).ID);
                    if (_toSaveQUOPORCostsPrice != null)
                    {
                        _toSaveQUOPORCostsPrice.AccountNote = ((ProductionOrder)e.NewObject).AccountNote;
                        _toSaveQUOPORCostsPrice.NonConformityCode = ((ProductionOrder)e.NewObject).NonConformityCode;
                    }
                    db.SubmitChanges();
                }
            }

            BindGrids();



        }

        protected void dtvProductionOrder_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (e.NewValues["ID_Customer"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CustomerIsMandatory);
                e.Cancel = true;
            }
            if (e.NewValues["Quantity"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
                e.Cancel = true;
            }
            //if (e.NewValues["ID_Quotation"] == null && e.NewValues["Price"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.PriceIsMandatoryForAuto);
            //    e.Cancel = true;
            //}
            if (e.NewValues["DeliveryDate"] == null && e.NewValues["DeliveryDate"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsMandatory);
                e.Cancel = true;
            }

        }

        protected void dtvProductionOrder_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (e.Values["ID_Customer"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CustomerIsMandatory);
                e.Cancel = true;
            }
            if (e.Values["Quantity"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
                e.Cancel = true;
            }
            //if (e.Values["ID_Quotation"] == null && e.Values["Price"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.PriceIsMandatoryForAuto);
            //    e.Cancel = true;
            //}
            if (e.Values["DeliveryDate"] == null && e.Values["DeliveryDate"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsMandatory);
                e.Cancel = true;
            }
            else
            {
                DateTime deliveryDate = DateTime.Parse(e.Values["DeliveryDate"].ToString());
                if (deliveryDate < DateTime.Today)
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.DeliveryDateIsInvalid);
                    e.Cancel = true;
                }
            }
        }

        protected void lbtPrintPOFinalCost_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            PrintPOFinalCost(_qc);
        }

        protected void grdProductionOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var _bound = (ProductionOrderDetail)e.Row.DataItem;
                //e.Row.FindControl("dycOwner").Visible = (_bound.FreeTypeCode == null);
                //e.Row.FindControl("dycPhase").Visible = (_bound.FreeTypeCode == null);
                //e.Row.FindControl("dycProductionTime").Visible = (_bound.FreeTypeCode == null);

                e.Row.FindControl("dycRawMaterial").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterial").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycRawMaterial").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.RFlag == null); 

                e.Row.FindControl("dycUMRawMaterial").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycUMRawMaterial").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycUMRawMaterial").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.RFlag == null);

                e.Row.FindControl("dycRawMaterialQuantity").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterialQuantity").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycRawMaterialQuantity").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.RFlag == null);

                e.Row.FindControl("dycSupplier").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycSupplier").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycSupplier").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.RFlag == null); 

                e.Row.FindControl("dycRawMaterialSup").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterialSup").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycRawMaterialSup").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycSupplierSup").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycSupplierSup").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycSupplierSup").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycUMUser").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycUMUser").Controls[0].Controls[0] is DropDownList)
                    ((DropDownList)e.Row.FindControl("dycUMUser").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycRawMaterialX").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterialX").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycRawMaterialX").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycOkCopiesCount").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycOkCopiesCount").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycOkCopiesCount").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycKoCopiesCount").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycKoCopiesCount").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycKoCopiesCount").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycRawMaterialY").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterialY").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycRawMaterialY").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycRawMaterialZ").Visible = (_bound.FreeTypeCode == null);
                if (e.Row.FindControl("dycRawMaterialZ").Controls[0].Controls[0] is TextBox)
                    ((TextBox)e.Row.FindControl("dycRawMaterialZ").Controls[0].Controls[0]).Visible =
                        (_bound.FreeTypeCode == null && _bound.SFlag == null);

                e.Row.FindControl("dycDirectSupply").Visible = (_bound.FreeTypeCode == null);

                e.Row.FindControl("dycFreeType").Visible = (_bound.FreeTypeCode != null);
                e.Row.FindControl("dycItemFreeType").Visible = (_bound.FreeTypeCode != null);
                e.Row.FindControl("dycFreeItemDescription").Visible = (_bound.FreeTypeCode != null);
            }
        }

        protected void lbtShowHide_Click(object sender, EventArgs e)
        {
            tblTestata.Visible = !tblTestata.Visible;
        }

        protected void grdProductionOrderDetails_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            using (var _ctx = new QuotationDataContext())
            {
                var _curItem =
                    _ctx.ProductionOrderDetails.SingleOrDefault(pod => pod.ID == Convert.ToInt32(e.Keys["ID"]));
                _curItem.HistoricalCostPhase = _curItem.CostCalcPhase;
                if (_curItem.RFlag == null)
                    _curItem.HistoricalCostRawM = _curItem.CostCalcRawM;
                if (_curItem.SFlag == null)
                    _curItem.HistoricalCostSupM = _curItem.CostCalcSupM;
                if (_curItem.FreeTypeCode != null)
                    _curItem.ID_Phase = null;
                _ctx.SubmitChanges();
            }
        }

        protected void DetailsDataSource_Updated(object sender, LinqDataSourceStatusEventArgs e)
        {

        }

        //protected void ldsProductionMPS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{

        //    var table = ldsProductionMPS.GetTable();
        //    var _qc = (QuotationDataContext)table.CreateContext();

        //    //ldsProductionMPS.OrderByParameters.Clear();
        //    //ldsProductionMPS.AutoGenerateOrderByClause = false;
        //    e.Result = _qc.ProductionMPs.Where(mp => mp.IDProductionOrder == POIdParameter).OrderBy(mp => Convert.ToInt32(mp.Order)).ThenBy(mp => mp.ProdEnd);
        //    //e.Result = _qc.ProductionMPs.Where(mp => mp.IDProductionOrder == POIdParameter).OrderBy(mp => mp.ProdEnd);

        //}


        //protected void ldsProductionMPS_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        //{
        //    ProductionMP originalObject = ((ProductionMP)e.OriginalObject);
        //    ProductionMP newObject = ((ProductionMP)e.NewObject);

        //    if (newObject.Status == 11)
        //    {
        //        if (newObject.IDProductionMachine != originalObject.IDProductionMachine)
        //        {
        //            var table = ldsProductionMPS.GetTable();
        //            using (QuotationDataContext db = (QuotationDataContext)table.CreateContext())
        //            {

        //                //db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m =>
        //                //    m.IDProductionOrder == originalObject.IDProductionOrder &&
        //                //    m.IDPickingItem.GetValueOrDefault() == originalObject.IDPickingItem.GetValueOrDefault() &&
        //                //    m.IDMacroItem.GetValueOrDefault() == originalObject.IDMacroItem.GetValueOrDefault() &&
        //                //    m.IDMacroItemDetail.GetValueOrDefault() == originalObject.IDMacroItemDetail.GetValueOrDefault() &&
        //                //    m.OldIDProductionMachine == originalObject.IDProductionMachine &&
        //                //    m.OldNumProductionMachine == originalObject.NumProductionMachine &&
        //                //    m.OldOrder == originalObject.Order
        //                //    ));
        //                //db.SubmitChanges();
        //                //ProductionMPSException newException = new ProductionMPSException();
        //                //newException.IDProductionOrder = newObject.IDProductionOrder;
        //                //newException.IDPickingItem = newObject.IDPickingItem;
        //                //newException.IDMacroItem = newObject.IDMacroItem;
        //                //newException.IDMacroItemDetail = originalObject.IDMacroItemDetail;
        //                //newException.IDQuotationDetail = newObject.IDQuotationDetail;

        //                //newException.OldIDProductionMachine = originalObject.IDProductionMachine;
        //                //newException.OldNumProductionMachine = originalObject.NumProductionMachine;
        //                //newException.OldOrder = originalObject.Order;

        //                //newException.NewIDProductionMachine = newObject.IDProductionMachine;
        //                //newException.NewNumProductionMachine = newObject.NumProductionMachine;
        //                //newException.NewOrder = newObject.Order;

        //                //db.ProductionMPSExceptions.InsertOnSubmit(newException);

        //                db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == originalObject.IDProductionOrder && m.NewIDProductionMachine == originalObject.IDProductionMachine && m.NewOrder == null));
        //                db.SubmitChanges();
        //                foreach (ProductionMP productionMP in db.ProductionMPs.Where(m => m.IDProductionOrder == originalObject.IDProductionOrder && m.IDProductionMachine == originalObject.IDProductionMachine && m.Status == 11))
        //                {
        //                    productionMP.IDProductionMachine = newObject.IDProductionMachine;

        //                    ProductionMPSException newException1 = new ProductionMPSException();
        //                    newException1.IDProductionOrder = productionMP.IDProductionOrder;
        //                    newException1.IDPickingItem = productionMP.IDPickingItem;
        //                    newException1.IDMacroItem = productionMP.IDMacroItem;
        //                    newException1.IDMacroItemDetail = productionMP.IDMacroItemDetail;
        //                    newException1.IDQuotationDetail = (productionMP.IDQuotationDetail == -1 ? null : productionMP.IDQuotationDetail);

        //                    newException1.OldIDProductionMachine = originalObject.IDProductionMachine;
        //                    newException1.OldNumProductionMachine = originalObject.NumProductionMachine;
        //                    newException1.NewIDProductionMachine = newObject.IDProductionMachine;
        //                    newException1.NewNumProductionMachine = newObject.NumProductionMachine;

        //                    db.ProductionMPSExceptions.InsertOnSubmit(newException1);
        //                }

        //                db.SubmitChanges();
        //            }
        //            e.Cancel = true;
        //            Response.Redirect(this.Request.RawUrl);

        //        }
        //    }


        //}




        protected void btnSuspend_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            using (var _qc = (QuotationDataContext)table.CreateContext())
            {
                var _toSuspendProductionOrder = _qc.ProductionOrders.Single(po => po.ID == POIdParameter);
                _toSuspendProductionOrder.Status = 2;
                _qc.SubmitChanges();
            }
            //grdProductionMPS.DataBind();
            BindGrids();
        }

        protected void btnPortaInCorso_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            using (var db = (QuotationDataContext)table.CreateContext())
            {
                var po = db.ProductionOrders.Single(p => p.ID == POIdParameter);
                po.Status = 1;
                ProductionOrderService.DeleteProductionOrderSchedule(db, po, false);
                ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);
                db.SubmitChanges();
            }
            //grdProductionMPS.DataBind();
            BindGrids();

        }

        protected void btnPortainAttesa_Click(object sender, EventArgs e)
        {

            var table = DetailsDataSource.GetTable();
            using (var db = (QuotationDataContext)table.CreateContext())
            {
                var po = db.ProductionOrders.Single(p => p.ID == POIdParameter);
                po.Status = 9;
                db.SubmitChanges();
            }
            //grdProductionMPS.DataBind();
            BindGrids();

        }

        protected void btnResetSchedule_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            using (var db = (QuotationDataContext)table.CreateContext())
            {
                ProductionOrder po = db.ProductionOrders.Single(p => p.ID == POIdParameter);
                ProductionOrderService.DeleteProductionOrderSchedule(db, po, false);
                ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);
            }
            //grdProductionMPS.DataBind();
            BindGrids();
        }

        protected void btnResetPrioritySchedule_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            using (var db = (QuotationDataContext)table.CreateContext())
            {
                ProductionOrder po = db.ProductionOrders.Single(p => p.ID == POIdParameter);
                //db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == po.ID && m.NewIDProductionMachine == null));
                db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == po.ID));
                ProductionOrderService.DeleteProductionOrderSchedule(db, po, true);
                ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);
            }
            //grdProductionMPS.DataBind();
            BindGrids();
        }


        //protected void grdProductionMPS_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Advance")
        //    {
        //        CambiaPriorita(Convert.ToInt32(e.CommandArgument), -1);
        //    }

        //    if (e.CommandName == "Retard")
        //    {
        //        CambiaPriorita(Convert.ToInt32(e.CommandArgument), 1);
        //    }

        //}


        //protected void CambiaPriorita(int idProductionMP, int segno)
        //{

        //    using (QuotationDataContext db = new QuotationDataContext())
        //    {

        //        ProductionMP hitItem = db.ProductionMPs.FirstOrDefault(p => p.ID == idProductionMP);

        //        List<ProductionMP> poMPSItems = db.ProductionMPs.Where(p => p.IDProductionOrder == hitItem.IDProductionOrder).OrderBy(m => Convert.ToInt32(m.Order)).ToList();

        //        decimal hitItemdOrder = 0;
        //        decimal dOrder = 0;

        //        foreach (ProductionMP poMPSItem in poMPSItems)
        //        {
        //            if (poMPSItem.Status != 11)
        //            {
        //                poMPSItem.dOrder = -1;
        //            }
        //            else
        //            {
        //                dOrder = dOrder + 1;
        //                poMPSItem.dOrder = dOrder;
        //                if (poMPSItem.ID == hitItem.ID)
        //                {
        //                    hitItemdOrder = dOrder;
        //                }
        //            }
        //        }

        //        foreach (ProductionMP poMPSItem in poMPSItems.Where(p => p.dOrder == hitItemdOrder))
        //        {
        //            if (segno < 0)
        //            {
        //                int index = poMPSItems.IndexOf(poMPSItem);
        //                if (poMPSItem.dOrder > 1)
        //                    poMPSItem.dOrder = poMPSItem.dOrder - 1.5m;
        //            }
        //            else
        //            {
        //                int index = poMPSItems.IndexOf(poMPSItem);
        //                if (poMPSItem.dOrder < poMPSItems.Max(d => d.dOrder))
        //                    poMPSItem.dOrder = poMPSItem.dOrder + 1.5m;
        //            }
        //        }

        //        int nOrder = 1;
        //        foreach (ProductionMP poMPSItem in poMPSItems.OrderBy(p => p.dOrder))
        //        {
        //            poMPSItem.Order = nOrder.ToString();
        //            nOrder = nOrder + 1;
        //        }

        //        db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == hitItem.IDProductionOrder && m.NewOrder != null));
        //        db.SubmitChanges();

        //        foreach (ProductionMP item in poMPSItems.OrderBy(p => p.Order))
        //        {
        //            ProductionMPSException newException = new ProductionMPSException();
        //            newException.IDProductionOrder = item.IDProductionOrder;
        //            newException.IDPickingItem = item.IDPickingItem;
        //            newException.IDMacroItem = item.IDMacroItem;
        //            newException.IDMacroItemDetail = item.IDMacroItemDetail;
        //            newException.IDQuotationDetail = (item.IDQuotationDetail == -1 ? null : item.IDQuotationDetail);
        //            newException.NewOrder = item.Order;
        //            db.ProductionMPSExceptions.InsertOnSubmit(newException);
        //        }
        //        db.SubmitChanges();

        //    }

        //    BindGrids();

        //}

        protected void CambiaPriorita_Grouped(int idProductionMP, int segno)
        {

            using (QuotationDataContext db = new QuotationDataContext())
            {

                VW_ProductionExtMPS_Lite hitItem = db.VW_ProductionExtMPS_Lites.FirstOrDefault(p => p.ID == idProductionMP);

                List<VW_ProductionExtMPS_Lite> poMPSItems = db.VW_ProductionExtMPS_Lites.Where(p => p.IDProductionOrder == hitItem.IDProductionOrder).OrderBy(m => Convert.ToInt32(m.Order)).ToList();

                decimal hitItemdOrder = 0;
                decimal dOrder = 0;
                int previousPickingItem = 0;

                foreach (VW_ProductionExtMPS_Lite poMPSItem in poMPSItems)
                {
                    if (poMPSItem.Status != 11)
                    {
                        poMPSItem.dOrder = -1;
                        poMPSItem.nOrder = -1;
                    }
                    else
                    {
                        if (poMPSItem.IDPickingItem != previousPickingItem)
                        {
                            dOrder = dOrder + 1;
                        }
                        poMPSItem.dOrder = dOrder;
                        if (poMPSItem.IDPickingItem == hitItem.IDPickingItem)
                        {
                            hitItemdOrder = dOrder;
                        }
                        previousPickingItem = poMPSItem.IDPickingItem.Value;
                    }
                }

                foreach (VW_ProductionExtMPS_Lite poMPSItem in poMPSItems.Where(p => p.dOrder == hitItemdOrder))
                {
                    if (segno < 0)
                    {
                        int index = poMPSItems.IndexOf(poMPSItem);
                        if (poMPSItem.dOrder > 1)
                            poMPSItem.dOrder = poMPSItem.dOrder - 1.5m;
                    }
                    else
                    {
                        int index = poMPSItems.IndexOf(poMPSItem);
                        if (poMPSItem.dOrder < poMPSItems.Max(d => d.dOrder))
                            poMPSItem.dOrder = poMPSItem.dOrder + 1.5m;
                    }
                }


                int nOrder = 1;
                foreach (VW_ProductionExtMPS_Lite poMPSItem in poMPSItems.OrderBy(p => p.dOrder))
                {
                    poMPSItem.Order = nOrder.ToString();
                    nOrder = nOrder + 1;
                }

                db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == hitItem.IDProductionOrder && m.NewOrder != null));
                db.SubmitChanges();

                foreach (VW_ProductionExtMPS_Lite item in poMPSItems.OrderBy(p => p.nOrder).ThenBy(p => p.Order))
                {
                    ProductionMPSException newException = new ProductionMPSException();
                    newException.IDProductionOrder = item.IDProductionOrder;
                    newException.IDPickingItem = item.IDPickingItem;
                    newException.IDMacroItem = item.IDMacroItem;
                    newException.IDMacroItemDetail = item.IDMacroItemDetail;
                    newException.IDQuotationDetail = (item.IDQuotationDetail == -1 ? null : item.IDQuotationDetail);
                    newException.NewOrder = item.Order;
                    db.ProductionMPSExceptions.InsertOnSubmit(newException);
                }
                db.SubmitChanges();

            }

            BindGrids();
        }


        protected void lbtShowHideDet_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = !pnlDetails.Visible;
        }

        protected void ibtQtaProdBOBST_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CurProducedQuantity = GetHistoricalDataFromBOBST1();
                if (CurProducedQuantity.Record != null)
                {
                    lblQtaProdBOBST.Text = string.Format("Quantità prodotta su macchina BOBST: {0:N0} (dato in tempo reale)", CurProducedQuantity.Record.SUM);
                    ibtQtaProdBOBST.Visible = true;
                }
            }
            catch
            {
                lblSuccess.Text = "Impossibile comunicare con la macchina BOBST1.";
            }
        }


        //protected void ldsVW_ProductionExtMPS_GroupedByPhases_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    var table = ldsVW_ProductionExtMPS_GroupedByPhases.GetTable();
        //    var _qc = (QuotationDataContext)table.CreateContext();

        //    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.Where(mp => mp.IDProductionOrder == POIdParameter && mp.Status == 11).OrderBy(mp => mp.ProdEnd);
        //}

        protected void grdVW_ProductionExtMPS_GroupedByPhasesNew_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Advance")
            {
                //CambiaPrioritaNew(Convert.ToInt32(e.CommandArgument), -1);
                CambiaPriorita_Grouped(Convert.ToInt32(e.CommandArgument), -1);
            }

            if (e.CommandName == "Retard")
            {
                //CambiaPrioritaNew(Convert.ToInt32(e.CommandArgument), 1);
                CambiaPriorita_Grouped(Convert.ToInt32(e.CommandArgument), 1);
            }
            SelectedRowId = Convert.ToInt32(e.CommandArgument);
            grdVW_ProductionExtMPS_GroupedByPhasesNew.DataBind();

        }

        protected void ddlProductionMachines_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlCurrentDropDownList = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            int ID = Convert.ToInt32(((HiddenField)gvr.FindControl("hidID1")).Value);

            using (var db = new QuotationDataContext())
            {
                ProductionMP current = db.ProductionMPs.FirstOrDefault(d => d.ID == ID);
                //foreach (ProductionMP sibling in db.ProductionMPs
                //    .Where(d => d.IDProductionOrder == current.IDProductionOrder
                //        && d.IDPickingItem == current.IDPickingItem
                //        && d.IDProductionMachine == current.IDProductionMachine))
                //{
                //    sibling.IDProductionMachine = Convert.ToInt32(ddlCurrentDropDownList.SelectedValue);
                //}

                db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == current.IDProductionOrder && m.NewIDProductionMachine == current.IDProductionMachine && m.NewOrder == null));
                db.SubmitChanges();
                foreach (ProductionMP productionMP in db.ProductionMPs.Where(m =>
                    m.IDProductionOrder == current.IDProductionOrder
                    && m.IDProductionMachine == current.IDProductionMachine
                    && m.IDPickingItem == current.IDPickingItem
                    && m.Status == 11))
                {


                    ProductionMPSException newException1 = new ProductionMPSException();
                    newException1.IDProductionOrder = productionMP.IDProductionOrder;
                    newException1.IDPickingItem = productionMP.IDPickingItem;
                    newException1.IDMacroItem = productionMP.IDMacroItem;
                    newException1.IDMacroItemDetail = productionMP.IDMacroItemDetail;
                    newException1.IDQuotationDetail = (productionMP.IDQuotationDetail == -1 ? null : productionMP.IDQuotationDetail);

                    newException1.OldIDProductionMachine = productionMP.IDProductionMachine;
                    newException1.OldNumProductionMachine = productionMP.NumProductionMachine;
                    newException1.NewIDProductionMachine = Convert.ToInt32(ddlCurrentDropDownList.SelectedValue);
                    newException1.NewNumProductionMachine = productionMP.NumProductionMachine;

                    productionMP.IDProductionMachine = Convert.ToInt32(ddlCurrentDropDownList.SelectedValue);

                    db.ProductionMPSExceptions.InsertOnSubmit(newException1);
                }

                //ProductionOrder po = db.ProductionOrders.First(p => p.ID == current.IDProductionOrder);
                //ProductionOrderService.DeleteProductionOrderSchedule(db, po, false);
                //ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);

                db.SubmitChanges();



            }
            BindGrids();

        }

        protected void ddlStatuses_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddlCurrentDropDownList = (DropDownList)sender;
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            int ID = Convert.ToInt32(((HiddenField)gvr.FindControl("hidID2")).Value);

            using (var _qc = new QuotationDataContext())
            {
                ProductionMP current = _qc.ProductionMPs.FirstOrDefault(d => d.ID == ID);
                foreach (ProductionMP sibling in _qc.ProductionMPs
                    .Where(d => d.IDProductionOrder == current.IDProductionOrder
                        && d.IDPickingItem == current.IDPickingItem
                        && d.IDProductionMachine == current.IDProductionMachine))
                {
                    sibling.Status = Convert.ToInt32(ddlCurrentDropDownList.SelectedValue);
                }
                _qc.SubmitChanges();
            }
            BindGrids();

        }

        protected void grdVW_ProductionExtMPS_GroupedByPhasesNew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                VW_ProductionExtMPS_GroupedByPhase_Lite curItem = (VW_ProductionExtMPS_GroupedByPhase_Lite)e.Row.DataItem;

                using (var _qc = new QuotationDataContext())
                {

                    DropDownList ddlProductionMachines = (e.Row.FindControl("ddlProductionMachines") as DropDownList);
                    if (curItem.IDProductionMachine != null)
                    {
                        //if (curItem.ID_Company == CurrentCompanyId)
                        //{
                            //var _machineItems = _qc.ProductionMachines.Where(d => (CurrentCompanyId == -1 || d.ID_Company == CurrentCompanyId)).OrderBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                            // merge aziendale
                            var _machineItems = _qc.ProductionMachines.OrderBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                            ddlProductionMachines.Items.AddRange(_machineItems);
                            ddlProductionMachines.Items.FindByValue(curItem.IDProductionMachine.ToString()).Selected = true;
                            ddlProductionMachines.Visible = EditMode;
                        //}
                        //else
                        //{
                        //    ddlProductionMachines.Visible = false;
                        //}
                    }
                    Label lblProductionMachine = (e.Row.FindControl("lblProductionMachine") as Label);
                    lblProductionMachine.Text = curItem.pmDescription;
                    lblProductionMachine.Visible = !EditMode;

                    DropDownList ddlStatuses = (e.Row.FindControl("ddlStatuses") as DropDownList);
                    if (curItem.IDProductionMachine != null)
                    {
                        //if (curItem.ID_Company == CurrentCompanyId)
                        //{
                        var _statusItems = _qc.Statuses.Where(d => d.StatusType == 3).OrderBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                        ddlStatuses.Items.AddRange(_statusItems);
                        ddlStatuses.Items.FindByValue(curItem.Status.ToString()).Selected = true;
                        ddlStatuses.Visible = EditMode;
                        //}
                        //else
                        //{
                        //    ddlStatuses.Visible = false;
                        //}
                    }
                    Label lblStatus = (e.Row.FindControl("lblStatus") as Label);
                    lblStatus.Text = curItem.mpstDescription;

                    lblStatus.Visible = !EditMode;

                }

                if (curItem.IDProductionMachine == 31 || curItem.IDProductionMachine == 30)
                {
                    if (CurOrderOnBOBST != null)
                        if (CurOrderOnBOBST.Id == curItem.IDProductionOrder)
                        {
                            var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                            _imgSemaphore.ToolTip = "Attualmente in lavorazione su BOBST";
                            _imgSemaphore.Width = 40;
                            var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                            _lblSemaphore.Text = string.Format("{0:P0} completato", CurOrderOnBOBST.PercLavorata);
                        }
                }

                else if (curItem.IDProductionMachine == 19)
                {
                    if (CurOrderOnEcoSystem != null)
                        if (CurOrderOnEcoSystem.Id == curItem.IDProductionOrder)
                        {
                            var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                            _imgSemaphore.ToolTip = "Attualmente in lavorazione su ECOSYSTEM";
                            _imgSemaphore.Width = 40;
                            var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                            _lblSemaphore.Text = string.Format("{0:P0} completato", CurOrderOnEcoSystem.PercLavorata);
                        }
                }
                else if (curItem.IDProductionMachine == 104)
                {
                    if (CurOrderOnSilkFoil != null)
                        if (CurOrderOnSilkFoil.Id == curItem.IDProductionOrder)
                        {
                            var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                            _imgSemaphore.ToolTip = "Attualmente in lavorazione su SILKFOIL";
                            _imgSemaphore.Width = 40;
                            var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                            _lblSemaphore.Text = string.Format("{0:P0} completato", CurOrderOnSilkFoil.PercLavorata);
                        }
                }
                else if (curItem.IDProductionMachine == 77)
                {
                    if (CurOrderOnEuroProgetti != null)
                        if (CurOrderOnEuroProgetti.Id == curItem.IDProductionOrder)
                        {
                            var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                            _imgSemaphore.ToolTip = "Attualmente in lavorazione su EUROPROGETTI";
                            _imgSemaphore.Width = 40;
                            var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                            _lblSemaphore.Text = string.Format("{0:P0} completato", CurOrderOnEuroProgetti.PercLavorata);
                        }
                }
                else if (curItem.IDProductionMachine == 107)
                {
                    if (CurOrderOnZechini != null)
                        if (CurOrderOnZechini.Id == curItem.IDProductionOrder)
                        {
                            var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                            _imgSemaphore.ToolTip = "Attualmente in lavorazione su ZECHINI";
                            _imgSemaphore.Width = 40;
                            var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                            _lblSemaphore.Text = string.Format("{0:P0} completato", CurOrderOnZechini.PercLavorata);
                        }
                }
                else
                {
                    if (curItem.isInLav == 1 && curItem.Status == 11)
                    {
                        var _imgSemaphore = (Image)e.Row.Cells[2].FindControl("imgSemaphore");
                        _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                        _imgSemaphore.ToolTip = "Attualmente in lavorazione";
                        _imgSemaphore.Width = 40;
                        var _lblSemaphore = (Label)e.Row.Cells[2].FindControl("lblSemaphore");
                        _lblSemaphore.Text = string.Format("{0:P0} completato", curItem.PercLavExe);
                    }

                }

                if (curItem.ID == SelectedRowId)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                }
            }

        }

        protected void hypEdit_Click(object sender, ImageClickEventArgs e)
        {
            EditMode = !EditMode;
            BindGrids();
        }

        //protected void lblShowHideMP_Click(object sender, EventArgs e)
        //{
        //    grdProductionMPS.Visible = !grdProductionMPS.Visible;
        //}

        protected void ldsDeliveryTrips_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            using (QuotationDataContext ctx = new QuotationDataContext())
                e.Result = ProductionOrderDetailsInsertController.GetDeliveryTrips(ctx, null, 0);
        }

        //protected void grdDeliveryTrips_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Selected")
        //    {
        //        using (QuotationDataContext ctx = new QuotationDataContext())
        //        {
        //            DeliveryTrip trip = ctx.DeliveryTrips.FirstOrDefault(d => d.ID == Convert.ToInt32(e.CommandArgument));
        //            DeliveryTripDetail detail = ctx.DeliveryTripDetails.FirstOrDefault(d => d.Direction == 'R' && d.ID_ProductionOrder == POIdParameter);
        //            if (detail != null)
        //            {
        //                ctx.DeliveryTripDetails.DeleteOnSubmit(detail);
        //            }

        //            int siblingsCount = ctx.DeliveryTripDetails.Where(d => d.ID_DeliveryTrip == Convert.ToInt32(e.CommandArgument) && d.Direction == 'R').Count();
        //            siblingsCount += 1;
        //            DeliveryTripDetail newItem = new DeliveryTripDetail();
        //            newItem.ID_DeliveryTrip = trip.ID;
        //            newItem.ID_Owner = WebUser.Employee.ID;
        //            newItem.ID_ProductionOrder = POIdParameter;
        //            newItem.InsertDate = DateTime.Now;
        //            newItem.Direction = 'R';
        //            newItem.Quota = 1m / siblingsCount;
        //            ctx.DeliveryTripDetails.InsertOnSubmit(newItem);

        //            foreach (DeliveryTripDetail sibling in ctx.DeliveryTripDetails.Where(d => d.ID_DeliveryTrip == Convert.ToInt32(e.CommandArgument)))
        //            {
        //                sibling.Quota = 1m / siblingsCount;
        //            }

        //            ctx.SubmitChanges();
        //        }
        //    }
        //    if (e.CommandName == "Closed")
        //    {
        //        using (QuotationDataContext ctx = new QuotationDataContext())
        //        {
        //            var dataOptions = new DataLoadOptions();
        //            dataOptions.LoadWith<DeliveryTrip>(c => c.DeliveryTripDetails);
        //            dataOptions.LoadWith<DeliveryTripDetail>(c => c.ProductionOrder);
        //            ctx.LoadOptions = dataOptions;

        //            DeliveryTrip trip = ctx.DeliveryTrips.FirstOrDefault(d => d.ID == Convert.ToInt32(e.CommandArgument));
        //            trip.Status = 1;
        //            trip.EndDate = DateTime.Now;

        //            new ProductionOrderDetailsInsertController().RecalcDeliveryCostDistribution(ctx, trip);
        //            ctx.SubmitChanges();
        //        }
        //    }
        //}

        //protected void grdDeliveryTrips_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        DeliveryTrip item = e.Row.DataItem as DeliveryTrip;

        //        using (QuotationDataContext ctx = new QuotationDataContext())
        //        {
        //            DeliveryTripDetail found = ctx.DeliveryTripDetails.FirstOrDefault(d => d.ID_DeliveryTrip == item.ID && d.Direction == 'R' && d.ID_ProductionOrder == POIdParameter);
        //            if (found != null)
        //            {
        //                e.Row.RowState = DataControlRowState.Selected;
        //            }
        //            else
        //            {
        //                e.Row.RowState = DataControlRowState.Normal;
        //            }
        //        }

        //        var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
        //        _hypEdit.Attributes.Add("onclick", "javascript:OpenItem('DeliveryTripPopup.aspx?" + DTIdKey + "=" + item.ID + "')");

        //    }

        //}

        //protected void grdDeliveryTrips_PreRender(object sender, EventArgs e)
        //{
        //    grdDeliveryTrips.DataBind();
        //}

        //protected void lblShowHideTrips_Click(object sender, EventArgs e)
        //{
        //    pnlTrips.Visible = !pnlTrips.Visible;
        //}

        protected void ldsProductionOrderTechSpecs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            var table = ldsProductionOrderTechSpecs.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            Quotation curQuotation = _qc.ProductionOrders.FirstOrDefault(d => d.ID == POIdParameter).Quotation;

            int[] poIds = _qc.ProductionOrders.Where(d => d.ID_Quotation == curQuotation.ID).Select(d => d.ID).ToArray();
            if (poIds != null)
            {
                e.Result = _qc.ProductionOrderTechSpecs.Where(s => poIds.Contains(s.ID_ProductionOrder.GetValueOrDefault())).OrderBy(d => d.ProductionDate).ToList();
            }

        }

        protected void lbtShowHideTechSpecs_Click(object sender, EventArgs e)
        {
            pnlTechSpecs.Visible = !pnlTechSpecs.Visible;
        }

        protected void dtvPlasticCoating_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
                EcoSystemGateway.RefreshMachineSchedule(db);
        }

        protected void ldsPlasticCoating_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvProductionOrder.CurrentMode != DetailsViewMode.Insert)
            {
                var table = ldsPlasticCoating.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.PlasticCoatingMachineParameters.Where(po => po.Id_ProductionOrder == POIdParameter);
            }
        }


        //protected void ldsPriceCom_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    var table = ldsPriceCom.GetTable();
        //    var _qc = (QuotationDataContext)table.CreateContext();
        //    e.Result = _qc.ProductionOrders.FirstOrDefault(d => d.ID == POIdParameter).Quotation;
        //}


    }
}