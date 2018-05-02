using System;

namespace Eitrum.EiNet
{
	public interface EiNetInterface
	{
		void NetWriteTo (EiBuffer buffer);

		void NetReadFrom (EiBuffer buffer);

		int NetPackageSize{ get; }
	}
}

