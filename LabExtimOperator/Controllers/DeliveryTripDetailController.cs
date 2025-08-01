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


    public class DeliveryTripDetailControllerSessionVariables
    {
        private System.Web.SessionState.HttpSessionState _session = HttpContext.Current.Session;

        public List<DeliveryTripDetail> _model
        {
            get { return (List<DeliveryTripDetail>)_session["_model_DeliveryTripDetail"]; }

            set { _session["_model_DeliveryTripDetail"] = value; }
        }

        public int? _IDmodel
        {
            get
            {
                if (_session["_IDmodel_DeliveryTripDetail"] != null)
                    return (int)_session["_IDmodel_DeliveryTripDetail"];
                return 0;
            }

            set { _session["_IDmodel_DeliveryTripDetail"] = value; }
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

        public char _currentDirection
        {
            get
            {
                if (_session["_currentDirection_DeliveryTripDetail"] == null)
                {
                    _session["_currentDirection_DeliveryTripDetail"] = 'C';
                }

                return (char)_session["_currentDirection_DeliveryTripDetail"];
            }
            set
            {
                _session["_currentDirection_DeliveryTripDetail"] = value;
            }
        }
    }


    public class DeliveryTripDetailController : Controller
    {


        //[Authorize]
        //public ActionResult Index(DateTime? curDate, int? EvadiODP)
        //{
        //    DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
        //    ViewBag.UserName = WebSecurity.CurrentUserName;

        //    //if (EvadiODP.HasValue)
        //    //{
        //    //    //evadi odp
        //    //    QuotationDataContext CtxQuotationDataContext = new QuotationDataContext();
        //    //    DLLabExtim.ProductionOrderDetail odpd = CtxQuotationDataContext.ProductionOrderDetails.Where(x => x.ID == EvadiODP).Single();
        //    //    ProductionOrder odp = CtxQuotationDataContext.ProductionOrders.Where(x => x.ID == odpd.ID_ProductionOrder).Single();
        //    //    odp.Status = 3;
        //    //    CtxQuotationDataContext.SubmitChanges();
        //    //}
        //    //else
        //    //{
        //    //    if (curDate == null)
        //    //    {
        //    //        variables._currentDate = DateTime.Today;
        //    //    }
        //    //    else
        //    //    {
        //    //        variables._currentDate = curDate.Value;
        //    //    }
        //    //}

        //    variables._IDmodel = 0;
        //    ViewBag.IdDeliveryTrip = "Tutti";

        //    ViewBag.CurrentDate = variables._currentDate;
        //    ViewBag.IsNew = false;
        //    return View();
        //}

        //public ActionResult GetDeliveryTrip(int? a, int? iDeliveryTrip)
        //{
        //    DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();

        //    using (QuotationDataContext db = new QuotationDataContext())
        //    {
        //        Employee curEmployee = db.Employees.FirstOrDefault(d => d.ID == a);
        //        variables._idUser = a;
        //        MembershipUser curUser = Membership.GetUser(curEmployee.UserGUID);
        //        ViewBag.UserName = curUser.UserName;
        //        Session["LabextimUser"] = new LabextimUser(Membership.GetUser(curUser.UserName));
        //        FormsAuthentication.SetAuthCookie(curUser.UserName, true);

        //    }

        //    variables._IDmodel = iDeliveryTrip.GetValueOrDefault();
        //    ViewBag.IDeliveryTrip = iDeliveryTrip;

        //    ViewBag.IsNew = false;

        //    return View("Index");
        //}

        public List<DeliveryTripDetail> DeliveryTripDetailViewPartialSetModelList()
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
            return new ProductionOrderDetailsInsertController().GetDeliveryTripDetailsOfAnOwner(null /*variables._idUser*/, null /*variables._IDmodel == 0 ? variables._currentDate : null as DateTime? */, variables._currentCompanyId, variables._IDmodel.GetValueOrDefault(0));
        }


        [ValidateInput(false)]
        public ActionResult DeliveryTripDetailViewPartial(DateTime? CurrentDate)
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
            ViewBag.CurrentDate = CurrentDate ?? DateTime.Today;
            ViewBag.IsNew = false;
            variables._model = DeliveryTripDetailViewPartialSetModelList();

            return PartialView("_DeliveryTripDetailViewPartial", variables._model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripDetailViewPartialAddNew(DeliveryTripDetail item)
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();

            if (item.ID_DeliveryTrip == 0)
            {
                ViewData["EditError"] = "Viaggio non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_DeliveryTripDetailViewPartial", variables._model);
            }

            if (item.ID_ProductionOrder == null)
            {
                ViewData["EditError"] = "Odp non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_DeliveryTripDetailViewPartial", variables._model);
            }

            if (item.Direction == 0)
            {
                ViewData["EditError"] = "Tipo viaggio non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_DeliveryTripDetailViewPartial", variables._model);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    using (var _quotationDataContext = new QuotationDataContext())
                    {
                        DeliveryTripDetail _deliveryTripDetail = null;
                        _deliveryTripDetail =
                            ProductionOrderDetailsInsertController.GetDeliveryTripDetail(_quotationDataContext,
                                item.ID);
                        if (_deliveryTripDetail == null)
                        {
                            _deliveryTripDetail = new DeliveryTripDetail();
                        }
                        else
                        {
                            _deliveryTripDetail.ID = item.ID;
                        }
                        _deliveryTripDetail.ID_ProductionOrder = item.ID_ProductionOrder;


                        if (item.ID_Owner == 0)
                            item.ID_Owner = null;
                        item.ID_Owner = variables._idUser;
                        _deliveryTripDetail.ID_Owner = item.ID_Owner;
                        _deliveryTripDetail.ID_DeliveryTrip = item.ID_DeliveryTrip;
                        _deliveryTripDetail.Direction = item.Direction;
                        _deliveryTripDetail.Quota = item.Quota;
                        _deliveryTripDetail.InsertDate = DateTime.Parse(item.InsertDate.Value.ToShortDateString());

                        _quotationDataContext.DeliveryTripDetails.InsertOnSubmit(_deliveryTripDetail);
                        _quotationDataContext.SubmitChanges();

                        //imposta l'id del nuovo inserimento
                        item.ID = _deliveryTripDetail.ID;


                        _quotationDataContext.SubmitChanges();

                        variables._model = DeliveryTripDetailViewPartialSetModelList();

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DeliveryTripDetailViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripDetailViewPartialEvadi(DeliveryTripDetail item)
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
            using (var _quotationDataContext = new QuotationDataContext())
            {
                //_quotationDataContext.ProductionOrders.
            }

            return PartialView("_DeliveryTripDetailViewPartial", variables._model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripDetailViewPartialUpdate(DeliveryTripDetail item)
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var _quotationDataContext = new QuotationDataContext())
                    {

                        DeliveryTripDetail _deliveryTripDetail;

                        _deliveryTripDetail = ProductionOrderDetailsInsertController.GetDeliveryTripDetail(_quotationDataContext,
                            item.ID);
                        if (_deliveryTripDetail == null)
                        {
                            _deliveryTripDetail = new DeliveryTripDetail();
                        }
                        else
                        {
                            _deliveryTripDetail.ID = item.ID;
                        }
                        _deliveryTripDetail.ID_ProductionOrder = item.ID_ProductionOrder;

                        _deliveryTripDetail.ID_Owner = variables._idUser;
                        item.ID_Owner = _deliveryTripDetail.ID_Owner;
                        _deliveryTripDetail.ID_DeliveryTrip = item.ID_DeliveryTrip;
                        _deliveryTripDetail.Direction = item.Direction;
                        _deliveryTripDetail.Quota = item.Quota;
                        _deliveryTripDetail.InsertDate = DateTime.Parse(item.InsertDate.Value.ToShortDateString());

                        _quotationDataContext.SubmitChanges();

                        variables._model = DeliveryTripDetailViewPartialSetModelList();


                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_DeliveryTripDetailViewPartial", variables._model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult DeliveryTripDetailViewPartialDelete(Int32 ID)
        {
            DeliveryTripDetailControllerSessionVariables variables = new DeliveryTripDetailControllerSessionVariables();
            ViewBag.IsNew = false;
            if (ID >= 0)
            {
                try
                {
                    DeliveryTripDetail pod = variables._model.Single(x => x.ID == ID);
                    new UILabExtim.ProductionOrderDetailsInsertController().DeleteDeliveryTripDetail(pod);
                    variables._model.Remove(pod);
                    variables._model = DeliveryTripDetailViewPartialSetModelList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DeliveryTripDetailViewPartial", variables._model);
        }




    }
}