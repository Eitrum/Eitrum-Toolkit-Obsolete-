using System;

namespace Eitrum
{
	public struct EiPriority<T>
	{
		#region Variables

		int priorityLevel;
		T target;

		#endregion

		#region Properties

		public int PriorityLevel {
			get {
				return priorityLevel;
			}
		}

		public T Target {
			get {
				return target;
			}
		}

		#endregion

		public EiPriority (int priorityLevel, T target)
		{
			this.priorityLevel = priorityLevel;
			this.target = target;
		}
	}
}

