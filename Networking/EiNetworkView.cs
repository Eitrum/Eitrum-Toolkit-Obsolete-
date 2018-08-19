using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Networking {
	[AddComponentMenu("Eitrum/Networking/Network View")]
	public class EiNetworkView : EiComponent {
		#region Variables

		private int viewId = -1;
		private EiNetworkPlayer owner = null;

		[SerializeField]
		private List<EiNetworkObservable> networkObservables;

		private Dictionary<int, byte> rpcMethodLookup = new Dictionary<int, byte>();
		private List<Action<byte[]>> rpcMethods = new List<Action<byte[]>>();

		private static Dictionary<int, EiNetworkView> networkViewDictionary = new Dictionary<int, EiNetworkView>();

		#endregion

		#region Properties

		public int ViewId {
			get {
				return viewId;
			}
			set {
				if (viewId >= 0) {
					if (networkViewDictionary.ContainsKey(viewId)) {
						networkViewDictionary.Remove(viewId);
					}
				}
				viewId = value;
				networkViewDictionary.Add(viewId, this);
			}
		}

		public EiNetworkPlayer Owner {
			get {
				return owner;
			}
			set {
				owner = value;
			}
		}

		public bool IsMine {
			get {
				return owner.IsMine;
			}
		}

		#endregion

		#region Core

		void OnDestroy() {
			if (networkViewDictionary.ContainsKey(viewId)) {
				networkViewDictionary.Remove(viewId);
			}
		}

		#endregion

		#region RPC

		public void SubscribeRPC(Action<byte[]> method) {
			var methodHash = method.GetHashCode();
			rpcMethodLookup.Add(methodHash, (byte)rpcMethods.Count);
			rpcMethods.Add(method);
		}

		public void RPC(Action<byte[]> method, byte[] data) {
			var key = method.GetHashCode();
			if (rpcMethodLookup.ContainsKey(key)) {
				var methodId = rpcMethodLookup[key];
				//Send to other clients
				method(data);
			}
			else {
				Debug.LogWarningFormat("{0} Method Key not found, can not send rpc to other clients", method.Method.Name);
			}
		}

		public void RPCPlayer(EiNetworkPlayer player, Action<byte[]> method, byte[] data) {
			var key = method.GetHashCode();
			if (rpcMethodLookup.ContainsKey(key)) {
				var methodId = rpcMethodLookup[key];
				//Send to other clients
				method(data);
			}
			else {
				Debug.LogWarningFormat("{0} Method Key not found, can not send rpc to other clients", method.Method.Name);
			}
		}

		#endregion

		#region Network Observable

		public void AddObserver(EiNetworkObservableInterface observer) {
			if (observer != null)
				networkObservables.Add(new EiNetworkObservable(observer));
		}

		public void RemoveObserver(EiNetworkObservableInterface observer) {
			for (int i = 0; i < networkObservables.Count; i++) {
				if (networkObservables[i].Interface == observer) {
					networkObservables.RemoveAt(i);
					break;
				}
			}
		}

		public void OnSerializeView(EiBuffer buffer) {
			if (IsMine) {// write
				var count = networkObservables.Count;
				for (int i = 0; i < count; i++) {
					networkObservables[i].Interface.OnNetworkSerialize(buffer, true);
				}
			}
			else {//read
				var count = networkObservables.Count;
				for (int i = 0; i < count; i++) {
					networkObservables[i].Interface.OnNetworkSerialize(buffer, false);
				}
			}
		}

		#endregion

		#region Static Utilities

		public static EiNetworkView Find(int id) {
			if (networkViewDictionary.ContainsKey(id)) {
				return networkViewDictionary[id];
			}
			return null;
		}

		#endregion

#if UNITY_EDITOR

		protected override void AttachComponents() {
			base.AttachComponents();
			networkObservables = new List<EiNetworkObservable>(EiNetworkObservable.ConvertArray(GetComponentsInChildren<EiNetworkObservableInterface>()));
		}

#endif
	}
}