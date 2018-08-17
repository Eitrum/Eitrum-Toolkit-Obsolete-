using System;
using System.Collections.Generic;

namespace Eitrum.Networking {
	public interface EiNetworkObservableInterface : EiBaseInterface {
		void OnNetworkSerialize(EiBuffer buffer, bool isWriting);
	}

	[Serializable]
	public class EiNetworkObservable : EiSerializeInterface<EiNetworkObservableInterface> {
		public EiNetworkObservable(EiNetworkObservableInterface interf) {
			this.component = interf.Component;
			this.targetInterface = interf;
		}

		public static EiNetworkObservable[] ConvertArray(EiNetworkObservableInterface[] interfaces) {
			EiNetworkObservable[] observables = new EiNetworkObservable[interfaces.Length];
			for (int i = 0; i < interfaces.Length; i++) {
				observables[i] = new EiNetworkObservable(interfaces[i]);
			}
			return observables;
		}
	}
}