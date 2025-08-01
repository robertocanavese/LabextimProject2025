using System;
using System.Collections.Generic;
using System.Linq;
using DLLabExtim;
using System.Web;
using CMLabExtim;
using CMLabExtim.WODClasses;

namespace UILabExtim
{
    public class ProductionOrderController : BaseController
    {
        public static readonly string POIdKey = "POid";
        public static readonly string PONameKey = "POName";
        public static readonly string POCustomerIdKey = "POcid";
        public static readonly string POCustomerOrderIdKey = "POcoid";
        public static readonly string POQuotationIdKey = "POquo";
        public static readonly string POQuantityKey = "POq";
        public static readonly string POStartDateKey = "POsd";
        public static readonly string DTIdKey = "DTid";

        public int POIdParameter
        {
            get
            {
                object temp = Request.QueryString[POIdKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        public string PONameParameter
        {
            get
            {
                object temp = Server.UrlDecode(Request.QueryString[PONameKey]);
                return temp == null ? string.Empty : temp.ToString();
            }
        }

        public int POCustomerIdParameter
        {
            get
            {
                object temp = Request.QueryString[POCustomerIdKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        public int POCustomerOrderIdParameter
        {
            get
            {
                object temp = Request.QueryString[POCustomerOrderIdKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        public int POQuotationIdParameter
        {
            get
            {
                object temp = Request.QueryString[POQuotationIdKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }

        }

        public int POQuantityParameter
        {
            get
            {
                object temp = Request.QueryString[POQuantityKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        public DateTime POStartDateParameter
        {
            get
            {
                object temp = Request.QueryString[POStartDateKey];
                return temp == null ? DateTime.Now.Date : Convert.ToDateTime(temp);
            }
        }

        public int DTIdParameter
        {
            get
            {
                object temp = Request.QueryString[DTIdKey];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        protected void PrintProductionOrder(QuotationDataContext context)
        {
            var _curProductionOrder = context.ProductionOrders.FirstOrDefault(po => po.ID == POIdParameter);

            ProductionOrderHeader = new KeyValuePair<int, string>(_curProductionOrder.ID,
                _curProductionOrder.Description);
            QuotationHeader = new KeyValuePair<int, string>(_curProductionOrder.Quotation.ID,
                _curProductionOrder.Quotation.Subject);

            Response.Write("<script>");
            Response.Write("window.resizeTo(850,650);window.moveTo((screen.width-850)/2,(screen.height-650)/2);");
            Response.Write("</script>");
            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                    QuotationKey,
                    _curProductionOrder.Quotation == null ? "-1" : _curProductionOrder.Quotation.ID.ToString(),
                    QuotationPrintPage,
                    ReportTypeKey,
                    2,
                    ReportOnProductionQuantityKey,
                    _curProductionOrder.Quantity,
                    ProductionOrderKey,
                    _curProductionOrder.ID,
                    ReportOnProductionIdCompanyKey,
                    _curProductionOrder.ID_Company
                    )
                , true);
        }

        protected void PrintPOFinalCost(QuotationDataContext context)
        {
            var _curProductionOrder = context.ProductionOrders.FirstOrDefault(po => po.ID == POIdParameter);

            ProductionOrderHeader = new KeyValuePair<int, string>(_curProductionOrder.ID,
                _curProductionOrder.Description);
            QuotationHeader = new KeyValuePair<int, string>(_curProductionOrder.Quotation.ID,
                _curProductionOrder.Quotation.Subject);

            Response.Write("<script>");
            Response.Write("window.resizeTo(850,650);window.moveTo((screen.width-850)/2,(screen.height-650)/2);");
            Response.Write("</script>");
            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}&{5}={6}&{7}={8}",
                    QuotationKey,
                    _curProductionOrder.Quotation == null ? "-1" : _curProductionOrder.Quotation.ID.ToString(),
                    QuotationPrintPage,
                    ReportTypeKey,
                    3,
                    ReportOnProductionQuantityKey,
                    _curProductionOrder.Quantity,
                    ProductionOrderKey,
                    _curProductionOrder.ID
                    )
                , true);
        }

        protected void CloneProductionOrder(QuotationDataContext context, ProductionOrderService.SchedulingType type)
        {
            var _curProductionOrder = context.ProductionOrders.FirstOrDefault(po => po.ID == POIdParameter);

            var _newProductionOrder = new ProductionOrder();
            _newProductionOrder.Description = _curProductionOrder.Description;
            _newProductionOrder.ID_Company = _curProductionOrder.ID_Company;
            _newProductionOrder.Cost = _curProductionOrder.Cost;
            _newProductionOrder.Customer = _curProductionOrder.Customer;
            _newProductionOrder.CustomerOrder = _curProductionOrder.CustomerOrder;
            _newProductionOrder.DeliveryDate = null;
            _newProductionOrder.DirectSupply = _curProductionOrder.DirectSupply;
            _newProductionOrder.Note = _curProductionOrder.Note;
            _newProductionOrder.Quantity = _curProductionOrder.Quantity;
            _newProductionOrder.Quotation = _curProductionOrder.Quotation;
            _newProductionOrder.StartDate = DateTime.Now.Date;
            _newProductionOrder.Statuse = _curProductionOrder.Statuse;

            context.ProductionOrders.InsertOnSubmit(_newProductionOrder);
            context.SubmitChanges();

            ProductionOrderService.DeleteProductionOrderSchedule(context, _newProductionOrder);
            if (_newProductionOrder.Status == 1 || _newProductionOrder.Status == 9)
            {
                ProductionOrderService.CreateProductionOrderSchedule(context, _newProductionOrder, type);
            }
            context.SubmitChanges();

            Response.Redirect("ProductionOrderPopup.aspx?" + POIdKey + "=" + _newProductionOrder.ID, true);
        }

        protected int CreateDummyQuotation(ProductionOrder productionOrder)
        {
            var _context = new QuotationDataContext();

            var _newQuotation = new Quotation();
            _newQuotation.CustomerCode = productionOrder.ID_Customer;
            _newQuotation.Subject = productionOrder.Description + " (Automatico da OdP " + productionOrder.Number + " [" +
                                    productionOrder.ID + "])";
            _newQuotation.Q1 = Convert.ToInt32(productionOrder.Quantity);
            _newQuotation.Q2 = Convert.ToInt32(productionOrder.Quantity);
            _newQuotation.Q3 = -1;
            _newQuotation.Q4 = -1;
            _newQuotation.Q5 = -1;
            _newQuotation.Date = productionOrder.StartDate;
            _newQuotation.Draft = false;
            _newQuotation.MarkUp = productionOrder.DirectSupply
                ? Convert.ToInt32(((Convert.ToDecimal(productionOrder.Price) - Convert.ToDecimal(productionOrder.Cost))) /
                                  Convert.ToDecimal(productionOrder.Cost) * 100m + 100m)
                : 100;
            if (GetCurrentEmployee() != null)
                _newQuotation.ID_Owner = GetCurrentEmployee().ID;
            _newQuotation.Status = productionOrder.Status;
            _newQuotation.Note = productionOrder.Note;

            var _uniqueQuotationDetail = new QuotationDetail();
            _uniqueQuotationDetail.Quantity = 1;
            _uniqueQuotationDetail.Position = "";
            _uniqueQuotationDetail.Cost = productionOrder.DirectSupply
                ? Convert.ToDecimal(productionOrder.Cost)
                : Convert.ToDecimal(productionOrder.Price);
            _uniqueQuotationDetail.Price = Convert.ToDecimal(productionOrder.Price);
            _uniqueQuotationDetail.MarkUp = productionOrder.DirectSupply
                ? Convert.ToInt32(((Convert.ToDecimal(productionOrder.Price) - Convert.ToDecimal(productionOrder.Cost))) /
                                  Convert.ToDecimal(productionOrder.Cost) * 100m + 100m)
                : 100;
            _uniqueQuotationDetail.Percentage = 100;
            _uniqueQuotationDetail.Inserted = true;
            _uniqueQuotationDetail.ItemTypeCode = 3;
            _uniqueQuotationDetail.TypeCode = 9;
            _uniqueQuotationDetail.ItemTypeDescription = "VOCE GENERATA AUTOMATICAMENTE";
            _uniqueQuotationDetail.UM = 7;
            _uniqueQuotationDetail.SelectPhase = true;
            _uniqueQuotationDetail.Multiply = false;
            _uniqueQuotationDetail.Save = true;
            _uniqueQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;

            _newQuotation.QuotationDetails.Add(_uniqueQuotationDetail);

            _context.Quotations.InsertOnSubmit(_newQuotation);
            _context.SubmitChanges();

            return _newQuotation.ID;
        }

        protected int UpdateDummyQuotation(ProductionOrder productionOrder)
        {
            var _context = new QuotationDataContext();

            var _toUpdateQuotation = _context.Quotations.SingleOrDefault(q => q.ID == productionOrder.ID_Quotation);


            //if (_toUpdateQuotation.Draft == true)
            if (_toUpdateQuotation.Q3 == -1 && _toUpdateQuotation.Q4 == -1 && _toUpdateQuotation.Q5 == -1)
            {
                _toUpdateQuotation.CustomerCode = productionOrder.ID_Customer;
                _toUpdateQuotation.Subject = productionOrder.Description + " (Automatico da OdP " +
                                             productionOrder.Number + " [" + productionOrder.ID + "])";
                _toUpdateQuotation.Q1 = Convert.ToInt32(productionOrder.Quantity);
                _toUpdateQuotation.Q2 = Convert.ToInt32(productionOrder.Quantity);
                _toUpdateQuotation.Q3 = -1;
                _toUpdateQuotation.Q4 = -1;
                _toUpdateQuotation.Q5 = -1;
                _toUpdateQuotation.Date = productionOrder.StartDate;
                _toUpdateQuotation.Draft = false;
                _toUpdateQuotation.MarkUp = productionOrder.DirectSupply
                    ? Convert.ToInt32(((Convert.ToDecimal(productionOrder.Price) -
                                        Convert.ToDecimal(productionOrder.Cost))) /
                                      Convert.ToDecimal(productionOrder.Cost) * 100m + 100m)
                    : 100;
                _toUpdateQuotation.Status = productionOrder.Status;
                _toUpdateQuotation.Note = productionOrder.Note;

                //QuotationDetail _uniqueQuotationDetail = new QuotationDetail();
                //_uniqueQuotationDetail.Quantity = 1;
                //_uniqueQuotationDetail.Position = "";
                //_uniqueQuotationDetail.Cost = productionOrder.DirectSupply == true ? Convert.ToDecimal(productionOrder.Cost) : Convert.ToDecimal(productionOrder.Price);
                //_uniqueQuotationDetail.Price = Convert.ToDecimal(productionOrder.Price);
                //_uniqueQuotationDetail.MarkUp = productionOrder.DirectSupply == true ? Convert.ToInt32(((Convert.ToDecimal(productionOrder.Price) - Convert.ToDecimal(productionOrder.Cost))) / Convert.ToDecimal(productionOrder.Cost) * 100m + 100m) : 100;
                //_uniqueQuotationDetail.Percentage = 100;
                //_uniqueQuotationDetail.Inserted = true;
                //_uniqueQuotationDetail.ItemTypeCode = 3;
                //_uniqueQuotationDetail.TypeCode = 9;
                //_uniqueQuotationDetail.ItemTypeDescription = "VOCE GENERATA AUTOMATICAMENTE";
                //_uniqueQuotationDetail.UM = 7;
                //_uniqueQuotationDetail.SelectPhase = true;
                //_uniqueQuotationDetail.Multiply = false;
                //_uniqueQuotationDetail.Save = true;
                //_uniqueQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;

                //_context.QuotationDetails.DeleteAllOnSubmit(_toUpdateQuotation.QuotationDetails);
                //_context.SubmitChanges();

                //_toUpdateQuotation.QuotationDetails.Add(_uniqueQuotationDetail);

                _context.SubmitChanges();
            }

            return _toUpdateQuotation.ID;
        }

        public IEnumerable<CloseOfDay> GetCloseOfDays(QuotationDataContext db)
        {
            return db.CloseOfDays;
        }

        public DHL GetRealTimeDataFromBOBST1()
        {
#if !DEBUG
            try
            {
                return WODGateway.GetRealTimeDataFromBOBST1();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromBOBST1 - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }
        public Query_OperationCount GetHistoricalDataFromBOBST1()
        {
#if !DEBUG
            try
            {
                return WODGateway.GetHistoricalDataFromBOBST1(POIdParameter);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromBOBST1 - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }


        public OdPBag GetRealTimeDataFromBOBST()
        {
#if !DEBUG
            try
            {
                return WODGateway.GetCurOdP();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromBOBST - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }
        public OdPBag GetHistoricalDataFromBOBST(int idPo, int quantity)
        {
#if !DEBUG
            try
            {
                return WODGateway.GetHistoricalDataFromBOBST(idPo, quantity);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromBOBST - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }


        public OdPBag GetRealTimeDataFromEcoSystem(QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return EcoSystemGateway.GetCurOdP(db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromEcoSystem - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }
        public OdPBag GetHistoricalDataFromEcoSystem(int idPo, QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return EcoSystemGateway.GetOdPHistoricalData(idPo, db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromEcoSystem - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }

        public OdPBag GetRealTimeDataFromSilkFoil(QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return Snap7Gateway.GetCurOdP();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromSilkFoil - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif

        }
        public OdPBag GetHistoricalDataFromSilkFoil(int idPo, QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return Snap7Gateway.GetOdPHistoricalData(idPo, db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromSilkFoil - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }

        public OdPBag GetRealTimeDataFromEuroProgetti(QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return EuroProgettiGateway.GetCurOdP(db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromEuroProgetti - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif

        }
        public OdPBag GetHistoricalDataFromEuroProgetti(int idPo, QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return EuroProgettiGateway.GetOdPHistoricalData(idPo, db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromEuroProgetti - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }

        public OdPBag GetRealTimeDataFromZechini(QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return ZechiniGateway.GetCurOdP(db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetRealTimeDataFromZechini - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif

        }
        public OdPBag GetHistoricalDataFromZechini(int idPo, QuotationDataContext db)
        {
#if !DEBUG
            try
            {
                return ZechiniGateway.GetOdPHistoricalData(idPo, db);
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("GetHistoricalDataFromZechini - {0}", ex.Message));
                return null;
            }
#else
            return null;
#endif
        }

    }
}