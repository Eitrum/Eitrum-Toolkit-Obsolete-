using System;
using UnityEngine;

namespace Eitrum.Networking.Internal
{
	public interface INetwork
	{
		#region Properties

		bool IsConnected{ get; }

		EiNetworkType NetworkType { get; }

		string NetworkTypeName{ get; }

		bool InServer{ get; }

		bool IsServer{ get; }

		bool IsClient{ get; }

		int ServerTime{ get; }

		bool NeedManualSynchronize{ get; }

		int SendRate{ set; }

		#endregion

		#region Startup Methods

		void Load (EiNetworkInternal networkInternal);

		void Connect ();

		void Disconnect ();

		void CreateServer (string name, int port, int maxPlayers, int password);

		void JoinServer (string address, int port, int password);

		#endregion

		#region Network Sync Methods

		void Instantiate (byte[] instantiateData);

		void Destroy (int viewId);

		void DestroyPlayerViews (int ownerId);

		void DestroyAll ();

		void RPC (byte[] rpcData, EiNetworkTarget target);

		void SerializeViews ();

		#endregion
	}
}