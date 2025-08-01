using System.Configuration;

namespace DLLabExtim
{
    partial class QuotationDataContext
    {

        //public QuotationDataContext() :
        //    base(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString, mappingSource)
        //{

        //}

        partial void OnCreated()
        {
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString;
        }



    }
}