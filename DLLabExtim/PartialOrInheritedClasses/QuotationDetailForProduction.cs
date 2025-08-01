namespace DLLabExtim
{
    public class QuotationDetailForProduction
    {
        public QuotationDetailForProduction()
        {
        }

        public QuotationDetailForProduction(QuotationDetail quotationDetail, PickingItem pickingItem)
        {
            QuotationDetail = quotationDetail;
            PickingItem = pickingItem;
            Quantity = 0;
        }

        public QuotationDetail QuotationDetail { get; set; }
        public PickingItem PickingItem { get; set; }
        public decimal Quantity { get; set; }
    }
}