using System.Configuration;

namespace DLLabExtim
{

    partial class Sql_EpDataContext
    {

        //public QuotationDataContext() :
        //    base(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString, mappingSource)
        //{

        //}

        partial void OnCreated()
        {
            Connection.ConnectionString = ConfigurationManager.ConnectionStrings["EuroProgettiConnectionString"].ConnectionString;
        }



    }
}
