using System.Configuration;

namespace DLLabExtim
{
    partial class MemberShipDataContext
    {
        public MemberShipDataContext() :
            base(ConfigurationManager.ConnectionStrings["MemberShipConnString"].ConnectionString, mappingSource)
        {
        }
    }
}