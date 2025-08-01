using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web;
using System.Web.Security;
using System.Configuration;
using CMLabExtim;
using CMLabExtim.CustomClasses;
using DevExpress.Web;
using DLLabExtim;
using LabExtimOperator.Models;
using UILabExtim;
using WebMatrix.WebData;
using DevExpress.Web.Mvc;
using DevExpress.XtraSpreadsheet;

namespace LabExtimOperator.Controllers
{

    public class EmployeeWorkingDayHoursControllerSessionVariables
    {
        private System.Web.SessionState.HttpSessionState _session = HttpContext.Current.Session;


        public int? _idUser
        {
            get
            {
                if (_session["_idUser"] == null || _session["_idUser"].ToString() == "0")
                {
                    _session["_idUser"] = new LabextimUser(Membership.GetUser()).Employee.ID;
                }
                return (int?)_session["_idUser"];
            }
            set { _session["_idUser"] = value; }
        }

    }

    public class EmployeeWorkingDayHoursController : Controller
    {


        [Authorize]
        public ActionResult Index()
        {
            EmployeeWorkingDayHoursControllerSessionVariables variables = new EmployeeWorkingDayHoursControllerSessionVariables();
            List<object[]> model = PivotDataAdapters.GetEmployeesWorkingDayHoursByEmployeeIdPivot(variables._idUser.Value, DateTime.Today.AddDays(-7).ToString("yyyyMMdd"), DateTime.Today.ToString("yyyyMMdd"));
            for (int i = 1; i < model[1].Count(); i++)
            {
                model[1][i] = (model[1][i] == DBNull.Value ? "0" : Utilities.DisplayAsCentiHours(Convert.ToInt64(model[1][i])));
            }
            return PartialView(model);
        }


    }
}