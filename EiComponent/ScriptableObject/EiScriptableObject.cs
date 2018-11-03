using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eitrum
{
	public class EiScriptableObject<T> : ScriptableObject, EiUpdateInterface, EiBaseInterface
	{
		#region EiUpdateInterface implementation

		public virtual void PreUpdateComponent (float time)
		{
			throw new NotImplementedException ("PreUpdateComponent");
		}

		public virtual void UpdateComponent (float time)
		{
			throw new NotImplementedException ("UpdateComponent");
		}

		public virtual void LateUpdateComponent (float time)
		{
			throw new NotImplementedException ("LateUpdateComponent");
		}

		public virtual void FixedUpdateComponent (float time)
		{
			throw new NotImplementedException ("FixedUpdateComponent");
		}

		public virtual void ThreadedUpdateComponent (float time)
		{
			throw new NotImplementedException ("ThreadedUpdateComponent");
		}

		#endregion

		#region EiBaseInterface implementation

		public EiComponent Component {
			get {
				throw new NotImplementedException ();
			}
		}

		public EiCore Core {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsNull {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}