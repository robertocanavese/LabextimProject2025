using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DLLabExtim;
using UILabExtim;
using System.Management;

namespace LabExtim
{
    public partial class QuotationPrint : QuotationController
    {
        private string m_ObjectId;
        private string m_ObjectIdLabel;
        private ReportDocument m_Report;

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(dtvQuotation);
            ConfigureReport();
        }

        private void ConfigureReport()
        {

            m_Report = new ReportDocument();
            Quotation _currentQuotation = null; ;

            if (ReportTypeParameter == (int)ReportType.Customer)
            {
                _currentQuotation = new QuotationDataContext().Quotations.SingleOrDefault(q => q.ID == Convert.ToInt32(QuotationParameter));
                if (_currentQuotation.ID_Manager == 2)
                    m_Report.Load(Server.MapPath(@"Reports/QuotationCartolabe2024.rpt"));
                else
                    m_Report.Load(Server.MapPath(@"Reports/QuotationLabe2024.rpt"));
                
                m_ObjectIdLabel = "Preventivo No ";
                m_ObjectId = QuotationParameter;
                lbtExportPDF.Text = "Registra e stampa in PDF";
            }
            if (ReportTypeParameter == (int)ReportType.Technical)
            {
                m_Report.Load(Server.MapPath(@"Reports/QuotationTechnical.rpt"));
                m_ObjectIdLabel = "Preventivo tecnico No ";
                m_ObjectId = QuotationParameter.TrimEnd();
            }
            if (ReportTypeParameter == (int)ReportType.ProductionOrder)
            {
                m_Report.Load(Server.MapPath(@"Reports/ProductionOrder.rpt"));
                m_ObjectIdLabel = "Ordine di produzione No ";
                m_ObjectId = ProductionOrderParameter.TrimEnd();
            }
            if (ReportTypeParameter == (int)ReportType.POFinalCost)
            {
                m_Report.Load(Server.MapPath(@"Reports/ProductionOrderStat.rpt"));
                m_ObjectIdLabel = "Ordine di produzione No ";
                m_ObjectId = ProductionOrderParameter.TrimEnd();
            }

            var _cn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString);
            var _currentInstance = _cn.DataSource;

            var myConnectionInfo = new ConnectionInfo();
            myConnectionInfo.ServerName = _currentInstance;
            myConnectionInfo.DatabaseName = _cn.Database; // "Labextim";
            myConnectionInfo.IntegratedSecurity = true;

            var ReportPath = string.Empty;
            if (ReportTypeParameter == (int)ReportType.Customer)
            {
               
                if (_currentQuotation.ID_Manager == 2)
                    m_Report.Load(Server.MapPath(@"Reports/QuotationCartolabe2024.rpt"));
                else
                    m_Report.Load(Server.MapPath(@"Reports/QuotationLabe2024.rpt"));
            }
            if (ReportTypeParameter == (int)ReportType.Technical)
            {
                ReportPath = Server.MapPath(@"Reports/QuotationTechnical.rpt");
            }
            if (ReportTypeParameter == (int)ReportType.ProductionOrder)
            {
                ReportPath = Server.MapPath(@"Reports/ProductionOrder.rpt");
            }
            if (ReportTypeParameter == (int)ReportType.POFinalCost)
            {
                ReportPath = Server.MapPath(@"Reports/ProductionOrderStat.rpt");
            }

            crvQuotation.ReportSource = m_Report;

            var _conns = m_Report.DataSourceConnections;

            _conns[0].SetConnection(_currentInstance, _cn.Database, true);

