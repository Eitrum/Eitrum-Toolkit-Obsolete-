using Eitrum.Engine.Core;
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

		private Vector3 localPosition;
		private Quaternion localRotation;

		private EiBoolStack disabled = new EiBoolStack();

		#endregion

		#region Properties

		public XRNode TrackingNode {
			get {
				return trackingNode;
			}
		}

		public bool IsEnabled {
			get {
				return !disabled;
			}
		}

		public bool IsDisabled {
			get {
				return disabled;
			}
		}

		public Vector3 InputLocalPosition {
			get {
				return localPosition;
			}
		}

		public Vector3 TrackedWorldPosition {
			get {
				return this.transform.position;
			}
		}

		public Quaternion InputLocalRotation {
			get {
				return localRotation;
			}
		}

		public Quaternion TrackedWorldRotation {
			get {
				return this.transform.rotation;
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
			localPosition = InputTracking.GetLocalPosition(trackingNode);
			localRotation = InputTracking.GetLocalRotation(trackingNode);
			if (!disabled) {
				this.transform.localPosition = localPosition;
				this.transform.localRotation = localRotation;
			}
		}

		public void DisableTracking() {
			disabled++;
		}

		public void EnableTracking() {
			disabled--;
		}

		#endregion
	}
}