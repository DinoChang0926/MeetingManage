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
        public static List<T> ToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList<T>();
        }

        public static IEnumerable<T> ToEnumerable<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
