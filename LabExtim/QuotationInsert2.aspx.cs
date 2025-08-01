using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationInsert2 : QuotationController //System.Web.UI.Page
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
                        TempQuotationConsolePage), true);
            }
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            insertedKey = ((TempQuotation)e.Result).ID_Quotation;

            var _subject = ((TempQuotation)e.Result).Subject;
            QuotationHeader = new KeyValuePair<int, string>(insertedKey, _subject);
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //string _uniqueId = new System.Random(DateTime.Now.Millisecond).Next().ToString();

            e.Values["ID_Quotation"] = GetTempQuotationUniqueId();
            e.Values["ID_Company"] = CurrentCompanyId;
            // Convert.ToInt32("-" + _uniqueId.Substring(_uniqueId.Length - 5));
            e.Values["SessionUser"] = GetCurrentEmployee().ID;
            //e.Values["Subject"] = "Preventivo senza nome " + new System.Random(DateTime.Now.Millisecond).Next().ToString();
            e.Values["Subject"] = "Nuovo preventivo senza nome " + e.Values["ID_Quotation"];
            e.Values["Date"] = DateTime.Now;
            e.Values["Draft"] = false;
            e.Values["Status"] = 9;
            e.Values["ID_Manager"] = 1;

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
                        Customer firstCustomer = null;

                        if (CurrentCompanyId == 1)
                            firstCustomer = QuotationDataContext.Customers.Where(c =>  c.Code < 199999999).OrderBy(c => c.Name).First();
                        if (CurrentCompanyId == 2)
                            firstCustomer = QuotationDataContext.Customers.Where(c =>  c.Code > 200000000 && c.Code < 299999999).OrderBy(c => c.Name).First();

                        e.Values["CustomerCode"] = firstCustomer.Code;
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

            e.Values["P1"] = false;
            e.Values["P2"] = false;
            e.Values["P3"] = false;
            e.Values["P4"] = false;
            e.Values["P5"] = false;

            int? _markUp = 0;
            //try
            //{
            //    _markUp = Convert.ToInt32(e.Values["MarkUp"]);
            //    if (_markUp < 110)
            //    {
            //        throw new Exception();
            //    }
            //}
            //catch
            //{
            try
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _curCustomersMarkUp =
                        QuotationDataContext.CustomersMarkUps.First(
                            CustomersMarkUp => CustomersMarkUp.Code == Convert.ToInt32(e.Values["CustomerCode"]));
                    _markUp = _curCustomersMarkUp.MarkUp;
                    if (_markUp < 110)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                _markUp = 110;
            }
            //}
            e.Values["MarkUp"] = _markUp;
        }
    }
}