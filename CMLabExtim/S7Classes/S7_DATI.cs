using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CMLabExtim.S7Classes
{
    public class S7_DATI
    {

        public UInt32 OdP { get; set; }

        public UInt32 CopieDaProdurre { get; set; }

        private string titoloOdP;
        public string TitoloOdp
        {
            get { return titoloOdP; }
            set
            {
                titoloOdP = (
                    (value == null || value.Length <= 30)
              ? value
              : value.Substring(0, 30)
              );
            }
        }

        public UInt32 CopieProdotte { get; set; }

        public UInt32 MetriLineariLavorati { get; set; }

        public UInt32 OreMinutiOreMacchinaAccesa { get; set; }
        public UInt32 OreMinutiOreMacchinaInPassaggio { get; set; }
        public UInt32 OreMinutiOreMacchinaInPressa { get; set; }

        public UInt32 OreMinutiMinutiMacchinaAccesa { get; set; }
        public UInt32 OreMinutiMinutiMacchinaInPassaggio { get; set; }
        public UInt32 OreMinutiMinutiMacchinaInPressa { get; set; }

        public UInt32 MinutiMacchinaAccesa { get; set; }
        public UInt32 MinutiMacchinaInPassaggio { get; set; }
        public UInt32 MinutiMacchinaInPressa { get; set; }

    }

}