            if (ReportTypeParameter == (int)ReportType.Customer)
            {

                var _customerCode = _currentQuotation.CustomerCode;

                int pos = -1;
                pos += 1;
                m_Report.SetParameterValue("@QuotationID", Convert.ToInt32(QuotationParameter));
                var field1 = crvQuotation.ParameterFieldInfo[pos];
                var val1 = new ParameterDiscreteValue();
                val1.Value = Convert.ToInt32(QuotationParameter);
                field1.CurrentValues.Add(val1);

                //var _recipientText = GetBestReportText(1, 1, _customerCode);
                //pos += 1;
                //m_Report.SetParameterValue("@Recipient", _recipientText);
                //var field2 = crvQuotation.ParameterFieldInfo[1];
                //var val2 = new ParameterDiscreteValue();
                //val2.Value = _recipientText;
                //field2.CurrentValues.Add(val2);

                var _objectText =
                    GetBestReportText(1, 2, _customerCode) == string.Empty
                        ? "Preventivo per " + _currentQuotation.Subject
                        : GetBestReportText(1, 2, _customerCode);
                pos += 1;
                m_Report.SetParameterValue("@Object", _objectText);
                var field3 = crvQuotation.ParameterFieldInfo[pos];
                var val3 = new ParameterDiscreteValue();
                val3.Value = _objectText;
                field3.CurrentValues.Add(val3);

                //var _portText = GetBestReportText(1, 3, _customerCode);
                //pos += 1;
                //m_Report.SetParameterValue("@Port", _portText);
                //var field4 = crvQuotation.ParameterFieldInfo[pos + 1];
                //var val4 = new ParameterDiscreteValue();
                //val4.Value = _portText;
                //field4.CurrentValues.Add(val4);

                var _paymentText = GetBestReportText(1, 4, _customerCode);
                pos += 1;
                m_Report.SetParameterValue("@Payment", _paymentText);
                var field5 = crvQuotation.ParameterFieldInfo[pos];
                var val5 = new ParameterDiscreteValue();
                val5.Value = _paymentText;
                field5.CurrentValues.Add(val5);

                //var _openingText = GetBestReportText(1, 5, _customerCode);
                //pos += 1;
                //m_Report.SetParameterValue("@Opening", _openingText);
                //var field6 = crvQuotation.ParameterFieldInfo[5];
                //var val6 = new ParameterDiscreteValue();
                //val6.Value = _openingText;
                //field6.CurrentValues.Add(val6);

                var _closingText = GetBestReportText(1, 6, _customerCode);
                pos += 1;
                m_Report.SetParameterValue("@Closing", _closingText);
                var field7 = crvQuotation.ParameterFieldInfo[pos];
                var val7 = new ParameterDiscreteValue();
                val7.Value = _closingText;
                field7.CurrentValues.Add(val7);
            }

            if (ReportTypeParameter == (int)ReportType.Technical)
            {
                m_Report.SetParameterValue("@QuotationID", Convert.ToInt32(QuotationParameter));
                m_Report.SetParameterValue("CalcQuantity", Convert.ToInt32(ReportOnProductionQuantityParameter));
                m_Report.SetParameterValue("@QuotationID1", Convert.ToInt32(QuotationParameter));
                m_Report.SetParameterValue("@Quantity", Convert.ToInt32(ReportOnProductionQuantityParameter));

                var field1 = crvQuotation.ParameterFieldInfo[0];
                var val1 = new ParameterDiscreteValue();
                val1.Value = Convert.ToInt32(QuotationParameter);
                field1.CurrentValues.Add(val1);

                var field2 = crvQuotation.ParameterFieldInfo[1];
                var val2 = new ParameterDiscreteValue();
                val2.Value = Convert.ToInt32(ReportOnProductionQuantityParameter);
                field2.CurrentValues.Add(val2);

                var field3 = crvQuotation.ParameterFieldInfo[2];
                var val3 = new ParameterDiscreteValue();
                val3.Value = Convert.ToInt32(QuotationParameter);
                field3.CurrentValues.Add(val3);

                var field4 = crvQuotation.ParameterFieldInfo[3];
                var val4 = new ParameterDiscreteValue();
                val4.Value = Convert.ToInt32(ReportOnProductionQuantityParameter);
                field4.CurrentValues.Add(val4);

            }

