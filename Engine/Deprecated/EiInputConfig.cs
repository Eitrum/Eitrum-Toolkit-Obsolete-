using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eitrum
{
	public class EiInputConfig : ScriptableObject
	{
		#region Classes

		[Serializable]
		public class EiInputMapKey
		{
			public KeyCode inputKey;
			public KeyCode outputKey;
		}

		[Serializable]
		public class EiInputMapAxis
		{
			public string inputAxis;
			public string outputAxis;
		}

		#endregion

		#region Variables

		public List<EiInputMapKey> mappedKeys = new List<EiInputMapKey> ();

		public List<EiInputMapAxis> mappedAxises = new List<EiInputMapAxis> ();

		protected Dictionary<KeyCode, KeyCode> keyCodeMapping = new Dictionary<KeyCode, KeyCode> ();

		protected Dictionary<string, string> axisMapping = new Dictionary<string, string> ();

		bool isInstantiated = false;

		#endregion

		#region Properties

		public static EiInputConfig Default {
			get {
				var instance = CreateInstance<EiInputConfig> ();
				instance.name = "Default Instance";
				return instance;
			}
		}

		#endregion

		#region Core

		public virtual void Instantiate ()
		{
			if (!isInstantiated) {
				isInstantiated = true;
				for (int i = 0; i < mappedKeys.Count; i++)
					keyCodeMapping.Add (mappedKeys [i].inputKey, mappedKeys [i].outputKey);
				for (int i = 0; i < mappedAxises.Count; i++)
					axisMapping.Add (mappedAxises [i].inputAxis, mappedAxises [i].outputAxis);
			}
		}

		public virtual bool GetKeyDown (KeyCode key)
		{
			if (keyCodeMapping.ContainsKey (key))
				return UnityEngine.Input.GetKeyDown (keyCodeMapping [key]);
			return UnityEngine.Input.GetKeyDown (key);
		}

		public virtual bool GetKeyUp (KeyCode key)
		{
			if (keyCodeMapping.ContainsKey (key))
				return UnityEngine.Input.GetKeyUp (keyCodeMapping [key]);
			return UnityEngine.Input.GetKeyUp (key);
		}

		public virtual bool GetKey (KeyCode key)
		{
			if (keyCodeMapping.ContainsKey (key))
				return UnityEngine.Input.GetKey (keyCodeMapping [key]);
			return UnityEngine.Input.GetKey (key);
		}

		public virtual float GetAxis (string axis)
		{
			if (axisMapping.ContainsKey (axis))
				return UnityEngine.Input.GetAxis (axisMapping [axis]);
			return UnityEngine.Input.GetAxis (axis);
		}

		public virtual float GetAxisRaw (string axis)
		{
			if (axisMapping.ContainsKey (axis))
				return UnityEngine.Input.GetAxisRaw (axisMapping [axis]);
			return UnityEngine.Input.GetAxisRaw (axis);
		}

		#endregion

		#region Editor

		#if UNITY_EDITOR

		[MenuItem ("Assets/Create/Eitrum/New Input Config")]
		public static void CreateInputConfig ()
		{
			var obj = Default;
			EiSOCreator.CreateAsset<EiInputConfig> (obj);
		}

		#endif

		#endregion

	}
}

