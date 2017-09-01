using System.Collections.Generic;

namespace QZCHY.Web.Framework.Response
{
    /// <summary>
    /// 列表相应
    /// </summary>
    public class ListResponse<T>
    {
        public long Time { get; set; }

        public Paging Paging { get; set; }

        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// 自定义统计模块
        /// </summary>
        public dynamic Statistics { get; set; }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    public class Paging
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Total { get; set; }

        /// <summary>
        /// 查询结果的数目
        /// </summary>
        public int FilterCount { get; set; }
    }
}