            if (ReportTypeParameter == (int)ReportType.ProductionOrder)
            {
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter));
                m_Report.SetParameterValue("@QuotationID1", Convert.ToInt32(QuotationParameter));
                m_Report.SetParameterValue("@Quantity", Convert.ToInt32(ReportOnProductionQuantityParameter));
                m_Report.SetParameterValue("@IdCompany", Convert.ToInt32(ReportOnProductionIdCompanyParameter));

                var field1 = crvQuotation.ParameterFieldInfo[0];
                var val1 = new ParameterDiscreteValue();
                val1.Value = Convert.ToInt32(QuotationParameter);
                field1.CurrentValues.Add(val1);

                var field2 = crvQuotation.ParameterFieldInfo[1];
                var val2 = new ParameterDiscreteValue();
                val2.Value = Convert.ToInt32(QuotationParameter);
                field2.CurrentValues.Add(val2);

                var field3 = crvQuotation.ParameterFieldInfo[2];
                var val3 = new ParameterDiscreteValue();
                val3.Value = Convert.ToInt32(ReportOnProductionQuantityParameter);
                field3.CurrentValues.Add(val3);

                var field4 = crvQuotation.ParameterFieldInfo[3];
                var val4 = new ParameterDiscreteValue();
                val4.Value = Convert.ToInt32(ReportOnProductionIdCompanyParameter);
                field4.CurrentValues.Add(val4);
            }

            if (ReportTypeParameter == (int)ReportType.POFinalCost)
            {
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter));
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter),
                    m_Report.Subreports[0].Name);
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter),
                    m_Report.Subreports[1].Name);
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter),
                    m_Report.Subreports[2].Name);
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter),
                    m_Report.Subreports[3].Name);
                m_Report.SetParameterValue("@ProductionOrderID", Convert.ToInt32(ProductionOrderParameter),
                    m_Report.Subreports[4].Name);

                var field1 = crvQuotation.ParameterFieldInfo[0];
                var val1 = new ParameterDiscreteValue();
                val1.Value = Convert.ToInt32(ProductionOrderParameter);
                field1.CurrentValues.Add(val1);

                var field2 = crvQuotation.ParameterFieldInfo[1];
                var val2 = new ParameterDiscreteValue();
                val2.Value = Convert.ToInt32(ProductionOrderParameter);
                field2.CurrentValues.Add(val2);

                var field3 = crvQuotation.ParameterFieldInfo[2];
                var val3 = new ParameterDiscreteValue();
                val3.Value = Convert.ToInt32(ProductionOrderParameter);
                field3.CurrentValues.Add(val3);

                var field4 = crvQuotation.ParameterFieldInfo[3];
                var val4 = new ParameterDiscreteValue();
                val4.Value = Convert.ToInt32(ProductionOrderParameter);
                field4.CurrentValues.Add(val4);

                var field5 = crvQuotation.ParameterFieldInfo[4];
                var val5 = new ParameterDiscreteValue();
                val5.Value = Convert.ToInt32(ProductionOrderParameter);
                field5.CurrentValues.Add(val5);

                var field6 = crvQuotation.ParameterFieldInfo[5];
                var val6 = new ParameterDiscreteValue();
                val6.Value = Convert.ToInt32(ProductionOrderParameter);
                field6.CurrentValues.Add(val6);
            }

            SetDBLogonForReport(myConnectionInfo, m_Report);
            SetDBLogonForSubreports(myConnectionInfo, m_Report);
            
            crvQuotation.ReportSource = m_Report;
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
            var myTableLogOnInfos = crvQuotation.LogOnInfo;
            foreach (TableLogOnInfo myTableLogOnInfo in myTableLogOnInfos)
            {
                myTableLogOnInfo.ConnectionInfo = myConnectionInfo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //((Site)this.Master).UpdateProgressVisible = false;
                FillControls();
            }
            FillDependingControls();
        }

        private void FillControls()
        {
            if (ProductionOrderParameter != string.Empty)
            {
                ((HtmlTable)Master.FindControl("tblHeader")).Visible = false;
                //hypQuotationConsole.NavigateUrl = "~/ProductionOrderPopUp.aspx?POid=" + ProductionOrderParameter;
                hypQuotationConsole.NavigateUrl = Request.UrlReferrer.PathAndQuery;
                hypQuotationConsole.Text = "Torna a Ordine di Produzione";
            }
            else
            {
                hypQuotationConsole.NavigateUrl = QuotationConsolePage + "?P0=" + QuotationParameter;
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

        protected void dtvQuotation_ItemCreated(object sender, EventArgs e)
        {
            //if (ProductionOrderHeader.Key == 0)
            //{
            if (ReportTypeParameter == (int)ReportType.ProductionOrder)
            {
                if (ProductionOrderHeader.Key == -1)
                {
                    ProductionOrderHeader = new KeyValuePair<int, string>(Convert.ToInt32(ProductionOrderParameter),
                        "Senza nome");
                }
            }
            else
            {
                ProductionOrderHeader = new KeyValuePair<int, string>(-1, "Non previsto");
            }
            //}

            var _currentQuotation = (Quotation)dtvQuotation.DataItem;
            if (QuotationHeader.Key == -1)
            {
                QuotationHeader = new KeyValuePair<int, string>(_currentQuotation.ID, _currentQuotation.Subject);
            }
            var _context = new QuotationDataContext();
            lbtReportTextsConfigure.Attributes.Add("onclick",
                "javascript:OpenReportTextsItem('ReportTextsPopup.aspx?ID=" +
                _context.Quotations.SingleOrDefault(q => q.ID == QuotationHeader.Key).CustomerCode +
                "&RType=1')");
        }


        protected void lbtExportPDF_Click(object sender, EventArgs e)
        {
            var diskFileName = Server.MapPath(ConfigurationManager.AppSettings["ExportPath"])
                               + m_ObjectIdLabel + m_ObjectId + ".pdf";
            var urlFileName = ConfigurationManager.AppSettings["ExportPath"]
                              + m_ObjectIdLabel + m_ObjectId + ".pdf";

            ExportToDisk(diskFileName, ExportFormatType.PortableDocFormat);
            Response.Redirect(urlFileName);
        }

        protected void lbtExportExcel_Click(object sender, EventArgs e)
        {
            var diskFileName = Server.MapPath(ConfigurationManager.AppSettings["ExportPath"])
                               + m_ObjectIdLabel + m_ObjectId + ".xls";
            var urlFileName = ConfigurationManager.AppSettings["ExportPath"]
                              + m_ObjectIdLabel + m_ObjectId + ".xls";

            ExportToDisk(diskFileName, ExportFormatType.Excel);
            Response.Redirect(urlFileName);
        }

        protected void lbtExportWord_Click(object sender, EventArgs e)
        {
            var diskFileName = Server.MapPath(ConfigurationManager.AppSettings["ExportPath"])
                               + m_ObjectIdLabel + m_ObjectId + ".doc";
            var urlFileName = ConfigurationManager.AppSettings["ExportPath"]
                              + m_ObjectIdLabel + m_ObjectId + ".doc";

            ExportToDisk(diskFileName, ExportFormatType.WordForWindows);
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

            m_Report.Export(exportOpts);

            m_Report.Close();
            m_Report.Clone();
            m_Report.Dispose();
            m_Report = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        protected void ibtUpdate_Click(object sender, ImageClickEventArgs e)
        {
            //m_Report.Refresh();
            ConfigureReport();
        }

        protected void PrintersList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {


            m_Report.PrintOptions.PrinterName = e.CommandName;

            System.Drawing.Printing.PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = e.CommandName;



            try
            {

                if (ImpersonateValidUser("stampe", "labe", "0497800500"))
                {

                    if (m_Report.PrintOptions.PrinterName != settings.PrinterName)
                        m_Report.PrintToPrinter(settings, new PageSettings(), false); // print with settings
                    else
                        m_Report.PrintToPrinter(settings.Copies, settings.Collate, 0, 0); // print with printer name


                    //m_Report.PrintToPrinter(1, false, 0, 0);
                    UndoImpersonation();
                }

                //m_Report.PrintToPrinter(1, false, 0, 0);
                //m_Report.Close();
                //m_Report.Dispose();

                m_Report.Close();
                m_Report.Clone();
                m_Report.Dispose();
                m_Report = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
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



        protected void dtvQuotation_ItemUpdated(object sender, System.Web.UI.WebControls.DetailsViewUpdatedEventArgs e)
        {

            using (var quotationDataContext = new QuotationDataContext())
            {
                var _curTempQuotation =
                  quotationDataContext.TempQuotations.FirstOrDefault(
                      Quotation =>
                          Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                          Quotation.SessionUser == GetCurrentEmployee().ID);

                if (_curTempQuotation != null)
                {
                    _curTempQuotation.P1 = Convert.ToBoolean(e.NewValues["P1"]);
                    _curTempQuotation.P2 = Convert.ToBoolean(e.NewValues["P2"]);
                    _curTempQuotation.P3 = Convert.ToBoolean(e.NewValues["P3"]);
                    _curTempQuotation.P4 = Convert.ToBoolean(e.NewValues["P4"]);
                    _curTempQuotation.P5 = Convert.ToBoolean(e.NewValues["P5"]);

                    quotationDataContext.SubmitChanges();

                }


            }

        }






    }
}