using System;

namespace Eitrum.Networking {
	public class EiNetworkLobby : EiCore {

		private EiNetworkRoom[] rooms;

		public EiNetworkRoom[] Rooms {
			get {
				return rooms;
			}
		}
	}
}