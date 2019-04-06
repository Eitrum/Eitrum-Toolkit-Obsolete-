using System;

namespace Eitrum.Threading
{
	public class EiCallUnityThread : EiCore, EiUnityThreadCallbackInterface
	{
		private Action method;

		public EiCallUnityThread (Action method)
		{
			this.method = method;
			while (!EiUpdateSystem.AddUnityThreadCallbackToQueue (this)) {

			}
		}

		public void UnityThreadOnChangeOnly ()
		{
			method ();
		}

		public static EiCallUnityThread New (Action method)
		{
			return new EiCallUnityThread (method);
		}
	}
}