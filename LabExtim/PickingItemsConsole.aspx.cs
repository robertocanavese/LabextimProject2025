using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class PickingItemsConsole : QuotationController
    {
        protected List<SPPickingItemCalculated> m_calculated;

        protected Mode CurPickingItemsConsoleMode
        {
            get
            {
                if (ViewState["CurPickingItemsConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode)ViewState["CurPickingItemsConsoleMode"];
            }
            set { ViewState["CurPickingItemsConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdPickingItems);

        }

        public void SetFilter()
        {

            GridDataSource.AutoGenerateWhereClause = false;

            GridDataSource.WhereParameters.Clear();
            var _filter = "TRUE ";


            if (itbNo.ReturnValue != 0)
            {
                GridDataSource.WhereParameters.Add("ID", DbType.Int32, itbNo.ReturnValue.ToString());
                _filter += " AND ID = @ID";
            }

            if (txtTitleContains.Text != string.Empty)
            {
                GridDataSource.WhereParameters.Add("ItemDescription", DbType.String, txtTitleContains.Text);
                _filter += " AND ItemDescription.Contains(@ItemDescription)";
            }

            if (ddlCompanies.SelectedValue != string.Empty)
            {
                GridDataSource.WhereParameters.Add("ID_Company", DbType.Int32, ddlCompanies.SelectedValue);
                _filter += " AND ID_Company == @ID_Company";
            }

            if (ddlTypes.SelectedValue != string.Empty)
            {
                GridDataSource.WhereParameters.Add("TypeCode", DbType.Int32, ddlTypes.SelectedValue);
                _filter += " AND TypeCode == @TypeCode";
            }
            if (ddlItemTypes.SelectedValue != string.Empty)
            {
                GridDataSource.WhereParameters.Add("ItemTypeCode", DbType.Int32, ddlItemTypes.SelectedValue);
                _filter += " AND ItemTypeCode == @ItemTypeCode";
            }

            if (ddlSuppliers.SelectedValue != string.Empty)
            {
                GridDataSource.WhereParameters.Add("SupplierCode", DbType.Int32, ddlSuppliers.SelectedValue);
                _filter += " AND SupplierCode == @SupplierCode";
            }

            //GridDataSource.WhereParameters.Add("Inserted", DbType.Boolean, "");
            if (CurPickingItemsConsoleMode == Mode.InputItems)
            {
                GridDataSource.WhereParameters.Add("Inserted", DbType.Boolean, true.ToString());
                _filter += " AND Inserted == @Inserted";
            }
            else if (CurPickingItemsConsoleMode == Mode.DeactivatedItems)
            {
                GridDataSource.WhereParameters.Add("Inserted", DbType.Boolean, false.ToString());
                _filter += " AND Inserted == @Inserted";
            }
            else
            {
                _filter.Replace(" AND Inserted == @Inserted", "");
            }

            GridDataSource.Where = _filter;

        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FillControls();
                SwitchDependingControls(CurPickingItemsConsoleMode);
                //SetFilter();
            }
            FillDependingControls(CurPickingItemsConsoleMode);



        }

        private void FillControls()
        {
            ddlOrderBy.Items.Add(new ListItem("Ordine", ""));
            ddlOrderBy.Items.Add(new ListItem("Ordine Tipo, Ordine Tipo voce, Ordine + Costo(descrescente) + Descrizione", "TipoTipoVoceOrdine"));
            ddlOrderBy.Items.Add(new ListItem("Descrizione voce", "ItemDescription"));
            ddlOrderBy.Items.Add(new ListItem("Fornitore", "SupplierName"));
            ddlOrderBy.Items.Add(new ListItem("Data creazione", "Date"));

            ddlOrderBy.DataBind();

            lbtNew.Attributes.Add("onclick",
                "javascript:OpenItem('PickingItemPopup.aspx?ID=-1');return false;");

            if (Session["PickingItemsTxtTitleContainsSelector"] != null)
                txtTitleContains.Text = Session["PickingItemsTxtTitleContainsSelector"].ToString();

        }

        protected void FillDependingControls(Mode mode)
        {
                using (var _qc = new QuotationDataContext())
                {
                    m_calculated = _qc.prc_LAB_MGet_LAB_PickingItems().ToList();
                }

        }

        protected void GetModeDescription(Mode mode)
        {
            switch (mode)
            {
                case Mode.InputItems:
                    lblModeDescription.Text = "(gestione voci attive)";
                    break;
                case Mode.Calculation:
                    lblModeDescription.Text = "(prospetto calcolato)";
                    break;
                case Mode.DeactivatedItems:
                    lblModeDescription.Text = "(gestione voci disattivate)";
                    break;
                default:
                    lblModeDescription.Text = "";
                    break;
            }
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            grdPickingItems.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            grdCalculated.Visible = mode == Mode.Calculation;
            lblddlOrderBy.Visible = (mode != Mode.Calculation);
            ddlOrderBy.Visible = (mode != Mode.Calculation);
            CurPickingItemsConsoleMode = mode;
            GetModeDescription(CurPickingItemsConsoleMode);
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = true.ToString();
            grdPickingItems.AutoGenerateDeleteButton = false;
            SwitchDependingControls(Mode.InputItems);
            SetFilter();
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewCalculated_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = "";
            SwitchDependingControls(Mode.Calculation);
            SetFilter();
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewDeactivatedItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = false.ToString();
            grdPickingItems.AutoGenerateDeleteButton = true;
            SwitchDependingControls(Mode.DeactivatedItems);
            SetFilter();
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void grdCalculated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCalculated.PageIndex = e.NewPageIndex;
            SwitchDependingControls(Mode.Calculation);
        }

        protected void grdPickingItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPickingItems.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurPickingItemsConsoleMode);
        }

        protected void CalculatedDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //e.Result = m_calculated;
            if (ddlOrderBy.SelectedValue == "")
            {
                CalculatedDataSource.OrderByParameters.Clear();
                CalculatedDataSource.AutoGenerateOrderByClause = false;
                var table = CalculatedDataSource.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result =
                    m_calculated.OrderBy(pi => pi.TypeCode)
                        .ThenBy(pi => pi.ItemTypeCode)
                        .ThenBy(pi => pi.ItemDescription);
                //.Join<PickingItem, DLLabExtim.Type, int, PickingItem>(_qc.Types, pi => pi.TypeCode, o => o.Code, (o, e2) => o)
                //.Join<PickingItem, DLLabExtim.ItemType, int, PickingItem>(_qc.ItemTypes, pi => pi.ItemTypeCode, o => o.Code, (o, e2) => o);
            }
        }

        protected void lbtPrintPickingItems_Click(object sender, EventArgs e)
        {
            var _selectionFormula = string.Empty;

            _selectionFormula += itbNo.Text == ""
                ? ""
                : " AND {VW_PickingItems.ID} = " + int.Parse(itbNo.ReturnValue.ToString());

            _selectionFormula += (ddlCompanies.SelectedValue == ""
               ? ""
               : " AND {VW_PickingItems.ID_Company} = " + ddlCompanies.SelectedValue);

            _selectionFormula += (txtTitleContains.Text == ""
                ? ""
                : " AND {VW_PickingItems.ItemTypeDescription} like '%" + txtTitleContains.Text + "%'");

            _selectionFormula += (ddlTypes.SelectedValue == ""
                ? ""
                : " AND {VW_PickingItems.TypeCode} = " + ddlTypes.SelectedValue);

            _selectionFormula += (ddlItemTypes.SelectedValue == ""
                ? ""
                : " AND {VW_PickingItems.ItemTypeCode} = " + ddlItemTypes.SelectedValue);

            _selectionFormula = _selectionFormula.Length > 0 ? _selectionFormula.Remove(0, 4) : string.Empty;

            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}", GenericReportKey, "PickingItems", GenericPrintPage,
                    SelectionFormulaKey, _selectionFormula), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdPickingItems.EditIndex = -1;
            grdPickingItems.PageIndex = 0;
        }

        protected void ddlCompanies_DataBound(object sender, EventArgs e)
        {
            ddlCompanies.Items.Insert(0, new ListItem("Tutte", ""));
            if (Session["PickingItemsCompanyIDsSelector"] != null)
            {
                ddlCompanies.Items.FindByValue(Session["PickingItemsCompanyIDsSelector"].ToString()).Selected = true;
                SetFilter();
                grdPickingItems.DataBind();
            }
        }

        protected void ddlTypes_DataBound(object sender, EventArgs e)
        {
            ddlTypes.Items.Insert(0, new ListItem("Tutti", ""));

            if (Session["PickingItemsTypesSelector"] != null)
            {
                ddlTypes.Items.FindByValue(Session["PickingItemsTypesSelector"].ToString()).Selected = true;
                SetFilter();
                grdPickingItems.DataBind();
            }
            else
            {
                if (Session["PickingItemsIdSelector"] == null)
                    using (var _qc = new QuotationDataContext())
                    {
                        var _firstType = _qc.Types.Where(t => t.Category == "I").OrderBy(t => t.Order).FirstOrDefault();
                        ddlTypes.Items.FindByValue(_firstType.Code.ToString()).Selected = true;
                        SetFilter();
                        grdPickingItems.DataBind();
                    }
            }
        }

        protected void ddlItemTypes_DataBound(object sender, EventArgs e)
        {
            ddlItemTypes.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["PickingItemsItemTypesSelector"] != null)
            {
                ddlItemTypes.Items.FindByValue(Session["PickingItemsItemTypesSelector"].ToString()).Selected = true;
                SetFilter();
                grdPickingItems.DataBind();
            }
        }

        protected void ddlSuppliers_DataBound(object sender, EventArgs e)
        {
            ddlSuppliers.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["PickingItemsSuppliersSelector"] != null)
            {
                ddlSuppliers.Items.FindByValue(Session["PickingItemsSuppliersSelector"].ToString()).Selected = true;
                SetFilter();
                grdPickingItems.DataBind();
            }
        }

        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["PickingItemsOrderBySelector"] != null)
            {
                ddlOrderBy.Items.FindByValue(Session["PickingItemsOrderBySelector"].ToString()).Selected = true;
                SetFilter();
                grdPickingItems.DataBind();
            }
        }

        protected void GridDataSource_Selected(object sender, LinqDataSourceStatusEventArgs e)
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
            //if (grdPickingItems.Rows.Count == 0 && grdPickingItems.PageIndex == 0)
            //{
            //    dtvPickingItem.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdPickingItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                var _row = e.Row;
                if (((PickingItem)e.Row.DataItem).Inserted == false)
                {
                    _row.ForeColor = Color.Red;
                }

                var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('PickingItemPopup.aspx?ID=" + ((PickingItem)e.Row.DataItem).ID + "')");

                if (((PickingItem)e.Row.DataItem).IsObsolete(GlobalConfiguration))
                {
                    var _dycDate = (DynamicControl)e.Row.Cells[15].FindControl("dycDate");
                    ((DataControlFieldCell)(_dycDate.Parent)).ForeColor = Color.Red;
                    ((DataControlFieldCell)(_dycDate.Parent)).ToolTip =
                        string.Format("Il costo di questa voce non è aggiornato da almeno {0} mesi",
                            GlobalConfiguration["PIMU"]);
                }

                if (!string.IsNullOrEmpty(((PickingItem)e.Row.DataItem).Link))
                {
                    var _dycLink = (DynamicControl)e.Row.Cells[16].FindControl("dycLink");
                    ((DataControlFieldCell)(_dycLink.Parent)).ToolTip = GetPickingItemDescription(((PickingItem)e.Row.DataItem).Link);

                }
                if (!string.IsNullOrEmpty(((PickingItem)e.Row.DataItem).MILink))
                {
                    var _dycMILink = (DynamicControl)e.Row.Cells[17].FindControl("dycMILink");
                    ((DataControlFieldCell)(_dycMILink.Parent)).ToolTip = GetMacroItemDescription(((PickingItem)e.Row.DataItem).MILink);

                }

            }
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {


            //if (!IsPostBack)
                //SetFilter();

            var table = GridDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();


            switch (ddlOrderBy.SelectedValue)
            {


                case ("TipoTipoVoceOrdine"):
                    e.Result = _qc.PickingItems.OrderBy(pi => pi.Type.Order)
                        .ThenBy(pi => pi.ItemType.Order)
                        .ThenBy(pi => pi.Order)
                        .ThenByDescending(pi => pi.Cost).ThenBy(pi => pi.ItemDescription);
                    break;

                case ("ItemDescription"):
                    e.Result = _qc.PickingItems.OrderBy(pi => pi.ItemDescription);
                    break;

                case ("SupplierName"):
                    e.Result = _qc.PickingItems.OrderBy(qt => qt.Supplier.Name);
                    break;

                case ("Date"):
                    e.Result = _qc.PickingItems.OrderBy(qt => qt.Date);
                    break;

                case (""):
                    e.Result = _qc.PickingItems.OrderBy(qt => qt.Order);
                    break;

                default:
                    break;
            }

            if (chkObsolete.Checked)
                e.Result = ((IEnumerable<PickingItem>)e.Result).Where(d => d.IsObsolete(GlobalConfiguration));
        }

        protected void grdPickingItems_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            SetFilter();
            Session["PickingItemsIdSelector"] = itbNo.Text;
            Session["PickingItemsTxtTitleContainsSelector"] = txtTitleContains.Text;
            Session["PickingItemsCompanyIDsSelector"] = ddlCompanies.SelectedValue;
            Session["PickingItemsTypesSelector"] = ddlTypes.SelectedValue;
            Session["PickingItemsItemTypesSelector"] = ddlItemTypes.SelectedValue;
            Session["PickingItemsSuppliersSelector"] = ddlSuppliers.SelectedValue;
            Session["PickingItemsOrderBySelector"] = ddlOrderBy.SelectedValue;

            //DropDownList curDdl = sender as DropDownList;
            //if (sender as DropDownList != null)
            //{
            //    if (curDdl.ID == "ddlOrderBy")
            //    {
            //        grdPickingItems.DataBind();
            //    }
            //}
            grdPickingItems.DataBind();

        }

        protected void grdPickingItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                grdPickingItems.DataBind();
            }
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }


    }


}