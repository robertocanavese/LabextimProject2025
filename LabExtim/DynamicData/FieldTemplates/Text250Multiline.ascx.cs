using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class TextField250Multiline : FieldTemplateUserControl
    {
        public string HeightInRows { get; set; }

        public override Control DataControl
        {
            get { return Literal1; }
        }

        protected override void OnLoad(EventArgs e)
        {
            Literal1.Rows = string.IsNullOrEmpty(HeightInRows) ? 3 : Convert.ToInt32(HeightInRows);
            base.OnLoad(e);
        }
    }
}