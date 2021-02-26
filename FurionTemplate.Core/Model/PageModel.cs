using System.Collections.Generic;

namespace FurionTemplate.Core.Model
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int page { get; set; }


        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; } = 1;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int dataCount { get; set; } = 0;


        /// <summary>
        /// 是否第一页
        /// </summary>
        public bool IsFirstPage { get; set; } = true;

        /// <summary>
        /// 是否最后一页
        /// </summary>
        public bool IsLastPage { get; set; } = true;
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }

    }
}
