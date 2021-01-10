using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SecurityManager
{
	public class Audit : IDisposable
	{

		private static EventLog customLog = null;
		const string SourceName = "SecurityManager.Audit";
		const string LogName = "MySecTest";

		static Audit()
		{
			try
			{
				if (!EventLog.SourceExists(SourceName))
				{
					EventLog.CreateEventSource(SourceName, LogName);
				}
				customLog = new EventLog(LogName,
					Environment.MachineName, SourceName);
			}
			catch (Exception e)
			{
				customLog = null;
				Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
			}
		}


		public static void AuthenticationSuccess(string userName)
		{
			

			if (customLog != null)
			{
				string UserAuthenticationSuccess = AuditEvents.AuthenticationSuccess;
				string message = String.Format(UserAuthenticationSuccess, userName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthenticationSuccess));
			}
		}

		public static void AuthorizationSuccess(string userName, string serviceName)
		{
			//TO DO
			if (customLog != null)
			{
				string AuthorizationSuccess = AuditEvents.AuthorizationSuccess;
				string message = String.Format(AuthorizationSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthorizationSuccess));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
		/// <param name="reason">permission name</param>
		public static void AuthorizationFailed(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string AuthorizationFailed = AuditEvents.AuthorizationFailed;
				string message = String.Format(AuthorizationFailed, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthorizationFailed));
			}
		}

		public static void createDatabaseSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string createDatabaseSuccess = AuditEvents.createDatabaseSuccess;
				string message = String.Format(createDatabaseSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.createDatabaseSuccess));
			}
		}


		public static void createDatabaseFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string AuthorizationFailed = AuditEvents.createDatabaseFailure;
				string message = String.Format(AuthorizationFailed, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.createDatabaseFailure));
			}
		}


		public static void removeDataSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string removeDataSuccess = AuditEvents.removeDataSuccess;
				string message = String.Format(removeDataSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.removeDataSuccess));
			}
		}


		public static void removeDataFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string removeDataFailure = AuditEvents.removeDataFailure;
				string message = String.Format(removeDataFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.removeDataFailure));
			}
		}

		public static void removeDatabaseSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string removeDatabaseSuccess = AuditEvents.removeDatabaseSuccess;
				string message = String.Format(removeDatabaseSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.removeDatabaseSuccess));
			}
		}


		public static void removeDatabaseFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string removeDatabaseFailure = AuditEvents.removeDatabaseFailure;
				string message = String.Format(removeDatabaseFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.removeDatabaseFailure));
			}
		}

		public static void archivateDatabaseSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string archivateDatabaseSuccess = AuditEvents.archivateDatabaseSuccess;
				string message = String.Format(archivateDatabaseSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.archivateDatabaseSuccess));
			}
		}


		public static void archivateDatabaseFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string archivateDatabaseFailure = AuditEvents.archivateDatabaseFailure;
				string message = String.Format(archivateDatabaseFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.archivateDatabaseFailure));
			}
		}

		public static void addDataSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string addDataSuccess = AuditEvents.addDataSuccess;
				string message = String.Format(addDataSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.addDataSuccess));
			}
		}


		public static void addDataFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string addDataFailure = AuditEvents.addDataFailure;
				string message = String.Format(addDataFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.addDataFailure));
			}
		}

		public static void modifyDataSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string modifyDataSuccess = AuditEvents.modifyDataSuccess;
				string message = String.Format(modifyDataSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.modifyDataSuccess));
			}
		}


		public static void modifyDataFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string modifyDataFailure = AuditEvents.createDatabaseFailure;
				string message = String.Format(modifyDataFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.modifyDataFailure));
			}
		}

		public static void averageForRegionSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string averageForRegionSuccess = AuditEvents.averageForRegionSuccess;
				string message = String.Format(averageForRegionSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.averageForRegionSuccess));
			}
		}


		public static void averageForRegionFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string averageForRegionFailure = AuditEvents.averageForRegionFailure;
				string message = String.Format(averageForRegionFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.averageForRegionFailure));
			}
		}

		public static void averageForCitySuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string averageForCitySuccess = AuditEvents.averageForCitySuccess;
				string message = String.Format(averageForCitySuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.averageForCitySuccess));
			}
		}


		public static void averageForCityFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string averageForCityFailure = AuditEvents.averageForCityFailure;
				string message = String.Format(averageForCityFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.averageForCityFailure));
			}
		}

		public static void maxConsumerForRegionSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string maxConsumerForRegionSuccess = AuditEvents.maxConsumerForRegionSuccess;
				string message = String.Format(maxConsumerForRegionSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.maxConsumerForRegionSuccess));
			}
		}


		public static void maxConsumerForRegionFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string maxConsumerForRegionFailure = AuditEvents.maxConsumerForRegionFailure;
				string message = String.Format(maxConsumerForRegionFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.maxConsumerForRegionFailure));
			}
		}

		public static void loadDbSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string loadDbSuccess = AuditEvents.loadDbSuccess;
				string message = String.Format(loadDbSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.loadDbSuccess));
			}
		}


		public static void loadDbFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string loadDbFailure = AuditEvents.loadDbFailure;
				string message = String.Format(loadDbFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.loadDbFailure));
			}
		}

		public static void loadAllDatabasesSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string loadAllDatabasesSuccess = AuditEvents.loadAllDatabasesSuccess;
				string message = String.Format(loadAllDatabasesSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.loadAllDatabasesSuccess));
			}
		}


		public static void loadAllDatabasesFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string loadAllDatabasesFailure = AuditEvents.loadAllDatabasesFailure;
				string message = String.Format(loadAllDatabasesFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.loadAllDatabasesFailure));
			}
		}

		public static void UploadDatabaseSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string UploadDatabaseSuccess = AuditEvents.UploadDatabaseSuccess;
				string message = String.Format(UploadDatabaseSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UploadDatabaseSuccess));
			}
		}


		public static void UploadDatabaseFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string UploadDatabaseFailure = AuditEvents.UploadDatabaseFailure;
				string message = String.Format(UploadDatabaseFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.UploadDatabaseFailure));
			}
		}

		public static void DownloadDatabaseSuccess(string userName, string serviceName)
		{ 
			if (customLog != null)
			{
				string DownloadDatabaseSuccess = AuditEvents.DownloadDatabaseSuccess;
				string message = String.Format(DownloadDatabaseSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DownloadDatabaseSuccess));
			}
		}


		public static void DownloadDatabaseFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string DownloadDatabaseFailure = AuditEvents.DownloadDatabaseFailure;
				string message = String.Format(DownloadDatabaseFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.DownloadDatabaseFailure));
			}
		}

		public static void SendDataSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string SendDataSuccess = AuditEvents.SendDataSuccess;
				string message = String.Format(SendDataSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.SendDataSuccess));
			}
		}


		public static void SendDataFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string SendDataFailure = AuditEvents.SendDataFailure;
				string message = String.Format(SendDataFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.SendDataFailure));
			}
		}

		public static void ArchiveSuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string ArchiveSuccess = AuditEvents.ArchiveSuccess;
				string message = String.Format(ArchiveSuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ArchiveSuccess));
			}
		}


		public static void ArchiveFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string ArchiveFailure = AuditEvents.ArchiveFailure;
				string message = String.Format(ArchiveFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ArchiveFailure));
			}
		}

		public static void SendKeySuccess(string userName, string serviceName)
		{
			if (customLog != null)
			{
				string SendKeySuccess = AuditEvents.SendKeySuccess;
				string message = String.Format(SendKeySuccess, userName, serviceName);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.SendKeySuccess));
			}
		}


		public static void SendKeyFailure(string userName, string serviceName, string reason)
		{
			if (customLog != null)
			{
				string SendKeyFailure = AuditEvents.SendKeyFailure;
				string message = String.Format(SendKeyFailure, userName, serviceName, reason);
				customLog.WriteEntry(message);
			}
			else
			{
				throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.SendKeyFailure));
			}
		}

		public void Dispose()
		{
			if (customLog != null)
			{
				customLog.Dispose();
				customLog = null;
			}
		}
	}
}
