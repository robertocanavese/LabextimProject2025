using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Net;

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

    public class TempProductionOrderDetailControllerSessionVariables
    {
        private System.Web.SessionState.HttpSessionState _session = HttpContext.Current.Session;

        public List<TempProductionOrderDetail> _model
        {
            get { return (List<TempProductionOrderDetail>)_session["_model"]; }

            set { _session["_model"] = value; }
        }

        public int? _IDmodel
        {
            get
            {
                if (_session["_IDmodel"] != null)
                    return (int)_session["_IDmodel"];
                return 0;
            }

            set { _session["_IDmodel"] = value; }
        }

        public int? _IDquotationDetail
        {
            get
            {
                if (_session["_IDquotationDetail"] != null)
                    return (int)_session["_IDquotationDetail"];
                return 0;
            }

            set { _session["_IDquotationDetail"] = value; }
        }

        public List<string> listaIdArticoli
        {
            get
            {
                if ((List<string>)_session["listaIdArticoli"] == null)
                {
                    _session["listaIdArticoli"] = new List<string>();
                }

                return (List<string>)_session["listaIdArticoli"];
            }
            set
            {
                _session["listaIdArticoli"] = value;
            }
        }

        public List<string> listaIdArticoli2
        {
            get
            {
                if ((List<string>)_session["listaIdArticoli2"] == null)
                {
                    _session["listaIdArticoli2"] = new List<string>();
                }

                return (List<string>)_session["listaIdArticoli2"];
            }
            set
            {
                _session["listaIdArticoli2"] = value;
            }
        }
        public List<Articolo> data1
        {
            get
            {
                if ((List<Articolo>)_session["data1"] == null)
                {
                    _session["data1"] = new List<Articolo>();
                }

                return (List<Articolo>)_session["data1"];
            }
            set
            {
                _session["data1"] = value;
            }
        }
        public List<Articolo> data2
        {
            get
            {
                if ((List<Articolo>)_session["data2"] == null)
                {
                    _session["data2"] = new List<Articolo>();
                }

                return (List<Articolo>)_session["data2"];
            }
            set
            {
                _session["data2"] = value;
            }
        }

        public string idUno
        {
            get { return _session["idUno"].ToString(); }
            set { _session["idUno"] = value; }
        }

        public string idDue
        {
            get { return _session["idDue"].ToString(); }
            set { _session["idDue"] = value; }
        }
        public int? _idUser
        {
            get
            {
                if (_session["_idUser"] == null || _session["_idUser"].ToString() == "0")
                {
                    //var ctxOperator = new LabExtimOperatorEntities();
                    //_session["_idUser"] = ctxOperator.UserProfile.Single(x => x.UserName == WebSecurity.CurrentUserName).UserLabeId;
                    _session["_idUser"] = new LabextimUser(Membership.GetUser()).Employee.ID;
                }
                return (int?)_session["_idUser"];
            }
            set { _session["_idUser"] = value; }
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

        public CloseOfDay _currentCloseOfDay
        {
            get
            {
                //var ctx = new LabExtimEntities();
                using (QuotationDataContext ctx = new QuotationDataContext())
                {
                    CloseOfDay temp = ctx.CloseOfDays.SingleOrDefault(x => x.Id_User == _idUser.Value && x.ProductionDate == _currentDate);
                    if (temp != null)
                    { _session["_currentCloseOfDay"] = temp; }
                    else
                    { _session["_currentCloseOfDay"] = new CloseOfDay { Id_User = _idUser.Value, ProductionDate = _currentDate, Note = string.Empty }; }
                    return (CloseOfDay)_session["_currentCloseOfDay"];
                }
            }
            set { _session["_currentCloseOfDay"] = value; }
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

    }


    public class TempProductionOrderDetailController : Controller
    {


        public ActionResult ListBoxPartial(string selectedFeatures)
        {

            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            QuotationDataContext CtxQuotationDataContext = new QuotationDataContext();
            LabextimUser currentUser = new LabextimUser(Membership.GetUser());

            ViewBag.SelectIndex1 = -1;

            var dataPrec = new List<Articolo>();
            variables.idUno = "";

            if (string.IsNullOrWhiteSpace(selectedFeatures))
            {
                if (variables.listaIdArticoli.Count > 1)
                {
                    selectedFeatures = variables.listaIdArticoli[variables.listaIdArticoli.Count - 2];
                    variables.listaIdArticoli.RemoveAt(variables.listaIdArticoli.Count - 1);
                    variables.listaIdArticoli.RemoveAt(variables.listaIdArticoli.Count - 1);
                }


            }

            if (string.IsNullOrWhiteSpace(selectedFeatures))
            {


                variables.listaIdArticoli.Clear();
                dataPrec = CtxQuotationDataContext.VW_MenuItems
                    .Join(CtxQuotationDataContext.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                    //.Where(pi => pi.Inserted && pi.ID_Company == currentUser.Employee.ID_Company)
                    // merge aziendale
                    .Where(pi => pi.Inserted)
                    .Select(pt => new { pt.TypeCode, pt.Type.Order })
                    .Distinct()
                    .OrderBy(pt => pt.Order)
                    .Select(
                        pt =>
                            new Articolo
                            {
                                Description =
                                    CtxQuotationDataContext.Types.Single(x => x.Code == pt.TypeCode).Description,
                                Id = CtxQuotationDataContext.Types.Single(x => x.Code == pt.TypeCode).Code.ToString(),
                                IdPadre = "0"
                            }).ToList();

            }
            else
            {
                variables.listaIdArticoli.Add(selectedFeatures);
            }
            if (variables.listaIdArticoli.Count == 1)
            {
                dataPrec = CtxQuotationDataContext.VW_MenuItems
                            .Join(CtxQuotationDataContext.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                    //.Where(pi => pi.Inserted && pi.Type.Code.ToString() == selectedFeatures && pi.ID_Company == currentUser.Employee.ID_Company)
                    // merge aziendale
                            .Where(pi => pi.Inserted && pi.Type.Code.ToString() == selectedFeatures)
                            .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                            .Distinct()
                            .OrderBy(pt => pt.Order)
                            .Select(pt =>
                            new Articolo
                            {
                                Description =
                                    CtxQuotationDataContext.ItemTypes.Single(x => x.Code == pt.ItemTypeCode).Description,
                                Id = CtxQuotationDataContext.ItemTypes.Single(x => x.Code == pt.ItemTypeCode).Code.ToString(),
                                IdPadre = selectedFeatures
                            }).ToList();
            }

            else if (variables.listaIdArticoli.Count == 2)
            {
                dataPrec.Clear();
                var unCastedDistinctPickingItems =
                               CtxQuotationDataContext.VW_MenuItems.Where(
                                   pi =>
                                       //pi.ID_Company == currentUser.Employee.ID_Company &&
                                       // merge aziendale
                                       pi.TypeCode.ToString() == variables.listaIdArticoli[0] && pi.ItemTypeCode.ToString() == variables.listaIdArticoli[variables.listaIdArticoli.Count - 1] &&
                                       pi.Inserted)
                                   .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                   .Distinct()
                                   .ToList();
                var distinctPickingItems = unCastedDistinctPickingItems.
                    OrderBy(o => o.Order)
                    .ThenByDescending(o => o.Cost)
                    .ThenBy(o => o.ItemDescription)
                    .
                    Select(o => o.ID).ToList();
                for (var k = 0; k < distinctPickingItems.Count; k++)
                {
                    var pickingItem =
                        CtxQuotationDataContext.VW_MenuItems.SingleOrDefault(pi => pi.ID == distinctPickingItems[k]);

                    if (distinctPickingItems[k].Substring(0, 1) == "P")
                    {
                        dataPrec.Add(new Articolo
                        {
                            Description = pickingItem.ItemDescription,
                            Id = pickingItem.ID,
                            IdPadre = variables.listaIdArticoli[1]
                        });
                    }
                    if (distinctPickingItems[k].Substring(0, 1) == "M")
                    {
                        dataPrec.Add(new Articolo
                        {
                            Description = "[" + pickingItem.ItemDescription + "]",
                            Id = pickingItem.ID,
                            IdPadre = variables.listaIdArticoli[1]
                        });
                    }
                }

            }
            if (dataPrec.Any())
            {
                variables.data1 = dataPrec;
                ViewBag.UM1 = string.Empty;
            }
            else
            {
                if (selectedFeatures != null)
                {
                    if (!variables.data1.First().Description.StartsWith("("))
                    {
                        var umdesc = string.Empty;
                        var um = 0;
                        try
                        {
                            //mod 20240725 
                            //um = variables.data1.First().Description.StartsWith("[")
                            //    ? CtxQuotationDataContext.MacroItems.SingleOrDefault(
                            //        x =>
                            //            x.MacroItemDescription ==
                            //            variables.data1.First().Description.Substring(1, variables.data1.First().Description.Length - 2)).UM
                            //    : CtxQuotationDataContext.PickingItems.SingleOrDefault(
                            //        x => x.ItemDescription == variables.data1.First().Description).UM;


                            um = variables.data1.First().Description.StartsWith("[")
                                ? CtxQuotationDataContext.MacroItems.FirstOrDefault(
                                    x =>
                                        x.MacroItemDescription ==
                                        variables.data1.First().Description.Split('|')[0].TrimEnd()).UM
                                : CtxQuotationDataContext.PickingItems.FirstOrDefault(
                                    x => x.ItemDescription ==
                                        variables.data1.First().Description.Split('|')[0].TrimEnd()).UM;

                        }
                        catch
                        {
                        }

                        if (um != 0)
                        {
                            umdesc = CtxQuotationDataContext.Units.FirstOrDefault(x => x.ID == um).Description.Trim();
                        }
                        variables.data1.FirstOrDefault().Description = string.Format("{0}{1}",
                            variables.data1.FirstOrDefault().Description.Split('|')[0].TrimEnd(), " | " + umdesc);
                    }

                    var id = selectedFeatures;
                    variables.data1 = variables.data1.Where(x => x.Id == id).ToList();
                    variables.idUno = variables.data1.Single(x => x.Id == id).Id;
                    ViewBag.SelectIndex1 = 0;
                }
            }
            return PartialView("ListBoxPartial", variables.data1);
        }

        public ActionResult ListBoxPartial2(string selectedFeatures)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            QuotationDataContext CtxQuotationDataContext = new QuotationDataContext();
            LabextimUser currentUser = new LabextimUser(Membership.GetUser());

            ViewBag.SelectIndex1 = -1;
            var data = new List<Articolo>();
            var dataPrec = new List<Articolo>();
            variables.idDue = "";

            if (string.IsNullOrWhiteSpace(selectedFeatures))
            {
                if (variables.listaIdArticoli2.Count > 1)
                {
                    selectedFeatures = variables.listaIdArticoli2[variables.listaIdArticoli2.Count - 2];
                    variables.listaIdArticoli2.RemoveAt(variables.listaIdArticoli2.Count - 1);
                    variables.listaIdArticoli2.RemoveAt(variables.listaIdArticoli2.Count - 1);
                }


            }



            if (string.IsNullOrWhiteSpace(selectedFeatures))
            {
                variables.listaIdArticoli2.Clear();
                dataPrec = CtxQuotationDataContext.VW_MenuItems
                    .Join(CtxQuotationDataContext.Types, pi => pi.TypeCode, t => t.Code, (pi, t) => pi)
                    //.Where(pi => pi.Inserted && pi.ID_Company == currentUser.Employee.ID_Company)
                    // merge aziendale
                    .Where(pi => pi.Inserted)
                    .Select(pt => new { pt.TypeCode, pt.Type.Order })
                    .Distinct()
                    .OrderBy(pt => pt.Order)
                    .Select(
                        pt =>
                            new Articolo
                            {
                                Description =
                                    CtxQuotationDataContext.Types.Single(x => x.Code == pt.TypeCode).Description,
                                Id = CtxQuotationDataContext.Types.Single(x => x.Code == pt.TypeCode).Code.ToString(),
                                IdPadre = "0"
                            }).ToList();
            }
            else
            {
                variables.listaIdArticoli2.Add(selectedFeatures);
            }
            if (variables.listaIdArticoli2.Count == 1)
            {
                dataPrec = CtxQuotationDataContext.VW_MenuItems
                            .Join(CtxQuotationDataContext.ItemTypes, pi => pi.ItemTypeCode, t => t.Code, (pi, t) => pi)
                    //.Where(pi => pi.Inserted && pi.Type.Code.ToString() == selectedFeatures &&  pi.ID_Company == currentUser.Employee.ID_Company)
                    // merge aziendale
                            .Where(pi => pi.Inserted && pi.Type.Code.ToString() == selectedFeatures)
                            .Select(pt => new { pt.ItemTypeCode, pt.ItemType.Order })
                            .Distinct()
                            .OrderBy(pt => pt.Order)
                            .Select(pt =>
                            new Articolo
                            {
                                Description =
                                    CtxQuotationDataContext.ItemTypes.Single(x => x.Code == pt.ItemTypeCode).Description,
                                Id = CtxQuotationDataContext.ItemTypes.Single(x => x.Code == pt.ItemTypeCode).Code.ToString(),
                                IdPadre = selectedFeatures
                            }).ToList();
            }

            else if (variables.listaIdArticoli2.Count == 2)
            {
                var unCastedDistinctPickingItems =
                               CtxQuotationDataContext.VW_MenuItems.Where(
                                   pi =>
                                       //pi.ID_Company == currentUser.Employee.ID_Company &&
                                       // merge aziendale
                                       pi.TypeCode.ToString() == variables.listaIdArticoli2[0] && pi.ItemTypeCode.ToString() == variables.listaIdArticoli2[1] &&
                                       pi.Inserted)
                                   .Select(l => new { l.ID, l.Order, l.Cost, l.ItemDescription })
                                   .Distinct()
                                   .ToList();
                var distinctPickingItems = unCastedDistinctPickingItems.
                    OrderBy(o => o.Order)
                    .ThenByDescending(o => o.Cost)
                    .ThenBy(o => o.ItemDescription)
                    .
                    Select(o => o.ID).ToList();
                dataPrec.Clear();
                for (var k = 0; k < distinctPickingItems.Count; k++)
                {
                    var pickingItem =
                        CtxQuotationDataContext.VW_MenuItems.SingleOrDefault(pi => pi.ID == distinctPickingItems[k]);

                    if (distinctPickingItems[k].Substring(0, 1) == "P")
                    {
                        dataPrec.Add(new Articolo
                        {
                            Description = pickingItem.ItemDescription,
                            Id = pickingItem.ID,
                            IdPadre = variables.listaIdArticoli2[1]
                        });
                    }
                    if (distinctPickingItems[k].Substring(0, 1) == "M")
                    {
                        dataPrec.Add(new Articolo
                        {
                            Description = "[" + pickingItem.ItemDescription + "]",
                            Id = pickingItem.ID,
                            IdPadre = variables.listaIdArticoli2[1]
                        });
                    }
                }
            }
            if (dataPrec.Any())
            {
                variables.data2 = dataPrec;

            }
            else
            {
                if (selectedFeatures != null)
                {
                    if (!variables.data2.First().Description.StartsWith("("))
                    {
                        var umdesc = string.Empty;
                        var um = 0;
                        try
                        {

                            // modificato 20240725
                            //um = variables.data2.First().Description.StartsWith("[")
                            //    ? CtxQuotationDataContext.MacroItems.SingleOrDefault(
                            //        x =>
                            //            x.MacroItemDescription ==
                            //            variables.data2.First().Description.Substring(1, variables.data2.First().Description.Length - 2)).UM
                            //    : CtxQuotationDataContext.PickingItems.SingleOrDefault(
                            //        x => x.ItemDescription == variables.data1.First().Description).UM;

                            um = variables.data2.First().Description.StartsWith("[")
                                ? CtxQuotationDataContext.MacroItems.FirstOrDefault(
                                    x =>
                                        x.MacroItemDescription ==
                                        variables.data2.First().Description.Split('|')[0].TrimEnd()).UM
                                : CtxQuotationDataContext.PickingItems.FirstOrDefault(
                                    x => x.ItemDescription ==
                                        variables.data2.First().Description.Split('|')[0].TrimEnd()).UM;

                        }
                        catch
                        {
                        }

                        if (um != 0)
                        {
                            umdesc = CtxQuotationDataContext.Units.FirstOrDefault(x => x.ID == um).Description.Trim();
                        }

                        variables.data2.FirstOrDefault().Description = string.Format("{0}{1}",
                            variables.data2.FirstOrDefault().Description.Split('|')[0].TrimEnd(), " | " + umdesc);
                    }
                    var id = selectedFeatures;
                    variables.data2 = variables.data2.Where(x => x.Id == id).ToList();
                    variables.idDue = variables.data2.Single(x => x.Id == id).Id;
                    ViewBag.SelectIndex1 = 0;
                }
            }
            return PartialView("ListBoxPartial2", variables.data2);
        }


        [Authorize]
        public ActionResult Index(DateTime? curDate, int? EvadiODP)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            ViewBag.UserName = WebSecurity.CurrentUserName;

            if (EvadiODP.HasValue)
            {
                //evadi odp
                using (QuotationDataContext _ctx = new QuotationDataContext())
                {
                    ProductionOrderDetail odpd = _ctx.ProductionOrderDetails.Where(x => x.ID == EvadiODP).Single();
                    ProductionOrder odp = _ctx.ProductionOrders.Where(x => x.ID == odpd.ID_ProductionOrder).Single();
                    odp.Status = 3;
                    _ctx.SubmitChanges();
                }
            }
            else
            {
                if (curDate == null)
                {
                    variables._currentDate = DateTime.Today;
                }
                else
                {
                    variables._currentDate = curDate.Value;
                }
            }

            variables._IDmodel = 0;
            variables._IDquotationDetail = 0;
            ViewBag.IdODP = "Tutti";

            ViewBag.CurrentDate = variables._currentDate;
            ViewBag.IsNew = false;
            ViewBag.CurrentCloseOfDaysNote = variables._currentCloseOfDay.Note;
            return View();
        }


        public ActionResult GetOdP(int? a, int? idODP, int? qd)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();

            using (QuotationDataContext db = new QuotationDataContext())
            {
                Employee curEmployee = db.Employees.FirstOrDefault(d => d.ID == a);
                variables._idUser = a;
                MembershipUser curUser = Membership.GetUser(curEmployee.UserGUID);
                ViewBag.UserName = curUser.UserName;
                Session["LabextimUser"] = new LabextimUser(Membership.GetUser(curUser.UserName));
                FormsAuthentication.SetAuthCookie(curUser.UserName, true);

            }

            //using (LabExtimOperatorEntities db = new LabExtimOperatorEntities())
            //{
            //    UserProfile curUser = db.UserProfile.FirstOrDefault(d => d.UserLabeId == a);
            //    ViewBag.UserName = curUser.UserName;
            //}

            variables._IDmodel = idODP.GetValueOrDefault();
            variables._IDquotationDetail = qd.GetValueOrDefault();
            ViewBag.IdODP = idODP;

            ViewBag.CurrentDate = variables._currentDate;
            ViewBag.IsNew = false;
            ViewBag.CurrentCloseOfDaysNote = variables._currentCloseOfDay.Note;

            return View("Index");
        }


        public ActionResult CloseDay(DateTime? curDate, string txtCloseOfDayNote)
        {

            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            ViewBag.UserName = WebSecurity.CurrentUserName;

            if (curDate == null)
            {
                variables._currentDate = DateTime.Today;
            }
            else
            {
                variables._currentDate = curDate.Value;
            }

            //var ctxOperator = new LabExtimOperatorEntities();
            //int idUser = ctxOperator.UserProfile.Single(x => x.UserName == WebSecurity.CurrentUserName).UserLabeId.Value;
            //var ctx = new QuotationDataContext();
            //Employee labextimUser = ctx.Employees.FirstOrDefault(e => e.ID == idUser);

            LabextimUser currentUser = new LabextimUser(Membership.GetUser());
            int idUser = currentUser.Employee.ID;

            CloseOfDay closeOfDays = new CloseOfDay();
            closeOfDays.ProductionDate = curDate.Value;
            closeOfDays.Id_User = idUser;
            closeOfDays.ID_Company = currentUser.Employee.ID_Company;
            closeOfDays.Note = txtCloseOfDayNote;
            variables._currentCloseOfDay = closeOfDays;

            ViewBag.CurrentDate = variables._currentDate;
            ViewBag.IsNew = false;
            ViewBag.CurrentCloseOfDaysNote = txtCloseOfDayNote;



            using (QuotationDataContext db = new QuotationDataContext())
            {
                CloseOfDay found = db.CloseOfDays.FirstOrDefault(c => c.ProductionDate == curDate && c.Id_User == idUser);
                if (found == null)
                {
                    found = new CloseOfDay();
                    found.ProductionDate = curDate.Value;
                    found.Id_User = idUser;
                    found.ID_Company = currentUser.Employee.ID_Company;
                    found.Note = txtCloseOfDayNote;
                    db.CloseOfDays.InsertOnSubmit(found);
                }
                else
                    found.Note = txtCloseOfDayNote;

                try
                {
                    var securityProtocol = (int)System.Net.ServicePointManager.SecurityProtocol;
                    // 0 = SystemDefault in .NET 4.7+
                    if (securityProtocol != 0)
                    {
                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    }

                    ViewBag.SuccessMessage = "";
                    ViewBag.ErrorMessage = "";
                    //#if !DEBUG
                    if (currentUser.Employee.ID_Company == 1)
                        Utilities.SendMail(ConfigurationManager.AppSettings["mailAddressToCSV_Azienda01"], null, null, string.Format("Labextim - Segnalazione ({0}) da operatore {1} a Direzione {2}", curDate.Value.ToString("dd/MM/yyyy"), currentUser.Employee.Name + " " + currentUser.Employee.Surname, currentUser.Employee.Company.Description), txtCloseOfDayNote, ConfigurationManager.AppSettings["mailAddressFrom"]);
                    if (currentUser.Employee.ID_Company == 2)
                        Utilities.SendMail(ConfigurationManager.AppSettings["mailAddressToCSV_Azienda02"], null, null, string.Format("Labextim - Segnalazione ({0}) da operatore {1} a Direzione {2}", curDate.Value.ToString("dd/MM/yyyy"), currentUser.Employee.Name + " " + currentUser.Employee.Surname, currentUser.Employee.Company.Description), txtCloseOfDayNote, ConfigurationManager.AppSettings["mailAddressFrom"]);
                    //#endif
                    ViewBag.SuccessMessage = "Messaggio registrato - Mail inviata con successo!";
                }
                catch (Exception ex)
                {
                    Log.WriteMessage(ex.Message);
                    ViewBag.ErrorMessage = string.Format("Messaggio registrato - {0}", ex.Message);
                }
                db.SubmitChanges();

            }
            return View("Index");
        }


        public List<TempProductionOrderDetail> TempProductionOrderViewPartialSetModelList()
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            //var ctxOperator = new LabExtimOperatorEntities();

            //variables._idUser = ctxOperator.UserProfile.Single(x => x.UserName == WebSecurity.CurrentUserName).UserLabeId;
            //return new ProductionOrderDetailsInsertController().GetProductionOrderDetailsOfAnOwner(variables._idUser, variables._IDmodel == 0 ? variables._currentDate : null as DateTime?, variables._currentCompanyId, variables._IDmodel.GetValueOrDefault(0));
            //merge aziendale
            return new ProductionOrderDetailsInsertController().GetProductionOrderDetailsOfAnOwner(variables._idUser, variables._IDmodel == 0 ? variables._currentDate : null as DateTime?, 0, variables._IDmodel.GetValueOrDefault(0));
        }


        [ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartial(DateTime? CurrentDate)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            ViewBag.CurrentDate = CurrentDate ?? DateTime.Today;
            ViewBag.IsNew = false;
            variables._model = TempProductionOrderViewPartialSetModelList();

            return PartialView("_TempProductionOrderViewPartial", variables._model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartialAddNew(TempProductionOrderDetail item)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            if (item.ID_Phase == null)
            {
                ViewData["EditError"] = "Fase non selezionata";
                ViewBag.IsNew = true;
                return PartialView("_TempProductionOrderViewPartial", variables._model);
            }

            if (item.ProductionDate == null)
            {
                ViewData["EditError"] = "Data non selezionata";
                ViewBag.IsNew = true;
                return PartialView("_TempProductionOrderViewPartial", variables._model);
            }

            /*
            if (item.ProductionDateTime == DateTime.MinValue)
            {
                ViewData["EditError"] = "Tempo di lavoro non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_TempProductionOrderViewPartial", variables._model);
            }
             */

            /*
            if (string.IsNullOrWhiteSpace(variables.idUno))
            {
                ViewData["EditError"] = "Articolo di base non selezionato";
                ViewBag.IsNew = true;
                return PartialView("_TempProductionOrderViewPartial", variables._model);
            }
            */

            //if (!string.IsNullOrWhiteSpace(variables.idUno) && item.RawMaterialQuantity == null)
            //{
            //    ViewData["EditError"] = "Non è stata selezionata la quantità della voce di base";
            //    ViewBag.IsNew = true;
            //    return PartialView("_TempProductionOrderViewPartial", variables._model);
            //}

            //if (!string.IsNullOrWhiteSpace(variables.idDue) && item.RawMaterialX == null)
            //{
            //    ViewData["EditError"] = "Non è stata inserito il numero di copie";
            //    ViewBag.IsNew = true;
            //    return PartialView("_TempProductionOrderViewPartial", variables._model);
            //}


            if (ModelState.IsValid)
            {
                try
                {
                    //var ctxOperator = new LabExtimEntities();
                    item.QuantityOver = false;
                    item.State = TempProductionOrderDetail.ItemState.Passed;
                    using (var _quotationDataContext = new QuotationDataContext())
                    {
                        ProductionOrderDetail _productionOrderDetail = null;
                        _productionOrderDetail =
                            ProductionOrderDetailsInsertController.GetProductionOrderDetail(_quotationDataContext,
                                item.ID);
                        if (_productionOrderDetail == null)
                        {
                            _productionOrderDetail = new ProductionOrderDetail();
                        }
                        else
                        {
                            _productionOrderDetail.ID = item.ID;
                        }
                        _productionOrderDetail.ID_ProductionOrder = item.ID_ProductionOrder;

                        if (item.ID_Owner == 0)
                            item.ID_Owner = null;
                        item.ID_Owner = variables._idUser;
                        _productionOrderDetail.ID_Owner = item.ID_Owner;


                        //_productionOrderDetail.ID_Company = variables._currentCompanyId;
                        //item.ID_Company = variables._currentCompanyId;
                        // merge aziendale
                        int idCompany = ProductionOrderDetailsInsertController.GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, item);
                        _productionOrderDetail.ID_Company = idCompany;
                        item.ID_Company = idCompany;

                        if (item.ID_Phase == 0)
                            item.ID_Phase = null;
                        _productionOrderDetail.ID_Phase = item.ID_Phase;

                        //new UILabExtim.ProductionOrderDetailsInsertController().ChangeProductionOrderScheduleStatus(_quotationDataContext, item.ID_ProductionOrder, item.ID_Phase, null, 12);

                        if (item.ProductionTime == null)
                            item.ProductionTime = 0;
                        _productionOrderDetail.ProductionTime = item.ProductionTime;


                        if (!string.IsNullOrWhiteSpace(variables.idUno))
                        {
                            item.ID_PickingItem = Convert.ToInt32(variables.idUno.Substring(1));


                            if (variables.idUno[0].ToString() == "M")
                            {
                                //macrovoce
                                MacroItem m = _quotationDataContext.MacroItems.SingleOrDefault(x => x.ID == item.ID_PickingItem);
                                item.RFlag = variables.idUno[0].ToString();
                                _productionOrderDetail.RFlag = item.RFlag;
                                item.SupplierCode = null;
                                item.UMRawMaterial = m.UM;
                            }
                            else
                            {
                                //prodotto singolo
                                PickingItem pi = _quotationDataContext.PickingItems.Single(x => x.ID == item.ID_PickingItem);
                                item.RFlag = null;
                                _productionOrderDetail.RFlag = null;
                                item.SupplierCode = pi.SupplierCode;
                                item.UMRawMaterial = pi.UM;
                            }
                        }
                        else
                        {
                            item.RFlag = null;
                            _productionOrderDetail.RFlag = null;
                            item.ID_PickingItem = null;
                            item.UMRawMaterial = null;
                        }
                        _productionOrderDetail.ID_PickingItem = item.ID_PickingItem;
                        _productionOrderDetail.SupplierCode = item.SupplierCode;
                        _productionOrderDetail.UMRawMaterial = item.UMRawMaterial;
                        _productionOrderDetail.RawMaterialQuantity = item.RawMaterialQuantity;


                        if (!string.IsNullOrWhiteSpace(variables.idDue))
                        {
                            //presente elemento selezionato: ne carica i dati
                            item.ID_PickingItemSup = Convert.ToInt32(variables.idDue.Substring(1));
                            if (variables.idDue[0].ToString() == "M")
                                item.SFlag = variables.idDue[0].ToString();
                            else
                                item.SFlag = null;

                            var _curPickingItemSup = UILabExtim.ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(item.ID_PickingItemSup, item.SFlag);
                            item.SupplierCodeSup = _curPickingItemSup.SupplierCode;
                            item.UMUser = _curPickingItemSup.UM;

                        }
                        else
                        {
                            item.ID_PickingItemSup = null;
                            item.UMUser = null;
                            item.SupplierCodeSup = null;
                            item.SFlag = null;
                        }
                        _productionOrderDetail.ID_PickingItemSup = item.ID_PickingItemSup;
                        _productionOrderDetail.SupplierCodeSup = item.SupplierCodeSup;
                        _productionOrderDetail.UMUser = item.UMUser;
                        _productionOrderDetail.SFlag = (item.SFlag == "M" ? item.SFlag : null);

                        _productionOrderDetail.RawMaterialX = item.RawMaterialX;
                        _productionOrderDetail.RawMaterialY = item.RawMaterialY;
                        _productionOrderDetail.RawMaterialZ = item.RawMaterialZ;

                        if (item.UMProduct == 0)
                            item.UMProduct = null;
                        _productionOrderDetail.UMProduct = item.UMProduct;

                        _productionOrderDetail.ProducedQuantity = item.ProducedQuantity;
                        _productionOrderDetail.QuantityOver = item.QuantityOver;
                        _productionOrderDetail.DirectSupply = item.DirectSupply;
                        _productionOrderDetail.Cost = item.Cost;
                        _productionOrderDetail.HistoricalCostPhase = item.CostCalcPhase;
                        _productionOrderDetail.HistoricalCostRawM = item.CostCalcRawM;
                        _productionOrderDetail.HistoricalCostSupM = item.CostCalcSupM;
                        _productionOrderDetail.Note = item.Note;
                        _productionOrderDetail.ProductionDate = DateTime.Parse(item.ProductionDate.Value.ToShortDateString());

                        _productionOrderDetail.FreeTypeCode = item.FreeTypeCode;
                        _productionOrderDetail.FreeItemTypeCode = item.FreeItemTypeCode;
                        _productionOrderDetail.FreeItemDescription = item.FreeItemDescription;

                        _productionOrderDetail.OkCopiesCount = item.OkCopiesCount;
                        _productionOrderDetail.KoCopiesCount = item.KoCopiesCount;
                        _productionOrderDetail.Special = item.Special ?? false;



                        _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
                        _quotationDataContext.SubmitChanges();

                        //imposta l'id del nuovo inserimento
                        item.ID = _productionOrderDetail.ID;

                        bool isFilmCaldo = false;
                        if (_productionOrderDetail.ID_PickingItemSup != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpPi != null)
                                    isFilmCaldo = (tmpPi.ItemTypeCode == 7);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpMi != null)
                                    isFilmCaldo = (tmpMi.ItemTypeCode == 7);
                            }
                        }

                        bool isFustellatura = false;
                        if (_productionOrderDetail.ID_Phase != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpPi != null)
                                    isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpMi != null)
                                    isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                            }
                        }
                        //if (_productionOrderDetail.ID_PickingItemSup != null)
                        //{
                        //    if (_productionOrderDetail.SFlag == null)
                        //    {
                        //        PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpPi != null)
                        //            isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                        //    }
                        //    else
                        //    {
                        //        MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpMi != null)
                        //            isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                        //    }
                        //}

                        ProductionOrderTechSpec specialSpecs = new ProductionOrderTechSpec();
                        if (_productionOrderDetail.Special == true)
                        {
                            specialSpecs.CodiceMarcaInchiostro = item.CodiceMarcaInchiostro;
                            specialSpecs.Ricetta = item.Ricetta ?? false;
                            specialSpecs.TelaioNumeroFili = item.TelaioNumeroFili;
                            specialSpecs.GelatinaSpessore = item.GelatinaSpessore;
                            specialSpecs.RaclaInclinazione = item.RaclaInclinazione;
                            specialSpecs.RaclaDurezzaSpigolo = item.RaclaDurezzaSpigolo;
                        }
                        else
                        {
                            specialSpecs.CodiceMarcaInchiostro = null;
                            specialSpecs.Ricetta = null;
                            specialSpecs.TelaioNumeroFili = null;
                            specialSpecs.GelatinaSpessore = null;
                            specialSpecs.RaclaInclinazione = null;
                            specialSpecs.RaclaDurezzaSpigolo = null;
                        }

                        if (isFilmCaldo)
                        {
                            specialSpecs.CodiceMarcaFilm = item.CodiceMarcaFilm;
                            specialSpecs.ClicheReso = item.ClicheReso;
                            specialSpecs.ClicheCondizioni = item.ClicheCondizioni;
                            specialSpecs.StampaTemperatura = item.StampaTemperatura;
                            specialSpecs.AltreInfo = item.AltreInfo;
                        }
                        else
                        {
                            specialSpecs.CodiceMarcaFilm = null;
                            specialSpecs.ClicheReso = null;
                            specialSpecs.ClicheCondizioni = null;
                            specialSpecs.StampaTemperatura = null;
                            specialSpecs.AltreInfo = null;
                        }

                        if (isFustellatura)
                        {
                            specialSpecs.FustellaResa = item.FustellaResa;
                            specialSpecs.FustellaCondizioni = item.FustellaCondizioni;
                            specialSpecs.ControCordonatori = item.ControCordonatori;
                        }
                        else
                        {
                            specialSpecs.FustellaResa = null;
                            specialSpecs.FustellaCondizioni = null;
                            specialSpecs.ControCordonatori = null;
                        }

                        specialSpecs.AltreNoteDaProduzione = item.AltreNoteDaProduzione;

                        if (_productionOrderDetail.Special == true || isFilmCaldo || isFustellatura || !string.IsNullOrEmpty(specialSpecs.AltreNoteDaProduzione))
                        {
                            specialSpecs.ID_ProductionOrder = _productionOrderDetail.ID_ProductionOrder;
                            specialSpecs.ID_ProductionOrderDetail = _productionOrderDetail.ID;
                            specialSpecs.ID_Owner = _productionOrderDetail.ID_Owner;
                            specialSpecs.ProductionDate = DateTime.Now;
                            specialSpecs.ID_Phase = _productionOrderDetail.ID_Phase;
                            specialSpecs.ID_QuotationDetail = _productionOrderDetail.ID_QuotationDetail;
                            specialSpecs.Status = 17;
                            _quotationDataContext.ProductionOrderTechSpecs.InsertOnSubmit(specialSpecs);
                        }
                        else
                        {
                            ProductionOrderTechSpec existing = _quotationDataContext.ProductionOrderTechSpecs.FirstOrDefault(d => d.ID_ProductionOrderDetail == _productionOrderDetail.ID);
                            if (existing != null)
                                _quotationDataContext.ProductionOrderTechSpecs.DeleteOnSubmit(existing);
                        }

                        if (variables._IDquotationDetail != 0)
                        {
                            VW_ProductionExtMP wmp = _quotationDataContext.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == item.ID_ProductionOrder && d.IDQuotationDetail == variables._IDquotationDetail);
                            ProductionOrderService.CloseProductionOrderSchedule(_quotationDataContext, item.ID_ProductionOrder, variables._IDquotationDetail);
                            // BOBST
                            if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                            {

                                try
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                                    WebResponse response = request.GetResponse();
                                    System.Threading.Thread.Sleep(2000);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                            {
                                try
                                {
                                    Snap7Gateway gw = new Snap7Gateway();
                                    gw.GetCurrentDataFromSilkFoil1(_quotationDataContext, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 77)
                            {
                                try
                                {
                                    using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                                    {
                                        EuroProgettiGateway.Close_EuroProgetti_DB_Ordine(_quotationDataContext, dbRem, wmp.IDProductionOrder.Value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina EUROPROGETTI - {0}", ex.Message));
                                }
                            }
                        }

                        _quotationDataContext.SubmitChanges();


                        variables._model = TempProductionOrderViewPartialSetModelList();

                        new UILabExtim.ProductionOrderDetailsInsertController().SubmitUnderlyingDetails(item, item.ProductionDate.Value, false);

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            variables.idUno = string.Empty;
            variables.idDue = string.Empty;
            return PartialView("_TempProductionOrderViewPartial", variables._model);

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartialEvadi(TempProductionOrderDetail item)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            using (var _quotationDataContext = new QuotationDataContext())
            {
                //_quotationDataContext.ProductionOrders.
            }

            return PartialView("_TempProductionOrderViewPartial", variables._model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartialUpdate_Prod(TempProductionOrderDetail item)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var _quotationDataContext = new QuotationDataContext())
                    {
                        //if (!string.IsNullOrWhiteSpace(variables.idUno))
                        //{
                        //    pckbase =
                        //        ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(
                        //            Convert.ToInt32(variables.idUno.Substring(1)), variables.idUno[0].ToString());
                        //    variables.idUno = string.Empty;
                        //}
                        //else
                        //{          
                        PickingItem pckbase = null;
                        if (item.ID_PickingItem != null)
                        {
                            pckbase =
                                    ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(
                                        item.ID_PickingItem, item.RFlag);
                        }

                        PickingItem pckSup = null;
                        if (item.ID_PickingItemSup != null && item.ID_PickingItemSup != 0)
                        {
                            pckSup = ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(item.ID_PickingItemSup, item.SFlag);
                        }

                        ProductionOrderDetail _productionOrderDetail;

                        _productionOrderDetail = ProductionOrderDetailsInsertController.GetProductionOrderDetail(_quotationDataContext,
                            item.ID);
                        if (_productionOrderDetail == null)
                        {
                            _productionOrderDetail = new ProductionOrderDetail();
                        }
                        else
                        {
                            _productionOrderDetail.ID = item.ID;
                        }
                        _productionOrderDetail.ID_ProductionOrder = item.ID_ProductionOrder;

                        _productionOrderDetail.ID_Owner = variables._idUser;
                        item.ID_Owner = _productionOrderDetail.ID_Owner;

                        //_productionOrderDetail.ID_Company = variables._currentCompanyId;
                        //item.ID_Company = variables._currentCompanyId;
                        // merge aziendale
                        int idCompany = ProductionOrderDetailsInsertController.GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, item);
                        _productionOrderDetail.ID_Company = idCompany;
                        item.ID_Company = idCompany;

                        if (item.ID_Phase == 0)
                            item.ID_Phase = null;
                        _productionOrderDetail.ID_Phase = item.ID_Phase;

                        //new UILabExtim.ProductionOrderDetailsInsertController().ChangeProductionOrderScheduleStatus(_quotationDataContext, item.ID_ProductionOrder, item.ID_Phase, null, 12);

                        if (item.ProductionTime == null)
                            item.ProductionTime = 0;
                        _productionOrderDetail.ProductionTime = item.ProductionTime;

                        if (pckbase != null && pckbase.ID == 0)
                        {
                            item.ID_PickingItem = null;
                            item.RFlag = null;
                            _productionOrderDetail.RFlag = null;
                        }
                        //if (item.ID_PickingItem == 0)
                        //{
                        //    item.ID_PickingItem = null;
                        //    item.RFlag = null;
                        //}
                        if (pckbase != null)
                            _productionOrderDetail.ID_PickingItem = pckbase.ID;
                        else
                        {
                            _productionOrderDetail.ID_PickingItem = null;
                        }


                        if (item.SupplierCode == 0)
                            item.SupplierCode = null;
                        _productionOrderDetail.SupplierCode = item.SupplierCode;

                        if (item.UMRawMaterial == 0 || pckbase == null)
                            item.UMRawMaterial = null;
                        else
                            item.UMRawMaterial = pckbase.UM;

                        _productionOrderDetail.UMRawMaterial = item.UMRawMaterial;

                        _productionOrderDetail.RawMaterialQuantity = item.RawMaterialQuantity;

                        if (item.UMUser == 0 || pckSup == null)
                            _productionOrderDetail.UMUser = null;
                        else
                        {
                            _productionOrderDetail.UMUser = pckSup.UM;
                        }


                        if (pckSup == null || pckSup.ID == 0)
                        {
                            item.ID_PickingItemSup = null;
                            item.SFlag = null;
                            _productionOrderDetail.ID_PickingItemSup = null;
                        }
                        else
                        {
                            _productionOrderDetail.ID_PickingItemSup = pckSup.ID;
                        }
                        item.UMUser = _productionOrderDetail.UMUser;
                        item.ID_PickingItemSup = _productionOrderDetail.ID_PickingItemSup;

                        _productionOrderDetail.SFlag = (item.SFlag == "M" ? item.SFlag : null);

                        if (pckSup != null)
                            item.SupplierCodeSup = pckSup.SupplierCode;
                        else
                            item.SupplierCodeSup = null;
                        _productionOrderDetail.SupplierCodeSup = item.SupplierCodeSup;

                        _productionOrderDetail.RawMaterialX = item.RawMaterialX;
                        _productionOrderDetail.RawMaterialY = item.RawMaterialY;
                        _productionOrderDetail.RawMaterialZ = item.RawMaterialZ;

                        if (item.UMProduct == 0)
                            item.UMProduct = null;
                        _productionOrderDetail.UMProduct = item.UMProduct;

                        _productionOrderDetail.ProducedQuantity = item.ProducedQuantity;
                        _productionOrderDetail.QuantityOver = item.QuantityOver;
                        _productionOrderDetail.DirectSupply = item.DirectSupply;
                        _productionOrderDetail.Cost = item.Cost;
                        _productionOrderDetail.HistoricalCostPhase = item.CostCalcPhase;
                        _productionOrderDetail.HistoricalCostRawM = item.CostCalcRawM;
                        _productionOrderDetail.HistoricalCostSupM = item.CostCalcSupM;
                        _productionOrderDetail.Note = item.Note;
                        _productionOrderDetail.ProductionDate = DateTime.Parse(item.ProductionDate.Value.ToShortDateString());

                        _productionOrderDetail.FreeTypeCode = item.FreeTypeCode;
                        _productionOrderDetail.FreeItemTypeCode = item.FreeItemTypeCode;
                        _productionOrderDetail.FreeItemDescription = item.FreeItemDescription;

                        _productionOrderDetail.OkCopiesCount = item.OkCopiesCount;
                        _productionOrderDetail.KoCopiesCount = item.KoCopiesCount;
                        _productionOrderDetail.Special = item.Special;

                        //ProductionOrderTechSpec existing = _quotationDataContext.ProductionOrderTechSpecs.FirstOrDefault(d =>
                        //         d.ID_ProductionOrder == _productionOrderDetail.ID_ProductionOrder && d.ID_Phase == _productionOrderDetail.ID_Phase);
                        ProductionOrderTechSpec existing = _quotationDataContext.ProductionOrderTechSpecs.FirstOrDefault(d =>
                                d.ID_ProductionOrderDetail == _productionOrderDetail.ID);

                        bool isNew = false;

                        bool isFilmCaldo = false;
                        if (_productionOrderDetail.ID_PickingItemSup != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpPi != null)
                                    isFilmCaldo = (tmpPi.ItemTypeCode == 7);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpMi != null)
                                    isFilmCaldo = (tmpMi.ItemTypeCode == 7);
                            }
                        }

                        bool isFustellatura = false;
                        if (_productionOrderDetail.ID_Phase != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpPi != null)
                                    isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpMi != null)
                                    isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                            }
                        }
                        //if (_productionOrderDetail.ID_PickingItemSup != null)
                        //{
                        //    if (_productionOrderDetail.SFlag == null)
                        //    {
                        //        PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpPi != null)
                        //            isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                        //    }
                        //    else
                        //    {
                        //        MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpMi != null)
                        //            isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                        //    }
                        //}

                        if (existing == null && (item.Special == true || isFilmCaldo || isFustellatura))
                        {
                            isNew = true;
                            existing = new ProductionOrderTechSpec();
                            existing.ID_ProductionOrder = _productionOrderDetail.ID_ProductionOrder;
                            existing.ID_ProductionOrderDetail = _productionOrderDetail.ID;
                            existing.ID_Owner = _productionOrderDetail.ID_Owner;
                            existing.ProductionDate = DateTime.Now;
                            existing.ID_Phase = _productionOrderDetail.ID_Phase;
                            existing.ID_QuotationDetail = _productionOrderDetail.ID_QuotationDetail;

                        }

                        if (item.Special == true)
                        {
                            existing.CodiceMarcaInchiostro = item.CodiceMarcaInchiostro;
                            existing.Ricetta = item.Ricetta ?? false;
                            existing.TelaioNumeroFili = item.TelaioNumeroFili;
                            existing.GelatinaSpessore = item.GelatinaSpessore;
                            existing.RaclaInclinazione = item.RaclaInclinazione;
                            existing.RaclaDurezzaSpigolo = item.RaclaDurezzaSpigolo;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.CodiceMarcaInchiostro = null;
                                existing.Ricetta = null;
                                existing.TelaioNumeroFili = null;
                                existing.GelatinaSpessore = null;
                                existing.RaclaInclinazione = null;
                                existing.RaclaDurezzaSpigolo = null;
                            }
                        }
                        if (isFilmCaldo)
                        {
                            existing.CodiceMarcaFilm = item.CodiceMarcaFilm;
                            existing.ClicheReso = item.ClicheReso;
                            existing.ClicheCondizioni = item.ClicheCondizioni;
                            existing.StampaTemperatura = item.StampaTemperatura;
                            existing.AltreInfo = item.AltreInfo;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.CodiceMarcaFilm = null;
                                existing.ClicheReso = null;
                                existing.ClicheCondizioni = null;
                                existing.StampaTemperatura = null;
                                existing.AltreInfo = null;
                            }
                        }
                        if (isFustellatura)
                        {
                            existing.FustellaResa = item.FustellaResa;
                            existing.FustellaCondizioni = item.FustellaCondizioni;
                            existing.ControCordonatori = item.ControCordonatori;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.FustellaResa = null;
                                existing.FustellaCondizioni = null;
                                existing.ControCordonatori = null;
                            }
                        }

                        if (existing != null)
                        {
                            existing.AltreNoteDaProduzione = item.AltreNoteDaProduzione;
                        }

                        if (isNew && (item.Special == true || isFilmCaldo || isFustellatura || !string.IsNullOrEmpty(existing.AltreNoteDaProduzione)))
                        {
                            _quotationDataContext.ProductionOrderTechSpecs.InsertOnSubmit(existing);
                        }

                        if (existing != null && (item.Special == false && isFilmCaldo == false && isFustellatura == false && string.IsNullOrEmpty(existing.AltreNoteDaProduzione)))
                        {
                            _quotationDataContext.ProductionOrderTechSpecs.DeleteOnSubmit(existing);
                        }

                        if (variables._IDquotationDetail != 0)
                        {
                            VW_ProductionExtMP wmp = _quotationDataContext.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == item.ID_ProductionOrder && d.IDQuotationDetail == variables._IDquotationDetail);
                            ProductionOrderService.CloseProductionOrderSchedule(_quotationDataContext, item.ID_ProductionOrder, variables._IDquotationDetail);
                            // BOBST
                            if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                            {

                                try
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                                    WebResponse response = request.GetResponse();
                                    System.Threading.Thread.Sleep(2000);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                            {
                                try
                                {
                                    Snap7Gateway gw = new Snap7Gateway();
                                    gw.GetCurrentDataFromSilkFoil1(_quotationDataContext, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 77)
                            {
                                try
                                {
                                    using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                                    {
                                        EuroProgettiGateway.Close_EuroProgetti_DB_Ordine(_quotationDataContext, dbRem, wmp.IDProductionOrder.Value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina EUROPROGETTI - {0}", ex.Message));
                                }
                            }
                        }

                        _quotationDataContext.SubmitChanges();

                        variables._model = TempProductionOrderViewPartialSetModelList();

                        new UILabExtim.ProductionOrderDetailsInsertController().SubmitUnderlyingDetails(item, item.ProductionDate.Value, false);





                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TempProductionOrderViewPartial", variables._model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartialUpdate(TempProductionOrderDetail item)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            if (ModelState.IsValid)
            {
                try
                {
                    using (var _quotationDataContext = new QuotationDataContext())
                    {

                        ProductionOrderDetail _productionOrderDetail;

                        _productionOrderDetail = ProductionOrderDetailsInsertController.GetProductionOrderDetail(_quotationDataContext,
                            item.ID);
                        if (_productionOrderDetail == null)
                        {
                            _productionOrderDetail = new ProductionOrderDetail();
                        }
                        else
                        {
                            _productionOrderDetail.ID = item.ID;
                        }
                        _productionOrderDetail.ID_ProductionOrder = item.ID_ProductionOrder;

                        _productionOrderDetail.ID_Owner = variables._idUser;
                        item.ID_Owner = _productionOrderDetail.ID_Owner;

                        //_productionOrderDetail.ID_Company = variables._currentCompanyId;
                        //item.ID_Company = variables._currentCompanyId;
                        // merge aziendale
                        int idCompany = ProductionOrderDetailsInsertController.GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, item);
                        _productionOrderDetail.ID_Company = idCompany;
                        item.ID_Company = idCompany;

                        if (item.ID_Phase == 0)
                            item.ID_Phase = null;
                        _productionOrderDetail.ID_Phase = item.ID_Phase;

                        //new UILabExtim.ProductionOrderDetailsInsertController().ChangeProductionOrderScheduleStatus(_quotationDataContext, item.ID_ProductionOrder, item.ID_Phase, null, 12);

                        if (item.ProductionTime == null)
                            item.ProductionTime = 0;
                        _productionOrderDetail.ProductionTime = item.ProductionTime;


                        PickingItem pckbase = null;
                        if (!string.IsNullOrWhiteSpace(variables.idUno))
                        {
                            _productionOrderDetail.ID_PickingItem = Convert.ToInt32(variables.idUno.Substring(1));


                            if (variables.idUno[0].ToString() == "M")
                            {
                                //macrovoce
                                MacroItem m = _quotationDataContext.MacroItems.SingleOrDefault(x => x.ID == _productionOrderDetail.ID_PickingItem);
                                _productionOrderDetail.RFlag = variables.idUno[0].ToString();
                                _productionOrderDetail.SupplierCode = null;
                                _productionOrderDetail.UMRawMaterial = m.UM;
                            }
                            else
                            {
                                //prodotto singolo
                                PickingItem pi = _quotationDataContext.PickingItems.Single(x => x.ID == _productionOrderDetail.ID_PickingItem);
                                _productionOrderDetail.RFlag = null;
                                _productionOrderDetail.SupplierCode = pi.SupplierCode;
                                _productionOrderDetail.UMRawMaterial = pi.UM;
                            }
                        }
                        else
                        {
                            if (item.ID_PickingItem != null)
                            {
                                pckbase =
                                        ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(
                                            item.ID_PickingItem, item.RFlag);
                                _productionOrderDetail.RFlag = item.RFlag;
                                _productionOrderDetail.ID_PickingItem = pckbase.ID;
                                _productionOrderDetail.UMRawMaterial = pckbase.UM;

                            }
                            else
                            {
                                _productionOrderDetail.RFlag = null;
                                _productionOrderDetail.ID_PickingItem = null;
                                _productionOrderDetail.UMRawMaterial = null;
                            }

                        }



                        PickingItem pckSup = null;
                        if (!string.IsNullOrWhiteSpace(variables.idDue))
                        {
                            //presente elemento selezionato: ne carica i dati
                            _productionOrderDetail.ID_PickingItemSup = Convert.ToInt32(variables.idDue.Substring(1));
                            if (variables.idDue[0].ToString() == "M")
                                _productionOrderDetail.SFlag = variables.idDue[0].ToString();
                            else
                                item.SFlag = null;

                            var _curPickingItemSup = UILabExtim.ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(item.ID_PickingItemSup, item.SFlag);
                            _productionOrderDetail.SupplierCodeSup = _curPickingItemSup.SupplierCode;
                            _productionOrderDetail.UMUser = _curPickingItemSup.UM;


                        }
                        else
                        {
                            if (item.ID_PickingItemSup != null && item.ID_PickingItemSup != 0)
                            {
                                pckSup = ProductionOrderDetailsInsertController.GetPickingItemFromMacroOrPickingItem(item.ID_PickingItemSup, item.SFlag);

                                _productionOrderDetail.ID_PickingItemSup = pckSup.ID;
                                _productionOrderDetail.UMUser = pckSup.UM;
                                _productionOrderDetail.SupplierCodeSup = pckSup.SupplierCode;
                                _productionOrderDetail.SFlag = (item.SFlag == "M" ? item.SFlag : null);

                            }
                            else
                            {
                                _productionOrderDetail.ID_PickingItemSup = null;
                                _productionOrderDetail.UMUser = null;
                                _productionOrderDetail.SupplierCodeSup = null;
                                _productionOrderDetail.SFlag = null;
                            }
                        }





                        //if (pckbase != null && pckbase.ID == 0)
                        //{
                        //    item.ID_PickingItem = null;
                        //    item.RFlag = null;
                        //    _productionOrderDetail.RFlag = null;
                        //}
                        //if (item.ID_PickingItem == 0)
                        //{
                        //    item.ID_PickingItem = null;
                        //    item.RFlag = null;
                        //}
                        //if (pckbase != null)
                        //    _productionOrderDetail.ID_PickingItem = pckbase.ID;
                        //else
                        //{
                        //    _productionOrderDetail.ID_PickingItem = null;
                        //}


                        //if (item.SupplierCode == 0)
                        //    item.SupplierCode = null;
                        //_productionOrderDetail.SupplierCode = item.SupplierCode;

                        if (_productionOrderDetail.UMRawMaterial == 0 || _productionOrderDetail.ID_PickingItem == null)
                            _productionOrderDetail.UMRawMaterial = null;
                        //else
                        //    item.UMRawMaterial = pckbase.UM;

                        if (_productionOrderDetail.UMUser == 0 || _productionOrderDetail.ID_PickingItemSup == null)
                            _productionOrderDetail.UMUser = null;
                        //else
                        //{
                        //    _productionOrderDetail.UMUser = pckSup.UM;
                        //}


                        //if (pckSup == null || pckSup.ID == 0)
                        //{
                        //    item.ID_PickingItemSup = null;
                        //    item.SFlag = null;
                        //    _productionOrderDetail.ID_PickingItemSup = null;
                        //}
                        //else
                        //{
                        //    _productionOrderDetail.ID_PickingItemSup = pckSup.ID;
                        //}
                        //item.UMUser = _productionOrderDetail.UMUser;
                        //item.ID_PickingItemSup = _productionOrderDetail.ID_PickingItemSup;

                        //_productionOrderDetail.SFlag = (item.SFlag == "M" ? item.SFlag : null);

                        //if (pckSup != null)
                        //    item.SupplierCodeSup = pckSup.SupplierCode;
                        //else
                        //    item.SupplierCodeSup = null;
                        //_productionOrderDetail.SupplierCodeSup = item.SupplierCodeSup;


                        _productionOrderDetail.RawMaterialQuantity = item.RawMaterialQuantity;
                        //if (item.UMRawMaterial == 0)
                        //    item.UMRawMaterial = null;
                        //_productionOrderDetail.UMRawMaterial = item.UMRawMaterial;


                        _productionOrderDetail.RawMaterialX = item.RawMaterialX;
                        _productionOrderDetail.RawMaterialY = item.RawMaterialY;
                        _productionOrderDetail.RawMaterialZ = item.RawMaterialZ;
                        //if (item.UMProduct == 0)
                        //    item.UMProduct = null;
                        //_productionOrderDetail.UMProduct = item.UMProduct;

                        _productionOrderDetail.ProducedQuantity = item.ProducedQuantity;
                        _productionOrderDetail.QuantityOver = item.QuantityOver;
                        _productionOrderDetail.DirectSupply = item.DirectSupply;
                        _productionOrderDetail.Cost = item.Cost;
                        _productionOrderDetail.HistoricalCostPhase = item.CostCalcPhase;
                        _productionOrderDetail.HistoricalCostRawM = item.CostCalcRawM;
                        _productionOrderDetail.HistoricalCostSupM = item.CostCalcSupM;
                        _productionOrderDetail.Note = item.Note;
                        _productionOrderDetail.ProductionDate = DateTime.Parse(item.ProductionDate.Value.ToShortDateString());

                        _productionOrderDetail.FreeTypeCode = item.FreeTypeCode;
                        _productionOrderDetail.FreeItemTypeCode = item.FreeItemTypeCode;
                        _productionOrderDetail.FreeItemDescription = item.FreeItemDescription;

                        _productionOrderDetail.OkCopiesCount = item.OkCopiesCount;
                        _productionOrderDetail.KoCopiesCount = item.KoCopiesCount;
                        _productionOrderDetail.Special = item.Special;

                        //ProductionOrderTechSpec existing = _quotationDataContext.ProductionOrderTechSpecs.FirstOrDefault(d =>
                        //         d.ID_ProductionOrder == _productionOrderDetail.ID_ProductionOrder && d.ID_Phase == _productionOrderDetail.ID_Phase);
                        ProductionOrderTechSpec existing = _quotationDataContext.ProductionOrderTechSpecs.FirstOrDefault(d =>
                                d.ID_ProductionOrderDetail == _productionOrderDetail.ID);

                        bool isNew = false;

                        bool isFilmCaldo = false;
                        if (_productionOrderDetail.ID_PickingItemSup != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpPi != null)
                                    isFilmCaldo = (tmpPi.ItemTypeCode == 7);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                                if (tmpMi != null)
                                    isFilmCaldo = (tmpMi.ItemTypeCode == 7);
                            }
                        }

                        bool isFustellatura = false;
                        if (_productionOrderDetail.ID_Phase != null)
                        {
                            if (_productionOrderDetail.SFlag == null)
                            {
                                PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpPi != null)
                                    isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                            }
                            else
                            {
                                MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_Phase);
                                if (tmpMi != null)
                                    isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                            }
                        }
                        //if (_productionOrderDetail.ID_PickingItemSup != null)
                        //{
                        //    if (_productionOrderDetail.SFlag == null)
                        //    {
                        //        PickingItem tmpPi = _quotationDataContext.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpPi != null)
                        //            isFustellatura = (tmpPi.TypeCode == 31 && tmpPi.ItemTypeCode == 31);
                        //    }
                        //    else
                        //    {
                        //        MacroItem tmpMi = _quotationDataContext.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup);
                        //        if (tmpMi != null)
                        //            isFustellatura = (tmpMi.TypeCode == 31 && tmpMi.ItemTypeCode == 31);
                        //    }
                        //}

                        if (existing == null && (item.Special == true || isFilmCaldo || isFustellatura))
                        {
                            isNew = true;
                            existing = new ProductionOrderTechSpec();
                            existing.ID_ProductionOrder = _productionOrderDetail.ID_ProductionOrder;
                            existing.ID_ProductionOrderDetail = _productionOrderDetail.ID;
                            existing.ID_Owner = _productionOrderDetail.ID_Owner;
                            existing.ProductionDate = DateTime.Now;
                            existing.ID_Phase = _productionOrderDetail.ID_Phase;
                            existing.ID_QuotationDetail = _productionOrderDetail.ID_QuotationDetail;

                        }

                        if (item.Special == true)
                        {
                            existing.CodiceMarcaInchiostro = item.CodiceMarcaInchiostro;
                            existing.Ricetta = item.Ricetta ?? false;
                            existing.TelaioNumeroFili = item.TelaioNumeroFili;
                            existing.GelatinaSpessore = item.GelatinaSpessore;
                            existing.RaclaInclinazione = item.RaclaInclinazione;
                            existing.RaclaDurezzaSpigolo = item.RaclaDurezzaSpigolo;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.CodiceMarcaInchiostro = null;
                                existing.Ricetta = null;
                                existing.TelaioNumeroFili = null;
                                existing.GelatinaSpessore = null;
                                existing.RaclaInclinazione = null;
                                existing.RaclaDurezzaSpigolo = null;
                            }
                        }
                        if (isFilmCaldo)
                        {
                            existing.CodiceMarcaFilm = item.CodiceMarcaFilm;
                            existing.ClicheReso = item.ClicheReso;
                            existing.ClicheCondizioni = item.ClicheCondizioni;
                            existing.StampaTemperatura = item.StampaTemperatura;
                            existing.AltreInfo = item.AltreInfo;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.CodiceMarcaFilm = null;
                                existing.ClicheReso = null;
                                existing.ClicheCondizioni = null;
                                existing.StampaTemperatura = null;
                                existing.AltreInfo = null;
                            }
                        }
                        if (isFustellatura)
                        {
                            existing.FustellaResa = item.FustellaResa;
                            existing.FustellaCondizioni = item.FustellaCondizioni;
                            existing.ControCordonatori = item.ControCordonatori;
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.FustellaResa = null;
                                existing.FustellaCondizioni = null;
                                existing.ControCordonatori = null;
                            }
                        }

                        if (existing != null)
                        {
                            existing.AltreNoteDaProduzione = item.AltreNoteDaProduzione;
                        }

                        if (isNew && (item.Special == true || isFilmCaldo || isFustellatura || !string.IsNullOrEmpty(existing.AltreNoteDaProduzione)))
                        {
                            _quotationDataContext.ProductionOrderTechSpecs.InsertOnSubmit(existing);
                        }

                        if (existing != null && (item.Special == false && isFilmCaldo == false && isFustellatura == false && string.IsNullOrEmpty(existing.AltreNoteDaProduzione)))
                        {
                            _quotationDataContext.ProductionOrderTechSpecs.DeleteOnSubmit(existing);
                        }

                        if (variables._IDquotationDetail != 0)
                        {
                            VW_ProductionExtMP wmp = _quotationDataContext.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == item.ID_ProductionOrder && d.IDQuotationDetail == variables._IDquotationDetail);
                            ProductionOrderService.CloseProductionOrderSchedule(_quotationDataContext, item.ID_ProductionOrder, variables._IDquotationDetail);
                            // BOBST
                            if (wmp.IDProductionMachine == 30 || wmp.IDProductionMachine == 31)
                            {

                                try
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/mrpweb?cmd=stop", ConfigurationManager.AppSettings["BOBST1_IPAddressAndPort"]));
                                    WebResponse response = request.GetResponse();
                                    System.Threading.Thread.Sleep(2000);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina BOBST - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 104)
                            {
                                try
                                {
                                    Snap7Gateway gw = new Snap7Gateway();
                                    gw.GetCurrentDataFromSilkFoil1(_quotationDataContext, wmp.IDProductionOrder.Value, Convert.ToInt32(wmp.Quantity), wmp.poDescription);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina SILKFOIL - {0}", ex.Message));
                                }

                            }
                            if (wmp.IDProductionMachine.GetValueOrDefault() == 77)
                            {
                                try
                                {
                                    using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                                    {
                                        EuroProgettiGateway.Close_EuroProgetti_DB_Ordine(_quotationDataContext, dbRem, wmp.IDProductionOrder.Value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteMessage(string.Format("Impossibile comunicare con la macchina EUROPROGETTI - {0}", ex.Message));
                                }
                            }
                        }

                        _quotationDataContext.SubmitChanges();

                        variables._model = TempProductionOrderViewPartialSetModelList();

                        new UILabExtim.ProductionOrderDetailsInsertController().SubmitUnderlyingDetails(item, item.ProductionDate.Value, false);





                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_TempProductionOrderViewPartial", variables._model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult TempProductionOrderViewPartialDelete(Int32 ID)
        {
            TempProductionOrderDetailControllerSessionVariables variables = new TempProductionOrderDetailControllerSessionVariables();
            ViewBag.IsNew = false;
            if (ID >= 0)
            {
                try
                {
                    TempProductionOrderDetail pod = variables._model.Single(x => x.ID == ID);
                    new UILabExtim.ProductionOrderDetailsInsertController().DeleteProductionOrderDetail(pod);
                    variables._model.Remove(pod);
                    variables._model = TempProductionOrderViewPartialSetModelList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_TempProductionOrderViewPartial", variables._model);
        }




    }
}