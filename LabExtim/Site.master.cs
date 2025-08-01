using System;
using System.Web.Security;
using System.Web.UI;
using DLLabExtim;
using UILabExtim;
using CMLabExtim;

namespace LabExtim
{
    public partial class Site : MasterPage
    {

        public Boolean MenuBarVisible
        {
            get { return tblHeader.Visible; }
            set { tblHeader.Visible = value; }
        }

        public Boolean UpdateProgressVisible
        {
            get { return UpdateProgress1.Visible; }
            set { UpdateProgress1.Visible = value; }
        }


        public string CurrentEmployeeName
        {
            get
            {
                try
                {
                    if (this.Page is BaseController)
                    {
                        LabextimUser curUser = ((BaseController)this.Page).WebUser;
                        return string.Format("Azienda: {0} - Utente: {1} {2}", curUser.Employee.Company.Description, curUser.Employee.Name, curUser.Employee.Surname);
                    }
                    else
                        return "(utente anonimo)";
                }
                catch
                {
                    return "(utente anonimo)";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                lbtLogOut.Visible = false;

            }
            else
            {
                setupClientScript();
                lblUserName.Text = CurrentEmployeeName;
                if (!IsPostBack)
                {
                    if (!this.Page.Request.Url.AbsolutePath.ToLowerInvariant().Contains("login.aspx"))
                    {
                        if (((BaseController)this.Page).IsTablet(Request.GetIPAddress()))
                        {
                            smpSite.SiteMapProvider = "Tablet";
                        }
                        else
                        {
                            smpSite.SiteMapProvider = "Workstation";
                        }
                    }
                }
            }



        }

        private void setupClientScript()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var csm = Page.ClientScript;
                if (!csm.IsClientScriptIncludeRegistered("Site"))
                {
                    csm.RegisterClientScriptInclude(Page.GetType(), "Site", "Site.js");
                }
            }
        }

        protected void lbtLogOut_Click(object sender, EventArgs e)
        {
            //Session.Abandon();
            var _result = new QuotationService().DelAllUserLocks(Membership.GetUser().ProviderUserKey.ToString(),
                Session["SessionTimeStamp"].ToString());
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}