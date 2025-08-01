using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMLabExtim.CustomClasses;
using DLLabExtim;
using UILabExtim;


namespace LabExtim
{
    public partial class MacroItemsConsole : QuotationController
    {
        protected List<SPPickingItemCalculated> m_calculated;
        protected short m_voicePosition;

        protected Mode CurMacroItemsConsoleMode
        {
            get
            {
                if (ViewState["CurMacroItemsConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode)ViewState["CurMacroItemsConsoleMode"];
            }
            set { ViewState["CurMacroItemsConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdMacroItems);
            DynamicDataManager1.RegisterControl(dtvMacroItem);
            DynamicDataManager1.RegisterControl(grdMacroItemDetails);
            m_voicePosition = 1000;
        }

        public void SetFilter()
        {

            ldsMacroItems.AutoGenerateWhereClause = false;

            ldsMacroItems.WhereParameters.Clear();
            var _filter = "TRUE ";


            if (itbNo.ReturnValue != 0)
            {
                ldsMacroItems.WhereParameters.Add("ID", DbType.Int32, itbNo.ReturnValue.ToString());
                _filter += " AND ID = @ID";
            }

            if (txtTitleContains.Text != string.Empty)
            {
                ldsMacroItems.WhereParameters.Add("MacroItemDescription", DbType.String, txtTitleContains.Text);
                _filter += " AND MacroItemDescription.Contains(@MacroItemDescription)";
            }

            if (ddlCompanies.SelectedValue != string.Empty)
            {
                ldsMacroItems.WhereParameters.Add("ID_Company", DbType.Int32, ddlCompanies.SelectedValue);
                _filter += " AND ID_Company == @ID_Company";
            }

            if (ddlTypes.SelectedValue != string.Empty)
            {
                ldsMacroItems.WhereParameters.Add("TypeCode", DbType.Int32, ddlTypes.SelectedValue);
                _filter += " AND TypeCode == @TypeCode";
            }
            if (ddlItemTypes.SelectedValue != string.Empty)
            {
                ldsMacroItems.WhereParameters.Add("ItemTypeCode", DbType.Int32, ddlItemTypes.SelectedValue);
                _filter += " AND ItemTypeCode == @ItemTypeCode";
            }

            //ldsMacroItems.WhereParameters.Add("Inserted", DbType.Boolean, "");
            ldsMacroItems.WhereParameters.Add("Inserted", DbType.Boolean, CurMacroItemsConsoleMode == Mode.DeactivatedItems ? "false" : "true");
            _filter += " AND Inserted == @Inserted";

            ldsMacroItems.Where = _filter;


        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                mvwMain.ActiveViewIndex = 1;
                FillControls();
                SwitchDependingControls(CurMacroItemsConsoleMode);
                LoadHeaderMenu(mnuOperations, MenuType.MenuOperations, CurrentCompanyId);

            }
            FillDependingControls(CurMacroItemsConsoleMode);
            SetFilter();

        }

        private void FillControls()
        {
            ddlOrderBy.Items.Add(new ListItem("Ordine", ""));
            ddlOrderBy.Items.Add(new ListItem("Tipo, Tipo voce, Descrizione macrovoce", "TipoTipoVoceDesc"));
            ddlOrderBy.Items.Add(new ListItem("Descrizione voce", "MacroItemDescription"));
            ddlOrderBy.Items.Add(new ListItem("Data creazione", "Date"));
            ddlOrderBy.DataBind();

        }

        protected void FillDependingControls(Mode mode)
        {

            using (var _qc = new QuotationDataContext())
            {

                if (dtvMacroItem.DataKey.Value != null)
                {
                    WriteQuantities(_qc);
                    _qc.SubmitChanges();

                    var _currentMacroItem =
                        _qc.MacroItems.First(MacroItem => MacroItem.ID == Convert.ToInt32(dtvMacroItem.DataKey.Value));
                    foreach (var _macroItemDetail in _currentMacroItem.MacroItemDetails)
                    {
                        if (_macroItemDetail.PickingItem.Inserted == false)
                        {
                            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ObsoleteItem);
                            break;
                        }
                    }
                }
            }

        }

        protected void GetModeDescription(Mode mode)
        {
            switch (mode)
            {
                case Mode.InputItems:
                    lblModeDescription.Text = "(gestione macrovoci attive)";
                    break;
                case Mode.Calculation:
                    lblModeDescription.Text = "(prospetto calcolato)";
                    break;
                case Mode.DeactivatedItems:
                    lblModeDescription.Text = "(gestione macrovoci disattivate)";
                    break;
                default:
                    lblModeDescription.Text = "";
                    break;
            }
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            grdMacroItems.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            dtvMacroItem.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            lblddlOrderBy.Visible = (mode != Mode.Calculation);
            ddlOrderBy.Visible = (mode != Mode.Calculation);
            CurMacroItemsConsoleMode = mode;
            GetModeDescription(CurMacroItemsConsoleMode);
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            ldsMacroItems.WhereParameters["Inserted"].DefaultValue = true.ToString();
            grdMacroItems.AutoGenerateDeleteButton = false;
            SwitchDependingControls(Mode.InputItems);
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewCalculated_Click(object sender, EventArgs e)
        {
            ldsMacroItems.WhereParameters["Inserted"].DefaultValue = "";
            SwitchDependingControls(Mode.Calculation);
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewDeactivatedItems_Click(object sender, EventArgs e)
        {
            ldsMacroItems.WhereParameters["Inserted"].DefaultValue = false.ToString();
            grdMacroItems.AutoGenerateDeleteButton = true;
            SwitchDependingControls(Mode.DeactivatedItems);
            OnFilterSelectedIndexChanged(null, null);
        }


        protected void grdMacroItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMacroItems.PageIndex = e.NewPageIndex;
            dtvMacroItem.ChangeMode(DetailsViewMode.ReadOnly);
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurMacroItemsConsoleMode);
        }


        protected void lbtPrintMacroItems_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "MacroItems", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdMacroItems.EditIndex = -1;
            grdMacroItems.PageIndex = 0;
        }

        protected void ddlTypes_DataBound(object sender, EventArgs e)
        {
            ddlTypes.Items.Insert(0, new ListItem("Tutti", ""));

            using (var _qc = new QuotationDataContext())
            {
                var _firstType = _qc.Types.OrderBy(t => t.Order).FirstOrDefault();
                ddlTypes.Items.FindByValue("4").Selected = true;
            }
        }

        protected void ddlCompanies_DataBound(object sender, EventArgs e)
        {

            using (var _qc = new QuotationDataContext())
            {
                ddlCompanies.Items.FindByValue(CurrentCompanyId.ToString()).Selected = true;
            }

        }

        protected void ddlItemTypes_DataBound(object sender, EventArgs e)
        {
            ddlItemTypes.Items.Insert(0, new ListItem("Tutti", ""));
        }

        protected void grdMacroItems_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //System.Type typeList = e.Result.GetType(); //List<T> for a select statement
            //System.Type typeObj = e.Result.GetType().GetGenericArguments()[0]; //<T>
            //object ojb = Activator.CreateInstance(typeObj);  //new T
            //// insert the new T into the list by using InvokeMember on the List<T>
            //object result = null;
            //object[] arguments = { 0, ojb };
            //result = typeList.InvokeMember("Insert", BindingFlags.InvokeMethod, null, e.Result, arguments);
        }

