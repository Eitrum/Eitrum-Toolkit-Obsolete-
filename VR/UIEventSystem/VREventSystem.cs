using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.VR.UI {
	public enum VREventState {
		Begin,
		Update,
		End
	}

	public class VREventSystem : EiComponent {

		#region Variables

		[Header("Settings")]
		[SerializeField]
		private KeyCode inputKey = KeyCode.JoystickButton0;
		[Range(0f, 10f)]
		[SerializeField]
		private float dragThreshold = 0.2f;

		[Header("Components")]
		[SerializeField]
		private VRPointer pointer;

		private Vector3 oldPointerPosition;
		private GameObject currentActiveObject;
		private VROnPointerHover currentHoverObject;
		private VROnPointerDrag currentDragObject;
		private VROnPointerClick currentClickObject;
		private Vector3 startPosition;
		private bool dragActive = false;

		#endregion

		#region Properties

		public GameObject CurrentActiveObject {
			get {
				return currentActiveObject;
			}
		}

		#endregion

		#region Core

		private void Awake() {
#if !EITRUM_PERFORMANCE_MODE
			if(!pointer)
				throw new System.Exception(string.Format("{0} (VREventSystem) do not have a pointer attached", gameObject.name));
#endif
			SubscribePreUpdate();
		}

		public override void PreUpdateComponent(float time) {
			if (pointer.IsDisabled || !pointer.Hit) {
				if (currentHoverObject != null) {
					currentHoverObject.OnPointerHover(this, VREventState.End);
					currentHoverObject = null;
				}
				if (currentDragObject != null) {
					currentDragObject.OnPointerDrag(this, Vector3.zero, VREventState.End);
					currentDragObject = null;
				}
				currentClickObject = null;
				return;
			}
			var currentPosition = pointer.WorldPosition;
			var delta = currentPosition - oldPointerPosition;
			oldPointerPosition = currentPosition;

			var isKeyActive = Input.GetKey(inputKey);

			var hitObj = pointer.HitObject;
			if (hitObj != currentActiveObject) {
				if (currentDragObject == null) {
					if (!isKeyActive) {
						if (currentHoverObject != null)
							currentHoverObject.OnPointerHover(this, VREventState.End);
					}
					else {
						if (currentClickObject != null)
							currentClickObject = null;
					}
					currentHoverObject = hitObj.GetComponent<VROnPointerHover>();
					if (currentHoverObject != null)
						currentHoverObject.OnPointerHover(this, VREventState.Begin);
					currentActiveObject = hitObj;
				}
			}

			if (Input.GetKeyDown(inputKey)) {
				startPosition = currentPosition;
				if (currentHoverObject != null) {
					currentHoverObject.OnPointerHover(this, VREventState.End);
					currentHoverObject = null;
				}
				var pointerDown = currentActiveObject.GetComponent<VROnPointerDown>();
				if (pointerDown != null)
					pointerDown.OnPointerDown(this);


				currentClickObject = currentActiveObject.GetComponent<VROnPointerClick>();
				currentDragObject = currentActiveObject.GetComponent<VROnPointerDrag>();
			}

			if (Input.GetKeyUp(inputKey)) {
				if (currentClickObject != null) {
					currentClickObject.OnPointerClick(this);
					currentClickObject = null;
				}
				if (currentDragObject != null) {
					currentDragObject.OnPointerDrag(this, delta, VREventState.End);
					dragActive = false;
				}
				currentHoverObject = hitObj.GetComponent<VROnPointerHover>();
				if (currentHoverObject != null)
					currentHoverObject.OnPointerHover(this, VREventState.Begin);
			}
			if (isKeyActive) {
				if (!dragActive && Vector3.Distance(startPosition, currentPosition) >= dragThreshold) {
					dragActive = true;
					currentDragObject.OnPointerDrag(this, currentPosition - startPosition, VREventState.Begin);
				}
				else if (dragActive && currentDragObject != null)
					currentDragObject.OnPointerDrag(this, delta, VREventState.Update);
			}
			else {
				if (currentHoverObject != null)
					currentHoverObject.OnPointerHover(this, VREventState.Update);
			}
		}

		#endregion

		#region Editor
#if UNITY_EDITOR
		protected override void AttachComponents() {
			base.AttachComponents();
			pointer = GetComponent<VRPointer>();
		}
#endif
		#endregion
	}
}