namespace MeetingManage.Helpers
{
    public class PageHelpers
    {
        private readonly IConfiguration _configuration;
        public PageHelpers(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        public (List<T>,PageModel) GetPage<T>(List<T> obj,int? CurrentPage)
        {            
            int pageSize = _configuration.GetValue<int>("PageSize");
            int currentPage = CurrentPage ?? 1;
            if (currentPage < 1)
                currentPage = 1;
            int totalRecords = obj.Count();
            var skip = (currentPage - 1) * pageSize;
            obj = obj.Skip(skip).Take(pageSize).ToList();
            PageModel pageModel = new PageModel
            {
                currentPage = currentPage,
                pageSizes = pageSize,
                totalRecords = totalRecords,
                totalPage = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(totalRecords) / Convert.ToDecimal(pageSize))),
            };
            return (obj,pageModel);
        }
        public class PageModel
        {
            //每頁數量
            public int pageSizes { get; set; }
            //總數量
            public int totalRecords { get; set; }
            //當前頁數
            public int currentPage { get; set; }
            //總頁數
            public int totalPage { get; set; }

            public void caculateTotalPage()
            {
                this.totalPage = (int)Math.Ceiling(this.totalRecords / (double)this.pageSizes);
            }
        }
    }
}
