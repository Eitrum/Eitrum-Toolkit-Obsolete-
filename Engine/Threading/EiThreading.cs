using Eitrum.Engine.Threading;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Eitrum {
    public class EiThreading {
        List<Action> actions = new List<Action>();
        EiSyncronizedQueue<Action> addQueue = new EiSyncronizedQueue<Action>();
        EiSyncronizedQueue<Action> removeQueue = new EiSyncronizedQueue<Action>();

        long startOfFrameTime = long.MaxValue;
        long frameTimeDelta = long.MaxValue;

        bool WaitForTargetFrameRate = false;
        int TargetFrameRate = 20;

        bool IsActive = true;

        public EiThreading() {
            try {
                var utf = UnityThreading.Instance;
                UnityThreading.CloseThreads.Subscribe(SuspendWorkthread);
            }
            catch (Exception e) {
                UnityEngine.Debug.LogException(e);
            }
            Thread thread = new Thread(new ThreadStart(Update));
            thread.IsBackground = true;
            thread.Start();
        }

        public void AddAction(Action action) {
            addQueue.Enqueue(action);
        }

        public void RemoveAction(Action action) {
            removeQueue.Enqueue(action);
        }

        void Update() {
            while (IsActive) {
                startOfFrameTime = DateTime.UtcNow.Ticks;

                for (int i = actions.Count - 1; i >= 0; i--) {
                    try {
                        actions[i]();
                    }
                    catch (Exception e) {
                        actions.RemoveAt(i);
                        UnityEngine.Debug.LogException(e);
                    }
                }
                Action action = null;
                while ((action = removeQueue.Dequeue()) != null)
                    actions.Remove(action);

                while ((action = addQueue.Dequeue()) != null)
                    actions.Add(action);

                frameTimeDelta = DateTime.UtcNow.Ticks - startOfFrameTime;

                if (WaitForTargetFrameRate && TargetFrameRate > 0) {
                    var time = (1f / (float)TargetFrameRate) - GetDeltaTime();
                    var milsec = (int)(time * (1000f));
                    if (milsec > 0)
                        Thread.Sleep(milsec);
                }
            }
        }

        public void SuspendWorkthread(bool value = true) {
            IsActive = !value;
        }

        public long GetDeltaTimeInTicks() {
            return frameTimeDelta;
        }

        public float GetDeltaTime() {
            return (float)frameTimeDelta / (float)TimeSpan.TicksPerSecond;
        }
    }
}

