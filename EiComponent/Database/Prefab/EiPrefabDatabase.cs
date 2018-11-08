using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Database
{
	public class EiPrefabDatabase : EiComponentSingleton<EiPrefabDatabase>
	{
		#region Singleton

		public override bool KeepInResources ()
		{
			return true;
		}

		#endregion

		#region Variables

		[SerializeField]
		private List<EiPrefab> cachedItems = new List<EiPrefab> ();

		#endregion

		#region Properties

		public int Length {
			get {
				return cachedItems.Count;
			}
		}

		public EiPrefab this [int index] {
			get {
				return cachedItems [index];
			}
		}

		#endregion

		#region Core

		void Awake ()
		{
			if (instance) {
				DestroyImmediate (this.gameObject);
			}
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		#endregion

		#region Get

		public EiPrefab Get (int index)
		{
			return cachedItems [index];
		}

		#endregion

		#region Editor

		[ContextMenu ("Build And Prefill database")]
		private void BuildAndPrefillDatabase ()
		{
			if (!gameObject.scene.IsValid ()) {
				Debug.Log ("Has to be in scene");
				return;
			}

		}

		#endregion
	}
}