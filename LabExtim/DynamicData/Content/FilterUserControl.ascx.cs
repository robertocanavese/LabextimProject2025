﻿using System;
using System.Web.DynamicData;

namespace LabExtim.DynamicData.Content
{
    public partial class FilterUserControl : FilterUserControlBase
    {
        public override string SelectedValue
        {
            get { return DropDownList1.SelectedValue; }
        }

        public event EventHandler SelectedIndexChanged
        {
            add { DropDownList1.SelectedIndexChanged += value; }
            remove { DropDownList1.SelectedIndexChanged -= value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateListControl(DropDownList1);

                // Set the initial value if there is one
                if (!String.IsNullOrEmpty(InitialValue))
                    DropDownList1.SelectedValue = InitialValue;
            }
        }
    }
}