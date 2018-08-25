using System;
using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {
	[AddComponentMenu("Eitrum/VR/Grabbable")]
	public class VRGrabbable : EiComponent, EiGrabInterface {

		#region Variables

		[Header("Settings")]
		public bool setAsChildOfhand = false;
		public bool canSwitchHand = false;

		[Header("Position Settings")]
		public bool lerpPositionToCenter = false;
		public Vector3 grabOffset = Vector3.zero;
		public bool lerpRotationToCenter = false;
		public Vector3 grabRotationOffset = Vector3.zero;

		private Vector3 onGrabLocation;
		private Quaternion onGrabRotation;
		private VRGrab lastGrabbedHand;

		#region Events

		private Action<VRGrab> onGrab;
		private Action<VRGrab, float, float> onGrabUpdate;
		private Action<VRGrab> onRelease;

		#endregion

		#endregion

		#region Properties

		public bool CanBeGrabbed {
			get {
				return canSwitchHand || lastGrabbedHand == null;
			}
		}

		#endregion

		#region Grab Interface

		bool EiGrabInterface.OnGrab(VRGrab grab) {
			if (!CanBeGrabbed)
				return false;
			lastGrabbedHand = grab;
			if (setAsChildOfhand) {
				Entity.SetParent(grab.transform);
				onGrabLocation = this.transform.localPosition;
				onGrabRotation = this.transform.localRotation;
			}
			else {
				onGrabLocation = Quaternion.Inverse(grab.transform.rotation) * (this.transform.position - grab.transform.position);
				onGrabRotation = Quaternion.Inverse(grab.transform.rotation) * this.transform.rotation;
			}
			if (onGrab != null)
				onGrab(grab);
			return true;
		}

		void EiGrabInterface.OnGrabUpdate(VRGrab grab, float value, float time) {
			if (lastGrabbedHand != grab)
				return;
			if (setAsChildOfhand) {
				if (lerpPositionToCenter)
					this.transform.localPosition = Vector3.Lerp(onGrabLocation, grabOffset, value);
				if (lerpRotationToCenter)
					this.transform.localRotation = Quaternion.Slerp(onGrabRotation, Quaternion.Euler(grabRotationOffset), value);
			}
			else {
				this.transform.position = grab.transform.position + (grab.transform.rotation) * (lerpPositionToCenter ? Vector3.Lerp(onGrabLocation, grabOffset, value) : onGrabLocation);
				this.transform.rotation = grab.transform.rotation * (lerpRotationToCenter ? Quaternion.Slerp(onGrabRotation, Quaternion.Euler(grabRotationOffset), value) : onGrabRotation);
			}
			if (onGrabUpdate != null)
				onGrabUpdate(grab, value, time);
		}

		void EiGrabInterface.OnRelase(VRGrab grab) {
			if (lastGrabbedHand != grab)
				return;
			lastGrabbedHand = null;

			if (setAsChildOfhand)
				Entity.ReleaseParent();

			if (onRelease != null)
				onRelease(grab);
		}

		#endregion

		#region Subscribe

		public void OnGrabSubscribe(Action<VRGrab> onGrab) {
			this.onGrab += onGrab;
		}

		public void OnGrabUnsubscribe(Action<VRGrab> onGrab) {
			if (this.onGrab != null)
				this.onGrab -= onGrab;
		}

		public void OnGrabUpdateSubscribe(Action<VRGrab, float, float> onGrabUpdate) {
			this.onGrabUpdate += onGrabUpdate;
		}

		public void OnGrabUpdateUnsubscribe(Action<VRGrab, float, float> onGrabUpdate) {
			if (this.onGrabUpdate != null)
				this.onGrabUpdate -= onGrabUpdate;
		}

		public void OnReleaseSubscribe(Action<VRGrab> onRelease) {
			this.onRelease += onRelease;
		}

		public void OnReleaseUnsubscribe(Action<VRGrab> onRelease) {
			if (this.onRelease != null)
				this.onRelease -= onRelease;
		}

		#endregion
	}
}