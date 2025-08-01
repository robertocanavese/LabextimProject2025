using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using UILabExtim;
using System.Management;

namespace LabExtim
{
    public partial class GenericPrint : BaseController
    {
        private ReportDocument m_Report;

        protected void Page_Init(object sender, EventArgs e)
        {
            TableHeader = new KeyValuePair<string, string>(ReportFileNameParameter, "");

            ConfigureReport();
        }

        private void ConfigureReport()
        {
            //if (!Page.IsPostBack)
            //{
            m_Report = new ReportDocument();

            try
            {
                m_Report.Load(Server.MapPath(@"Reports/" + ReportFileNameParameter + ".rpt"));
            }
            catch
            {
                Response.Redirect(Request.UrlReferrer.PathAndQuery, true);
                return;
            }

            var _cn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString);
            var _currentInstance = _cn.DataSource;

            var myConnectionInfo = new ConnectionInfo();
            myConnectionInfo.ServerName = _currentInstance;
            myConnectionInfo.DatabaseName = "Labextim";
            myConnectionInfo.IntegratedSecurity = true;
            //myConnectionInfo.UserID = "usrw";
            //myConnectionInfo.Password = "usrw";

            var ReportPath = string.Empty;

            ReportPath = Server.MapPath(@"Reports/" + ReportFileNameParameter + ".rpt");

            //crvGeneric.ReportSource = ReportPath;
            crvGeneric.ReportSource = m_Report;

            var _conns = m_Report.DataSourceConnections;
            //_conns[0].SetConnection("localhost", "LabExtim", "usrw", "usrw");
            _conns[0].SetConnection(_currentInstance, "LabExtim", true);

            SetDBLogonForReport(myConnectionInfo, m_Report);
            SetDBLogonForSubreports(myConnectionInfo, m_Report);
            //}
            //crvGeneric.ReportSource = m_Report;
        }

