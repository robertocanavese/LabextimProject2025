using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMLabExtim;

namespace DLLabExtim
{
    public static partial class ProductionOrderService
    {


        public static DateTime GetPreviousWorkingSlot(DateTime startDate, ProductionMachine machine, int machineNumber)
        {

            int curDoW = Convert.ToInt32(startDate.DayOfWeek);
            DateTime nextDate = startDate;
            while (true)
            {
                if (machine.DaysOfWeek.Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()))
                {
                    if (nextDate < startDate)
                    {
                        nextDate = nextDate.Add(machine.WorkTimeEnd.GetValueOrDefault(new TimeSpan(17, 0, 0)));
                    }
                    break;
                }
                nextDate = nextDate.Date.AddDays(-1);
            }
            return nextDate;

        }

        //public static DateTime GetNextWorkingDay(DateTime startDate, ProductionMachine machine, int machineNumber)
        //{

        //    DateTime nextDate = startDate;
        //    while (true)
        //    {
        //        nextDate = nextDate.Date.AddDays(1);
        //        if (machine.DaysOfWeek.Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()))
        //        {

        //            if (nextDate > startDate)
        //            {
        //                nextDate = nextDate.Add(machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0)));
        //            }
        //            break;
        //        }
        //    }
        //    return nextDate;

        //}

