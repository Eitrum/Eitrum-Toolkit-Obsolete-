using System;
using UnityEngine;
using System.Collections.Generic;
using Eitrum.Networking.Plugin;
using Eitrum.Engine.Core;

namespace Eitrum.Networking.Internal
{
	public class EiNetworkInternal : EiNetwork
	{
		#region Properties

		public new int AllocateViewId {
			get {
				return base.AllocateViewId;
			}
			set {
				freeViewId.Clear ();
				allocateViewId = value;
			}
		}

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
				return;
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
			if (network != null && network.IsConnected && EiComponent.GameRunning) {
				network.Disconnect ();
			}
		}

		#endregion

		#region Instantiate

		public void Instantiate (byte[] data)
		{
			EiBuffer buffer = new EiBuffer (data);
			var unpack = (EiNetworkInstantiateMask)buffer.ReadByte ();
			var prefabId = buffer.ReadInt ();
			var viewId = buffer.ReadInt ();
			var ownerId = buffer.ReadInt ();
			EiPrefab prefab = Eitrum.Database.EiPrefabDatabase.Instance [prefabId];
		
			// Calculate Unpack type
			Vector3 position = unpack.HasFlag (EiNetworkInstantiateMask.Position) ? buffer.ReadVector3 () : Vector3.zero;
			Quaternion rotation = unpack.HasFlag (EiNetworkInstantiateMask.Rotation) ? buffer.ReadQuaternion () : Quaternion.identity;
			float scale = unpack.HasFlag (EiNetworkInstantiateMask.Scale) ? buffer.ReadFloat () : 1f;
			Vector3 scale3D = unpack.HasFlag (EiNetworkInstantiateMask.Scale3D) ? buffer.ReadVector3 () : new Vector3 (scale, scale, scale);
			Transform parent = null;
			if (unpack.HasFlag (EiNetworkInstantiateMask.Parent)) {
				var parentViewId = buffer.ReadInt ();
				var parentView = EiNetworkView.Find (parentViewId);
				if (parentView)
					parent = parentView.transform;
			}

			// Instantiate Prefab
			var go = prefab.Instantiate (position, rotation, scale3D, parent);
			var view = go.GetComponent<EiNetworkView> ();
			if (view) {
				EiNetworkView.SetViewId (view, viewId);
				EiNetworkView.SetOwnerId (view, ownerId);
				EiNetworkView.SetNetwork (view, this);
			}
		}

		#endregion

		#region Destroy

		public new void Destroy (int viewId)
		{
			var view = EiNetworkView.Find (viewId);
			if (view) {
				view.Destroy ();
				allocatedViews.Remove (viewId);
			}
		}

		public new void DestroyPlayerViews (int ownerId)
		{
			foreach (var netView in allocatedViews) {
				if (netView.Value.OwnerId == ownerId) {
					netView.Value.Destroy ();
					allocatedViews.Remove (netView.Key);
				}
			}
		}

		public new void DestroyAll ()
		{
			foreach (var netView in allocatedViews) {
				netView.Value?.Destroy ();
			}
			allocatedViews.Clear ();
		}

		#endregion

		#region Callbacks

		public void OnConnected ()
		{
			Log ("Connected to service");
			Message.Publish (new NetworkConnectedMessage ());
		}

		public void OnConnectedFailed (string error)
		{
			LogError ("Failed connecting to service: " + error);
			Message.Publish (new NetworkConnectedFailedMessage (error));
		}

		public void OnDisconnected ()
		{
			Log ("Disconnected from service successfully");
			Message.Publish (new NetworkDisconnectedMessage ());
			playerList.Clear ();
			serverList.Clear ();
			localPlayer = null;
		}

		public void OnDisconnectedError (string errorCode)
		{
			LogError ("Disconnected from service: " + errorCode);
            Message.Publish (new NetworkDisconnectedMessage (errorCode));
		}

		public void OnCreatedServer (EiNetworkServerInternal serverInternal)
		{
			Log ("Server Created - " + serverInternal.Name);
            Message.Publish (new NetworkServerCreatedMessage (serverInternal));
		}

		public void OnCreatedServerFailed (string errorCode)
		{
			LogError ("Server Failed to create: " + errorCode);
            Message.Publish (new NetworkServerCreatedFailedMessage (errorCode));
		}

		public void OnJoinedServer (EiNetworkServerInternal serverInternal)
		{
			Log ("Joined server - " + serverInternal.Name);
            Message.Publish (new NetworkServerJoinedMessage (serverInternal));
		}

		public void OnJoinedServerFailed (string errorCode)
		{
			Log ("Joined server failed: " + errorCode);
            Message.Publish (new NetworkServerJoinedFailedMessage (errorCode));
		}

