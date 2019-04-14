using Eitrum.Engine.Threading;
using System;

namespace Eitrum
{
	public class EiTrigger
	{
		#region Variables

		Action onAnyThreadTrigger;
		Action onUnityThreadTrigger;

		#endregion

		#region Subscribe Unsub

		public EiTrigger AddAction(Action action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger AddAction(Action action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger Subscribe(Action action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger Subscribe(Action action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger RemoveAction(Action action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		public EiTrigger Unsubscribe(Action action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		#endregion

		#region Core

		public void Trigger()
		{
			EiTask.Run(Nothing, UnityThreadTrigger, AnyThreadTrigger);
		}

		public void Clear()
		{
			onAnyThreadTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing()
		{

		}

		void AnyThreadTrigger()
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger();
		}

		void UnityThreadTrigger()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger();
		}

		#endregion
	}

	public class EiTrigger<T>
	{
		#region Variables

		T value;
		Action<T> onAnyThreadTrigger;
		Action<T> onUnityThreadTrigger;

		#endregion

		#region Subscribe Unsub

		public EiTrigger<T> AddAction(Action<T> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T> AddAction(Action<T> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T> Subscribe(Action<T> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T> Subscribe(Action<T> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T> RemoveAction(Action<T> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		public EiTrigger<T> Unsubscribe(Action<T> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		#endregion

		#region Core

		public void Trigger(T value)
		{
			this.value = value;
			if (UnityThreading.IsMainThread)
			{
				UnityThreadTrigger();
				EiTask.Run(AnyThreadTrigger);
			}
			else
				EiTask.Run(Nothing, UnityThreadTrigger, AnyThreadTrigger);
		}

		public void Clear()
		{
			onAnyThreadTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing()
		{

		}

		void AnyThreadTrigger()
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger(value);
		}

		void UnityThreadTrigger()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(value);
		}

		#endregion
	}

	public class EiTrigger<T1, T2>
	{
		#region Variables

		T1 value1;
		T2 value2;
		Action<T1, T2> onAnyThreadTrigger;
		Action<T1, T2> onUnityThreadTrigger;

		#endregion

		#region Subscribe Unsub

		public EiTrigger<T1, T2> AddAction(Action<T1, T2> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2> AddAction(Action<T1, T2> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2> Subscribe(Action<T1, T2> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2> Subscribe(Action<T1, T2> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2> RemoveAction(Action<T1, T2> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		public EiTrigger<T1, T2> Unsubscribe(Action<T1, T2> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		#endregion

		#region Core

		public void Trigger(T1 value1, T2 value2)
		{
			this.value1 = value1;
			this.value2 = value2;
			EiTask.Run(Nothing, UnityThreadTrigger, AnyThreadTrigger);
		}

		public void Clear()
		{
			onAnyThreadTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing()
		{

		}

		void AnyThreadTrigger()
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger(value1, value2);
		}

		void UnityThreadTrigger()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(value1, value2);
		}

		#endregion
	}

	public class EiTrigger<T1, T2, T3>
	{
		#region Variables

		T1 value1;
		T2 value2;
		T3 value3;
		Action<T1, T2, T3> onAnyThreadTrigger;
		Action<T1, T2, T3> onUnityThreadTrigger;

		#endregion

		#region Subscribe Unsub

		public EiTrigger<T1, T2, T3> AddAction(Action<T1, T2, T3> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3> AddAction(Action<T1, T2, T3> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3> Subscribe(Action<T1, T2, T3> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3> Subscribe(Action<T1, T2, T3> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3> RemoveAction(Action<T1, T2, T3> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		public EiTrigger<T1, T2, T3> Unsubscribe(Action<T1, T2, T3> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		#endregion

		#region Core

		public void Trigger(T1 value1, T2 value2, T3 value3)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
			EiTask.Run(Nothing, UnityThreadTrigger, AnyThreadTrigger);
		}
		public void Clear()
		{
			onAnyThreadTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing()
		{

		}

		void AnyThreadTrigger()
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger(value1, value2, value3);
		}

		void UnityThreadTrigger()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(value1, value2, value3);
		}

		#endregion
	}

	public class EiTrigger<T1, T2, T3, T4>
	{
		#region Variables

		T1 value1;
		T2 value2;
		T3 value3;
		T4 value4;
		Action<T1, T2, T3, T4> onAnyThreadTrigger;
		Action<T1, T2, T3, T4> onUnityThreadTrigger;

		#endregion

		#region Subscribe Unsub

		public EiTrigger<T1, T2, T3, T4> AddAction(Action<T1, T2, T3, T4> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3, T4> AddAction(Action<T1, T2, T3, T4> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3, T4> Subscribe(Action<T1, T2, T3, T4> action)
		{
			onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3, T4> Subscribe(Action<T1, T2, T3, T4> action, bool anyThread)
		{
			if (anyThread)
				onAnyThreadTrigger += action;
			else
				onUnityThreadTrigger += action;
			return this;
		}

		public EiTrigger<T1, T2, T3, T4> RemoveAction(Action<T1, T2, T3, T4> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		public EiTrigger<T1, T2, T3, T4> Unsubscribe(Action<T1, T2, T3, T4> action)
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger -= action;
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
			return this;
		}

		#endregion

		#region Core

		public void Trigger(T1 value1, T2 value2, T3 value3, T4 value4)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
			this.value4 = value4;
			EiTask.Run(Nothing, UnityThreadTrigger, AnyThreadTrigger);
		}

		public void Clear()
		{
			onAnyThreadTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing()
		{

		}

		void AnyThreadTrigger()
		{
			if (onAnyThreadTrigger != null)
				onAnyThreadTrigger(value1, value2, value3, value4);
		}

		void UnityThreadTrigger()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger(value1, value2, value3, value4);
		}

		#endregion
	}
}