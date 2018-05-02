using System;

namespace Eitrum
{
	public class EiValue<T>
	{
		T value;

		public EiValue ()
		{
			value = default(T);
		}

		public EiValue (T value)
		{
			this.value = value;
		}

		public void SetValue (T value)
		{
			lock (this)
				this.value = value;
		}

		public T GetValue ()
		{
			lock (this)
				return value;
		}
	}
}