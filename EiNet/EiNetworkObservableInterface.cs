using System;

namespace Eitrum.EiNet
{
	public interface EiNetworkObservableInterface : EiBaseInterface
	{
		void OnNetworkSerialize (EiBuffer buffer, bool isWriting);
	}
}