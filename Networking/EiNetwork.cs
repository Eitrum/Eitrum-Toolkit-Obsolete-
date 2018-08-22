using System;
using UnityEngine;
using Eitrum.Networking.Internal;

namespace Eitrum.Networking {
	public class EiNetwork : EiComponentSingleton<EiNetwork> {
		#region Singleton

		public override void SingletonCreation() {
			KeepAlive();
		}

		#endregion

		#region Properties

		public static EiNetworkRegion CurrentRegion {
			get {
				return EiNetworkInternal.currentRegion;
			}
		}

		public static EiNetworkRoom CurrentRoom {
			get {
				return EiNetworkInternal.currentRoom;
			}
		}

		public static EiNetworkLobby CurrentLobby {
			get {
				return EiNetworkInternal.currentLobby;
			}
		}

		public static EiNetworkRoom[] RoomList {
			get {
				return EiNetworkInternal.currentLobby.Rooms;
			}
		}

		public static EiNetworkPlayer LocalPlayer {
			get {
				return EiNetworkInternal.localPlayer;
			}
		}

		public static bool IsConnecting {
			get {
				return false;
			}
		}

		public static bool IsConnected {
			get {
				return false;
			}
		}

		public static float ServerTime {
			get {
				if (IsConnected && CurrentRoom != null)
					return EiNetworkInternal.serverTime;
				return -1f;
			}
		}

		#endregion
	}
}