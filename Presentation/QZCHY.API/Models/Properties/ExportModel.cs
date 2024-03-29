﻿using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class ExportModel: BaseQMEntityModel
    {
        [Description("资产名称")]
        public bool isName { get; set; }
        public bool isGovernment { get; set; }
        public bool isPGovernment { get; set; }
        public bool isPropertyType { get; set; }
        public bool isGovernmentType { get; set; }
        public bool isRegion { get; set; }
        public bool isAddress { get; set; }
        public bool isConstructArea { get; set; }
        public bool isLandArea { get; set; }
        public bool isPropertyID { get; set; }
        public bool isPropertyNature { get; set; }
        public bool isLandNature { get; set; }
        public bool isPrice { get; set; }
        public bool isGetedDate { get; set; }
        public bool isLifeTime { get; set; }
        public bool isUsedPeople { get; set; }
        public bool isCurrentUse_Self { get; set; }
        public bool isCurrentUse_Rent { get; set; }
        public bool isCurrentUse_Lend { get; set; }
        public bool isCurrentUse_Idle { get; set; }
        public bool isNextStepUsage { get; set; }
        public bool isEstateId { get; set; }
        public bool isConstructId { get; set; }
        public bool isLandId { get; set; }
        public bool isHasConstructID { get; set; }
        public bool isHasLandID { get; set; }
        public bool isRent { get; set; }
        public bool isLend { get; set; }   
    }
}