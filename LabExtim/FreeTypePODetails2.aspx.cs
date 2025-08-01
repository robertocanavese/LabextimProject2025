using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;
using CustomExtensions;

namespace LabExtim
{
    public partial class FreeTypePODetails2 : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
        }

        public void SetFilter()
        {
            using (var _qc = new QuotationDataContext())
            {
                var _filtered = false;

                IEnumerable<VW_FreeTypePOD> _resultPOD = _qc.VW_FreeTypePOD;
                //IEnumerable<VW_FreeTypeDDT> _resultDDT = _qc.VW_FreeTypeDDT;

                if (!string.IsNullOrEmpty(senMain.ItbNo.Text))
                {
                    var _temp = 0;
                    int.TryParse(senMain.ItbNo.ReturnValue.ToString(), out _temp);
                    _resultPOD = _resultPOD.Where(po => po.ID == _temp);
                    //_resultDDT = _resultDDT.Where(ddt => ddt.mm_lotto == _temp);
                    _filtered = true;
                }

                if (!string.IsNullOrEmpty(senMain.YctNumber.ReturnValue))
                {
                    if (senMain.YctNumber.ReturnValue.StartsWith("/"))
                    {
                        _resultPOD = _resultPOD.Where(po => senMain.YctNumber.ReturnValue.Substring(senMain.YctNumber.ReturnValue.IndexOf('/') + 1) == po.Number.Substring(po.Number.IndexOf('/') + 1));
                    }
                    else if (senMain.YctNumber.ReturnValue.Length > 4)
                    {
                        _resultPOD = _resultPOD.Where(po => po.Number.Substring(2) == senMain.YctNumber.ReturnValue);
                    }
                    else
                    {
                        _resultPOD = _resultPOD.Where(po => po.Number.Substring(2, 2) == senMain.YctNumber.ReturnValue.Substring(2));
                    }
                }

                if (!string.IsNullOrEmpty(senMain.TextField2Text))
                {
                    _resultPOD = _resultPOD.Where(po => po.FreeItemDescription.ContainsCaseInsensitive(senMain.TextField2Text.ToString()));
                    //_resultDDT = _resultDDT.Where(ddt => ddt.mm_descr.ContainsCaseInsensitive(senMain.TextField2Text.ToString()));
                    _filtered = true;
                }

                if (!string.IsNullOrEmpty(senMain.TextDateFromText))
                {
                    //data di produzione odp
                    _resultPOD = _resultPOD.Where(po => po.ProductionDate >= DateTime.Parse(senMain.TextDateFromText));
                    //_resultDDT = _resultDDT.Where(ddt => ddt.DataDDT >= DateTime.Parse(senMain.TextDateFromText));
                    _filtered = true;
                }

                if (!string.IsNullOrEmpty(senMain.TextDateToText))
                {
                    //data di produzione odp
                    _resultPOD = _resultPOD.Where(po => po.ProductionDate <= DateTime.Parse(senMain.TextDateToText));
                    //_resultDDT = _resultDDT.Where(ddt => ddt.DataDDT <= DateTime.Parse(senMain.TextDateFromText));
                    _filtered = true;
                }

                if (!string.IsNullOrEmpty(senMain.DropDownList1.SelectedValue))
                {
                    //cliente
                    _resultPOD = _resultPOD.Where(po => po.ID_Customer == Convert.ToInt32(senMain.DropDownList1.SelectedValue));
                    //_resultDDT = _resultDDT.Where(po => po.CodCliente == Convert.ToInt32(senMain.DropDownList1.SelectedValue));
                    _filtered = true;
                    //
                }
                if (!string.IsNullOrEmpty(senMain.DropDownList2.SelectedValue))
                {
                    //fornitore
                    _resultPOD = _resultPOD.Where(po => po.SupplierCode == Convert.ToInt32(senMain.DropDownList2.SelectedValue));
                    //_resultDDT = _resultDDT.Where(po => _resultPOD.Select(pdo => pdo.ID).Distinct().ToArray().Contains(po.mm_lotto)).ToList();
                    _filtered = true;
                }
                if (!_filtered)
                {
                    _resultPOD = _resultPOD.Where(po => po.ProductionDate >= DateTime.Today.AddMonths(-3));
                    //_resultDDT = _resultDDT.Where(ddt => ddt.DataDDT >= DateTime.Today.AddMonths(-3));
                }

                BindGrdProductionOrders(_resultPOD);
                //BindGrdDDTs(_resultDDT);
            }
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
            //grdDDTs.PageIndex = 0;
            SetFilter();
        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
            //grdDDTs.PageIndex = 0;
            SetFilter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillControls();
                PopulateSearchEngine();
                SetFilter();
            }
        }

        private void PopulateSearchEngine()
        {
            senMain.LblYearCounterText = "Anno/Numero";
            //senMain.LblTextField1Text = "Numero DDT contiene...";
            senMain.LblTextField2Text = "Descrizione contiene...";
            senMain.LblDateFromText = "Data produzione da";
            senMain.LblDateToText = "Data produzione a";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Cliente";

                if (CurrentCompanyId == 1)
                {
                    var _customerItems = _qc.Customers.Where(c => !c.Name.StartsWith("**") && c.Code >= 1 && c.Code <= 199999999).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                    senMain.DropDownList1.Items.AddRange(_customerItems);
                }
                if (CurrentCompanyId == 2)
                {
                    var _customerItems = _qc.Customers.Where(c => !c.Name.StartsWith("**") && c.Code >= 200000000 && c.Code <= 299999999).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                    senMain.DropDownList1.Items.AddRange(_customerItems);
                }
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));
            }

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList2Text = "Fornitore";

                if (CurrentCompanyId == 1)
                {
                    var _supplierItems = _qc.Suppliers.Where(c => !c.Name.StartsWith("**") && c.Code >= 1 && c.Code <= 199999999).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                    senMain.DropDownList2.Items.AddRange(_supplierItems);
                }
                if (CurrentCompanyId == 2)
                {
                    var _supplierItems = _qc.Suppliers.Where(c => !c.Name.StartsWith("**") && c.Code >= 200000000 && c.Code <= 299999999).Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                    senMain.DropDownList2.Items.AddRange(_supplierItems);
                }

                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Personalizzato", "Custom"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Più recenti", "ProductionDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Fornitore", "SupplierName"));
        }

        private void FillControls()
        {
        }

        protected void grdProductionOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrders.PageIndex = e.NewPageIndex;
            //grdProductionOrders.DataBind();
            SetFilter();
        }

        //protected void grdDDTs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdDDTs.PageIndex = e.NewPageIndex;
        //    //grdDDTs.DataBind();
        //    SetFilter();
        //}

        protected void lbtPrintProductionOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}", GenericReportKey, "VW_FreeTypeProductionOrderDetails", GenericPrintPage),
                true);
        }

        protected void grdProductionOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";


                //HyperLink _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                //_hypEdit.Attributes.Add("onclick", "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" + ((ProductionOrder)e.Row.DataItem).ID + "')");
            }
        }

        //protected void grdDDTs_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
        //        e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";


        //        //HyperLink _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
        //        //_hypEdit.Attributes.Add("onclick", "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" + ((ProductionOrder)e.Row.DataItem).ID + "')");
        //    }
        //}

        protected void BindGrdProductionOrders(IEnumerable<VW_FreeTypePOD> _result)
        {
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                case ("Custom"):

                    _result = _result
                        .OrderBy(qt => qt.ID)
                        .ThenBy(qt => qt.SupplierName)
                        .ThenBy(qt => qt.ProductionDate)
                        .ThenBy(qt => qt.CustomerName)
                        .ThenBy(qt => qt.UMRawMaterial)
                        .ThenBy(qt => qt.RawMaterialQuantity)
                        .ThenBy(qt => qt.Cost).ToList()
                        ;
                    break;
                case ("CustomerName"):

                    _result = _result.OrderBy(qt => qt.CustomerName).ToList();
                    break;
                case ("SupplierName"):

                    _result = _result.OrderBy(qt => qt.SupplierName).ToList();
                    break;
                case ("ProductionDate"):

                    _result =
                        _result.OrderByDescending(qt => qt.ProductionDate).ThenByDescending(qt => qt.ID).ToList();
                    break;
                default:
                    break;
            }
            grdProductionOrders.DataSource = _result;
            grdProductionOrders.Columns[7].FooterText = _result.Sum(gd => gd.Cost).Value.ToString("N2");
            grdProductionOrders.DataBind();

        }

        //protected void BindGrdDDTs(IEnumerable<VW_FreeTypeDDT>_result)
        //{
        //    switch (senMain.DdlOrderBy.SelectedValue)
        //    {
        //        case ("Custom"):
        //            _result // _qc.VW_FreeTypeDDT
        //                .OrderBy(qt => qt.mm_lotto)
        //                .ThenBy(qt => qt.NumDDT)
        //                .ThenBy(qt => qt.mm_descr)
        //                .ThenBy(qt => qt.mm_quant)
        //                .ThenBy(qt => qt.mm_prezzo)
        //                .ThenBy(qt => qt.mm_valore).ToList()
        //                ;
        //            break;
        //        case ("ProductionDate"):
        //            _result =
        //                _result.OrderByDescending(qt => qt.DataDDT).ThenByDescending(qt => qt.NumDDT).ToList();
        //            break;
        //        default:
        //            break;
        //    }
        //    grdDDTs.DataSource = _result;
        //    grdDDTs.Columns[6].FooterText = _result.Sum(gd => gd.mm_valore).GetValueOrDefault(0m).ToString("N2");
        //    grdDDTs.DataBind();

        //}

        protected void grdProductionOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                //grdProductionOrders.DataBind();
            }


            if (e.CommandName == "GoToQuotation")
            {
                //int _destinationId = Convert.ToInt32(e.CommandArgument.ToString() == string.Empty ? 0.ToString() : e.CommandArgument.ToString());
                //using (QuotationDataContext _qc = new QuotationDataContext())
                //{
                //    try
                //    {
                //        List<SPQDetailTotal> m_totals = _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(_destinationId)).ToList<SPQDetailTotal>();
                //        Response.Redirect(string.Format("{2}?{0}={1}", QuotationKey, _destinationId, QuotationConsolePage), true);
                //    }
                //    catch
                //    {
                //        lblSuccess.Text = "Il preventivo No " + _destinationId + " è inutilizzabile: se ne consiglia l'eliminazione.";
                //    }

                //}
            }
        }

        protected void grdProductionOrders_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdProductionOrders.EditIndex = e.NewEditIndex;
            SetFilter();
        }

        protected void grdProductionOrders_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            HiddenField hid_ID_ProductionOrderDetail = grdProductionOrders.Rows[e.RowIndex].FindControl("hid_ID_ProductionOrderDetail") as HiddenField;
            TextBox txt_RawMaterialQuantity = grdProductionOrders.Rows[e.RowIndex].FindControl("txt_RawMaterialQuantity") as TextBox;
            TextBox txt_Cost = grdProductionOrders.Rows[e.RowIndex].FindControl("txt_Cost") as TextBox;

            using (var db = new QuotationDataContext())
            {
                ProductionOrderDetail pod = db.ProductionOrderDetails.FirstOrDefault(d => d.ID == Convert.ToInt32(hid_ID_ProductionOrderDetail.Value));
                float rawMaterialQuantity = 0f;
                decimal cost = 0m;

                if (float.TryParse(txt_RawMaterialQuantity.Text, out rawMaterialQuantity))
                    pod.RawMaterialQuantity = rawMaterialQuantity;
                if (decimal.TryParse(txt_Cost.Text, out cost))
                    pod.Cost = cost;

                db.SubmitChanges();
            }

            grdProductionOrders.EditIndex = -1;
            SetFilter();
        }

        protected void grdProductionOrders_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdProductionOrders.EditIndex = -1;
            SetFilter();
        }

        //protected void grdDDTs_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Reload")
        //    {
        //        //grdProductionOrders.DataBind();
        //    }


        //    if (e.CommandName == "GoToQuotation")
        //    {
        //        //int _destinationId = Convert.ToInt32(e.CommandArgument.ToString() == string.Empty ? 0.ToString() : e.CommandArgument.ToString());
        //        //using (QuotationDataContext _qc = new QuotationDataContext())
        //        //{
        //        //    try
        //        //    {
        //        //        List<SPQDetailTotal> m_totals = _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(_destinationId)).ToList<SPQDetailTotal>();
        //        //        Response.Redirect(string.Format("{2}?{0}={1}", QuotationKey, _destinationId, QuotationConsolePage), true);
        //        //    }
        //        //    catch
        //        //    {
        //        //        lblSuccess.Text = "Il preventivo No " + _destinationId + " è inutilizzabile: se ne consiglia l'eliminazione.";
        //        //    }

        //        //}
        //    }
        //}
    }


}