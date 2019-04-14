using Eitrum.Engine.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eitrum.Database {
    public class EiPrefabPoolLoadBalancer : IUpdate {

        #region Optimized Singleton

        private static EiPrefabPoolLoadBalancer instance;

        public static EiPrefabPoolLoadBalancer Instance {
            get {
                if (instance == null)
                    instance = new EiPrefabPoolLoadBalancer();
                return instance;
            }
        }

        #endregion

        #region Data Struct

        struct Data {
            public IPrefabPool poolData;
            public int count;

            public Data(IPrefabPool target, int count) {
                this.poolData = target;
                this.count = count;
            }
        }

        #endregion

        #region Variables

        private int allocatedTicksPerFrame = 0;

        private List<Data> toInstantiate = new List<Data>();

        private EiLLNode<IUpdate> updateNode;
        private Stopwatch sw = new Stopwatch();

        #endregion

        #region Properties

        object IBase.Target => this;

        bool IBase.IsNull => toInstantiate.Count == 0;

        public int AllocatedTicksPerFrame {
            get {
                return allocatedTicksPerFrame;
            }
            set {
                allocatedTicksPerFrame = System.Math.Max(value, 0);
            }
        }

        #endregion

        #region Core

        public static void Add(IPrefabPool edit, int count) {
            Instance._Add(new Data(edit, count));
        }

        void _Add(Data data) {
            if (toInstantiate.Count == 0)
                UpdateSystem.Instance.SubscribeUpdate(this);
            toInstantiate.Add(data);
        }

        #endregion

        #region Update Impl

        void IUpdate.UpdateComponent(float time) {
            sw.Reset();
            sw.Start();
            Data data = toInstantiate[0];
            do {
                if (data.count > 0) {
                    data.poolData.LoadPrefab();
                    data.count--;
                }
                else {
                    toInstantiate.RemoveAt(0);
                    if (toInstantiate.Count > 0)
                        data = toInstantiate[0];
                    else
                        return;
                }
            } while (sw.ElapsedTicks < allocatedTicksPerFrame);
            toInstantiate[0] = data;
        }

        #endregion
    }
}
