using System;
using System.Linq;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class PODetailsNotes : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblItemNo.Text = POIdParameter == -1 ? " [Nuovo]" : " No " + POIdParameter;
                if (POIdParameter != -1)
                {
                    using (var _ctx = new QuotationDataContext())
                    {
                        grdProductionOrderDetails.DataSource =
                            _ctx.prc_LAB_MGet_LAB_NoteByPhaseAndPOID(POIdParameter).ToList();
                        grdProductionOrderDetails.DataBind();
                    }
                }
            }
        }
    }
}