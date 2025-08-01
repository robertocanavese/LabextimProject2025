using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class _HomeTablet : BaseController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
            ((HtmlControl)((Site)Master).FindControl("divMain")).Style.Clear();
            //System.Collections.IList _options = new List<LinkOption>();

            //_options.Add(new LinkOption(1, "Nuovo preventivo", "~/QuotationInsert.aspx"));
            //_options.Add(new LinkOption(2, "Trova preventivo", "~/QuotationFind.aspx"));
            //_options.Add(new LinkOption(3, "Nuovo modello di preventivo", "~/QuotationTemplateInsert.aspx"));
            //_options.Add(new LinkOption(4, "Trova modello di preventivo", "~/QuotationTemplateFind.aspx"));
            //_options.Add(new LinkOption(5, "Gestione tabella base", "~/PickingItemsConsole.aspx"));
            //_options.Add(new LinkOption(6, "Gestione tabelle di supporto", "~/TablesMan.aspx"));
            ////_options.Add(new LinkOption(5, "Importazione tabelle", "~/TablesImport.aspx"));


            //if (_options.Count == 0)
            //{
            //    throw new InvalidOperationException("Non ci sono opzioni disponibili.");
            //}
            //Menu1.DataSource = _options;
            //Menu1.DataBind();
            if (!IsPostBack)
            {
                LoadTreeView();
                GlobalConfiguration = GetConfiguration();
            }
            //tvwMainMenu.Nodes[0].Selected = true;
        }

        protected void LoadTreeView()
        {

            tvwMainMenu.Nodes.Add(new TreeNode("Pianificazione attività"));

            tvwMainMenu.Nodes[tvwMainMenu.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Calendario lavorazioni di reparto", string.Empty, string.Empty,
                "~/ProductionOrdersDepSched.aspx", string.Empty));

            tvwMainMenu.Nodes[tvwMainMenu.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Schedulatore grafico lavorazioni", string.Empty, string.Empty,
              "~/ProductionGraphicScheduler.aspx", string.Empty));


        }


    }
}