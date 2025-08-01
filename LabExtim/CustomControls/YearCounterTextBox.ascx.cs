using System;
using System.Drawing;
using System.Web.UI;

namespace LabExtim.CustomControls
{
    public partial class YearCounterTextBox : UserControl
    {
        public string Text
        {
            get { return TextBox1.Text; }
            set { TextBox1.Text = value; }
        }

        public string ReturnValue
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TextBox1.Text))
                {
                    if (string.IsNullOrEmpty(ddlYears.SelectedValue))
                    {
                        return string.Format("/{0}",TextBox1.Text.PadLeft(5, '0'));
                    }
                    return string.Format("{0}/{1}", ddlYears.SelectedValue.Substring(2), TextBox1.Text.PadLeft(5, '0'));
                }
                else
                {
                    //return string.Empty;
                    return ddlYears.SelectedValue;
                }
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
            {
                ibtFind.Enabled = false;
            }
            if (!IsPostBack)
            {
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 10; i--)
                {
                    ddlYears.Items.Add(i.ToString());
                }
            }

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

        public void Reset()
        {
            TextBox1.Text= "";
            ddlYears.SelectedValue = "";
        }

    }
}