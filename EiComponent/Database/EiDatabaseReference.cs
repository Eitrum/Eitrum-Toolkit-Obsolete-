using System;
using UnityEngine;

namespace Eitrum
{
	[Serializable]
	public class EiDatabaseReference
	{
		#region Variables

		[SerializeField]
		private int uniqueIdReference = -1;
		private EiEntry cached;

		#endregion

		#region Properties

		public EiEntry Entry {
			get {
				if (cached == null && uniqueIdReference != -1) {
					cached = EiDatabase.GetEntry (uniqueIdReference);
				}
				return cached;
			}
		}

		public string ItemName {
			get {
				if (Entry != null)
					return Entry.ItemName;
				return "";
			}
		}

		public int UniqueIdReference {
			get {
				return uniqueIdReference;
			}
		}

		public UnityEngine.Object Object {
			get {
				if (Entry != null)
					return Entry.Object;
				return null;
			}
		}

		public GameObject GameObject {
			get {
				if (Entry != null)
					return Entry.GameObject;
				return null;
			}
		}

		public AudioClip AudioClip {
			get {
				if (Entry != null)
					return Entry.AudioClip;
				return null;
			}
		}

		public Animation Animation {
			get {
				if (Entry != null)
					return Entry.AnimationClip;
				return null;
			}
		}

		public string SceneName {
			get {
				if (Entry != null)
					return Entry.SceneName;
				return "";
			}
		}

		public Type Type {
			get {
				return Entry.Type;
			}
		}

		#endregion

		#region Scene

		public void LoadScene ()
		{
			EiDatabase.LoadScene (uniqueIdReference);
		}

		#endregion

		#region Instantiate

		#region Normal Instantiate

		public UnityEngine.Object Instantiate ()
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.Object;
			if (obj) {
				return MonoBehaviour.Instantiate (obj);
			}
			return null;
		}

		public UnityEngine.Object Instantiate (Transform parent)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.Object;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, parent);
			}
			return null;
		}

		public UnityEngine.Object Instantiate (Vector3 position)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.Object;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, Quaternion.identity);
			}
			return null;
		}

		public UnityEngine.Object Instantiate (Vector3 position, Quaternion rotation)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.Object;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, rotation);
			}
			return null;
		}

		public UnityEngine.Object Instantiate (Vector3 position, Quaternion rotation, Transform parent)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.Object;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, rotation, parent);
			}
			return null;
		}

		#endregion

		#region GameObject Instantiate

		public GameObject InstantiateAsGameObject ()
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.GameObject;
			if (obj) {
				return MonoBehaviour.Instantiate (obj);
			}
			return null;
		}

		public GameObject InstantiateAsGameObject (Transform parent)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.GameObject;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, parent);
			}
			return null;
		}

		public GameObject InstantiateAsGameObject (Vector3 position)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.GameObject;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, Quaternion.identity);
			}
			return null;
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.GameObject;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, rotation);
			}
			return null;
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation, Transform parent)
		{
			if (uniqueIdReference == -1) {
				return null;
			}
			var obj = Entry.GameObject;
			if (obj) {
				return MonoBehaviour.Instantiate (obj, position, rotation, parent);
			}
			return null;
		}

		#endregion

		#endregion

		#region Helpers

		public bool Is<T> ()
		{
			return Entry.Is<T> ();
		}

		public bool Is (Type type)
		{
			return Entry.Is (type);
		}

		#endregion
	}
}