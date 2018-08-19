using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum.Networking.Internal {
	public class EiPhotonBridge : EiComponentSingleton<EiPhotonBridge> {
		public override void SingletonCreation() {
			EiNetworkInternal.requestRegionSwitch.AddRequestMethod(OnRequestRegionSwitch);
		}

		public void OnRequestRegionSwitch(EiNetworkRegion region) {

		}

		void OnRegionChanged(EiNetworkRegion region) {

		}
	}
}
