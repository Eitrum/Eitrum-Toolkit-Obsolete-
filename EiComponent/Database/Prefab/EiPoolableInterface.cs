using System;

namespace Eitrum
{
	public interface EiPoolableInterface : EiBaseInterface
	{
		void OnPoolInstantiate ();

		void OnPoolDestroy ();
	}

	[Serializable]
	public class EiPoolableComponent : EiSerializeInterface<EiPoolableInterface>
	{
		public EiPoolableComponent (EiPoolableInterface interf) : base (interf)
		{
		}
	}
}
