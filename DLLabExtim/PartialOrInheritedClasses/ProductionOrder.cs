using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DLLabExtim
{
    public partial class ProductionOrder
    {
        public float? OutstandingQuantity
        {
            get
            {
                if (Quantity != null)
                {
                    return Quantity - QuantityOver;
                }
                return null;
            }
        }

        public float? QuantityOver
        {
            get { return ProductionOrderDetails.Where(p => p.QuantityOver).Sum(p => p.ProducedQuantity); }
        }

        partial void OnStatusChanging(int value)
        {
            if (value == 3)
            {
                using (var _ctx = new QuotationDataContext())
                {
                    if (_ctx.ProductionOrders.Where(po => po.Quotation.ID == ID_Quotation && po.Statuse.ID != 3).Count() <= 1)
                    {
                        _ctx.Quotations.SingleOrDefault(q => q.ID == ID_Quotation).Status = value;
                        _ctx.SubmitChanges();
                    }
                }
            }
        }

        public string NoteFromProduction
        {
            get;
            set;
        }

        public string AccountNoteFromProduction
        {
            get;
            set;
        }

        
    }
}