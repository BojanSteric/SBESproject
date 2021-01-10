using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace SecurityManager
{
	public enum AuditEventTypes
	{
		AuthenticationSuccess = 0,
		AuthorizationSuccess = 1,
		AuthorizationFailed = 2,
		createDatabaseSuccess = 3,
		createDatabaseFailure = 4,
		removeDataSuccess = 5,
		removeDataFailure = 6,
		removeDatabaseSuccess = 7,
		removeDatabaseFailure = 8,
		archivateDatabaseSuccess = 9,
		archivateDatabaseFailure = 10,
		addDataSuccess = 11,
		addDataFailure = 12,
		modifyDataSuccess = 13,
		modifyDataFailure = 14,
		averageForRegionSuccess = 15,
		averageForRegionFailure = 16,
		averageForCitySuccess = 17,
		averageForCityFailure = 18,
		maxConsumerForRegionSuccess = 19,
		maxConsumerForRegionFailure = 20,
		loadDbSuccess = 21,
		loadDbFailure = 22,
		loadAllDatabasesSuccess = 23,
		loadAllDatabasesFailure = 24,
		UploadDatabaseSuccess = 25,
		UploadDatabaseFailure = 26,
		DownloadDatabaseSuccess = 27,
		DownloadDatabaseFailure = 28,
		SendDataSuccess = 29,
		SendDataFailure = 30,
		ArchiveSuccess = 31,
		ArchiveFailure = 32,
		SendKeySuccess = 33,
		SendKeyFailure = 34
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string AuthenticationSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
			}
		}

		public static string AuthorizationSuccess
		{
			get
			{	
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
			}
		}

		public static string AuthorizationFailed
		{
			get
			{		
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
			}
		}

		public static string createDatabaseSuccess
        {
			get
            {
				return ResourceMgr.GetString(AuditEventTypes.createDatabaseSuccess.ToString());
            }
        }

		public static string createDatabaseFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.createDatabaseFailure.ToString());
			}
		}

		public static string removeDataSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.removeDataSuccess.ToString());
			}
		}

		public static string removeDataFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.removeDataFailure.ToString());
			}
		}

		public static string removeDatabaseSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.removeDatabaseSuccess.ToString());
			}
		}

		public static string removeDatabaseFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.removeDatabaseFailure.ToString());
			}
		}

		public static string archivateDatabaseSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.archivateDatabaseSuccess.ToString());
			}
		}

		public static string archivateDatabaseFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.archivateDatabaseFailure.ToString());
			}
		}

		public static string addDataSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.addDataSuccess.ToString());
			}
		}

		public static string addDataFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.addDataFailure.ToString());
			}
		}

		public static string modifyDataSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.modifyDataSuccess.ToString());
			}
		}

		public static string modifyDataFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.modifyDataFailure.ToString());
			}
		}

		public static string averageForRegionSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.averageForRegionSuccess.ToString());
			}
		}

		public static string averageForRegionFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.averageForRegionFailure.ToString());
			}
		}

		public static string averageForCitySuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.averageForCitySuccess.ToString());
			}
		}

		public static string averageForCityFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.averageForCityFailure.ToString());
			}
		}

		public static string maxConsumerForRegionSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.maxConsumerForRegionSuccess.ToString());
			}
		}

		public static string maxConsumerForRegionFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.maxConsumerForRegionFailure.ToString());
			}
		}

		public static string loadDbSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.loadDbSuccess.ToString());
			}
		}

		public static string loadDbFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.loadDbFailure.ToString());
			}
		}

		public static string loadAllDatabasesSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.loadAllDatabasesSuccess.ToString());
			}
		}

		public static string loadAllDatabasesFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.loadAllDatabasesFailure.ToString());
			}
		}

		public static string UploadDatabaseSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UploadDatabaseSuccess.ToString());
			}
		}

		public static string UploadDatabaseFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UploadDatabaseFailure.ToString());
			}
		}

		public static string DownloadDatabaseSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.DownloadDatabaseSuccess.ToString());
			}
		}

		public static string DownloadDatabaseFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.DownloadDatabaseFailure.ToString());
			}
		}

		public static string SendDataSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SendDataSuccess.ToString());
			}
		}

		public static string SendDataFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SendDataFailure.ToString());
			}
		}

		public static string ArchiveSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ArchiveSuccess.ToString());
			}
		}

		public static string ArchiveFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ArchiveFailure.ToString());
			}
		}

		public static string SendKeySuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SendKeySuccess.ToString());
			}
		}

		public static string SendKeyFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SendKeyFailure.ToString());
			}
		}
	}
}
