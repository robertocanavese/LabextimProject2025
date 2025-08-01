using System;
using System.Collections.Specialized;
using System.Web.DynamicData;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class TimeToTicksField : FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //public override Control DataControl
        //{
        //    get
        //    {

        //        return txtProductionTime;
        //    }

        //}

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (FieldValue != null)
                txtProductionTime.Text = new TimeSpan(Convert.ToInt64(FieldValue)).ToString();
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            //dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);

            if (txtProductionTime.Text != string.Empty)
            {
                var _timeParts = txtProductionTime.Text.Split(':');
                try
                {
                    dictionary[Column.Name] =
                        Convert.ToDecimal(
                            new TimeSpan(Int32.Parse(_timeParts[0]), Int32.Parse(_timeParts[1]),
                                Int32.Parse(_timeParts[2])).Ticks);
                }
                catch
                {
                }
            }
        }
    }
}