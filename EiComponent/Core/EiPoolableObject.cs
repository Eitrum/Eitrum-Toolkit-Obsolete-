using System;

namespace Eitrum
{
	public class EiPoolableObject : EiCore
	{
		protected virtual void OnDispose ()
		{

		}
	}

	public class EiPoolableObject<T> : EiPoolableObject where T : EiPoolableObject
	{

		static EiSyncronizedQueue<T> pooled = new EiSyncronizedQueue<T> ();

		public static T NewInstance {
			get {
				T item;
				if (pooled.TryDequeue (out item)) {
					return item;
				}
				return Activator.CreateInstance<T> ();
			}
		}

		public void Dispose ()
		{
			base.OnDispose ();
			pooled.Enqueue (this as T);
		}

		public static void Clear ()
		{
			pooled = new EiSyncronizedQueue<T> ();
		}
	}
}

