using System;
using System.Runtime.Remoting.Messaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;

namespace LabExtim.CustomControls
{
    public partial class SearchEngine : UserControl
    {
        public IntTextBox ItbNo
        {
            get { return itbNo; }
        }

        public YearCounterTextBox YctNumber
        {
            get { return yctNumber; }
        }

        public string LblAgente
        {
            get { return lblAgente.Text; }
            set { lblAgente.Text = value; }
        }

        public string LblYearCounterText
        {
            get { return lblNumber.Text; }
            set { lblNumber.Text = value; }
        }

        public string LblTextField1Text
        {
            get { return lblTextField1.Text; }
            set { lblTextField1.Text = value; }
        }

        public string TextField1Text
        {
            get { return txtTextField1.Text; }
            set { txtTextField1.Text = value; }
        }

        public string ValueHidField1Text
        {
            get { return hidTextField1.Value; }
            set { hidTextField1.Value = value; }
        }


        public string LblTextField2Text
        {
            get { return lblTextField2.Text; }
            set { lblTextField2.Text = value; }
        }

        public string TextField2Text
        {
            get { return txtTextField2.Text; }
            set { txtTextField2.Text = value; }
        }

        public string ValueHidField2Text
        {
            get { return hidTextField2.Value; }
            set { hidTextField2.Value = value; }
        }


        public string LblTextField3Text
        {
            get { return lblTextField3.Text; }
            set { lblTextField3.Text = value; }
        }

        public string TextField3Text
        {
            get { return txtTextField3.Text; }
            set { txtTextField3.Text = value; }
        }

        public string ValueHidField3Text
        {
            get { return hidTextField3.Value; }
            set { hidTextField3.Value = value; }
        }

        public string LblDateFromText
        {
            get { return lblDateFrom.Text; }
            set { lblDateFrom.Text = value; }
        }

        public string TextDateFromText
        {
            get { return txtDateFrom.Text; }
            set { txtDateFrom.Text = value; }
        }

        public string LblDateToText
        {
            get { return lblDateTo.Text; }
            set { lblDateTo.Text = value; }
        }

        public string TextDateToText
        {
            get { return txtDateTo.Text; }
            set { txtDateTo.Text = value; }
        }

        public string LblDropDownList1Text
        {
            get { return lblDropDownList1.Text; }
            set { lblDropDownList1.Text = value; }
        }

        public string LblDropDownList2Text
        {
            get { return lblDropDownList2.Text; }
            set { lblDropDownList2.Text = value; }
        }

        public string LblDropDownList3Text
        {
            get { return lblDropDownList3.Text; }
            set { lblDropDownList3.Text = value; }
        }

        public string LblDropDownList4Text
        {
            get { return lblDropDownList4.Text; }
            set { lblDropDownList4.Text = value; }
        }

        public string LblDropDownList5Text
        {
            get { return lblDropDownList5.Text; }
            set { lblDropDownList5.Text = value; }
        }

        public DropDownList DropDownList1
        {
            get { return ddlDropDownList1; }
        }

        public DropDownList DropDownList2
        {
            get { return ddlDropDownList2; }
        }

        public DropDownList DropDownList3
        {
            get { return ddlDropDownList3; }
        }

        public DropDownList DropDownList4
        {
            get { return ddlDropDownList4; }
        }

        public DropDownList DropDownList5
        {
            get { return ddlDropDownList5; }
        }

        public DropDownList DropDownAgenti
        {
            get { return DropDownListAgente; }
        }

        public DropDownList DdlOrderBy
        {
            get { return ddlOrderBy; }
        }

        public bool UseBigButtons
        {
            get;
            set;
        }

        public string CustomCssClass
        {
            get;
            set;
        }


        public string GetCurrentSelection()
        {

            string result = "";

            result += YctNumber.Text + "|";
            result += txtTextField1.Text + "|";
            result += txtTextField2.Text + "|";
            result += txtTextField3.Text + "|";
            result += txtDateFrom.Text + "|";
            result += txtDateTo.Text + "|";

            result += hidTextField1.Value + "|";
            result += hidTextField2.Value + "|";
            result += hidTextField3.Value + "|";

            result += DropDownList1.SelectedValue + "|";
            result += DropDownList2.SelectedValue + "|";
            result += DropDownList3.SelectedValue + "|";
            result += DropDownList4.SelectedValue + "|";
            result += DropDownList5.SelectedValue + "|";
            result += DropDownListAgente.SelectedValue;

            return result;

        }

