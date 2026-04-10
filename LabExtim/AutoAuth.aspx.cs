using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class AutoAuth : System.Web.UI.Page
    {
        Token Found;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
                {
                    int idUser = -1;
                    using (var _qc = new QuotationDataContext())
                    {
                        Found = _qc.Tokens.FirstOrDefault(d => d.IdToken == Request.QueryString["tkn"]);
                        if (Found != null)
                        {
                            Uri uri = new Uri(Found.RedirectUrl);
                            pnlLeaveRequestsConsole.Visible = (uri.AbsoluteUri.ToLower().Contains("LeaveRequestsConsole".ToLower()));
                            int idLeaveRequest = Convert.ToInt32(HttpUtility.ParseQueryString(uri.Query).Get("toauthid"));
                            LeaveRequest leaveRequest = _qc.LeaveRequests.FirstOrDefault(d => d.ID == idLeaveRequest);
                            lblRequest.Text =
                                string.Format(
                             "<table>" +
                             "<thead><tr><th><b>DETTAGLIO RICHIESTA:</b></th></tr></thead><tbody>" +
                             "<tr><td>Id richiesta:<b>{0}</b></td></tr>" +
                             "<tr><td>Azienda:<b>{1}</b></td></tr>" +
                             "<tr><td>Richiedente:<b>{2}</b></td></tr>" +
                             "<tr><td>Data richiesta:<b>{3}</b></td></tr>" +
                             "<tr><td>Tipo permesso:<b>{4}</b></td></tr>" +
                             "<tr><td>Data inizio assenza:<b>{5}</b></td></tr>" +
                             "<tr><td>Data fine assenza:<b>{6}</b></td></tr>" +
                             "<tr><td>Orario:<b>{7}</b></td></tr>" +
                             "<tr><td>Giorni di assenza:<b>{8}</b></td></tr>" +
                             "<tr><td>Responsabile:<b>{9}</b></td></tr>" +
                             "<tr><td>Messaggio a responsabile:<b>{10}</b></td></tr>" +
                             "<tr><td>Stato richiesta:<b>{11}</b></td></tr>" +
                             "<tr><td>Aggiornamento:<b>{12}</b></td></tr>" +
                             "</tbody><table>",
                             leaveRequest.ID,
                             leaveRequest.Employee.Company.Description,
                             leaveRequest.Employee.Name + " " + leaveRequest.Employee.Surname,
                             leaveRequest.RequestDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm"),
                             leaveRequest.LeaveType1.Description,
                             leaveRequest.StartDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                             leaveRequest.EndDate.GetValueOrDefault().ToString("dd/MM/yyyy"),
                             leaveRequest.DayFraction1.Description,
                             leaveRequest.VacationDays,
                             leaveRequest.Employee1.Name + " " + leaveRequest.Employee1.Surname,
                             leaveRequest.MessageToManager,
                             leaveRequest.Statuse.Description,
                             leaveRequest.StatusDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm")
                             );
                        }
                        else
                        {
                            lblMessage.Text = "Autoautenticazione fallita!";
                        }
                    }


                }
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            using (var _qc = new QuotationDataContext())
            {
                Found = _qc.Tokens.FirstOrDefault(d => d.IdToken == Request.QueryString["tkn"]);
                Uri uri = new Uri(Found.RedirectUrl);
                int idLeaveRequest = Convert.ToInt32(HttpUtility.ParseQueryString(uri.Query).Get("toauthid"));
                new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(_qc, 20, idLeaveRequest, Found.IdUser, txtmessageToApplicant.Text);
                lblMessage.Text = string.Format("Richiesta permesso No. {0} autorizzata con successo!", idLeaveRequest);
                _qc.Tokens.DeleteOnSubmit(Found);
                _qc.SubmitChanges();
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            using (var _qc = new QuotationDataContext())
            {
                Found = _qc.Tokens.FirstOrDefault(d => d.IdToken == Request.QueryString["tkn"]);
                Uri uri = new Uri(Found.RedirectUrl);
                int idLeaveRequest = Convert.ToInt32(HttpUtility.ParseQueryString(uri.Query).Get("toauthid"));
                new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(_qc, 21, idLeaveRequest, Found.IdUser, txtmessageToApplicant.Text);
                lblMessage.Text = string.Format("Richiesta permesso No. {0} respinta con successo!", idLeaveRequest);
                _qc.Tokens.DeleteOnSubmit(Found);
                _qc.SubmitChanges();
            }
        }
    }
}