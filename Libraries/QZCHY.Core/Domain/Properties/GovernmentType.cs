using System.ComponentModel;

namespace QZCHY.Core.Domain.Properties
{
    public enum GovernmentType
    {
        [Description("行政机关")]
        Government = 0,
        [Description("事业单位")]
        Institution = 1,
        [Description("企业")]
        Company =2,
        [Description("团体")]
        Group = 3
    }
}
