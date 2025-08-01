using System;
using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class ForeignKeyField : FieldTemplateUserControl
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

        protected string GetDisplayString()
        {
            return FormatFieldValue(ForeignKeyColumn.ParentTable.GetDisplayString(FieldValue));
        }

        protected string GetNavigateUrl()
        {
            if (!AllowNavigation)
            {
                return null;
            }

            if (String.IsNullOrEmpty(NavigateUrl))
            {
                return ForeignKeyPath;
            }
            return BuildForeignKeyPath(NavigateUrl);
        }
    }
}