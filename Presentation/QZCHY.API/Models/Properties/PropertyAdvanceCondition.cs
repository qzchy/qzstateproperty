using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    /// <summary>
    /// 高级搜索条件
    /// </summary>
    public class PropertyAdvanceConditionModel : BaseQMModel
    {
        public string ids { get; set; }
        public long Time { get; set; }

        public GovernmentFilterModel Government { get; set; }

        public ExtentModel Extent { get; set; }

        public PropertyTypeModel PropertyType { get; set; }

        public RegionModel Region { get; set; }

        public Certificate Certificate { get; set; }

        public CurrentModel Current { get; set; }

        public NextStepModel NextStep { get; set; }
        
        public RangeList ConstructArea { get; set; }

        public RangeList LandArea { get; set; }

        public RangeList Price { get; set; }

        public Range LifeTime { get; set; }

        public Range GetedDate { get; set; }

        public ExportModel Fields { get; set; }

        public string Query { get; set; }

        public string Sort { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool ShowHidden { get; set; }

    }

    public class GovernmentFilterModel
    {
        public bool Manage { get; set; }

        public bool IsGovernment { get; set; }

        public bool IsInstitution { get; set; }

        public bool IsCompany { get; set; }

        public int SelectedId { get; set; }
    }

    public class ExtentModel
    {
        public string Type { get; set; }

        public string Geo { get; set; }
    }

    public class PropertyTypeModel
    {
        public bool Construct { get; set; }

        public bool Land { get; set; }

        public bool ConstructOnLand { get; set; }
    }

    public class RegionModel
    {
        public bool Old { get; set; }

        public bool West { get; set; }

        public bool Jjq { get; set; }

        public bool Kc { get; set; }

        public bool Qj { get; set; }

        public bool Other { get; set; }
    }
 
    public class Certificate
    {
        public bool Both { get; set; }

        public bool Land { get; set; }

        public bool Construct { get; set; }

        public bool None { get; set; }
    }

    public class CurrentModel
    {
        public bool Self { get; set; }

        public bool Rent { get; set; }

        public bool Lend { get; set; }

        public bool Idle { get; set; }
    }

    public class NextStepModel
    {
        public int MapType { get; set; }

        public bool Auction { get; set; }

        public bool Ct { get; set; }

        public bool Jt { get; set; }

        public bool Jk { get; set; }

        public bool Self { get; set; }

        public bool StoreUp { get; set; }

        public bool Adjust { get; set; }

        public bool Greenland { get; set; }

        public bool House { get; set; }
    }

    public class Range
    {
        public double Min { get; set; }

        public double Max { get; set; }
    }

    public class RangeList
    {
        public IList<Range> Ranges { get; set; }
    }
}