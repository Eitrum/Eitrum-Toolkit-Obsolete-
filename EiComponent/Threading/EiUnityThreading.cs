using System.Threading;

namespace Eitrum {
	public class EiUnityThreading : EiComponentSingleton<EiUnityThreading> {
		public override void SingletonCreation() {
			KeepAlive();
		}

		public static EiPropertyEvent<bool> CloseThreads = new EiPropertyEvent<bool>(false);
		public static bool gameRunning = true;

		static Thread mainThread = Thread.CurrentThread;

		public static Thread MainThread {
			get {
				return mainThread;
			}
		}

		public static bool IsMainThread {
			get {
				return Thread.CurrentThread == mainThread;
			}
		}

		void OnDestroy() {
			gameRunning = false;
			CloseThreads.Value = true;
		}
	}
}

