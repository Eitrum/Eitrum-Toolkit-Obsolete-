using Eitrum.Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum.VR.UI {
	public class VRSliderHandle : EiComponent, VROnPointerDrag {

		private Action<Vector3> onMoved;
		private Action onEndEdit;

		public void InstantiateHandle(Action<Vector3> onMoved, Action onEndEdit) {
			this.onMoved = onMoved;
			this.onEndEdit = onEndEdit;
		}

		void VROnPointerDrag.OnPointerDrag(VREventSystem eventSystem, Vector3 delta, VREventState state) {
			if (state == VREventState.End)
				onEndEdit();
			else if (onMoved != null)
				onMoved(eventSystem.PointerPosition);
		}
	}
}
