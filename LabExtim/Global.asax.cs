using System;
using System.Collections.Generic;
using System.Web;
using System.Web.DynamicData;
using System.Web.Routing;
using DLLabExtim;
using System.Globalization;
using NotAClue.Web.DynamicData;

namespace LabExtim
{
    public class Global : HttpApplication
    {
        public static Dictionary<int, string> ReportTypes;
        public static Dictionary<int, string> ReportTextTypes;
        public static ProductionOrderService.SchedulingType CurrentSchedulingType;

        public static void RegisterRoutes(RouteCollection routes)
        {
            var model = new AdvancedMetaModel();

            //                    IMPORTANT: DATA MODEL REGISTRATION 
            // Uncomment this line to register LINQ to SQL classes or an ADO.NET Entity Data
            // model for ASP.NET Dynamic Data. Set ScaffoldAllTables = true only if you are sure 
            // that you want all tables in the data model to support a scaffold (i.e. templates) 
            // view. To control scaffolding for individual tables, create a partial class for 
            // the table and apply the [Scaffold(true)] attribute to the partial class.
            // Note: Make sure that you change "YourDataContextType" to the name of the data context
            // class in your application.

            model.RegisterContext(typeof (QuotationDataContext), new ContextConfiguration {ScaffoldAllTables = false});
            model.RegisterContext(typeof (GeneralDataContext), new ContextConfiguration {ScaffoldAllTables = false});

            // The following statement supports separate-page mode, where the List, Detail, Insert, and 
            // Update tasks are performed by using separate pages. To enable this mode, uncomment the following 
            // route definition, and comment out the route definitions in the combined-page mode section that follows.
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new {action = "List|Details|Edit|Insert"}),
                Model = model
            });
            //routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            //{
            //    Constraints = new RouteValueDictionary(new { action = "QuotationInsert|QuotationConsole" }),
            //    Model = model
            //});
            //routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            //{
            //    Constraints = new RouteValueDictionary(new { action = "QuotationFind|QuotationConsole" }),
            //    Model = model
            //});

            // The following statements support combined-page mode, where the List, Detail, Insert, and
            // Update tasks are performed by using the same page. To enable this mode, uncomment the
            // following routes and comment out the route definition in the separate-page mode section above.
            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx")
            //{
            //    Action = PageAction.List,
            //    ViewName = "ListDetails",
            //    Model = model
            //});

            //routes.Add(new DynamicDataRoute("{table}/ListDetails.aspx")
            //{
            //    Action = PageAction.Details,
            //    ViewName = "ListDetails",
            //    Model = model
            //});
        }

        private void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            LoadGlobalVariables();

            CultureInfo _culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            CultureInfo _uiculture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

            _culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            _uiculture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

            System.Threading.Thread.CurrentThread.CurrentCulture = _culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = _uiculture;
        }

        private void Session_End(object sender, EventArgs e)
        {
            //string _result = new QuotationService().DelLocks(Membership.GetUser().ProviderUserKey.ToString(), Session["SessionTimeStamp"].ToString());
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }

        private void Session_Start(object sender, EventArgs e)
        {
            Session["CurrentReportTexts"] = new List<ReportText>();
        }

        private void LoadGlobalVariables()
        {
            ReportTypes = new Dictionary<int, string>();
            ReportTypes.Add(1, "Preventivo Cliente");

            ReportTextTypes = new Dictionary<int, string>();
            ReportTextTypes.Add(1, "Destinatario");
            ReportTextTypes.Add(2, "Oggetto");
            ReportTextTypes.Add(3, "Porto");
            ReportTextTypes.Add(4, "Condizioni pagamento");
            ReportTextTypes.Add(5, "Testo apertura");
            ReportTextTypes.Add(6, "Testo chiusura");


            CurrentSchedulingType = ProductionOrderService.SchedulingType.FiniteForwardCapacity;

        }
    }
}
