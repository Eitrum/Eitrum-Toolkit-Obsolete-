using UnityEngine;
using UnityEngine.XR;

namespace Eitrum.VR {
	public class VRPointer : EiComponent {

		#region Variables

		[Header("Settings")]
		[SerializeField]
		private float pointerRange = 20f;

		[Header("Visual Settings (Optional, Local Position)")]
		[SerializeField]
		private GameObject pointerTarget;
		[SerializeField]
		private LineRenderer lineRenderer;

		private bool didHit = false;
		private RaycastHit lastHit;

		private EiBoolStack isDisabled = new EiBoolStack();

		#endregion

		#region Properties

		public bool IsActive {
			get {
				return !isDisabled;
			}
		}

		public bool IsDisabled {
			get {
				return isDisabled;
			}
		}

		public bool Hit {
			get {
				return didHit;
			}
		}

		public GameObject HitObject {
			get {
				return lastHit.collider.gameObject;
			}
		}

		public float Distance {
			get {
				return lastHit.distance;
			}
		}

		public Vector3 WorldPosition {
			get {
				return lastHit.point;
			}
		}

		#endregion

		#region Core

		private void Awake() {
			SubscribePreUpdate();
			if (pointerTarget)
				pointerTarget.SetActive(didHit);
			if (lineRenderer)
				lineRenderer.enabled = didHit;
		}

		public override void PreUpdateComponent(float time) {
			if (isDisabled)
				return;
			var hit = this.transform.ToRay().Hit(out lastHit, pointerRange);
			if (hit != didHit) {
				didHit = hit;
				if (pointerTarget)
					pointerTarget.SetActive(didHit);
				if (lineRenderer)
					lineRenderer.enabled = didHit;
			}
			if (hit) {
				var localPosition = new Vector3(0, 0, lastHit.distance);
				if (lineRenderer)
					lineRenderer.SetPosition(1, localPosition);
				if (pointerTarget)
					pointerTarget.transform.localPosition = localPosition;
			}
		}

		#endregion

		#region Enable/Disable

		public void Enable() {
			isDisabled.Decrement();
		}

		public void Disable() {
			isDisabled.Increment();
			if (isDisabled)
				didHit = false;
		}

		public void SetActive(bool value) {
			isDisabled.Set(!value);
			if (isDisabled)
				didHit = false;
		}

		#endregion

		#region Editor

#if UNITY_EDITOR
		protected override void AttachComponents() {
			base.AttachComponents();
			if (lineRenderer == null)
				lineRenderer = GetComponentInChildren<LineRenderer>();
			if (pointerTarget == null) {
				var tran = transform.Find("Pointer");
				if (tran)
					pointerTarget = tran.gameObject;
			}
		}
#endif

		#endregion
	}
}
