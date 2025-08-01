using System.Linq;

namespace DLLabExtim
{
    public class MemberShipService
    {
        //public aspnet_User GetUser(string GUIDUser)
        //{
        //    using (var _ctx = new MemberShipDataContext())
        //    {
        //        return _ctx.aspnet_Users.SingleOrDefault(u => u.UserId.ToString() == GUIDUser);
        //    }
        //}
        public aspnet_User GetUser(string GUIDUser)
        {
            using (var _ctx = new MemberShipDataContext())
            {
                return _ctx.aspnet_Users.SingleOrDefault(u => u.UserId.ToString() == GUIDUser);
            }
        }
    }
}