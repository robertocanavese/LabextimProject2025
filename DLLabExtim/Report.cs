using System.Configuration;

namespace DLLabExtim
{
    partial class ReportDataContext
    {
        public ReportDataContext() :
            base(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString, mappingSource)
        {
        }
    }
}