using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Eitrum
{
	[Serializable]
	public class EiEntry
	{
		#region Variables

		[Readonly]
		[SerializeField]
		private string itemName = "empty";
		[SerializeField]
		private UnityEngine.Object item;
		[Readonly]
		[SerializeField]
		private int uniqueId = -1;

		#endregion

		#region Properties

		public string ItemName {
			get {
				return itemName;
			}
		}

		public int UniqueId {
			get {
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

		#endregion

		#region Core

		public T GetObjectAs<T> () where T : UnityEngine.Object
		{
			return item as T;
		}

		#endregion
	}
}