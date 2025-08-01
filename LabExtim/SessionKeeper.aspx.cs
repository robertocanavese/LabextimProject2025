using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim
{
    public partial class SessionKeeper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SessionKeeper"] != null)
                UltimoCaricamento.InnerText = Session["SessionKeeper"].ToString();
            else
                UltimoCaricamento.InnerText = "Mai";
            Session["SessionKeeper"] = DateTime.Now;

        }
    }
}