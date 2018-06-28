using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eitrum
{
	public class EiScriptableObject<T> : ScriptableObject, EiUpdateInterface, EiBaseInterface
		where T : ScriptableObject
	{
		#region Properties

		public string FileName {
			get {
				return base.name;
			}
		}

		#endregion

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

		#region Scriptable Object Creation

		public static T Default {
			get {
				var so = CreateInstance<T> ();
				so.name = typeof(T).Name;
				return so;
			}
		}



		#if UNITY_EDITOR

		public void CreateFile ()
		{
			EiSOCreator.CreateAsset<T> (this as T);
		}

		public static T CreateAsset ()
		{
			var obj = Default;
			EiSOCreator.CreateAsset (obj);
			return obj;
		}

		public static T CreateAsset (string path)
		{
			var obj = Default;
			EiSOCreator.CreateAsset (obj, path);
			return obj;
		}

		public void DestroyFile ()
		{
			DestroyAsset (this);
		}

		public static void DestroyAsset<Tso> (Tso asset) where Tso : ScriptableObject
		{
			EiSOCreator.DestroyAsset<Tso> (asset);
		}

		#endif

		#endregion
	}
}