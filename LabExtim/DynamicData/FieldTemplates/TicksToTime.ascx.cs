using System.Web.DynamicData;
using System.Web.UI;

namespace LabExtim.DynamicData.FieldTemplates
{
    public partial class TicksToTimeField : FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get { return Literal1; }
        }
    }
}