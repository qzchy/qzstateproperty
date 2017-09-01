using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Web.Framework
{
    /// <summary>
    /// Web API
    /// </summary>
    public class ApiResponseResult
    {
        public string Message { get; set; }

        public int Code { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public int RequestTime { get; set; }
    }

    public class ApiResponseResult<T>:ApiResponseResult
    {
        public T Object { get; set; }
    }
}
