using System;
using System.Collections.Generic;
using UILabExtim;

namespace LabExtim
{
    public partial class Configuration : ConfigurationController
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var _config = GetConfiguration();
                itbPIMustUpdateAfter.Text = _config.ContainsKey("PIMU") ? _config["PIMU"] : null;
                itbPIMIDeactivateAfter.Text = _config.ContainsKey("PIDE") ? _config["PIDE"] : null;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var _config = new Dictionary<string, string>();

            var _tmpPIMU = 0;
            if (int.TryParse(itbPIMustUpdateAfter.Text, out _tmpPIMU))
                _config.Add("PIMU", _tmpPIMU.ToString());

            var _tmpPIDE = 0;
            if (int.TryParse(itbPIMIDeactivateAfter.Text, out _tmpPIDE))
                _config.Add("PIDE", _tmpPIDE.ToString());

            SetConfiguration(_config);
        }
    }
}