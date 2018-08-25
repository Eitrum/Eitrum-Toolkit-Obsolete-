using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {

	[AddComponentMenu("Eitrum/VR/Tracker")]
	public class VRTracker : EiComponent {

		#region Variables
		[Header("Settings")]
		[SerializeField]
		private XRNode trackingNode = XRNode.Head;

		#endregion

		#region Properties

		public XRNode TrackingNode {
			get {
				return trackingNode;
			}
		}

		#endregion

		#region Core

		private void Awake() {
			SubscribePreUpdate();
		}

		public override void PreUpdateComponent(float time) {
			if (!VRCore.HasTracking(trackingNode))
				return;
			this.transform.localPosition = InputTracking.GetLocalPosition(trackingNode);
			this.transform.localRotation = InputTracking.GetLocalRotation(trackingNode);
		}

		#endregion
	}
}
