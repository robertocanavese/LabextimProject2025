using System;
using System.Collections.Specialized;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class Integer_EditField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return TextBox1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.ToolTip = Column.Description;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(CompareValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(RangeValidator1);
            SetUpValidator(DynamicValidator1);
            SetUpValidator(CustomValidator1);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Mode == DataBoundControlMode.Insert || Mode == DataBoundControlMode.Edit)
            {
                if (Table.Name == "Quotations" && Column.Name == "MarkUp")
                    args.IsValid = Convert.ToInt32(args.Value) >= 120;
                else
                    args.IsValid = true;
            }
        }
    }
}