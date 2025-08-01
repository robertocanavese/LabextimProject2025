using System;
using System.Linq;

namespace DLLabExtim
{
    public partial class ProductionMP
    {
        public double ProdTimeMinDouble
        {
            get;
            set;
        }

        public int nOrder
        {
            get
            {
                return Convert.ToInt32(Order);
            }
            set
            {
                Order = value.ToString();
            }
        }

        public decimal dOrder
        {
            get;
            set;
        }


    }
}