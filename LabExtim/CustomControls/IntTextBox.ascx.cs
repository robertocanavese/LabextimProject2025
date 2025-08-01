using System;
using System.Drawing;
using System.Web.UI;

namespace LabExtim.CustomControls
{
    public partial class IntTextBox : UserControl
    {
        public string Text
        {
            get { return TextBox1.Text; }
            set { TextBox1.Text = value; }
        }

        public string ToolTip
        {
            get { return TextBox1.ToolTip; }
            set { TextBox1.ToolTip = value; }
        }

        public int ReturnValue
        {
            get
            {
                int _temp;
                int.TryParse(TextBox1.Text, out _temp);
                return _temp;
            }
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

        public bool ShowFindButton
        {
            get { return ibtFind.Visible; }
            set { ibtFind.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Enabled == false)
                ibtFind.Enabled = false;
        }

        public event EventHandler SearchClick;

        protected void OnSearchClick(EventArgs e)
        {
            if (SearchClick != null)
            {
                SearchClick(this, e);
            }
        }

        protected void ibtFind_Click(object sender, ImageClickEventArgs e)
        {
            OnSearchClick(e);
        }
    }
}