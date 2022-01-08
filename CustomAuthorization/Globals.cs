namespace MeetingManage.CustomAuthorization
{
    public class Globals
    {      
        public enum RoleType : byte
        {
            Admin  =  99,
            UserManage = 1,
            User = 2,    
        }
        public enum ErrorCode: byte
        {
            未授權 = 1,
            閒置時間過長 = 2,
        }
    }
}
