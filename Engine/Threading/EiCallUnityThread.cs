using Eitrum.Engine.Core;
using System;

namespace Eitrum.Engine.Threading {
    public class EiCallUnityThread : EiUnityThreadCallbackInterface {
        private Action method;

        public EiCallUnityThread(Action method) {
            this.method = method;
            while (!UpdateSystem.AddUnityThreadCallbackToQueue(this)) {

            }
        }

        public void UnityThreadOnChangeOnly() {
            method();
        }

        public static EiCallUnityThread New(Action method) {
            return new EiCallUnityThread(method);
        }
    }
}