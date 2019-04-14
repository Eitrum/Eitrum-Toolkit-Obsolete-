using System;

namespace Eitrum
{
	public class Message
	{
		#region Publish

		public static void Publish<T> (T message) where T : class
		{
			Message<T>.Publish (message);
		}

		public static void Publish<T> (T message, int channel) where T: class
		{
			Message<T>.Publish (message, channel);
		}

		#endregion

		#region Subscribe

		public static EiLLNode<MessageSubscriber<T>> Subscribe<T> (IBase target, Action<T> method, int channel = 0)
		{
			var newSub = new MessageSubscriber <T> (target, method);
			newSub.channel = channel;
			return Message<T>.subscribers.Add (newSub);
		}

		#endregion

		#region Unsubscribe

		public static void Unsubscribe<T> (EiLLNode<MessageSubscriber<T>> component)
		{
			Message<T>.subscribers.Remove (component);
		}

		#endregion
	}

	public class MessageSubscriber<T>
	{
		#region Variables

		Action<T> method;
		IBase baseInterface;

		public int calls = 0;
		public int channel = 0;

		#endregion

		#region Constructors

		public MessageSubscriber (IBase baseInterface, Action<T> method)
		{
			this.baseInterface = baseInterface;
			this.method = method;
		}

		#endregion

		#region Core

		public bool IsDestroyed {
			get {
				return baseInterface == null || baseInterface.IsNull;
			}
		}

		public void Send (T obj)
		{
			method (obj);
			calls++;
		}

		#endregion
	}

	public class Message<T>
	{
		public static EiLinkedList<MessageSubscriber<T>> subscribers = new EiLinkedList<MessageSubscriber<T>> ();

		#region Publish

		public static void Publish (T message)
		{
			var iterator = subscribers.GetIterator ();
			EiLLNode<MessageSubscriber<T>> subsNode;
			while (iterator.Next (out subsNode)) {
				if (subsNode.Value.IsDestroyed) {
					iterator.DestroyCurrent ();
				} else
					subsNode.Value.Send (message);
			}
		}

		public static void Publish (T message, int channel)
		{
			var iterator = subscribers.GetIterator ();
			EiLLNode<MessageSubscriber<T>> subsNode;
			while (iterator.Next (out subsNode)) {
				if (subsNode.Value.IsDestroyed) {
					iterator.DestroyCurrent ();
				} else if (subsNode.Value.channel == channel)
					subsNode.Value.Send (message);
			}
		}

		#endregion
	}
}