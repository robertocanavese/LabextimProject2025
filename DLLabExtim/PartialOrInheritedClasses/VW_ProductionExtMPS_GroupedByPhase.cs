using System;
using CMLabExtim;

namespace DLLabExtim
{
    public partial class VW_ProductionExtMPS_GroupedByPhase
    {
        public string SemaphoreImage
        {
            get
            {
                if (this.poStatus == 2)
                    return "RedCircle";
                if (this.poStatus == 9)
                    return "GreyCircle";
                if (this.Status == 13)
                    return "Delete";
                if (this.IDProductionMachine == this.curMachineId)
                    return "GreenCircle";
                if (this.Status == 12)
                    return "OK";
                return "YellowCircle";
            }
        }
        public string SemaphoreTitle
        {
            get
            {
                if (this.poStatus == 2)
                    return "Sospeso (non lavorare)";
                if (this.poStatus == 9)
                    return "In attesa di documentazione/attrezzature";
                if (this.Status == 13)
                    return "Annullata";
                if (this.IDProductionMachine == this.curMachineId)
                    return "Disponibile per la lavorazione";
                if (this.Status == 12)
                    return "Completata";
                return "Non ancora disponibile";
            }
        }
        public int SemaphoreCode
        {
            get
            {
                if (this.poStatus == 2)
                    return 0;
                if (this.poStatus == 9)
                    return 2;
                if (this.Status == 13)
                    return 4;
                if (this.IDProductionMachine == this.curMachineId)
                    return 1;
                if (this.Status == 12)
                    return 3;
                return 2;
            }
        }

        public string PercLavExe
        {
            get
            {
                return (Convert.ToDecimal(ProdEffMin.GetValueOrDefault()) / Convert.ToDecimal(ProdTimeMin == 0 ? 1 : ProdTimeMin)).ToString("P0");
            }
        }

        public string ProdEffTime
        {
            get
            {
                //return new DateTime(2000, 1, 1, this.ProdEffMin.GetValueOrDefault() / 60, this.ProdEffMin.GetValueOrDefault() % 60, 0).ToString("HH:mm");
                if (this.ProdEffMin.GetValueOrDefault() > 0)
                    return string.Format("{0}:{1:00}",
                        this.ProdEffMin.GetValueOrDefault() / 60,
                         this.ProdEffMin.GetValueOrDefault() % 60);
                else
                    return "0:00";
            }
        }

        public int DatiMacchinaCopieRichieste { get;set; }
        public int DatiMacchinaCopieLavorate { get; set; }

    }


    public partial class VW_ProductionExtMPS_GroupedByPhase_Lite
    {
        public string SemaphoreImage
        {
            get
            {
                if (this.poStatus == 2)
                    return "RedCircle";
                if (this.poStatus == 9)
                    return "GreyCircle";
                if (this.Status == 13)
                    return "Delete";
                if (this.IDProductionMachine == this.curMachineId)
                    return "GreenCircle";
                if (this.Status == 12)
                    return "OK";
                return "YellowCircle";
            }
        }
        public string SemaphoreTitle
        {
            get
            {
                if (this.poStatus == 2)
                    return "Sospeso (non lavorare)";
                if (this.poStatus == 9)
                    return "In attesa di documentazione/attrezzature";
                if (this.Status == 13)
                    return "Annullata";
                if (this.IDProductionMachine == this.curMachineId)
                    return "Disponibile per la lavorazione";
                if (this.Status == 12)
                    return "Completata";
                return "Non ancora disponibile";
            }
        }
        public int SemaphoreCode
        {
            get
            {
                if (this.poStatus == 2)
                    return 0;
                if (this.poStatus == 9)
                    return 2;
                if (this.Status == 13)
                    return 4;
                if (this.IDProductionMachine == this.curMachineId)
                    return 1;
                if (this.Status == 12)
                    return 3;
                return 2;
            }
        }

        public string PercLavExe
        {
            get
            {
                return (Convert.ToDecimal(ProdEffMin.GetValueOrDefault()) / Convert.ToDecimal(ProdTimeMin == 0 ? 1 : ProdTimeMin)).ToString("P0");
            }
        }

        public string ProdEffTime
        {
            get
            {
                //return new DateTime(2000, 1, 1, this.ProdEffMin.GetValueOrDefault() / 60, this.ProdEffMin.GetValueOrDefault() % 60, 0).ToString("HH:mm");
                if (this.ProdEffMin.GetValueOrDefault() > 0)
                    return string.Format("{0}:{1:00}",
                        this.ProdEffMin.GetValueOrDefault() / 60,
                         this.ProdEffMin.GetValueOrDefault() % 60);
                else
                    return "0:00";
            }
        }

        public int DatiMacchinaCopieRichieste { get; set; }
        public int DatiMacchinaCopieLavorate { get; set; }

    }

}