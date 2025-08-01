using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLLabExtim
{
    public static partial class ProductionOrderService
    {

        public static DateTime GetNextWorkingSlot(DateTime startDate, ProductionMachine machine, int machineNumber, Holyday[] holydays)
        {

            int curDoW = Convert.ToInt32(startDate.DayOfWeek);
            DateTime nextDate = startDate;
            while (true)
            {
                if (machine.DaysOfWeek.Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()) && holydays.FirstOrDefault(h => h.Day.Date == nextDate.Date) == null)
                {
                    if (nextDate > startDate)
                    {
                        nextDate = nextDate.Add(machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0)));
                    }
                    break;
                }
                nextDate = nextDate.Date.AddDays(1);
            }
            return nextDate;

        }

        public static DateTime GetNextWorkingDay(DateTime startDate, ProductionMachine machine, Holyday[] holydays)
        {

            DateTime nextDate = startDate;
            while (true)
            {
                nextDate = nextDate.Date.AddDays(1);
                if (machine.DaysOfWeek.Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()) && holydays.FirstOrDefault(h => h.Day.Date == nextDate.Date) == null)
                {
                    break;
                }
            }
            return nextDate;

        }

        public static DateTime GetNextWorkingDay(DateTime startDate, Holyday[] holydays)
        {

            DateTime nextDate = startDate;
            while (true)
            {
                nextDate = nextDate.Date.AddDays(1);
                if ("1,2,3,4,5".Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()) && holydays.FirstOrDefault(h => h.Day.Date == nextDate.Date) == null)
                {

                    if (nextDate >= startDate)
                    {
                        nextDate = nextDate.Add(new TimeSpan(0, 0, 0));
                    }
                    break;
                }
            }
            return nextDate;

        }


        public static DateTime GetEndDateTime(DateTime? startDateTime, int prodTimeMin, ProductionMachine machine, int machineNum, Holyday[] holydays)
        {
            DateTime start = startDateTime.Value;
            DateTime result;
            int breakTimeMinutes = 0;
            int stopTimeMinutes = 0;

            while (true)
            {
                if (start.TimeOfDay == new TimeSpan(23, 59, 0))
                {
                    DateTime saveStart = start;
                    start = GetNextWorkingDay(start.Date.AddDays(-1), machine, holydays).Add(start.TimeOfDay);
                    stopTimeMinutes += Convert.ToInt32((start - saveStart).TotalMinutes);

                }
                // test minuto pausa
                if (start.TimeOfDay > machine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0)) && start.TimeOfDay <= machine.BreakTimeEnd.GetValueOrDefault(new TimeSpan(13, 0, 0)))
                {
                    start = start.AddMinutes(1);
                    breakTimeMinutes += 1;
                    continue;
                }
                // test minuto stop
                else if (start.TimeOfDay < machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0)) || start.TimeOfDay >= machine.WorkTimeEnd.GetValueOrDefault(new TimeSpan(17, 0, 0)))
                {
                    start = start.AddMinutes(1);
                    stopTimeMinutes += 1;
                    continue;
                }
                else
                {
                    if ((Convert.ToInt32((start - startDateTime.Value).TotalMinutes) - breakTimeMinutes - stopTimeMinutes) >= prodTimeMin)
                    {
                        result = start;
                        break;
                    }
                    start = start.AddMinutes(1);
                }
            }


            return result;

        }


        public static List<ProductionMP> GetFFCUnusedSlotsOfADay2(QuotationDataContext db, DateTime startDate, DateTime? startTime, ProductionMachine machine, int machineNum, int minToAssign, Holyday[] holydays)
        {

            DateTime start = startDate.Add(startTime == null ? machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0)) : startTime.Value.TimeOfDay);
            List<ProductionMP> usedSlots = db.ProductionMPs.Where(p => p.ProductionMachine == machine && p.NumProductionMachine == machineNum).Where(p => p.Status == 11 || p.Status == 15).OrderBy(p => p.ProdStart).ToList();
            //
            List<ProductionMP> result = new List<ProductionMP>();
            ProductionMP currentSlot = new ProductionMP();

            TimeSpan machineWorkTimeStart = machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0));
            TimeSpan machineWorkTimeEnd = machine.WorkTimeEnd.GetValueOrDefault(new TimeSpan(17, 0, 0));
            TimeSpan machineBreakTimeStart = machine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0));
            TimeSpan machineBreakTimeEnd = machine.BreakTimeEnd.GetValueOrDefault(new TimeSpan(13, 0, 0));

            bool slotBuilding = false;
            double breakTimeMinutes = 0d;
            double stopTimeMinutes = 0d;

            while (true)
            {
                if (start.TimeOfDay == new TimeSpan(23, 59, 0))
                {
                    DateTime saveStart = start;
                    start = GetNextWorkingDay(start.Date.AddDays(0), machine, holydays); //Add(start.TimeOfDay);
                    stopTimeMinutes += (start - saveStart).TotalMinutes;
                }

                // test minuto impegnato
                if (usedSlots.FirstOrDefault(u => u.ProdStart <= start && u.ProdEnd >= start) != null)
                {

                    if (slotBuilding == true)
                    {
                        if (currentSlot.ProdTimeMinDouble > 0d)
                        {
                            currentSlot.ProdTimeMin = Convert.ToInt32(currentSlot.ProdTimeMinDouble);
                            result.Add(currentSlot);
                        }
                        currentSlot = new ProductionMP();
                        slotBuilding = false;
                    }

                    start = start.AddMinutes(1d);
                    continue;
                }
                // test minuto pausa
                else if (start.TimeOfDay > machineBreakTimeStart && start.TimeOfDay < machineBreakTimeEnd)
                {
                    start = start.AddMinutes(1d);
                    breakTimeMinutes += 1d;
                    continue;
                }
                // test minuto stop
                else if (start.TimeOfDay < machineWorkTimeStart || start.TimeOfDay > machineWorkTimeEnd)
                {
                    start = start.AddMinutes(1d);
                    stopTimeMinutes += 1d;
                    continue;
                }
                else
                {
                    if (slotBuilding == false)
                    {
                        currentSlot.ProdStart = start;
                        breakTimeMinutes = 0d;
                        stopTimeMinutes = 0d;
                        slotBuilding = true;
                    }
                    currentSlot.ProdEnd = start;
                    currentSlot.ProdTimeMinDouble = (currentSlot.ProdEnd.Value - currentSlot.ProdStart.Value).TotalMinutes - breakTimeMinutes - stopTimeMinutes;
                    if (currentSlot.ProdTimeMinDouble >= Convert.ToDouble(minToAssign))
                    {
                        currentSlot.ProdTimeMin = Convert.ToInt32(currentSlot.ProdTimeMinDouble);
                        break;
                    }
                    start = start.AddMinutes(1d);
                }
            }
            if (slotBuilding == true)
            {
                result.Add(currentSlot);
                currentSlot = new ProductionMP();
                slotBuilding = false;
            }

            return result;

        }

        //public static ProductionMachine GetBestMachine2(QuotationDataContext db, prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op, DateTime? minStartDate, DateTime? minStartTime, int minToAssign, Holyday[] holydays, out int bestMachineNum)
        public static ProductionMachine GetBestMachine2FFC(QuotationDataContext db, ProductionOrder po, prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult op, DateTime? minStartDate, DateTime? minStartTime, int minToAssign, Holyday[] holydays, out int bestMachineNum)
        {

            // loop tra le macchine equivalenti del tipo macchina per cercare la prima disponibile
            ProductionMachine machine = null;
            ProductionMachine bestMachine = null;
            bestMachineNum = 0;
            DateTime? bestStart = DateTime.MaxValue;

            ProductionMPSException exception = db.ProductionMPSExceptions.FirstOrDefault(m =>
                           m.IDProductionOrder == po.ID &&
                           m.IDPickingItem.GetValueOrDefault() == op.piid &&
                           m.IDMacroItem.GetValueOrDefault() == op.miid.GetValueOrDefault() &&
                           m.IDMacroItemDetail.GetValueOrDefault() == op.mdid.GetValueOrDefault() &&
                           m.OldIDProductionMachine.GetValueOrDefault() != m.NewIDProductionMachine.GetValueOrDefault() && m.NewIDProductionMachine != null// prendo solo le eccezioni che prevedono un cambiamento macchina, non quelle relative al cambio priorità
                            );

            if (exception != null)
            {

                machine = db.ProductionMachines.FirstOrDefault(m => m.ID == exception.NewIDProductionMachine);
                for (int n = 0; n < machine.Quantity; n++)
                {
                    ProductionMP firstUnusedSlot = null;
                    while (firstUnusedSlot == null)
                    {
                        firstUnusedSlot = GetFFCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, machine, n, minToAssign, holydays)
                                    //.Where(s => s.ProdTimeMin >= minToAssign || s.ProdStart == s.ProdStart.GetValueOrDefault().Date.Add(machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0))))
                                    .Where(s => (s.ProdTimeMin >= minToAssign)).OrderBy(s => s.ProdStart).ThenBy(s => s.ProdTimeMin) // || (s.ProdTimeMin == machine.MinPerDay))
                                    .FirstOrDefault();
                        if (firstUnusedSlot != null)
                        {
                            if (firstUnusedSlot.ProdStart <= bestStart)
                            {
                                bestStart = firstUnusedSlot.ProdStart;
                                bestMachine = machine;
                                bestMachineNum = n;
                            }
                        }
                        else
                        {
                            minStartDate = GetNextWorkingDay(minStartDate.Value, machine, holydays);
                            minStartTime = null;
                        }
                    }
                }

            }
            else
            {

                //foreach (ProductionMachinesToPickingItem machineToPickingItem in db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid && m.ProductionMachine.ID_Company == po.ID_Company).OrderByDescending(m => m.Priority))
                // merge aziendale
                    foreach (ProductionMachinesToPickingItem machineToPickingItem in db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid).OrderByDescending(m => m.Priority))
                {
                    machine = machineToPickingItem.ProductionMachine;
                    for (int n = 0; n < machine.Quantity; n++)
                    {
                        ProductionMP firstUnusedSlot = null;
                        while (firstUnusedSlot == null)
                        {
                            firstUnusedSlot = GetFFCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, machine, n, minToAssign, holydays)
                                        //.Where(s => s.ProdTimeMin >= minToAssign || s.ProdStart == s.ProdStart.GetValueOrDefault().Date.Add(machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0))))
                                        .Where(s => (s.ProdTimeMin >= minToAssign)).OrderBy(s => s.ProdStart).ThenBy(s => s.ProdTimeMin) // || (s.ProdTimeMin == machine.MinPerDay))
                                        .FirstOrDefault();
                            if (firstUnusedSlot != null)
                            {
                                if (firstUnusedSlot.ProdStart <= bestStart)
                                {
                                    bestStart = firstUnusedSlot.ProdStart;
                                    bestMachine = machineToPickingItem.ProductionMachine;
                                    bestMachineNum = n;
                                }
                            }
                            else
                            {
                                minStartDate = GetNextWorkingDay(minStartDate.Value, machine, holydays);
                                minStartTime = null;
                            }
                        }
                    }
                }

            }
            return bestMachine;

        }

        public static void CreateProductionOrderScheduleFFC2_NoSplit(QuotationDataContext db, ProductionOrder po, int newScheduleFlag = 11)
        {
            //int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).Count();
            int posInSequence = 1; // db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity).Count();
            Holyday[] holydays = db.Holydays.ToArray();
            ProductionMachine curMachine = null;
            int num = 0;

            int lastMachineId = 0;
            int lastNum = 0;
            int lastPickingItemId = 0;

            // loop tra le fasi dell'OdP
            //foreach (prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op in db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).OrderByDescending(op => op.qdPosition).ThenByDescending(op => op.mdPosition).ThenByDescending(op => op.piOrder))

            //prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op;
            //List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).OrderByDescending(o => o.qdPosition).ThenByDescending(o => o.mdPosition).ThenByDescending(o => o.piOrder).ToList();
            //nuova versione con cambio al volo della macchina utilizzata per una fase
            prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult op;
            List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).OrderBy(o => o.qdPosition).ThenBy(o => o.mdPosition).ThenBy(o => o.piOrder).ToList();
            int machineTotalUninterruptableTime = 0;

            for (int j = 0; j < ops.Count; j++)
            {
                op = ops[j];

                ProductionMP completedPhase = db.ProductionMPs.FirstOrDefault(p => p.IDProductionOrder == po.ID && p.IDQuotationDetail == op.qdid);
                if (completedPhase != null)
                {
                    if (completedPhase.Status != 11)
                    {
                        continue;
                    }
                }


                machineTotalUninterruptableTime = Convert.ToInt32(op.ProdTimeMin);

                for (int k = j + 1; k < ops.Count; k++)
                {
                    if (ops[k].piid == op.piid)
                    {
                        machineTotalUninterruptableTime += Convert.ToInt32(ops[k].ProdTimeMin);
                    }
                }


                int minToAssign = Convert.ToInt32(op.ProdTimeMin);
                if (minToAssign <= 0)
                {
                    continue;
                }

                DateTime? minStartDate;
                DateTime? minStartTime;

                ProductionMP lastProdEnd = db.ProductionMPs.Where(i => i.IDProductionOrder == po.ID).OrderByDescending(i => i.ProdEnd).FirstOrDefault();
                if (lastProdEnd == null || lastProdEnd.ProdEnd < GetNextWorkingDay(DateTime.Now.Date, holydays))
                {
                    minStartDate = GetNextWorkingDay(DateTime.Now.Date, holydays);
                    minStartTime = null;
                }
                else
                {
                    minStartTime = (lastProdEnd.ProdEnd.GetValueOrDefault() > DateTime.Now ? lastProdEnd.ProdEnd.GetValueOrDefault() : DateTime.Now);
                    minStartDate = minStartTime.Value.Date;

                }

                bool hasMachine = (db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid && m.ProductionMachine.ID_Company == po.ID_Company).FirstOrDefault() != null);
                if (hasMachine)
                {
                    if (op.piid == lastPickingItemId)
                    {
                        curMachine = db.ProductionMachines.FirstOrDefault(m => m.ID == lastMachineId);
                        num = lastNum;
                    }
                    else
                    {
                        curMachine = GetBestMachine2FFC(db, po, op, minStartDate, minStartTime, machineTotalUninterruptableTime, holydays, out num);
                    }
                }
                else
                // si verifica quando una fase di produzione non è assegnata ad una macchina (ad esempio sola manodopera)
                {
                    continue;
                }

                //curMachine = GetBestAlterativeMachine(db, op, minStartDate, minStartTime, minToAssign);
                //int num = GetBestParallelMachine(db, curMachine, minStartDate, minStartTime, minToAssign);

                // loop per assegnare la slot (il loop deve essere e questo livello per vedere se una fase può essere conclusa in una slot libera intermedia della giornata
                while (minToAssign > 0m)
                {
                    // loop per trovare il primo giorno disponibile
                    while (true)
                    {
                        ProductionMP firstUnusedSlot = GetFFCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, curMachine, num, machineTotalUninterruptableTime, holydays)
                                .Where(s => (s.ProdTimeMin >= machineTotalUninterruptableTime)).OrderBy(s => s.ProdStart).ThenBy(s => s.ProdTimeMin)
                            .FirstOrDefault();
                        if (firstUnusedSlot != null)
                        {
                            ProductionMP mpsl = new ProductionMP();
                            mpsl.IDProductionOrder = po.ID;
                            mpsl.IDQuotationDetail = op.qdid;
                            mpsl.IDMacroItem = op.miid;
                            mpsl.IDMacroItemDetail = op.mdid;
                            mpsl.IDPickingItem = op.piid;
                            mpsl.IDProductionMachine = curMachine.ID;
                            mpsl.NumProductionMachine = num;
                            mpsl.Order = posInSequence.ToString();
                            mpsl.Priority = 0;
                            mpsl.ProdTimeMin = Math.Min(minToAssign, firstUnusedSlot.ProdTimeMin.Value);

                            mpsl.ProdStart = firstUnusedSlot.ProdStart;
                            mpsl.ProdEnd = GetEndDateTime(firstUnusedSlot.ProdStart, mpsl.ProdTimeMin.Value, curMachine, num, holydays);

                            mpsl.Status = 11;

                            db.ProductionMPs.InsertOnSubmit(mpsl);
                            db.SubmitChanges();
                            minToAssign -= mpsl.ProdTimeMin.GetValueOrDefault(0);
                            lastMachineId = curMachine.ID;
                            lastNum = num;
                            lastPickingItemId = op.piid;
                            break;
                        }
                        minStartDate = GetNextWorkingDay(minStartDate.Value, curMachine, holydays);
                        minStartTime = null;
                    }

                }

                posInSequence += 1;
            }


        }



        public static void CreateProductionOrderScheduleFFC2(QuotationDataContext db, ProductionOrder po, int newScheduleFlag = 11)
        {
            //int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).Count();
            int posInSequence = 1; // db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity).Count();
            Holyday[] holydays = db.Holydays.ToArray();
            ProductionMachine curMachine = null;
            int num = 0;

            int lastMachineId = 0;
            int lastNum = 0;
            int lastPickingItemId = 0;

            // loop tra le fasi dell'OdP
            //foreach (prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op in db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).OrderByDescending(op => op.qdPosition).ThenByDescending(op => op.mdPosition).ThenByDescending(op => op.piOrder))

            //prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op;
            //List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).OrderByDescending(o => o.qdPosition).ThenByDescending(o => o.mdPosition).ThenByDescending(o => o.piOrder).ToList();

            //nuova versione con cambio al volo della macchina utilizzata per una fase
            //prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult op;
            //List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).OrderBy(o => o.qdPosition).ThenBy(o => o.mdPosition).ThenBy(o => o.piOrder).ToList();

            // merge aziendale
            prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult op;
            List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity_MergeCompanies(po.ID_Quotation, po.ID, po.Quantity).OrderBy(o => o.qdPosition).ThenBy(o => o.mdPosition).ThenBy(o => o.piOrder).ToList();




            int machineTotalUninterruptableTime = 0;

            for (int j = 0; j < ops.Count; j++)
            {

                op = ops[j];

                ProductionMP completedPhase = db.ProductionMPs.FirstOrDefault(p => p.IDProductionOrder == po.ID && p.IDQuotationDetail == op.qdid);
                if (completedPhase != null)
                {
                    if (completedPhase.Status != 11)
                    {
                        continue;
                    }
                }


                machineTotalUninterruptableTime = Convert.ToInt32(op.ProdTimeMin);

                for (int k = j + 1; k < ops.Count; k++)
                {
                    if (ops[k].piid == op.piid)
                    {
                        machineTotalUninterruptableTime += Convert.ToInt32(ops[k].ProdTimeMin);
                    }
                }


                int minToAssign = Convert.ToInt32(op.ProdTimeMin);
                if (minToAssign <= 0)
                {
                    continue;
                }

                DateTime? minStartDate;
                DateTime? minStartTime;

                ProductionMP lastProdEnd = db.ProductionMPs.Where(i => i.IDProductionOrder == po.ID).OrderByDescending(i => i.ProdEnd).FirstOrDefault();
                if (lastProdEnd == null || lastProdEnd.ProdEnd < GetNextWorkingDay(DateTime.Now.Date, holydays))
                {
                    minStartDate = GetNextWorkingDay(DateTime.Now.Date, holydays);
                    minStartTime = null;
                }
                else
                {
                    minStartDate = lastProdEnd.ProdEnd.Value.Date;
                    minStartTime = lastProdEnd.ProdEnd.GetValueOrDefault();
                }

                //bool hasMachine = (db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid && m.ProductionMachine.ID_Company == po.ID_Company).FirstOrDefault() != null);
                // merge aziendale
                bool hasMachine = (db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid).FirstOrDefault() != null);
                if (hasMachine)
                {
                    if (op.piid == lastPickingItemId)
                    {
                        curMachine = db.ProductionMachines.FirstOrDefault(m => m.ID == lastMachineId);
                        num = lastNum;
                    }
                    else
                    {
                        curMachine = GetBestMachine2FFC(db, po, op, minStartDate, minStartTime, machineTotalUninterruptableTime, holydays, out num);
                    }
                }
                else
                // si verifica quando una fase di produzione non è assegnata ad una macchina (ad esempio sola manodopera)
                {
                    continue;
                }

                //curMachine = GetBestAlterativeMachine(db, op, minStartDate, minStartTime, minToAssign);
                //int num = GetBestParallelMachine(db, curMachine, minStartDate, minStartTime, minToAssign);

                // loop per assegnare la slot (il loop deve essere e questo livello per vedere se una fase può essere conclusa in una slot libera intermedia della giornata

                 while (minToAssign > 0m)
                {
                    // loop per trovare il primo giorno disponibile

                    while (true)
                    {
                        ProductionMP firstUnusedSlot = GetFFCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, curMachine, num, machineTotalUninterruptableTime, holydays)
                                .Where(s => (s.ProdTimeMin >= machineTotalUninterruptableTime)).OrderBy(s => s.ProdStart).ThenBy(s => s.ProdTimeMin)
                            .FirstOrDefault();
                        if (firstUnusedSlot != null)
                        {
                            DateTime prodStart = GetNextWorkingSlot(firstUnusedSlot.ProdStart.GetValueOrDefault(), curMachine, num, holydays);
                            //DateTime prodEnd = GetEndDateTime(firstUnusedSlot.ProdStart, firstUnusedSlot.ProdTimeMin.Value, curMachine, num, holydays);
                            DateTime prodEnd = GetEndDateTime(prodStart, minToAssign, curMachine, num, holydays);

                            DateTime curDate = prodStart.Date;

                            while (curDate <= prodEnd.Date)
                            {
                                ProductionMP mpsl = new ProductionMP();
                                mpsl.IDProductionOrder = po.ID;
                                mpsl.IDQuotationDetail = op.qdid;
                                mpsl.IDMacroItem = op.miid;
                                mpsl.IDMacroItemDetail = op.mdid;
                                mpsl.IDPickingItem = op.piid;
                                mpsl.IDProductionMachine = curMachine.ID;
                                mpsl.NumProductionMachine = num;
                                mpsl.Order = posInSequence.ToString();
                                mpsl.Priority = 0;

                                //mpsl.ProdStart = firstUnusedSlot.ProdStart;
                                //mpsl.ProdEnd = GetEndDateTime(firstUnusedSlot.ProdStart, mpsl.ProdTimeMin.Value, curMachine, num, holydays);



                                if (prodStart.Date < curDate.Date && prodEnd.Date > curDate.Date)
                                {
                                    mpsl.ProdStart = curDate.Date.AddTicks(curMachine.WorkTimeStart.GetValueOrDefault().Ticks); ;
                                    mpsl.ProdEnd = curDate.Date.AddTicks(curMachine.WorkTimeEnd.GetValueOrDefault().Ticks); ;
                                }
                                else if (prodStart.Date < curDate.Date && prodEnd.Date == curDate.Date)
                                {
                                    mpsl.ProdStart = curDate.Date.AddTicks(curMachine.WorkTimeStart.GetValueOrDefault().Ticks);
                                    mpsl.ProdEnd = prodEnd;
                                }
                                else if (prodStart.Date == curDate.Date && prodEnd.Date > curDate.Date)
                                {
                                    mpsl.ProdStart = prodStart;
                                    mpsl.ProdEnd = curDate.Date.AddTicks(curMachine.WorkTimeEnd.GetValueOrDefault().Ticks);
                                }
                                else
                                {
                                    mpsl.ProdStart = prodStart;
                                    mpsl.ProdEnd = prodEnd;
                                }

                                int noWorkTimeMinutes = 0;
                                if (mpsl.ProdStart.GetValueOrDefault().TimeOfDay <= curMachine.BreakTimeStart && mpsl.ProdEnd.GetValueOrDefault().TimeOfDay >= curMachine.BreakTimeEnd)
                                {
                                    noWorkTimeMinutes = Convert.ToInt32((curMachine.BreakTimeEnd.GetValueOrDefault() - curMachine.BreakTimeStart.GetValueOrDefault()).TotalMinutes);
                                }

                                mpsl.ProdTimeMin = Convert.ToInt32((mpsl.ProdEnd.GetValueOrDefault() - mpsl.ProdStart.GetValueOrDefault()).TotalMinutes) - noWorkTimeMinutes;
                                mpsl.Status = newScheduleFlag;

                                if (mpsl.ProdStart.Value.DayOfWeek == DayOfWeek.Saturday || mpsl.ProdStart.Value.DayOfWeek == DayOfWeek.Sunday)
                                { }

                                db.ProductionMPs.InsertOnSubmit(mpsl);
                                db.SubmitChanges();
                                curDate = GetNextWorkingDay(curDate, holydays);

                            }
                            //minToAssign -= mpsl.ProdTimeMin.GetValueOrDefault(0);
                            minToAssign -= Math.Min(minToAssign, firstUnusedSlot.ProdTimeMin.Value);

                            lastMachineId = curMachine.ID;
                            lastNum = num;
                            lastPickingItemId = op.piid;

                            break;
                        }

                        minStartDate = GetNextWorkingDay(minStartDate.Value, curMachine, holydays);
                        minStartTime = null;
                    }

                }

                posInSequence += 1;
            }


        }


        public static void MovePhase(QuotationDataContext db, int poID, int machineId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate)
        {
            // intercetta tutte le fasi raggruppate nell'evento del calendario
            ProductionOrder po = db.ProductionOrders.FirstOrDefault(p => p.ID == poID);
            TimeSpan offset = (newStartDate - oldStartDate);

            foreach (ProductionMP step in db.ProductionMPs.Where(p => p.IDProductionOrder == poID && (p.Status == 11 || p.Status == 15) && p.IDProductionMachine == machineId && p.ProdStart >= oldStartDate && p.ProdEnd <= oldEndDate))
            {
                if (step.ProdStart >= oldStartDate && step.ProdEnd <= oldEndDate)
                {
                    step.ProdStart = step.ProdStart.Value.Add(offset);
                    step.ProdEnd = step.ProdEnd.Value.Add(offset);
                }
            }
            db.SubmitChanges();
            if (offset.TotalMinutes < 0)
            {
                // se l'offset è minore di 0, allora blocco tutte le fasi successive e ricalcolo all'indietro quelle restanti
                foreach (ProductionMP step in db.ProductionMPs.Where(p => p.IDProductionOrder == poID && p.Status == 11 && p.ProdStart >= newStartDate && p.ProdEnd <= newEndDate).OrderByDescending(p => p.ProdEnd))
                {
                    step.Status = 15;
                }
                db.SubmitChanges();

                DeleteProductionOrderSchedule(db, po);
                CreateProductionOrderScheduleIBC2(db, po, 15, newStartDate);
            }
            else
            {
                // se l'offset è minore di 0, allora blocco tutte le fasi successive e ricalcolo all'indietro quelle restanti
                foreach (ProductionMP step in db.ProductionMPs.Where(p => p.IDProductionOrder == poID && p.Status == 11 && p.ProdEnd <= newEndDate).OrderByDescending(p => p.ProdEnd))
                {
                    step.Status = 15;
                }
                db.SubmitChanges();

                CreateProductionOrderScheduleFFC2(db, po, 15);
            }

        }

        public static void CancelPriority(QuotationDataContext db, int poID)
        {
            ProductionOrder po = db.ProductionOrders.FirstOrDefault(p => p.ID == poID);

            db.ProductionMPs.DeleteAllOnSubmit(db.ProductionMPs.Where(p => p.IDProductionOrder == poID));
            db.SubmitChanges();

            CreateProductionOrderScheduleFFC2(db, po);

        }





    }
}
