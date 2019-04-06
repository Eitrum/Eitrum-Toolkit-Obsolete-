using System;

namespace Eitrum
{
	public interface EiBufferInterface
	{
		void WriteTo (EiBuffer buffer);

		void ReadFrom (EiBuffer buffer);
	}
}

