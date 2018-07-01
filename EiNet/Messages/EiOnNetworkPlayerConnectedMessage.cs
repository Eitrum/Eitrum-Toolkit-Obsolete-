using System;

namespace Eitrum.Networking
{
	public class EiOnNetworkPlayerConnectedMessage : EiCore
	{
		private EiNetworkPlayer player;

		public EiNetworkPlayer Player {
			get {
				return player;
			}
		}

		public EiOnNetworkPlayerConnectedMessage (EiNetworkPlayer player)
		{
			this.player = player;
		}
	}
}

