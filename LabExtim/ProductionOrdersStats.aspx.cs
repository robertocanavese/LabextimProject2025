using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.UI;
using System.Reflection;

using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersStats : ProductionOrderController
    {
        //protected List<SPProductionOrderCalculated> m_calculated;

        private HtmlTextWriter m_HtmlTextWriter;
        private StringWriter m_StringWriter;
        private bool isExporting;

        private string GridRender
        {
            get { return Session["ProductionOrdersStatsGridRender"].ToString(); }
            set { Session["ProductionOrdersStatsGridRender"] = value; }
        }

        private string dlsNonConformitiesRender
        {
            get { return Session["ProductionOrdersStatsdlsNonConformitiesRender"].ToString(); }
            set { Session["ProductionOrdersStatsdlsNonConformitiesRender"] = value; }
        }
        private string dlsCorrectiveActionsRender
        {
            get { return Session["ProductionOrdersStatsdlsCorrectiveActionsRender"].ToString(); }
            set { Session["ProductionOrdersStatsdlsCorrectiveActionsRender"] = value; }
        }


        protected Mode CurProductionOrdersConsoleMode
        {
            get
            {
                if (ViewState["CurProductionOrdersConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode)ViewState["CurProductionOrdersConsoleMode"];
            }
            set { ViewState["CurProductionOrdersConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionOrdersStats);
            //DynamicDataManager1.RegisterControl(grdQuotationResults);
            itbNoOdP.SearchClick += StartSearch;
            itbNo.SearchClick += StartSearch;

        }

        public void StartSearch(object sender, EventArgs e)
        {
            grdProductionOrdersStats.DataBind();
        }
        protected void lbtUpdateGrid_Click(Object sender, EventArgs e)
        {
            grdProductionOrdersStats.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = lbtUpdateGrid.UniqueID;
            if (!IsPostBack)
            {
                FillControls();
            }

        }

        private void FillControls()
        {
            ddlOrderBy.Items.Add(new ListItem("ID OdP", "ID"));
            ddlOrderBy.Items.Add(new ListItem("Numero OdP", "Number"));
            ddlOrderBy.Items.Add(new ListItem("Data fine", "DataBolla"));
            ddlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            ddlOrderBy.Items.Add(new ListItem("Proprietario preventivo", "OwnerUniqueName"));
            ddlOrderBy.Items.Add(new ListItem("% risparmio", "Margin"));
            ddlOrderBy.Items.Add(new ListItem("% risparmio (discendente)", "Margin DESC"));
            ddlOrderBy.DataBind();


            using (QuotationDataContext db = new QuotationDataContext())
            {
                dlsNonConformitiesRender = "";
                foreach (string row in db.NonConformities.Select(d => string.Format("{0}-{1}  ", d.ID, d.Description)))
                    dlsNonConformitiesRender += row;
                dlsCorrectiveActionsRender = "";
                foreach (string row in db.CorrectiveActions.Select(d => string.Format("{0}-{1}  ", d.ID, d.Description)))
                    dlsCorrectiveActionsRender += row;

                dlsNonConformities.DataSource = new QuotationDataContext().NonConformities.Select(d => string.Format("<b>{0}</b>  {1}", d.ID, d.Description)).ToArray();
                dlsNonConformities.DataBind();
                dlsCorrectiveActions.DataSource = new QuotationDataContext().CorrectiveActions.Select(d => string.Format("<b>{0}</b>  {1}", d.ID, d.Description)).ToArray();
                dlsCorrectiveActions.DataBind();

            }



        }

        protected void lbtEmpty_Click(object sender, EventArgs e)
        {
            itbNo.Text = string.Empty;
            yctNumber.Text = string.Empty;
            txtBolFrom.Text = "";
            txtBolTo.Text = "";
            txtCustomer.Text = "";
            txtDateStartFrom.Text = "";
            txtDateStartTo.Text = "";
            //txtEndDateFrom.Text = "";
            //txtEndDateTo.Text = "";
            txtTitleContains.Text = "";

            hidCustomer.Value = "";

            ddlAgente1.SelectedIndex = 0;
            //ddlAgente2.SelectedIndex = 0;
            ddlClosed.SelectedIndex = 0;
            ddlLossMaking.SelectedIndex = 0;
            ddlOrderBy.SelectedIndex = 0;
            ddlOwners.SelectedIndex = 0;
            ddlNonConformities.SelectedIndex = 0;
            ddlComplaintReceived.SelectedIndex = 0;
            ddlCorrectiveActions.SelectedIndex = 0;
            ddlManagers.SelectedIndex = 0;
            ddlOperators.SelectedIndex = 0;
            PersistSelection(null, null);

        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = true.ToString();
            grdProductionOrdersStats.AutoGenerateDeleteButton = false;
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void grdProductionOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrdersStats.PageIndex = e.NewPageIndex;
        }

        protected void lbtPrintProductionOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}", GenericReportKey, "ProductionOrdersStats", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdProductionOrdersStats.EditIndex = -1;
            grdProductionOrdersStats.PageIndex = 0;
        }

        protected void ddlOwners_DataBound(object sender, EventArgs e)
        {
            ddlOwners.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["ProductionOrdersStatsOwnersSelector"] != null)
            {
                ddlOwners.Items.FindByValue(Session["ProductionOrdersStatsOwnersSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlAgente1_DataBound(object sender, EventArgs e)
        {
            ddlAgente1.Items.Insert(0, new ListItem("Tutti", ""));

            if (Session["Agente1OrderBySelector"] != null)
            {
                ddlAgente1.Items.FindByValue(Session["Agente1OrderBySelector"].ToString()).Selected = true;
            }
        }

        //protected void ddlAgente2_DataBound(object sender, EventArgs e)
        //{
        //    ddlAgente2.Items.Insert(0, new ListItem("Tutti", ""));
        //    if (Session["Agente2OrderBySelector"] != null)
        //    {
        //        ddlAgente2.Items.FindByValue(Session["Agente2OrderBySelector"].ToString()).Selected =
        //            true;
        //    }
        //}

        //protected void ddlCustomers_DataBound(object sender, EventArgs e)
        //{
        //    ddlCustomers.Items.Insert(0, new ListItem("Tutti", ""));
        //    if (Session["ProductionOrdersStatsCustomersSelector"] != null)
        //    {
        //        ddlCustomers.Items.FindByValue(Session["ProductionOrdersStatsCustomersSelector"].ToString()).Selected =
        //            true;
        //    }
        //}

        protected void ddlNonConformities_DataBound(object sender, EventArgs e)
        {
            ddlNonConformities.Items.Insert(0, new ListItem("Tutte", ""));
            if (Session["NonConformitySelector"] != null)
            {
                ddlNonConformities.Items.FindByValue(Session["NonConformitySelector"].ToString()).Selected = true;
            }
        }

        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["ProductionOrdersStatsOrderBySelector"] != null)
            {
                ddlOrderBy.Items.FindByValue(Session["ProductionOrdersStatsOrderBySelector"].ToString()).Selected = true;
            }
        }

        protected void ddlManagers_DataBound(object sender, EventArgs e)
        {
            ddlManagers.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["ProductionOrdersStatsManagersSelector"] != null)
            {
                ddlManagers.Items.FindByValue(Session["ProductionOrdersStatsManagersSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlCorrectiveActions_DataBound(object sender, EventArgs e)
        {
            ddlCorrectiveActions.Items.Insert(0, new ListItem("Tutte", ""));
            if (Session["ProductionOrdersStatsCorrectiveActionsSelector"] != null)
            {
                ddlCorrectiveActions.Items.FindByValue(Session["ProductionOrdersStatsCorrectiveActionsSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlComplaintReceived_DataBound(object sender, EventArgs e)
        {
            if (Session["ProductionOrdersStatsComplaintReceivedSelector"] != null)
            {
                ddlComplaintReceived.Items.FindByValue(Session["ProductionOrdersStatsComplaintReceivedSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlOperators_DataBound(object sender, EventArgs e)
        {
            ddlOperators.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["ProductionOrdersStatsOperatorsSelector"] != null)
            {
                ddlOperators.Items.FindByValue(Session["ProductionOrdersStatsOperatorsSelector"].ToString()).Selected = true;
            }
        }

        double _costoPreventivoTotal;
        double _costoDaConsuntivo;
        decimal _costoDaStoricoConsuntivo;
        decimal _totaleCosti;
        decimal _fatturato;
        decimal _differenza;
        decimal _mediaRisparmio;
        decimal _PORTotHistoricalOrNotCost;
        decimal _ProvvTotValue;

        protected void grdProductionOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                _costoPreventivoTotal = 0d;
                _costoDaConsuntivo = 0d;
                _costoDaStoricoConsuntivo = 0m;
                _totaleCosti = 0m;
                _fatturato = 0m;
                _differenza = 0m;
                _mediaRisparmio = 0m;
                _PORTotHistoricalOrNotCost = 0m;
                _ProvvTotValue = 0m;

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                e.Row.Cells[14].Attributes["onmouseover"] = string.Format("this.title='{0}'", dlsNonConformitiesRender);
                e.Row.Cells[14].Attributes["onmouseout"] = "this.title=''";

                e.Row.Cells[18].Attributes["onmouseover"] = string.Format("this.title='{0}'", dlsCorrectiveActionsRender);
                e.Row.Cells[18].Attributes["onmouseout"] = "this.title=''";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                var _dycID = (DynamicControl)e.Row.Cells[2].FindControl("dycID");
                var _dycNumber = (DynamicControl)e.Row.Cells[3].FindControl("dycNumber");
                if (((VW_QUOPORCostsPrice)e.Row.DataItem).Status != 3)
                {
                    _dycID.CssClass = "red";
                    _dycNumber.CssClass = "red";
                }

                //HyperLink _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                //_hypEdit.Attributes.Add("onclick", "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" + ((VW_QUOPORCostsPrice)e.Row.DataItem).ID + "')");

                // 20141119 DAVIDE BOGNOLO
                // AGGIUNTO PARAMENTRO ALLA CHIAMATA OpenBigItem2 in modo da personalizzare la finestra di apertura per poidkey e abilitare in questo modo la possibilità
                // di avere più finestre aperte di diversi ordini.
                var _hypDetails = (HyperLink)e.Row.Cells[1].FindControl("hypDetails");
                _hypDetails.Attributes.Add("onclick",
                    "javascript:OpenBigItem2('ProductionOrderQuotationStats.aspx?" + POIdKey + "=" +
                    ((VW_QUOPORCostsPrice)e.Row.DataItem).ID + "' , " + ((VW_QUOPORCostsPrice)e.Row.DataItem).ID +
                    " ) ");

                var _lbtClose = (LinkButton)e.Row.Cells[1].FindControl("CloseLinkButton");

                if (((VW_QUOPORCostsPrice)e.Row.DataItem).Status == 3)
                {
                    _lbtClose.Enabled = false;
                    _lbtClose.Text = "Evaso";
                    _lbtClose.OnClientClick = "";
                }


                var qUoporCostsPrices = ((VW_QUOPORCostsPrice)e.Row.DataItem);
                _costoPreventivoTotal += qUoporCostsPrices.QUOTotCost.GetValueOrDefault(0d);
                _costoDaConsuntivo += qUoporCostsPrices.PORTotCost.GetValueOrDefault(0d);

                _costoDaStoricoConsuntivo += qUoporCostsPrices.PORTotHistoricalCost.GetValueOrDefault(0m);
                _totaleCosti += qUoporCostsPrices.TotCosts.GetValueOrDefault(0m);
                _fatturato += qUoporCostsPrices.FATTotValue.GetValueOrDefault(0m);
                _differenza += qUoporCostsPrices.Saving.GetValueOrDefault(0m);
                //_mediaRisparmio += qUoporCostsPrices.PercentageSaving ?? 0;

                _PORTotHistoricalOrNotCost += qUoporCostsPrices.PORTotHistoricalOrNotCost.GetValueOrDefault(0m);
                _ProvvTotValue += qUoporCostsPrices.ProvvTotValue.GetValueOrDefault(0m);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[5 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5 + 1].Text = "TOTALI";
                e.Row.Cells[6 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6 + 1].Text = _costoPreventivoTotal.ToString("N2");

                e.Row.Cells[7 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7 + 1].Text = _costoDaConsuntivo.ToString("N2");


                e.Row.Cells[8 + 1].HorizontalAlign = HorizontalAlign.Right;
                //e.Row.Cells[8 + 1].Text = _costoDaStoricoConsuntivo.ToString("N2");
                e.Row.Cells[8 + 1].Text = _ProvvTotValue.ToString("N2");


                e.Row.Cells[9 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[9 + 1].Text = _totaleCosti.ToString("N2");
                e.Row.Cells[10 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[10 + 1].Text = _fatturato.ToString("N2");
                e.Row.Cells[11 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[11 + 1].Text = _differenza.ToString("N2");

                if (_fatturato != 0m)
                {
                    _mediaRisparmio = (_fatturato - _PORTotHistoricalOrNotCost - _ProvvTotValue) / Math.Abs(_fatturato); // dobbiamo disattivare il fatturato negativo
                }
                e.Row.Cells[12 + 1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[12 + 1].Text = _mediaRisparmio.ToString("P2");
            }




        }

        protected void grdProductionOrders_DataBound(object sender, EventArgs e)
        {

            grdProductionOrdersStats.Columns[2].Visible = !isExporting;
            grdProductionOrdersStats.Columns[14].Visible = !isExporting;
            grdProductionOrdersStats.Columns[15].Visible = isExporting;
            grdProductionOrdersStats.Columns[16].Visible = !isExporting;
            grdProductionOrdersStats.Columns[17].Visible = isExporting;
            grdProductionOrdersStats.Columns[18].Visible = !isExporting;
            grdProductionOrdersStats.Columns[19].Visible = isExporting;

            if (grdProductionOrdersStats.Rows.Count > 0)
            {
                m_StringWriter = new StringWriter();
                m_HtmlTextWriter = new HtmlTextWriter(m_StringWriter);
                grdProductionOrdersStats.RenderControl(m_HtmlTextWriter);
                GridRender = m_StringWriter.ToString().Replace("<textarea", "<span").Replace("</textarea", "</span").Replace("<a", "<span").Replace("</a", "</span").Replace("€", "Euro");
                m_StringWriter.Dispose();
                m_HtmlTextWriter.Dispose();
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void grdProductionOrders_PreRender(object sender, EventArgs e)
        {


        }

        protected void ldsProductionOrders_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var _qc = new DLLabExtim.QuotationDataContext();
            _qc.CommandTimeout = 60;


            //var _result = _qc.VW_QUOPORCostsPrices.Join(_qc.ProductionOrders, vw => vw.ID, po => po.ID, (vw, po) => new VW_QUOPORCostsPrice
            //var _result = _qc.VW_QUOPORCostsPrices.Join(_qc.Customers, vw => vw.PORID_Customer, cu => cu.Code,
            //    (vw, cu) => new VW_QUOPORCostsPrices
            //    {
            //        FATTotValue = vw.FATTotValue,
            //        ProvvTotValue = vw.ProvvTotValue,
            //        ID = vw.ID,
            //        Number = vw.Number,
            //        ID_Quotation = vw.ID_Quotation,
            //        PORID_Customer = vw.PORID_Customer,
            //        PORSProducedQuantity = vw.PORSProducedQuantity,
            //        PORSQuantity = vw.PORSQuantity,
            //        PORTotCost = vw.PORTotCost,
            //        PORTotHistoricalCost = vw.PORTotHistoricalCost,
            //        QUOOwner = vw.QUOOwner,
            //        QUOSubject = vw.QUOSubject,
            //        QUOTotCost = vw.QUOTotCost,
            //        Status = vw.Status,
            //        ID_Customer = vw.ID_Customer,
            //        //CustomerName = po.Customer.Name
            //        //ID_Customer = vw.ID_Customer,
            //        CustomerName = cu.Name,
            //        StartDate = vw.StartDate,
            //        EndDate = vw.EndDate,
            //        DataBolla = vw.DataBolla,
            //        AccountNote = vw.AccountNote,
            //        Note = vw.Note,
            //        OwnerName = vw.OwnerName,
            //        IDAgente1 = vw.IDAgente1,
            //        DescrizioneAgente1 = vw.DescrizioneAgente1,
            //        PriceCom = vw.PriceCom
            //        //Agente1 = cu.IDAgente1,
            //        // Agente2 = vw.Agente2
            //    });


            var _result = _qc.VW_QUOPORCostsPrices.AsEnumerable();

            var _dateEndFrom = DateTime.MinValue;
            var _dateEndTo = DateTime.MinValue;
            var _dateStartFrom = DateTime.MinValue;
            var _dateStartTo = DateTime.MinValue;
            var _dateBolFrom = DateTime.MinValue;
            var _dateBolTo = DateTime.MinValue;

            //DateTime.TryParse(txtEndDateFrom.Text, out _dateEndFrom);
            //DateTime.TryParse(txtEndDateTo.Text, out _dateEndTo);
            DateTime.TryParse(txtDateStartFrom.Text, out _dateStartFrom);
            DateTime.TryParse(txtDateStartTo.Text, out _dateStartTo);
            DateTime.TryParse(txtBolFrom.Text, out _dateBolFrom);
            DateTime.TryParse(txtBolTo.Text, out _dateBolTo);

            // merge aziendale
            //_result = _result.Where(po => (CurrentCompanyId == -1 || po.ID_Company == CurrentCompanyId));

            if (_dateEndFrom != DateTime.MinValue)
                _result = _result.Where(po => po.EndDate >= _dateEndFrom);
            if (_dateEndTo != DateTime.MinValue)
                _result = _result.Where(po => po.EndDate <= _dateEndTo);

            if (_dateStartFrom != DateTime.MinValue)
                _result = _result.Where(po => po.StartDate >= _dateStartFrom);
            if (_dateStartTo != DateTime.MinValue)
                _result = _result.Where(po => po.StartDate <= _dateStartTo);

            if (_dateBolFrom != DateTime.MinValue)
                _result = _result.Where(po => po.DataBolla >= _dateBolFrom);
            if (_dateBolTo != DateTime.MinValue)
                _result = _result.Where(po => po.DataBolla <= _dateBolTo);

            if (ddlAgente1.Text != "")
            {
                _result = _result.Where(po => po.IDAgente1 == Convert.ToInt32(ddlAgente1.SelectedValue));
            }

            if (ddlNonConformities.Text != "")
            {
                _result = _result.Where(po => po.NonConformityCode == Convert.ToInt32(ddlNonConformities.SelectedValue));
            }

            if (ddlComplaintReceived.Text != "")
            {
                _result = _result.Where(po => po.ComplaintReceived == Convert.ToInt32(ddlComplaintReceived.SelectedValue));
            }

            if (ddlCorrectiveActions.Text != "")
            {
                _result = _result.Where(po => po.CorrectiveActionCode == Convert.ToInt32(ddlCorrectiveActions.SelectedValue));
            }

            if (ddlManagers.Text != "")
            {
                _result = _result.Where(po => po.ID_Manager == Convert.ToInt32(ddlManagers.SelectedValue));
            }

            //if (chkLossMaking.Checked)
            //    _result = _result.Where(po =>
            //        po.FATTotValue.GetValueOrDefault(0m) > 0m && (po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m) - Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))) < 0m);

            if (!string.IsNullOrEmpty(itbNoOdP.Text))
            {
                var _temp = 0;
                int.TryParse(itbNoOdP.Text, out _temp);
                _result = _result.Where(po => po.ID == _temp);
            }
            else
            {
                if (!IsPostBack)
                {
                    _result = _result.Where(po => po.ID == 2);
                }
            }

            //if (!string.IsNullOrEmpty(yctNumber.ReturnValue))
            //{
            //    if (yctNumber.ReturnValue.StartsWith("/"))
            //    {
            //        _result = _result.Where(po => yctNumber.ReturnValue.Substring(yctNumber.ReturnValue.IndexOf('/') + 1) == po.Number.Substring(po.Number.IndexOf('/') + 1));
            //    }
            //    else if (yctNumber.ReturnValue.Length > 4)
            //    {
            //        _result = _result.Where(po => po.Number == yctNumber.ReturnValue);
            //    }
            //    else
            //    {
            //        // filtro anno specifico attivo
            //        _result = _result.Where(po => po.StartDate.GetValueOrDefault().Year.ToString() == yctNumber.ReturnValue);
            //    }
            //}


            if (!string.IsNullOrEmpty(yctNumber.ReturnValue))
            {
                if (yctNumber.ReturnValue.StartsWith("/"))
                {
                    _result = _result.Where(po => yctNumber.ReturnValue.Substring(yctNumber.ReturnValue.IndexOf('/') + 1) == po.Number.Substring(po.Number.IndexOf('/') + 1));
                }
                else if (yctNumber.ReturnValue.Length > 4)
                {
                    _result = _result.Where(po => po.Number.Substring(2) == yctNumber.ReturnValue);
                }
                else
                {
                    _result = _result.Where(po => po.Number.Substring(2, 2) == yctNumber.ReturnValue.Substring(2));
                }
            }

            if (!string.IsNullOrEmpty(itbNo.Text))
            {
                var _temp = 0;
                int.TryParse(itbNo.Text, out _temp);
                _result = _result.Where(po => po.ID_Quotation == _temp);
            }
            if (!string.IsNullOrEmpty(txtTitleContains.Text))
            {
                _result = _result.Where(po => po.QUOSubject.ToLowerInvariant().Contains(txtTitleContains.Text.ToLowerInvariant()));
            }
            //if (!string.IsNullOrEmpty(txtCustomerContains.Text))
            //{
            //    _result = _result.Where(po => po.CustomerName.Contains(txtCustomerContains.Text));
            //}
            //if (!string.IsNullOrEmpty(ddlCustomers.SelectedValue))
            //{
            //    int _temp = 0;
            //    int.TryParse(ddlCustomers.SelectedValue, out _temp);
            //    _result = _result.Where(po => po.PORID_Customer == _temp);
            //}
            if (!string.IsNullOrEmpty(hidCustomer.Value))
            {
                int _temp = 0;
                int.TryParse(hidCustomer.Value, out _temp);
                _result = _result.Where(po => po.PORID_Customer == _temp);
            }
            if (!string.IsNullOrEmpty(ddlOwners.SelectedValue))
            {
                int _temp = 0;
                int.TryParse(ddlOwners.SelectedValue, out _temp);
                _result = _result.Where(po => po.QUOOwner == _temp);
            }
            if (!string.IsNullOrEmpty(ddlOperators.SelectedValue))
            {
                int _temp = 0;
                int.TryParse(ddlOperators.SelectedValue, out _temp);
                int[] lavoratiOperatore = _qc.ProductionOrderDetails.Where(d => d.ID_Owner == _temp).Select(d => d.ID_ProductionOrder).Distinct().ToArray();
                _result = _result.Where(po => lavoratiOperatore.Contains(po.ID));
            }

            //if (ddlLossMaking.SelectedValue == "1")
            //    _result = _result.Where(po =>
            //        po.FATTotValue.GetValueOrDefault(0m) > 0m &&
            //        (po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
            //         -
            //         (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
            //             ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
            //             : po.PORTotHistoricalCost.GetValueOrDefault(0m)))
            //        < 0m);

            //if (ddlLossMaking.SelectedValue == "2")
            //    _result = _result.Where(po =>
            //        po.FATTotValue.GetValueOrDefault(0m) > 0m &&
            //        (po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
            //         -
            //         (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
            //             ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
            //             : po.PORTotHistoricalCost.GetValueOrDefault(0m)))
            //        >= 0m);

            //if (ddlLossMaking.SelectedValue == "3")
            //    _result = _result.Where(po =>
            //        po.FATTotValue.GetValueOrDefault(0m) > 0m &&
            //        ((po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
            //         -
            //         (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
            //             ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
            //             : po.PORTotHistoricalCost.GetValueOrDefault(0m))))
            //             /
            //              (po.FATTotValue.GetValueOrDefault(0m))
            //        >= 0.3m);

            //if (ddlLossMaking.SelectedValue == "4")
            //    _result = _result.Where(po =>
            //        po.FATTotValue.GetValueOrDefault(0m) > 0m &&
            //        ((po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
            //         -
            //         (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
            //             ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
            //             : po.PORTotHistoricalCost.GetValueOrDefault(0m))))
            //             /
            //              (po.FATTotValue.GetValueOrDefault(0m))
            //        >= 0.7m);


            if (ddlLossMaking.SelectedValue == "1")
                _result = _result.Where(po =>
                    po.FATTotValue.HasValue &&
                    (po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
                     -
                     (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                         ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
                         : po.PORTotHistoricalCost.GetValueOrDefault(0m)))
                    < 0m);

            if (ddlLossMaking.SelectedValue == "2")
                _result = _result.Where(po =>
                    po.FATTotValue.HasValue &&
                    (po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
                     -
                     (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                         ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
                         : po.PORTotHistoricalCost.GetValueOrDefault(0m)))
                    >= 0m);

            if (ddlLossMaking.SelectedValue == "3")
                _result = _result.Where(po =>
                    po.FATTotValue.GetValueOrDefault(0m) != 0m &&
                    ((po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
                     -
                     (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                         ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
                         : po.PORTotHistoricalCost.GetValueOrDefault(0m))))
                         /
                          (Math.Abs(po.FATTotValue.GetValueOrDefault(0m)))
                    >= 0.3m);

            if (ddlLossMaking.SelectedValue == "4")
                _result = _result.Where(po =>
                    po.FATTotValue.GetValueOrDefault(0m) != 0m &&
                    ((po.FATTotValue.GetValueOrDefault(0m) - po.ProvvTotValue.GetValueOrDefault(0m)
                     -
                     (po.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                         ? Convert.ToDecimal(po.PORTotCost.GetValueOrDefault(0d))
                         : po.PORTotHistoricalCost.GetValueOrDefault(0m))))
                         /
                          (Math.Abs(po.FATTotValue.GetValueOrDefault(0m)))
                    >= 0.7m);




            if (ddlClosed.SelectedValue == "1")
                _result = _result.Where(po => po.Status == 3);

            if (ddlClosed.SelectedValue == "2")
                _result = _result.Where(po => po.Status != 3);

            if (dlBollettato.SelectedValue == "1")
            {
                //Bollettato
                _result = _result.Where(po => po.DataBolla != null);
            }
            if (dlBollettato.SelectedValue == "2")
            {
                //Non bollettato
                _result = _result.Where(po => po.DataBolla == null);
            }

            if (dlFatturato.SelectedValue == "1")
            {
                //Fatturato
                _result = _result.Where(po => po.FATTotValue != null);
            }
            if (dlFatturato.SelectedValue == "2")
            {
                //Non fatturato
                _result = _result.Where(po => po.FATTotValue == null);

            }

            switch (ddlOrderBy.SelectedValue)
            {

                case ("ID"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderByDescending(qt => qt.ID);
                    break;
                case ("Number"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderByDescending(qt => qt.Number);
                    break;
                case ("DataBolla"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderByDescending(qt => qt.DataBolla);
                    break;
                case ("OwnerUniqueName"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderBy(qt => qt.QUOOwner);
                    break;
                case ("CustomerName"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderBy(qt => qt.CustomerName);
                    break;
                case ("Margin"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderBy(qt =>
                        qt.FATTotValue.GetValueOrDefault(0m) != 0m
                            ? (qt.FATTotValue
                               -
                               (qt.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                                   ? Convert.ToDecimal(qt.PORTotCost.GetValueOrDefault(0d))
                                   : qt.PORTotHistoricalCost.GetValueOrDefault(0m))
                               - (qt.ProvvTotValue ?? 0m)) / qt.FATTotValue
                            : 0m);
                    break;
                case ("Margin DESC"):
                    ldsProductionOrdersStats.OrderByParameters.Clear();
                    ldsProductionOrdersStats.AutoGenerateOrderByClause = false;
                    e.Result = _result.OrderByDescending(qt =>
                        qt.FATTotValue.GetValueOrDefault(0m) != 0m
                            ? (qt.FATTotValue
                               -
                               (qt.PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                                   ? Convert.ToDecimal(qt.PORTotCost.GetValueOrDefault(0d))
                                   : qt.PORTotHistoricalCost.GetValueOrDefault(0m))
                               - (qt.ProvvTotValue ?? 0m)) / qt.FATTotValue
                            : 0m);
                    break;

                default:
                    e.Result = _result.OrderByDescending(qt => qt.ID);
                    break;
            }
        }

        protected void grdProductionOrders_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["ProductionOrdersStatsTypesSelector"] = ddlTypes.SelectedValue;
            Session["ProductionOrdersStatsStatusesSelector"] = ddlOwners.SelectedValue;
            //Session["ProductionOrdersStatsSuppliersSelector"] = ddlCustomers.SelectedValue;
            Session["ProductionOrdersStatsSuppliersSelector"] = hidCustomer.Value;
            Session["ProductionOrdersStatsOrderBySelector"] = ddlOrderBy.SelectedValue;
            Session["Agente1OrderBySelector"] = ddlAgente1.SelectedValue;
            //Session["Agente2OrderBySelector"] = ddlAgente1.SelectedValue;
            Session["NonConformitySelector"] = ddlNonConformities.SelectedValue;
            Session["ComplaintReceivedSelector"] = ddlComplaintReceived.SelectedValue;
            Session["CorrectiveActionSelector"] = ddlCorrectiveActions.SelectedValue;
            Session["ProductionOrdersStatsManagersSelector"] = ddlManagers.SelectedValue;
            Session["ProductionOrdersStatsOperatorsSelector"] = ddlOperators.SelectedValue;

        }

        protected void grdProductionOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                grdProductionOrdersStats.DataBind();
            }
            if (e.CommandName == "GoToQuotation")
            {
                LoadPersistedQuotation = true;
                Response.Redirect(QuotationConsolePage + "?" + QuotationKey + "=" + e.CommandArgument + "&Usage=0", true);
            }
            if (e.CommandName == "Close")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _toDeleteProductionOrder =
                        _qc.ProductionOrders.Single(po => po.ID == Convert.ToInt32(e.CommandArgument));
                    _toDeleteProductionOrder.Status = 3;
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.ID);
                    _qc.SubmitChanges();
                }
                grdProductionOrdersStats.DataBind();
            }

            //if (e.CommandName == "Save")
            //{
            //    var row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            //    using (var _qc = new QuotationDataContext())
            //    {
            //        var _toSaveProductionOrder =
            //            _qc.ProductionOrders.Single(po => po.ID == Convert.ToInt32(e.CommandArgument));
            //        _toSaveProductionOrder.AccountNote =
            //            ((TextBox)((((DynamicControl)row.FindControl("dycAccountNote"))).FieldTemplate).Controls[0])
            //                .Text;
            //        _qc.SubmitChanges();
            //    }
            //    grdProductionOrdersStats.DataBind();
            //}
        }

        protected void lbtExportToExcel_Click(object sender, EventArgs e)
        {
            isExporting = true;
            grdProductionOrdersStats.PagerSettings.Visible = false;
            grdProductionOrdersStats.DataBind();
            ExportToExcel(GridRender);
            isExporting = false;
            grdProductionOrdersStats.PagerSettings.Visible = true;

        }



        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }

        protected void ddlAgente1_Selectiong(object sender, LinqDataSourceSelectEventArgs e)
        {
            var db = new QuotationDataContext();

            var countries = (from c in db.Customers
                             where c.IDAgente1 != 0
                             select new { c.DescrizioneAgente1, c.IDAgente1 }).Distinct();
            e.Result = countries;

        }


        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string WriteNoteToOdP(string pipedParams)
        {

            int idOdp = 0;
            string note = string.Empty;

            try
            {
                string[] parameters = pipedParams.Split('|');
                idOdp = Convert.ToInt32(parameters[0]);
                note = parameters[1]; //.Substring(0, parameters[1].IndexOf('('));

                using (QuotationDataContext _qc = new QuotationDataContext())
                {
                    ProductionOrder _toSaveProductionOrder = _qc.ProductionOrders.Single(po => po.ID == idOdp);
                    _toSaveProductionOrder.AccountNote = note;
                    QUOPORCostsPrice _toSaveQUOPORCostsPrice = _qc.QUOPORCostsPrices.FirstOrDefault(po => po.ID == idOdp);
                    if (_toSaveQUOPORCostsPrice != null)
                    {
                        _toSaveQUOPORCostsPrice.AccountNote = note;
                    }
                    _qc.SubmitChanges();
                }
                return "";
            }
            catch
            {
                return string.Format("Errore di scrittura sull'OdP {0}, ritentare", idOdp);
            }

        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string WriteNonConformityCodeToOdP(string pipedParams)
        {

            int idOdp = 0;
            string code = null;

            try
            {
                string[] parameters = pipedParams.Split('|');
                idOdp = Convert.ToInt32(parameters[0]);
                code = parameters[1];

                using (QuotationDataContext _qc = new QuotationDataContext())
                {
                    ProductionOrder _toSaveProductionOrder = _qc.ProductionOrders.Single(po => po.ID == idOdp);
                    if (string.IsNullOrEmpty(code)) { _toSaveProductionOrder.NonConformityCode = null; } else { _toSaveProductionOrder.NonConformityCode = Convert.ToInt32(code); };
                    QUOPORCostsPrice _toSaveQUOPORCostsPrice = _qc.QUOPORCostsPrices.FirstOrDefault(po => po.ID == idOdp);
                    if (_toSaveQUOPORCostsPrice != null)
                    {
                        if (string.IsNullOrEmpty(code)) { _toSaveQUOPORCostsPrice.NonConformityCode = null; } else { _toSaveQUOPORCostsPrice.NonConformityCode = Convert.ToInt32(code); };
                    }
                    _qc.SubmitChanges();
                }
                return "";
            }
            catch
            {
                return string.Format("Errore di scrittura sull'OdP {0}, ritentare", idOdp);
            }

        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string WriteComplaintReceivedToOdP(string pipedParams)
        {

            int idOdp = 0;
            string code = null;

            try
            {
                string[] parameters = pipedParams.Split('|');
                idOdp = Convert.ToInt32(parameters[0]);
                code = parameters[1];

                using (QuotationDataContext _qc = new QuotationDataContext())
                {
                    ProductionOrder _toSaveProductionOrder = _qc.ProductionOrders.Single(po => po.ID == idOdp);
                    if (string.IsNullOrEmpty(code)) { _toSaveProductionOrder = null; } else { _toSaveProductionOrder.ComplaintReceived = Convert.ToInt32(code); };
                    QUOPORCostsPrice _toSaveQUOPORCostsPrice = _qc.QUOPORCostsPrices.FirstOrDefault(po => po.ID == idOdp);
                    if (_toSaveQUOPORCostsPrice != null)
                    {
                        if (string.IsNullOrEmpty(code)) { _toSaveQUOPORCostsPrice.ComplaintReceived = null; } else { _toSaveQUOPORCostsPrice.ComplaintReceived = Convert.ToInt32(code); };
                    }
                    _qc.SubmitChanges();
                }
                return "";
            }
            catch
            {
                return string.Format("Errore di scrittura sull'OdP {0}, ritentare", idOdp);
            }

        }


        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string WriteCorrectiveActionCodeToOdP(string pipedParams)
        {

            int idOdp = 0;
            string code = null;

            try
            {
                string[] parameters = pipedParams.Split('|');
                idOdp = Convert.ToInt32(parameters[0]);
                code = parameters[1];

                using (QuotationDataContext _qc = new QuotationDataContext())
                {
                    ProductionOrder _toSaveProductionOrder = _qc.ProductionOrders.Single(po => po.ID == idOdp);
                    if (string.IsNullOrEmpty(code)) { _toSaveProductionOrder = null; } else { _toSaveProductionOrder.CorrectiveActionCode = Convert.ToInt32(code); };
                    QUOPORCostsPrice _toSaveQUOPORCostsPrice = _qc.QUOPORCostsPrices.FirstOrDefault(po => po.ID == idOdp);
                    if (_toSaveQUOPORCostsPrice != null)
                    {
                        if (string.IsNullOrEmpty(code)) { _toSaveQUOPORCostsPrice.CorrectiveActionCode = null; } else { _toSaveQUOPORCostsPrice.CorrectiveActionCode = Convert.ToInt32(code); };
                    }
                    _qc.SubmitChanges();
                }
                return "";
            }
            catch
            {
                return string.Format("Errore di scrittura sull'OdP {0}, ritentare", idOdp);
            }

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




    }


}