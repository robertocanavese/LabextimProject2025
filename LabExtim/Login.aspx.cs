using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DLLabExtim;
using System.Configuration;
using CMLabExtim;

namespace LabExtim
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((HtmlControl) ((Site) Master).FindControl("divMain")).Style.Clear();
            Login1.DestinationPageUrl = (IsTablet(Request.GetIPAddress()) ? "~/HomeTablet.aspx" : "~/Home.aspx");
            Login1.Focus();
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            //Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Session["LabextimUser"] = null;
            Session["SessionTimeStamp"] = Session.SessionID;
            var _result = new QuotationService().SetSessionLock("", Session["SessionTimeStamp"].ToString());
        }
        public static bool IsTablet(string ip)
        {
            System.Net.IPAddress incomingIp = System.Net.IPAddress.Parse(ip);

            foreach (var subnet in ConfigurationManager.AppSettings["Tablets_IPAddresses"].ToString().Split('|'))
            {
                System.Net.IPNetwork network = System.Net.IPNetwork.Parse(subnet);
                if (network.Contains(incomingIp))
                    return true;
            }

            return false;
        }

        


    }
}