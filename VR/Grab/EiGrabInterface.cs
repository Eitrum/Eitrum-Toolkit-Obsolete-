using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum.VR {
	public interface EiGrabInterface : IBase {
		bool OnGrab(VRGrab grab);
		void OnRelase(VRGrab grab);
		void OnGrabUpdate(VRGrab grab, float value, float time);
	}
}
