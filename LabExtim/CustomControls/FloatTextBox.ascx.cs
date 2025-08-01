using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LabExtim.CustomControls
{
    public partial class FloatTextBox : UserControl
    {
        public string Text
        {
            get { return TextBox1.Text; }
            set { TextBox1.Text = value; }
        }

        public bool Enabled
        {
            get { return TextBox1.Enabled; }
            set { TextBox1.Enabled = value; }
        }

        public Color BackColor
        {
            get { return TextBox1.BackColor; }
            set { TextBox1.BackColor = value; }
        }

        public Color ForeColor
        {
            get { return TextBox1.ForeColor; }
            set { TextBox1.ForeColor = value; }
        }

        public double Width
        {
            get { return TextBox1.Width.Value; }
            set { TextBox1.Width = new Unit(value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox1.Attributes.Add("onkeyup", "this.value = this.value.replace('.',',');");
        }
    }
}