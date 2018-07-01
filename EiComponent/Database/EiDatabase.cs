using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Eitrum
{
	public class EiDatabase : EiComponentSingleton<EiDatabase>
	{

		#region Singleton

		public override bool KeepInResources ()
		{
			return true;
		}

		public override void SingletonCreation ()
		{
			totalEntries = 0;
			for (int i = 0; i < categories.Count; i++) {
				var category = categories [i];
				var entriesLength = category.Length;
				for (int e = 0; e < entriesLength; e++) {
					var entry = category [e];
					var uid = entry.UniqueId;
					var item = entry.Object;
					dictionaryObjectLookup.Add (uid, item);
					dictionaryEntryLookup.Add (uid, entry);
					totalEntries++;
				}
			}
		}

		#endregion

		#region Variables

		[SerializeField]
		#pragma warning disable
		private int uniqueIdGenerator = 0;
		private int totalEntries = 0;
		[SerializeField]
		private List<EiCategory> categories = new List<EiCategory> ();
		private Dictionary<int, UnityEngine.Object> dictionaryObjectLookup = new Dictionary<int, UnityEngine.Object> ();
		private Dictionary<int, EiEntry> dictionaryEntryLookup = new Dictionary<int, EiEntry> ();

		#endregion

		#region Properties

		public int _TotalEntries {
			get {
				return totalEntries;
			}
		}

		public int _Length {
			get {
				return categories.Count;
			}
		}

		public EiCategory this [int index] {
			get {
				return categories [index];
			}
		}

		#endregion

		#region Categories

		public EiCategory _GetCategory (int index)
		{
			return categories [index];
		}

		public int _CategoriesLength ()
		{
			return categories.Count;
		}

		#endregion

		#region GetObject

		public EiEntry _GetEntry (int uniqueId)
		{
			if (dictionaryEntryLookup.ContainsKey (uniqueId))
				return dictionaryEntryLookup [uniqueId];
			return null;
		}

		public UnityEngine.Object _GetObject (int uniqueId)
		{
			if (dictionaryObjectLookup.ContainsKey (uniqueId))
				return dictionaryObjectLookup [uniqueId];
			return null;
		}

		public GameObject _GetGameObject (int uniqueId)
		{
			return _GetObject (uniqueId) as GameObject;
		}

		public AudioClip _GetAudioClip (int uniqueId)
		{
			return _GetObject (uniqueId) as AudioClip;
		}

		public Animation _GetAnimation (int uniqueId)
		{
			return _GetObject (uniqueId) as Animation;
		}

		public string _GetSceneName (int uniqueId)
		{
			return _GetEntry (uniqueId).SceneName;
		}

		public T _GetObjectAs<T> (int uniqueId) where T : UnityEngine.Object
		{
			return _GetObject (uniqueId) as T;
		}

		public void _LoadScene (int uniqueId)
		{
			SceneManager.LoadScene (_GetEntry (uniqueId).SceneName);
		}

		#endregion

		#region Static

		#region Static.Properties

		public static int TotalEntries {
			get {
				return Instance._TotalEntries;
			}
		}

		public static int Length {
			get {
				return Instance._Length;
			}
		}

		#endregion

		#region Static.Categories

		public static EiCategory GetCategory (int index)
		{
			return Instance._GetCategory (index);
		}

		public static int CategoriesLength ()
		{
			return Instance._CategoriesLength ();
		}

		#endregion

		#region Static.GetObject

		public static EiEntry GetEntry (int uniqueId)
		{
			return Instance._GetEntry (uniqueId);
		}

		public static UnityEngine.Object GetObject (int uniqueId)
		{
			return Instance._GetObject (uniqueId);
		}

		public static GameObject GetGameObject (int uniqueId)
		{
			return Instance._GetGameObject (uniqueId);
		}

		public static AudioClip GetAudioClip (int uniqueId)
		{
			return Instance._GetAudioClip (uniqueId);
		}

		public static Animation GetAnimation (int uniqueId)
		{
			return Instance._GetAnimation (uniqueId);
		}

		public static string GetSceneName (int uniqueId)
		{
			return Instance._GetSceneName (uniqueId);
		}

		public static T GetObjectAs<T> (int uniqueId) where T : UnityEngine.Object
		{
			return Instance._GetObjectAs<T> (uniqueId);
		}

		public static void LoadScene (int uniqueId)
		{
			Instance._LoadScene (uniqueId);
		}

		#endregion

		#endregion
	}
}