using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class YearCounterText_EditField : FieldTemplateUserControl
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
            if (!IsPostBack)
            {
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 10; i--)
                {
                    ddlYears.Items.Add(i.ToString());
                }
            }

        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            if (!string.IsNullOrWhiteSpace(TextBox1.Text))
            {
                if (string.IsNullOrEmpty(ddlYears.SelectedValue))
                {
                    dictionary[Column.Name] = ConvertEditedValue(string.Format("/{0}", TextBox1.Text.PadLeft(5, '0')));
                }
                else
                {
                    dictionary[Column.Name] = ConvertEditedValue(string.Format("{0}/{1}", ddlYears.SelectedValue.Substring(2), TextBox1.Text.PadLeft(5, '0')));
                }
            }
            else
            {
                dictionary[Column.Name] = ConvertEditedValue(ddlYears.SelectedValue);
            }

        }

    }
}