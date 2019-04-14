using System;
using UnityEngine;
using System.Collections.Generic;
using Eitrum.Networking.Internal;

namespace Eitrum.Networking {
    public abstract class EiNetwork {
        #region Variables

        // Debugging
        protected EiNetworkLogLevel logLevel;

        // The actualy networking
        protected INetwork network;

        // Default Connect settings
        protected int defaultPort = 7777;
        protected int defaultMaxPlayers = 1000;

        // Local data
        protected EiNetworkPlayerInternal localPlayer;
        protected EiNetworkServerInternal currentServer;
        protected List<EiNetworkPlayerInternal> playerList = new List<EiNetworkPlayerInternal>();

        // Server list when servers are available
        protected List<EiNetworkServerInternal> serverList = new List<EiNetworkServerInternal>();

        // View id and id generator
        protected Dictionary<int, EiNetworkView> allocatedViews = new Dictionary<int, EiNetworkView>();
        protected int allocateViewId = 0;
        protected Queue<int> freeViewId = new Queue<int>();

        // Shared buffer class for sending data
        protected EiBuffer buffer = new EiBuffer(true);

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

        public bool IsServer {
            get {
                return network.IsServer;
            }
        }

        public bool IsClient {
            get {
                return network.IsClient;
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

        public int AllocateViewId {
            get {
                if (freeViewId.Count > 0) {
                    return freeViewId.Dequeue();
                }
                return ++allocateViewId;
            }
        }

        public int ServerTime {
            get {
                return network.ServerTime;
            }
        }

        #endregion

        #region Base Connect Methods

        public void Connect() {
            if (network.IsConnected) {
                Debug.LogWarning("Can't connect when you already connected");
            }
            else {
                network.Connect();
            }
        }

        public void Disconnect() {
            if (network.IsConnected) {
                network.Disconnect();
            }
            else {
                Debug.LogWarning("Can't disconnect when you already disconnected");
            }
        }

        #endregion

        #region Create Server

        public void CreateServer() {
            string name = "ServerRandom" + Eitrum.Mathematics.EiRandom.Range(100000, 999999);
            network.CreateServer(name, defaultPort, defaultMaxPlayers, 0);
        }

        public void CreateServer(string name) {
            network.CreateServer(name, defaultPort, defaultMaxPlayers, 0);
        }

        public void CreateServer(string name, int port, int maxPlayers) {
            network.CreateServer(name, port, maxPlayers, 0);
        }

        public void CreateServer(string name, int port, int maxPlayers, int password) {
            network.CreateServer(name, port, maxPlayers, password);
        }

        #endregion

        #region Join Server

        public bool JoinServer() {
            int attempt = 0;
            EiNetworkServer server;
            do {
                server = Eitrum.Mathematics.EiRandom.Element(serverList);
                if (++attempt > 20)
                    break;
            } while (server.IsPasswordProtected);

            if (!server.IsPasswordProtected) {
                network.JoinServer(server.Address, server.Port, 0);
                return true;
            }
            return false;
        }

        public void JoinServer(EiNetworkServer server) {
            network.JoinServer(server.Address, server.Port, 0);
        }

        public void JoinServer(EiNetworkServer server, int password) {
            network.JoinServer(server.Address, server.Port, password);
        }

        public void JoinServer(string address) {
            network.JoinServer(address, defaultPort, 0);
        }

        public void JoinServer(string address, int port, int password) {
            network.JoinServer(address, port, password);
        }

        #endregion

        #region Name

        public void SetName(string name) {
            //assign name to local player
        }

        #endregion

        #region Server List

        public void UpdateServerList() {

        }

        #endregion

        #region Instantiate

        public void Instantiate(EiPrefab prefab) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.None);                              /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 13 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.Position);                          /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 25 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.PositionRotation);                  /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            buffer.Write(rotation);                                                     /*12*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 37 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, EiNetworkView parent) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.PositionRotationParent);            /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            buffer.Write(rotation);                                                     /*12*/
            buffer.Write(parent.ViewId);                                                    /*04*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 41 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, float scale) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.PositionRotationScale);         /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            buffer.Write(rotation);                                                     /*12*/
            buffer.Write(scale);                                                            /*04*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 41 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, float scale, EiNetworkView parent) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.PositionRotationScaleParent);       /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            buffer.Write(rotation);                                                     /*12*/
            buffer.Write(scale);                                                            /*04*/
            buffer.Write(parent.ViewId);                                                    /*04*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 45 bytes*/
        }

        public void Instantiate(EiPrefab prefab, Vector3 position, Quaternion rotation, Vector3 scale3d, EiNetworkView parent) {
            buffer.ClearBuffer();
            buffer.Write((byte)EiNetworkInstantiateMask.PositionRotationScale3DParent); /*01*/
            buffer.Write(prefab.Id);                                                    /*04*/
            buffer.Write(AllocateViewId);                                                   /*04*/
            buffer.Write(localPlayer.Id);                                                   /*04*/
            buffer.Write(position);                                                     /*12*/
            buffer.Write(rotation);                                                     /*12*/
            buffer.Write(scale3d);                                                          /*12*/
            buffer.Write(parent.ViewId);                                                    /*04*/
            network.Instantiate(buffer.GetWrittenBuffer());                             /*Total 53 bytes*/
        }

        #endregion

        #region Destroy

        public void Destroy(int viewId) {
            network.Destroy(viewId);
        }

        public void DestroyPlayerViews(int ownerId) {
            network.DestroyPlayerViews(ownerId);
        }

        public void DestroyAll() {
            network.DestroyAll();
        }

        #endregion

        #region Help Methods

        public EiNetworkPlayer GetPlayerById(int id) {
            for (int i = playerList.Count - 1; i >= 0; i--) {
                if (playerList[i].Id == id) {
                    return playerList[i];
                }
            }
            return null;
        }

        public EiNetworkPlayer GetPlayerByName(string name) {
            for (int i = playerList.Count - 1; i >= 0; i--) {
                if (playerList[i].Name == name) {
                    return playerList[i];
                }
            }
            return null;
        }

        public EiNetworkServer GetServerById(int id) {
            for (int i = serverList.Count - 1; i >= 0; i--) {
                if (serverList[i].Id == id) {
                    return serverList[i];
                }
            }
            return null;
        }

        public EiNetworkServer GetServerByName(string name) {
            for (int i = serverList.Count - 1; i >= 0; i--) {
                if (serverList[i].Name == name) {
                    return serverList[i];
                }
            }
            return null;
        }

        #endregion

        #region Static Creation

        public static EiNetwork Create(INetwork network, EiNetworkLogLevel logLevel = EiNetworkLogLevel.None) {
            var netInternal = new EiNetworkInternal(network);
            netInternal.logLevel = logLevel;
            return netInternal as EiNetwork;
        }

        public static EiNetwork Create(EiNetworkType type, EiNetworkLogLevel logLevel = EiNetworkLogLevel.None) {
            var netInternal = new EiNetworkInternal(type);
            netInternal.logLevel = logLevel;
            return netInternal as EiNetwork;
        }

        #endregion
    }
}