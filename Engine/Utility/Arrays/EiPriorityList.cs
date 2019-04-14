using System;

namespace Eitrum {
    public class EiPriorityList<T> {
        #region Variables

        protected EiSyncronizedList<EiPriority<T>> priorityList = new EiSyncronizedList<EiPriority<T>>();

        #endregion

        #region Properties

        public EiPriority<T> this[int index] {
            get {
                return priorityList[index];
            }
        }

        public int Count {
            get {
                return priorityList.Count;
            }
        }

        public int Length {
            get {
                return priorityList.Length;
            }
        }

        #endregion

        #region Core

        public void Add(int priorityLevel, T item) {
            int difference = 0;
            for (int i = priorityList.Count - 1; i >= 0; i--) {
                difference = priorityLevel - priorityList[i].PriorityLevel;
                if (difference > 0) {
                    priorityList.Insert(i + 1, new EiPriority<T>(priorityLevel, item));
                    return;
                }
            }
            priorityList.Insert(0, new EiPriority<T>(priorityLevel, item));
        }

        public void Remove(T item) {
            for (int i = priorityList.Count - 1; i >= 0; i--) {
                if (priorityList[i].Target.Equals(item)) {
                    priorityList.RemoveAt(i);
                    break;
                }
            }
        }

        #endregion
    }
}

