using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class _Home : BaseController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
            ((HtmlControl)((Site)Master).FindControl("divMain")).Style.Clear();

            if (!IsPostBack)
            {
                LoadTreeView();
                GlobalConfiguration = GetConfiguration();
            }
            //tvwMenu1.Nodes[0].Selected = true;
        }

        protected void LoadTreeView()
        {



            if (WebUser.IsOfficeUser)
            {
                tvwMenu1.Nodes.Add(new TreeNode("Preventivo"));
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
                //tvwMenu1.Nodes[0].ChildNodes.Add(new TreeNode("Nuovo vecchio ", string.Empty, string.Empty, "~/QuotationInsert.aspx", string.Empty));
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Nuovo", string.Empty, string.Empty,
                    "~/QuotationInsert2.aspx", string.Empty));
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Trova", string.Empty, string.Empty, "~/QuotationFind.aspx",
                    string.Empty));

                tvwMenu1.Nodes.Add(new TreeNode());

                tvwMenu1.Nodes.Add(new TreeNode("Modello"));
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Nuovo", string.Empty, string.Empty,
                    "~/QuotationTemplateInsert.aspx", string.Empty));
                tvwMenu1.Nodes[tvwMenu1.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Trova", string.Empty, string.Empty,
                    "~/QuotationTemplateFind.aspx", string.Empty));

                tvwMenu1.Nodes.Add(new TreeNode());

                tvwMenu1.Nodes.Add(new TreeNode("Clienti", string.Empty, string.Empty, "~/CustomersConsole.aspx",
                    string.Empty));

                tvwMenu1.Nodes.Add(new TreeNode("Fornitori", string.Empty, string.Empty, "~/SuppliersConsole.aspx",
                 string.Empty));

                tvwMenu1.Nodes.Add(new TreeNode("Destinazioni viaggi", string.Empty, string.Empty, "~/LocationsConsole.aspx",
                string.Empty));

                tvwMenu1.Nodes.Add(new TreeNode("Viaggi", string.Empty, string.Empty, "~/DeliveryTripsConsole.aspx",
                string.Empty));

                //tvwMenu1.Nodes.Add(new TreeNode("Ordini Clienti", string.Empty, string.Empty,
                //    "~/CustomerOrdersConsole.aspx", string.Empty));



            }

            tvwMenu2.Nodes.Add(new TreeNode("Ordini di produzione"));
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Gestione", string.Empty, string.Empty,
                "~/ProductionOrdersConsole.aspx?Usage=0", string.Empty));
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Denuncia voci libere/impreviste per DATA", string.Empty,
                string.Empty, "~/ProductionOrderDetailsInsertByD.aspx", string.Empty));
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Denuncia operazioni per OPERATORE", string.Empty,
                string.Empty, "~/ProductionOrderDetailsInsertByO3.aspx", string.Empty));
            //tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Visualizzazione avanzamanto", string.Empty, string.Empty,
            //    "~/ProductionOrdersSchedule.aspx", string.Empty));

            tvwMenu2.Nodes.Add(new TreeNode());

            tvwMenu2.Nodes.Add(new TreeNode("Pianificazione attività"));

            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
            //tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Pianificazione produzione", string.Empty, string.Empty,
            //    "~/ProductionOrdersMPS.aspx", string.Empty));

            //tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Calendario lavorazioni di reparto", string.Empty, string.Empty,
            //    "~/ProductionOrdersDepSched.aspx", string.Empty));

            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Calendario lavorazioni di reparto", string.Empty, string.Empty,
              "~/ProductionOrdersDepSched2.aspx", string.Empty));

            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Monitor macchine (tempo reale)", string.Empty, string.Empty,
              "~/RealTimeWorkShopMonitor.aspx", string.Empty));

            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Schedulatore grafico lavorazioni", string.Empty, string.Empty,
              "~/ProductionGraphicScheduler.aspx", string.Empty));
            //tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("OdP in consegna", string.Empty, string.Empty,
            //   "~/ProductionOrdersOnDelivery.aspx", string.Empty));

            tvwMenu2.Nodes.Add(new TreeNode());

            tvwMenu2.Nodes.Add(new TreeNode("Tabella base"));
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Gestione", string.Empty, string.Empty,
                "~/PickingItemsConsole.aspx", string.Empty));
            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Voci libere preventivi", string.Empty, string.Empty,
                "~/QuotationFreeItems.aspx", string.Empty));

            tvwMenu2.Nodes[tvwMenu2.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Macrovoci", string.Empty, string.Empty,
                "~/MacroItemsConsole.aspx", string.Empty));

            tvwMenu2.Nodes.Add(new TreeNode("Tabelle di supporto", string.Empty, string.Empty, "~/TablesMan.aspx",
                string.Empty));


            if (WebUser.IsOfficeUser)
            {

                

                tvwMenu3.Nodes.Add(new TreeNode("Statistiche"));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Redditività ordini di produzione", string.Empty,
                    string.Empty, "~/ProductionOrdersStats.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Ore lavoro operatori", string.Empty, string.Empty,
                    "~/EmployeesWorkingDayHoursStats.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Consumo prodotti", string.Empty, string.Empty,
                    "~/ProductionOrderDetailsConsumption.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Mancata denuncia prodotti preventivo", string.Empty, string.Empty,
                    "~/ProductsUseAnomalies.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Denuncia operazioni per OdP", string.Empty, string.Empty,
                    "~/ProductionOrderDetailsInsertByOdP.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Voci libere (denuncia produzione)", string.Empty,
                    string.Empty, "~/FreeTypePODetails2.aspx", string.Empty));

                tvwMenu3.Nodes.Add(new TreeNode());

                tvwMenu3.Nodes.Add(new TreeNode("Operazioni amministrative"));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].SelectAction = TreeNodeSelectAction.SelectExpand;
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Testi report standard", string.Empty, string.Empty,
                    "javascript:OpenReportTextsItem('ReportTextsPopup.aspx?ID=-1&RType=-1')", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Importazione dati", string.Empty, string.Empty,
                    "~/TablesImport.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Elimina tutti i blocchi utente",
                    "EliminaTuttiIBlocchiUtente"));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Elimina tutte le bozze utente",
                    "EliminaTutteLeBozzeUtente"));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Altre opzioni", string.Empty, string.Empty,
                    "~/Options.aspx", string.Empty));
                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Configurazione", string.Empty, string.Empty,
                    "~/Configuration.aspx", string.Empty));
            }

            if (WebUser.IsAdministrator)
            {

                tvwMenu3.Nodes[tvwMenu3.Nodes.Count - 1].ChildNodes.Add(new TreeNode("Gestione credenziali accesso", string.Empty, string.Empty,
                    "~/Restricted/ManageUser.aspx", string.Empty));

            }



        }

        protected void tvwMenu3_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (tvwMenu3.SelectedValue == "EliminaTuttiIBlocchiUtente")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _result = string.Empty;
                    _qc.prc_LAB_Del_LAB_AllLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp, ref _result);
                }
            }
            if (tvwMenu3.SelectedValue == "EliminaTutteLeBozzeUtente")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _result = string.Empty;
                    _qc.prc_LAB_Del_LAB_AllUserTempQuotations(GetCurrentEmployee().ID);
                }
            }
        }
    }
}