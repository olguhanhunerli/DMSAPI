using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Authorization
{
	public static class DmsRoles
	{
		public const string SUPER_ADMIN = "SUPER_ADMIN";
		public const string Admin = "Admin";
		public const string Editor = "Editor";
		public const string Approver = "Approver";
		public const string User = "User";
		public const string Viewer = "Viewer";
		public const string Auditor = "Auditor";
		public const string ExternalUser = "External User";
	}

	public static class RoleGroups
	{
		public const string SuperAdmins = DmsRoles.SUPER_ADMIN;

		public const string Admins = DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;

		public const string InternalRead =
			DmsRoles.Viewer + "," + DmsRoles.User + "," + DmsRoles.Editor + "," +
			DmsRoles.Approver + "," + DmsRoles.Auditor + "," +
			DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;

		public const string ReadWithExternal =
			DmsRoles.ExternalUser + "," + InternalRead;

		public const string ContentWrite =
			DmsRoles.Editor + "," + DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;

		public const string Approval =
			DmsRoles.Approver + "," + DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;

		public const string AuditRead =
			DmsRoles.Auditor + "," + DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;

		public const string MasterDataWrite =
			DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN;
		public const string ComplaintWriteRoles =
			DmsRoles.Admin + ", " + DmsRoles.SUPER_ADMIN + "," + DmsRoles.Approver;
	}

}
