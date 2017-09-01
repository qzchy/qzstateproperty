using System.ComponentModel;

namespace QZCHY.Core.Domain.Properties
{
    public enum Region
    {
        [Description("老城区")]
        OldCity = 0,
        [Description("西区")]
        West = 1,
        [Description("集聚区")]
        Clusters = 2,
        [Description("柯城区")]
        KC = 3,
        [Description("衢江区")]
        QJ = 4,
        [Description("其他")]
        Others = 99
    }
}
