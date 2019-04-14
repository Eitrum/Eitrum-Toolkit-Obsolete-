using Eitrum.Engine.Core;
using Eitrum.Engine.Core.Singleton;
using System.Threading;

namespace Eitrum.Engine.Threading {
    public class UnityThreading : EiComponentSingleton<UnityThreading> {

        #region Singleton

        protected override bool KeepAlive { get => true; set => base.KeepAlive = value; }

        #endregion

        #region Variables

        public static EiPropertyEvent<bool> CloseThreads = new EiPropertyEvent<bool>(false);
        public static bool gameRunning = true;

        static Thread mainThread = Thread.CurrentThread;

        #endregion

        #region Properties

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

        #endregion

        #region Unity Methods

        void OnDestroy() {
            gameRunning = false;
            CloseThreads.Value = true;
        }

        #endregion
    }
}

