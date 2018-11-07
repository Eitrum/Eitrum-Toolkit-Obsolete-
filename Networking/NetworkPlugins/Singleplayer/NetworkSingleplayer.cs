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

		void INetwork.Disconnect ()
		{
			isConnected = false;
		}

		void INetwork.Connect ()
		{
			isConnected = true;
		}

		void INetwork.CreateServer (string name, int port, int maxPlayers, int password)
		{
			inServer = true;
		}

		void INetwork.JoinServer (string name, int port, int password)
		{
			//Error ("Can't join a server in single player");
		}

		#endregion

		#region Synchronization

		void INetwork.Instantiate (byte[] instantiateData)
		{
			throw new NotImplementedException ();
		}

		void INetwork.Destroy (ushort viewId)
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