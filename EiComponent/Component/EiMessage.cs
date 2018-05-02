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

		EiCore core;
		EiComponent comp;

		public int calls = 0;
		public int channel = 0;

		#endregion

		#region Constructors

		public EiMessageSubscriber (EiComponent component, Action<T> method)
		{
			this.comp = component;
			this.method = method;
		}

		public EiMessageSubscriber (EiCore core, Action<T> method)
		{
			this.core = core;
			this.method = method;
		}

		#endregion

		#region Core

		public override bool IsDestroyed {
			get {
				return comp == null || (core == null || core.IsDestroyed) || base.IsDestroyed;
			}
		}

		public void Send (T obj)
		{
			method (obj);
			calls++;
		}

		#endregion

		#region Helper

		public bool IsCore ()
		{
			return core != null;
		}

		public bool IsComponent ()
		{
			return comp != null;
		}

		public EiCore GetCore ()
		{
			return core;
		}

		public EiComponent GetComponent ()
		{
			return comp;
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
					subscribers.Remove (subsNode);
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
					subscribers.Remove (subsNode);
				} else if (subsNode.Value.channel == channel)
					subsNode.Value.Send (message);
			}
		}

		#endregion
	}
}

