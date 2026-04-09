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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
            {
                int idUser = -1;
                using (var _qc = new QuotationDataContext())
                {
                    Token found = _qc.Tokens.FirstOrDefault(d => d.IdToken == Request.QueryString["tkn"]);
                    if (found != null)
                    {
                        Uri uri = new Uri(found.RedirectUrl);

                        if (uri.AbsoluteUri.ToLower().Contains("LeaveRequestsConsole".ToLower()))
                        {
                            int idLeaveRequest = Convert.ToInt32(HttpUtility.ParseQueryString(uri.Query).Get("toauthid"));
                            new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(20, idLeaveRequest, found.IdUser);
                            lblMessage.Text = string.Format("Richiesta permesso No. {0} autorizzata con successo!", idLeaveRequest);
                        }
                        _qc.Tokens.DeleteOnSubmit(found);
                        _qc.SubmitChanges();
                    }
                    else
                    {
                        lblMessage.Text = "Autoautenticazione fallita!";
                    }
                }


            }

        }
    }
}