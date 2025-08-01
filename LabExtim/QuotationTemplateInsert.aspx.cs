using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationTemplateInsert : QuotationController //System.Web.UI.Page
    {
        protected int insertedKey;
        protected MetaTable table;

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(DetailsView1);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            table = DetailsDataSource.GetTable();
            Title = table.DisplayName;
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.CancelCommandName)
            {
                //Response.Redirect(table.ListActionPath);
                Response.Redirect(HomePage);
            }
        }

        protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                try
                {
                    QuotationTemplateHeader = new KeyValuePair<int, string>(insertedKey,
                        e.Values["Description"].ToString());
                    //if (Convert.ToInt32(e.Values["CustomerCode"]) <= 0)
                    //{ throw new Exception(); }
                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.InputFailed);
                    return;
                }
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationTemplateKey, insertedKey.ToString().TrimEnd(),
                        QuotationTemplateConsolePage), true);
            }
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            insertedKey = ((QuotationTemplate) e.Result).ID;

            Cache.Remove(MenuType.MenuQuotationTemplates.ToString());
 
 
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {

            e.Values["Inserted"] = true;
            if (Convert.ToInt32(e.Values["Q1"]) == 0) e.Values["Q1"] = 100;
            if (Convert.ToInt32(e.Values["Q2"]) == 0) e.Values["Q2"] = 200;
            if (Convert.ToInt32(e.Values["Q3"]) == 0) e.Values["Q3"] = 500;
            if (Convert.ToInt32(e.Values["Q4"]) == 0) e.Values["Q4"] = 1000;
            if (Convert.ToInt32(e.Values["Q5"]) == 0) e.Values["Q5"] = 2000;

            //int? _markUp = 0;
            //try
            //{
            //    _markUp = Convert.ToInt32(e.Values["MarkUp"]);
            //    if (_markUp <= 0) { throw new Exception(); }
            //}
            //catch
            //{
            //    try
            //    {
            //        using (QuotationDataContext QuotationDataContext = new QuotationDataContext())
            //        {
            //            CustomersMarkUp _curCustomersMarkUp = QuotationDataContext.CustomersMarkUps.First(CustomersMarkUp => CustomersMarkUp.Code == Convert.ToInt32(e.Values["CustomerCode"]));
            //            _markUp = _curCustomersMarkUp.MarkUp;
            //        }
            //    }
            //    catch
            //    {
            //        _markUp = 100;
            //    }
            //}
            //e.Values["MarkUp"] = _markUp;
        }
    }
}