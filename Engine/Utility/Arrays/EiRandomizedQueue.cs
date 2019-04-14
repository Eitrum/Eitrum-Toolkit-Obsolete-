using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eitrum.Mathematics;

namespace Eitrum {
    public class EiRandomizedQueue<T> {
        #region Variables

        [SerializeField]
        protected List<T> content = new List<T>();
        protected EiRandom random = new EiRandom();

        #endregion

        #region Properties

        public virtual int Seed {
            get {
                return random._Seed;
            }
            set {
                random.SetSeed(value);
            }
        }

        public virtual bool HasValues {
            get {
                return content.Count > 0;
            }
        }

        public virtual int RandomIndex {
            get {
                return random._Range(0, content.Count);
            }
        }

        public virtual T Random {
            get {
                return content[RandomIndex];
            }
        }

        #endregion

        #region Constructors

        public EiRandomizedQueue() {
            random = new EiRandom(EiRandom.Range(0, 1000000));
        }

        public EiRandomizedQueue(IEnumerable<T> items) {
            random = new EiRandom(EiRandom.Range(0, 1000000));
            content.AddRange(items);
        }

        #endregion

        #region Add

        public virtual void Add(T item) {
            content.Add(item);
        }

        public virtual void AddRange(IEnumerable<T> items) {
            content.AddRange(items);
        }

        #endregion

        #region Remove

        public virtual void Remove(T item) {
            content.Remove(item);
        }

        public virtual void RemoveAt(int index) {
            content.RemoveAt(index);
        }

        #endregion

        #region Queue / Dequeue

        public virtual void Queue(T item) {
            content.Add(item);
        }

        public virtual void Queue(IEnumerable<T> items) {
            content.AddRange(items);
        }

        public virtual T Dequeue() {
            if (!HasValues)
                return default(T);
            var index = random._Range(0, content.Count);
            var value = content[index];
            content.RemoveAt(index);
            return value;
        }

        #endregion

        #region Utils

        public virtual void Clear() {
            content.Clear();
        }

        #endregion
    }

    public class EiRandomizedQueueInt : EiRandomizedQueue<int> {
        /// <summary>
        /// Fill the specified range, inclusive min to exlusive max
        /// </summary>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public void Fill(int min, int max) {
            for (int i = min; i < max; i++) {
                content.Add(i);
            }
        }
    }
}