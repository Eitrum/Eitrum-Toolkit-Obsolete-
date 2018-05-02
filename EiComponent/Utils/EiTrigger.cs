using System;

namespace Eitrum
{
	public class EiTrigger
	{
		Action onTrigger;
		Action onUnityThreadTrigger;

		public void Trigger ()
		{
			EiTask.Run (Nothing, ThreadTrigger, UnityThreadTrigger);
		}

		public void AddAction (Action action)
		{
			onTrigger += action;
		}

		public void AddActionUnityThread (Action action)
		{
			onUnityThreadTrigger += action;
		}

		public void RemoveAction (Action action)
		{
			if (onTrigger != null)
				onTrigger -= action;
		}

		public void RemoveActionUnityThread (Action action)
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		public void Clear ()
		{
			onTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing ()
		{

		}

		void ThreadTrigger ()
		{
			if (onTrigger != null)
				onTrigger ();
		}

		void UnityThreadTrigger ()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger ();
		}
	}

	public class EiTrigger<T>
	{
		T value;
		Action<T> onTrigger;
		Action<T> onUnityThreadTrigger;

		public void Trigger (T value)
		{
			this.value = value;
			EiTask.Run (Nothing, ThreadTrigger, UnityThreadTrigger);
		}

		public void AddAction (Action<T> action)
		{
			onTrigger += action;
		}

		public void AddActionUnityThread (Action<T> action)
		{
			onUnityThreadTrigger += action;
		}

		public void RemoveAction (Action<T> action)
		{
			if (onTrigger != null)
				onTrigger -= action;
		}

		public void RemoveActionUnityThread (Action<T> action)
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		public void Clear ()
		{
			onTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing ()
		{

		}

		void ThreadTrigger ()
		{
			if (onTrigger != null)
				onTrigger (value);
		}

		void UnityThreadTrigger ()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger (value);
		}
	}

	public class EiTrigger<T1, T2>
	{
		T1 value1;
		T2 value2;
		Action<T1, T2> onTrigger;
		Action<T1, T2> onUnityThreadTrigger;

		public void Trigger (T1 value1, T2 value2)
		{
			this.value1 = value1;
			this.value2 = value2;
			EiTask.Run (Nothing, ThreadTrigger, UnityThreadTrigger);
		}

		public void AddAction (Action<T1, T2> action)
		{
			onTrigger += action;
		}

		public void AddActionUnityThread (Action<T1, T2> action)
		{
			onUnityThreadTrigger += action;
		}

		public void RemoveAction (Action<T1, T2> action)
		{
			if (onTrigger != null)
				onTrigger -= action;
		}

		public void RemoveActionUnityThread (Action<T1, T2> action)
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		public void Clear ()
		{
			onTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing ()
		{

		}

		void ThreadTrigger ()
		{
			if (onTrigger != null)
				onTrigger (value1, value2);
		}

		void UnityThreadTrigger ()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger (value1, value2);
		}
	}

	public class EiTrigger<T1, T2, T3>
	{
		T1 value1;
		T2 value2;
		T3 value3;
		Action<T1, T2, T3> onTrigger;
		Action<T1, T2, T3> onUnityThreadTrigger;

		public void Trigger (T1 value1, T2 value2, T3 value3)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
			EiTask.Run (Nothing, ThreadTrigger, UnityThreadTrigger);
		}

		public void AddAction (Action<T1, T2, T3> action)
		{
			onTrigger += action;
		}

		public void AddActionUnityThread (Action<T1, T2, T3> action)
		{
			onUnityThreadTrigger += action;
		}

		public void RemoveAction (Action<T1, T2, T3> action)
		{
			if (onTrigger != null)
				onTrigger -= action;
		}

		public void RemoveActionUnityThread (Action<T1, T2, T3> action)
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		public void Clear ()
		{
			onTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing ()
		{

		}

		void ThreadTrigger ()
		{
			if (onTrigger != null)
				onTrigger (value1, value2, value3);
		}

		void UnityThreadTrigger ()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger (value1, value2, value3);
		}
	}

	public class EiTrigger<T1, T2, T3, T4>
	{
		T1 value1;
		T2 value2;
		T3 value3;
		T4 value4;
		Action<T1, T2, T3, T4> onTrigger;
		Action<T1, T2, T3, T4> onUnityThreadTrigger;

		public void Trigger (T1 value1, T2 value2, T3 value3, T4 value4)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
			this.value4 = value4;
			EiTask.Run (Nothing, ThreadTrigger, UnityThreadTrigger);
		}

		public void AddAction (Action<T1, T2, T3, T4> action)
		{
			onTrigger += action;
		}

		public void AddActionUnityThread (Action<T1, T2, T3, T4> action)
		{
			onUnityThreadTrigger += action;
		}

		public void RemoveAction (Action<T1, T2, T3, T4> action)
		{
			if (onTrigger != null)
				onTrigger -= action;
		}

		public void RemoveActionUnityThread (Action<T1, T2, T3, T4> action)
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger -= action;
		}

		public void Clear ()
		{
			onTrigger = null;
			onUnityThreadTrigger = null;
		}

		void Nothing ()
		{

		}

		void ThreadTrigger ()
		{
			if (onTrigger != null)
				onTrigger (value1, value2, value3, value4);
		}

		void UnityThreadTrigger ()
		{
			if (onUnityThreadTrigger != null)
				onUnityThreadTrigger (value1, value2, value3, value4);
		}
	}
}

