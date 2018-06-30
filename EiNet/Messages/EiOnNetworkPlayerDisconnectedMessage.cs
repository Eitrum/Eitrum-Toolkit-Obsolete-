using System;

namespace Eitrum.EiNet
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

