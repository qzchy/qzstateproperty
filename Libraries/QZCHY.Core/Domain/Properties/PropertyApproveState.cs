using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
    /// <summary>
    /// 资产处置状态
    /// </summary>
    public enum PropertyApproveState
    {
        [Description("临时保存")]
        Start = -1,
        [Description("主管部门审批")]
        DepartmentApprove = 0,
        [Description("管理员审批")]
        AdminApprove = 1,
        [Description("结束")]
        Finish = 2
    }
}
