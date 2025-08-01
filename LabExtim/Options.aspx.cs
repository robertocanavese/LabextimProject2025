using System;
using UILabExtim;
using DLLabExtim;
using CMLabExtim.S7Classes;


namespace LabExtim
{
    public partial class Options : OptionsController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnExecute_Click(object sender, EventArgs e)
        {
            DateTime _tempDateFrom;
            DateTime _tempDateTo;
            if (DateTime.TryParse(txtDateFrom.Text, out _tempDateFrom) &&
                DateTime.TryParse(txtDateTo.Text, out _tempDateTo))
                if (_tempDateTo >= _tempDateFrom)
                    RecalcProductionOrderDetails(_tempDateFrom, _tempDateTo);
        }

        protected void btnMPSRecalc_Click(object sender, EventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderService.RecalcMPS(db, Global.CurrentSchedulingType);
            }
        }

        protected void btnMenuUpdate_Click(object sender, EventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {

                foreach (Company cp in db.Companies)
                {
                    Cache.Remove(cp.ID + "|" + MenuType.MenuPickingItems.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperations.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperationNoPhases.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuMaterials.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuProdRecord.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuQuotationTemplates.ToString());
                }
            }
            Cache.Remove("PickingItems");
            Cache.Remove("MacroItems");
        }

        protected void btnStatsRecalc_Click(object sender, EventArgs e)
        {
            DateTime _tempDateFrom;
            if (DateTime.TryParse(txtDate1From.Text, out _tempDateFrom))
            {
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ProductionOrderService.RecalcVW_QUOPORCostsPrices(db, _tempDateFrom.ToString("yyyyMMdd"));
                }
            }
        }


        protected void btnTest_Click(object sender, EventArgs e)
        {

#if DEBUG
            using (QuotationDataContext db = new QuotationDataContext())
            {
                //Snap7Gateway gtw = new Snap7Gateway();
                //gtw.SetOdPDataToSilkFoil1(db, 62491, 1000, "Silk foil per biglietti augura");
                //db.SubmitChanges();
                //gtw.GetCurrentDataFromSilkFoil1(db, 44352, 100, "TEST SILK FOIL PER TECNOFOIL");
                //db.SubmitChanges();
                //gtw.UpdateLastDataFromSilkFoil1(db);
                //db.SubmitChanges();
                //ZechiniGateway gtw = new ZechiniGateway();
                //gtw.ReceiveDataFromFtp();
                //gtw.SendNewDataset();

                //Snap7Gateway gtw = new Snap7Gateway();
                ////gtw.GetPauseSignal();
                ////gtw = new Snap7Gateway();
                //gtw.SetPauseSignal(62491, false);
                //gtw = new Snap7Gateway();
                //gtw.GetPauseSignal();
                Snap7Gateway.GetCurOdP();

            }
#endif

        }

    }
}