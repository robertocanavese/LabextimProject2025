using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationInsert : QuotationController //System.Web.UI.Page
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

            DetailsView1.InsertItem(false);
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
                    //e.Values["Subject"] = "Preventivo Senza Nome No. " + insertedKey.ToString();
                    //e.Values["Date"] = DateTime.Now.Date;

                    //QuotationHeader = new KeyValuePair<int, string>(insertedKey, e.Values["Subject"].ToString());

                    //try
                    //{
                    //    if (Convert.ToInt32(e.Values["CustomerCode"]) <= 0)

                    //        e.Values["CustomerCode"] = -1;
                    //}
                    //catch
                    //{ e.Values["CustomerCode"] = -1; }

                    ////{ throw new Exception(); }
                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.InputFailed);
                    return;
                }
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationKey, insertedKey.ToString().TrimEnd(),
                        "~/quotationconsole.aspx"), true);
            }
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            insertedKey = ((Quotation) e.Result).ID;

            var _subject = ((Quotation) e.Result).Subject;
            QuotationHeader = new KeyValuePair<int, string>(insertedKey, _subject);
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //e.Values["Subject"] = "Preventivo senza nome " + new System.Random(DateTime.Now.Millisecond).Next().ToString();
            e.Values["Subject"] = "Bozza senza nome " + new Random(DateTime.Now.Millisecond).Next();
            e.Values["Date"] = DateTime.Now;
            e.Values["Draft"] = true;
            e.Values["Status"] = 9;

            //QuotationHeader = new KeyValuePair<int, string>(insertedKey, e.Values["Subject"].ToString());

            if (Convert.ToInt32(e.Values["ID_Owner"]) <= 0)
                if (GetCurrentEmployee() != null)
                    e.Values["ID_Owner"] = GetCurrentEmployee().ID;

            try
            {
                if (Convert.ToInt32(e.Values["CustomerCode"]) <= 0)

                    //e.Values["CustomerCode"] = -1;
                    using (var QuotationDataContext = new QuotationDataContext())
                    {
                        var _firstCustomer = QuotationDataContext.Customers.OrderBy(c => c.Name).First();
                        e.Values["CustomerCode"] = _firstCustomer.Code;
                    }
            }
            catch
            {
                e.Values["CustomerCode"] = -1;
            }

            //{ throw new Exception(); }


            if (Convert.ToInt32(e.Values["Q1"]) == 0) e.Values["Q1"] = 100;
            if (Convert.ToInt32(e.Values["Q2"]) == 0) e.Values["Q2"] = 200;
            if (Convert.ToInt32(e.Values["Q3"]) == 0) e.Values["Q3"] = 500;
            if (Convert.ToInt32(e.Values["Q4"]) == 0) e.Values["Q4"] = 1000;
            if (Convert.ToInt32(e.Values["Q5"]) == 0) e.Values["Q5"] = 2000;

            int? _markUp = 0;
            try
            {
                _markUp = Convert.ToInt32(e.Values["MarkUp"]);
                if (_markUp < 120)
                {
                    throw new Exception();
                }
            }
            catch
            {
                try
                {
                    using (var QuotationDataContext = new QuotationDataContext())
                    {
                        var _curCustomersMarkUp =
                            QuotationDataContext.CustomersMarkUps.First(
                                CustomersMarkUp => CustomersMarkUp.Code == Convert.ToInt32(e.Values["CustomerCode"]));
                        _markUp = _curCustomersMarkUp.MarkUp;
                        if (_markUp < 120)
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    _markUp = 120;
                }
            }
            e.Values["MarkUp"] = _markUp;
        }
    }
}