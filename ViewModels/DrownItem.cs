namespace MeetingManage.ViewModels
{
    public class DrownItem
    {
        public string Description { set; get; }
        public string Value { set; get; }

        public DrownItem(string d, string v)
        {
            Description = d;
            Value = v;
        }
    }
}
