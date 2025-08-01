using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class PickingItemsPopup : QuotationController
    {
        //protected MetaTable table;

        protected List<SPPickingItem> m_pickingItems;
        //protected new string QuotationConsolePage = "QuotationConsole.aspx";

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

        //public string MenuParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[MenuKey];
        //        return temp == null ? string.Empty : temp.ToString();
        //    }
        //}

        public string TypeParameter
        {
            get
            {
                object temp = Request.QueryString["Q"];
                return temp == null ? string.Empty : temp.ToString();
            }
        }

        //public string ItemTypeParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString["IT"];
        //        return temp == null ? string.Empty : temp.ToString();
        //    }
        //}

        //public string ItemManufacturingParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString["IM"];
        //        return temp == null ? string.Empty : temp.ToString();
        //    }
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var _qc = new QuotationDataContext();
            m_pickingItems =
                _qc.prc_LAB_MGet_LAB_PickingItemByTy_ITy_IMa_Tp(Convert.ToInt32(TypeParameter)).ToList();
            //GridDataSource.GetTable();
            //m_pickingItems = _qc.PickingItems.Where(pi => pi.TypeCode = <TypeCode> && pi.ItemTypeCode = <ItemTypeCode>);
            //Title = table.DisplayName;

            //string filterString = "1=1";
            //if (TypeParameter != string.Empty)
            //{
            //    string[] values = TypeParameter.Split(',');
            //    filterString += " and (";
            //    foreach (string value in values)
            //    { filterString += "Type = " + '"' + value + '"' + " or "; }

            //    filterString = filterString.Substring(0, filterString.Length - 3);
            //    filterString += ")";
            //    //GridDataSource.WhereParameters.Add("Type", DbType.String, TypeParameter);
            //}
            //if (ItemTypeParameter != string.Empty)
            //{
            //    string[] values = ItemTypeParameter.Split(',');
            //    filterString += " and (";
            //    foreach (string value in values)
            //    { filterString += "ItemType = " + '"' + value + '"' + " or "; }

            //    filterString = filterString.Substring(0, filterString.Length - 3);
            //    filterString += ")";
            //    //GridDataSource.WhereParameters.Add("Type", DbType.String, TypeParameter);
            //}
            //if (ItemManufacturingParameter != string.Empty)
            //{
            //    string[] values = ItemManufacturingParameter.Split(',');
            //    filterString += " and (";
            //    foreach (string value in values)
            //    { filterString += "ItemManufacturing = " + '"' + value + '"' + " or "; }

            //    filterString = filterString.Substring(0, filterString.Length - 3);
            //    filterString += ")";
            // GridDataSource.WhereParameters.Add("Type", DbType.String, TypeParameter);
            //}


            //GridDataSource.AutoGenerateWhereClause = false;

            //InsertHyperLink.NavigateUrl = table.GetActionPath(PageAction.Insert);

            //GridView1.DataSource = _selected;
            //DataBind();

            // Disable various options if the table is readonly
            //if (table.IsReadOnly)
            //{
            //    GridView1.Columns[0].Visible = false;
            //    InsertHyperLink.Visible = false;
            //}
        }

        //protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridView1.PageIndex = 0;
        //}

        protected void lbtAddToQuotation_Click(object sender, EventArgs e)
        {
            //using (QuotationDataContext QuotationDataContext =
            //    new QuotationDataContext(ConfigurationManager.ConnectionStrings["LabeConnectionString"].ConnectionString))
            using (var QuotationDataContext =
                new QuotationDataContext())
            {
                var _curQuotation =
                    QuotationDataContext.Quotations.First(
                        Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                for (var i = 0; i < GridView1.Rows.Count; i++)
                {
                    var _ck = (CheckBox) GridView1.Rows[i].Cells[0].FindControl("chkInserted");
                    //DynamicControl _dc = (DynamicControl)GridView1.Rows[i].Cells[1].FindControl("dynID");
                    //int _toAddId = Convert.ToInt32(((Literal)_dc.FieldTemplate.Controls[0]).Text);
                    var _toAddId = m_pickingItems[i].ID;

                    if (_ck.Checked)
                    {
                        //PickingItem _toAddPickingItem = QuotationDataContext.PickingItems.First(PickingItem => PickingItem.ID == _toAddId);
                        var _toAddPickingItem =
                            m_pickingItems.First(SPPickingItem => SPPickingItem.ID == _toAddId);
                        //QuotationDetail _toAddQuotationDetail = new QuotationDetail();
                        //_toAddQuotationDetail.ID_Quotation = Convert.ToInt32(QuotationParameter);
                        //_toAddQuotationDetail.Cost = _toAddPickingItem.Cost;
                        //_toAddQuotationDetail.Price = _toAddPickingItem.Price;
                        //_toAddQuotationDetail.Multiply = _toAddPickingItem.Multiply;
                        //_toAddQuotationDetail.Percentage = _toAddPickingItem.Percentage;
                        //_toAddQuotationDetail.SupplierCode = _toAddPickingItem.SupplierCode;
                        //_toAddQuotationDetail.TypeCode = Convert.ToInt32(TypeParameter);
                        //_toAddQuotationDetail.UM = _toAddPickingItem.UM;
                        //_toAddQuotationDetail.ItemTypeDescription = _toAddPickingItem.ItemDescription;
                        //_toAddQuotationDetail.ItemTypeCode = _toAddPickingItem.ItemTypeCode;
                        //_toAddQuotationDetail.CommonKey = _toAddPickingItem.ID;

                        foreach (var _item in m_pickingItems)
                        {
                            //if (_item.Link != null && _toAddPickingItem.Dependent != null)
                            //{
                            //    if (_item.Link.Trim() == _toAddPickingItem.Dependent.Trim())
                            //    {
                            //        _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _item));
                            //    }
                            //}
                            if (_toAddPickingItem.Link != null)
                            {
                                if (_item.ID == Convert.ToInt32(_toAddPickingItem.Link))
                                {
                                    _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _item));
                                }
                            }
                        }

                        _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                        _ck.Checked = false;
                    }
                }
                QuotationDataContext.SubmitChanges();
                Response.Redirect(string.Format("{2}?{0}={1}", QuotationKey, QuotationParameter, QuotationConsolePage),
                    true);
            }
            //QuotationDataContext.Dispose();
            //Session["QuotationDataContext"] = null;          
            //Response.Write("<script> window.close(); window.opener.location.reload(); return false;</script>");
            //Page.RegisterStartupScript("","<script> window.close() ;return false;</script>");
        }

        protected QuotationDetail CreateDetail(Quotation quotation, SPPickingItem pickingItem)
        {
            var _toAddQuotationDetail = new QuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = pickingItem.Cost;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            try
            {
                _toAddQuotationDetail.Price = pickingItem.Price*Convert.ToDecimal(quotation.MarkUp/100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.Price;
            }
            _toAddQuotationDetail.Multiply = pickingItem.Multiply;
            _toAddQuotationDetail.Percentage = pickingItem.Percentage;
            _toAddQuotationDetail.SupplierCode = pickingItem.SupplierCode;
            _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            _toAddQuotationDetail.UM = pickingItem.UM;
            _toAddQuotationDetail.ItemTypeDescription = pickingItem.ItemDescription;
            _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddQuotationDetail.CommonKey = pickingItem.ID;
            _toAddQuotationDetail.Position = pickingItem.Order;
            _toAddQuotationDetail.Inserted = pickingItem.Inserted;

            return _toAddQuotationDetail;
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = m_pickingItems;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _boundItem = (SPPickingItem) e.Row.DataItem;
                if (_boundItem.Link != null && _boundItem.Dependent == null)
                {
                    var _ck = (CheckBox) e.Row.Cells[0].FindControl("chkInserted");
                    _ck.Enabled = false;
                }
            }
        }
    }
}