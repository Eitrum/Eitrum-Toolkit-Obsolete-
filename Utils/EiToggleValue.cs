using System;

namespace Eitrum
{
	public class EiToggleValue<T> where T : struct
	{
		#region Variables

		T value1 = default(T);
		T value2 = default(T);

		bool toggle = true;

		#endregion

		#region Properties

		public T Value {
			get {
				return toggle ? value1 : value2;
			}
		}

		#endregion

		#region Core

		public void Toggle ()
		{
			toggle = !toggle;
		}

		public void Toggle (bool toggle)
		{
			this.toggle = toggle;
		}

		public bool GetToggle ()
		{
			return toggle;
		}

		public T GetValue ()
		{
			return toggle ? value1 : value2;
		}

		#endregion
	}
}

