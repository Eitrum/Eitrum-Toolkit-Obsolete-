using UnityEngine;
using Eitrum.Mathematics;

namespace Eitrum.VR.UI {
	public class VRSlider : EiComponent {
		public VRSliderHandle handle;

		public Transform startPosition;
		public Transform endPosition;

		public EiPropertyEventFloat value = new EiPropertyEventFloat(0f, true);
		public EiTrigger<float> onEndEdit = new EiTrigger<float>();

		private void Awake() {
			if (!startPosition || !endPosition || !handle)
				throw new System.Exception(string.Format("VR Slider do not have everything implemented"));
			handle.InstantiateHandle(OnSliderDragged, OnEndEdit);
		}

		void OnSliderDragged(Vector3 pointerPosition) {
			Line line = new Line(startPosition.position, endPosition.position);
			value.Value = line.GetValueFromPointOnLine(pointerPosition);
			handle.transform.position = line.GetPointOnLine(value.Value);
		}

		void OnEndEdit() {
			onEndEdit.Trigger(value.Value);
		}
	}
}
