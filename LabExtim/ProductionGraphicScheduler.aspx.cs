using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections;


namespace LabExtim
{
    public partial class ProductionGraphicScheduler : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Array GetEvents(string idDepartment, string idMachine, string customerCode, string productionOrderId)
        {
            try
            {



                int CurrentCompanyId = (((LabextimUser)HttpContext.Current.Session["LabextimUser"]).Employee.ID_Company ?? -1);

                using (var ctx = new QuotationDataContext())
                {
                    var data = ctx.VW_ProductionMPSGroupedByMachines.AsQueryable();

                    // merge aziendale
                    //data = data.Where(po => (CurrentCompanyId == -1 || po.ID_Company == CurrentCompanyId));

                    if (!string.IsNullOrEmpty(idDepartment))
                    {
                        data = data.Where(d => d.IDDepartment == Convert.ToInt32(idDepartment));
                    }
                    if (!string.IsNullOrEmpty(idMachine))
                    {
                        data = data.Where(d => d.IDProductionMachine == Convert.ToInt32(idMachine));
                    }
                    if (!string.IsNullOrEmpty(customerCode))
                    {
                        data = data.Where(d => d.CustomerCode == Convert.ToInt32(customerCode));
                    }
                    if (!string.IsNullOrEmpty(productionOrderId))
                    {
                        data = data.Where(d => d.IDProductionOrder == Convert.ToInt32(productionOrderId));
                    }
                    return data.OrderBy(m => m.ProdStart).ToArray().Select(c => new
                    {
                        id = c.IDProductionOrder,
                        start = c.ProdStart.Value.ToString("yyyy-MM-ddTHH:mm:ss"),
                        end = c.ProdEnd.Value.ToString("yyyy-MM-ddTHH:mm:ss"),
                        resource = c.IDProductionMachine.ToString(),
                        status = c.Status,
                        text = String.Format("({0}) {1} {2}", c.PMDescription, c.IDProductionOrder.ToString(), c.PODescription), //  > 40 ? c.PODescription.Substring(0, 37) + "..." : c.PODescription),
                        tooltip = String.Format("({0}) Odp:{1} {2}", c.PMDescription, c.IDProductionOrder.ToString(), c.PODescription),
                        barColor = c.BarColor
                    }).ToArray();

                }
            }
            catch (Exception ex)
            {
                //return String.Format("Errore: {0}", ex.Message);
                return null;
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Array GetDepartments()
        {
            try
            {
                int currentCompanyId = (((LabextimUser)HttpContext.Current.Session["LabextimUser"]).Employee.ID_Company ?? -1);

                using (var ctx = new QuotationDataContext())
                {

                    //return ctx.Departments.Where(d => (currentCompanyId == -1 || d.ID_Company == currentCompanyId)).OrderBy(c => c.Order).Select(c => new { name = c.Description, id = c.ID.ToString() }).ToArray();
                    // merge aziendale
                     return ctx.Departments.OrderBy(c => c.Order).Select(c => new { name = c.Description, id = c.ID.ToString() }).ToArray();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static Array GetProductionMachines()
        {
            try
            {
                int currentCompanyId = (((LabextimUser)HttpContext.Current.Session["LabextimUser"]).Employee.ID_Company ?? -1);

                using (var ctx = new QuotationDataContext())
                {
                    //return ctx.ProductionMachines.Where(d => (currentCompanyId == -1 || d.ID_Company == currentCompanyId)).OrderBy(c => c.Description).Select(c => new { name = c.Description, id = c.ID.ToString() }).ToArray();
                    // merge aziendale
                    return ctx.ProductionMachines.Where(c => c.Inserted == true).OrderBy(c => c.Description).Select(c => new { name = c.Description, id = c.ID.ToString() }).ToArray();
                }
            }
            catch (Exception ex)
            {
                //return String.Format("Errore: {0}", ex.Message);
                return null;
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCustomers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'"))).OrderBy(c => c.Name).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetProductionOrders(string q, string customerCode)
        {
            try
            {

                int currentCompanyId = (((LabextimUser)HttpContext.Current.Session["LabextimUser"]).Employee.ID_Company ?? -1);

                using (var ctx = new QuotationDataContext())
                {
                    //var data = ctx.ProductionOrders.Where(c => (currentCompanyId == -1 || c.ID_Company == currentCompanyId) && c.Description.Contains(q.Replace("%27", "'")) || c.ID.ToString().Contains(q.Replace("%27", "'")));
                    // merge aziendale
                    var data = ctx.ProductionOrders.Where(c => c.Description.Contains(q.Replace("%27", "'")) || c.ID.ToString().Contains(q.Replace("%27", "'")));
                    if (!string.IsNullOrEmpty(customerCode))
                    {
                        data = data.Where(d => d.ID_Customer == Convert.ToInt32(customerCode));
                    }
                    return new JavaScriptSerializer().Serialize(data.OrderBy(c => c.Description).Select(c => new { Code = c.ID, Name = c.ID.ToString() + " " + c.Description }).ToList());
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string MovePhase(string pipedParams)
        {
            //try
            //{
                string[] parameters = pipedParams.Split('|');
                int poId = Convert.ToInt32(parameters[0]);
                int machineId = Convert.ToInt32(parameters[1]);
                DateTime oldStart = DateTime.ParseExact(parameters[2], "yyyy-MM-ddTHH:mm:ss", null);
                DateTime oldEnd = DateTime.ParseExact(parameters[3], "yyyy-MM-ddTHH:mm:ss", null);
                DateTime newStart = DateTime.ParseExact(parameters[4], "yyyy-MM-ddTHH:mm:ss", null);
                DateTime newEnd = DateTime.ParseExact(parameters[5], "yyyy-MM-ddTHH:mm:ss", null);
                using (var db = new QuotationDataContext())
                {
                    ProductionOrderService.MovePhase(db, poId, machineId, oldStart, oldEnd, newStart, newEnd);
                    return null;
                }
            //}
            //catch (Exception ex)
            //{
            //    //return String.Format("Errore: {0}", ex.Message);
            //    return null;
            //}

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CancelPriority(string pipedParams)
        {
            //try
            //{
            string[] parameters = pipedParams.Split('|');
            int poId = Convert.ToInt32(parameters[0]);
            using (var db = new QuotationDataContext())
            {
                ProductionOrderService.CancelPriority(db, poId);
                return null;
            }
            //}
            //catch (Exception ex)
            //{
            //    //return String.Format("Errore: {0}", ex.Message);
            //    return null;
            //}

        }

    }

}