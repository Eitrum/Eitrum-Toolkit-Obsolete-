using Eitrum.Engine.Core.Singleton;
using System;

namespace Eitrum.Engine.Core {
    public class UpdateSystem : EiComponentSingleton<UpdateSystem> {

        #region Singleton stuff

        protected override bool KeepAlive { get => true; set => base.KeepAlive = value; }

        #endregion

        #region Custom Classes

        [Serializable]
        public class TimerUpdateData {
            public IUpdate comp;
            public float timer;
            public float time;
            Action method;

            public TimerUpdateData(IUpdate comp, float time, Action method) {
                this.comp = comp;
                this.time = time;
                timer = time;
                this.method = method;
            }

            public void Update(float deltaTime) {
                timer -= deltaTime;
                if (timer <= 0f) {
                    timer += time;
                    method();
                }
            }
        }

        #endregion

        #region Variables

        EiLinkedList<TimerUpdateData> timerUpdateList = new EiLinkedList<TimerUpdateData>();
        EiLinkedList<IPreUpdate> preUpdateList = new EiLinkedList<IPreUpdate>();
        EiLinkedList<IUpdate> updateList = new EiLinkedList<IUpdate>();
        EiLinkedList<ILateUpdate> lateUpdateList = new EiLinkedList<ILateUpdate>();
        EiLinkedList<IFixedUpdate> fixedUpdateList = new EiLinkedList<IFixedUpdate>();

        static bool isRunningUnityThreadCallback = false;
        static EiLinkedList<EiUnityThreadCallbackInterface> unityThreadQueue = new EiLinkedList<EiUnityThreadCallbackInterface>();

        #endregion

        #region Core Update Loops

        void Update() {
            var time = UnityEngine.Time.deltaTime;

            #region Property Event Unity Main Thread Call
            isRunningUnityThreadCallback = true;
            var unityThreadIterator = unityThreadQueue.GetIterator();
            EiLLNode<EiUnityThreadCallbackInterface> propertyEvent;
            while (unityThreadIterator.Next(out propertyEvent)) {
                propertyEvent.Value.UnityThreadOnChangeOnly();
            }
            unityThreadQueue.Clear();
            isRunningUnityThreadCallback = false;
            #endregion

            #region TimerUpdateList

            EiLLNode<TimerUpdateData> dataNode;
            var dataIterator = timerUpdateList.GetIterator();
            while (dataIterator.Next(out dataNode)) {
                if (dataNode.Value.comp.IsNull)
                    dataIterator.DestroyCurrent();
                else
                    dataNode.Value.Update(time);
            }

            #endregion

            #region Pre Update Loop

            EiLLNode<IPreUpdate> pre;
            var preiterator = preUpdateList.GetIterator();
            while (preiterator.Next(out pre)) {
                if (pre.Value.IsNull)
                    preiterator.DestroyCurrent();
                else
                    pre.Value.PreUpdateComponent(time);
            }

            #endregion

            #region Update Loop 

            EiLLNode<IUpdate> comp;
            var iterator = updateList.GetIterator();
            while (iterator.Next(out comp)) {
                if (comp.Value.IsNull)
                    iterator.DestroyCurrent();
                else
                    comp.Value.UpdateComponent(time);
            }

            #endregion

        }

        void LateUpdate() {
            EiLLNode<ILateUpdate> comp;
            var time = UnityEngine.Time.deltaTime;
            var iterator = lateUpdateList.GetIterator();
            while (iterator.Next(out comp)) {
                if (comp.Value.IsNull)
                    iterator.DestroyCurrent();
                else
                    comp.Value.LateUpdateComponent(time);
            }
        }

        void FixedUpdate() {
            EiLLNode<IFixedUpdate> comp;
            var time = UnityEngine.Time.fixedDeltaTime;
            var iterator = fixedUpdateList.GetIterator();
            while (iterator.Next(out comp)) {
                if (comp.Value.IsNull)
                    iterator.DestroyCurrent();
                else
                    comp.Value.FixedUpdateComponent(time);
            }
        }

        #endregion

        #region Subscribe Unsubscribe Update Timer

        public EiLLNode<TimerUpdateData> SubscribeUpdateTimer(IUpdate component, float repeatTime, Action method) {
            return timerUpdateList.Add(new TimerUpdateData(component, repeatTime, method));
        }

        public void UnsubscribeTimerUpdate(EiLLNode<TimerUpdateData> timerUpdateNode) {
            timerUpdateList.Remove(timerUpdateNode);
        }

        #endregion

        #region Subscribe/Unsubscribe

        public EiLLNode<IPreUpdate> SubscribePreUpdate(IPreUpdate component) {
            return preUpdateList.Add(component);
        }

        public EiLLNode<IUpdate> SubscribeUpdate(IUpdate component) {
            return updateList.Add(component);
        }

        public EiLLNode<ILateUpdate> SubscribeLateUpdate(ILateUpdate component) {
            return lateUpdateList.Add(component);
        }

        public EiLLNode<IFixedUpdate> SubscribeFixedUpdate(IFixedUpdate component) {
            return fixedUpdateList.Add(component);
        }

        public void UnsubscribePreUpdate(EiLLNode<IPreUpdate> component) {
            preUpdateList.Remove(component);
        }

        public void UnsubscribeUpdate(EiLLNode<IUpdate> component) {
            updateList.Remove(component);
        }

        public void UnsubscribeLateUpdate(EiLLNode<ILateUpdate> component) {
            lateUpdateList.Remove(component);
        }

        public void UnsubscribeFixedUpdate(EiLLNode<IFixedUpdate> component) {
            fixedUpdateList.Remove(component);
        }

        #endregion

        #region Unity Thread Queue

        public static bool AddUnityThreadCallbackToQueue(EiUnityThreadCallbackInterface propertyEvent) {
            lock (unityThreadQueue) {
                if (isRunningUnityThreadCallback)
                    return false;
                unityThreadQueue.Add(propertyEvent);
                return true;
            }
        }

        #endregion
    }
}