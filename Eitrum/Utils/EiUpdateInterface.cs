using System;

namespace Eitrum
{
	public interface EiUpdateInterface
	{
		void PreUpdateComponent (float time);

		void UpdateComponent (float time);

		void LateUpdateComponent (float time);

		void FixedUpdateComponent (float time);

		void ThreadedUpdateComponent (float time);
	}
}

