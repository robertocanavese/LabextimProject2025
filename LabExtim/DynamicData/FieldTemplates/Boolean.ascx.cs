using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class BooleanField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return CheckBox1; }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            var val = FieldValue;
            if (val != null)
                CheckBox1.Checked = (bool) val;
        }
    }
}