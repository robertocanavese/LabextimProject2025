using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMLabExtim;
using CMLabExtim.Menuitems;
using DLLabExtim;
using System.Data.Linq;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Configuration;


namespace UILabExtim
{
    public class BaseController : Page
    {
        public enum LabExtimErrorType
        {
            InputFailed,
            ImportFailed,
            ObsoleteItem,
            ModelLoadingFailed,
            ModelObsoleteItem,
            ItemLoadingFailed,
            CannotDelete,
            Empty,
            CustomerIsMandatory,
            QuantityIsMandatory,
            PriceIsMandatoryForAuto,
            DeliveryDateIsMandatory,
            DeliveryDateIsInvalid
        }

        //protected Menu CustomMenu;
        public enum MenuType
        {
            MenuPickingItems,
            MenuOperations,
            MenuMaterials,
            MenuProdRecord,
            MenuOperationNoPhases,
            MenuQuotationTemplates
        }

        public enum ReportType
        {
            Customer,
            Technical,
            ProductionOrder,
            POFinalCost
        }

        public const int INTERNAL_SUPPLIERCODE = 11010481;
        protected static readonly string QuotationKey = "P0";
        protected static readonly string ReportTypeKey = "P1";
        protected static readonly string ReportOnProductionQuantityKey = "P4";
        protected static readonly string ReportOnProductionIdCompanyKey = "P9";
        protected static readonly string CustomerOrderKey = "P5";
        protected static readonly string ProductionOrderKey = "P6";
        protected static readonly string CustomerKey = "P7";
        protected static readonly string SelectionFormulaKey = "P8";
        protected static readonly string GenericReportKey = "P2";
        protected static readonly string LocationKey = "P10";
        protected static readonly string DeliveryTripKey = "P11";


