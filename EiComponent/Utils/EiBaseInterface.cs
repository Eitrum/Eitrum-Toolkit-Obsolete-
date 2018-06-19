using System;

namespace Eitrum
{
	public interface EiBaseInterface
	{
		EiComponent Component{ get; }

		EiCore Core{ get; }

		bool IsNull{ get; }
	}
}

