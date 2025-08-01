using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class _TablesImport : TablesImportController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataControls();
            }
        }

        protected void BindDataControls()
        {
            var _options = new Dictionary<int, string>();
            _options.Add(0, "Clienti/Fornitori");
            _options.Add(1, "Prodotti");
            _options.Add(2, "Tutto");

            if (_options.Count == 0)
            {
                throw new InvalidOperationException(
                    "There are no accessible options. Make sure that at least one data model is registered in Global.asax and scaffolding is enabled or implement custom pages.");
            }
            Menu1.DataSource = _options;
            Menu1.DataBind();
            //Menu1.Enabled = false; // bloccato nell'attesa che il fornitore FERRARO sia inserito sul gestionale.
        }

        protected void Menu1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ToggleSuccessMessage(ImportGateway.Import(Convert.ToInt32(e.CommandArgument)), lblSuccess,
                LabExtimErrorType.ImportFailed);
            //ToggleSuccessMessage(false, lblSuccess);
        }
    }
}