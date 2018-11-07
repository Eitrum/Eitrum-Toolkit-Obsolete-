using System;

namespace Eitrum.Networking
{
	public abstract class EiNetworkServer
	{
		#region Variables

		protected EiNetwork network;


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