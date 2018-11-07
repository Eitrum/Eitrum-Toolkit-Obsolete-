using System;

namespace Eitrum.Networking.Internal
{
	public class EiNetworkInternal : EiNetwork, EiPreUpdateInterface
	{

		#region Variables

		#endregion

		#region Properties

		

		#endregion

		#region Constructor

		public EiNetworkInternal (INetwork network)
		{
			Load (network);
		}

		public EiNetworkInternal (EiNetworkType type)
		{
			switch (type) {
			case EiNetworkType.Singleplayer:
				Load (new NetworkSingleplayer ());
				break;
			}
			throw new ArgumentException ("The network type provided is not supported by current setup of network, please provide a supported network type");
		}

		void Load (INetwork network)
		{
			this.network = network;
			network.Load (this);
		}

		~EiNetworkInternal ()
		{
			if (network != null && network.IsConnected) {
				network.Disconnect ();
			}
		}

		#endregion

		#region EiPreUpdateInterface implementation

		void EiPreUpdateInterface.PreUpdateComponent (float time)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region EiBaseInterface implementation

		object EiBaseInterface.This {
			get {
				return this;
			}
		}

		bool EiBaseInterface.IsNull {
			get {
				return false;
			}
		}

		#endregion
	}
}