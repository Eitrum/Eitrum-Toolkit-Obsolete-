using System;

namespace Eitrum
{
    //TODO rewrite this shit make it similar to Component Singleton
	public class EiClassSingleton<T> : EiClassSingleton where T : EiClassSingleton, new()
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

	public class EiClassSingleton : EiClass
	{
		public virtual void SingletonCreation ()
		{

		}
	}
}

