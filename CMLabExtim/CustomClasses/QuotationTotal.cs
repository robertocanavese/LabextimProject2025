using System;

namespace CMLabExtim.CustomClasses
{
    public class QuotationTotal
    {
        public int Quantity { get; set; }
        public decimal CstTot { get; set; }
        public decimal CstUni { get; set; }
        public decimal CstPTot { get; set; }
        public decimal CstPUni { get; set; }
        public decimal PrcTot { get; set; }
        public decimal PrcUni { get; set; }
        public decimal CstToolsTot { get; set; }
        public decimal CstToolsUni { get; set; }
        public decimal CstToolsPTot { get; set; }
        public decimal CstToolsPUni { get; set; }
        public decimal PrcToolsTot { get; set; }
        public decimal PrcToolsUni { get; set; }

        public decimal CstProdTot
        {
            get { return CstTot - CstToolsTot; }
        }

        public decimal CstProdUni
        {
            get { return CstUni - (Quantity == 0 ? CstToolsTot : CstToolsTot/Convert.ToDecimal(Quantity)); }
        }

        //public decimal PrcProdTot
        //{
        //    get { return PrcTot - PrcToolsTot; }
        //}

        //public decimal PrcProdUni
        //{
        //    get { return PrcUni - (Quantity == 0 ? PrcToolsTot : PrcToolsTot/Convert.ToDecimal(Quantity)); }
        //}


        public decimal PrcProdTot
        {
            get { return CstPTot - CstToolsPTot; }
        }

        public decimal PrcProdUni
        {
            get { return CstPUni - (Quantity == 0 ? CstToolsPTot : CstToolsPTot / Convert.ToDecimal(Quantity)); }
        }

    }
}