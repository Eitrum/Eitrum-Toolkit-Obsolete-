using System;
using UnityEngine;
using Eitrum.Networking.Internal;
using System.Collections.Generic;

namespace Eitrum.Networking
{
	public abstract class EiNetwork
	{
		#region Variables

		protected INetwork network;
		protected int defaultPort = 7777;
		protected int defaultMaxPlayers = 1000;

		protected EiNetworkPlayerInternal localPlayer;
		protected EiNetworkServerInternal currentServer;

		protected List<EiNetworkPlayerInternal> playerList = new List<EiNetworkPlayerInternal> ();
		protected List<EiNetworkServerInternal> serverList = new List<EiNetworkServerInternal> ();

		#endregion

		#region Properties

		public bool IsConnected {
			get {
				return network.IsConnected;
			}
		}

		public bool InServer {
			get {
				return network.InServer;
			}
		}

		public EiNetworkPlayer LocalPlayer {
			get {
				return localPlayer;
			}
		}

		public EiNetworkServer Server {
			get {
				return currentServer;
			}
		}

		#endregion

		#region Base Connect Methods

		public void Connect ()
		{
			//network.Connect();
		}

		public void Disconnect ()
		{
			network.Disconnect ();
		}

		#endregion

		#region Create Server

		public void CreateServer ()
		{
			//generate name, use it
			//network.CreateServer(name, defaultPort,defaultMaxPlayers, 0);
		}

		public void CreateServer (string name)
		{
			//network.CreateServer(name, defaultPort,defaultMaxPlayers, 0);
		}

		public void CreateServer (string name, int port, int maxPlayers)
		{
			//network.CreateServer(name, port, maxPlayers, 0);
		}

		public void CreateServer (string name, int port, int maxPlayers, int password)
		{
			//network.CreateServer(name, port, maxPlayers, password);
		}

		#endregion

		#region Join Server

		public void JoinServer ()
		{
			//take random server, join it
			//network.JoinServer(name, port, password);
		}

		public void JoinServer (string name)
		{
			//network.JoinServer(name, defaultPort, 0);
		}

		public void JoinServer (string name, int port, int password)
		{
			//network.JoinServer(name, port, password);
		}

		#endregion

		#region Name

		public void SetName (string name)
		{
			//assign name to local player
		}

		#endregion

		#region Server List

		public void UpdateServerList ()
		{

		}

		#endregion

		#region Static Creation

		public static EiNetwork Create (EiNetworkType type)
		{
			var netInternal = new EiNetworkInternal (type);
			return netInternal as EiNetwork;
		}

		#endregion
	}
}