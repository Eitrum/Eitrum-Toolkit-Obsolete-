using System;

namespace Eitrum
{
	public interface IPoolable : IBase
	{
		void OnInstantiate ();

		void OnDestroy ();
	}
}
