using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitrum
{
	public struct EiBoolStack
	{
		#region Variables

		private int boo;

		private EiTrigger<bool> trigger;

		#endregion

		#region Properties

		public bool set
		{
			set
			{
				if (value)
					Increment();
				else
					Decrement();
			}
		}

		public bool IsTrue
		{
			get
			{
				return boo > 0;
			}
		}

		public bool IsFalse
		{
			get
			{
				return boo == 0;
			}
		}

		public EiBoolStack Copy
		{
			get
			{
				return new EiBoolStack(this);
			}
		}

		#endregion

		#region Constructors

		public EiBoolStack(EiBoolStack multiBool)
		{
			boo = multiBool.boo;
			trigger = multiBool.trigger;
		}

		#endregion

		#region API Methods

		public void Add()
		{
			Increment();
		}

		public void Remove()
		{
			Decrement();
		}

		/// <summary>
		/// This will run Increment or Decrement based on the parameter value.
		/// </summary>
		/// <param name="value"></param>
		public void Set(bool value)
		{
			if (value)
				Increment();
			else
				Decrement();
		}

		#endregion

		#region Subscribe

		public void Subscribe(Action<bool> action)
		{
			if (trigger == null)
				trigger = new EiTrigger<bool>();
			trigger.Subscribe(action);
		}

		public void Subscribe(Action<bool> action, bool anyThread)
		{
			if (trigger == null)
				trigger = new EiTrigger<bool>();
			trigger.Subscribe(action, anyThread);
		}

		public void Unsubscribe(Action<bool> action)
		{
			if (trigger != null)
				trigger.Unsubscribe(action);
		}

		#endregion

		#region Internal Increment and Decrement

		public void Increment()
		{
			if (boo++ == 0 && trigger != null)
				trigger.Trigger(true);
		}

		public void Decrement()
		{
			if (--boo == 0 && trigger != null)
				trigger.Trigger(false);
		}

		public void Reset()
		{
			if (boo > 0 && trigger != null)
			{
				boo = 0;
				trigger.Trigger(false);
			}
			else
			{
				boo = 0;
			}
		}

		#endregion

		#region Operators

		public static implicit operator bool(EiBoolStack multiBool)
		{
			return multiBool.boo > 0;
		}

		public static EiBoolStack operator ++(EiBoolStack multiBool)
		{
			multiBool.Increment();
			return multiBool;
		}

		public static EiBoolStack operator --(EiBoolStack multiBool)
		{
			multiBool.Decrement();
			return multiBool;
		}

		#endregion
	}
}
