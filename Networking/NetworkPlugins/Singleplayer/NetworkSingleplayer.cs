using System;

namespace Eitrum.Networking.Internal
{
	public class NetworkSingleplayer : INetwork
	{
		#region Variables

		private bool isConnected = false;
		private bool inServer = false;
		private EiNetworkInternal networkInternal;

		#endregion

		#region Properties

		bool INetwork.IsConnected {
			get {
				return isConnected;
			}
		}

		EiNetworkType INetwork.NetworkType {
			get {
				return EiNetworkType.Singleplayer;
			}
		}

		string INetwork.NetworkTypeName {
			get {
				return "Single Player";
			}
		}

		bool INetwork.IsServer {
			get {
				return true;
			}
		}

		bool INetwork.IsClient {
			get {
				return true;
			}
		}

		bool INetwork.InServer {
			get {
				return inServer;
			}
		}

		int INetwork.ServerTime {
			get {
				return (int)(UnityEngine.Time.time * 1000f);
			}
		}

		bool INetwork.NeedManualSynchronize {
			get {
				return false;
			}
		}

		int INetwork.SendRate {
			set {
				
			}
		}

		#endregion

		#region INetwork implementation

		void INetwork.Load (EiNetworkInternal networkInternal)
		{
			this.networkInternal = networkInternal;
		}

		void INetwork.Connect ()
		{
			isConnected = true;
			networkInternal.OnConnected ();
		}

		void INetwork.Disconnect ()
		{
			isConnected = false;
			networkInternal.OnDisconnected ();
		}

		void INetwork.CreateServer (string name, int port, int maxPlayers, int password)
		{
			inServer = true;
			var localPlayer = new EiNetworkPlayerInternal (networkInternal, "Local Player", 0);
			networkInternal.AssignLocalPlayer (localPlayer);
			networkInternal.OnCreatedServer (new EiNetworkServerInternal (networkInternal));
		}

		void INetwork.JoinServer (string address, int port, int password)
		{
			UnityEngine.Debug.LogError ("Can't join a server in single player");
		}

		#endregion

		#region Synchronization

		void INetwork.Instantiate (byte[] instantiateData)
		{
			networkInternal.Instantiate (instantiateData);
		}

		void INetwork.Destroy (int viewId)
		{

		}

		void INetwork.DestroyPlayerViews (int ownerId)
		{
			
		}

		void INetwork.DestroyAll ()
		{

		}

		void INetwork.RPC (byte[] rpcData, EiNetworkTarget target)
		{
			throw new NotImplementedException ();
		}

		void INetwork.SerializeViews ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}