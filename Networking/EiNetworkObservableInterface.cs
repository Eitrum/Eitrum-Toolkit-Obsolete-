using System;

namespace Eitrum.Networking
{
	public interface EiNetworkObservableInterface : EiBaseInterface
	{
		void OnNetworkSerialize (EiBuffer buffer, bool isWriting);
	}
}