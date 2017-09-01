using System.ComponentModel;

namespace QZCHY.Core.Domain.Properties
{
    /// <summary>
    /// 资产类别
    /// </summary>
    public enum PropertyType
    {
        [Description("房屋")]
        House = 0,   //
        [Description("土地")]
        Land = 1,     //
        [Description("对应房屋土地")]
        LandUnderHouse = 2, //
        [Description("其他")]
        Others = 99 
    }
}
