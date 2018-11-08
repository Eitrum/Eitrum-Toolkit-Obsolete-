using System;

namespace Eitrum.Networking
{
	public abstract class EiNetworkServer
	{
		#region Variables

		protected EiNetwork network;
		protected int id = 0;
		protected string name = "";

		protected string address = "";
		protected int port = 0;
		protected bool isPasswordProtected = false;

		#endregion

		#region Properties

		public int Id {
			get {
				return id;
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public string Address {
			get {
				return address;
			}
		}

		public int Port {
			get {
				return port;
			}
		}

		public bool IsPasswordProtected {
			get {
				return isPasswordProtected;
			}
		}

		#endregion
	}
}

namespace Eitrum.Networking.Internal
{
	public class EiNetworkServerInternal : EiNetworkServer
	{
		#region Properties


		#endregion

		#region Constructor

		public EiNetworkServerInternal (EiNetwork network)
		{
			this.network = network;
		}

		#endregion
	}
}