        protected void OnGridViewDataBound(object sender, EventArgs e)
        {
            if (CurMacroItemsConsoleMode == Mode.InputItems && grdMacroItems.Rows.Count == 0 &&
                grdMacroItems.PageIndex == 0)
            {
                dtvMacroItem.ChangeMode(DetailsViewMode.Insert);
            }
        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            //Session[MenuType.MenuOperations.ToString()] = null;
            //Cache.Remove(MenuType.MenuOperations.ToString()) ;
            grdMacroItems.DataBind();
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (!TypeAndItemTypeAreEqual(e.NewValues, e.OldValues))
            {
                //Session[MenuType.MenuOperations.ToString()] = null;
                //Cache.Remove(MenuType.MenuOperations.ToString());
            }
            grdMacroItems.DataBind();
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            //Session[MenuType.MenuOperations.ToString()] = null;
            //Cache.Remove(MenuType.MenuOperations.ToString());
            grdMacroItems.DataBind();
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode != DetailsViewMode.ReadOnly)
            {
                grdMacroItems.EditIndex = -1;
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            dtvMacroItem.DataBind();
            var rowCount = dtvMacroItem.Rows.Count;
            if (rowCount > 0)
            {
                SetDeleteConfirmation(dtvMacroItem.Rows[rowCount - 1]);

                var _lbtDeactivate = (LinkButton)dtvMacroItem.FindControl("lbtDeactivate");
                var _lbtActivate = (LinkButton)dtvMacroItem.FindControl("lbtActivate");
                _lbtDeactivate.Enabled = (CurMacroItemsConsoleMode == Mode.InputItems);
                _lbtActivate.Enabled = (CurMacroItemsConsoleMode == Mode.DeactivatedItems);
            }

            if (dtvMacroItem.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvMacroItem.Rows[0];

                var _dycCompany = (DynamicControl)_dvr.FindControl("dycCompany");
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).SelectedValue = ddlCompanies.SelectedValue;
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
                        btn.OnClientClick = "return confirm('Sei sicuro di voler eliminare questa macrovoce?');";
                    }
                }
            }
        }

        protected void grdMacroItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(grdMacroItems,
                    "Select$" + e.Row.RowIndex);

                var _row = e.Row;
                if (((MacroItem)e.Row.DataItem).Inserted == false)
                {
                    _row.ForeColor = Color.Red;
                }

                if (!string.IsNullOrEmpty(((MacroItem)e.Row.DataItem).Link))
                {
                    var _dycLink = (DynamicControl)e.Row.Cells[9].FindControl("dycLink");
                    ((DataControlFieldCell)(_dycLink.Parent)).ToolTip = GetMacroItemDescription(((MacroItem)e.Row.DataItem).Link);

                }
                if (!string.IsNullOrEmpty(((MacroItem)e.Row.DataItem).PILink))
                {
                    var _dycMILink = (DynamicControl)e.Row.Cells[10].FindControl("dycPILink");
                    ((DataControlFieldCell)(_dycMILink.Parent)).ToolTip = GetPickingItemDescription(((MacroItem)e.Row.DataItem).PILink);

                }

            }
        }

        protected void DetailsDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            ((MacroItem)e.NewObject).Inserted = true;
            ((MacroItem)e.NewObject).Cost = 0m;
        }

        protected void dtvMacroItem_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Deactivate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _macroItem =
                        _qc.MacroItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvMacroItem.DataKey.Value));
                    _macroItem.Inserted = false;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuOperations.ToString()] = null;
                    //Cache.Remove(MenuType.MenuOperations.ToString());
                }
                grdMacroItems.DataBind();
            }
            if (e.CommandName == "Activate")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _macroItem =
                        _qc.MacroItems.SingleOrDefault(pi => pi.ID == Convert.ToInt32(dtvMacroItem.DataKey.Value));
                    _macroItem.Inserted = true;
                    _qc.SubmitChanges();
                    //Session[MenuType.MenuOperations.ToString()] = null;
                    //Cache.Remove(MenuType.MenuOperations.ToString());
                }
                grdMacroItems.DataBind();
            }
        }

        protected void dtvMicroItem_ItemCreated(object sender, EventArgs e)
        {
            var _lbtDeactivate = (LinkButton)dtvMacroItem.FindControl("lbtDeactivate");
            var _lbtActivate = (LinkButton)dtvMacroItem.FindControl("lbtActivate");
            //if (CurMacroItemsConsoleMode == Mode.InputItems)
            //{
            if (_lbtDeactivate != null && _lbtActivate != null)
            {
                _lbtDeactivate.OnClientClick =
                    @"return confirm('Sei sicuro di voler disattivare definitivamente questa macrovoce? (non comparirà più in questa lista e l\'utente sarà avvisato se, in quanto facente parte di un modello di preventivo già esistente al momento della disattivazione, dovrà essere automaticamente inserita in un preventivo)');";
                //}
                //if (CurMacroItemsConsoleMode == Mode.DeactivatedItems)
                //{
                _lbtActivate.OnClientClick =
                    @"return confirm('Sei sicuro di voler riattivare questa ,acrovoce? (non comparirà più in questa lista ma tornerà a comparire nella lista principale e nei menu)');";
                //}
            }
        }

        protected void ldsMacroItems_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //lblddlOrderBy.Visible = true;
            //ddlOrderBy.Visible = true;

            if (!IsPostBack)
                SetFilter();

            ldsMacroItems.OrderByParameters.Clear();
            ldsMacroItems.AutoGenerateOrderByClause = false;
            var table = ldsMacroItems.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            switch (ddlOrderBy.SelectedValue)
            {
                case "TipoTipoVoceDesc":
                    _qc.MacroItems.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                    break;
                case "MacroItemDescription":
                    e.Result =
               _qc.MacroItems.OrderBy(qd => qd.MacroItemDescription);
                    break;
                case "Date":
                    e.Result =
               _qc.MacroItems.OrderBy(qd => qd.Date);
                    break;
                case "":
                    e.Result =
               _qc.MacroItems.OrderBy(qd => qd.Order);
                    break;

                default:
                    break;

            }

        }

        protected void grdMacroItems_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void grdMacroItemDetails_DataBound(object sender, EventArgs e)
        {
            if (grdMacroItemDetails.Rows.Count > 0)
            {
                var _tabStart = (TextBox)grdMacroItemDetails.Rows[0].Cells[7].Controls[1];
                _tabStart.Attributes.Add("onfocus",
                    "this.select();"
                    );
                var _tabStop =
                    (TextBox)grdMacroItemDetails.Rows[grdMacroItemDetails.Rows.Count - 1].Cells[7].Controls[1];
                _tabStop.Attributes.Add("onblur",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_grdMacroItemDetails_ctl02_txtQuantity').focus();" +
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_grdMacroItemDetails_ctl02_txtQuantity').select();"
                    );
            }
        }

        protected void grdMacroItemDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }


        private void SetDeleteDetailConfirmation(TableRow row)
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

        protected void grdMacroItemDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _txtQuantity = (TextBox)e.Row.Cells[7].FindControl("txtQuantity");
                _txtQuantity.Text = ((MacroItemDetail)e.Row.DataItem).Quantity.ToString();

                var _row = e.Row;
                if (((MacroItemDetail)e.Row.DataItem).PickingItem.Inserted == false)
                {
                    _row.ForeColor = Color.Red;
                }
                m_voicePosition += 1;
                _txtQuantity.TabIndex = m_voicePosition;
                _txtQuantity.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");


                SetDeleteDetailConfirmation(e.Row);

                if (((MacroItemDetail)e.Row.DataItem).PickingItem.IsObsolete(GlobalConfiguration))
                {
                    var _lblDate1 = (Label)e.Row.Cells[12].FindControl("lblDate1");
                    _lblDate1.ForeColor = Color.Red;
                    _lblDate1.ToolTip = string.Format("Il costo di questa voce non è aggiornato da almeno {0} mesi",
                        GlobalConfiguration["PIMU"]);
                }
            }
        }

        protected void grdMacroItemDetails_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
        }

        protected void ldsMacroItemDetails_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            ldsMacroItemDetails.OrderByParameters.Clear();
            ldsMacroItemDetails.AutoGenerateOrderByClause = false;
            var table = ldsMacroItemDetails.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            e.Result =
         _qc.MacroItemDetails.OrderBy(qd => qd.PickingItem.Type.Order)
             .ThenBy(qd => qd.PickingItem.ItemType.Order)
             .ThenBy(qd => qd.PickingItem.Order);


        }

        protected void ldsMacroItemDetails_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
        }

        protected void ldsMacroItems_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
        }

        protected void mnuOperations_MenuItemClick(object sender, MenuEventArgs e)
        {
            using (var QuotationDataContext = new QuotationDataContext())
            {
                IEnumerable<PickingItem> _pickingItems = QuotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = QuotationDataContext.MacroItems;
                var _menuPath = e.Item.Value.Split('.');
                var _curMacroItem =
                    QuotationDataContext.MacroItems.First(
                        MacroItem => MacroItem.ID == Convert.ToInt32(dtvMacroItem.DataKey.Value));
                // PickingItem _toAddPickingItem = _pickingItems.First(pi => pi.ID == Convert.ToInt32(_menuPath[_menuPath.Length - 1]));

                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _cbk.CommonKey);
                    //RecursiveDependenciesAdd(_curMacroItem, _toAddPickingItem, _pickingItems);
                    _curMacroItem.MacroItemDetails.Add(CreateDetail(_curMacroItem, _toAddPickingItem));
                    RecursiveDependenciesAdd(_curMacroItem, _toAddPickingItem, _pickingItems);
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var _toAddMacroItem = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    if (_curMacroItem.ID != _cbk.CommonKey)
                    {
                        foreach (var _toAddMacroItemDetail in _toAddMacroItem.MacroItemDetails)
                        {
                            _curMacroItem.MacroItemDetails.Add(CreateDetail(_curMacroItem, _toAddMacroItemDetail));
                        }
                    }
                }

                //foreach (PickingItem _item in _pickingItems)
                //{
                //    if (_toAddPickingItem.Link != null)
                //    {
                //        try
                //        {
                //            if (_item.ID == Convert.ToInt32(_toAddPickingItem.Link))
                //            {
                //                _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _item));
                //            }
                //        }
                //        catch { }
                //    }
                //}
                //RecursiveDependenciesAdd(_curMacroItem, _toAddPickingItem, _pickingItems);

                //_curMacroItem.MacroItemDetails.Add(CreateDetail(_curMacroItem, _toAddPickingItem));
                QuotationDataContext.SubmitChanges();
            }
        }

        protected void RecursiveDependenciesAdd(MacroItem macroItem, PickingItem toAddPickingItem,
            IEnumerable<PickingItem> pickingItems)
        {
            foreach (var _item in pickingItems)
            {
                if (toAddPickingItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
                        {
                            if (macroItem.MacroItemDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                macroItem.MacroItemDetails.Add(CreateDetail(macroItem, _item));
                                RecursiveDependenciesAdd(macroItem, _item, pickingItems);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected MacroItemDetail CreateDetail(MacroItem macroItem, PickingItem pickingItem)
        {
            var _toAddMacroItemDetail = new MacroItemDetail();
            _toAddMacroItemDetail.ID_MacroItem = macroItem.ID; //Convert.ToInt32(MacroItemParameter);
            //_toAddMacroItemDetail.Cost = pickingItem.Cost;
            //_toAddMacroItemDetail.MarkUp = Convert.ToInt32(MacroItem.MarkUp);
            //try
            //{ _toAddMacroItemDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m) * Convert.ToDecimal(MacroItem.MarkUp / 100m); }
            //catch
            //{ _toAddMacroItemDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m); }
            _toAddMacroItemDetail.Multiply = pickingItem.Multiply;
            //_toAddMacroItemDetail.Percentage = pickingItem.PercentageAuto;
            //_toAddMacroItemDetail.SupplierCode = pickingItem.SupplierCode;
            //_toAddMacroItemDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            //_toAddMacroItemDetail.UM = pickingItem.UM;
            //_toAddMacroItemDetail.ItemTypeDescription = pickingItem.ItemDescription;
            //_toAddMacroItemDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddMacroItemDetail.CommonKey = pickingItem.ID;
            _toAddMacroItemDetail.Position = pickingItem.Order;
            _toAddMacroItemDetail.Inserted = pickingItem.Inserted;

            return _toAddMacroItemDetail;
        }

        protected MacroItemDetail CreateDetail(MacroItem macroItem, MacroItemDetail macroItemDetail)
        {
            var _toAddMacroItemDetail = new MacroItemDetail();
            _toAddMacroItemDetail.ID_MacroItem = macroItem.ID; //Convert.ToInt32(MacroItemParameter);
            //_toAddMacroItemDetail.Cost = pickingItem.Cost;
            //_toAddMacroItemDetail.MarkUp = Convert.ToInt32(MacroItem.MarkUp);
            //try
            //{ _toAddMacroItemDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m) * Convert.ToDecimal(MacroItem.MarkUp / 100m); }
            //catch
            //{ _toAddMacroItemDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m); }
            _toAddMacroItemDetail.Multiply = macroItemDetail.PickingItem.Multiply;
            //_toAddMacroItemDetail.Percentage = pickingItem.PercentageAuto;
            //_toAddMacroItemDetail.SupplierCode = pickingItem.SupplierCode;
            //_toAddMacroItemDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            //_toAddMacroItemDetail.UM = pickingItem.UM;
            //_toAddMacroItemDetail.ItemTypeDescription = pickingItem.ItemDescription;
            //_toAddMacroItemDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddMacroItemDetail.CommonKey = macroItemDetail.PickingItem.ID;
            _toAddMacroItemDetail.Position = macroItemDetail.PickingItem.Order;
            _toAddMacroItemDetail.Inserted = macroItemDetail.PickingItem.Inserted;
            _toAddMacroItemDetail.Quantity = macroItemDetail.Quantity;

            return _toAddMacroItemDetail;
        }

        private void WriteQuantities(QuotationDataContext qc)
        {
            //using (QuotationDataContext qc = new QuotationDataContext())
            //{
            if (mvwMain.ActiveViewIndex == 0)
            {
                for (var i = 0; i < grdMacroItemDetails.Rows.Count; i++)
                {
                    var _txtQuantity = (TextBox)grdMacroItemDetails.Rows[i].Cells[7].FindControl("txtQuantity");
                    var _litID = (Literal)grdMacroItemDetails.Rows[i].Cells[1].FindFieldTemplate("ID").Controls[0];
                    var _toModifyId = Convert.ToInt32(_litID.Text);
                    var _toUpdateMacroItemDetail =
                        qc.MacroItemDetails.Single(MacroItemDetail => MacroItemDetail.ID == _toModifyId);
                    var _quantity = 0f;
                    _txtQuantity.Text = _txtQuantity.Text.Replace('.', ',');
                    if (_txtQuantity.Text.Contains('/'))
                    {
                        var _digits = _txtQuantity.Text.Split('/');
                        var _digitToDivide = 0f;
                        float.TryParse(_digits[0], out _digitToDivide);
                        for (var j = 1; j < _digits.Length; j++)
                        {
                            var _digitDivisor = 0f;
                            float.TryParse(_digits[j], out _digitDivisor);
                            _digitToDivide = _digitToDivide / _digitDivisor;
                        }
                        _quantity = _digitToDivide;
                    }
                    else
                    {
                        float.TryParse(_txtQuantity.Text, out _quantity);
                    }
                    _toUpdateMacroItemDetail.Quantity = _quantity;
                }
            }

            //qc.SubmitChanges();

            //}
        }

        protected void grdMacroItemDetails_PreRender(object sender, EventArgs e)
        {
            grdMacroItemDetails.DataBind();
        }

        protected void lbtToggleView_Click(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = mvwMain.ActiveViewIndex == 1
                ? mvwMain.ActiveViewIndex = 0
                : mvwMain.ActiveViewIndex = 1;
        }

        protected void mvwMain_ActiveViewChanged(object sender, EventArgs e)
        {
            mnuOperations.Enabled = mvwMain.ActiveViewIndex == 0;
            itbNo.Enabled = mvwMain.ActiveViewIndex == 1;
            txtTitleContains.Enabled = mvwMain.ActiveViewIndex == 1;
            ddlCompanies.Enabled = mvwMain.ActiveViewIndex == 1;
            ddlItemTypes.Enabled = mvwMain.ActiveViewIndex == 1;
            ddlTypes.Enabled = mvwMain.ActiveViewIndex == 1;
            ddlOrderBy.Enabled = mvwMain.ActiveViewIndex == 1;
        }


        protected void txtTitleContains_TextChanged(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = 1;
        }

        protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = 1;
        }

        protected void ddlItemTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = 1;
        }

        protected void ddlOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = 1;
        }

        protected void ddlCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {
            mvwMain.ActiveViewIndex = 1;
            SwitchDependingControls(CurMacroItemsConsoleMode);
            LoadHeaderMenu(mnuOperations, MenuType.MenuOperations, int.Parse(ddlCompanies.SelectedValue));
            FillDependingControls(CurMacroItemsConsoleMode);
        }

        protected void grdMacroItems_PreRender(object sender, EventArgs e)
        {
            grdMacroItems.DataBind();
        }

        protected void btnRecalc_Click(object sender, EventArgs e)
        {
        }

        protected void btnUpdateMenu_Click(object sender, EventArgs e)
        {
            Cache.Remove(MenuType.MenuPickingItems.ToString());
            Cache.Remove(MenuType.MenuOperations.ToString());
            Cache.Remove(MenuType.MenuOperationNoPhases.ToString());
            Cache.Remove(MenuType.MenuMaterials.ToString());
            Cache.Remove(MenuType.MenuProdRecord.ToString());
            Cache.Remove(MenuType.MenuQuotationTemplates.ToString());

            Response.Redirect(MacroItemConsolePage);
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }




    }
}