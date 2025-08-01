using System;
using System.Web.Security;
using System.Web.UI;
using DLLabExtim;

namespace LabExtim
{
    public partial class PageTemplates : MasterPage
    {

        public Boolean UpdateProgressVisible
        {
            get { return UpdateProgress1.Visible; }
            set { UpdateProgress1.Visible = value; }
        }

 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                lbtLogOut.Visible = false;
            }
            else
                setupClientScript();
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