        private void SetDBLogonForSubreports(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
            var sections = reportDocument.ReportDefinition.Sections;
            foreach (Section section in sections)
            {
                var reportObjects = section.ReportObjects;
                foreach (ReportObject reportObject in reportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        var subreportObject = (SubreportObject)reportObject;
                        var subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        SetDBLogonForReport(connectionInfo, subReportDocument);
                    }
                }
            }
        }

        private void SetDBLogonForReport(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
            var tables = reportDocument.Database.Tables;
            foreach (Table table in tables)
            {
                var tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogonInfo);
            }
        }

        private void SetDBLogonForReport(ConnectionInfo myConnectionInfo)
        {
            var myTableLogOnInfos = crvGeneric.LogOnInfo;
            foreach (TableLogOnInfo myTableLogOnInfo in myTableLogOnInfos)
            {
                myTableLogOnInfo.ConnectionInfo = myConnectionInfo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillControls();
            }
            FillDependingControls();
        }

        private void FillControls()
        {
            hypTableConsole.NavigateUrl = "~/" + ReportFileNameParameter + "Console.aspx";

            if (ReportFileNameParameter == "PickingItems")
            {
                //hypTableConsole.NavigateUrl = "~/" + ReportFileNameParameter + "2Console.aspx";
                crvGeneric.SelectionFormula = SelectionFormulaParameter;
                //crvGeneric.SelectionFormula = "{Customer.Last Year's Sales} > 11000.00 AND Mid({Customer.Customer Name}, 1, 1) = \"A\"";
            }

            LoadPrinters();
        }

        protected void FillDependingControls()
        {
        }


        protected void LoadPrinters()
        {
            //PrintersList.DataSource = PrinterSettings.InstalledPrinters;
            PrintersList.DataSource = GetAllPrinterList();
            PrintersList.DataBind();
        }


        private List<string> GetAllPrinterList()
        {
            List<string> result = new List<string>();

            if (ImpersonateValidUser("stampe", "labe", "0497800500"))
            {

                System.Management.ManagementScope objMS =
                    new System.Management.ManagementScope(ManagementPath.DefaultPath);
                objMS.Connect();

                SelectQuery objQuery = new SelectQuery("SELECT * FROM Win32_Printer");
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher(objMS, objQuery);
                System.Management.ManagementObjectCollection objMOC = objMOS.Get();

                foreach (ManagementObject Printers in objMOC)
                {
                    //if (Convert.ToBoolean(Printers["Local"]))       // LOCAL PRINTERS.
                    //{
                    //    cmbLocalPrinters.Items.Add(Printers["Name"]);
                    //}
                    //if (Convert.ToBoolean(Printers["Network"]))     // ALL NETWORK PRINTERS.
                    //{
                    //    cmbNetworkPrinters.Items.Add(Printers["Name"]);
                    //}

                    if (Convert.ToBoolean(Printers["Local"]))       // LOCAL PRINTERS.
                    {
                        result.Add(Printers["Name"].ToString());
                    }
                    if (Convert.ToBoolean(Printers["Network"]))     // ALL NETWORK PRINTERS.
                    {
                        result.Add(Printers["Name"].ToString());
                    }
                }
                UndoImpersonation();
            }

            return result;
        }


        protected void lbtExportPDF_Click(object sender, EventArgs e)
        {
            var diskFileName = Server.MapPath(ConfigurationManager.AppSettings["ExportPath"])
                               + "Tabella " + ReportFileNameParameter.Trim() + ".pdf";
            var urlFileName = ConfigurationManager.AppSettings["ExportPath"]
                              + "Tabella " + ReportFileNameParameter.Trim() + ".pdf";

            ExportToDisk(diskFileName, ExportFormatType.PortableDocFormat);
            Response.Redirect(urlFileName);
        }

        protected void lbtExportExcel_Click(object sender, EventArgs e)
        {
            var diskFileName = Server.MapPath(ConfigurationManager.AppSettings["ExportPath"])
                               + "Tabella " + ReportFileNameParameter.Trim() + ".xls";
            var urlFileName = ConfigurationManager.AppSettings["ExportPath"]
                              + "Tabella " + ReportFileNameParameter.Trim() + ".xls";

            ExportToDisk(diskFileName, ExportFormatType.Excel);
            Response.Redirect(urlFileName);
        }

        private void ExportToDisk(string fileName, ExportFormatType type)
        {
            var diskOpts =
                ExportOptions.CreateDiskFileDestinationOptions();

            var exportOpts = new ExportOptions();
            exportOpts.ExportFormatType = type;
            exportOpts.ExportDestinationType =
                ExportDestinationType.DiskFile;

            diskOpts.DiskFileName = fileName;
            exportOpts.ExportDestinationOptions = diskOpts;

            m_Report.RecordSelectionFormula = SelectionFormulaParameter;
            m_Report.Refresh();
            m_Report.Export(exportOpts);
            m_Report.Close();
            m_Report.Dispose();
            ;
        }

        protected void PrintersList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            m_Report.PrintOptions.PrinterName = e.CommandName;

            System.Drawing.Printing.PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = e.CommandName;

            try
            {
                m_Report.RecordSelectionFormula = SelectionFormulaParameter;
                m_Report.Refresh();

                if (ImpersonateValidUser("stampe", "labe", "0497800500"))
                {

                    if (m_Report.PrintOptions.PrinterName != settings.PrinterName)
                        m_Report.PrintToPrinter(settings, new PageSettings(), false); // print with settings
                    else
                        m_Report.PrintToPrinter(settings.Copies, settings.Collate, 0, 0); // print with printer name


                    //m_Report.PrintToPrinter(1, false, 0, 0);
                    UndoImpersonation();
                }

                m_Report.Close();
                //m_Report.Dispose();
            }
            catch
            {
            }
        }

        protected void PrintersList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.LinkButton btn = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("Printer");
                btn.Text = e.Item.DataItem.ToString();
                btn.CommandName = btn.Text;
            }

        }
    }
}