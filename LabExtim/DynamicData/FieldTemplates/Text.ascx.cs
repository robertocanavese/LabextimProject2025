using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class TextField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return Literal1; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            Decimal _temp;
            if (Decimal.TryParse(Literal1.Text.Replace("%", ""), out _temp))
                if (_temp < 0m)
                {
                    var _text = "<span style=\"color: red;\">" + Literal1.Text + "</span>";
                    Literal1.Text = _text;
                }

            base.OnPreRender(e);
        }
    }
}