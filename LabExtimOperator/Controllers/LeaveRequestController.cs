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

    public class LeaveRequestControllerSessionVariables
    {
        private System.Web.SessionState.HttpSessionState _session = HttpContext.Current.Session;

        public List<LeaveRequest> _model
        {
            get { return (List<LeaveRequest>)_session["_model_LeaveRequest"]; }

            set { _session["_model_LeaveRequest"] = value; }
        }

        public int? _IDmodel
        {
            get
            {
                if (_session["_IDmodel_LeaveRequest"] != null)
                    return (int)_session["_IDmodel_LeaveRequest"];
                return 0;
            }

            set { _session["_IDmodel_LeaveRequest"] = value; }
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


    public class LeaveRequestController : Controller
    {


        //[Authorize]
        //public ActionResult PopupLeaveRequestViewPartial()
        //{

        //    LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();
        //    ViewBag.UserName = WebSecurity.CurrentUserName;

        //    variables._IDmodel = 0;
        //    ViewBag.IdLeaveRequest = "Tutti";

        //    ViewBag.CurrentDate = variables._currentDate;
        //    ViewBag.IsNew = false;
        //    variables._model = LeaveRequestViewPartialSetModel();
        //    return PartialView("_PopupLeaveRequestViewPartial", variables._model);

        //}

        [Authorize]
        public ActionResult Index()
        {
            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();

            using (QuotationDataContext db = new QuotationDataContext())
            {
                Employee curEmployee = db.Employees.FirstOrDefault(d => d.ID == variables._idUser);
                ViewBag.UserName = curEmployee.UniqueName;
            }

            //variables._IDmodel = 0;
            //ViewBag.IdLeaveRequest = "Tutti";

            //ViewBag.CurrentDate = variables._currentDate;
            //ViewBag.IsNew = false;
            //variables._model = LeaveRequestViewPartialSetModel();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult LeaveRequestViewPartial(DateTime? CurrentDate)
        {
            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();
            ViewBag.UserName = WebSecurity.CurrentUserName;

            variables._IDmodel = 0;
            ViewBag.IdLeaveRequest = "Tutti";

            ViewBag.CurrentDate = variables._currentDate;
            ViewBag.IsNew = false;
            variables._model = LeaveRequestViewPartialSetModelList();

            return PartialView("_LeaveRequestViewPartial", variables._model);
        }

        public ActionResult GetLeaveRequest(int? a, int? iLeaveRequest)
        {
            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();

            using (QuotationDataContext db = new QuotationDataContext())
            {
                Employee curEmployee = db.Employees.FirstOrDefault(d => d.ID == a);
                variables._idUser = a;
                MembershipUser curUser = Membership.GetUser(curEmployee.UserGUID);
                ViewBag.UserName = curUser.UserName;
                Session["LabextimUser"] = new LabextimUser(Membership.GetUser(curUser.UserName));
                FormsAuthentication.SetAuthCookie(curUser.UserName, true);

            }

            variables._IDmodel = iLeaveRequest.GetValueOrDefault();
            ViewBag.ILeaveRequest = iLeaveRequest;

            ViewBag.IsNew = false;

            return View("Index");
        }

        //public LeaveRequest LeaveRequestViewPartialSetModel()
        //{
        //    LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();
        //    return new LeaveRequest
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

        public List<LeaveRequest> LeaveRequestViewPartialSetModelList()
        {
            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();

            //return new ProductionOrderDetailsInsertController().GetLeaveRequestsOfAnOwner(null, null, variables._currentCompanyId);
            //merge aziendale
            return new ProductionOrderDetailsInsertController().GetLeaveRequestsOfAnOwner(variables._idUser, null, -1);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult SubmitLeaveRequest(LeaveRequest item)
        //{

        //    LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();

        //    if (item.Description == null)
        //    {
        //        ViewData["EditError"] = "La descrizione è obbligatoria";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupLeaveRequestViewPartial", variables._model);
        //    }

        //    if (item.ID_Owner == 0)
        //    {
        //        ViewData["EditError"] = "Operatore non selezionato";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupLeaveRequestViewPartial", variables._model);
        //    }

        //    if (item.StartDate == null)
        //    {
        //        ViewData["EditError"] = "Data inizio non selezionata";
        //        ViewBag.IsNew = true;
        //        return PartialView("_PopupLeaveRequestViewPartial", variables._model);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            using (var _quotationDataContext = new QuotationDataContext())
        //            {
        //                LeaveRequest _LeaveRequest = null;
        //                _LeaveRequest =
        //                    ProductionOrderDetailsInsertController.GetLeaveRequest(_quotationDataContext,
        //                        item.ID);
        //                if (_LeaveRequest == null)
        //                {
        //                    _LeaveRequest = new LeaveRequest();
        //                }
        //                else
        //                {
        //                    _LeaveRequest.ID = item.ID;
        //                }

        //                _LeaveRequest.ID_Owner = item.ID_Owner;
        //                _LeaveRequest.ID_Company = _quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Owner).ID_Company;
        //                _LeaveRequest.Description = item.Description;
        //                _LeaveRequest.CustomerCode = item.CustomerCode;
        //                _LeaveRequest.LocationCode = item.LocationCode;
        //                _LeaveRequest.StartDate = item.StartDate;
        //                _LeaveRequest.Note = item.Note;
        //                _LeaveRequest.Status = 0;
        //                _LeaveRequest.MacroRef = 411;

        //                _quotationDataContext.LeaveRequests.InsertOnSubmit(_LeaveRequest);
        //                _quotationDataContext.SubmitChanges();

        //                //imposta l'id del nuovo inserimento
        //                item.ID = _LeaveRequest.ID;


        //                _quotationDataContext.SubmitChanges();

        //                variables._model = LeaveRequestViewPartialSetModelList();
        //                //return RedirectToAction("Index", "TempProductionOrderDetail");
        //                return RedirectToAction("Index", "LeaveRequest");
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //        ViewData["EditError"] = "Please, correct all errors.";
        //    //return PartialView("_PopupLeaveRequestViewPartial", item);
        //    return RedirectToAction("Index", "LeaveRequest");

        //}

        [HttpPost, ValidateInput(false)]
        public ActionResult LeaveRequestViewPartialAddNew(LeaveRequest item)
        {

            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();

            if (item.ID_Manager == 0)
            {
                ViewData["EditError"] = "Responsabile destinatario non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_LeaveRequestViewPartial", variables._model);
            }

            if (item.StartDate == null)
            {
                ViewData["EditError"] = "Data inizio assenza non selezionata";
                ViewBag.IsNew = true;
                return PartialView("_LeaveRequestViewPartial", variables._model);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    string applicantMailAddress = null;
                    string managerMailAddress = null;
                    Employee applicant = null;
                    Employee manager = null;


                    using (var _quotationDataContext = new QuotationDataContext())
                    {

                        applicantMailAddress = Membership.GetUser(_quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Applicant).UserGUID).Email;
                        managerMailAddress = Membership.GetUser(_quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Manager).UserGUID).Email;

                        LeaveRequest _LeaveRequest = null;
                        _LeaveRequest =
                            ProductionOrderDetailsInsertController.GetLeaveRequest(_quotationDataContext,
                                item.ID);
                        if (_LeaveRequest == null)
                        {
                            _LeaveRequest = new LeaveRequest();
                        }
                        else
                        {
                            _LeaveRequest.ID = item.ID;
                        }

                        _LeaveRequest.ID_Applicant = variables._idUser;
                        _LeaveRequest.ID_Company = _quotationDataContext.Employees.FirstOrDefault(d => d.ID == variables._idUser).ID_Company;
                        _LeaveRequest.LeaveType = item.LeaveType;
                        _LeaveRequest.RequestDate = DateTime.Now;
                        _LeaveRequest.StartDate = item.StartDate;
                        _LeaveRequest.EndDate = (item.EndDate == null ? item.StartDate : item.EndDate);
                        _LeaveRequest.DayFraction = (item.DayFraction == null ? 'G' : item.DayFraction);
                        _LeaveRequest.VacationDays = (item.VacationDays == null ? 1 : item.VacationDays);
                        _LeaveRequest.MessageToManager = item.MessageToManager;
                        _LeaveRequest.ID_Manager = item.ID_Manager;
                        _LeaveRequest.Status = 19;
                        _LeaveRequest.StatusDate = DateTime.Now;

                        _quotationDataContext.LeaveRequests.InsertOnSubmit(_LeaveRequest);

                        Guid g = Guid.NewGuid();
                        string autoAuthUrl = string.Format("{0}/{1}?tkn={2}", ConfigurationManager.AppSettings["LabextimUrl"], "AutoAuth.aspx", g.ToString());
                        string whereUrl = string.Format("{0}/{1}?toauthid={2}", ConfigurationManager.AppSettings["LabextimUrl"], "LeaveRequestsConsole.aspx", _LeaveRequest.ID);

                        Token token = new Token();
                        token.IdToken = g.ToString();
                        token.IdUser = _LeaveRequest.ID_Manager.GetValueOrDefault();
                        token.RedirectUrl = whereUrl;
                        _quotationDataContext.Tokens.InsertOnSubmit(token);


                        Utilities.SendMail(
                            managerMailAddress,
                            null,
                            null,
                            string.Format("Labextim - Richiesta permesso/ferie da operatore {1} a Direzione {2} ({3})", _LeaveRequest.Employee.Name + " " + _LeaveRequest.Employee.Surname, _LeaveRequest.Employee.Company.Description, _LeaveRequest.Employee1.Name + " " + _LeaveRequest.Employee1.Surname),
                            string.Format(
                            "<table cellspacing='0' cellpadding='3' style='font-family:verdana;font-size:12px'>" +
                            "<thead><tr><th style='background-color:navy;color:white' colspan='2' ><b>DETTAGLIO RICHIESTA:</b></th></tr></thead><tbody>" +
                            "<tr><td>Id richiesta:</td><td>{0}</td></tr>" +
                            "<tr><td>Azienda:</td><td>{1}</td></tr>" +
                            "<tr><td>Richiedente:</td><td>{2}</td></tr>" +
                            "<tr><td>Data richiesta:</td><td>{3}</td></tr>" +
                            "<tr><td>Tipo permesso:</td><td>{4}</td></tr>" +
                            "<tr><td>Data inizio assenza:</td><td>{5}</td></tr>" +
                            "<tr><td>Data fine assenza:</td><td>{6}</td></tr>" +
                            "<tr><td>Orario:</td><td>{7}</td></tr>" +
                            "<tr><td>Giorni di assenza:</td><td>{8}</td></tr>" +
                            "<tr><td>Responsabile:</td><td>{9}</td></tr>" +
                            "<tr><td>Messaggio a responsabile:</td><td>{10}</td></tr>" +
                            "<tr><td>Stato richiesta:</td><td>{11}</td></tr>" +
                            "<tr><td>Aggiornamento:</td><td>{12}</td></tr>" +
                            "<tr><td>Gestita da:</td>{13}</td></tr>" +
                            "<tr><td>Messaggio a richiedente:</td><td>{14}</td></tr>" +
                            "<tr><td colspan='2' >Per autorizzare la richiesta premere sul link sottostante</td></tr>" +
                            "<tr><td colspan='2' >{15}</td></tr>" +
                            "</tbody><table>",
                            _LeaveRequest.ID,
                            _LeaveRequest.Company.Description,
                            _LeaveRequest.Employee.Name + " " + _LeaveRequest.Employee.Surname,
                            _LeaveRequest.RequestDate.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm"),
                            _LeaveRequest.StartDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                            _LeaveRequest.EndDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                            _LeaveRequest.DayFraction1.Description,
                            _LeaveRequest.VacationDays,
                            _LeaveRequest.Employee1.Name + " " + _LeaveRequest.Employee1.Surname,
                            _LeaveRequest.MessageToManager,
                            _LeaveRequest.Statuse.Description,
                            _LeaveRequest.StatusDate.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm"),
                            _LeaveRequest.Employee2.Name + " " + _LeaveRequest.Employee2.Surname,
                            _LeaveRequest.MessageToApplicant,
                            autoAuthUrl
                            ),
                            applicantMailAddress);


                        _quotationDataContext.SubmitChanges();

                        //imposta l'id del nuovo inserimento
                        item.ID = _LeaveRequest.ID;


                        _quotationDataContext.SubmitChanges();

                        variables._model = LeaveRequestViewPartialSetModelList();
                        //return RedirectToAction("Index", "TempProductionOrderDetail");
                        //return RedirectToAction("Index", "LeaveRequest");
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            //return PartialView("_PopupLeaveRequestViewPartial", item);
            return PartialView("_LeaveRequestViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LeaveRequestViewPartialUpdate(LeaveRequest item)
        {

            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();
            if (ModelState.IsValid)
            {
                try
                {
                    string applicantMailAddress = null;
                    string managerMailAddress = null;

                    using (var _quotationDataContext = new QuotationDataContext())
                    {

                        applicantMailAddress = Membership.GetUser(_quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Applicant).UserGUID).Email;
                        managerMailAddress = Membership.GetUser(_quotationDataContext.Employees.FirstOrDefault(d => d.ID == item.ID_Manager).UserGUID).Email;

                        LeaveRequest _LeaveRequest = null;
                        _LeaveRequest =
                            ProductionOrderDetailsInsertController.GetLeaveRequest(_quotationDataContext,
                                item.ID);
                        if (_LeaveRequest == null)
                        {
                            _LeaveRequest = new LeaveRequest();
                        }
                        else
                        {
                            _LeaveRequest.ID = item.ID;
                        }

                        if (_LeaveRequest.Status > 19)
                        {
                            throw new Exception("Impossibile modificare, nel frattempo la richiesta è già stata evasa dal responsabile!");
                        }

                        _LeaveRequest.LeaveType = item.LeaveType;
                        _LeaveRequest.RequestDate = DateTime.Now;
                        _LeaveRequest.StartDate = item.StartDate;
                        _LeaveRequest.EndDate = (item.EndDate == null ? item.StartDate : item.EndDate);
                        _LeaveRequest.DayFraction = (item.DayFraction == null ? 'G' : item.DayFraction);
                        _LeaveRequest.VacationDays = (item.VacationDays == null ? 1 : item.VacationDays);
                        _LeaveRequest.MessageToManager = item.MessageToManager;
                        _LeaveRequest.ID_Manager = item.ID_Manager;
                        _LeaveRequest.Status = 19;
                        _LeaveRequest.StatusDate = DateTime.Now;


                        Guid g = Guid.NewGuid();
                        string autoAuthUrl = string.Format("{0}/{1}?tkn={2}", ConfigurationManager.AppSettings["LabextimUrl"], "AutoAuth.aspx", g.ToString());
                        string whereUrl = string.Format("{0}/{1}?toauthid={2}", ConfigurationManager.AppSettings["LabextimUrl"], "LeaveRequestsConsole.aspx", _LeaveRequest.ID);

                        Token token = new Token();
                        token.IdToken = g.ToString();
                        token.IdUser = _LeaveRequest.ID_Manager.GetValueOrDefault();
                        token.RedirectUrl = whereUrl;
                        _quotationDataContext.Tokens.InsertOnSubmit(token);


                        Utilities.SendMail(
                            managerMailAddress,
                            null,
                            null,
                            string.Format("Labextim - Richiesta permesso/ferie da operatore {1} a Direzione {2} ({3})", _LeaveRequest.Employee.Name + " " + _LeaveRequest.Employee.Surname, _LeaveRequest.Employee.Company.Description, _LeaveRequest.Employee1.Name + " " + _LeaveRequest.Employee1.Surname),
                            string.Format(
                            "<table cellspacing='0' cellpadding='3' style='font-family:verdana;font-size:12px'>" +
                            "<thead><tr><th style='background-color:navy;color:white' colspan='2' ><b>DETTAGLIO RICHIESTA:</b></th></tr></thead><tbody>" +
                            "<tr><td>Id richiesta:</td><td>{0}</td></tr>" +
                            "<tr><td>Azienda:</td><td>{1}</td></tr>" +
                            "<tr><td>Richiedente:</td><td>{2}</td></tr>" +
                            "<tr><td>Data richiesta:</td><td>{3}</td></tr>" +
                            "<tr><td>Tipo permesso:</td><td>{4}</td></tr>" +
                            "<tr><td>Data inizio assenza:</td><td>{5}</td></tr>" +
                            "<tr><td>Data fine assenza:</td><td>{6}</td></tr>" +
                            "<tr><td>Orario:</td><td>{7}</td></tr>" +
                            "<tr><td>Giorni di assenza:</td><td>{8}</td></tr>" +
                            "<tr><td>Responsabile:</td><td>{9}</td></tr>" +
                            "<tr><td>Messaggio a responsabile:</td><td>{10}</td></tr>" +
                            "<tr><td>Stato richiesta:</td><td>{11}</td></tr>" +
                            "<tr><td>Aggiornamento:</td><td>{12}</td></tr>" +
                            "<tr><td>Gestita da:</td>{13}</td></tr>" +
                            "<tr><td>Messaggio a richiedente:</td><td>{14}</td></tr>" +
                            "<tr><td colspan='2' >Per autorizzare la richiesta premere sul link sottostante</td></tr>" +
                            "<tr><td colspan='2' >{15}</td></tr>" +
                            "</tbody><table>",
                            _LeaveRequest.ID,
                            _LeaveRequest.Company.Description,
                            _LeaveRequest.Employee.Name + " " + _LeaveRequest.Employee.Surname,
                            _LeaveRequest.RequestDate.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm"),
                            _LeaveRequest.StartDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                            _LeaveRequest.EndDate.GetValueOrDefault().ToString("yyyy/MM/dd"),
                            _LeaveRequest.DayFraction1.Description,
                            _LeaveRequest.VacationDays,
                            _LeaveRequest.Employee1.Name + " " + _LeaveRequest.Employee1.Surname,
                            _LeaveRequest.MessageToManager,
                            _LeaveRequest.Statuse.Description,
                            _LeaveRequest.StatusDate.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm"),
                            _LeaveRequest.Employee2.Name + " " + _LeaveRequest.Employee2.Surname,
                            _LeaveRequest.MessageToApplicant,
                            autoAuthUrl
                            ),
                            applicantMailAddress);

                        _quotationDataContext.SubmitChanges();

                        variables._model = LeaveRequestViewPartialSetModelList();

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Si prega di correggere tutti gli errori.";
            //return PartialView("_PopupLeaveRequestViewPartial", item);
            return PartialView("_LeaveRequestViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult LeaveRequestViewPartialDelete(Int32 ID)
        {
            LeaveRequestControllerSessionVariables variables = new LeaveRequestControllerSessionVariables();
            ViewBag.IsNew = false;
            if (ID >= 0)
            {
                try
                {
                    LeaveRequest pod = variables._model.Single(x => x.ID == ID);
                    new UILabExtim.ProductionOrderDetailsInsertController().DeleteLeaveRequest(pod.ID);
                    //variables._model.Remove(pod);
                    variables._model = LeaveRequestViewPartialSetModelList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_LeaveRequestViewPartial", variables._model);
        }

    }
}