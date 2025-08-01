using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using CMLabExtim;
using DLLabExtim;
using LabExtim.CustomControls;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrderDetailsInsertByD : ProductionOrderDetailsInsertController
    {
        public DateTime CurProductionDate
        {
            get
            {
                if (ViewState["CurProductionDate"] != null)
                    return Convert.ToDateTime(ViewState["CurProductionDate"]);
                return DateTime.MinValue;
            }
            set { ViewState["CurProductionDate"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //if (CurrentCompanyId == 1)
            //    ldsSupplier.Where = "code >= 1 && code <= 199999999";
            //if (CurrentCompanyId == 2)
            //    ldsSupplier.Where = "code >= 200000000 && code <= 299999999";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurProductionDate = Utilities.GetNextWorkDate(-1, DateTime.Now.Date);
                txtDate.Text = CurProductionDate.ToString("dd/MM/yyyy");
                TempProductionOrderDetails = null;
                BindData();
            }
            ReBindData();
        }

        private void BindData()
        {
            DateTime _tempDate;
            lbtNewItem.Enabled = DateTime.TryParse(txtDate.Text, out _tempDate);
            GetGridData();
            var _temp = new List<TempProductionOrderDetail>();

            //_temp.AddRange(GetProductionOrderDetailsOfADate(_tempDate, CurrentCompanyId));
            //merge aziendale
            _temp.AddRange(GetProductionOrderDetailsOfADate(_tempDate, -1));

            TempProductionOrderDetails = _temp;
            grdProductionOrderDetails.DataSource = TempProductionOrderDetails;
            grdProductionOrderDetails.DataBind();
        }

        private void ReBindData()
        {
            GetGridData();
            grdProductionOrderDetails.DataSource = TempProductionOrderDetails;
            grdProductionOrderDetails.DataBind();
        }

        private void GetGridData()
        {
            for (var i = 0; i < grdProductionOrderDetails.Rows.Count; i++)
            {
                try
                {
                    var p = 2;

                    var _txtID_ProductionOrder =
                        (IntTextBox)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("txtID_ProductionOrder");
                    if (_txtID_ProductionOrder.Text != string.Empty)
                        TempProductionOrderDetails[i].ID_ProductionOrder = Convert.ToInt32(_txtID_ProductionOrder.Text);
                    p += 1;

                    var _ddlType =
                        (DropDownList)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("ddlType");
                    if (_ddlType.SelectedValue != string.Empty)
                        TempProductionOrderDetails[i].FreeTypeCode = Convert.ToInt32(_ddlType.SelectedValue);
                    p += 1;

                    var _ddlItemType =
                        (DropDownList)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("ddlItemType");
                    if (_ddlItemType.SelectedValue != string.Empty)
                        TempProductionOrderDetails[i].FreeItemTypeCode = Convert.ToInt32(_ddlItemType.SelectedValue);
                    p += 1;

                    var _txtDescription =
                        (TextBox)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("txtDescription");
                    if (_txtDescription.Text != string.Empty)
                        TempProductionOrderDetails[i].FreeItemDescription = _txtDescription.Text;

                    var _ddlUMRawMaterial =
                        (DropDownList)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("ddlUMRawMaterial");
                    TempProductionOrderDetails[i].UMRawMaterial = Convert.ToInt32(_ddlUMRawMaterial.SelectedValue);
                    p += 1;

                    var _txtRawMaterialQuantity =
                        (FloatTextBox)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("txtRawMaterialQuantity");
                    if (_txtRawMaterialQuantity.Text != string.Empty)
                        TempProductionOrderDetails[i].RawMaterialQuantity =
                            Convert.ToSingle(_txtRawMaterialQuantity.Text);
                    p += 1;

                    var _txtUnitCost =
                        (FloatTextBox)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("txtUnitCost");
                    if (_txtUnitCost.Text != string.Empty && _txtRawMaterialQuantity.Text != string.Empty)
                        TempProductionOrderDetails[i].Cost = Convert.ToDecimal(_txtUnitCost.Text) *
                                                             Convert.ToDecimal(_txtRawMaterialQuantity.Text);
                    else
                        TempProductionOrderDetails[i].Cost = 0m;
                    p += 1;

                    var _ddlSupplier =
                        (DropDownList)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("ddlSupplier");
                    if (_ddlSupplier.SelectedValue != string.Empty)
                        TempProductionOrderDetails[i].SupplierCode = Convert.ToInt32(_ddlSupplier.SelectedValue);
                    p += 1;

                    var _txtNote = (TextBox)grdProductionOrderDetails.Rows[i].Cells[p].FindControl("txtNote");
                    if (_txtNote.Text != string.Empty)
                        TempProductionOrderDetails[i].Note = _txtNote.Text;
                }
                catch
                {
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

        protected void grdProductionOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderDetails.PageIndex = e.NewPageIndex;
        }

        protected void lbtNewItem_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < 5; i++)
                InsertRow(CurProductionDate, CurrentCompanyId, null);
            grdProductionOrderDetails.DataBind();
            grdProductionOrderDetails.SelectedIndex = grdProductionOrderDetails.Rows.Count - 5;
        }

        protected void ldsOfaDropDownList_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            if (((LinqDataSourceView)sender).TableName == "Suppliers")
            {
                using (var _ctx = new QuotationDataContext())
                {
                    int min = 0;
                    int max = 0;
                    if (CurrentCompanyId == 1)
                    {
                        min = 1;
                        max = 199999999;
                    }
                    if (CurrentCompanyId == 2)
                    {
                        min = 200000000;
                        max = 299999999;
                    }

                    e.Result = _ctx.Suppliers.Where(p => p.Code >= min && p.Code <= max).OrderBy(p => p.Name).ToArray();
                }
            }

        }

        protected void ldsOfaDropDownList_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            if (((LinqDataSourceView)sender).TableName == ldsSupplier.TableName)
            {
                var _dummy = new Supplier();
                _dummy.Code = 0;
                _dummy.Name = "N/DEF";
                ((IList)e.Result).Insert(0, _dummy);
            }
        }

        protected void grdProductionOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
            }
        }

        protected void grdProductionOrderDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DeleteRow(e.RowIndex);
            grdProductionOrderDetails.DataBind();
            grdProductionOrderDetails.SelectedIndex = grdProductionOrderDetails.Rows.Count - 1;
        }

        protected void lbtSubmit_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < grdProductionOrderDetails.Rows.Count; i++)
            {
                try
                {
                    //SubmitRow(i, txtDate.Text, null);
                    SubmitFreeTypeRow(i, txtDate.Text, CurrentCompanyId);
                }
                catch
                {
                    var _errorControl =
                        (IntTextBox)grdProductionOrderDetails.Rows[i].Cells[3].FindControl("txtID_ProductionOrder");
                    _errorControl.ForeColor = Color.Red;
                    _errorControl.BackColor = Color.Yellow;
                }
            }
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void grdProductionOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    var p = 1;
                    var _lblState = (Label)e.Row.Cells[p].FindControl("lblState");
                    _lblState.Text = ((TempProductionOrderDetail)e.Row.DataItem).State ==
                                     TempProductionOrderDetail.ItemState.Error
                        ? "???"
                        : "OK";
                    p += 1;

                    var _txtID_ProductionOrder = (IntTextBox)e.Row.Cells[p].FindControl("txtID_ProductionOrder");
                    _txtID_ProductionOrder.Text =
                        ((TempProductionOrderDetail)e.Row.DataItem).ID_ProductionOrder.ToString();

                    var _found = false;
                    _txtID_ProductionOrder.ToolTip = GetProductionOrderToolTip(_txtID_ProductionOrder.Text, out _found);
                    p += 1;

                    var _ddlType = (DropDownList)e.Row.Cells[p].FindControl("ddlType");
                    if (((TempProductionOrderDetail)e.Row.DataItem).FreeTypeCode != null)
                        _ddlType.SelectedValue = ((TempProductionOrderDetail)e.Row.DataItem).FreeTypeCode.ToString();
                    p += 1;

                    var _ddlItemType = (DropDownList)e.Row.Cells[p].FindControl("ddlItemType");
                    if (((TempProductionOrderDetail)e.Row.DataItem).FreeItemTypeCode != null)
                        _ddlItemType.SelectedValue =
                            ((TempProductionOrderDetail)e.Row.DataItem).FreeItemTypeCode.ToString();
                    p += 1;

                    var _txtDescription = (TextBox)e.Row.Cells[p].FindControl("txtDescription");
                    if (((TempProductionOrderDetail)e.Row.DataItem).FreeItemDescription != null)
                        _txtDescription.Text = ((TempProductionOrderDetail)e.Row.DataItem).FreeItemDescription;

                    var _ddlUMRawMaterial = (DropDownList)e.Row.Cells[p].FindControl("ddlUMRawMaterial");
                    if (((TempProductionOrderDetail)e.Row.DataItem).UMRawMaterial != null)
                        _ddlUMRawMaterial.SelectedValue =
                            ((TempProductionOrderDetail)e.Row.DataItem).UMRawMaterial.ToString();
                    p += 1;

                    var _txtRawMaterialQuantity =
                        (FloatTextBox)e.Row.Cells[p].FindControl("txtRawMaterialQuantity");
                    _txtRawMaterialQuantity.Text =
                        ((TempProductionOrderDetail)e.Row.DataItem).RawMaterialQuantity.ToString();
                    p += 1;

                    var _txtUnitCost = (FloatTextBox)e.Row.Cells[p].FindControl("txtUnitCost");
                    if (((TempProductionOrderDetail)e.Row.DataItem).Cost != null)
                        _txtUnitCost.Text =
                            (((TempProductionOrderDetail)e.Row.DataItem).Cost /
                             (
                                 Convert.ToDecimal(((TempProductionOrderDetail)e.Row.DataItem).RawMaterialQuantity) !=
                                 0
                                     ? Convert.ToDecimal(
                                         ((TempProductionOrderDetail)e.Row.DataItem).RawMaterialQuantity)
                                     : 1))
                                .ToString();
                    p += 1;

                    var _ddlSupplier = (DropDownList)e.Row.Cells[p].FindControl("ddlSupplier");
                    if (((TempProductionOrderDetail)e.Row.DataItem).SupplierCode != null)
                        _ddlSupplier.SelectedValue =
                            ((TempProductionOrderDetail)e.Row.DataItem).SupplierCode.ToString();
                    p += 1;

                    var _txtNote = (TextBox)e.Row.Cells[p].FindControl("txtNote");
                    if (((TempProductionOrderDetail)e.Row.DataItem).Note != null)
                        _txtNote.Text = ((TempProductionOrderDetail)e.Row.DataItem).Note;
                    //EnableDependentControls(e, 10);
                }
                catch
                {
                }
            }
        }

        //private void EnableDependentControls(GridViewRowEventArgs e, int colStartPosition)
        //{

        //    DropDownList _ddlRawMaterial = (DropDownList)e.Row.Cells[colStartPosition].FindControl("ddlRawMaterial");
        //    if (_ddlRawMaterial.SelectedValue != "0")
        //    {
        //        FloatTextBox _txtRawMaterialX = (FloatTextBox)e.Row.Cells[colStartPosition].FindControl("txtRawMaterialX");
        //        _txtRawMaterialX.Enabled = true;
        //        colStartPosition += 1;

        //        FloatTextBox _txtRawMaterialY = (FloatTextBox)e.Row.Cells[colStartPosition].FindControl("txtRawMaterialY");
        //        _txtRawMaterialY.Enabled = true;
        //        colStartPosition += 1;

        //        FloatTextBox _txtRawMaterialZ = (FloatTextBox)e.Row.Cells[colStartPosition].FindControl("txtRawMaterialZ");
        //        _txtRawMaterialZ.Enabled = true;
        //        colStartPosition += 1;
        //    }

        //}

        protected void grdProductionOrderDetails_PreRender(object sender, EventArgs e)
        {
            grdProductionOrderDetails.DataBind();
        }
    }
}