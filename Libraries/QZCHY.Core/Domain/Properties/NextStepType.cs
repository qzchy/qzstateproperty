using System.ComponentModel;

namespace QZCHY.Core.Domain.Properties
{
    /// <summary>
    /// 处置建议类别
    /// </summary>
    public enum NextStepType
    {
        [Description("注入国资公司-城投")]
        InjectionCT =0,
        [Description("注入国资公司-交投")]
        InjectionJT =1,
        /// <summary>
        /// 国有收储
        /// </summary>
        [Description("国有收储")]
        Storeup = 4,
        /// <summary>
        /// 拍卖处置
        /// </summary>
        [Description("拍卖处置")]
        Auction = 2,
        /// <summary>
        /// 调剂使用
        /// </summary>
        [Description("调剂使用")]
        Adjust = 3,
        /// <summary>
        /// 公共绿地
        /// </summary>
        [Description("公共绿地")]
        Greenland = 6,
        /// <summary>
        /// 自用
        /// </summary>
        [Description("自用")]
        Self = 5,
        /// <summary>
        /// 注入国资公司
        /// </summary>
        [Description("保障性住房")]
        House = 7,
        [Description("注入国资公司-金控")]
        InjectionJK = 8

    }
}
