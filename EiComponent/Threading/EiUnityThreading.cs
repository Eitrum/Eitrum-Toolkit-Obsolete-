using System;
using System.Collections.Generic;
using System.Threading;

namespace Eitrum
{
	public class EiUnityThreading : EiComponentSingleton<EiUnityThreading>
	{
		public override void SingletonCreation ()
		{
			KeepAlive ();
		}

		public static EiPropertyEvent<bool> CloseThreads = new EiPropertyEvent<bool> (false);

		static Thread mainThread = Thread.CurrentThread;

		public static Thread MainThread {
			get {
				return mainThread;
			}
		}

		void OnDestroy ()
		{
			CloseThreads.Value = true;
		}
	}
}

