using System;

namespace Eitrum
{
	public interface EiPreUpdateInterface : EiBaseInterface
	{
		void PreUpdateComponent (float time);
	}

	public interface EiLateUpdateInterface : EiBaseInterface
	{
		void LateUpdateComponent (float time);
	}

	public interface EiFixedUpdateInterface : EiBaseInterface
	{
		void FixedUpdateComponent (float time);
	}

	public interface EiThreadedUpdateInterface : EiBaseInterface
	{
		void ThreadedUpdateComponent (float time);
	}

	public interface EiUpdateInterface : EiBaseInterface
	{
		void UpdateComponent (float time);
	}
}