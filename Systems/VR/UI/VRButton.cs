using Eitrum.Engine.Core;
using Eitrum.Engine.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Eitrum.VR.UI {
	public class VRButton : EiComponent, VROnPointerHover, VROnPointerClick {

		#region Variables

		public Image image;

		public Color normal = new Color(1f, 1f, 1f, 1f);
		public Color onPointerClick = new Color(0.7f, 0.7f, 0.7f, 1f);
		public Color onPointerHover = new Color(0.8f, 0.8f, 0.8f, 1f);
		public Color disabled = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		public bool interactable = true;

		private bool isHover = false;
		public UnityEvent onClick;
		private Color lastColor;
		private Color newColor;
		private Color currentColor;
		private Coroutine transition;
		private float transitionTime = 0.2f;

		#endregion

		#region Properties

		public bool IsHover {
			get {
				return isHover;
			}
		}

		#endregion

		#region Core

		void Awake() {
			currentColor = normal;
		}

		void VROnPointerHover.OnPointerHover(VREventSystem eventSystem, VREventState state) {
			if (!interactable)
				return;
			isHover = state == VREventState.Update;
			if (state == VREventState.Begin)
				ApplyColorTransition(onPointerHover);
			if (state == VREventState.End)
				ApplyColorTransition(normal);
		}

		void VROnPointerClick.OnPointerClick(VREventSystem eventSystem) {
			if (!interactable)
				return;
			onClick.Invoke();
			ApplyClickTransition();
		}

		void ApplyClickTransition() {
			lastColor = currentColor;
			newColor = onPointerClick;
			Timer.Stop(transition);
			transition = Timer.Animate(transitionTime, Animation, () => ApplyColorTransition(normal));
		}

		void ApplyColorTransition(Color color) {
			lastColor = currentColor;
			newColor = color;
			Timer.Stop(transition);
			transition = Timer.Animate(transitionTime, Animation);
		}

		void Animation(float time) {
			currentColor = Color.Lerp(lastColor, newColor, time);
			UpdateGraphics();
		}

		void UpdateGraphics() {
			if (image)
				image.color = currentColor;
		}

		#endregion

		#region Editor
#if UNITY_EDITOR
		protected override void AttachComponents() {
			base.AttachComponents();
			image = GetComponent<Image>();
		}

		[ContextMenu("Align Box Collider")]
		private void AlignBoxCollider() {
			var boxCollider = GetComponent<BoxCollider>();
			var rect = transform.GetRectTransform();
			if (boxCollider && rect) {
				boxCollider.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1f);
				boxCollider.center = rect.GetCenter();
			}
		}

		[ContextMenu("Attach Box Collider")]
		private void AttachBoxCollider() {
			var boxCollider = transform.AddComponent<BoxCollider>();
			var rect = transform.GetRectTransform();
			if (rect) {
				boxCollider.size = new Vector3(rect.sizeDelta.x, rect.sizeDelta.y, 1f);
				boxCollider.center = rect.GetCenter();
			}
		}
#endif
		#endregion
	}
}