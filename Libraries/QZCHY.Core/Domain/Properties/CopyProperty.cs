using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace QZCHY.Core.Domain.Properties
{
    public class CopyProperty : BaseEntity
    {
        public CopyProperty()
        {
        }  
        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// 资产所处区域
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 建筑面积
        /// </summary>
        public double ConstructArea { get; set; }

        /// <summary>
        /// 土地面积
        /// </summary>
        public double LandArea { get; set; }

        /// <summary>
        /// 产权证号
        /// </summary>
        public string PropertyID { get; set; }

        /// <summary>
        /// 拥有房产证
        /// </summary>
        public bool HasConstructID { get; set; }

        /// <summary>
        /// 拥有土地证
        /// </summary>
        public bool HasLandID { get; set; }

        /// <summary>
        /// 房产性质
        /// </summary>
        public string PropertyNature { get; set; }

        /// <summary>
        /// 土地性质
        /// </summary>
        public string LandNature { get; set; }

        /// <summary>
        /// 账面价格，单位万元
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 取得时间
        /// </summary>
        public DateTime? GetedDate { get; set; }

        /// <summary>
        /// 使用年限
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// 使用方
        /// </summary>
        public string UsedPeople { get; set; }

        /// <summary>
        /// 自用面积
        /// </summary>
        public double CurrentUse_Self { get; set; }

        /// <summary>
        /// 出租面积
        /// </summary>
        public double CurrentUse_Rent { get; set; }

        /// <summary>
        /// 出借面积
        /// </summary>
        public double CurrentUse_Lend { get; set; }

        /// <summary>
        /// 闲置面积
        /// </summary>
        public double CurrentUse_Idle { get; set; }

        /// <summary>
        /// 下步使用或处置建议
        /// </summary>
        public NextStepType NextStepUsage { get; set; }

        /// <summary>
        /// 坐落位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 范围，房产不需要
        /// </summary>
        public string Extent { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 资产所有方id
        /// </summary>
        public int Government_Id { get; set; }

        /// <summary>
        /// 不动产证
        /// </summary>
        public string EstateId { get; set; }

        /// <summary>
        /// 房产证
        /// </summary>
        public string ConstructId { get; set; }

        /// <summary>
        /// 土地证号
        /// </summary>
        public string LandId { get; set;}
   
        /// <summary>
        /// 
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// 是否核销
        /// </summary>
        public bool Off { get; set; }
        /// <summary>
        /// 资产id
        /// </summary>
        public int Property_Id { get; set; }             

        public string PrictureIds { get; set; }

        public string FileIds { get; set; }

        public int LogoPicture_Id { get; set; }
    }
}