        public static DateTime GetPreviousWorkingDay(DateTime startDate, ProductionMachine machine, Holyday[] holydays)
        {

            DateTime nextDate = startDate;
            while (true)
            {
                nextDate = nextDate.Date.AddDays(-1);
                if (machine.DaysOfWeek.Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()) && holydays.FirstOrDefault(h => h.Day.Date == nextDate.Date) == null)
                {
                    break;
                }
            }
            return nextDate;

        }

        public static DateTime GetPreviousWorkingDay(DateTime startDate, Holyday[] holydays)
        {

            DateTime nextDate = startDate;
            while (true)
            {
                nextDate = nextDate.Date.AddDays(-1);
                if ("1,2,3,4,5".Contains(Convert.ToInt32(nextDate.DayOfWeek).ToString()) && holydays.FirstOrDefault(h => h.Day.Date == nextDate.Date) == null)
                {

                    if (nextDate <= startDate)
                    {
                        nextDate = nextDate.Add(new TimeSpan(0, 0, 0));
                    }
                    break;
                }
            }
            return nextDate;

        }




        //public static void CreateProductionOrderScheduleFFC(QuotationDataContext db, ProductionOrder po)
        //{
        //    //db.ProductionMPs.DeleteAllOnSubmit(db.ProductionMPs.Where(p => p.IDProductionOrder == po.ID));
        //    //db.SubmitChanges();
        //    int posInSequence = 0;

        //    foreach (prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantityResult op in db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity))
        //    {

        //        VW_ProductionMPSGroupedByDate slotAvailable = null;
        //        VW_ProductionMPSGroupedByDate lastUsedSlot = null;
        //        int minToAssign = Convert.ToInt32(op.ProdTimeMin);
        //        ProductionMachine curMachine = null;
        //        int curMachineNum = 0;
        //        DateTime minStartDate;
        //        DateTime curStartDate;

        //        ProductionMP lastProdEnd = db.ProductionMPs.Where(i => i.IDProductionOrder == po.ID).OrderByDescending(i => i.ProdEnd).FirstOrDefault();
        //        if (lastProdEnd == null)
        //        {
        //            minStartDate = DateTime.Now;
        //        }
        //        else
        //        {
        //            minStartDate = lastProdEnd.ProdEnd.GetValueOrDefault();
        //        }

        //        int curProdTimeAvail = 0;
        //        bool firstLoop = true;

        //        while (minToAssign > 0m)
        //        {
        //            foreach (ProductionMachinesToPickingItem machineToPickingItem in db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid).OrderBy(m => m.Priority))
        //            {
        //                curMachine = db.ProductionMachines.FirstOrDefault(m => m.ID == machineToPickingItem.IDProductionMachine);

        //                for (int num = 0; num < curMachine.Quantity; num++)
        //                {
        //                    slotAvailable = db.VW_ProductionMPSGroupedByDates.Where(i => i.ProdDate >= minStartDate.Date && i.IDProductionMachine == machineToPickingItem.IDProductionMachine && i.NumProductionMachine == num && i.ProdTimeAvailMin > 0).OrderBy(i => i.ProdDate).FirstOrDefault();
        //                    lastUsedSlot = db.VW_ProductionMPSGroupedByDates.Where(i => i.ProdDate >= minStartDate.Date && i.IDProductionMachine == machineToPickingItem.IDProductionMachine && i.NumProductionMachine == num).OrderByDescending(i => i.ProdDate).FirstOrDefault();
        //                    if (slotAvailable != null)
        //                    {
        //                        curMachineNum = num;
        //                        break;
        //                    }
        //                }
        //            }

        //            if (slotAvailable == null)
        //            {

        //                if (firstLoop)
        //                {
        //                    curMachine = db.ProductionMachines.FirstOrDefault(m => m.ID == db.ProductionMachinesToPickingItems.FirstOrDefault(m1 => m1.IDPickingItem == op.piid).IDProductionMachine);
        //                    curStartDate = GetNextWorkingSlot(minStartDate, curMachine, curMachineNum);
        //                    firstLoop = false;
        //                }
        //                else
        //                {
        //                    curStartDate = GetNextWorkingDay(lastUsedSlot.ProdDate.Value, curMachine, curMachineNum);
        //                }
        //                curProdTimeAvail = curMachine.MinPerDay.GetValueOrDefault(0);

        //            }
        //            else
        //            {
        //                if (firstLoop)
        //                {
        //                    firstLoop = false;
        //                }
        //                curStartDate = slotAvailable.ProdEnd.GetValueOrDefault();
        //                curProdTimeAvail = slotAvailable.ProdTimeAvailMin.GetValueOrDefault(0);
        //            }


        //            ProductionMP mpsl = new ProductionMP();
        //            mpsl.IDProductionOrder = po.ID;
        //            mpsl.IDQuotationDetail = op.qdid;
        //            mpsl.IDMacroItemDetail = op.mdid;
        //            mpsl.IDPickingItem = op.piid;
        //            mpsl.IDProductionMachine = curMachine.ID;
        //            mpsl.NumProductionMachine = curMachineNum;
        //            mpsl.Order = posInSequence.ToString();
        //            mpsl.Priority = 0;
        //            mpsl.ProdStart = curStartDate;
        //            mpsl.ProdTimeMin = Math.Min(minToAssign, curProdTimeAvail);

        //            if (mpsl.ProdStart.Value > mpsl.ProdStart.Value.Date.Add(curMachine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0))))
        //            {
        //                mpsl.ProdEnd = mpsl.ProdStart.Value.AddMinutes(Convert.ToDouble(mpsl.ProdTimeMin.Value));
        //            }
        //            else if (mpsl.ProdStart.Value.AddMinutes(Convert.ToDouble(mpsl.ProdTimeMin.Value)) > mpsl.ProdStart.Value.Date.Add(curMachine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0))))
        //            {
        //                mpsl.ProdEnd = mpsl.ProdStart.Value.AddMinutes(Convert.ToDouble(mpsl.ProdTimeMin.Value)).Add(curMachine.BreakTimeEnd.GetValueOrDefault(new TimeSpan(13, 0, 0)) - curMachine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0)));
        //            }
        //            else
        //            {
        //                mpsl.ProdEnd = mpsl.ProdStart.Value.AddMinutes(Convert.ToDouble(mpsl.ProdTimeMin.Value));
        //            }

        //            mpsl.Status = 11;

        //            db.ProductionMPs.InsertOnSubmit(mpsl);
        //            db.SubmitChanges();
        //            minToAssign -= mpsl.ProdTimeMin.GetValueOrDefault(0);

        //        }
        //        posInSequence += 1;
        //    }


        //}

        public static DateTime GetStartDateTime(DateTime? endDateTime, int prodTimeMin, ProductionMachine machine, int machineNum, Holyday[] holydays)
        {
            DateTime start = endDateTime.Value;
            DateTime result;
            int breakTimeMinutes = 0;
            int stopTimeMinutes = 0;

            while (true)
            {
                if (start.TimeOfDay == new TimeSpan(23, 59, 0))
                {
                    DateTime saveStart = start;
                    start = GetPreviousWorkingDay(start.Date.AddDays(1), machine, holydays).Add(start.TimeOfDay);
                    stopTimeMinutes -= Convert.ToInt32((start - saveStart).TotalMinutes);

                }
                // test minuto pausa
                if (start.TimeOfDay > machine.BreakTimeStart.GetValueOrDefault(new TimeSpan(12, 0, 0)) && start.TimeOfDay <= machine.BreakTimeEnd.GetValueOrDefault(new TimeSpan(13, 0, 0)))
                {
                    start = start.AddMinutes(-1);
                    breakTimeMinutes += 1;
                    continue;
                }
                // test minuto stop
                else if (start.TimeOfDay < machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0)) || start.TimeOfDay >= machine.WorkTimeEnd.GetValueOrDefault(new TimeSpan(17, 0, 0)))
                {
                    start = start.AddMinutes(-1);
                    stopTimeMinutes += 1;
                    continue;
                }
                else
                {
                    if ((Convert.ToInt32((endDateTime.Value - start).TotalMinutes) - breakTimeMinutes - stopTimeMinutes) >= prodTimeMin)
                    {
                        result = start;
                        break;
                    }
                    start = start.AddMinutes(-1);
                }
            }


            return result;

        }


        public static List<ProductionMP> GetIBCUnusedSlotsOfADay2(QuotationDataContext db, DateTime startDate, DateTime? startTime, ProductionMachine machine, int machineNum, int minToAssign, Holyday[] holydays)
        {

            DateTime start = startDate.Add(startTime == null ? machine.WorkTimeEnd.GetValueOrDefault(new TimeSpan(17, 0, 0)) : startTime.Value.TimeOfDay);
            List<ProductionMP> usedSlots = db.ProductionMPs.Where(p => p.ProductionMachine == machine && p.NumProductionMachine == machineNum).Where(p => (p.Status == 11 || p.Status == 15)).OrderByDescending(p => p.ProdEnd).ToList();
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
                    start = GetPreviousWorkingDay(start.Date.AddDays(1), machine, holydays).Add(start.TimeOfDay);
                    stopTimeMinutes -= (start - saveStart).TotalMinutes;
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

                    start = start.AddMinutes(-1d);
                    continue;
                }
                // test minuto pausa
                else if (start.TimeOfDay > machineBreakTimeStart && start.TimeOfDay < machineBreakTimeEnd)
                {
                    start = start.AddMinutes(-1d);
                    breakTimeMinutes += 1d;
                    continue;
                }
                // test minuto stop
                else if (start.TimeOfDay < machineWorkTimeStart || start.TimeOfDay > machineWorkTimeEnd)
                {
                    start = start.AddMinutes(-1d);
                    stopTimeMinutes += 1d;
                    continue;
                }
                else
                {
                    if (slotBuilding == false)
                    {
                        currentSlot.ProdEnd = start;
                        breakTimeMinutes = 0d;
                        stopTimeMinutes = 0d;
                        slotBuilding = true;
                    }
                    currentSlot.ProdStart = start;
                    currentSlot.ProdTimeMinDouble = (currentSlot.ProdEnd.Value - currentSlot.ProdStart.Value).TotalMinutes - breakTimeMinutes - stopTimeMinutes;
                    if (currentSlot.ProdTimeMinDouble >= Convert.ToDouble(minToAssign))
                    {
                        currentSlot.ProdTimeMin = Convert.ToInt32(currentSlot.ProdTimeMinDouble);
                        break;
                    }
                    start = start.AddMinutes(-1d);
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
        public static ProductionMachine GetBestMachine2(QuotationDataContext db, int companyId, prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult op, DateTime? minStartDate, DateTime? minStartTime, int minToAssign, Holyday[] holydays, out int bestMachineNum)
        {

            // loop tra le macchine equivalenti del tipo macchina per cercare la prima disponibile
            ProductionMachine machine = null;
            ProductionMachine bestMachine = null;
            bestMachineNum = 0;
            DateTime? bestEnd = DateTime.MinValue;

            foreach (ProductionMachinesToPickingItem machineToPickingItem in db.ProductionMachinesToPickingItems.Where(m => m.IDPickingItem == op.piid && m.ProductionMachine.ID_Company == companyId).OrderByDescending(m => m.Priority))
            {
                machine = machineToPickingItem.ProductionMachine;
                for (int n = 0; n < machine.Quantity; n++)
                {
                    ProductionMP firstUnusedSlot = null;
                    while (firstUnusedSlot == null)
                    {
                        firstUnusedSlot = GetIBCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, machine, n, minToAssign, holydays)
                            //.Where(s => s.ProdTimeMin >= minToAssign || s.ProdStart == s.ProdStart.GetValueOrDefault().Date.Add(machine.WorkTimeStart.GetValueOrDefault(new TimeSpan(8, 0, 0))))
                                    .Where(s => (s.ProdTimeMin >= minToAssign)).OrderByDescending(s => s.ProdEnd).ThenBy(s => s.ProdTimeMin) // || (s.ProdTimeMin == machine.MinPerDay))
                                    .FirstOrDefault();
                        if (firstUnusedSlot != null)
                        {
                            if (firstUnusedSlot.ProdEnd >= bestEnd)
                            {
                                bestEnd = firstUnusedSlot.ProdEnd;
                                bestMachine = machineToPickingItem.ProductionMachine;
                                bestMachineNum = n;
                            }
                        }
                        else
                        {
                            minStartDate = GetPreviousWorkingDay(minStartDate.Value, machine, holydays);
                            minStartTime = null;
                        }
                    }
                }
            }
            return bestMachine;

        }

        public static void CreateProductionOrderScheduleIBC2_NoSplit(QuotationDataContext db, ProductionOrder po)
        {

            //int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).Count();
            int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).Count();
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
            List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).OrderByDescending(o => o.qdPosition).ThenByDescending(o => o.mdPosition).ThenByDescending(o => o.piOrder).ToList();
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

                ProductionMP lastProdEnd = db.ProductionMPs.Where(i => i.IDProductionOrder == po.ID).OrderBy(i => i.ProdStart).FirstOrDefault();
                if (lastProdEnd == null)
                {
                    minStartDate = GetPreviousWorkingDay(po.DeliveryDate.GetValueOrDefault(), holydays);
                    minStartTime = null;
                }
                else
                {
                    minStartDate = lastProdEnd.ProdStart.Value.Date;
                    minStartTime = lastProdEnd.ProdStart.GetValueOrDefault();
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
                        curMachine = GetBestMachine2(db, po.ID_Company.Value, op, minStartDate, minStartTime, machineTotalUninterruptableTime, holydays, out num);
                    }
                }
                else
                // si verifica quando una fase di produzione non è assegnata ad una macchina (ad esempio sola manodopera)
                {
                    continue;
                }

                //curMachine = GetBestAlterativeMachine(db, op, minStartDate, minStartTime, minToAssign);
                //int num = GetBestParallelMachine(db, curMachine, minStartDate, minStartTime, minToAssign);

                // loop per assegnare la slot a ritroso (il loop deve essere e questo livello per vedere se una fase può essere conclusa in una slot libera intermedia della giornata
                while (minToAssign > 0m)
                {
                    // loop per trovare il primo giorno disponibile
                    while (true)
                    {
                        ProductionMP firstUnusedSlot = GetIBCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, curMachine, num, machineTotalUninterruptableTime, holydays)
                                .Where(s => (s.ProdTimeMin >= machineTotalUninterruptableTime)).OrderByDescending(s => s.ProdEnd).ThenBy(s => s.ProdTimeMin)
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

                            mpsl.ProdStart = GetStartDateTime(firstUnusedSlot.ProdEnd, mpsl.ProdTimeMin.Value, curMachine, num, holydays);
                            mpsl.ProdEnd = firstUnusedSlot.ProdEnd;

                            mpsl.Status = 11;

                            db.ProductionMPs.InsertOnSubmit(mpsl);
                            db.SubmitChanges();
                            minToAssign -= mpsl.ProdTimeMin.GetValueOrDefault(0);
                            lastMachineId = curMachine.ID;
                            lastNum = num;
                            lastPickingItemId = op.piid;
                            break;
                        }
                        minStartDate = GetPreviousWorkingDay(minStartDate.Value, curMachine, holydays);
                        minStartTime = null;
                    }

                }

                posInSequence -= 1;
            }


        }

        public static void CreateProductionOrderScheduleIBC2(QuotationDataContext db, ProductionOrder po, int newScheduleFlag = 11, DateTime? forcedEndOfPhase = null)
        {

            //int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandQuantity(po.ID_Quotation, po.Quantity).Count();
            int posInSequence = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).Count();
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
            List<prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantityResult> ops = db.prc_LAB_MGet_LAB_ProductionScheduleByQuotationIDandPoIDandQuantity(po.ID_Quotation, po.ID, po.Quantity, po.ID_Company).OrderByDescending(o => o.qdPosition).ThenByDescending(o => o.mdPosition).ThenByDescending(o => o.piOrder).ToList();
            int machineTotalUninterruptableTime = 0;

            for (int j = 0; j < ops.Count; j++)
            {

                op = ops[j];

                ProductionMP completedPhase = db.ProductionMPs.FirstOrDefault(p => p.IDProductionOrder == po.ID && p.IDQuotationDetail == op.qdid);
                if (completedPhase != null)
                {
                    if (completedPhase.Status != 11)
                    {
                        posInSequence -= 1;
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


                if (forcedEndOfPhase != null)
                {
                    minStartDate = forcedEndOfPhase.Value.Date;
                    minStartTime = forcedEndOfPhase.GetValueOrDefault();
                    forcedEndOfPhase = null;
                }
                else
                {
                    ProductionMP lastProdEnd = db.ProductionMPs.Where(i => i.IDProductionOrder == po.ID).OrderBy(i => i.ProdStart).FirstOrDefault();
                    if (lastProdEnd == null)
                    {
                        minStartDate = GetPreviousWorkingDay(po.DeliveryDate.GetValueOrDefault(), holydays);
                        minStartTime = null;
                    }
                    else
                    {
                        minStartDate = lastProdEnd.ProdStart.Value.Date;
                        minStartTime = lastProdEnd.ProdStart.GetValueOrDefault();
                    }
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
                        curMachine = GetBestMachine2(db, po.ID_Company.Value, op, minStartDate, minStartTime, machineTotalUninterruptableTime, holydays, out num);
                    }
                }
                else
                // si verifica quando una fase di produzione non è assegnata ad una macchina (ad esempio sola manodopera)
                {
                    continue;
                }

                //curMachine = GetBestAlterativeMachine(db, op, minStartDate, minStartTime, minToAssign);
                //int num = GetBestParallelMachine(db, curMachine, minStartDate, minStartTime, minToAssign);

                // loop per assegnare la slot a ritroso (il loop deve essere e questo livello per vedere se una fase può essere conclusa in una slot libera intermedia della giornata


                while (minToAssign > 0m)
                {
                    // loop per trovare il primo giorno disponibile

                    while (true)
                    {
                        ProductionMP firstUnusedSlot = GetIBCUnusedSlotsOfADay2(db, minStartDate.Value, minStartTime, curMachine, num, machineTotalUninterruptableTime, holydays)
                                .Where(s => (s.ProdTimeMin >= machineTotalUninterruptableTime)).OrderBy(s => s.ProdEnd).ThenBy(s => s.ProdTimeMin)
                            .FirstOrDefault();
                        if (firstUnusedSlot != null)
                        {
                            //DateTime prodStart = firstUnusedSlot.ProdStart.GetValueOrDefault();
                            //DateTime prodEnd = GetEndDateTime(firstUnusedSlot.ProdStart, firstUnusedSlot.ProdTimeMin.Value, curMachine, num, holydays);
                            DateTime prodStart = GetStartDateTime(firstUnusedSlot.ProdEnd, minToAssign, curMachine, num, holydays);
                            DateTime prodEnd = firstUnusedSlot.ProdEnd.GetValueOrDefault();

                            DateTime curDate = prodStart.Date;

                            while (curDate >= prodEnd.Date)
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

                                db.ProductionMPs.InsertOnSubmit(mpsl);
                                db.SubmitChanges();
                                curDate = GetPreviousWorkingDay(curDate, holydays);

                            }
                            //minToAssign -= mpsl.ProdTimeMin.GetValueOrDefault(0);
                            minToAssign -= Math.Min(minToAssign, firstUnusedSlot.ProdTimeMin.Value);

                            lastMachineId = curMachine.ID;
                            lastNum = num;
                            lastPickingItemId = op.piid;

                            break;
                        }

                        minStartDate = GetPreviousWorkingDay(minStartDate.Value, curMachine, holydays);
                        minStartTime = null;
                    }

                }

                posInSequence -= 1;
            }


        }



        public enum SchedulingType
        {
            FiniteForwardCapacity, InfiniteBackwardCapacity
        }


        public static void CreateProductionOrderSchedule(QuotationDataContext db, ProductionOrder po, SchedulingType type)
        {

            if (type == SchedulingType.FiniteForwardCapacity)
                CreateProductionOrderScheduleFFC2(db, po);
            if (type == SchedulingType.InfiniteBackwardCapacity)
                CreateProductionOrderScheduleIBC2(db, po);
            EcoSystemGateway.RefreshMachineSchedule(db);

        }

        public static void DeleteProductionOrderSchedule(QuotationDataContext db, ProductionOrder po, bool allPhases = false)
        {
            //foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == po.ID && s.Status != 11))
            //{
            //    slot.QuotationDetail = null;
            //}
            if (allPhases == true)
                db.ProductionMPs.DeleteAllOnSubmit(db.ProductionMPs.Where(s => s.IDProductionOrder == po.ID));
            else
                db.ProductionMPs.DeleteAllOnSubmit(db.ProductionMPs.Where(s => s.IDProductionOrder == po.ID && (s.Status == 11 || s.Status == 15)));

            db.SubmitChanges();

        }

        public static void DeleteProductionOrderScheduleExceptions(QuotationDataContext db, ProductionOrder po)
        {

            db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(s => s.IDProductionOrder == po.ID));
            db.SubmitChanges();

        }

        public static void CloseProductionOrderSchedule(QuotationDataContext db, int poid, int? idQuotationDetail = null)
        {
            if (idQuotationDetail == null)
            {
                foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == poid && (s.Status == 11 || s.Status == 15) && s.IDQuotationDetail != -1))
                    slot.Status = 12;
            }
            else
            {
                ProductionMP curProductionMP = db.ProductionMPs.FirstOrDefault(s => s.IDProductionOrder == poid && (s.Status == 11 || s.Status == 15) && s.IDQuotationDetail == idQuotationDetail.Value);

                List<ProductionMP> curPhaseProductionMPs = db.ProductionMPs.Where(s => s.IDProductionOrder == poid && (s.Status == 11 || s.Status == 15) && s.IDPickingItem == curProductionMP.IDPickingItem).OrderBy(s => s.ProdStart).ToList();

                int pos = 0;
                for (int i = 0; i < curPhaseProductionMPs.Count; i++)
                {
                    ProductionMP slot = curPhaseProductionMPs[i];
                    if (slot.IDQuotationDetail == curProductionMP.IDQuotationDetail)
                        pos = i;
                    break;
                }

                for (int i = pos - 1; i >= 0 && i < pos; i--)
                {
                    ProductionMP slot = curPhaseProductionMPs[i];
                    if (slot.IDPickingItem == curProductionMP.IDPickingItem)
                    {
                        ProductionMP toUnload = db.ProductionMPs.FirstOrDefault(s => s.ID == slot.ID);
                        toUnload.Status = 12;
                    }
                    else
                        break;
                }
                for (int i = pos; i >= 0 && i < curPhaseProductionMPs.Count; i++)
                {
                    ProductionMP slot = curPhaseProductionMPs[i];
                    if (slot.IDPickingItem == curProductionMP.IDPickingItem)
                    {
                        ProductionMP toUnload = db.ProductionMPs.FirstOrDefault(s => s.ID == slot.ID);
                        toUnload.Status = 12;
                    }
                    else
                        break;
                }
            }
            EcoSystemGateway.RefreshMachineSchedule(db);
            db.SubmitChanges();

        }

        public static void CloseProductionOrderSchedule(QuotationDataContext db, int poid, int idProductionPhase, bool closePreviousPhases)
        {
            if (closePreviousPhases == false)
            {
                foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == poid && (s.Status == 11 || s.Status == 15) && s.ID == idProductionPhase))
                    slot.Status = 12;
            }
            else
            {
                foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == poid && (s.Status == 11 || s.Status == 15) && s.ID <= idProductionPhase))
                    slot.Status = 12;
            }
            db.SubmitChanges();

        }

        public static void ReopenProductionOrderSchedule(QuotationDataContext db, ProductionOrder po, int? idQuotationDetail = null)
        {
            if (idQuotationDetail == null)
            {
                foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == po.ID && s.Status == 12))
                    slot.Status = 11;
            }
            else
            {
                foreach (ProductionMP slot in db.ProductionMPs.Where(s => s.IDProductionOrder == po.ID && s.Status == 12 && s.IDQuotationDetail == idQuotationDetail.Value))
                    if (slot != null)
                        slot.Status = 11;
            }
            db.SubmitChanges();

        }

        public static void CreateProductionOrderSchedulesOfAQuotation(QuotationDataContext db, Quotation qt, SchedulingType type)
        {

            foreach (ProductionOrder po in qt.ProductionOrders)
                if (po.Status == 1)
                {
                    CreateProductionOrderSchedule(db, po, type);
                }

        }

        public static void DeleteProductionOrderSchedulesOfAQuotation(QuotationDataContext db, Quotation qt)
        {

            foreach (ProductionOrder po in qt.ProductionOrders)
            {
                DeleteProductionOrderSchedule(db, po);
                DeleteProductionOrderScheduleExceptions(db, po);
            }

        }


        public static void SyncroniseQuotationSubject(QuotationDataContext db, ProductionOrder po)
        {
            Quotation toUpdate = db.Quotations.FirstOrDefault(q => q.ID == po.ID_Quotation);
            if (toUpdate != null)
            {
                toUpdate.Subject = po.Description;
                toUpdate.Note = po.Note;
            }

        }

        public static void SyncroniseProductionOrdersDescription(QuotationDataContext db, Quotation qt, string description, string note, string note1, int contractor)
        {

            foreach (ProductionOrder po in qt.ProductionOrders)
            {
                po.Description = description;
                po.Note = note;
                po.Note1 = note1;
                po.ID_Contractor = contractor;
            }

        }

        public static void RecalcMPS(QuotationDataContext db, SchedulingType schedulingType)
        {
            db.ProductionMPs.DeleteAllOnSubmit(db.ProductionMPs.Where(p => p.Status == 11 && !(p.ProductionOrder.Status == 3 && (p.IDProductionMachine == 99 || p.IDProductionMachine == 100))));
            db.SubmitChanges();
            foreach (ProductionOrder po in db.ProductionOrders.Where(po => new int[] { 1, 2, 9 }.Contains(po.Status)).OrderBy(po => po.DeliveryDate))
            {
                ProductionOrderService.CreateProductionOrderSchedule(db, po, schedulingType);
            }
            EcoSystemGateway.RefreshMachineSchedule(db);

        }

        public static void Check_MCCG(QuotationDataContext db, GeneralDataContext dbg)
        {
            Configuration check = dbg.Configuration.First(d => d.ConfigKey == "MCCG");
            if (check.ConfigValue == "1")
            {
                check.ConfigValue = "0";
                dbg.SubmitChanges();
                EcoSystemGateway.RefreshMachineSchedule(db);
                db.SubmitChanges();
                Log.WriteMessage("Daily Manager: Check_MCCG eseguito per cambio stato su lista ordini macchina ECOSYSTEM");
            }

        }


        public static void RecalcVW_QUOPORCostsPrices(QuotationDataContext db, string startDate)
        {
            db.CommandTimeout = 600;
            db.prc_LAB_Upd_LAB_VW_QUOPORCostsPrices(startDate);
        }


        public static string GetNoteFromProduction(int idPo)
        {

            using (QuotationDataContext db = new QuotationDataContext())
            {

                Quotation curQuotation = db.ProductionOrders.FirstOrDefault(d => d.ID == idPo).Quotation;
                StringBuilder rs = new StringBuilder();

                rs.AppendLine(curQuotation.PriceCom);

                int[] poIds = db.ProductionOrders.Where(d => d.ID_Quotation == curQuotation.ID).Select(d => d.ID).ToArray();
                if (poIds != null)
                {
                    //List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.Where(s => poIds.Contains(s.ID_ProductionOrder.GetValueOrDefault()) && s.Status == 17).OrderBy(d => d.ProductionDate).ToList();
                    List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.Where(s => poIds.Contains(s.ID_ProductionOrder.GetValueOrDefault())).OrderBy(d => d.ProductionDate).ToList();
                    if (tss != null)
                    {

                        foreach (ProductionOrderTechSpec ts in tss)
                        {
                            rs.AppendLine(
                                      (string.Format("{0} {1} {2}: ", ts.ProductionDate.Value.ToString("dd/MM/yyyy"), ts.Employee.Surname, ts.Employee.Name)) +
                                      (ts.CodiceMarcaInchiostro != null ? "SERIGRAFIA SPECIALE: " : "") +
                                      (ts.CodiceMarcaInchiostro != null ? ("Codice marca inchiostro: " + ts.CodiceMarcaInchiostro + " ,") : "") +
                                      (ts.Ricetta != null ? ("Ricetta: " + (ts.Ricetta.GetValueOrDefault() ? "SI" : "NO") + " ,") : "") +
                                      (ts.TelaioNumeroFili != null ? ("Telaio numero fili: " + ts.TelaioNumeroFili.ToString() + " ,") : "") +
                                      (ts.GelatinaSpessore != null ? ("Gelatina spessore: " + ts.GelatinaSpessore.ToString() + " ,") : "") +
                                      (ts.RaclaInclinazione != null ? ("Racla inclinazione: " + ts.RaclaInclinazione.ToString() + " ,") : "") +
                                      (ts.RaclaDurezzaSpigolo != null ? ("Racla durezza spigolo: " + ts.RaclaDurezzaSpigolo.ToString() + " ,") : "") +
                                      (ts.CodiceMarcaFilm != null ? "FILM CALDO: " : "") +
                                      (ts.CodiceMarcaFilm != null ? ("Codice marca film: " + ts.CodiceMarcaFilm + " ,") : "") +
                                      (ts.ClicheReso != null ? ("Cliche: " + (ts.ClicheReso == "C" ? "Reso al Cliente" : "Ns archivio") + " ,") : "") +
                                      (ts.ClicheCondizioni != null ? ("Condizioni: " + (ts.ClicheCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                      (ts.StampaTemperatura != null ? ("Temperatura stampa: " + ts.StampaTemperatura.ToString() + " ,") : "") +
                                      (ts.AltreInfo != null ? ("Info: " + ts.AltreInfo.ToString() + " ,") : "") +
                                      (ts.FustellaResa != null ? ("Fustella: " + (ts.FustellaResa == "C" ? "Resa al Cliente" : "Ns archivio") + " ,") : "") +
                                      (ts.FustellaCondizioni != null ? ("Condizioni: " + (ts.FustellaCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                      (ts.AltreNoteDaProduzione != null ? ("ALTRE NOTE: " + ts.AltreNoteDaProduzione + " ,") : "")
                           );
                        }
                    }
                }
                return rs.ToString();

            }
        }

        public static string GetNoteFromPreviousProduction(int idQuotation)
        {
            if (idQuotation != -1)
                using (QuotationDataContext db = new QuotationDataContext())
                {

                    Quotation curQuotation = db.Quotations.FirstOrDefault(d => d.ID == idQuotation);
                    StringBuilder rs = new StringBuilder();

                    rs.AppendLine(curQuotation.PriceCom);

                    int[] poIds = db.ProductionOrders.Where(d => d.ID_Quotation == curQuotation.ID).Select(d => d.ID).ToArray();
                    if (poIds != null)
                    {
                        //List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.Where(s => poIds.Contains(s.ID_ProductionOrder.GetValueOrDefault()) && s.Status == 17).OrderBy(d => d.ProductionDate).ToList();
                        List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.Where(s => poIds.Contains(s.ID_ProductionOrder.GetValueOrDefault())).OrderBy(d => d.ProductionDate).ToList();
                        if (tss != null)
                        {

                            foreach (ProductionOrderTechSpec ts in tss)
                            {
                                rs.AppendLine(
                                          (string.Format("{0} {1} {2}: ", ts.ProductionDate.Value.ToString("dd/MM/yyyy"), ts.Employee.Surname, ts.Employee.Name)) +
                                          (ts.CodiceMarcaInchiostro != null ? "SERIGRAFIA SPECIALE: " : "") +
                                          (ts.CodiceMarcaInchiostro != null ? ("Codice marca inchiostro: " + ts.CodiceMarcaInchiostro + " ,") : "") +
                                          (ts.Ricetta != null ? ("Ricetta: " + (ts.Ricetta.GetValueOrDefault() ? "SI" : "NO") + " ,") : "") +
                                          (ts.TelaioNumeroFili != null ? ("Telaio numero fili: " + ts.TelaioNumeroFili.ToString() + " ,") : "") +
                                          (ts.GelatinaSpessore != null ? ("Gelatina spessore: " + ts.GelatinaSpessore.ToString() + " ,") : "") +
                                          (ts.RaclaInclinazione != null ? ("Racla inclinazione: " + ts.RaclaInclinazione.ToString() + " ,") : "") +
                                          (ts.RaclaDurezzaSpigolo != null ? ("Racla durezza spigolo: " + ts.RaclaDurezzaSpigolo.ToString() + " ,") : "") +
                                          (ts.CodiceMarcaFilm != null ? "FILM CALDO: " : "") +
                                          (ts.CodiceMarcaFilm != null ? ("Codice marca film: " + ts.CodiceMarcaFilm + " ,") : "") +
                                          (ts.ClicheReso != null ? ("Cliche: " + (ts.ClicheReso == "C" ? "Reso al Cliente" : "Ns archivio") + " ,") : "") +
                                          (ts.ClicheCondizioni != null ? ("Condizioni: " + (ts.ClicheCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                          (ts.StampaTemperatura != null ? ("Temperatura stampa: " + ts.StampaTemperatura.ToString() + " ,") : "") +
                                          (ts.AltreInfo != null ? ("Info: " + ts.AltreInfo.ToString() + " ,") : "") +
                                          (ts.FustellaResa != null ? ("Fustella: " + (ts.FustellaResa == "C" ? "Resa al Cliente" : "Ns archivio") + " ,") : "") +
                                          (ts.FustellaCondizioni != null ? ("Condizioni: " + (ts.FustellaCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                          (ts.AltreNoteDaProduzione != null ? ("ALTRE NOTE: " + ts.AltreNoteDaProduzione + " ,") : "")
                               );
                            }
                        }
                    }
                    return rs.ToString();

                }
            else return null;
        }


        public static string GetAccountNotesFromProduction(int idPo)
        {

            using (QuotationDataContext db = new QuotationDataContext())
            {
                StringBuilder rs = new StringBuilder();
                List<prc_LAB_MGet_LAB_NoteByPhaseAndPOIDResult> nps = db.prc_LAB_MGet_LAB_NoteByPhaseAndPOID(idPo).ToList();
                if (nps != null)
                {
                    foreach (prc_LAB_MGet_LAB_NoteByPhaseAndPOIDResult ts in nps)
                    {
                        rs.AppendLine(
                                  (string.Format("{0} {1} ({2}): ", ts.ProductionDate.Value.ToString("dd/MM/yyyy"), ts.UniqueName, ts.ItemDescription)) +
                                  (ts.Note != null ? ("NOTE OdP corrente: " + ts.Note) : ""));
                    }
                }
                return rs.ToString();

            }
        }

        public static bool StartPauseCurrentProductionPhase(QuotationDataContext db, int userId, int productionOrderId, int minQuotatioDetailId, bool forzaChiusura = false)
        {

            bool onProd = false;


            var toManageTimeStamp = db.ProductionTimeStamps.OrderByDescending(po => po.ID).FirstOrDefault(po => po.IDProductionOrder == productionOrderId && po.MinIDQuotationDetail == minQuotatioDetailId);
            if (!forzaChiusura)
            {
                if (toManageTimeStamp == null)
                {
                    toManageTimeStamp = new ProductionTimeStamp();
                    toManageTimeStamp.IDProductionOrder = productionOrderId;
                    toManageTimeStamp.MinIDQuotationDetail = minQuotatioDetailId;
                    toManageTimeStamp.ProdStart = DateTime.Now;
                    toManageTimeStamp.IdUser = userId;
                    db.ProductionTimeStamps.InsertOnSubmit(toManageTimeStamp);
                    onProd = true;
                }
                else if (toManageTimeStamp != null && toManageTimeStamp.ProdEnd != null)
                {
                    toManageTimeStamp = new ProductionTimeStamp();
                    toManageTimeStamp.IDProductionOrder = productionOrderId;
                    toManageTimeStamp.MinIDQuotationDetail = minQuotatioDetailId;
                    toManageTimeStamp.ProdStart = DateTime.Now;
                    toManageTimeStamp.IdUser = userId;
                    db.ProductionTimeStamps.InsertOnSubmit(toManageTimeStamp);
                    onProd = true;
                }
                else if (toManageTimeStamp != null && toManageTimeStamp.ProdEnd == null)
                {
                    toManageTimeStamp.ProdEnd = DateTime.Now;
                    toManageTimeStamp.IdUser = userId;
                    onProd = false;
                }
            }
            else
            {
                if (toManageTimeStamp == null)
                {
                    toManageTimeStamp = new ProductionTimeStamp();
                    toManageTimeStamp.IDProductionOrder = productionOrderId;
                    toManageTimeStamp.MinIDQuotationDetail = minQuotatioDetailId;
                    toManageTimeStamp.ProdStart = DateTime.Now;
                    toManageTimeStamp.ProdEnd = DateTime.Now;
                    toManageTimeStamp.IdUser = userId;
                    db.ProductionTimeStamps.InsertOnSubmit(toManageTimeStamp);
                    onProd = false;
                }
                else
                {
                    toManageTimeStamp.ProdEnd = DateTime.Now;
                    toManageTimeStamp.IdUser = userId;
                    onProd = false;
                }
            }

            return onProd;

        }

        public static void MassimizzaPriorita_Grouped(QuotationDataContext db, int poId, int qdId)
        {


            VW_ProductionExtMP hitItem = null;
            List<VW_ProductionExtMP> poMPSItems = db.VW_ProductionExtMPs.Where(p => p.IDProductionOrder == poId).OrderBy(m => Convert.ToInt32(m.Order)).ToList();


            foreach (VW_ProductionExtMP poMPSItem in poMPSItems)
            {
                if (poMPSItem.Status != 11)
                {
                    poMPSItem.nOrder = -1;
                }
                else if (poMPSItem.IDQuotationDetail == qdId)
                {
                    hitItem = poMPSItem;
                    poMPSItem.forced = true;
                }
                else
                    poMPSItem.nOrder = Convert.ToInt32(poMPSItem.Order);

            }


            foreach (VW_ProductionExtMP poMPSItem in poMPSItems.Where(d => d.nOrder < hitItem.nOrder).OrderByDescending(n => n.nOrder))
            {
                if (poMPSItem.IDPickingItem == hitItem.IDPickingItem)
                {
                    poMPSItem.forced = true;
                }
                else
                    break;
            }
            foreach (VW_ProductionExtMP poMPSItem in poMPSItems.Where(d => d.nOrder > hitItem.nOrder).OrderBy(n => n.nOrder))
            {
                if (poMPSItem.IDPickingItem == hitItem.IDPickingItem)
                {
                    poMPSItem.forced = true;
                }
                else
                    break;
            }

            foreach (VW_ProductionExtMP poMPSItem in poMPSItems)
                if (poMPSItem.forced)
                    poMPSItem.nOrder = 0;



            db.ProductionMPSExceptions.DeleteAllOnSubmit(db.ProductionMPSExceptions.Where(m => m.IDProductionOrder == hitItem.IDProductionOrder && m.NewOrder != null));
            db.SubmitChanges();
            int newOrder = 1;
            foreach (VW_ProductionExtMP item in poMPSItems.OrderBy(p => p.nOrder).ThenBy(p => p.Order))
            {
                ProductionMPSException newException = new ProductionMPSException();
                newException.IDProductionOrder = item.IDProductionOrder;
                newException.IDPickingItem = item.IDPickingItem;
                newException.IDMacroItem = item.IDMacroItem;
                newException.IDMacroItemDetail = item.IDMacroItemDetail;
                newException.IDQuotationDetail = (item.IDQuotationDetail == -1 ? null : item.IDQuotationDetail);
                newException.NewOrder = newOrder.ToString();
                db.ProductionMPSExceptions.InsertOnSubmit(newException);
                newOrder += 1;
            }

        }



    }
}
