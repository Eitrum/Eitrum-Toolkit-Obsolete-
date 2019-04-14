using System;
using System.Collections.Generic;
using System.Linq;

namespace Eitrum
{
	[Serializable]
	public class EiSyncronizedList<T>
	{
		#region Variables

		[UnityEngine.SerializeField]
		private List<T> list;

		#endregion

		#region Properties

		public int Length {
			get {
				lock (list)
					return list.Count;
			}
		}

		public int Count {
			get {
				lock (list)
					return list.Count;
			}
		}

		public T this [int i] {
			get {
				lock (list) {
					if (list.Count > i) {
						return list [i];
					}
				}
				return default(T);
			}
		}

		#endregion

		#region Constructors

		public EiSyncronizedList ()
		{
			list = new List<T> ();
		}

		public EiSyncronizedList (int capacity)
		{
			list = new List<T> (capacity);
		}

		public EiSyncronizedList (IEnumerable<T> collection)
		{
			list = new List<T> (collection);
		}

		#endregion

		#region Add

		public void Add (T item)
		{
			lock (list) {
				list.Add (item);
			}
		}

		public void AddRange (IEnumerable<T> items)
		{
			lock (list) {
				list.AddRange (items);
			}
		}

		public void Insert (int index, T item)
		{
			lock (list) {
				list.Insert (index, item);
			}
		}

		public void InsertRange (int index, IEnumerable<T> items)
		{
			lock (list) {
				list.InsertRange (index, items);
			}
		}

		#endregion

		#region Remove

		public void Remove (T item)
		{
			lock (list) {
				list.Remove (item);
			}
		}

		public void RemoveAt (int index)
		{
			lock (list) {
				list.RemoveAt (index);
			}
		}

		public void RemoveRange (int index, int count)
		{
			lock (list) {
				list.RemoveRange (index, count);
			}
		}

		public void RemoveAll (Predicate<T> predicate)
		{
			lock (list) {
				list.RemoveAll (predicate);
			}
		}

		#endregion

		#region Extra

		public void Clear ()
		{
			lock (list) {
				list.Clear ();
			}
		}

		public bool Has (T item)
		{
			lock (list) {
				return list.Contains (item);
			}
		}

		public bool Contains (T item)
		{
			lock (list) {
				return list.Contains (item);
			}
		}

		public bool Contains (Func<T, bool> func)
		{
			lock (list) {
				for (int i = list.Count; i >= 0; i--) {
					if (func (list [i]))
						return true;
				}
				return false;
			}
		}

		#endregion

		#region Getters

		public List<T>.Enumerator GetEnumerator ()
		{
			lock (list) {
				return list.GetEnumerator ();
			}
		}

		public T Get (int index)
		{
			lock (list) {
				if (list.Count > index) {
					return list [index];
				}
			}
			return default(T);
		}

		public bool TryGet (int index, out T item)
		{
			lock (list) {
				if (list.Count > index) {
					item = list [index];
					return true;
				}
			}

			item = default(T);
			return false;
		}

		public T GetFirst ()
		{
			lock (list) {
				return list [0];
			}
		}

		public T GetLast ()
		{
			lock (list) {
				var lastElement = Length - 1;
				return list [lastElement];
			}
		}

		public List<T> GetCopy ()
		{
			lock (list) {
				return new List<T> (list);
			}
		}

		#endregion
	}
}

