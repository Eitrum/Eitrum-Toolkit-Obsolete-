using System;

namespace Eitrum
{
	public class EiMessage
	{
		#region Publish

		public static void Publish<T> (T message) where T : EiCore
		{
			EiMessage<T>.Publish (message);
		}

		public static void Publish<T> (T message, int channel) where T: EiCore
		{
			EiMessage<T>.Publish (message, channel);
		}

		#endregion

		#region Subscribe

		public static EiLLNode<EiMessageSubscriber<T>> Subscribe<T> (EiCore core, Action<T> method, int channel = 0)
		{
			var newSub = new EiMessageSubscriber<T> (core, method);
			newSub.channel = channel;
			return EiMessage<T>.subscribers.Add (newSub);

		}

		public static EiLLNode<EiMessageSubscriber<T>> Subscribe<T> (EiComponent component, Action<T> method, int channel = 0)
		{
			var newSub = new EiMessageSubscriber <T> (component, method);
			newSub.channel = channel;
			return EiMessage<T>.subscribers.Add (newSub);
		}

		#endregion

		#region Unsubscribe

		public static void Unsubscribe<T> (EiLLNode<EiMessageSubscriber<T>> component)
		{
			EiMessage<T>.subscribers.Remove (component);
		}

		#endregion
	}

	public class EiMessageSubscriber<T> : EiCore
	{
		#region Variables

		Action<T> method;
		EiBaseInterface baseInterface;

		public int calls = 0;
		public int channel = 0;

		#endregion

		#region Constructors

		public EiMessageSubscriber (EiBaseInterface baseInterface, Action<T> method)
		{
			this.baseInterface = baseInterface;
			this.method = method;
		}

		#endregion

		#region Core

		public new bool IsDestroyed {
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

	public class EiMessage<T>
	{
		public static EiLinkedList<EiMessageSubscriber<T>> subscribers = new EiLinkedList<EiMessageSubscriber<T>> ();

		#region Publish

		public static void Publish (T message)
		{
			var iterator = subscribers.GetIterator ();
			EiLLNode<EiMessageSubscriber<T>> subsNode;
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
			EiLLNode<EiMessageSubscriber<T>> subsNode;
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

