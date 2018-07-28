using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum
{
	public interface EiPoolableInterface : EiBaseInterface
	{
		void OnPoolInstantiate();

		void OnPoolDestroy();
	}
}
