using System;

namespace Eitrum.Networking
{
	public interface EiObserverInterface : EiBaseInterface
	{
		void Serialize (EiBuffer buffer, bool isWriting);
	}

	public class EiNetworkObservables : EiSerializeInterface<EiObserverInterface>
	{
		public EiNetworkObservables (EiObserverInterface observer) : base (observer)
		{
		}
	}
}