using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiSerializeInterface<T> : ISerializationCallbackReceiver where T : EiBaseInterface
	{
		#region Variables

		[SerializeField]
		protected UnityEngine.Object obj;
		protected T targetInterface;

		#endregion

		#region Properties

		public T Get {
			get {
				return targetInterface;
			}
		}

		public T Interface {
			get {
				return targetInterface;
			}
		}

		public UnityEngine.Object Object {
			get {
				return obj;
			}
		}

		#endregion

		#region Constructors

		public EiSerializeInterface ()
		{

		}

		public EiSerializeInterface (T interf)
		{
			obj = interf.This as UnityEngine.Object;
			targetInterface = interf;
		}

		#endregion

		#region ISerializationCallbackReceiver Implementation

		public void OnAfterDeserialize ()
		{
			if (obj)
				targetInterface = (T)((object)obj);
		}

		public void OnBeforeSerialize ()
		{
			if (obj && obj.GetType ().GetInterface (typeof(T).Name) == null)
				obj = null;
		}

		#endregion
	}
}
