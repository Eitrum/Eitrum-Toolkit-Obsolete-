using System;

namespace Eitrum
{
	public interface IPreUpdate : IBase
	{
		void PreUpdateComponent (float time);
	}

	public interface ILateUpdate : IBase
	{
		void LateUpdateComponent (float time);
	}

	public interface IFixedUpdate : IBase
	{
		void FixedUpdateComponent (float time);
	}

	public interface IThreadedUpdate : IBase
	{
		void ThreadedUpdateComponent (float time);
	}

	public interface IUpdate : IBase
	{
		void UpdateComponent (float time);
	}
}