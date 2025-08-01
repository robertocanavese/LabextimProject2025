using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web;
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
    public class ProductionMPSController : Controller
    {
        // GET: ProductionMPS
        public ActionResult Index()
        {
            return View();
        }
    }
}