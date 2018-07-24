using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eitrum
{
	[Serializable]
	public class EiDatabaseItem : EiScriptableObject<EiDatabaseItem>
	{
		#region Variables

		[Readonly]
		[SerializeField]
		private string itemName = "empty";
		[SerializeField]
		private UnityEngine.Object item = null;
        [SerializeField]
        [Readonly]
        private int uniqueId = 0;
		[SerializeField]
		private EiDatabaseResource database = null;

		#endregion

		#region Properties

		public string ItemName {
			get {
				return itemName;
			}
		}

        public int UniqueId
        {
            get
            {
                return uniqueId;
            }
        }

		public UnityEngine.Object Item {
			get {
				return item;
			}
		}

		public UnityEngine.Object Object {
			get {
				return item;
			}
		}

		public EiDatabaseResource Database {
			get {
				return database;
			}
		}

		public GameObject GameObject {
			get {
				return item as GameObject;
			}
		}

		public AudioClip AudioClip {
			get {
				return item as AudioClip;
			}
		}

		public Animation AnimationClip {
			get {
				return item as Animation;
			}
		}

		public string SceneName {
			get {
				return itemName;
			}
		}

		public Type Type {
			get {
				if (item)
					return item.GetType ();
				return null;
			}
		}

		#endregion

		#region Scene

		public void LoadScene ()
		{
			SceneManager.LoadScene (SceneName);
		}

		#endregion

		#region Core

		public T GetObjectAs<T> () where T : UnityEngine.Object
		{
			return item as T;
		}

		public bool Is<T> ()
		{
			return item is T;
		}

		public bool Is (Type type)
		{
			return item.GetType () == type;
		}

		#endregion

		#region Instantiate

		#region Normal Instantiate

		public UnityEngine.Object Instantiate ()
		{
			return MonoBehaviour.Instantiate (Object);
		}

		public UnityEngine.Object Instantiate (Transform parent)
		{
			return MonoBehaviour.Instantiate (Object, parent);
		}

		public UnityEngine.Object Instantiate (Vector3 position)
		{
			return MonoBehaviour.Instantiate (Object, position, Quaternion.identity);
		}

		public UnityEngine.Object Instantiate (Vector3 position, Quaternion rotation)
		{
			return MonoBehaviour.Instantiate (Object, position, rotation);
		}

		public UnityEngine.Object Instantiate (Vector3 position, Quaternion rotation, Transform parent)
		{
			return MonoBehaviour.Instantiate (Object, position, rotation, parent);
		}

		#endregion

		#region GameObject Instantiate

		public GameObject InstantiateAsGameObject ()
		{
			return MonoBehaviour.Instantiate (GameObject);
		}

		public GameObject InstantiateAsGameObject (Transform parent)
		{
			return MonoBehaviour.Instantiate (GameObject, parent);
		}

		public GameObject InstantiateAsGameObject (Vector3 position)
		{
			return MonoBehaviour.Instantiate (GameObject, position, Quaternion.identity);
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation)
		{
			return MonoBehaviour.Instantiate (GameObject, position, rotation);
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation, Transform parent)
		{
			return MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation, Vector3 scale)
		{
			var go = MonoBehaviour.Instantiate (GameObject, position, rotation);
			var goScale = go.transform.localScale;
			goScale.Scale (scale);
			go.transform.localScale = goScale;
			return go;
		}

		public GameObject InstantiateAsGameObject (Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
		{
			var go = MonoBehaviour.Instantiate (GameObject, position, rotation, parent);
			var goScale = go.transform.localScale;
			goScale.Scale (scale);
			go.transform.localScale = goScale;
			return go;
		}

		#endregion

		#endregion
	}
}