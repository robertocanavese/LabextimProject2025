namespace DLLabExtim
{
    public class QuotationService
    {
        public string SetLock(int? lockTypeCode, int? IDRef, object GUIDUser, string sessionID)
        {
            using (var _ctx = new QuotationDataContext())
            {
                string procResult = null;
                var _result = _ctx.prc_LAB_Ins_LAB_Lock(lockTypeCode, IDRef, GUIDUser.ToString(), sessionID,
                    ref procResult);
                return procResult.TrimEnd();
            }
        }

        public string SetSessionLock(object GUIDUser, string sessionID)
        {
            using (var _ctx = new QuotationDataContext())
            {
                string procResult = null;
                var _result = _ctx.prc_LAB_Ins_LAB_SessionLock(GUIDUser.ToString(), sessionID, ref procResult);
                return procResult.TrimEnd();
            }
        }

        public string GetLock(int? lockTypeCode, int? IDRef, object GUIDUser, string sessionID)
        {
            using (var _ctx = new QuotationDataContext())
            {
                string procResult = null;
                var _result = _ctx.prc_LAB_Get_LAB_Lock(lockTypeCode, IDRef, GUIDUser.ToString(), sessionID,
                    ref procResult);
                return procResult.TrimEnd();
            }
        }

        public string DelLocks(object GUIDUser, string sessionID)
        {
            using (var _ctx = new QuotationDataContext())
            {
                string procResult = null;
                var _result = _ctx.prc_LAB_Del_LAB_Locks(GUIDUser.ToString(), sessionID, ref procResult);
                return procResult.TrimEnd();
            }
        }

        public string DelAllUserLocks(object GUIDUser, string sessionID)
        {
            using (var _ctx = new QuotationDataContext())
            {
                string procResult = null;
                var _result = _ctx.prc_LAB_Del_LAB_AllUserLocks(GUIDUser.ToString(), sessionID, ref procResult);
                return procResult.TrimEnd();
            }
        }
    }
}