using System;
using System.Collections;
using System.Web.DynamicData;
using System.Web.UI;
using UILabExtim;

namespace LabExtim
{
    public partial class _TablesMan : BaseController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList visibleTables = MetaModel.Default.VisibleTables;
            // Hashtable _hashtable = new Hashtable();
            // List<MetaTable> visibleTables = MetaModel.Default.VisibleTables;

            //foreach (MetaTable _metatable in visibleTables)
            //    switch (_metatable.DisplayName)
            //    {
            //        case "Customers":
            //            _hashtable.Add("Clienti", _metatable);
            //            break;
            //        case "Quotations":
            //            _hashtable.Add("Preventivi", _metatable);
            //            break;
            //        case "QuotationDetails":
            //            _hashtable.Add("Dettagli Preventivi", _metatable);
            //            break;
            //        case "Suppliers":
            //            _hashtable.Add("Fornitori", _metatable);
            //            break;
            //        default:
            //            _hashtable.Add("Non disponibile", _metatable);
            //            break;
            //    }

            if (visibleTables.Count == 0)
            {
                throw new InvalidOperationException(
                    "There are no accessible tables. Make sure that at least one data model is registered in Global.asax and scaffolding is enabled or implement custom pages.");
            }
            Menu1.DataSource = visibleTables;
            // Menu1.DataSource = _hashtable;
            Menu1.DataBind();
        }
    }
}