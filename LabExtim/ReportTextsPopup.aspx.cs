using System;
using System.Linq;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ReportTextsPopup : QuotationController
    {
        //protected MetaTable table;


        public int ReferenceParameter
        {
            get
            {
                object temp = Request.QueryString["ID"];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        public int TypeParameter
        {
            get
            {
                object temp = Request.QueryString["RType"];
                return temp == null ? -1 : Convert.ToInt32(temp);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ReferenceParameter == -1)
                {
                    lblItemNo.Text = "STANDARD";
                }
                else
                {
                    if (TypeParameter == 1)
                        lblItemNo.Text =
                            new QuotationDataContext().Customers.SingleOrDefault(c => c.Code == ReferenceParameter).Name;
                }

                ddlReportTypes.DataSource = Global.ReportTypes;
                ddlReportTypes.DataBind();
                ddlReportTypes.SelectedValue = TypeParameter != -1 ? TypeParameter.ToString() : "1";
                ddlReportTypes.Enabled = TypeParameter == -1;
            }
            rcsReportTexts.IDReference = ReferenceParameter;
            rcsReportTexts.ReportTypeCode = Convert.ToInt32(ddlReportTypes.SelectedValue);
        }
    }
}