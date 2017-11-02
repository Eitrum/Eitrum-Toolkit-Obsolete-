using System;

namespace Eitrum
{
	public class EiLinkedList<T> where T : class
	{
		#region Variables

		static EiSyncronizedQueue<EiLLNode<T>> nodes = new EiSyncronizedQueue<EiLLNode<T>> ();

		private EiLLNode<T> node;
		private int count;

		#endregion

		#region Add

		public EiLLNode<T> Add (T value)
		{
			lock (this) {
				EiLLNode<T> node;
				if (!nodes.TryDequeue (out node))
					node = new EiLLNode<T> (value);
				else
					node.Value = value;

				if (count == 0) {
					this.node = node;
					node.Next = this.node;
					node.Prev = this.node;
				} else {
					node.Prev = this.node.Prev;
					node.Next = this.node;
					this.node.Prev.Next = node;
					this.node.Prev = node;
				}

				count++;
				node.List = this;
			}
			return node;
		}

		public EiLLNode<T>[] AddRange (T[] values)
		{
			lock (this) {
				EiLLNode<T>[] nodes = new EiLLNode<T>[values.Length];
				for (int i = 0; i < values.Length; i++) {
					nodes [i] = Add (values [i]);
				}
				return nodes;
			}
		}

		#endregion

		#region Remove

		/// <summary>
		/// Remove the specified node object, very slow.
		/// Will iterate through object until it finds object, then delete it.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void Remove (T nodeObject)
		{
			lock (this) {
				var iterator = GetIterator ();
				EiLLNode<T> node = null;
				while (iterator.Next (out node)) {
					if (node.Value == nodeObject) {
						Remove (node);
						return;
					}
				}
			}
		}

		public void Remove (EiLLNode<T> node)
		{
			lock (this) {
				if (node.List == this)
					return;

				node.Prev.Next = node.Next;
				node.Next.Prev = node.Prev;
				node.Prev = null;
				node.Next = null;
				node.List = null;
				nodes.Enqueue (node);

				if (count <= 1) {
					this.node = null;
				} 
				count--;
			}
		}

		public void Clear ()
		{
			for (int i = count; i > 0; i--)
				Remove (node);
			ClearFast ();
		}

		public void ClearFast ()
		{
			lock (this) {
				node = null;
				count = 0;
			}
		}

		#endregion

		#region Helper

		public EiLLIterator<T> GetIterator ()
		{
			lock (this) {
				return new EiLLIterator<T> (node);
			}
		}

		public int Count ()
		{
			lock (this) {
				return count;
			}
		}

		#endregion
	}

	public struct EiLLIterator<T> where T : class
	{
		#region Variables

		private EiLLNode<T> first;
		private EiLLNode<T> current;

		#endregion

		#region Constructors

		public EiLLIterator (EiLLNode<T> first)
		{
			this.first = first;
			current = null;
		}

		#endregion

		#region Next

		public bool Next ()
		{
			return Next (out (current));
		}

		public bool Next (out EiLLNode<T> node)
		{
			if (current == null) {
				if (first == null) {
					node = null;
					return false;
				}
				node = current = first;
				return true;
			}

			node = (current = current.Next);
			return current != first;
		}

		public bool Next (out T value)
		{
			if (current == null) {
				if (first == null) {
					value = default(T);
					return false;
				}
				value = (current = first).Value;
				return true;
			}

			value = (current = current.Next).Value;
			return current != first;
		}

		#endregion
	}

	public class EiLLNode<T> where T : class
	{
		#region Variables

		public EiLLNode<T> Next;
		public EiLLNode<T> Prev;
		public EiLinkedList<T> List;

		public T Value;

		#endregion

		#region Constructor

		public EiLLNode (T value)
		{
			this.Value = value;
			Next = null;
			Prev = null;
			List = null;
		}

		#endregion
	}
}