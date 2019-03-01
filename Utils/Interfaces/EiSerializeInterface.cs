using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiSerializeInterface<T> : ISerializationCallbackReceiver where T : IBase
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
			obj = interf.Target as UnityEngine.Object;
			targetInterface = interf;
		}

		#endregion

		#region Static Constructor

		public static TObject Create<TObject> (T interf)
			where TObject : EiSerializeInterface<T>
		{
			var tObj = Activator.CreateInstance<TObject> ();
			tObj.obj = interf.Target as UnityEngine.Object;
			tObj.targetInterface = interf;
			return tObj;
		}

		#endregion

		#region ISerializationCallbackReceiver Implementation

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			if (obj)
				targetInterface = (T)((object)obj);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize ()
		{
			if (obj && obj.GetType ().GetInterface (typeof(T).Name) == null)
				obj = null;
		}

		#endregion
	}

	public static class EiSerializeInterfaceExtension
	{
		#region To Serializable Arrays

		public static TObject[] ToSerializableArray<TObject, TInterface> (this TInterface[] oldList) 
			where TObject : EiSerializeInterface<TInterface>
			where TInterface : IBase
		{
			TObject[] array = new TObject[oldList.Length];
			for (int i = 0; i < array.Length; i++) {
				array [i] = EiSerializeInterface<TInterface>.Create<TObject> (oldList [i]);
			}

			return array;
		}

		public static void ToSerializableArray<TObject, TInterface> (this TInterface[] oldList, ref TObject[] array) 
			where TObject : EiSerializeInterface<TInterface>
			where TInterface : IBase
		{

			array = new TObject[oldList.Length];
			for (int i = 0; i < array.Length; i++) {
				array [i] = EiSerializeInterface<TInterface>.Create<TObject> (oldList [i]);
			}
		}

		#endregion
	}
}