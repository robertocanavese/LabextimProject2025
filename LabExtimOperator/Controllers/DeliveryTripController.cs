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

    public class DeliveryTripControllerSessionVariables
    {
        private System.Web.SessionState.HttpSessionState _session = HttpContext.Current.Session;

        public List<DeliveryTrip> _model
        {
            get { return (List<DeliveryTrip>)_session["_model_DeliveryTrip"]; }

            set { _session["_model_DeliveryTrip"] = value; }
        }

        public int? _IDmodel
        {
            get
            {
                if (_session["_IDmodel_DeliveryTrip"] != null)
                    return (int)_session["_IDmodel_DeliveryTrip"];
                return 0;
            }

            set { _session["_IDmodel_DeliveryTrip"] = value; }
        }


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

        public int _currentCompanyId
        {
            get
            {
                if (_session["_currentCompanyId"] == null)
                {
                    _session["_currentCompanyId"] = ((DLLabExtim.LabextimUser)_session["LabextimUser"]).Employee.ID_Company.GetValueOrDefault();
                }
                return Convert.ToInt32(_session["_currentCompanyId"]);
            }
            set { _session["_currentCompanyId"] = value; }
        }

        public DateTime _currentDate
        {
            get
            {
                if (_session["_currentDate"] == null)
                {
                    _session["_currentDate"] = DateTime.Today;
                }

                return (DateTime)_session["_currentDate"];
            }
            set
            {
                _session["_currentDate"] = value;
            }
        }

    }


    public class DeliveryTripController : Controller
    {


        //[Authorize]
        //public ActionResult PopupDeliveryTripViewPartial()
        //{

        //    DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
        //    ViewBag.UserName = WebSecurity.CurrentUserName;

        //    variables._IDmodel = 0;
        //    ViewBag.IdDeliveryTrip = "Tutti";

        //    ViewBag.CurrentDate = variables._currentDate;
        //    ViewBag.IsNew = false;
        //    variables._model = DeliveryTripViewPartialSetModel();
        //    return PartialView("_PopupDeliveryTripViewPartial", variables._model);

        //}

        [Authorize]
        public ActionResult Index(DateTime? curDate, int? EvadiODP)
        {
            //DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
            //ViewBag.UserName = WebSecurity.CurrentUserName;

            //variables._IDmodel = 0;
            //ViewBag.IdDeliveryTrip = "Tutti";

            //ViewBag.CurrentDate = variables._currentDate;
            //ViewBag.IsNew = false;
            //variables._model = DeliveryTripViewPartialSetModel();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult DeliveryTripViewPartial(DateTime? CurrentDate)
        {
            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
            ViewBag.UserName = WebSecurity.CurrentUserName;

            variables._IDmodel = 0;
            ViewBag.IdDeliveryTrip = "Tutti";

            ViewBag.CurrentDate = variables._currentDate;
            ViewBag.IsNew = false;
            variables._model = DeliveryTripViewPartialSetModelList();

            return PartialView("_DeliveryTripViewPartial", variables._model);
        }

        public ActionResult GetDeliveryTrip(int? a, int? iDeliveryTrip)
        {
            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();

            using (QuotationDataContext db = new QuotationDataContext())
            {
                Employee curEmployee = db.Employees.FirstOrDefault(d => d.ID == a);
                variables._idUser = a;
                MembershipUser curUser = Membership.GetUser(curEmployee.UserGUID);
                ViewBag.UserName = curUser.UserName;
                Session["LabextimUser"] = new LabextimUser(Membership.GetUser(curUser.UserName));
                FormsAuthentication.SetAuthCookie(curUser.UserName, true);

            }

            variables._IDmodel = iDeliveryTrip.GetValueOrDefault();
            ViewBag.IDeliveryTrip = iDeliveryTrip;

            ViewBag.IsNew = false;

            return View("Index");
        }

        //public DeliveryTrip DeliveryTripViewPartialSetModel()
        //{
        //    DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
        //    return new DeliveryTrip
        //    {
        //        ID_Company = variables._currentCompanyId,
        //        CustomerCode = null,
        //        LocationCode = null,
        //        ID_Owner = variables._idUser,
        //        StartDate = DateTime.Today,
        //        EndDate = DateTime.Today,
        //        MacroRef = 411,
        //        Note = null,
        //        Status = 0
        //    };
        //}

        public List<DeliveryTrip> DeliveryTripViewPartialSetModelList()
        {
            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();

            //return new ProductionOrderDetailsInsertController().GetDeliveryTripsOfAnOwner(null, null, variables._currentCompanyId);
            //merge aziendale
            return new ProductionOrderDetailsInsertController().GetDeliveryTripsOfAnOwner(null, null, -1);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult SubmitDeliveryTrip(DeliveryTrip item)
        //{

        //    DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();

        //    if (item.Description == null)
        //    {
        //        ViewData["EditError"] = "La descrizione è obbligatoria";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupDeliveryTripViewPartial", variables._model);
        //    }

        //    if (item.ID_Owner == 0)
        //    {
        //        ViewData["EditError"] = "Operatore non selezionato";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupDeliveryTripViewPartial", variables._model);
        //    }

        //    if (item.StartDate == null)
        //    {
        //        ViewData["EditError"] = "Data inizio non selezionata";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupDeliveryTripViewPartial", variables._model);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            using (var _quotationDataContext = new QuotationDataContext())
        //            {
        //                DeliveryTrip _deliveryTrip = null;
        //                _deliveryTrip =
        //                    ProductionOrderDetailsInsertController.GetDeliveryTrip(_quotationDataContext,
        //                        item.ID);
        //                if (_deliveryTrip == null)
        //                {
        //                    _deliveryTrip = new DeliveryTrip();
        //                }
        //                else
        //                {
        //                    _deliveryTrip.ID = item.ID;
        //                }

        //                _deliveryTrip.ID_Owner = item.ID_Owner;
        //                _deliveryTrip.ID_Company = _quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Owner).ID_Company;
        //                _deliveryTrip.Description = item.Description;
        //                _deliveryTrip.CustomerCode = item.CustomerCode;
        //                _deliveryTrip.LocationCode = item.LocationCode;
        //                _deliveryTrip.StartDate = item.StartDate;
        //                _deliveryTrip.Note = item.Note;
        //                _deliveryTrip.Status = 0;
        //                _deliveryTrip.MacroRef = 411;

        //                _quotationDataContext.DeliveryTrips.InsertOnSubmit(_deliveryTrip);
        //                _quotationDataContext.SubmitChanges();

        //                //imposta l'id del nuovo inserimento
        //                item.ID = _deliveryTrip.ID;


        //                _quotationDataContext.SubmitChanges();

        //                variables._model = DeliveryTripViewPartialSetModelList();
        //                //return RedirectToAction("Index", "TempProductionOrderDetail");
        //                return RedirectToAction("Index", "DeliveryTrip");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    //return PartialView("_PopupDeliveryTripViewPartial", item);
        //    return RedirectToAction("Index", "DeliveryTrip");

        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripViewPartialAddNew(DeliveryTrip item)
        {

            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();

            if (item.ID_Owner == 0)
            {
                ViewData["EditError"] = "Operatore non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_DeliveryTripViewPartial", variables._model);
            }

            if (item.StartDate == null)
            {
                ViewData["EditError"] = "Data inizio non selezionata";
                ViewBag.IsNew = true;
                return PartialView("_DeliveryTripViewPartial", variables._model);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    using (var _quotationDataContext = new QuotationDataContext())
                    {
                        DeliveryTrip _deliveryTrip = null;
                        _deliveryTrip =
                            ProductionOrderDetailsInsertController.GetDeliveryTrip(_quotationDataContext,
                                item.ID);
                        if (_deliveryTrip == null)
                        {
                            _deliveryTrip = new DeliveryTrip();
                        }
                        else
                        {
                            _deliveryTrip.ID = item.ID;
                        }

                        _deliveryTrip.ID_Owner = item.ID_Owner;
                        _deliveryTrip.ID_Company = _quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Owner).ID_Company;
                        _deliveryTrip.Description = item.Description;
                        _deliveryTrip.CustomerCode = item.CustomerCode;
                        _deliveryTrip.LocationCode = item.LocationCode;
                        _deliveryTrip.StartDate = item.StartDate;
                        _deliveryTrip.Note = item.Note;
                        _deliveryTrip.Status = 0;
                        _deliveryTrip.MacroRef = 411;

                        _quotationDataContext.DeliveryTrips.InsertOnSubmit(_deliveryTrip);
                        _quotationDataContext.SubmitChanges();

                        //imposta l'id del nuovo inserimento
                        item.ID = _deliveryTrip.ID;


                        _quotationDataContext.SubmitChanges();

                        variables._model = DeliveryTripViewPartialSetModelList();
                        //return RedirectToAction("Index", "TempProductionOrderDetail");
                        //return RedirectToAction("Index", "DeliveryTrip");
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            //return PartialView("_PopupDeliveryTripViewPartial", item);
            return PartialView("_DeliveryTripViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripViewPartialUpdate(DeliveryTrip item)
        {

            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
            if (ModelState.IsValid)
            {
                try
                {

                    using (var _quotationDataContext = new QuotationDataContext())
                    {
                        DeliveryTrip _deliveryTrip = null;
                        _deliveryTrip =
                            ProductionOrderDetailsInsertController.GetDeliveryTrip(_quotationDataContext,
                                item.ID);
                        if (_deliveryTrip == null)
                        {
                            _deliveryTrip = new DeliveryTrip();
                        }
                        else
                        {
                            _deliveryTrip.ID = item.ID;
                        }

                        _deliveryTrip.ID_Owner = item.ID_Owner;
                        _deliveryTrip.ID_Company = _quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Owner).ID_Company;
                        _deliveryTrip.Description = item.Description;
                        _deliveryTrip.CustomerCode = item.CustomerCode;
                        _deliveryTrip.LocationCode = item.LocationCode;
                        _deliveryTrip.StartDate = item.StartDate;
                        _deliveryTrip.Note = item.Note;
                        _deliveryTrip.Status = 0;
                        _deliveryTrip.MacroRef = 411;

                        _quotationDataContext.SubmitChanges();

                        variables._model = DeliveryTripViewPartialSetModelList();

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            //return PartialView("_PopupDeliveryTripViewPartial", item);
            return PartialView("_DeliveryTripViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripViewPartialDelete(Int32 ID)
        {
            DeliveryTripControllerSessionVariables variables = new DeliveryTripControllerSessionVariables();
            ViewBag.IsNew = false;
            if (ID >= 0)
            {
                try
                {
                    DeliveryTrip pod = variables._model.Single(x => x.ID == ID);
                    new UILabExtim.ProductionOrderDetailsInsertController().DeleteDeliveryTrip(pod.ID);
                    variables._model.Remove(pod);
                    variables._model = DeliveryTripViewPartialSetModelList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DeliveryTripViewPartial", variables._model);
        }

    }
}