using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{
    public class PropertyApproveListModel: BaseQMEntityModel
    {

        public string ApproveType { get; set; }

        public string ProcessDate { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public int Property_Id { get; set; }

        /// <summary>
        /// 是否拥有审批权限
        /// </summary>
        public bool CanApprove { get; set; }

        /// <summary>
        /// 是否可以删除和修改
        /// </summary>
        public bool CanEditAndDelete { get; set; }
    }
}