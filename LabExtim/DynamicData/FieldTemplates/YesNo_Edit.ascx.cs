using System;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class YesNo_EditField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return DropDownList1; }
        }

        public bool AllowNullValue
        {
            get
            {
                if (this.Attributes["AllowNullValue"] == null)
                    return true;
                return Convert.ToBoolean(this.Attributes["AllowNullValue"]);
            }
            set
            {
                this.Attributes["AllowNullValue"] = value.ToString();
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (this.Attributes["ReadOnly"] == null)
                    return false;
                return Convert.ToBoolean(this.Attributes["ReadOnly"]);
            }
            set
            {
                this.Attributes["ReadOnly"] = value.ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (DropDownList1.Items.Count == 0)
            {
                if ((!Column.IsRequired) && AllowNullValue)
                {
                    DropDownList1.Items.Add(new ListItem("  ", ""));
                }

                DropDownList1.Items.Add(new ListItem("Sì", "1"));
                DropDownList1.Items.Add(new ListItem("No", "0"));
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            //if (Mode == DataBoundControlMode.Edit)
            //{
            try
            {
                //var foreignkey = FieldValue.ToString();
                var foreignkey = (FieldValue == null ? "0": FieldValue.ToString());
                var item = DropDownList1.Items.FindByValue(foreignkey);
                if (item != null)
                {
                    DropDownList1.SelectedValue = foreignkey;
                    DropDownList1.Enabled = !ReadOnly;
                }
            }
                catch
            {}
            //}
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
            // If it's an empty string, change it to null
            var val = DropDownList1.SelectedValue;
            if (val == String.Empty)
                val = null;

            //ExtractForeignKey(dictionary, val);
            dictionary[Column.Name] = ConvertEditedValue(val);
        }

    }
}