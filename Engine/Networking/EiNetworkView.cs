using System;
using UnityEngine;
using System.Collections.Generic;
using Eitrum.Networking.Internal;
using Eitrum.Engine.Core;

namespace Eitrum.Networking {
    public sealed class EiNetworkView : EiComponent {
        #region Variables

        [SerializeField] [Readonly] private int viewId = 0;
        [SerializeField] [Readonly] private int ownerId = 0;
        [SerializeField] private List<EiNetworkObservables> observables;

        private Eitrum.Networking.Internal.EiNetworkInternal network;
        private Action<EiNetworkView> onUpdate;

        #endregion

        #region Properties

        public int ViewId {
            get {
                return viewId;
            }
        }

        public int OwnerId {
            get {
                return ownerId;
            }
        }

        public EiNetworkPlayer Owner {
            get {
                return network.GetPlayerById(ownerId);
            }
        }

        #endregion

        #region Subscribe

        public void Susbcribe(Action<EiNetworkView> method) {
            onUpdate += method;
        }

        public void Unsubscribe(Action<EiNetworkView> method) {
            if (onUpdate != null)
                onUpdate -= method;
        }

        #endregion

        #region Static Help

        public static EiNetworkView Find(int viewId) {
            return null;
        }

        public static void Set(EiNetworkView view, int viewId, int ownerId, Eitrum.Networking.Internal.EiNetworkInternal network) {
            view.viewId = viewId;
            view.ownerId = ownerId;
            view.network = network;
            view.onUpdate?.Invoke(view);
        }

        public static void SetViewId(EiNetworkView view, int viewId) {
            view.viewId = viewId;
            view.onUpdate?.Invoke(view);
        }

        public static void SetOwnerId(EiNetworkView view, int ownerId) {
            view.ownerId = ownerId;
            view.onUpdate?.Invoke(view);
        }

        public static void SetNetwork(EiNetworkView view, Eitrum.Networking.Internal.EiNetworkInternal network) {
            view.network = network;
            view.onUpdate?.Invoke(view);
        }

        #endregion
    }
}