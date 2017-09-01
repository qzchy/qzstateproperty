using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
    public enum OffType
    {
        [Description("拍卖")]
        Auction = 0,
        [Description("拆迁")]
        Remove = 1,
        [Description("收储")]
        Storeup = 2,
        [Description("区域外划拨")]
        RegionOutSide = 3,
        [Description("征收安置")]
        Levy = 4,
        [Description("其他")]
        Others = 5
    }
}