        public LabextimUser WebUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (Session["LabextimUser"] == null)
                    {
                        LabextimUser user = new LabextimUser(Membership.GetUser());
                        Session["LabextimUser"] = user;
                    }
                    return Session["LabextimUser"] as LabextimUser;
                }
                return null;
            }
        }


        public int CurrentCompanyId
        {
            get
            {
                if (WebUser.Employee.Company != null)
                {
                    int idCompany = WebUser.Employee.Company.ID;
                    return idCompany;
                }
                return -1;
            }
            set { 

            }
        }

        public string SessionTimeStamp
        {
            get
            {
                return Session["SessionTimeStamp"] != null ? Session["SessionTimeStamp"].ToString() : string.Empty;
            }
            set { Session["SessionTimeStamp"] = value; }
        }

        public bool LoadPersistedQuotation
        {
            get
            {
                if (Session["LoadPersistedQuotation"] == null)
                    Session["LoadPersistedQuotation"] = false;
                return Convert.ToBoolean(Session["LoadPersistedQuotation"]);
            }
            set { Session["LoadPersistedQuotation"] = value; }
        }

        public Dictionary<string, string> GlobalConfiguration
        {
            get
            {
                return Session["GlobalConfiguration"] as Dictionary<string, string> ?? new Dictionary<string, string>();
            }
            set { Session["GlobalConfiguration"] = value; }
        }

        public string QuotationParameter
        {
            get
            {
                object temp = Request.QueryString[QuotationKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
            }
        }

        public int ReportTypeParameter
        {
            get
            {
                object temp = Request.QueryString[ReportTypeKey];
                return temp == null ? 0 : Convert.ToInt32(temp);
            }
        }

        public int ReportOnProductionQuantityParameter
        {
            get
            {
                object temp = Request.QueryString[ReportOnProductionQuantityKey];
                return temp.ToString() == string.Empty ? 0 : Convert.ToInt32(temp);
            }
        }

        public int ReportOnProductionIdCompanyParameter
        {
            get
            {
                object temp = Request.QueryString[ReportOnProductionIdCompanyKey];
                return temp.ToString() == string.Empty ? 0 : Convert.ToInt32(temp);
            }
        }

        public string CustomerOrderParameter
        {
            get
            {
                object temp = Request.QueryString[CustomerOrderKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
            }
        }

        public string ProductionOrderParameter
        {
            get
            {
                object temp = Request.QueryString[ProductionOrderKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
            }
        }

        public string CustomerParameter
        {
            get
            {
                object temp = Request.QueryString[CustomerKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(9, ' ').Substring(0, 9);
            }
        }

        public string LocationParameter
        {
            get
            {
                object temp = Request.QueryString[LocationKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(9, ' ').Substring(0, 9);
            }
        }

        public string DeliveryTripParameter
        {
            get
            {
                object temp = Request.QueryString[DeliveryTripKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(9, ' ').Substring(0, 9);
            }
        }

        public string SelectionFormulaParameter
        {
            get
            {
                object temp = Request.QueryString[SelectionFormulaKey];
                return temp == null ? string.Empty : temp.ToString();
            }
        }

        public int UsageParameter
        {
            get
            {
                object temp = Request.QueryString["Usage"];
                return temp == null ? 0 : Convert.ToInt32(temp);
            }
        }

        public string ReportFileNameParameter
        {
            get
            {
                object temp = Request.QueryString[GenericReportKey];
                return temp.ToString();
            }
        }

        public KeyValuePair<string, string> TableHeader
        {
            get
            {
                if (Session["TableHeader"] == null)
                {
                    return new KeyValuePair<string, string>(ReportFileNameParameter, "Senza nome");
                }
                return (KeyValuePair<string, string>)Session["TableHeader"];
            }
            set { Session["TableHeader"] = value; }
        }

        public KeyValuePair<int, string> QuotationHeader
        {
            get
            {
                if (Session["QuotationHeader"] == null)
                {
                    return
                        new KeyValuePair<int, string>(
                            QuotationParameter != string.Empty ? Convert.ToInt32(QuotationParameter) : -1, "Senza nome");
                }
                return (KeyValuePair<int, string>)Session["QuotationHeader"];
            }
            set { Session["QuotationHeader"] = value; }
        }

        public KeyValuePair<int, string> CustomerOrderHeader
        {
            get
            {
                if (Session["CustomerOrderHeader"] == null)
                {
                    return
                        new KeyValuePair<int, string>(
                            CustomerOrderParameter != string.Empty ? Convert.ToInt32(CustomerOrderParameter) : -1,
                            "Senza nome");
                }
                return (KeyValuePair<int, string>)Session["CustomerOrderHeader"];
            }
            set { Session["CustomerOrderHeader"] = value; }
        }

        public KeyValuePair<int, string> ProductionOrderHeader
        {
            get
            {
                if (Session["ProductionOrderHeader"] == null)
                {
                    return
                        new KeyValuePair<int, string>(
                            ProductionOrderParameter != string.Empty ? Convert.ToInt32(ProductionOrderParameter) : -1,
                            "Senza nome");
                }
                return (KeyValuePair<int, string>)Session["ProductionOrderHeader"];
            }
            set { Session["ProductionOrderHeader"] = value; }
        }

        public static string QuotationConsolePage
        {
            get
            {
                //return @"~/QuotationConsole.aspx";
                return @"~/QuotationConsole2.aspx";
            }
        }

        public static string TempQuotationConsolePage
        {
            get { return @"~/QuotationConsole2.aspx"; }
        }

        public static string QuotationTemplateConsolePage
        {
            get { return @"~/QuotationTemplateConsole.aspx"; }
        }

        public static string MacroItemConsolePage
        {
            get { return @"~/MacroItemsConsole.aspx"; }
        }

        public static string QuotationPrintPage
        {
            get { return @"~/QuotationPrint.aspx"; }
        }

        public static string QuotationTemplatePrintPage
        {
            get { return @"~/QuotationTemplatePrint.aspx"; }
        }

        public static string CustomerOrderConsolePage
        {
            get { return @"~/CustomerOrdersConsole.aspx"; }
        }

        public static string ProductionOrderConsolePage
        {
            get { return @"~/ProductionOrderConsole.aspx"; }
        }

        public static string GenericPrintPage
        {
            get { return @"~/GenericPrint.aspx"; }
        }

        public static string HomePage
        {
            get { return @"~/Home.aspx"; }
        }

        public GeneralDataContext GeneralDataContext { get; private set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Verifico se arrivo con un mac e cambio il rendering
            var safariRegex = @"AppleWebKit/(?'version'(?'major'\d)(?'minor'\d+)(?'letters'\w*))";
            var useragent = Request["http_user_agent"];
            if (Regex.IsMatch(useragent, safariRegex))
            {
                Page.ClientTarget = "uplevel";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (WebUser.Member == null)
                {
                    Session.Clear();
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("~/Login.aspx?AccountDisabled");
                }


                //InitGeneralDataContext();
                var _result = new QuotationService().SetSessionLock(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

                if (WebUser.Employee.Company != null)
                {
                    int idCompany = WebUser.Employee.Company.ID;
                    Session["CurrentCompanyId"] = idCompany;
                }

            }
            Page_PreInit(null, e);
            base.OnInit(e);
        }

        //public void InitGeneralDataContext()
        //{
        //    //m_GeneralDataContext = Session["GeneralDataContext"] as GeneralDataContext;
        //    if (GeneralDataContext == null)
        //    {
        //        GeneralDataContext = new GeneralDataContext();
        //        //Session["GeneralDataContext"] = m_GeneralDataContext;
        //    }
        //}

        protected override void OnUnload(EventArgs e)
        {
            //Session["GeneralDataContext"] = m_GeneralDataContext;
            base.OnUnload(e);
        }

        /// <summary>
        /// idCompany: -1 per menu multiaziendali (solo MenuType.MenuQuotationTemplates), codice Azienda negli altri casi
        /// </summary>
        /// <param name="customMenu"></param>
        /// <param name="menuType"></param>
        /// <param name="idCompany"></param>
        public void LoadHeaderMenu(Menu customMenu, MenuType menuType, int idCompany)
        {
            var _BNMenuItems = new List<BnMenuItem>();

            switch (menuType)
            {
                case MenuType.MenuPickingItems:
                    {
                        if (Cache[idCompany.ToString() + "|" + menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                IEnumerable<int> _distinctTypes = _qc.PickingItems
                                    .Join(_qc.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                                    .Where(pi => pi.Inserted && pi.Type.Category == "I" && pi.ItemType.Category == "I" && pi.ID_Company == idCompany)
                                    .Select(pt => new { pt.TypeCode, pt.Type.Order })
                                    .Distinct()
                                    .OrderBy(pt => pt.Order)
                                    .Select(pt => pt.TypeCode)
                                    ;

                                for (var i = 0; i < _distinctTypes.ToList().Count; i++)
                                {
                                    var _type = _qc.Types.Where(t => t.Code == _distinctTypes.ToList()[i]).SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1), true, false, _type.Description,
                                        _type.Code.ToString(), null));
                                    IEnumerable<int> _distinctItemTypes = _qc.PickingItems
                                        .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                        .Where(pi => pi.Inserted && pi.Type.Code == _type.Code && pi.ID_Company == idCompany)
                                        .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                        .Distinct()
                                        .OrderBy(pt => pt.Order)
                                        .Select(pt => pt.ItemTypeCode)
                                        ;
                                    for (var j = 0; j < _distinctItemTypes.ToList().Count; j++)
                                    {
                                        var _itemType =
                                            _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                                .SingleOrDefault();
                                        _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1), true,
                                            false, _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                        var _distinctPickingItems = _qc.PickingItems.
                                            OrderBy(pi => pi.Order)
                                            .ThenByDescending(pi => pi.Cost)
                                            .ThenBy(pi => pi.ItemDescription)
                                            .
                                            Join(_qc.ItemTypes, pi => pi.ItemTypeCode, o => o.Code, (o, e2) => o)
                                            .Where(
                                                pi =>
                                                    pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                    pi.Inserted && pi.ID_Company == idCompany)
                                            .Select(pi => pi.ID)
                                            .Distinct()
                                            .ToList();
                                        for (var k = 0; k < _distinctPickingItems.Count; k++)
                                        {
                                            var _pickingItem =
                                                _qc.PickingItems.Where(pi => pi.ID == _distinctPickingItems[k] && pi.ID_Company == idCompany)
                                                    .SingleOrDefault();
                                            PickingItem _dependsOn = null;
                                            try
                                            {
                                                _dependsOn =
                                                    _qc.PickingItems.Where(pi => Convert.ToInt32(pi.Link) == _pickingItem.ID && pi.ID_Company == idCompany)
                                                        .SingleOrDefault();
                                            }
                                            catch
                                            {
                                            }
                                            if (_dependsOn == null)
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true, _pickingItem.ItemDescription,
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                        }
                                    }
                                }
                            }
                            Cache.Insert(idCompany.ToString() + "|" + menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[idCompany.ToString() + "|" + menuType.ToString()];
                        }
                        ;
                        break;
                    }

                case MenuType.MenuOperations:
                    {
                        if (Cache[idCompany.ToString() + "|" + menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _distinctTypes = _qc.VW_MenuItems
                                    .Join(_qc.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                                    .Where(pi => pi.Inserted && pi.Type.Category == "I" && pi.ItemType.Category == "I" && pi.ID_Company == idCompany)
                                    .Select(pt => new { pt.TypeCode, pt.Type.Order })
                                    .Distinct()
                                    .OrderBy(pt => pt.Order)
                                    .Select(pt => pt.TypeCode).ToList();
                                ;
                                for (var i = 0; i < _distinctTypes.Count; i++)
                                {
                                    var _type = _qc.Types.Where(t => t.Code == _distinctTypes.ToList()[i]).SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1), true, false, _type.Description,
                                        _type.Code.ToString(), null));
                                    var _distinctItemTypes = _qc.VW_MenuItems
                                        .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                        .Where(pi => pi.Inserted && pi.TypeCode == _type.Code && pi.ID_Company == idCompany)
                                        .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                        .Distinct()
                                        .OrderBy(pt => pt.Order)
                                        .Select(pt => pt.ItemTypeCode).ToList();
                                    ;
                                    for (var j = 0; j < _distinctItemTypes.Count; j++)
                                    {
                                        var _itemType =
                                            _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                                .SingleOrDefault();
                                        _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1), true,
                                            false, _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                        var _unCastedDistinctPickingItems =
                                            _qc.VW_MenuItems.Where(
                                                pi =>
                                                    pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                    pi.Inserted && pi.ID_Company == idCompany)
                                                .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                                .Distinct()
                                                .ToList();
                                        var _distinctPickingItems = _unCastedDistinctPickingItems.
                                            OrderBy(o => o.Order)
                                            .ThenByDescending(o => o.Cost)
                                            .ThenBy(o => o.ItemDescription)
                                            .
                                            Select(o => o.ID).ToList();
                                        for (var k = 0; k < _distinctPickingItems.Count; k++)
                                        {
                                            var _pickingItem =
                                                _qc.VW_MenuItems.Where(pi => pi.ID == _distinctPickingItems[k] && pi.ID_Company == idCompany)
                                                    .SingleOrDefault();
                                            if (_distinctPickingItems[k].Substring(0, 1) == "P")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true, _pickingItem.ItemDescription,
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                            if (_distinctPickingItems[k].Substring(0, 1) == "M")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true,
                                                        "[" + _pickingItem.ItemDescription + "]",
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                        }
                                    }
                                }
                            }
                            Cache.Insert(idCompany.ToString() + "|" + menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[idCompany.ToString() + "|" + menuType.ToString()];
                        }
                        ;
                        break;
                    }

                case MenuType.MenuMaterials:
                    {
                        if (Cache[idCompany.ToString() + "|" + menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _type = _qc.Types.SingleOrDefault(t => t.Code == 6);
                                _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(0 + 1), true, false, "+", _type.Code.ToString(),
                                    null));
                                var _distinctItemTypes = _qc.VW_MenuItems
                                    .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                    .Where(pi => pi.Inserted && pi.Type == _type && pi.Type.Category == "I" && pi.ItemType.Category == "I" && pi.ID_Company == idCompany)
                                    .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                    .Distinct()
                                    .OrderBy(pt => pt.Order)
                                    .Select(pt => pt.ItemTypeCode).ToList();
                                ;
                                for (var j = 0; j < _distinctItemTypes.Count; j++)
                                {
                                    var _itemType =
                                        _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                            .SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(0 + 1) + "." + IntTo3cStr(j + 1), true, false,
                                        _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                    var _unCastedDistinctPickingItems =
                                        _qc.VW_MenuItems.Where(
                                            pi =>
                                                pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                pi.Inserted && pi.ID_Company == idCompany)
                                            .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                            .Distinct()
                                            .ToList();
                                    var _distinctPickingItems = _unCastedDistinctPickingItems.
                                        OrderBy(o => o.Order).ThenByDescending(o => o.Cost).ThenBy(o => o.ItemDescription).
                                        Select(o => o.ID).ToList();
                                    for (var k = 0; k < _distinctPickingItems.Count; k++)
                                    {
                                        var _pickingItem =
                                            _qc.VW_MenuItems.Where(pi => pi.ID == _distinctPickingItems[k] && pi.ID_Company == idCompany)
                                                .SingleOrDefault();
                                        if (_distinctPickingItems[k].Substring(0, 1) == "P")
                                        {
                                            _BNMenuItems.Add(
                                                new BnMenuItem(
                                                    IntTo3cStr(0 + 1) + "." + IntTo3cStr(j + 1) + "." + IntTo3cStr(k + 1),
                                                    true, true, _pickingItem.ItemDescription,
                                                    _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                        }
                                        if (_distinctPickingItems[k].Substring(0, 1) == "M")
                                        {
                                            _BNMenuItems.Add(
                                                new BnMenuItem(
                                                    IntTo3cStr(0 + 1) + "." + IntTo3cStr(j + 1) + "." + IntTo3cStr(k + 1),
                                                    true, true, "[" + _pickingItem.ItemDescription + "]",
                                                    _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                        }
                                    }
                                }
                            }
                            Cache.Insert(idCompany.ToString() + "|" + menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[idCompany.ToString() + "|" + menuType.ToString()];
                        }
                        ;
                        break;
                    }


                case MenuType.MenuProdRecord:
                    {
                        if (Cache[idCompany.ToString() + "|" + menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _distinctTypes =
                                    _qc.VW_MenuItems.Where(m => m.TypeCode == 6 || m.ID.Substring(0, 1) == "M")
                                        .Join(_qc.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                                        .Where(pi => pi.Inserted && pi.Type.Category == "I" && pi.ItemType.Category == "I" && pi.ID_Company == idCompany)
                                        .Select(pt => new { pt.TypeCode, pt.Type.Order })
                                        .Distinct()
                                        .OrderBy(pt => pt.Order)
                                        .Select(pt => pt.TypeCode).ToList();
                                ;
                                for (var i = 0; i < _distinctTypes.Count; i++)
                                {
                                    var _type = _qc.Types.Where(t => t.Code == _distinctTypes.ToList()[i]).SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1), true, false, _type.Description,
                                        _type.Code.ToString(), null));
                                    var _distinctItemTypes =
                                        _qc.VW_MenuItems.Where(m => m.TypeCode == 6 || m.ID.Substring(0, 1) == "M")
                                            .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                            .Where(pi => pi.Inserted && pi.Type == _type && pi.ID_Company == idCompany)
                                            .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                            .Distinct()
                                            .OrderBy(pt => pt.Order)
                                            .Select(pt => pt.ItemTypeCode).ToList();
                                    ;
                                    for (var j = 0; j < _distinctItemTypes.Count; j++)
                                    {
                                        var _itemType =
                                            _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                                .SingleOrDefault();
                                        _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1), true,
                                            false, _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                        var _unCastedDistinctPickingItems =
                                            _qc.VW_MenuItems.Where(
                                                pi =>
                                                    pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                    pi.Inserted && pi.ID_Company == idCompany)
                                                .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                                .Distinct()
                                                .ToList();
                                        var _distinctPickingItems = _unCastedDistinctPickingItems.
                                            OrderBy(o => o.Order)
                                            .ThenByDescending(o => o.Cost)
                                            .ThenBy(o => o.ItemDescription)
                                            .
                                            Select(o => o.ID).ToList();
                                        for (var k = 0; k < _distinctPickingItems.Count; k++)
                                        {
                                            var _pickingItem =
                                                _qc.VW_MenuItems.Where(pi => pi.ID == _distinctPickingItems[k] && pi.ID_Company == idCompany)
                                                    .SingleOrDefault();
                                            if (_distinctPickingItems[k].Substring(0, 1) == "P")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true, _pickingItem.ItemDescription,
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                            if (_distinctPickingItems[k].Substring(0, 1) == "M")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true,
                                                        "[" + _pickingItem.ItemDescription + "]",
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                        }
                                    }
                                }
                            }
                            Cache.Insert(idCompany.ToString() + "|" + menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[idCompany.ToString() + "|" + menuType.ToString()];
                        }
                        ;
                        break;
                    }


                case MenuType.MenuOperationNoPhases:
                    {
                        if (Cache[idCompany.ToString() + "|" + menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _distinctTypes =
                                    _qc.VW_MenuItems.Where(m => m.TypeCode != 31 || m.ID.Substring(0, 1) == "M")
                                        .Join(_qc.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                                        .Where(pi => pi.Inserted && pi.Type.Category == "I" && pi.ItemType.Category == "I" && pi.ID_Company == idCompany)
                                        .Select(pt => new { pt.TypeCode, pt.Type.Order })
                                        .Distinct()
                                        .OrderBy(pt => pt.Order)
                                        .Select(pt => pt.TypeCode).ToList();
                                ;
                                for (var i = 0; i < _distinctTypes.Count; i++)
                                {
                                    var _type = _qc.Types.Where(t => t.Code == _distinctTypes.ToList()[i]).SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1), true, false, _type.Description,
                                        _type.Code.ToString(), null));
                                    var _distinctItemTypes =
                                        _qc.VW_MenuItems.Where(m => m.TypeCode != 31 || m.ID.Substring(0, 1) == "M")
                                            .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                            .Where(pi => pi.Inserted && pi.TypeCode == _type.Code && pi.ID_Company == idCompany)
                                            .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                            .Distinct()
                                            .OrderBy(pt => pt.Order)
                                            .Select(pt => pt.ItemTypeCode).ToList();
                                    ;
                                    for (var j = 0; j < _distinctItemTypes.Count; j++)
                                    {
                                        var _itemType =
                                            _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                                .SingleOrDefault();
                                        _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1), true,
                                            false, _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                        var _unCastedDistinctPickingItems =
                                            _qc.VW_MenuItems.Where(
                                                pi =>
                                                    pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                    pi.Inserted && pi.ID_Company == idCompany)
                                                .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                                .Distinct()
                                                .ToList();
                                        var _distinctPickingItems = _unCastedDistinctPickingItems.
                                            OrderBy(o => o.Order)
                                            .ThenByDescending(o => o.Cost)
                                            .ThenBy(o => o.ItemDescription)
                                            .
                                            Select(o => o.ID).ToList();
                                        for (var k = 0; k < _distinctPickingItems.Count; k++)
                                        {
                                            var _pickingItem =
                                                _qc.VW_MenuItems.Where(pi => pi.ID == _distinctPickingItems[k] && pi.ID_Company == idCompany)
                                                    .SingleOrDefault();
                                            if (_distinctPickingItems[k].Substring(0, 1) == "P")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true, _pickingItem.ItemDescription,
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                            if (_distinctPickingItems[k].Substring(0, 1) == "M")
                                            {
                                                _BNMenuItems.Add(
                                                    new BnMenuItem(
                                                        IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                        IntTo3cStr(k + 1), true, true,
                                                        "[" + _pickingItem.ItemDescription + "]",
                                                        _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));
                                            }
                                        }
                                    }
                                }
                            }
                            Cache.Insert(idCompany.ToString() + "|" + menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[idCompany.ToString() + "|" + menuType.ToString()];
                        }
                        ;
                        break;
                    }

                case MenuType.MenuQuotationTemplates:
                    {
                        if (Cache[menuType.ToString()] == null)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _distinctTypes = _qc.VW_MenuQuotationTemplates
                                    .Join(_qc.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                                    .Where(pi => pi.Inserted && pi.Type.Category == "T" && pi.ItemType.Category == "T")
                                    .Select(pt => new { pt.TypeCode, pt.Type.Order })
                                    .Distinct()
                                    .OrderBy(pt => pt.Order)
                                    .Select(pt => pt.TypeCode).ToList();
                                ;
                                for (var i = 0; i < _distinctTypes.Count; i++)
                                {
                                    var _type = _qc.Types.Where(t => t.Code == _distinctTypes.ToList()[i]).SingleOrDefault();
                                    _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1), true, false, _type.Description,
                                        _type.Code.ToString(), null));
                                    var _distinctItemTypes = _qc.VW_MenuQuotationTemplates
                                        .Join(_qc.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                                        .Where(pi => pi.Inserted && pi.TypeCode == _type.Code)
                                        .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                                        .Distinct()
                                        .OrderBy(pt => pt.Order)
                                        .Select(pt => pt.ItemTypeCode).ToList();
                                    ;
                                    for (var j = 0; j < _distinctItemTypes.Count; j++)
                                    {
                                        var _itemType =
                                            _qc.ItemTypes.Where(it => it.Code == _distinctItemTypes.ToList()[j])
                                                .SingleOrDefault();
                                        _BNMenuItems.Add(new BnMenuItem(IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1), true,
                                            false, _itemType.Description, _type.Code + "." + _itemType.Code, null));
                                        var _unCastedDistinctPickingItems =
                                            _qc.VW_MenuQuotationTemplates.Where(
                                                pi =>
                                                    pi.TypeCode == _type.Code && pi.ItemTypeCode == _itemType.Code &&
                                                    pi.Inserted)
                                                .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                                .Distinct()
                                                .ToList();
                                        var _distinctPickingItems = _unCastedDistinctPickingItems.
                                            OrderBy(o => o.Order)
                                            .ThenByDescending(o => o.Cost)
                                            .ThenBy(o => o.ItemDescription)
                                            .
                                            Select(o => o.ID).ToList();
                                        for (var k = 0; k < _distinctPickingItems.Count; k++)
                                        {
                                            var _pickingItem =
                                                _qc.VW_MenuQuotationTemplates.Where(pi => pi.ID == _distinctPickingItems[k])
                                                    .SingleOrDefault();
                                            _BNMenuItems.Add(
                                                new BnMenuItem(
                                                    IntTo3cStr(i + 1) + "." + IntTo3cStr(j + 1) + "." +
                                                    IntTo3cStr(k + 1), true, true,
                                                    "{" + _pickingItem.ItemDescription + "}",
                                                    _type.Code + "." + _itemType.Code + "." + _pickingItem.ID, null));

                                        }
                                    }
                                }
                            }
                            Cache.Insert(menuType.ToString(), _BNMenuItems, null, DateTime.MaxValue,
                                Cache.NoSlidingExpiration);
                        }
                        else
                        {
                            _BNMenuItems = (List<BnMenuItem>)Cache[menuType.ToString()];
                        }
                        ;
                        break;
                    }


                default:
                    {
                        break;
                    }
            }

            //_BNMenuItems = MenuManager.CustomizeBNMenuItems(UserID, _BNMenuItems);

            switch (menuType)
            {
                case MenuType.MenuPickingItems:
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                case MenuType.MenuOperations:
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                case MenuType.MenuMaterials: // non usato
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                case MenuType.MenuProdRecord: // non usato
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                case MenuType.MenuOperationNoPhases:
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                case MenuType.MenuQuotationTemplates:
                    {
                        MenuManager.LoadMenu(customMenu, _BNMenuItems);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            //CreateMenuLinks(customMenu, menuType);
        }


        public string IntTo3cStr(int x)
        {
            return x.ToString("000");
        }

        public virtual void CreateMenuLinks(Menu customMenu, MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.MenuOperations:
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        protected void ClearAllMenusCache(string idCompany)
        {
            //if (idCompany == "-1")
            //{
                //Cache.Remove("1|" + MenuType.MenuPickingItems.ToString());
                //Cache.Remove("1|" + MenuType.MenuOperations.ToString());
                //Cache.Remove("1|" + MenuType.MenuMaterials.ToString());
                //Cache.Remove("1|" + MenuType.MenuProdRecord.ToString());
                //Cache.Remove("2|" + MenuType.MenuPickingItems.ToString());
                //Cache.Remove("2|" + MenuType.MenuOperations.ToString());
                //Cache.Remove("2|" + MenuType.MenuMaterials.ToString());
                //Cache.Remove("2|" + MenuType.MenuProdRecord.ToString());
            //}
            //else
            //{
            //    Cache.Remove(idCompany + "|" + MenuType.MenuPickingItems.ToString());
            //    Cache.Remove(idCompany + "|" + MenuType.MenuOperations.ToString());
            //    Cache.Remove(idCompany + "|" + MenuType.MenuMaterials.ToString());
            //    Cache.Remove(idCompany + "|" + MenuType.MenuProdRecord.ToString());
            //}

            Cache.Remove("PickingItems");
            Cache.Remove("MacroItems");

            using (QuotationDataContext db = new QuotationDataContext())
            {

                foreach (Company cp in db.Companies)
                {
                    Cache.Remove(cp.ID + "|" + MenuType.MenuPickingItems.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperations.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperationNoPhases.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuMaterials.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuProdRecord.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuQuotationTemplates.ToString());
                }
            }

        }

        public void ToggleSuccessMessage(bool result, Label logLabel, LabExtimErrorType errorType)
        {
            logLabel.Text = String.Empty;
            switch (errorType)
            {
                case LabExtimErrorType.ImportFailed:
                    logLabel.Text += result
                        ? "Operazione eseguita con successo alle " + DateTime.Now.ToLongTimeString() + " di " +
                          DateTime.Now.ToLongDateString() + "<br/>"
                        : "Operazione fallita alle " + DateTime.Now.ToLongTimeString() + " di " +
                          DateTime.Now.ToLongDateString() +
                          " (riprovare o consultare il log errori dell'applicazione).<br/>";
                    break;
                case LabExtimErrorType.InputFailed:
                    logLabel.Text += result
                        ? ""
                        : "Input non valido (verificare congruità e completezza dati immessi).<br/>";
                    break;

                case LabExtimErrorType.ObsoleteItem:
                    logLabel.Text += result
                        ? ""
                        : "Una o più voci inserite proveniente da un modello od una macrovoce esistente risulta disattivata (obsoleta). Si consiglia di verificare le voci indicate in rosso e di sostituirle con altre aggiornate.<br/>";
                    break;

                case LabExtimErrorType.ModelLoadingFailed:
                    logLabel.Text += result
                        ? ""
                        : "Impossibile caricare il modello o la macrovoce perchè una o più voci risultano inesistenti sulla tabella base o tra le macrovoci. Si consiglia di ricostruire il modello od il gruppo che si è tentato di caricare.<br/>";
                    break;

                case LabExtimErrorType.ModelObsoleteItem:
                    logLabel.Text += result
                        ? ""
                        : "Una o più voci inserite proveniente dalla tabella base o dalle macrovoci risulta disattivata (obsoleta). Si consiglia di verificare le voci indicate in rosso e di sostituirle con altre aggiornate.<br/>";
                    break;

                case LabExtimErrorType.ItemLoadingFailed:
                    logLabel.Text += result
                        ? ""
                        : "Una o più voci inserite proveniente dalla tabella base o dalle macrovoci risulta inesistente. Eliminarle le voci indicate in blu e sostituirle con altre aggiornate.<br/>";
                    break;

                case LabExtimErrorType.CannotDelete:
                    logLabel.Text += result
                        ? ""
                        : "Impossibile eliminare questa voce perchè è contenuta in uno o più preventivi.<br/>";
                    break;

                case LabExtimErrorType.CustomerIsMandatory:
                    logLabel.Text += result ? "" : "Il Cliente è obbligatorio.<br/>";
                    break;

                case LabExtimErrorType.QuantityIsMandatory:
                    logLabel.Text += result ? "" : "La quantità è obbligatoria.<br/>";
                    break;

                case LabExtimErrorType.PriceIsMandatoryForAuto:
                    logLabel.Text += result
                        ? ""
                        : "Il prezzo è obbligatorio se il preventivo deve essere generato automaticamente.<br/>";
                    break;

                case LabExtimErrorType.DeliveryDateIsMandatory:
                    logLabel.Text += result ? "" : "La data di consegna è obbligatoria.<br/>";
                    break;

                case LabExtimErrorType.DeliveryDateIsInvalid:
                    logLabel.Text += result ? "" : "La data di consegna non è valida.<br/>";
                    break;

                default:
                    logLabel.Text = String.Empty;
                    break;
            }
        }

        protected bool TypeAndItemTypeAreEqual(IOrderedDictionary newValues, IOrderedDictionary oldValues)
        {
            return Convert.ToInt32(newValues["TypeCode"]) == Convert.ToInt32(oldValues["TypeCode"])
                   && Convert.ToInt32(newValues["ItemTypeCode"]) == Convert.ToInt32(oldValues["ItemTypeCode"]);
        }

        //protected Employee GetCurrentEmployee()
        //{
        //    using (var QuotationDataContext = new QuotationDataContext())
        //    {
        //        Employee _currentUser;
        //        _currentUser =
        //            QuotationDataContext.Employees.FirstOrDefault(o => o.UserGUID == (Guid?)WebUser.ProviderUserKey);
        //        if (_currentUser == null)
        //            _currentUser =
        //                QuotationDataContext.Employees.FirstOrDefault(
        //                    o => o.Surname.ToLower() == WebUser.UserName.ToLower());
        //        return _currentUser;
        //    }
        //}

        protected Employee GetCurrentEmployee()
        {
            return WebUser.Employee;
        }

        public Dictionary<string, string> GetConfiguration()
        {
            try
            {
                using (var _db = new GeneralDataContext())
                {
                    return _db.Configuration.ToDictionary(d => d.ConfigKey, e => e.ConfigValue);
                }
            }
            catch (Exception _ex)
            {
                Log.Write("Errore nel salvataggio della configurazione - ", _ex);
                throw _ex;
            }
        }


        public string GetPickingItemDescription(string id)
        {
            if (Cache["PickingItems"] == null)
            {
                using (QuotationDataContext ctx = new QuotationDataContext())
                {
                    DataLoadOptions options = new DataLoadOptions();
                    options.LoadWith<PickingItem>(ii => ii.Type);
                    options.LoadWith<PickingItem>(ii => ii.ItemType);
                    ctx.LoadOptions = options;
                    Cache.Insert("PickingItems", ctx.PickingItems.ToArray(), null, DateTime.MaxValue, Cache.NoSlidingExpiration);
                }
            }

            string desc = "";
            if (!string.IsNullOrEmpty(id))
            {
                int nid = Convert.ToInt32(id);
                PickingItem[] data = (PickingItem[])Cache["PickingItems"];
                desc += (data.FirstOrDefault(pi => pi.ID == nid).Type.Description.TrimEnd() + " ");
                desc += (data.FirstOrDefault(pi => pi.ID == nid).ItemType.Description.TrimEnd() + " ");
                desc += (data.FirstOrDefault(pi => pi.ID == nid).ItemDescription.TrimEnd());
            }
            return desc;
        }
        public string GetMacroItemDescription(string id)
        {
            if (Cache["MacroItems"] == null)
            {
                using (QuotationDataContext ctx = new QuotationDataContext())
                {
                    DataLoadOptions options = new DataLoadOptions();
                    options.LoadWith<MacroItem>(ii => ii.Type);
                    options.LoadWith<MacroItem>(ii => ii.ItemType);
                    ctx.LoadOptions = options;
                    Cache.Insert("MacroItems", ctx.MacroItems.ToArray(), null, DateTime.MaxValue, Cache.NoSlidingExpiration);
                }
            }

            string desc = "";
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    int nid = Convert.ToInt32(id);
                    MacroItem[] data = (MacroItem[])Cache["MacroItems"];
                    if (data.FirstOrDefault(pi => pi.ID == nid) != null)
                    {
                        desc += (data.FirstOrDefault(pi => pi.ID == nid).Type.Description.TrimEnd() + " ");
                        desc += (data.FirstOrDefault(pi => pi.ID == nid).ItemType.Description.TrimEnd() + " ");
                        desc += (data.FirstOrDefault(pi => pi.ID == nid).MacroItemDescription.TrimEnd());
                    }
                }
                catch
                { }
            }
            return desc;
        }


        public void ExportToExcel(string gridRender)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid() + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/octet-stream"; // Excel 2010
            Response.Write(gridRender);
            Response.End();
        }


        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_BATCH = 4;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);


        public bool ImpersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        public void UndoImpersonation()
        {
            impersonationContext.Undo();
        }

        public bool IsTablet(string ip)
        {
            System.Net.IPAddress incomingIp = System.Net.IPAddress.Parse(ip);

            foreach (var subnet in ConfigurationManager.AppSettings["Tablets_IPAddresses"].ToString().Split('|'))
            {
                System.Net.IPNetwork network = System.Net.IPNetwork.Parse(subnet);
                if (network.Contains(incomingIp))
                    return true;
            }

            return false;
        }


        public void BuildGridTotals(ref decimal[] totals, int[] totalizedFields, int[] percTern, System.Web.UI.WebControls.GridViewRowEventArgs e, int[] countOnlyFields = null)
        {
            if (countOnlyFields == null)
                countOnlyFields = new int[] { -1 };

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                    totals = new decimal[e.Row.Cells.Count + 1];

                for (var i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    string sNum = string.Empty;
                    if (e.Row.Cells[i].Controls.Count > 1)
                    {
                        if (countOnlyFields.Contains(i))
                            sNum = "1";
                        else if (e.Row.Cells[i].Controls[1].GetType() == typeof(LinkButton))
                            sNum = "1";
                        else if (e.Row.Cells[i].Controls[1].GetType() == typeof(Label))
                            sNum = ((Label)e.Row.Cells[i].Controls[1]).Text;
                    }
                    else if (countOnlyFields.Contains(i))
                        sNum = "1";
                    else
                        sNum = e.Row.Cells[i].Text;
                    if (Utilities.IsNumeric(sNum))
                    {
                        double sum;
                        if (double.TryParse(sNum, out sum))
                            totals[i] += Convert.ToDecimal(sum);
                    }
                    else if (sNum.Contains("&nbsp;"))
                        totals[i] += 0;
                    else
                        totals[i] += 1;
                }
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (var i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    if ((countOnlyFields.Contains(i) | totalizedFields.Contains(i)))
                    {
                        int curColumnIndex = percTern[0];
                        if (i == curColumnIndex)
                        {
                            e.Row.Cells[i].Text = (totals[percTern[2]] != 0 ? (totals[percTern[1]] / Convert.ToDecimal(totals[percTern[2]])) : 0).ToString("P0");
                            break;
                        }
                        else
                        {
                            e.Row.Cells[i].Text = totals[i].ToString();
                            break;
                        }
                    }
                }
            }
        }


    }
}