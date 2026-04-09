using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UILabExtim;

namespace LabExtim
{
    public partial class AutoAuth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["tkn"]))
            {



                new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(20, Convert.ToInt32(e.CommandArgument), WebUser.Employee.ID);

            }

        }
    }
}