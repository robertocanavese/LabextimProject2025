using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMLabExtim;
using CMLabExtim.CustomClasses;
using DLLabExtim;
using LabExtim.CustomControls;
using UILabExtim;
using Unit = DLLabExtim.Unit;

namespace LabExtim
{
    public partial class ProductionOrderDetailsInsertByO3 : ProductionOrderDetailsInsertController
    {
        private const string SCRIPT_DOFOCUS =
            @"window.setTimeout('DoFocus()', 1);
                    function DoFocus()
                    {
                        try {
                            document.getElementById('REQUEST_LASTFOCUS').focus();
                        } catch (ex) {}
                    }";

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

        public int CurMenuTarget
        {
            get
            {
                if (ViewState["CurMenuTarget"] != null)
                    return Convert.ToInt32(ViewState["CurMenuTarget"]);
                return 0;
            }
            set { ViewState["CurMenuTarget"] = value; }
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            itbNoOdP.SearchClick += StartSearch;
            yctNumber.SearchClick += SetIdOdp;
        }

        public void StartSearch(object sender, EventArgs e)
        {
            BindData();
        }

        public void SetIdOdp(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(yctNumber.ReturnValue))
            {
                if (yctNumber.ReturnValue.StartsWith("/"))
                {
                    using (DLLabExtim.QuotationDataContext db = new QuotationDataContext())
                    { 
                        itbNoOdP.Text = db.ProductionOrders.OrderByDescending(d => d.ID).FirstOrDefault(d => d.Number.Contains(yctNumber.ReturnValue.Substring(yctNumber.ReturnValue.IndexOf('/') + 1))).ID.ToString();
                    }
                }
                else if (yctNumber.ReturnValue.Length > 4)
                {
                    using (DLLabExtim.QuotationDataContext db = new QuotationDataContext())
                    {
                        itbNoOdP.Text = db.ProductionOrders.OrderByDescending(d => d.ID).FirstOrDefault(d => d.Number.Substring(2) == yctNumber.ReturnValue).ID.ToString();
                    }
                }
                else
                {
                    using (DLLabExtim.QuotationDataContext db = new QuotationDataContext())
                    {
                        itbNoOdP.Text = db.ProductionOrders.OrderByDescending(d => d.ID).FirstOrDefault(d => d.Number.Substring(2, 2) == yctNumber.ReturnValue).ID.ToString();
                    }
                }
            }
        }

        //        /// <summary>
        //        /// This function goes recursively all child controls and sets 
        //        /// onfocus attribute if the control has one of defined types.
        //        /// </summary>
        //        /// <param name="CurrentControl">the control to hook.</param>
        //        private void HookOnFocus(Control CurrentControl)
        //        {
        //            //checks if control is one of TextBox, DropDownList, ListBox or Button
        //            if ((CurrentControl is TextBox) ||
        //                (CurrentControl is DropDownList) ||
        //                (CurrentControl is ListBox) ||
        //                (CurrentControl is Button))
        //                //adds a script which saves active control on receiving focus 
        //                //in the hidden field __LASTFOCUS.
        //                (CurrentControl as WebControl).Attributes.Add(
        //                   "onfocus",
        //                   "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        //            //checks if the control has children
        //            if (CurrentControl.HasControls())
        //                //if yes do them all recursively
        //                foreach (Control CurrentChildControl in CurrentControl.Controls)
        //                    HookOnFocus(CurrentChildControl);
        //        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //    HookOnFocus(this.Page as Control);
                ////replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with 
                ////the posted value from Request    
                ////["__LASTFOCUS"]
                ////and registers the script to start after the page was rendered
                //Page.ClientScript.RegisterStartupScript(
                //    typeof(ProductionOrderDetailsInsertByO),
                //    "ScriptDoFocus",
                //    SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS",
                //                           Request["__LASTFOCUS"]),
                //    true);

