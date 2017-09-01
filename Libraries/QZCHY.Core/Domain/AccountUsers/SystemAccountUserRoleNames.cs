
namespace QZCHY.Core.Domain.AccountUsers
{
    public static partial class SystemAccountUserRoleNames
    {
        public static string Administrators { get { return "管理员"; } }

        public static string DataReviewer { get { return "数据查看员"; } }

        public static string GovAuditor { get { return "行政事业审批员"; } }

        public static string StateOwnerAuditor { get { return "国资企业审批员"; } }

        public static string ParentGovernmentorAuditor { get { return "主管部门审批员"; } }

        public static string Registered { get { return "注册单位"; } }

        public static string Guests { get { return "访客"; } }
    }
}