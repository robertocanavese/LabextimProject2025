using System.Configuration;

namespace DLLabExtim
{
    partial class GeneralDataContext
    {
        public GeneralDataContext() :
            base(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString, mappingSource)
        {
        }
    }
}