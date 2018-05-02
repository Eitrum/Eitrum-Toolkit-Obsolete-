using System;
using System.Collections.Generic;

namespace Eitrum
{
	public class EiSyncronizedQueue<T> : EiCore
	{
		
		Queue<T> queue = new Queue<T> ();

		public void Enqueue (T item)
		{
			lock (queue) {
				queue.Enqueue (item);
			}
		}

		public T Dequeue ()
		{
			lock (queue) {
				if (queue.Count > 0) {
					return queue.Dequeue ();
				}
			}
			return default(T);
		}

		public bool TryDequeue (out T item)
		{
			lock (queue) {
				if (queue.Count > 0) {
					item = queue.Dequeue ();
					return true;
				}
			}

			item = default(T);
			return false;
		}
	}
}

