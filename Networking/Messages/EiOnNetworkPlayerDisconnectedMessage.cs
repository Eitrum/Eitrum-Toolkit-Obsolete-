using System;

namespace Eitrum.Networking
{
	public class EiOnNetworkPlayerDisconnectedMessage : EiCore
	{
		private EiNetworkPlayer player;

		public EiNetworkPlayer Player {
			get {
				return player;
			}
		}

		public EiOnNetworkPlayerDisconnectedMessage (EiNetworkPlayer player)
		{
			this.player = player;
		}
	}
}

