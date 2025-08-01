using System.Linq;

namespace DLLabExtim
{
    public partial class CustomerOrder
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
            get { return ProductionOrders.Sum(p => p.QuantityOver); }
        }
    }
}