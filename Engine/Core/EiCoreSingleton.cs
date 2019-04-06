using System;

namespace Eitrum
{
	public class EiCoreSingleton<T> : EiCoreSingleton where T : EiCoreSingleton, new()
	{
		protected static T instance;

		public static T Instance {
			get {
				if (instance == null) {
					instance = new T ();
					instance.SingletonCreation ();
				}
				return instance;
			}
		}
	}

	public class EiCoreSingleton : EiCore
	{
		public virtual void SingletonCreation ()
		{

		}
	}
}

