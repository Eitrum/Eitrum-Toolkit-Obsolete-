using UnityEngine;

namespace Eitrum.VR.UI {
	public interface VROnPointerDrag : IBase {
		void OnPointerDrag(VREventSystem eventSystem, Vector3 delta, VREventState state);
	}
}
