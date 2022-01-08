namespace MeetingManage.Helpers
{
    public class PageModel
    {
        public class PageResult<T> where T : class
        {
            public List<T> dataList { get; }

            //每頁數量
            public int pageSizes { get; set; }
            //總數量
            public int totalRecords { get; set; }
            //當前頁數
            public int currentPage { get; set; }
            //總頁數
            public int totalPage { get; set; }

            public PageResult(List<T> theList)
            {
                dataList = theList;
            }

            public void caculateTotalPage()
            {
                this.totalPage = (int)Math.Ceiling(this.totalRecords / (double)this.pageSizes);
            }
        }
    }
}