		public void OnLeftServer ()
		{
			Log ("Left Server successfully");
            Message.Publish (new NetworkServerLeftMessage ());
			playerList.Clear ();
			serverList.Clear ();
			localPlayer = null;
		}

		#endregion

		#region Player Sync

		public void OnPlayerJoined (EiNetworkPlayerInternal player)
		{
			//Fix player joined setup
		}

		#endregion

		#region Internal Add/Remove

		public void AssignLocalPlayer (EiNetworkPlayerInternal player)
		{
			localPlayer = player;
			Add (player);
		}

		public void Add (EiNetworkPlayerInternal player)
		{
			bool hasPlayer = false;
			for (int i = playerList.Count - 1; i >= 0; i--) {
				if (playerList [i].Id == player.Id) {
					hasPlayer = true;
					break;
				}
			}
			if (hasPlayer) {
				LogError ("Already has a player with given id: " + player.Id);
			} else {
				playerList.Add (player);
			}
		}

		public void Remove (EiNetworkPlayerInternal player)
		{
			for (int i = playerList.Count - 1; i >= 0; i--) {
				if (playerList [i].Id == player.Id) {
					playerList.RemoveAt (i);
					Log ("Removed player with id: " + player.Id);
				}
			}
		}

		public void Add (EiNetworkServerInternal server)
		{
			bool hasServer = false;
			for (int i = serverList.Count - 1; i >= 0; i--) {
				if (serverList [i].Id == server.Id) {
					hasServer = true;
					break;
				}
			}
			if (hasServer) {
				LogError ("Already has a server with given id: " + server.Id);
			} else {
				serverList.Add (server);
			}
		}

		public void Remove (EiNetworkServerInternal server)
		{
			for (int i = serverList.Count - 1; i >= 0; i--) {
				if (serverList [i].Id == server.Id) {
					serverList.RemoveAt (i);
					Log ("Removed server with id: " + server.Id);
				}
			}
		}

		public void UpdateServerList (EiNetworkServerInternal[] servers)
		{
			List<bool> toRemove = new List<bool> ();
			for (int i = serverList.Count - 1; i >= 0; i--) {
				toRemove.Add (true);
			}
			for (int i = 0; i < servers.Length; i++) {
				bool hasServer = false;
				var server = servers [i];
				for (int si = serverList.Count - 1; si >= 0; si--) {
					if (serverList [si].Id == server.Id) {
						toRemove [si] = false;
						hasServer = true;
						break;
					}
				}
				if (!hasServer) {
					serverList.Add (server);
				}
			}
			for (int i = toRemove.Count - 1; i >= 0; i--) {
				if (toRemove [i]) {
					serverList.RemoveAt (i);
				}
			}
		}

		#endregion

		#region RPC

		public void ReceiveRPC (byte[] rpcData)
		{
			EiBuffer buffer = new EiBuffer (rpcData);
			var viewId = buffer.ReadInt ();
			var methodId = buffer.ReadByte ();
			var array = buffer.ReadByteArray (rpcData.Length - 5);
			var view = EiNetworkView.Find (viewId);
			if (view) {
				//view.ReceiveRPC(methodId, array);
			} else {
				Debug.LogWarning ("Received an RPC for a network View that does not exists");
			}
		}

		public void SendRPC (int viewId, byte methodId, EiNetworkTarget targets, byte[] rpcData)
		{
			buffer.ClearBuffer ();
			buffer.Write (viewId);
			buffer.Write (methodId);
			buffer.Write (rpcData);
			network.RPC (buffer.GetWrittenBuffer (), targets);
		}

		#endregion

		#region Logging

		[System.Diagnostics.Conditional ("UNITY_EDITOR")]
		public void Log (string message)
		{
			if (logLevel == EiNetworkLogLevel.Everything) {
				UnityEngine.Debug.Log (string.Format ("[TIME-{0}] - {1}", ServerTime, message));
			}
		}

		[System.Diagnostics.Conditional ("UNITY_EDITOR")]
		public void LogWarning (string message)
		{
			if (logLevel == EiNetworkLogLevel.Everything) {
				UnityEngine.Debug.LogWarning (string.Format ("[TIME-{0}] - {1}", ServerTime, message));
			}
		}

		[System.Diagnostics.Conditional ("UNITY_EDITOR")]
		public void LogError (string message)
		{
			if (logLevel != EiNetworkLogLevel.None) {
				UnityEngine.Debug.LogError (string.Format ("[TIME-{0}] - {1}", ServerTime, message));
			}
		}

		#endregion
	}
}