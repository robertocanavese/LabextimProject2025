using System;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim.DynamicData.Content
{
    public partial class GridViewPager : UserControl
    {
        private GridView _gridView;
        private MetaTable _table;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    Control c = Parent;
        //    while (c != null)
        //    {
        //        if (c is GridView)
        //        {
        //            _gridView = (GridView)c;
        //            break;
        //        }
        //        c = c.Parent;
        //    }
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    Control c = Parent;
        //    while (c != null)
        //    {
        //        if (c is GridView)
        //        {
        //            _gridView = (GridView)c;

        //            // if pager size saved then restore it
        //            var pagerSize = Session["DD_PagerSize"];
        //            if (_gridView != null && pagerSize != null)
        //            {
        //                _gridView.PageSize = Convert.ToInt32(pagerSize);
        //            }

        //            break;
        //        }
        //        c = c.Parent;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            var c = Parent;
            while (c != null)
            {
                if (c is GridView)
                {
                    _gridView = (GridView) c;

                    // if pager size saved then restore it
                    var GridDataSource = _gridView.FindDataSourceControl();
                    _table = GridDataSource.GetTable();

                    if (_table != null)
                    {
                        if (Session[_table.Name + "_PagerSize"] == null)
                        {
                            if (_table.Name == "Customers" || _table.Name == "Suppliers" || _table.Name == "Quotations" ||
                                _table.Name == "ProductionOrders" || _table.Name == "VW_QUOPORCostsPrices" || _table.Name == "VW_PlasticCoatingMachineStats_news")
                            {
                                DropDownListPageSize.Items.FindByText("30").Selected = true;
                                Session[_table.Name + "_PagerSize"] = 30;
                            }
                            else
                            {
                                DropDownListPageSize.Items.FindByText("Tutte").Selected = true;
                                Session[_table.Name + "_PagerSize"] = short.MaxValue;
                            }
                        }


                        var pagerSize = Session[_table.Name + "_PagerSize"];
                        if (_gridView != null && pagerSize != null)
                        {
                            _gridView.PageSize = Convert.ToInt32(pagerSize);
                        }
                    }


                    break;
                }
                c = c.Parent;
            }
        }

        protected void TextBoxPage_TextChanged(object sender, EventArgs e)
        {
            if (_gridView == null)
            {
                return;
            }
            int page;
            if (int.TryParse(TextBoxPage.Text.Trim(), out page))
            {
                if (page <= 0)
                {
                    page = 1;
                }
                if (page > _gridView.PageCount)
                {
                    page = _gridView.PageCount;
                }
                _gridView.PageIndex = page - 1;
            }
            TextBoxPage.Text = (_gridView.PageIndex + 1).ToString();
        }

        //protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_gridView == null)
        //    {
        //        return;
        //    }
        //    DropDownList dropdownlistpagersize = (DropDownList)sender;

        //    if (dropdownlistpagersize.SelectedValue != "Tutte")
        //        _gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue);
        //    else
        //        _gridView.PageSize = short.MaxValue;

        //    int pageindex = _gridView.PageIndex;
        //    _gridView.DataBind();
        //       if (_gridView.PageIndex != pageindex)
        //    {
        //        //if page index changed it means the previous page was not valid and was adjusted. Rebind to fill control with adjusted page
        //        _gridView.DataBind();
        //    }
        //}

        //protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (_gridView == null)
        //        return;

        //    DropDownList dropdownlistpagersize = (DropDownList)sender;
        //    _gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue);

        //    // save pager size
        //    if (Session["DD_PagerSize"] != null)
        //        Session["DD_PagerSize"] = _gridView.PageSize.ToString();
        //    else
        //        Session.Add("DD_PagerSize", _gridView.PageSize.ToString());

        //    int pageindex = _gridView.PageIndex;
        //    _gridView.DataBind();
        //    if (_gridView.PageIndex != pageindex)
        //    {
        //        //if page index changed it means the previous page was not valid and was adjusted. Rebind to fill control with adjusted page
        //        _gridView.DataBind();
        //    }
        //}

        protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gridView == null)
                return;

            var dropdownlistpagersize = (DropDownList) sender;
            //_gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue);
            if (dropdownlistpagersize.SelectedValue != "Tutte")
                _gridView.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue);
            else
                _gridView.PageSize = short.MaxValue;

            // save pager size
            if (Session[_table.Name + "_PagerSize"] != null)
                Session[_table.Name + "_PagerSize"] = _gridView.PageSize.ToString();
            else
                Session.Add(_table.Name + "_PagerSize", _gridView.PageSize.ToString());

            var pageindex = _gridView.PageIndex;
            _gridView.DataBind();
            if (_gridView.PageIndex != pageindex)
            {
                // if page index changed it means the previous page was page 
                // not valid and was adjusted. Rebind to fill control with adjusted
                _gridView.DataBind();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_gridView != null)
            {
                LabelNumberOfPages.Text = _gridView.PageCount.ToString();
                TextBoxPage.Text = (_gridView.PageIndex + 1).ToString();
                DropDownListPageSize.SelectedValue = _gridView.PageSize.ToString();
            }
        }
    }
}