                ////replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from 
                ////Request ["__LASTFOCUS"]
                ////and registers the script to start after Update panel was rendered
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ScriptDoFocus",
                    SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                    true);

                ldsOwner.AutoGenerateWhereClause = false;
                Parameter idCompany = new Parameter();
                idCompany.Name = "idCompany";
                idCompany.Type = TypeCode.Int32;
                idCompany.DefaultValue = CurrentCompanyId.ToString();
                ldsOwner.WhereParameters.Add(idCompany);
                Parameter leavingDate = new Parameter();
                leavingDate.Name = "leavingDate";
                leavingDate.Type = TypeCode.DateTime;
                leavingDate.DefaultValue = DateTime.Today.ToString();
                ldsOwner.WhereParameters.Add(leavingDate);
                ldsOwner.Where = "ID_Company = @idCompany && LeavingDate > @leavingDate"; //, CurrentCompanyId.ToString(), DateTime.Today.ToString());

                CurProductionDate = Utilities.GetNextWorkDate(-1, DateTime.Now.Date);
                txtDate.Text = CurProductionDate.ToString("dd/MM/yyyy");
                TempProductionOrderDetails = null;
                BindData();
                //LoadHeaderMenu(mnuOperations, MenuType.MenuProdRecord);


                //LoadHeaderMenu(mnuOperations, MenuType.MenuOperationNoPhases, CurrentCompanyId);
                //mnuOperations.Visible = false;
                //merge aziendale
                LoadHeaderMenu(mnuOperations1, MenuType.MenuOperationNoPhases, 1);
                LoadHeaderMenu(mnuOperations2, MenuType.MenuOperationNoPhases, 2);
                mnuOperations1.Visible = false;
                mnuOperations2.Visible = false;
            }
            //ReBindData();
        }

        private void BindData()
        {
            DateTime _tempDate;
            lbtNewItem.Enabled = DateTime.TryParse(txtDate.Text, out _tempDate);
            int _tempIdOwner;
            lbtNewItem.Enabled = int.TryParse(ddlEmployees.SelectedValue, out _tempIdOwner);

            lbtNewItem.Enabled = (ddlEmployees.SelectedValue != "" && ddlEmployees.SelectedValue != "0" && _tempDate != DateTime.MinValue);
            //riattivato controllo per pulsante salvataggio su giornata intera - disattivato dopo errori spostamento righe
            lbtSubmit.Enabled = (ddlEmployees.SelectedValue != "" && ddlEmployees.SelectedValue != "0" && _tempDate != DateTime.MinValue);

            int _tempIdProductionOrder;
            int.TryParse(itbNoOdP.Text, out _tempIdProductionOrder);

            //GetGridData();
            var _temp = new List<TempProductionOrderDetail>();

            //_temp.AddRange(GetProductionOrderDetailsOfAnOwner(_tempIdOwner, _tempDate, CurrentCompanyId, _tempIdProductionOrder));
            //merge aziendale
            _temp.AddRange(GetProductionOrderDetailsOfAnOwner(_tempIdOwner, _tempDate, 0, _tempIdProductionOrder));

            TempProductionOrderDetails = _temp;
            grdProductionOrderDetails.DataSource = TempProductionOrderDetails;
            grdProductionOrderDetails.DataBind();
            grdProductionOrderDetails.Columns[4].Visible = (ddlEmployees.SelectedValue == "" ||
                                                            ddlEmployees.SelectedValue == "0");

            SetWorkDayDurationAlert(lblOverTimeMessage);
        }

        private void ReBindData()
        {
            grdProductionOrderDetails.DataSource = TempProductionOrderDetails;
            grdProductionOrderDetails.DataBind();
            SetWorkDayDurationAlert(lblOverTimeMessage);
        }

        protected void ddlRawMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gvr = (GridViewRow) (((Control) sender).NamingContainer);
            var i = gvr.RowIndex;

            var _ddlRawMaterial = (DropDownList) gvr.FindControl("ddlRawMaterial");

            if (_ddlRawMaterial.SelectedValue != "P0")
            {
                UpdateDataSource(i);
                TempProductionOrderDetails[i].ID_PickingItem =
                    Convert.ToInt32(_ddlRawMaterial.SelectedValue.Substring(1));
                TempProductionOrderDetails[i].RFlag = (_ddlRawMaterial.SelectedValue.Substring(0, 1) == "M" ? _ddlRawMaterial.SelectedValue.Substring(0, 1) : null);  //_ddlRawMaterial.SelectedValue.Substring(0, 1);
                //PickingItem _curPickingItem = GetPickingItem(TempProductionOrderDetails[i].ID_PickingItem);
                var _curPickingItem =
                    GetPickingItemFromMacroOrPickingItem(TempProductionOrderDetails[i].ID_PickingItem,
                        TempProductionOrderDetails[i].RFlag);
                TempProductionOrderDetails[i].SupplierCode = _curPickingItem.SupplierCode;

                var _ddlUMRawMaterial = (DropDownList) gvr.FindControl("ddlUMRawMaterial");
                _ddlUMRawMaterial.ClearSelection();
                _ddlUMRawMaterial.Items.FindByValue(_curPickingItem.UM.ToString()).Selected = true;
                TempProductionOrderDetails[i].UMRawMaterial = _curPickingItem.UM;
            }
            else
            {
                TempProductionOrderDetails[i].ID_PickingItem = null;
                TempProductionOrderDetails[i].RFlag = null;
                TempProductionOrderDetails[i].SupplierCode = null;
                TempProductionOrderDetails[i].UMRawMaterial = null;
                TempProductionOrderDetails[i].RawMaterialQuantity = null;
            }
        }

        protected void ddlRawMaterialSup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var gvr = (GridViewRow) (((Control) sender).NamingContainer);
            var i = gvr.RowIndex;

            var _ddlRawMaterialSup = (DropDownList) gvr.FindControl("ddlRawMaterialSup");

            if (_ddlRawMaterialSup.SelectedValue != "P0")
            {
                UpdateDataSource(i);
                TempProductionOrderDetails[i].ID_PickingItemSup =
                    Convert.ToInt32(_ddlRawMaterialSup.SelectedValue.Substring(1));
                TempProductionOrderDetails[i].SFlag = (_ddlRawMaterialSup.SelectedValue.Substring(0, 1) == "M" ? _ddlRawMaterialSup.SelectedValue.Substring(0, 1) : null);
                //PickingItem _curPickingItemSup = GetPickingItem(TempProductionOrderDetails[i].ID_PickingItemSup);
                var _curPickingItemSup =
                    GetPickingItemFromMacroOrPickingItem(TempProductionOrderDetails[i].ID_PickingItemSup,
                        TempProductionOrderDetails[i].SFlag);
                TempProductionOrderDetails[i].SupplierCodeSup = _curPickingItemSup.SupplierCode;

                var _ddlUMUser = (DropDownList) grdProductionOrderDetails.Rows[i].FindControl("ddlUMUser");
                _ddlUMUser.ClearSelection();
                _ddlUMUser.Items.FindByValue(_curPickingItemSup.UM.ToString()).Selected = true;
                TempProductionOrderDetails[i].UMUser = _curPickingItemSup.UM;
            }
            else
            {
                TempProductionOrderDetails[i].ID_PickingItemSup = null;
                TempProductionOrderDetails[i].SFlag = null;
                TempProductionOrderDetails[i].SupplierCodeSup = null;
                TempProductionOrderDetails[i].UMUser = null;
                TempProductionOrderDetails[i].RawMaterialX = null;
                TempProductionOrderDetails[i].RawMaterialY = null;
                TempProductionOrderDetails[i].RawMaterialZ = null;
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

        protected void grdProductionOrderDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderDetails.PageIndex = e.NewPageIndex;
        }

        protected void lbtNewItem_Click(object sender, EventArgs e)
        {
            int _tempIdProductionOrder;
            int.TryParse(itbNoOdP.Text, out _tempIdProductionOrder);

            for (var i = 0; i < 5; i++)
                InsertRow(CurProductionDate, CurrentCompanyId, Convert.ToInt32(ddlEmployees.SelectedValue), _tempIdProductionOrder);
            grdProductionOrderDetails.DataSource = TempProductionOrderDetails;
            grdProductionOrderDetails.DataBind();
            grdProductionOrderDetails.SelectedIndex = grdProductionOrderDetails.Rows.Count - 5;
        }

        protected void ldsOfaDropDownList_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (((LinqDataSourceView) sender).TableName == "") //ldsRawMaterial.TableName)
            {
                using (var _ctx = new QuotationDataContext())
                {
                    var _items = new List<ComboListItem>();
                    //_items.AddRange(_ctx.PickingItems.OrderBy(p => p.Order).Where(p => p.TypeCode == 6).Select(p => new ComboListItem { Prefix = "P", CommonKey = p.ID, Text = p.ItemDescription, Order = p.Order }).ToList());
                    //_items.AddRange(_ctx.MacroItems.OrderBy(p => p.Order).Where(p => p.TypeCode == 6).Select(p => new ComboListItem { Prefix = "M", CommonKey = p.ID, Text = p.MacroItemDescription, Order = p.Order }).ToList());

                    _items.AddRange(
                        //_ctx.PickingItems.Where(p => p.TypeCode != 31 && p.ID_Company == CurrentCompanyId)
                        //merge aziendale
                        _ctx.PickingItems.Where(p => p.TypeCode != 31)
                            .OrderBy(p => p.Order)
                            .Select(
                                p =>
                                    new ComboListItem
                                    {
                                        Prefix = "P",
                                        CommonKey = p.ID,
                                        Text = p.ItemDescription,
                                        Order = p.Order
                                    })
                            .ToList());
                    _items.AddRange(
                        //_ctx.MacroItems.Where(p => p.ID_Company == CurrentCompanyId).OrderBy(p => p.Order)
                        //merge aziendale
                        _ctx.MacroItems.OrderBy(p => p.Order)
                            .Select(
                                p =>
                                    new ComboListItem
                                    {
                                        Prefix = "M",
                                        CommonKey = p.ID,
                                        Text = p.MacroItemDescription,
                                        Order = p.Order
                                    })
                            .ToList());

                    //_items.AddRange(_ctx.VW_HitsOfPickingItems.Where(p => p.TypeCode != 31).Select(p => new ComboListItem { Prefix = "P", CommonKey = p.ID, Text = p.ItemDescription, Order = p.Order, Hits = p.Hits }).ToList());
                    //_items.AddRange(_ctx.VW_HitsOfMacroItems.Select(p => new ComboListItem { Prefix = "M", CommonKey = p.ID, Text = p.MacroItemDescription, Order = p.Order, Hits = p.Hits }).ToList());

                    e.Result = _items.OrderBy(i => i.Order);
                }
            }

            if (((LinqDataSourceView)sender).TableName == "PickingItems")
            {
                using (var _ctx = new QuotationDataContext())
                {
                    //e.Result = _ctx.PickingItems.Where(p => p.TypeCode == 31 && p.ID_Company == CurrentCompanyId).OrderBy(p => p.Order).ToArray();
                    // merge aziendale
                    e.Result = _ctx.PickingItems.Where(p => p.TypeCode == 31).OrderBy(p => p.Order).ToArray();
                }
            }

            if (((LinqDataSourceView)sender).TableName == "Suppliers")
            {
                using (var _ctx = new QuotationDataContext())
                {
                    int min = 0;
                    int max = 0;
                    if (CurrentCompanyId == 1){
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
            if (((LinqDataSourceView) sender).TableName == ldsOwner.TableName)
            {
                var _dummy = new Employee();
                _dummy.ID = 0;
                _dummy.UniqueName = "N/DEF";
                ((IList) e.Result).Insert(0, _dummy);
            }
            if (((LinqDataSourceView) sender).TableName == ldsSupplier.TableName)
            {
                var _dummy = new Supplier();
                _dummy.Code = 0;
                _dummy.Name = "N/DEF";
                ((IList) e.Result).Insert(0, _dummy);
            }
            if (((LinqDataSourceView) sender).TableName == ldsUM.TableName)
            {
                var _dummy = new Unit();
                _dummy.ID = 0;
                _dummy.Description = "N/DEF";
                ((IList) e.Result).Insert(0, _dummy);
            }
            if (((LinqDataSourceView) sender).TableName == ldsRawMaterial.TableName)
            {
                //DLLabExtim.PickingItem _dummy = new DLLabExtim.PickingItem();
                var _dummy = new ComboListItem();
                _dummy.CommonKey = 0;
                _dummy.Prefix = "P";
                _dummy.Text = "N/DEF";
                ((IList) e.Result).Insert(0, _dummy);
            }
        }

        protected void grdProductionOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
            }
            if (e.CommandName == "Submit")
            {
                var i = Convert.ToInt32(e.CommandArgument);
                try
                {
                    SubmitRow(i, txtDate.Text, ddlEmployees.SelectedValue);
                }
                catch
                {
                    var _errorControl =
                        (Label) grdProductionOrderDetails.Rows[i].Cells[2].FindControl("txtID_ProductionOrder");
                    _errorControl.ForeColor = Color.Red;
                    _errorControl.BackColor = Color.Yellow;
                }
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
                    //salvataggio multiriga disattivato per problemi di spostamento dati in giornate intere.
                    //if (ddlEmployees.SelectedValue != "" && ddlEmployees.SelectedValue != "0")
                    //{
                        //sono selezionate sia la data che l'operatore
                        SubmitRow(i, txtDate.Text, ddlEmployees.SelectedValue);
                    //}
                    //else
                    //{
                        //SubmitRow(i, txtDate.Text, TempProductionOrderDetails[i].ID_Owner.ToString());
                    //}
                    
                }
                catch
                {
                    var _errorControl =
                        (Label) grdProductionOrderDetails.Rows[i].Cells[2].FindControl("txtID_ProductionOrder");
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
            if (e.Row.RowType == DataControlRowType.DataRow && !e.Row.RowState.ToString().Contains("Edit"))
            {
                try
                {
                    var _lblState = (Label) e.Row.FindControl("lblState");
                    _lblState.Text = ((TempProductionOrderDetail) e.Row.DataItem).State ==
                                     TempProductionOrderDetail.ItemState.Error
                        ? "???"
                        : "OK";

                    var _txtID_ProductionOrder = (Label) e.Row.FindControl("txtID_ProductionOrder");
                    //_txtID_ProductionOrder.Text = ((TempProductionOrderDetail)e.Row.DataItem).ID_ProductionOrder.ToString();
                    var _found = false;
                    _txtID_ProductionOrder.ToolTip = GetProductionOrderToolTip(_txtID_ProductionOrder.Text, out _found, -1);

                    var _txtProductionTime = (TextBox) e.Row.FindControl("txtProductionTime");
                    if (((TempProductionOrderDetail) e.Row.DataItem).ProductionTime != null)
                        _txtProductionTime.Text =
                            new TimeSpan(
                                Convert.ToInt64(((TempProductionOrderDetail) e.Row.DataItem).ProductionTime.Value))
                                .ToString();

                    ((LinkButton) e.Row.FindControl("SubmitLinkButton")).Visible = (ddlEmployees.SelectedValue != "" &&
                                                                                    ddlEmployees.SelectedValue != "0");
                    //((LinkButton)e.Row.FindControl("EditLinkButton")).Visible = (ddlEmployees.SelectedValue != "" && ddlEmployees.SelectedValue != "0");
                    ((LinkButton) e.Row.Cells[2].Controls[0]).Visible = (ddlEmployees.SelectedValue != "" &&
                                                                         ddlEmployees.SelectedValue != "0");
                    ((LinkButton) e.Row.FindControl("DeleteLinkButton")).Visible = (ddlEmployees.SelectedValue != "" &&
                                                                                    ddlEmployees.SelectedValue != "0");
                }
                catch
                {
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.ToString().Contains("Edit"))
            {
                //    GridViewRow _curRow = e.Row ;
                //    TempProductionOrderDetail _curItem = TempProductionOrderDetails[e.Row.RowIndex];

                //Label _lblState = (Label)e.Row.FindControl("lblState");
                //_lblState.Text = ((TempProductionOrderDetail)e.Row.DataItem).State == TempProductionOrderDetail.ItemState.Error ? "???" : "OK";

                var _txtID_ProductionOrder = (IntTextBox) e.Row.FindControl("txtID_ProductionOrder");
                if (_txtID_ProductionOrder != null)
                {
                    //_txtID_ProductionOrder.Text = ((TempProductionOrderDetail)_curItem).ID_ProductionOrder.ToString();
                    var _found = false;
                    //_txtID_ProductionOrder.ToolTip = GetProductionOrderToolTip(_txtID_ProductionOrder.Text, out _found, CurrentCompanyId);
                    //merge aziendale
                    _txtID_ProductionOrder.ToolTip = GetProductionOrderToolTip(_txtID_ProductionOrder.Text, out _found, -1);
                }

                var _ddlPhase = (DropDownList) e.Row.FindControl("ddlPhase");
                if (_ddlPhase != null)
                {
                    if (((TempProductionOrderDetail) e.Row.DataItem).ID_Phase != null)
                        _ddlPhase.SelectedValue = ((TempProductionOrderDetail) e.Row.DataItem).ID_Phase.ToString();
                }

                var _txtProductionTime = (TextBox) e.Row.FindControl("txtProductionTime");
                if (_txtProductionTime != null)
                {
                    if (((TempProductionOrderDetail) e.Row.DataItem).ProductionTime != null)
                        _txtProductionTime.Text =
                            new TimeSpan(
                                Convert.ToInt64(((TempProductionOrderDetail) e.Row.DataItem).ProductionTime.Value))
                                .ToString();
                }

                var _ddlRawMaterial = (DropDownList) e.Row.FindControl("ddlRawMaterial");
                if (_ddlRawMaterial != null)
                {
                    Helper.BindTooltip(_ddlRawMaterial);
                    if (((TempProductionOrderDetail) e.Row.DataItem).ID_PickingItem != null)
                        _ddlRawMaterial.SelectedValue = ((TempProductionOrderDetail) e.Row.DataItem).RFlag.ToString() +
                                                        ((TempProductionOrderDetail) e.Row.DataItem).ID_PickingItem;
                }

                var _ddlUMRawMaterial = (DropDownList) e.Row.FindControl("ddlUMRawMaterial");
                if (_ddlUMRawMaterial != null)
                {
                    if (((TempProductionOrderDetail) e.Row.DataItem).UMRawMaterial != null)
                        _ddlUMRawMaterial.SelectedValue =
                            ((TempProductionOrderDetail) e.Row.DataItem).UMRawMaterial.ToString();
                }

                var _ddlRawMaterialSup = (DropDownList) e.Row.FindControl("ddlRawMaterialSup");
                if (_ddlRawMaterialSup != null)
                {
                    Helper.BindTooltip(_ddlRawMaterialSup);
                    if (((TempProductionOrderDetail) e.Row.DataItem).ID_PickingItemSup != null)
                        _ddlRawMaterialSup.SelectedValue = ((TempProductionOrderDetail) e.Row.DataItem).SFlag.ToString() +
                                                           ((TempProductionOrderDetail) e.Row.DataItem)
                                                               .ID_PickingItemSup;
                }

                var _ddlUMUser = (DropDownList) e.Row.FindControl("ddlUMUser");
                if (_ddlUMUser != null)
                {
                    if (((TempProductionOrderDetail) e.Row.DataItem).UMUser != null)
                        _ddlUMUser.SelectedValue = ((TempProductionOrderDetail) e.Row.DataItem).UMUser.ToString();
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
            ReBindData();
            //grdProductionOrderDetails.DataBind();
        }

        protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
            grdProductionOrderDetails.EditIndex = -1;
        }

        protected void grdProductionOrderDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState.ToString().Contains("Edit"))
            //{
            //    Menu _mnuMaterials = (Menu)e.Row.Cells[5].FindControl("mnuMaterials");
            //    LoadHeaderMenu(_mnuMaterials, MenuType.MenuMaterials);
            //    Menu _mnuMaterialsSup = (Menu)e.Row.Cells[8].FindControl("mnuMaterialsSup");
            //    LoadHeaderMenu(_mnuMaterialsSup, MenuType.MenuMaterials);

            //}
        }

        protected void mnuOperations_MenuItemClick(object sender, MenuEventArgs e)
        {
            //GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            //int i = gvr.RowIndex;
            var i = grdProductionOrderDetails.EditIndex;
            var gvr = grdProductionOrderDetails.Rows[i];


            using (var QuotationDataContext = new QuotationDataContext())
            {

                //merge aziendale
                //IEnumerable<PickingItem> _pickingItems = QuotationDataContext.PickingItems.Where(p => p.ID_Company == CurrentCompanyId);
                //IEnumerable<MacroItem> _macroItems = QuotationDataContext.MacroItems.Where(p => p.ID_Company == CurrentCompanyId);
                IEnumerable<PickingItem> _pickingItems = QuotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = QuotationDataContext.MacroItems;

                var _um = -1;
                int? _supplierCode = -1;

                var _menuPath = e.Item.Value.Split('.');

                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    var _item = _pickingItems.First(pi => pi.ID == _cbk.CommonKey);
                    _um = _item.UM;
                    _supplierCode = _item.SupplierCode;
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var _item = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    _um = _item.UM;
                    _supplierCode = null;
                }
                //string _radix = ((Control)sender).ClientID.Replace("mnuMaterials", "");
                var _clientPos = grdProductionOrderDetails.EditIndex;
                // Convert.ToInt32(((Control)sender).ClientID.Substring(55, 2));
                //if (!((Control)sender).ClientID.ToLower().Contains("sup"))
                if (CurMenuTarget == 0)
                {
                    TempProductionOrderDetails[_clientPos].ID_PickingItem = _cbk.CommonKey;
                    TempProductionOrderDetails[_clientPos].RFlag = (_cbk.Prefix == "M" ? _cbk.Prefix : null);

                    var _ddlUMRawMaterial = (DropDownList) gvr.FindControl("ddlUMRawMaterial");
                    _ddlUMRawMaterial.ClearSelection();
                    _ddlUMRawMaterial.Items.FindByValue(_um.ToString()).Selected = true;
                    TempProductionOrderDetails[i].UMRawMaterial = _um;
                    if (_supplierCode != null) TempProductionOrderDetails[i].SupplierCode = _supplierCode;
                    CurMenuTarget = 1;
                }
                else
                {
                    TempProductionOrderDetails[_clientPos].ID_PickingItemSup = _cbk.CommonKey;
                    TempProductionOrderDetails[_clientPos].SFlag = (_cbk.Prefix == "M"? _cbk.Prefix : null);

                    var _ddlUMUser = (DropDownList) grdProductionOrderDetails.Rows[i].FindControl("ddlUMUser");
                    _ddlUMUser.ClearSelection();
                    _ddlUMUser.Items.FindByValue(_um.ToString()).Selected = true;
                    TempProductionOrderDetails[i].UMUser = _um;
                    if (_supplierCode != null) TempProductionOrderDetails[i].SupplierCodeSup = _supplierCode;
                    CurMenuTarget = 0;
                }
            }
            UpdateDataSource(i);
            grdProductionOrderDetails.DataBind();
            //ReBindData();
        }

        protected void grdProductionOrderDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdProductionOrderDetails.EditIndex = e.NewEditIndex;

            //mnuOperations.Visible = true;
            //merge aziendale
            mnuOperations1.Visible = true;
            mnuOperations2.Visible = true;
        }

        //protected int GetSelectedIndex(DropDownList control, object value)
        //{

        //    if (value != null)
        //        return control.Items.IndexOf(control.Items.FindByValue(value.ToString()));
        //    else
        //        return 0;

        //}

        protected void grdProductionOrderDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                UpdateDataSource(e.RowIndex);
                grdProductionOrderDetails.EditIndex = -1;

                //mnuOperations.Visible = false;
                //merge aziendale
                mnuOperations1.Visible = false;
                mnuOperations2.Visible = false;


                CurMenuTarget = 0;
                SubmitRow(e.RowIndex, txtDate.Text, ddlEmployees.SelectedValue);
            }
            catch
            {
                e.Cancel = true;
            }
        }

        public void UpdateDataSource(int rowIndex)
        {
            var i = rowIndex;
            try
            {
                TempProductionOrderDetails[i].State = TempProductionOrderDetail.ItemState.Error;

                var _txtID_ProductionOrder =
                    (IntTextBox) grdProductionOrderDetails.Rows[i].FindControl("txtID_ProductionOrder");
                if (_txtID_ProductionOrder.Text != string.Empty)
                    TempProductionOrderDetails[i].ID_ProductionOrder = Convert.ToInt32(_txtID_ProductionOrder.Text);

                var _ddlPhase = (DropDownList) grdProductionOrderDetails.Rows[i].FindControl("ddlPhase");
                if (_ddlPhase.SelectedValue != string.Empty)
                    TempProductionOrderDetails[i].ID_Phase = Convert.ToInt32(_ddlPhase.SelectedValue);

                var _txtProductionTime =
                    (TextBox) grdProductionOrderDetails.Rows[i].FindControl("txtProductionTime");
                if (_txtProductionTime.Text != string.Empty)
                {
                    var _timeParts = _txtProductionTime.Text.Split(':');
                    TempProductionOrderDetails[i].ProductionTime =
                        new TimeSpan(Int32.Parse(_timeParts[0]), Int32.Parse(_timeParts[1]), 0).Ticks;
                }

                var _ddlRawMaterial =
                    (DropDownList) grdProductionOrderDetails.Rows[i].FindControl("ddlRawMaterial");
                if (_ddlRawMaterial.SelectedValue != "P0")
                {
                    var _txtRawMaterialQuantity =
                        (FloatTextBox) grdProductionOrderDetails.Rows[i].FindControl("txtRawMaterialQuantity");
                    if (_txtRawMaterialQuantity.Text != string.Empty)
                        TempProductionOrderDetails[i].RawMaterialQuantity =
                            Convert.ToSingle(_txtRawMaterialQuantity.Text);
                }
                else
                {
                    TempProductionOrderDetails[i].RawMaterialQuantity = null;
                }


                var _ddlRawMaterialSup =
                    (DropDownList) grdProductionOrderDetails.Rows[i].FindControl("ddlRawMaterialSup");

                if (_ddlRawMaterialSup.SelectedValue != "P0")
                {
                    var _txtRawMaterialX =
                        (FloatTextBox) grdProductionOrderDetails.Rows[i].FindControl("txtRawMaterialX");
                    if (_txtRawMaterialX.Text != string.Empty)
                        TempProductionOrderDetails[i].RawMaterialX = Convert.ToSingle(_txtRawMaterialX.Text);

                    var _txtRawMaterialY =
                        (FloatTextBox) grdProductionOrderDetails.Rows[i].FindControl("txtRawMaterialY");
                    if (_txtRawMaterialY.Text != string.Empty)
                        TempProductionOrderDetails[i].RawMaterialY = Convert.ToSingle(_txtRawMaterialY.Text);

                    var _txtRawMaterialZ =
                        (FloatTextBox) grdProductionOrderDetails.Rows[i].FindControl("txtRawMaterialZ");
                    if (_txtRawMaterialZ.Text != string.Empty)
                        TempProductionOrderDetails[i].RawMaterialZ = Convert.ToSingle(_txtRawMaterialZ.Text);
                }
                else
                {
                    TempProductionOrderDetails[i].RawMaterialX = null;
                    TempProductionOrderDetails[i].RawMaterialY = null;
                    TempProductionOrderDetails[i].RawMaterialZ = null;
                }

                var _chkQuantityOver = (CheckBox) grdProductionOrderDetails.Rows[i].FindControl("chkQuantityOver");
                TempProductionOrderDetails[i].QuantityOver = _chkQuantityOver.Checked;

                var _txtNote = (TextBox) grdProductionOrderDetails.Rows[i].FindControl("txtNote");
                if (_txtNote.Text != string.Empty)
                    TempProductionOrderDetails[i].Note = _txtNote.Text;
            }
            catch
            {
                throw;
            }
        }

        protected void grdProductionOrderDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdProductionOrderDetails.EditIndex = -1;

            //mnuOperations.Visible = false;
            //merge aziendale
            mnuOperations1.Visible = false;
            mnuOperations2.Visible = false;

        }

        protected void rdbRawMaterialSup_CheckedChanged(object sender, EventArgs e)
        {
            CurMenuTarget = 1;
        }

        protected void rdbRawMaterial_CheckedChanged(object sender, EventArgs e)
        {
            CurMenuTarget = 0;
        }
    }
}