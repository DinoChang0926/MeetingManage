namespace MeetingManage.ViewModels.Admin
{
    public class RoleTypeSelect
    {
        public RoleTypeSelect(string n,byte v)
        {
            RoleName = n;
            RoleValue = v;
        }
        public string RoleName { get; set; }
        public byte RoleValue { get; set; }
    }
}
