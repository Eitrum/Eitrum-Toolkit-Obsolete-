using UnityEngine;

namespace Eitrum.VR.UI {
	public interface VROnPointerDrag : EiBaseInterface {
		void OnPointerDrag(VREventSystem eventSystem, Vector3 delta, VREventState state);
	}
}
