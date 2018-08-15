using System;
using UnityEngine;
using Eitrum.Networking.Internal;

namespace Eitrum.Networking
{
	public class EiNetwork
	{
		#region Singleton

		private static EiNetwork instance;

		private static EiNetwork Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EiNetwork();
				}
				return instance;
			}
		}

		#endregion
		
		#region Properties

		public static EiNetworkRegion CurrentRegion
		{
			get
			{
				return EiNetworkInternal.currentRegion;
			}
		}

		public static EiNetworkRoom CurrentRoom
		{
			get
			{
				return EiNetworkInternal.currentRoom;
			}
		}

		public static EiNetworkLobby CurrentLobby
		{
			get
			{
				return EiNetworkInternal.currentLobby;
			}
		}

		public static EiNetworkRoom[] RoomList
		{
			get
			{
				return EiNetworkInternal.roomsInLobby;
			}
		}

		public static EiNetworkPlayer LocalPlayer
		{
			get
			{
				return EiNetworkInternal.localPlayer;
			}
		}

		public static bool IsConnecting
		{
			get
			{
				return false;
			}
		}

		public static bool IsConnected
		{
			get
			{
				return false;
			}
		}

		public static float ServerTime
		{
			get
			{
				if (IsConnected && CurrentRoom != null)
					return EiNetworkInternal.serverTime;
				return -1f;
			}
		}

		#endregion
	}
}