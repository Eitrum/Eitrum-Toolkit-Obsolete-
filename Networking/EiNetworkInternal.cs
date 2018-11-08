using System;
using UnityEngine;

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
			if (network != null && network.IsConnected) {
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
			}
		}

		public new void DestroyAll ()
		{

		}

		#endregion

		#region Callbacks

		public void OnConnected ()
		{
			Log ("Connected to service");
			Publish (new NetworkConnectedMessage ());
		}

		public void OnConnectedFailed (string error)
		{
			LogError ("Failed connecting to service: " + error);
			Publish (new NetworkConnectedFailedMessage (error));
		}

		public void OnDisconnected ()
		{
			Log ("Disconnected from service successfully");
			Publish (new NetworkDisconnectedMessage ());
		}

		public void OnDisconnectedError (string errorCode)
		{
			LogError ("Disconnected from service: " + errorCode);
			Publish (new NetworkDisconnectedMessage (errorCode));
		}

		public void OnCreatedServer (EiNetworkServerInternal serverInternal)
		{
			Log ("Server Created - " + serverInternal.Name);
			Publish (new NetworkServerCreatedMessage (serverInternal));
		}

		public void OnCreatedServerFailed (string errorCode)
		{
			LogError ("Server Failed to create: " + errorCode);
			Publish (new NetworkServerCreatedFailedMessage (errorCode));
		}

		public void OnJoinedServer (EiNetworkServerInternal serverInternal)
		{
			Log ("Joined server - " + serverInternal.Name);
			Publish (new NetworkServerJoinedMessage (serverInternal));
		}

		public void OnJoinedServerFailed (string errorCode)
		{
			Log ("Joined server failed: " + errorCode);
			Publish (new NetworkServerJoinedFailedMessage (errorCode));
		}

		public void OnLeftServer ()
		{
			Log ("Left Server successfully");
			Publish (new NetworkServerLeftMessage ());
		}

		#endregion

		#region Internal Add/Remove

		public void Add (EiNetworkPlayerInternal player)
		{
			
		}

		public void Add (EiNetworkServerInternal server)
		{

		}

		public void Update (EiNetworkPlayerInternal playerCopy)
		{

		}

		public void Update (EiNetworkServerInternal serverCopy)
		{

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