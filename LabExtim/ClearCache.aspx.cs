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
    public partial class ClearCache : BaseController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {

                foreach (Company cp in db.Companies)
                {
                    Cache.Remove(cp.ID + "|" + MenuType.MenuPickingItems.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperations.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuOperationNoPhases.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuMaterials.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuProdRecord.ToString());
                    Cache.Remove(cp.ID + "|" + MenuType.MenuQuotationTemplates.ToString());
                }
            }
            Cache.Remove("PickingItems");
            Cache.Remove("MacroItems");
        }
    }
}