using System;
using CMLabExtim;

namespace DLLabExtim
{
    public partial class VW_ProductionExtMP
    {
        public string SemaphoreImage
        {
            get
            {
                if (this.poStatus == 2)
                    return "RedCircle";
                if (this.IDProductionMachine == this.curMachineId)
                    return "GreenCircle";
                return "YellowCircle";
            }
        }
        public string SemaphoreTitle
        {
            get
            {
                if (this.poStatus == 2)
                    return "Sospeso (non lavorare)";
                if (this.IDProductionMachine == this.curMachineId)
                    return "Disponibile per la lavorazione";
                return "Non ancora disponibile";
            }
        }
        public int SemaphoreCode
        {
            get
            {
                if (this.poStatus == 2)
                    return 0;
                if (this.IDProductionMachine == this.curMachineId)
                    return 1;
                return 2;
            }
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

        public bool forced
        {
            get;
            set;
        }

    }

    public partial class VW_ProductionExtMPS_Lite
    {
        //public string SemaphoreImage
        //{
        //    get
        //    {
        //        if (this.poStatus == 2)
        //            return "RedCircle";
        //        if (this.IDProductionMachine == this.curMachineId)
        //            return "GreenCircle";
        //        return "YellowCircle";
        //    }
        //}
        //public string SemaphoreTitle
        //{
        //    get
        //    {
        //        if (this.poStatus == 2)
        //            return "Sospeso (non lavorare)";
        //        if (this.IDProductionMachine == this.curMachineId)
        //            return "Disponibile per la lavorazione";
        //        return "Non ancora disponibile";
        //    }
        //}
        //public int SemaphoreCode
        //{
        //    get
        //    {
        //        if (this.poStatus == 2)
        //            return 0;
        //        if (this.IDProductionMachine == this.curMachineId)
        //            return 1;
        //        return 2;
        //    }
        //}

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

        public bool forced
        {
            get;
            set;
        }

    }
}