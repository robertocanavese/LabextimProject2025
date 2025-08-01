using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class ChildrenField : FieldTemplateUserControl
    {
        private bool _allowNavigation = true;
        public string NavigateUrl { get; set; }

        public bool AllowNavigation
        {
            get { return _allowNavigation; }
            set { _allowNavigation = value; }
        }

        public override Control DataControl
        {
            get { return HyperLink1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HyperLink1.Text = "Lista " + ChildrenColumn.ChildTable.DisplayName;
        }

        protected string GetChildrenPath()
        {
            if (!AllowNavigation)
            {
                return null;
            }

            if (String.IsNullOrEmpty(NavigateUrl))
            {
                return ChildrenPath;
            }
            return BuildChildrenPath(NavigateUrl);
        }
    }
}