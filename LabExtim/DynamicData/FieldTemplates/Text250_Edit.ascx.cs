using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class Text250_EditField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return TextBox1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.MaxLength = Column.MaxLength;
            if (Column.MaxLength < 20)
                TextBox1.Columns = Column.MaxLength;
            TextBox1.ToolTip = Column.Description;

            TextBox1.Width = 250;

            SetUpValidator(RequiredFieldValidator1);
            SetUpValidator(RegularExpressionValidator1);
            SetUpValidator(DynamicValidator1);
            SetUpValidator(CustomValidator1);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Mode == DataBoundControlMode.Insert)
            {
                using (var _qc = new QuotationDataContext())
                {
                    if (Table.Name == "Quotations" && Column.Name == "Subject")
                        args.IsValid =
                            _qc.Quotations.Where(t => t.Draft == false)
                                .FirstOrDefault(t => t.Subject.Replace(" ", "").Contains(args.Value.Replace(" ", ""))) ==
                            null;
                    else if (Table.Name == "QuotationTemplates" && Column.Name == "Description")
                        args.IsValid =
                            _qc.QuotationTemplates.FirstOrDefault(
                                t => t.Description.Replace(" ", "").Contains(args.Value.Replace(" ", ""))) == null;
                    else
                        args.IsValid = true;
                }
            }
            else
            {
                using (var _qc = new QuotationDataContext())
                {
                    if (Table.Name == "Quotations" && Column.Name == "Subject")
                        args.IsValid =
                            _qc.Quotations.Where(
                                t =>
                                    t.Draft == false && t.Subject.Replace(" ", "").Contains(args.Value.Replace(" ", "")))
                                .Count() <= 1;
                    else if (Table.Name == "QuotationTemplates" && Column.Name == "Description")
                        args.IsValid =
                            _qc.QuotationTemplates.Where(
                                t => t.Description.Replace(" ", "").Contains(args.Value.Replace(" ", ""))).Count() <= 1;
                    else
                        args.IsValid = true;
                }
            }
        }
    }
}