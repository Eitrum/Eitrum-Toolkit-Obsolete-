using System;

namespace Eitrum.Networking
{
	public interface EiObserverInterface : IBase
	{
		void Serialize (EiBuffer buffer, bool isWriting);
	}

	[Serializable]
	public class EiNetworkObservables : EiSerializeInterface<EiObserverInterface>
	{
	}
}