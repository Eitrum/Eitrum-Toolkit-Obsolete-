using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Eitrum.Database.Prefab{
	[CustomEditor(typeof(EiPrefabDatabase))]
	public class EiPrefabDatabaseEditor : Editor {
		#region Variables

		const string simplePath = "Eitrum/EiComponent/Database/Prefab/Items";
		const string configPath = "Assets/Eitrum/Configuration/EiPrefabDatabase.prefab";

		private List<EiPrefab> prefabs = new List<EiPrefab>();
		private int id = 0;

		#endregion

		#region Editor

		[MenuItem("Eitrum/Prefab Database")]
		public static void OpenPrefabDatabase() {
			var go = AssetDatabase.LoadAssetAtPath<GameObject>(configPath);
			if (!go) {
				var tempObj = new GameObject("EiPrefabDatabase", typeof(EiPrefabDatabase));
				go = PrefabUtility.CreatePrefab(configPath, tempObj);
				UnityEngine.MonoBehaviour.DestroyImmediate(tempObj);
			}
			var window = EditorWindow.GetWindow<EiPrefabDatabaseWindow>();
			window.database = go.GetComponent<EiPrefabDatabase>();
		}

		#endregion

		#region Core

		public override void OnInspectorGUI() {
			var db = (EiPrefabDatabase)target;

			prefabs.Clear();
			id = 0;

			DrawDatabase(db);
			FixID(db);
			EditorUtility.SetDirty(db);
		}

		#endregion

		#region ID fix

		private void FixID(EiPrefabDatabase db) {
			var cache = GetCachedItemList(db);
			cache.Clear();
			var list = GetSubCategoryList(db);
			for (int i = 0; i < list.Count; i++) {
				if (list[i] == null) {
					list.RemoveAt(i--);
					continue;
				}
				FixID(list[i], cache);
			}
		}

		private void FixID(EiPrefabSubCategory subCategory, List<EiPrefab> cache) {
			var list = subCategory.subCategories;
			for (int i = 0; i < list.Count; i++) {
				if (list[i] == null) {
					list.RemoveAt(i--);
					continue;
				}
				FixID(list[i], cache);
			}
			var items = subCategory.items;
			for (int i = 0; i < items.Count; i++) {
				ApplyItemId(items[i], id++);
				cache.Add(items[i]);
			}
		}

		#endregion

		#region Draw

		public static void DrawDatabase(EiPrefabDatabase db) {
			var subCatList = GetSubCategoryList(db);
			EditorGUILayout.LabelField(string.Format("Categories ({0})", subCatList.Count));
			for (int i = 0; i < subCatList.Count; i++) {
				var subCat = subCatList[i];
				if (subCat == null) {
					subCatList.RemoveAt(i--);
					continue;
				}
				DrawSubCategory(db, subCatList[i]);
			}

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Add Category", GUILayout.Width(100f))) {

				var dc = EiPrefabSubCategory.CreateAsset(simplePath);
				GetSubCategoryList(db).Add(dc);
			}

			if (GUILayout.Button("Clear", GUILayout.Width(100f))) {
				if (EditorUtility.DisplayDialog("Clear Database", "Do you really wanna clear the database?", "Yes", "No")) {
					ClearDatabase(db);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorUtility.SetDirty(db);
		}

		private static void DrawSubCategory(EiPrefabDatabase db, EiPrefabSubCategory subCategory) {
			var subCatList = subCategory.subCategories;
			var items = subCategory.items;

			EditorGUILayout.BeginHorizontal();
			subCategory.isFolded = EditorGUILayout.Foldout(subCategory.isFolded, "Sub Category", true);
			subCategory.categoryName = EditorGUILayout.TextField(subCategory.categoryName);
			if (GUILayout.Button("X", GUILayout.Width(24f))) {
				ClearSubCategory(subCategory);
				return;
			}
			EditorGUILayout.EndHorizontal();

			if (subCategory.isFolded) {
				EnterSubCategory();
				for (int i = 0; i < subCatList.Count; i++) {
					var subCat = subCatList[i];
					if (subCat == null) {
						subCatList.RemoveAt(i--);
						continue;
					}
					DrawSubCategory(db, subCatList[i]);
				}
				for (int i = 0; i < items.Count; i++) {
					var item = items[i];
					if (item == null) {
						items.RemoveAt(i--);
						continue;
					}
					DrawItem(db, item);
				}

				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("Add Item", GUILayout.Width(100f))) {
					var entryToAdd = EiPrefab.CreateAsset(simplePath);
					Undo.RecordObject(entryToAdd, "Add new Item");
					items.Add(entryToAdd);
					Undo.RecordObject(subCategory, "Added Item");
					EditorUtility.SetDirty(entryToAdd);
					EditorUtility.SetDirty(subCategory);
				}
				if (GUILayout.Button("Add Sub Category", GUILayout.Width(160f))) {
					var dc = EiPrefabSubCategory.CreateAsset(simplePath);
					subCatList.Add(dc);
					Undo.RecordObject(subCategory, "Added Category");
				}

				EditorGUILayout.EndHorizontal();
				LeaveSubCategory();
			}
			EditorUtility.SetDirty(subCategory);
		}

		private static void DrawItem(EiPrefabDatabase db, EiPrefab item) {
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(item, "Entry Change");
			ApplyItemName(item, item.Item ? item.Item.name : "empty");
			ApplyItemGameObject(item, EditorGUILayout.ObjectField(item.Item, typeof(GameObject), false) as GameObject);
			if (item.Database == null)
				ApplyItemDatabaseReference(item, db);
			if (GUILayout.Button("X", GUILayout.Width(24f))) {
				item.DestroyFile();
				EditorGUILayout.EndHorizontal();
				return;
			}
			EditorGUILayout.EndHorizontal();

#if EITRUM_POOLING
			var poolData = GetItemPoolData(item);
			var hasEntityComponent = item.GameObject ? item.GameObject.GetComponent<EiEntity>() != null : false;
			if (hasEntityComponent) {
				EditorGUILayout.BeginHorizontal();
				ApplyKeepPoolAlive(poolData, EditorGUILayout.ToggleLeft("Keep Pool Alive", poolData.KeepPoolAlive, GUILayout.MaxWidth(130f)));
				ApplyPoolSize(poolData, Math.Max(0, EditorGUILayout.IntField("Pool Size", poolData.PoolSize)));
				EditorGUILayout.EndHorizontal();
			}
			else {
				EditorGUILayout.LabelField("Pooling not enabled, need \"EiEntity\" component");
				ApplyKeepPoolAlive(poolData, false);
				ApplyPoolSize(poolData, 0);
			}
			poolData.GetType().GetField("prefab", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(poolData, item);
#endif
			GUILayout.Space(8f);
			EditorUtility.SetDirty(item);
		}

		#endregion

		#region SubCategory Helper

		private static void EnterSubCategory() {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(24f);
			EditorGUILayout.BeginVertical();
		}

		private static void LeaveSubCategory() {
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region Helpers

		public static void ClearDatabase(EiPrefabDatabase db) {
			var subCatList = GetSubCategoryList(db);
			for (int i = subCatList.Count - 1; i >= 0; i--)
				ClearSubCategory(subCatList[i]);
		}

		public static void ClearSubCategory(EiPrefabSubCategory subCategory) {
			var subCatList = subCategory.subCategories;
			for (int i = subCatList.Count - 1; i >= 0; i--)
				ClearSubCategory(subCatList[i]);

			var itemList = subCategory.items;
			for (int i = itemList.Count - 1; i >= 0; i--)
				itemList[i].DestroyFile();

			subCategory.DestroyFile();
		}

		#region Item Config

		public static void ApplyItemName(EiPrefab item, string name) {
			typeof(EiPrefab).GetField("itemName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, name);
		}

		public static void ApplyItemGameObject(EiPrefab item, GameObject go) {
			typeof(EiPrefab).GetField("item", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, go);
		}

		public static void ApplyItemId(EiPrefab item, int id) {
			typeof(EiPrefab).GetField("uniqueId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, id);
		}

		public static void ApplyItemDatabaseReference(EiPrefab item, EiPrefabDatabase db) {
			typeof(EiPrefab).GetField("database", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, db);
		}

		public static EiPoolData GetItemPoolData(EiPrefab item) {
			return typeof(EiPrefab).GetField("poolData", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(item) as EiPoolData;
		}

		public static void ApplyPoolSize(EiPoolData poolData, int value) {
			typeof(EiPoolData).GetField("poolSize", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(poolData, value);
		}

		public static void ApplyKeepPoolAlive(EiPoolData poolData, bool value) {
			typeof(EiPoolData).GetField("keepPoolAlive", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(poolData, value);
		}

		#endregion

		public static List<EiPrefab> GetCachedItemList(EiPrefabDatabase db) {
			return typeof(EiPrefabDatabase).GetField("cachedItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db) as List<EiPrefab>;
		}

		public static List<EiPrefabSubCategory> GetSubCategoryList(EiPrefabDatabase database) {
			return typeof(EiPrefabDatabase).GetField("subCategories", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(database) as List<EiPrefabSubCategory>;
		}

		public static List<EiPrefabSubCategory> GetSubCategoryList(EiPrefabSubCategory subCategory) {
			return typeof(EiPrefabSubCategory).GetField("subCategories", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(subCategory) as List<EiPrefabSubCategory>;
		}

		public static void ApplyCategoryName(EiPrefabSubCategory subCategory, string value) {
			typeof(EiPrefabSubCategory).GetField("categoryName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(subCategory, value);
		}

		#endregion
	}

	public class EiPrefabDatabaseWindow : EditorWindow {

		const string configPath = "Assets/Eitrum/Configuration/EiPrefabDatabase.prefab";
		public EiPrefabDatabase database;
		private Vector2 scroll;

		private void OnGUI() {
			scroll = EditorGUILayout.BeginScrollView(scroll);
			if (database) {
				EiPrefabDatabaseDrawer.DrawDatabase(database);
			}
			EditorGUILayout.EndScrollView();
		}
	}
}
