using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Networking
{
	[AddComponentMenu ("Eitrum/Networking/Network View")]
	public class EiNetworkView : EiComponent
	{
		#region Variables

		private int viewId = -1;
		private EiNetworkPlayer owner = null;

		[SerializeField]
		[TypeFilter (typeof(EiNetworkObservableInterface))]
		private List<UnityEngine.Object> observableComponentObjects = new List<UnityEngine.Object> ();
		private List<EiNetworkObservableInterface> observableComponentInterfaces = new List<EiNetworkObservableInterface> ();
		private static Dictionary<int, EiNetworkView> networkViewDictionary = new Dictionary<int, EiNetworkView> ();

		#endregion

		#region Properties

		public int ViewId {
			get {
				return viewId;
			}
			set {
				if (viewId >= 0) {
					if (networkViewDictionary.ContainsKey (viewId)) {
						networkViewDictionary.Remove (viewId);
					}

				} 
				viewId = value;
				networkViewDictionary.Add (viewId, this);
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

		void Awake ()
		{
			LoadObservers ();
		}

		private void LoadObservers ()
		{
			for (int i = 0; i < observableComponentObjects.Count; i++) {
				AddObserver (observableComponentObjects as EiNetworkObservableInterface);
			}
		}

		void OnDestroy ()
		{
			if (networkViewDictionary.ContainsKey (viewId)) {
				networkViewDictionary.Remove (viewId);
			}
		}

		#endregion

		#region Network Observable

		public void AddObserver (EiNetworkObservableInterface observer)
		{
			if (observer != null)
				observableComponentInterfaces.Add (observer);
		}

		public void RemoveObserver (EiNetworkObservableInterface observer)
		{
			observableComponentInterfaces.Remove (observer);
		}

		public void OnSerializeView (EiBuffer buffer)
		{
			if (IsMine) {// write
				var count = observableComponentInterfaces.Count;
				for (int i = 0; i < count; i++) {
					observableComponentInterfaces [i].OnNetworkSerialize (buffer, true);
				}
			} else {//read
				var count = observableComponentInterfaces.Count;
				for (int i = 0; i < count; i++) {
					observableComponentInterfaces [i].OnNetworkSerialize (buffer, false);
				}
			}
		}

		#endregion

		#region Static Utilities

		public static EiNetworkView Find (int id)
		{
			if (networkViewDictionary.ContainsKey (id)) {
				return networkViewDictionary [id];
			}
			return null;
		}

		#endregion

		#if UNITY_EDITOR

		protected override void AttachComponents ()
		{
			base.AttachComponents ();
			observableComponentObjects = new List<UnityEngine.Object> ();
			var objs = GetComponents<EiNetworkObservableInterface> ();
			for (int i = 0; i < objs.Length; i++) {
				observableComponentObjects.Add (objs [i].Component);
			}
		}

		#endif
	}
}