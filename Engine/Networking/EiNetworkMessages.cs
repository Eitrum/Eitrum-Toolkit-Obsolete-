using System;

namespace Eitrum.Networking
{
	#region Connect Messages

	public class NetworkConnectedMessage
	{

	}

	public class NetworkConnectedFailedMessage
	{
		private string errorCode = "";

		public string ErrorCode {
			get {
				return errorCode;
			}
		}

		public NetworkConnectedFailedMessage (string errorCode)
		{
			this.errorCode = errorCode;
		}
	}

	public class NetworkDisconnectedMessage
	{
		private string errorCode = "";

		public bool IsError {
			get {
				return string.IsNullOrEmpty (errorCode);
			}
		}

		public string ErrorCode {
			get {
				return errorCode;
			}
		}

		public NetworkDisconnectedMessage ()
		{

		}

		public NetworkDisconnectedMessage (string errorCode)
		{
			this.errorCode = errorCode;
		}
	}

	public class NetworkServerCreatedMessage
	{
		private EiNetworkServer server;

		public EiNetworkServer Server {
			get {
				return server;
			}
		}

		public NetworkServerCreatedMessage (EiNetworkServer server)
		{
			this.server = server;
		}
	}

	public class NetworkServerCreatedFailedMessage
	{
		private string errorCode = "";

		public string ErrorCode {
			get {
				return errorCode;
			}
		}

		public NetworkServerCreatedFailedMessage (string errorCode)
		{
			this.errorCode = errorCode;
		}
	}

	public class NetworkServerJoinedMessage
	{
		private EiNetworkServer server;

		public EiNetworkServer Server {
			get {
				return server;
			}
		}

		public NetworkServerJoinedMessage (EiNetworkServer server)
		{
			this.server = server;
		}
	}

	public class NetworkServerJoinedFailedMessage
	{
		private string errorCode = "";

		public string ErrorCode {
			get {
				return errorCode;
			}
		}

		public NetworkServerJoinedFailedMessage (string errorCode)
		{
			this.errorCode = errorCode;
		}
	}

	public class NetworkServerLeftMessage
	{

	}

	#endregion
}