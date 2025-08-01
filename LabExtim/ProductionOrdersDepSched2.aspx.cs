using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Net;
using System.Configuration;

using CMLabExtim.WODClasses;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersDepSched2 : ProductionOrderController
    {

        public DHL CurOrderOnBOBST1;
        //{
        //    get
        //    {
        //        return ViewState["CurOrderOnBOBST1"] as DHL;
        //    }
        //    set
        //    {
        //        ViewState["CurOrderOnBOBST1"] = value;
        //    }
        //}


        public string dlButtonsSelectedIndex
        {
            get
            {
                if (ViewState["dlButtonsSelectedIndex"] != null)
                    return ViewState["dlButtonsSelectedIndex"].ToString();
                return null;
            }
            set
            {
                ViewState["dlButtonsSelectedIndex"] = value;
            }
        }

        //public string rdlGreenOnlySelectedValue
        //{
        //    get
        //    {
        //        if (ViewState["rdlGreenOnlySelectedValue"] != null)
        //            return ViewState["rdlGreenOnlySelectedValue"].ToString();
        //        return null;
        //    }
        //    set
        //    {
        //        ViewState["rdlGreenOnlySelectedValue"] = value;
        //    }
        //}




        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionMPS);
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
            senMain.DropDownList1.SelectedIndexChanged += DropDownList_SelectedIndexChanged;
            senMain.DropDownList2.SelectedIndexChanged += DropDownList_SelectedIndexChanged;
            senMain.DropDownList3.SelectedIndexChanged += DropDownList_SelectedIndexChanged;
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFilter();
        }

        public void SetFilter()
        {
            try
            {
                CurOrderOnBOBST1 = GetRealTimeDataFromBOBST1();
            }
            catch
            {
                CurOrderOnBOBST1 = null;
                lblSuccess.Text = "Impossibile comunicare con la macchina BOBST1.";
            }

            if (senMain.DropDownList2.SelectedValue == "2")
                using (QuotationDataContext db = new QuotationDataContext())
                    EcoSystemGateway.RefreshMachineSchedule(db);

            ldsProductionMPS.AutoGenerateWhereClause = false;

            ldsProductionMPS.WhereParameters.Clear();
            var _filter = "TRUE";

            // merge aziendale
            //if (CurrentCompanyId != -1)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ID_Company", DbType.Int16, CurrentCompanyId.ToString());
            //    _filter += " AND ID_Company = @ID_Company";
            //}

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsProductionMPS.WhereParameters.Add("IDProductionOrder", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND IDProductionOrder = @IDProductionOrder";
            }

            if (!string.IsNullOrEmpty(senMain.YctNumber.ReturnValue))
            {
                if (senMain.YctNumber.ReturnValue.StartsWith("/"))
                {
                    ldsProductionMPS.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue.Substring(senMain.YctNumber.ReturnValue.IndexOf('/') + 1));
                    _filter += " AND Number.Contains(@Number)";
                }
                else if (senMain.YctNumber.ReturnValue.Length > 4)
                {
                    ldsProductionMPS.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue);
                    _filter += " AND Number.Substring(2) == @Number";
                }
                else
                {
                    ldsProductionMPS.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue.Substring(2));
                    //_filter += " AND Year(StartDate) == @Number";
                    _filter += " AND Number.Substring(2,2) == @Number";
                }
            }

            //if (senMain.TextField1Text != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("poDescription", DbType.String, senMain.TextField1Text);
            //    _filter += " AND poDescription.Contains(@poDescription)";
            //}

            if (senMain.ValueHidField1Text != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.ValueHidField1Text);
                _filter += " AND ID_Customer == @ID_Customer";
            }
            if (senMain.ValueHidField2Text != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.ValueHidField2Text);
                _filter += " AND ID_Customer != @ID_Customer";
            }

            //if (senMain.TextDateFromText != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ProdStart", DbType.DateTime,
            //        DateTime.Parse(senMain.TextDateFromText).ToString());
            //    _filter += " AND ProdStart >= @ProdStart";
            //}
            //if (senMain.TextDateToText != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ProdEnd", DbType.DateTime,
            //        DateTime.Parse(senMain.TextDateToText).ToString());
            //    _filter += " AND ProdEnd <= @ProdEnd";
            //}

            _filter += " AND (poStatus == 1 OR poStatus == 2 OR poStatus == 3 OR poStatus == 9)";
            _filter += " AND (Status == 11 OR Status == 15)";

            if (senMain.DropDownList3.SelectedValue != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("ID_Manager", DbType.Int32, senMain.DropDownList3.SelectedValue);
                _filter += " AND ID_Company == @ID_Manager";
            }
            //if (senMain.DropDownList4.SelectedValue != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.DropDownList4.SelectedValue);
            //    _filter += " AND ID_Customer != @ID_Customer";
            //}
            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("IDProductionMachine", DbType.Int32, senMain.DropDownList1.SelectedValue);
                _filter += " AND IDProductionMachine == @IDProductionMachine";
            }
            //if (senMain.DropDownList3.SelectedValue != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("NumProductionMachine", DbType.Int32, senMain.DropDownList3.SelectedValue);
            //    _filter += " AND NumProductionMachine == @NumProductionMachine";
            //}
            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("IDDepartment", DbType.Int32, senMain.DropDownList2.SelectedValue);
                _filter += " AND IDDepartment == @IDDepartment";
            }
            if (rdlGreenOnly.SelectedValue == "P")
            {
                _filter += " AND IDProductionMachine == curMachineID AND poStatus <> 2";
            }
            if (_filter != ("TRUE AND ID_Company = @ID_Company AND (poStatus == 1 OR poStatus == 2 OR poStatus == 3) AND (Status == 11 OR Status == 15)"))
                ldsProductionMPS.Where = _filter;
            else
            {
                ldsProductionMPS.Where = _filter += " AND IDProductionMachine == curMachineId ";
                ldsProductionMPS.WhereParameters.Add("ProdStart", DbType.DateTime,
                    DateTime.Today.AddMonths(-12).ToString());
                ldsProductionMPS.Where = _filter += " AND ProdStart >= @ProdStart";
            }


            if (dlButtonsSelectedIndex != null)
            {
                ldsProductionMPS.WhereParameters.Remove(ldsProductionMPS.WhereParameters["IDProductionMachine"]);
                ldsProductionMPS.Where.Replace(" AND IDProductionMachine == @IDProductionMachine", "");
                if (dlButtonsSelectedIndex != null)
                {
                    ldsProductionMPS.WhereParameters.Add("IDProductionMachine", DbType.Int32, dlButtonsSelectedIndex);
                    ldsProductionMPS.Where += " AND IDProductionMachine == @IDProductionMachine";
                }
            }

        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            if (senMain.DropDownList2.SelectedValue == "10" || senMain.DropDownList2.SelectedValue == "15")
            {
                rdlGreenOnly.SelectedValue = "P";
            }
            else
            {
                rdlGreenOnly.SelectedValue = "T";
            }
            grdProductionMPS.PageIndex = 0;
            SetFilter();

        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            rdlGreenOnly.SelectedValue = "T";
            grdProductionMPS.PageIndex = 0;
            dlButtonsSelectedIndex = null;
            //rdlGreenOnlySelectedValue = "T";
            SetFilter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //FillControls();
                PopulateSearchEngine();
            }
            SetFilter();

        }

        private void PopulateSearchEngine()
        {
            senMain.LblYearCounterText = "Numero OdP";
            //senMain.LblTextField1Text = "Descrizione OdP contiene...";
            senMain.LblTextField1Text = "Cliente da INCLUDERE";
            senMain.LblTextField2Text = "Cliente da ESCLUDERE";
            //senMain.LblDateFromText = "Data produzione da";
            //senMain.LblDateToText = "Data produzione a";

            using (var _qc = new QuotationDataContext())
            {
                //senMain.LblDropDownList1Text = "Stato";
                //var _statusItems =
                //    _qc.Statuses.Where(s => s.StatusType == 3)
                //        .Select(s => new ListItem {Text = s.Description, Value = s.ID.ToString()})
                //        .ToArray();
                //senMain.DropDownList1.Items.AddRange(_statusItems);
                //senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));
                //senMain.DropDownList1.SelectedIndex = 1;

                //senMain.LblDropDownList1Text = "Cliente";
                //var _customerItems =
                //    _qc.Customers.OrderBy(d => d.Name).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                //senMain.DropDownList1.Items.AddRange(_customerItems);
                //senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblDropDownList1Text = "Macchina";
                //var _machineItems =
                //    _qc.ProductionMachines.Where(d => (CurrentCompanyId == -1 || d.ID_Company == CurrentCompanyId)).OrderBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                var _machineItems = _qc.ProductionMachines.Where(d => d.Inserted == true).OrderBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();

                senMain.DropDownList1.Items.AddRange(_machineItems);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutte", ""));
                senMain.DropDownList1.AutoPostBack = true;

                senMain.LblDropDownList2Text = "Reparto";
                var _depItems =
                    _qc.Departments.OrderBy(d => d.ID_Company).ThenBy(d => d.Description).Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                senMain.DropDownList2.Items.AddRange(_depItems);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));
                senMain.DropDownList2.AutoPostBack = true;

                //senMain.LblDropDownList4Text = "Cliente escluso";
                //var _customerItems1 =
                //    _qc.Customers.OrderBy(d => d.Name).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                //senMain.DropDownList4.Items.AddRange(_customerItems);
                //senMain.DropDownList4.Items.Insert(0, new ListItem("Nessuno", ""));

                //senMain.LblDropDownList3Text = "Numero macchina";
                //senMain.DropDownList3.Items.Insert(0, new ListItem("Tutte", ""));
                //senMain.DropDownList3.Items.Insert(1, new ListItem("0", "0"));
                //senMain.DropDownList3.Items.Insert(2, new ListItem("1", "1"));
                //senMain.DropDownList3.Items.Insert(3, new ListItem("2", "2"));

                senMain.LblDropDownList3Text = "Gestione";
                var _managerItems = _qc.Managers.Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                senMain.DropDownList3.Items.AddRange(_managerItems);
                senMain.DropDownList3.Items.Insert(0, new ListItem("Tutti", ""));
                senMain.DropDownList3.AutoPostBack = true;


            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Data consegna", "DeliveryDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Sequenza produttiva", "StartDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Macchina", "ProductionMachine"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Attualmente in", "AttualmenteIn"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Luce semaforo", "Luce semaforo"));


        }

        //private void FillControls()
        //{
        //    lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" + POIdKey + "=-1')");


        //}

        protected void grdProductionMPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionMPS.PageIndex = e.NewPageIndex;
            //SetFilter();
            grdProductionMPS.DataBind();
        }

        //protected void lbtPrintProductionMPS_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "ProductionMPS", GenericPrintPage), true);
        //}

        protected void ldsProductionMPS_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (ldsProductionMPS.WhereParameters["IDProductionMachine"] == null)
                if (ldsProductionMPS.WhereParameters["IDDepartment"] != null)
                {
                    if (ldsProductionMPS.WhereParameters["IDDepartment"].DefaultValue != "")
                    {
                        List<KeyValuePair<int, string>> buttons = new List<KeyValuePair<int, string>>();
                        buttons.Add(new KeyValuePair<int, string>(0, "TUTTE"));
                        buttons.AddRange(((List<VW_ProductionExtMPS_GroupedByPhase>)e.Result)
                            .Where(d => d.IDDepartment == Convert.ToInt32(ldsProductionMPS.WhereParameters["IDDepartment"].DefaultValue))
                            .GroupBy(d => new { d.IDProductionMachine, d.pmDescription })
                            .Select(d => new KeyValuePair<int, string>(d.Key.IDProductionMachine.Value, d.Key.pmDescription))
                            .ToList());
                        dlButtons.DataSource = buttons;
                        dlButtons.DataBind();
                    }
                    else
                    {
                        dlButtons.DataSource = new List<KeyValuePair<int, string>>();
                        dlButtons.DataBind();
                    }
                }
                else
                {
                    dlButtons.DataSource = new List<KeyValuePair<int, string>>();
                    dlButtons.DataBind();
                }
        }

        protected void grdProductionMPS_DataBound(object sender, EventArgs e)
        {
        }

        protected void grdProductionMPS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VW_ProductionExtMPS_GroupedByPhase item = ((VW_ProductionExtMPS_GroupedByPhase)e.Row.DataItem);

                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";


                var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                    ((VW_ProductionExtMPS_GroupedByPhase)e.Row.DataItem).IDProductionOrder + "')");

                //var _CloseCurPhaseLinkButton = (LinkButton)e.Row.Cells[8].FindControl("CloseCurPhaseLinkButton");
                var _CloseCurPhaseLinkButton = (LinkButton)e.Row.Cells[5].FindControl("CloseCurPhaseLinkButton");
                //_CloseCurPhaseLinkButton.Visible = ((item.curPhaseStatus == 11 || item.curPhaseStatus == 15) && item.poStatus == 1);
                _CloseCurPhaseLinkButton.Visible = (((item.Status == 11 || item.Status == 15) && item.poStatus == 1 && item.curPhaseQuotationDetail != -1) || (item.poStatus == 3 && item.curPhaseQuotationDetail == -1));

                //var _CloseCurPhaseAndMaterialLinkButton = (LinkButton)e.Row.Cells[9].FindControl("CloseCurPhaseAndMaterialLinkButton");
                var _CloseCurPhaseAndMaterialLinkButton = (LinkButton)e.Row.Cells[5].FindControl("CloseCurPhaseAndMaterialLinkButton");
                _CloseCurPhaseAndMaterialLinkButton.Visible = ((item.Status == 11 || item.Status == 15) && item.poStatus == 1 && item.curPhaseQuotationDetail != -1);

                var _imgSemaphore = (Image)e.Row.Cells[4].FindControl("imgSemaphore");

                var imgPercExe = (Image)e.Row.Cells[4].FindControl("imgPercExe");
                imgPercExe.Visible = (item.SemaphoreImage == "GreenCircle" && item.ProdEffMin.HasValue);
                if (CurOrderOnBOBST1 != null)
                    imgPercExe.Visible = imgPercExe.Visible || (item.IDProductionOrder.ToString() == CurOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value);


                var lblPercExe = (Label)e.Row.Cells[4].FindControl("lblPercExe");
                lblPercExe.Visible = (item.SemaphoreImage == "GreenCircle" && item.ProdEffMin.HasValue);


                var ibtStart = (Image)e.Row.Cells[6].FindControl("ibtStart");
                ibtStart.Visible = ((!new int[] { 99, 100 }.Contains(item.curMachineId.Value)) && new int[] { 1 }.Contains(item.poStatus)) && !item.ProdEffMin.HasValue && item.SemaphoreImage == "GreenCircle";

                var ibtPause = (Image)e.Row.Cells[6].FindControl("ibtPause");
                ibtPause.Visible = ((!new int[] { 99, 100 }.Contains(item.curMachineId.Value)) && new int[] { 1 }.Contains(item.poStatus)) && item.ProdEffMin.HasValue && item.SemaphoreImage == "GreenCircle";
                if (ibtPause.Visible)
                    if (item.isInLav == 1)
                    {
                        ibtPause.ImageUrl = "~/Images/control_pause.png";
                        ibtPause.ToolTip = "Metti in pausa";
                        _imgSemaphore.Visible = false;

                    }
                    else
                    {
                        ibtPause.ImageUrl = "~/Images/control_play.png";
                        ibtPause.ToolTip = "Riprendi";
                        _imgSemaphore.Visible = true;
                    }


                var ibtEnd = (Image)e.Row.Cells[6].FindControl("ibtEnd");
                ibtEnd.Visible = (new int[] { 1 }.Contains(item.poStatus)) && item.ProdEffMin.HasValue && item.SemaphoreImage == "GreenCircle";
                //ibtEnd.Visible = (new int[] { 1 }.Contains(item.poStatus));

                var ibtForceEnd = (Image)e.Row.Cells[6].FindControl("ibtForceEnd");
                //ibtForceEnd.Visible = (new int[] { 1 }.Contains(item.poStatus)) && !item.ProdEffMin.HasValue;

                ibtForceEnd.Visible = (
                    (new int[] { 1 }.Contains(item.poStatus) && !new int[] { 99, 100 }.Contains(item.curMachineId.Value))
                    ||
                    (new int[] { 3 }.Contains(item.poStatus) && new int[] { 99, 100 }.Contains(item.curMachineId.Value))
                    )
                    && !item.ProdEffMin.HasValue;


                //var _hidIdProductionOrder = (HiddenField)e.Row.Cells[9].FindControl("hidIdProductionOrder");
                var _hidIdProductionOrder = (HiddenField)e.Row.Cells[5].FindControl("hidIdProductionOrder");
                _hidIdProductionOrder.Value = item.IDProductionOrder.ToString();

                //var _hidQuotationDetail = (HiddenField)e.Row.Cells[9].FindControl("hidQuotationDetail");
                var _hidQuotationDetail = (HiddenField)e.Row.Cells[5].FindControl("hidQuotationDetail");
                _hidQuotationDetail.Value = item.IDQuotationDetail.ToString();

                //if (CurOrderOnBOBST1 != null)
                //{
                //    if (item.SemaphoreCode != 0 && (senMain.DropDownList2.SelectedValue == "5" || senMain.DropDownList2.SelectedValue == "7"))
                //    {
                //        var ibtSendToMachine = (ImageButton)e.Row.Cells[0].FindControl("ibtSendToMachine");
                //        ibtSendToMachine.Visible = true;
                //        if (item.IDProductionOrder.ToString() == CurOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value)
                //        {
                //            var _ibtStopBatch = (ImageButton)e.Row.Cells[0].FindControl("ibtStopBatch");
                //            _ibtStopBatch.Visible = true;
                //            ibtSendToMachine.Visible = false;
                //        }
                //    }
                //}

                //if (CurOrderOnBOBST1 != null)
                //{
                //    if (senMain.DropDownList2.SelectedValue == "5" || senMain.DropDownList2.SelectedValue == "7" || item.IDDepartment == 5 || item.IDDepartment == 7)
                //    {
                //        if (item.IDProductionOrder.ToString() == CurOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value)
                //        {
                //            //var _imgSemaphore = (Image)e.Row.Cells[4].FindControl("imgSemaphore");
                //            _imgSemaphore.ImageUrl = "~/Images/under-construction-2.gif";
                //            _imgSemaphore.ToolTip = "Attualmente in lavorazione su BOBST1";
                //            _imgSemaphore.Width = 40;
                //        }
                //    }
                //}

                if (item.Status == 15)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }

                if (item.ID_ExternalCompany.HasValue)
                {
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.BackColor = System.Drawing.Color.Gray;
                }

            }
        }

        protected void ldsProductionMPS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsProductionMPS.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            ldsProductionMPS.OrderByParameters.Clear();
            ldsProductionMPS.AutoGenerateOrderByClause = false;
            //e.Result = _qc.VW_ProductionExtMPs.OrderBy(qt => qt.IDProductionOrder).OrderBy(qt => qt.ProdStart);

            switch (senMain.DdlOrderBy.SelectedValue)
            {

                case ("StartDate"):
                    ldsProductionMPS.OrderByParameters.Clear();
                    ldsProductionMPS.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.OrderBy(qt => qt.ProdStart).ThenBy(qt => qt.ID);
                    break;
                case ("DeliveryDate"):
                    ldsProductionMPS.OrderByParameters.Clear();
                    ldsProductionMPS.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.OrderBy(qt => qt.DeliveryDate).ThenBy(qt => qt.ID);
                    break;
                case ("ProductionMachine"):
                    ldsProductionMPS.OrderByParameters.Clear();
                    ldsProductionMPS.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.OrderBy(qt => qt.pmDescription).ThenBy(qt => qt.ID);
                    break;
                case ("AttualmenteIn"):
                    ldsProductionMPS.OrderByParameters.Clear();
                    ldsProductionMPS.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.OrderBy(qt => qt.curMachineDescription).ThenBy(qt => qt.ID);
                    break;
                case ("Luce semaforo"):
                    ldsProductionMPS.OrderByParameters.Clear();
                    ldsProductionMPS.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_ProductionExtMPS_GroupedByPhases.ToList().OrderBy(qt => qt.SemaphoreCode).ThenBy(qt => qt.ID);
                    break;
                default:

                    break;
            }



        }

        protected void grdProductionMPS_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void grdProductionMPS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var _destinationId = -1;
            if (e.CommandName != "Page")
            {
                _destinationId =
                    Convert.ToInt32(e.CommandArgument.ToString().Split('|')[0] == string.Empty
                        ? 0.ToString()
                        : e.CommandArgument.ToString().Split('|')[0]);
            }
            if (e.CommandName == "Reload")
            {
                UpdateList(false);
                //grdProductionMPS.DataBind();
            }
            //if (e.CommandName == "Delete")
            //{
            //    using (var _qc = new QuotationDataContext())
            //    {
            //        //int _toDeleteId = Convert.ToInt32(e.CommandArgument.ToString());
            //        var _toDeleteProductionOrder = _qc.ProductionMPs.Single(po => po.ID == _destinationId);
            //        _qc.ProductionMPs.DeleteAllOnSubmit(_toDeleteProductionOrder.ProductionMPs);
            //        _qc.SubmitChanges();
            //        _qc.ProductionMPs.DeleteOnSubmit(_toDeleteProductionOrder);
            //        _qc.SubmitChanges();
            //    }
            //    //grdProductionMPS.DataBind();
            //}
            if (e.CommandName == "Close")
            {
                var table = ldsProductionMPS.GetTable();
                using (var _qc = (QuotationDataContext)table.CreateContext())
                {
                    //int _toCloseId = Convert.ToInt32(e.CommandArgument.ToString());
                    var _toDeleteProductionOrder = _qc.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == _destinationId);
                    _toDeleteProductionOrder.ProductionOrder.Status = 3;
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.IDProductionOrder.Value);
                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionMPS.DataBind();
            }

            if (e.CommandName == "CloseToDoPhase")
            {
                var table = ldsProductionMPS.GetTable();
                using (var _qc = (QuotationDataContext)table.CreateContext())
                {
                    //int _toCloseId = Convert.ToInt32(e.CommandArgument.ToString());
                    var _toDeleteProductionOrder = _qc.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == _destinationId);
                    //_toDeleteProductionOrder.ProductionOrder.Status = 3; non de
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.IDProductionOrder.Value, Convert.ToInt32(e.CommandArgument.ToString().Split('|')[1]), true);
                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionMPS.DataBind();
            }

            if (e.CommandName == "CloseCurPhase")
            {
                var table = ldsProductionMPS.GetTable();
                using (var _qc = (QuotationDataContext)table.CreateContext())
                {
                    //int _toCloseId = Convert.ToInt32(e.CommandArgument.ToString());
                    var _toDeleteProductionOrder = _qc.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == _destinationId);
                    //_toDeleteProductionOrder.ProductionOrder.Status = 3; non de
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.IDProductionOrder.Value, Convert.ToInt32(e.CommandArgument.ToString().Split('|')[1]));
                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionMPS.DataBind();
            }

            if (e.CommandName == "GoToQuotation")
            {
                using (var _qc = new QuotationDataContext())
                {
                    try
                    {
                        var m_totals =
                            _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(_destinationId))
                                .ToList();
                        LoadPersistedQuotation = true;
                        Response.Redirect(
                            string.Format("{2}?{0}={1}", QuotationKey, _destinationId, TempQuotationConsolePage), true);
                    }
                    catch
                    {
                        lblSuccess.Text = "Il preventivo No " + _destinationId +
                                          " non può avere quantità zero: modificare la quantità dell'Odp per permetterne il caricamento.";
                    }
                }
            }

            if (e.CommandName == "SendToMachine")
            {
                try
                {
                    ProductionMP _toSendProductionMP = null;
                    var table = ldsProductionMPS.GetTable();
                    using (var _qc = (QuotationDataContext)table.CreateContext())
                    {
                        _toSendProductionMP = _qc.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == Convert.ToInt32(e.CommandArgument));

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                            string.Format("http://{0}/mrpweb?cmd=push&OpRef={1}&OpName=ODP {1}&JobName={3}&JobNumber=PREV {4}",
                            ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"],
                            _toSendProductionMP.IDProductionOrder,
                            _toSendProductionMP.ProductionOrder.Description.Length <= 20 ? _toSendProductionMP.ProductionOrder.Description : _toSendProductionMP.ProductionOrder.Description.Replace("\"", "").Substring(0, 20),
                            _toSendProductionMP.ProductionOrder.Quotation.Subject.Length <= 20 ? _toSendProductionMP.ProductionOrder.Quotation.Subject : _toSendProductionMP.ProductionOrder.Quotation.Subject.Replace("\"", "").Substring(0, 20),
                            _toSendProductionMP.ProductionOrder.Quotation.ID));
                        WebResponse response = request.GetResponse();
                    }
                }
                catch
                {
                    lblSuccess.Text = "Impossibile comunicare con la macchina BOBST1.";
                }
            }
        }

        protected void grdProductionMPS_PreRender(object sender, EventArgs e)
        {
            //grdProductionMPS.DataBind();
        }

        protected void lbtUpdateGrid_Click(object sender, EventArgs e)
        {
            UpdateList(true);
        }

        protected void UpdateList(bool resetCurrentPage)
        {
            if (resetCurrentPage) grdProductionMPS.PageIndex = 0;
            grdProductionMPS.DataBind();
        }

        protected void tmrScheduling_Tick(object sender, EventArgs e)
        {
            UpdateList(true);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCustomers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "1")
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 1 && c.Code <= 199999999).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                    else if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "2")
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 200000000 && c.Code <= 299999999).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        protected void btnMPSRecalc_Click(object sender, EventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderService.RecalcMPS(db, Global.CurrentSchedulingType);

            }
        }

        protected void ibtStopBatch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                WebResponse response = request.GetResponse();
                System.Threading.Thread.Sleep(2000);

            }
            catch
            {
                lblSuccess.Text = "Impossibile comunicare con la macchina BOBST1.";
            }
            SetFilter();
            grdProductionMPS.DataBind();
        }

        protected void rdlGreenOnly_SelectedIndexChanged(object sender, EventArgs e)
        {
            //rdlGreenOnlySelectedValue= rdlGreenOnly.SelectedValue;
            UpdateList(true);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetProductionOrderOpenPhases(string pipedParams)
        {


            try
            {

                string JSONString;
                string[] pars = pipedParams.Split('|');
                int poId = Convert.ToInt32(pars[0]);

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ProductionOrder po = db.ProductionOrders.FirstOrDefault(d => d.ID == poId);
                    string dataT = string.Format("OdP {0} - {1}", po.ID, po.Description);

                    List<VW_ProductionExtMPS_GroupedByPhase> dataD = db.VW_ProductionExtMPS_GroupedByPhases.Where(d => d.IDProductionOrder == poId && d.Status == 11 && d.IDQuotationDetail != -1)
                        .OrderBy(d => Convert.ToInt32(d.Order)).ThenBy(d => d.ProdEnd).ToList();

                    // se è rimasta la sola spedizione (qd = -1), la visualizza per permetterne lo scarico.
                    if (dataD.Count == 0)
                    {
                        dataD = db.VW_ProductionExtMPS_GroupedByPhases.Where(d => d.IDProductionOrder == poId && d.Status == 11)
                        .OrderBy(d => Convert.ToInt32(d.Order)).ThenBy(d => d.ProdEnd).ToList();
                    }

                    JSONString = new JavaScriptSerializer().Serialize(new
                    {
                        header = dataT,
                        details = dataD.Select((d => new
                        {
                            idProductionMp = d.ID,
                            idQuotationDetail = d.IDQuotationDetail,
                            pmDescription = d.pmDescription,
                            prodTimeMin = d.ProdTimeMin
                        }))
                    });
                }
                return JSONString;

            }
            catch (Exception ex)
            {
                return string.Format("Errore: {0}", ex.Message);
            }

        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string SetCurrentProductionPhase(int userId, string pipedParams)
        {

            string[] pars = pipedParams.Split('|');
            int poId = Convert.ToInt32(pars[0]);
            int qdId = Convert.ToInt32(pars[1]);
            bool onProd = false;

            try
            {

                using (QuotationDataContext db = new QuotationDataContext())
                {

                    ProductionOrder po = db.ProductionOrders.Single(p => p.ID == poId);
                    ProductionOrderService.MassimizzaPriorita_Grouped(db, poId, qdId);
                    ProductionOrderService.DeleteProductionOrderSchedule(db, po, false);
                    ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);
                    onProd = ProductionOrderService.StartPauseCurrentProductionPhase(db, userId, poId, qdId);

                    VW_ProductionExtMP wmp = db.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == poId && d.IDQuotationDetail == qdId);

                    //#if !DEBUG

                    // BOBST
                    if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                    {

                        try
                        {
                            DHL curOrderOnBOBST1 = WODGateway.GetRealTimeDataFromBOBST1();
                            if (poId.ToString() != curOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value)
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                                    string.Format("http://{0}/mrpweb?cmd=push&OpRef={1}&OpName=ODP {1}&JobName={3}&JobNumber=PREV {4}",
                                    ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"],
                                    poId,
                                    po.Description.Length <= 20 ? po.Description : po.Description.Replace("\"", "").Substring(0, 20),
                                    po.Quotation.Subject.Length <= 20 ? po.Quotation.Subject : po.Quotation.Subject.Replace("\"", "").Substring(0, 20),
                                    po.Quotation.ID));
                                WebResponse response = request.GetResponse();
                            }
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message);
                        }


                    }

                    // SILKFOIL
                    //if (_toSendProductionMP.IDDepartment.GetValueOrDefault() == 2 || _toSendProductionMP.IDProductionMachine.GetValueOrDefault() == 104)
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                    {
                        try
                        {
                            Snap7Gateway gw = new Snap7Gateway();
                            //gw.UpdateLastDataFromSilkFoil1(db);
                            //gw = new Snap7Gateway();
                            gw.SetOdPDataToSilkFoil1(db, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message);
                        }


                    }

                    // ACCOPPIATRICE MANUALE ZECHINI
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 107)
                    {
                        try
                        {
                            ZechiniGateway gw = new ZechiniGateway();
                            gw.SendNewDataset();
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina ACCOPPIATRICE MANUALE ZECHINI - {0}", ex.Message);
                        }


                    }

                    //#endif


                    db.SubmitChanges();
                }

                string JSONString = new JavaScriptSerializer().Serialize(new
                {
                    header = onProd,

                });
                return JSONString;

            }
            catch (Exception ex)
            {
                return string.Format("Errore in prioritizzazione ed avvio della fase Odp: {0}- Qd: {1}, ritentare", poId, qdId);
            }

        }


        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCurrentPhaseTotalTime(string pipedParams)
        {


            try
            {

                string JSONString;
                string[] pars = pipedParams.Split('|');
                int poId = Convert.ToInt32(pars[0]);
                int qdId = Convert.ToInt32(pars[1]);

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ProductionOrder po = db.ProductionOrders.FirstOrDefault(d => d.ID == poId);
                    string dataT = string.Format("OdP {0} - {1}", po.ID, po.Description);

                    VW_ProductionExtMPS_GroupedByPhase dataD = db.VW_ProductionExtMPS_GroupedByPhases.FirstOrDefault(d => d.IDProductionOrder == poId && d.Status == 11 && d.IDQuotationDetail == qdId);
                    JSONString = new JavaScriptSerializer().Serialize(new
                    {
                        header = dataT,
                        detail = new
                        {
                            idProductionMp = dataD.ID,
                            idQuotationDetail = dataD.IDQuotationDetail,
                            pmDescription = dataD.pmDescription,
                            prodTimeMin = dataD.ProdTimeMin,
                            prodEffMin = dataD.ProdEffMin
                        }
                    });
                }
                return JSONString;

            }
            catch (Exception ex)
            {
                return string.Format("Errore: {0}", ex.Message);
            }

        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CloseCurrentProductionPhase(int userId, string pipedParams)
        {

            string[] pars = pipedParams.Split('|');
            int poId = Convert.ToInt32(pars[0]);
            int qdId = Convert.ToInt32(pars[1]);

            try
            {

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    //var mp = db.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == poId);

                    VW_ProductionExtMP wmp = db.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == poId && d.IDQuotationDetail == qdId);
                    ProductionOrderService.CloseProductionOrderSchedule(db, poId, qdId);
                    ProductionOrderService.StartPauseCurrentProductionPhase(db, userId, poId, qdId, true);


                    //#if !DEBUG

                    //BOBST (oro caldo)
                    if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                    {

                        try
                        {
                            DHL curOrderOnBOBST1 = WODGateway.GetRealTimeDataFromBOBST1();
                            if (poId.ToString() == curOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value)
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                                WebResponse response = request.GetResponse();
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message);
                        }


                    }

                    // SILKFOIL
                    //if (_toSendProductionMP.IDDepartment.GetValueOrDefault() == 2 || _toSendProductionMP.IDProductionMachine.GetValueOrDefault() == 104)
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                    {
                        try
                        {
                            Snap7Gateway gw = new Snap7Gateway();
                            gw.GetCurrentDataFromSilkFoil1(db, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message);
                        }


                    }

                    // EUROPROGETTI
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 77)
                    {
                        try
                        {
                            using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                            {
                                EuroProgettiGateway.Close_EuroProgetti_DB_Ordine(db, dbRem, wmp.IDProductionOrder.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina EUROPROGETTI - {0}", ex.Message);
                        }
                    }


                    //#endif
                    db.SubmitChanges();

                }
                return string.Format("La fase è stata chiusa correttamente");
            }
            catch (Exception ex)
            {
                return string.Format("Errore in chiusura della fase Odp: {0}- Qd: {1}, ritentare", poId, qdId);
            }

        }



        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string ForceCloseCurrentProductionPhase(int userId, string pipedParams)
        {

            string[] pars = pipedParams.Split('|');
            int poId = Convert.ToInt32(pars[0]);
            int qdId = Convert.ToInt32(pars[1]);

            try
            {

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    //var mp = db.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == poId);
                    //ProductionOrder pof = db.ProductionOrders.Single(p => p.ID == poId);

                    VW_ProductionExtMP wmp = db.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == poId && d.IDQuotationDetail == qdId);
                    ProductionOrderService.CloseProductionOrderSchedule(db, poId, qdId);
                    ProductionOrderService.StartPauseCurrentProductionPhase(db, userId, poId, qdId, true);

                    //#if !DEBUG
                    // BOBST
                    if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                    {

                        try
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                            WebResponse response = request.GetResponse();
                            System.Threading.Thread.Sleep(2000);
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message);
                        }

                    }
                    // SILKFOIL
                    //if (_toSendProductionMP.IDDepartment.GetValueOrDefault() == 2 || _toSendProductionMP.IDProductionMachine.GetValueOrDefault() == 104)
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                    {
                        try
                        {
                            Snap7Gateway gw = new Snap7Gateway();
                            gw.GetCurrentDataFromSilkFoil1(db, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message);
                        }

                    }
                    // EUROPROGETTI
                    if (wmp.IDProductionMachine.GetValueOrDefault() == 77)
                    {
                        try
                        {
                            using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                            {
                                EuroProgettiGateway.Close_EuroProgetti_DB_Ordine(db, dbRem, wmp.IDProductionOrder.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            return string.Format("Impossibile comunicare con la macchina EUROPROGETTI - {0}", ex.Message);
                        }
                    }


                    //#endif

                    db.SubmitChanges();
                }
                return string.Format("La fase è stata chiusa correttamente");
            }
            catch (Exception ex)
            {
                return string.Format("Errore in chiusura della fase Odp: {0}- Qd: {1}, ritentare", poId, qdId);
            }

        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string PauseRestartCurrentProductionPhase(int userId, string pipedParams)
        {

            string[] pars = pipedParams.Split('|');
            int poId = Convert.ToInt32(pars[0]);
            int qdId = Convert.ToInt32(pars[1]);
            bool onProd = false;

            try
            {

                using (QuotationDataContext db = new QuotationDataContext())
                {

                    onProd = ProductionOrderService.StartPauseCurrentProductionPhase(db, userId, poId, qdId);
                    VW_ProductionExtMP wmp = db.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == poId && d.IDQuotationDetail == qdId);

                    //#if !DEBUG
                    // se non si trova  in produzione (premuto riavvio)
                    if (onProd)
                    {
                        // BOBST
                        if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                        {
                            try
                            {
                                DHL curOrderOnBOBST1 = WODGateway.GetRealTimeDataFromBOBST1();
                                ProductionOrder pof = db.ProductionOrders.Single(p => p.ID == poId);
                                if (poId.ToString() != curOrderOnBOBST1.Var.FirstOrDefault(d => d.Prefix == "Bobst_Machine" && d.Suffix == "OperationRef").Value)
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                                        string.Format("http://{0}/mrpweb?cmd=push&OpRef={1}&OpName=ODP {1}&JobName={3}&JobNumber=PREV {4}",
                                        ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"],
                                        poId,
                                        pof.Description.Length <= 20 ? pof.Description : pof.Description.Replace("\"", "").Substring(0, 20),
                                        pof.Quotation.Subject.Length <= 20 ? pof.Quotation.Subject : pof.Quotation.Subject.Replace("\"", "").Substring(0, 20),
                                        pof.Quotation.ID));
                                    WebResponse response = request.GetResponse();
                                }
                            }
                            catch (Exception ex)
                            {
                                return string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message);
                            }
                        }

                        // SILKFOIL
                        if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                        {
                            try
                            {
                                Snap7Gateway gw = new Snap7Gateway();
                                //gw.UpdateLastDataFromSilkFoil1(db);
                                //gw = new Snap7Gateway();
                                gw.SetOdPDataToSilkFoil1(db, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                                //gw = new Snap7Gateway();
                                //gw.SetPauseSignal(wmp.IDProductionOrder.Value, true);
                            }
                            catch (Exception ex)
                            {
                                return string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message);
                            }


                        }


                    }
                    // se si trova  in produzione (premuto pausa)
                    else
                    {
                        // SILKFOIL
                        //if (_toSendProductionMP.IDDepartment.GetValueOrDefault() == 2 || _toSendProductionMP.IDProductionMachine.GetValueOrDefault() == 104)
                        if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                        {
                            try
                            {
                                Snap7Gateway gw = new Snap7Gateway();
                                gw.GetCurrentDataFromSilkFoil1(db, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                            }
                            catch (Exception ex)
                            {
                                return string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message);
                            }

                        }
                    }
                    //#endif

                    db.SubmitChanges();
                }
                string JSONString = new JavaScriptSerializer().Serialize(new
                    {
                        header = onProd,

                    });
                return JSONString;

            }
            catch (Exception ex)
            {
                return string.Format("Errore in avvio, pausa o chiusura della fase Odp: {0}- Qd: {1}, ritentare", poId, qdId);
            }

        }

        protected void dlButtons_ItemCommand(object source, DataListCommandEventArgs e)
        {
            dlButtonsSelectedIndex = e.CommandArgument.ToString();
            ldsProductionMPS.WhereParameters.Remove(ldsProductionMPS.WhereParameters["IDProductionMachine"]);
            ldsProductionMPS.Where.Replace(" AND IDProductionMachine == @IDProductionMachine", "");
            if (e.CommandArgument.ToString() != "0")
            {
                ldsProductionMPS.WhereParameters.Add("IDProductionMachine", DbType.Int32, e.CommandArgument.ToString());
                ldsProductionMPS.Where += " AND IDProductionMachine == @IDProductionMachine";
            }
            grdProductionMPS.DataBind();
        }



    }


}