        public void SetCurrentSelection(string pipedValues)
        {

            string[] values = pipedValues.Split('|');

            if (!String.IsNullOrEmpty(values[0])) YctNumber.Text = values[0];
            if (!String.IsNullOrEmpty(values[1])) txtTextField1.Text = values[1];
            if (!String.IsNullOrEmpty(values[2])) txtTextField2.Text = values[2];
            if (!String.IsNullOrEmpty(values[3])) txtTextField3.Text = values[3];
            if (!String.IsNullOrEmpty(values[4])) txtDateFrom.Text = values[4];
            if (!String.IsNullOrEmpty(values[5])) txtDateTo.Text = values[5];
            if (!String.IsNullOrEmpty(values[6])) hidTextField1.Value = values[6];
            if (!String.IsNullOrEmpty(values[7])) hidTextField2.Value = values[7];
            if (!String.IsNullOrEmpty(values[8])) hidTextField3.Value = values[8];
            if (!String.IsNullOrEmpty(values[9])) DropDownList1.SelectedValue = values[9];
            if (!String.IsNullOrEmpty(values[10])) DropDownList2.SelectedValue = values[10];
            if (!String.IsNullOrEmpty(values[11])) DropDownList3.SelectedValue = values[11];
            if (!String.IsNullOrEmpty(values[12])) DropDownList4.SelectedValue = values[12];
            if (!String.IsNullOrEmpty(values[13])) DropDownList5.SelectedValue = values[13];
            if (!String.IsNullOrEmpty(values[14])) DropDownListAgente.SelectedValue = values[14];

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ItbNo.SearchClick += SearchClick;
            YctNumber.SearchClick += SearchClick;

            YctNumber.Visible = LblYearCounterText != string.Empty;
            txtTextField1.Visible = LblTextField1Text != string.Empty;
            txtTextField2.Visible = LblTextField2Text != string.Empty;
            txtTextField3.Visible = LblTextField3Text != string.Empty;
            txtDateFrom.Visible = LblDateFromText != string.Empty;
            ImageButton1.Visible = LblDateFromText != string.Empty;
            txtDateTo.Visible = LblDateToText != string.Empty;
            ImageButton2.Visible = LblDateToText != string.Empty;

            DropDownList1.Visible = LblDropDownList1Text != string.Empty;
            DropDownList2.Visible = LblDropDownList2Text != string.Empty;
            DropDownList3.Visible = LblDropDownList3Text != string.Empty;
            DropDownList4.Visible = LblDropDownList4Text != string.Empty;
            DropDownList5.Visible = LblDropDownList5Text != string.Empty;
            DropDownListAgente.Visible = lblAgente.Text != string.Empty;

            if ((LblDropDownList1Text != string.Empty || LblDropDownList2Text != string.Empty) &&
                (LblDropDownList3Text + LblDropDownList4Text + LblDropDownList5Text) == string.Empty)
            {
                tdLbl1.ColSpan = 2;
                tdLbl2.ColSpan = 2;
                tdDdl1.ColSpan = 2;
                tdDdl2.ColSpan = 2;
                tdLbl3.Visible = false;
                tdLbl4.Visible = false;
                tdDdl3.Visible = false;
                tdDdl4.Visible = false;
            }

            lbtSearch.Visible = !UseBigButtons;
            lbtEmpty.Visible = !UseBigButtons;
            ibtSearch.Visible = UseBigButtons;
            ibtEmpty.Visible = UseBigButtons;

            if (!string.IsNullOrEmpty(CustomCssClass))
            {
                this.Table1.Attributes["class"] = CustomCssClass;
                this.Table2.Attributes["class"] = CustomCssClass;
            }

        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["ProductionOrdersTypesSelector"] = ddlTypes.SelectedValue;
            //Session["ProductionOrdersStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["ProductionOrdersSuppliersSelector"] = ddlCustomers.SelectedValue;
            //Session["ProductionOrdersOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        public event EventHandler SearchClick;
        public event EventHandler EmptyClick;

        protected void OnSearchClick(EventArgs e)
        {
            if (SearchClick != null)
            {
                SearchClick(this, e);
            }
        }

        protected void OnEmptyClick(EventArgs e)
        {
            if (EmptyClick != null)
            {
                EmptyClick(this, e);
            }
        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            OnSearchClick(e);
        }

        protected void lbtEmpty_Click(object sender, EventArgs e)
        {
            itbNo.Text = string.Empty;
            yctNumber.Text = string.Empty;
            txtTextField1.Text = string.Empty;
            txtTextField2.Text = string.Empty;
            txtTextField3.Text = string.Empty;
            hidTextField1.Value = string.Empty;
            hidTextField2.Value = string.Empty;
            hidTextField3.Value = string.Empty;
            txtDateFrom.Text = string.Empty;
            txtDateTo.Text = string.Empty;
            if (DropDownList1.Visible) DropDownList1.SelectedIndex = 0;
            if (DropDownList2.Visible) DropDownList2.SelectedIndex = 0;
            if (DropDownList3.Visible) DropDownList3.SelectedIndex = 0;
            if (DropDownList4.Visible) DropDownList4.SelectedIndex = 0;
            if (DropDownList5.Visible) DropDownList5.SelectedIndex = 0;
            if (DropDownListAgente.Visible) DropDownListAgente.SelectedIndex = 0;

            OnEmptyClick(e);
        }


    }
}