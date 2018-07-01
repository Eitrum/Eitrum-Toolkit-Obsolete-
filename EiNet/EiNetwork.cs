using System;
using UnityEngine;

namespace Eitrum.Networking
{
	public class EiNetwork : EiCoreSingleton<EiNetwork>
	{
		#region Variables

		private EiNetworkRoom currentRoom;
		private EiNetworkLobby currentLobby;

		#endregion

		#region Properties

		public EiNetworkRoom CurrentRoom {
			get {
				return currentRoom;
			}
		}

		public EiNetworkLobby CurrentLobby {
			get {
				return currentLobby;
			}
		}

		#endregion

		#region Static Methods

		#region Server

		public static EiCallback ConnectToMasterServer ()
		{
			EiCallback promise = new EiCallback ();


			return promise;
		}

		public static void Diconnect ()
		{

		}

		#endregion

		#region Region

		public static void ConnectToRegion (EiNetworkRegion region)
		{
			
		}

		#endregion

		#region Lobby

		public static void ConnectToLobby (EiNetworkLobby lobby)
		{

		}

		public static void ConnectToLobby (string lobbyName)
		{

		}

		public static void LeaveLobby ()
		{

		}

		#endregion

		#region Room

		public static void ConnectToRoom (string roomName)
		{

		}

		public static void ConnectToRoom (EiNetworkRoom room)
		{

		}

		public static void CreateRoom (string roomName)
		{
			CreateRoom (new EiNetworkRoom (roomName));
		}

		public static void CreateRoom (EiNetworkRoom room)
		{

		}

		public static void CreateRoom (EiNetworkRoomOptions roomOptions)
		{
			CreateRoom (new EiNetworkRoom (roomOptions));
		}

		public static void LeaveRoom ()
		{
			
		}

		#endregion

		#endregion
	}
}