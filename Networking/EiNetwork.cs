using System;
using UnityEngine;

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

		#region Variables

		private EiNetworkRegion currentRegion = EiNetworkRegion.None;
		private EiNetworkRoom currentRoom = null;
		private EiNetworkLobby currentLobby = null;
		private EiNetworkPlayer localPlayer = new EiNetworkPlayer();
		private EiNetworkRoom[] roomsInLobby = new EiNetworkRoom[0];
		private float serverTime = 0f;

		private EiCallback onMasterServerCallback = new EiCallback();
		private EiCallback<EiNetworkRegion> onConnectedToRegion = new EiCallback<EiNetworkRegion>();
		private EiCallback<EiNetworkLobby> onJoinLobby = new EiCallback<EiNetworkLobby>();
		private EiCallback<EiNetworkRoom> onJoinRoom = new EiCallback<EiNetworkRoom>();
		private EiCallback<EiNetworkRoom> onCreatRoom = new EiCallback<EiNetworkRoom>();
		private EiCallback<EiNetworkRoom[]> onRoomListUpdated = new EiCallback<EiNetworkRoom[]>();
		private EiCallback onDisconnect = new EiCallback();

		#endregion

		#region Properties

		public static EiNetworkRegion CurrentRegion
		{
			get
			{
				return Instance.currentRegion;
			}
		}

		public static EiNetworkRoom CurrentRoom
		{
			get
			{
				return Instance.currentRoom;
			}
		}

		public static EiNetworkLobby CurrentLobby
		{
			get
			{
				return Instance.currentLobby;
			}
		}

		public static EiNetworkRoom[] RoomList
		{
			get
			{
				return Instance.roomsInLobby;
			}
		}

		public static EiNetworkPlayer LocalPlayer
		{
			get
			{
				return Instance.localPlayer;
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
					return Instance.serverTime;
				return -1f;
			}
		}

		#endregion

		#region Static Methods

		#region Name

		public static void AssignName(string name)
		{
			Instance.AssignName_Internal(name);
		}

		#endregion

		#region Server

		public static EiCallback ConnectToMasterServer()
		{

			return Instance.ConnectToMasterServer_Internal();
		}

		public static void Diconnect()
		{
			Instance.Diconnect_Internal();
		}

		#endregion

		#region Region

		public static EiCallback ConnectToRegion(EiNetworkRegion region)
		{
			EiCallback promise = new EiCallback();

			return promise;
		}

		#endregion

		#region Lobby

		public static EiCallback<EiNetworkLobby> ConnectToLobby(EiNetworkLobby lobby)
		{
			EiCallback<EiNetworkLobby> promise = new EiCallback<EiNetworkLobby>();

			return promise;
		}

		public static EiCallback ConnectToLobby(string lobbyName)
		{
			EiCallback promise = new EiCallback();

			return promise;
		}

		public static EiCallback LeaveLobby()
		{
			EiCallback promise = new EiCallback();

			return promise;
		}

		#endregion

		#region Room

		public static void ConnectToRoom(string roomName)
		{

		}

		public static void ConnectToRoom(EiNetworkRoom room)
		{

		}

		public static void CreateRoom(string roomName)
		{
			CreateRoom(new EiNetworkRoom(roomName));
		}

		public static void CreateRoom(EiNetworkRoom room)
		{

		}

		public static void CreateRoom(EiNetworkRoomOptions roomOptions)
		{
			CreateRoom(new EiNetworkRoom(roomOptions));
		}

		public static void LeaveRoom()
		{

		}

		#endregion

		#endregion

		#region Internal Local Methods

		#region Name

		private void AssignName_Internal(string name)
		{
			localPlayer.AssignName(name);
		}

		#endregion

		#region Server

		private EiCallback ConnectToMasterServer_Internal()
		{

			return onMasterServerCallback;
		}

		private void Diconnect_Internal()
		{

		}

		#endregion

		#region Region

		private EiCallback ConnectToRegion_Internal(EiNetworkRegion region)
		{

			return null;
		}

		#endregion

		#region Lobby

		private EiCallback<EiNetworkLobby> ConnectToLobby_Internal(EiNetworkLobby lobby)
		{

			return null;
		}

		private EiCallback ConnectToLobby_Internal(string lobbyName)
		{

			return null;
		}

		private EiCallback LeaveLobby_Internal()
		{

			return null;
		}

		#endregion

		#region Room

		private void ConnectToRoom_Internal(string roomName)
		{

		}

		private void ConnectToRoom_Internal(EiNetworkRoom room)
		{

		}

		private void CreateRoom_Internal(string roomName)
		{

		}

		private void CreateRoom_Internal(EiNetworkRoom room)
		{

		}

		private void CreateRoom_Internal(EiNetworkRoomOptions roomOptions)
		{

		}

		private void LeaveRoom_Internal()
		{

		}

		#endregion

		#endregion
	}
}