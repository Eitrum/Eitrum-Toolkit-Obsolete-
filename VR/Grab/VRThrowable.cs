using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {

	[AddComponentMenu("Eitrum/VR/Throwable")]
	public class VRThrowable : EiComponent, EiGrabInterface {

		#region Variables
		[Header("Grab Settings")]
		public bool setAsChildOfhand = false;
		public bool canSwitchHand = false;

		[Header("Position Settings")]
		public bool lerpPositionToCenter = false;
		public Vector3 grabOffset = Vector3.zero;
		public bool lerpRotationToCenter = false;
		public Vector3 grabRotationOffset = Vector3.zero;

		[Header("Throw Settings")]
		public float forceMultiplier = 2f;
		[Tooltip("Not Implemented Yet")]
		public bool advancedThrowingCalculations = false;

		[Header("Throw Step Settings")]
		public float recordStepInterval = 0.02f;
		[Range(1, 10)]
		public int recordStepKeyframes = 5;

		private Vector3 lastPosition;
		private Quaternion lastRotation;
		private float timeUntilNextRecord = 0f;
		private Keyframe[] keyframes;
		private int index = 0;

		private Vector3 onGrabLocation;
		private Quaternion onGrabRotation;
		private VRGrab lastGrabbedHand;

		#endregion

		#region Properties

		public bool CanBeGrabbed {
			get {
				return canSwitchHand || lastGrabbedHand == null;
			}
		}

		#endregion

		#region Core

#if !EITRUM_PERFORMANCE_MODE
		void Awake() {
			if (!Entity || !Entity.Body) {
				throw new System.Exception(string.Format("VR Throwable object ({0}) is missing Entity or Rigidbody Component", this.gameObject.name));
			}
			if(GetComponent<EiGrabInterface>() != this)
				throw new System.Exception(string.Format("VR Throwable object ({0}) is having other Grab Interface Component", this.gameObject.name));
		}
#endif

		#endregion

		#region Velocity Getters

		public Vector3 GetVelocity() {
			Vector3 velocity = Vector3.zero;
			if (index == 0)
				return velocity;

			if (index < recordStepKeyframes) {
				for (int i = 0; i < index; i++) {
					velocity += keyframes[i].deltaPosition;
				}
				return velocity * (forceMultiplier / recordStepInterval / (float)index);
			}

			for (int i = 0; i < recordStepKeyframes; i++) {
				velocity += keyframes[i].deltaPosition;
			}
			return velocity * (forceMultiplier / recordStepInterval / (float)recordStepKeyframes);
		}

		public Vector3 GetAngularVelocity() {
			Vector3 angVel = Vector3.zero;
			if (index == 0)
				return angVel;

			if (index < recordStepKeyframes) {
				for (int i = 0; i < index; i++) {
					angVel += keyframes[i].angularVel;
				}
				return angVel * (forceMultiplier / recordStepInterval / (float)index);
			}

			for (int i = 0; i < recordStepKeyframes; i++) {
				angVel += keyframes[i].angularVel;
			}
			return angVel * (forceMultiplier / recordStepInterval / (float)recordStepKeyframes);
		}

		#endregion

		#region Recording

		private void Record(Keyframe keyframe) {
			keyframes[(index++) % recordStepKeyframes] = keyframe;
		}

		private void Record() {
			var currentPosition = this.transform.position;
			var currentRotation = this.transform.rotation;
			var deltaPosition = currentPosition - lastPosition;
			var deltaRotation = currentRotation * Quaternion.Inverse(lastRotation);
			lastPosition = currentPosition;
			lastRotation = currentRotation;
			Record(new Keyframe(deltaPosition, deltaRotation));
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

			Entity.FreezePhysics();

			if (keyframes == null)
				keyframes = new Keyframe[recordStepKeyframes];
			else
				for (int i = 0; i < recordStepKeyframes; i++)
					keyframes[i] = new Keyframe();

			index = 0;
			timeUntilNextRecord = recordStepInterval;
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

			timeUntilNextRecord -= time;
			if (timeUntilNextRecord <= 0f) {
				timeUntilNextRecord += recordStepInterval;
				Record();
			}
		}

		void EiGrabInterface.OnRelase(VRGrab grab) {
			if (lastGrabbedHand != grab)
				return;
			lastGrabbedHand = null;

			if (setAsChildOfhand)
				Entity.ReleaseParent();

			Entity.UnfreezePhysics();
			Entity.Body.velocity = GetVelocity();
			Entity.Body.angularVelocity = GetAngularVelocity();
		}

		#endregion

		#region Record Data

		[System.Serializable]
		private struct Keyframe {
			public Vector3 deltaPosition;
			public Quaternion deltaRotation;
			public Vector3 angularVel;

			public Keyframe(Vector3 deltaPosition, Quaternion deltaRotation) {
				this.deltaPosition = deltaPosition;
				this.deltaRotation = deltaRotation;
				angularVel = deltaRotation.ToAngularVelocity();
			}
		}

		#endregion
	}
}