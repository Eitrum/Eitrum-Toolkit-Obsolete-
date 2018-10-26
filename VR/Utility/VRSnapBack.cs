using UnityEngine;

namespace Eitrum.VR.Utility {
	public class VRSnapBack : EiComponent {

		#region Variables

		[Header("Settings")]
		[SerializeField]
		private float duration = 0.2f;
		[SerializeField]
		private AnimationCurve animationCurve;

		[Header("Components")]
		public VRGrabbable grabbable;

		private Vector3 lastMovedPosition;
		private Quaternion lastMovedRotation;
		private bool hasStartPosition = false;
		private Vector3 startPosition;
		private Quaternion startRotation;
#pragma warning disable
		private Coroutine coroutine;

		#endregion

		#region Core

		private void Awake() {
			grabbable.OnGrabSubscribe(OnGrab);
			grabbable.OnReleaseSubscribe(OnRelease);
		}

		void OnGrab(VRGrab grab) {
			if (!hasStartPosition) {
				hasStartPosition = true;
				startPosition = this.transform.position;
				startRotation = this.transform.rotation;
			}
			EiTimer.Stop(coroutine);
		}

		void OnRelease(VRGrab grab) {
			lastMovedPosition = this.transform.position;
			lastMovedRotation = this.transform.rotation;
			coroutine = EiTimer.Animate(duration, AnimateBack, Reset);
		}

		void AnimateBack(float time) {
			if (this == null)
				return;
			this.transform.position = Vector3.LerpUnclamped(lastMovedPosition, startPosition, animationCurve.Evaluate(time));
			this.transform.rotation = Quaternion.SlerpUnclamped(lastMovedRotation, startRotation, animationCurve.Evaluate(time));
		}

		void Reset() {
			hasStartPosition = false;
			coroutine = null;
		}

		#endregion

		#region Editor
#if UNITY_EDITOR
		protected override void AttachComponents() {
			base.AttachComponents();
			grabbable = this.GetOrAddComponent<VRGrabbable>();
		}
#endif
		#endregion
	}
}
