using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMLabExtim
{
    [Serializable]
    public class OdPBag
    {

        public int Id { get; set; }
        public int CopieRichieste { get; set; }
        public int CopieLavorate { get; set; }
        public decimal PercLavorata {
            get
            {
                if (CopieRichieste > 0)
                    return Convert.ToDecimal(CopieLavorate) / Convert.ToDecimal(CopieRichieste);
                else
                    return 0;
            }
        }

    }
}
