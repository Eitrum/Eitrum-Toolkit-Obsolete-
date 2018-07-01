using System;
using UnityEngine;

namespace Eitrum.Networking
{
	public class EiNetwork : EiCoreSingleton<EiNetwork>
	{
		private EiNetworkRegion networkRegion = EiNetworkRegion.None;

		private EiNetworkRoom currentRoom;
		private EiNetworkLobby currentLobby;
	}
}