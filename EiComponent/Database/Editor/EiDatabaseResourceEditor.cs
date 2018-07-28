using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Eitrum
{
	[CustomEditor(typeof(EiDatabaseResource))]
	public class EiDatabaseResourceEditor : Editor
	{
		#region Variables

		static string simplePath = "Eitrum/EiComponent/Database/Items";

		private FieldInfo categoryList = null;
		private FieldInfo subCategoryList = null;
		private FieldInfo entryList = null;
		private FieldInfo allocateId = null;
		private FieldInfo itemUniqueId = null;


		#endregion

		#region OnInspectorGUI

		public override void OnInspectorGUI()
		{
			var database = (EiDatabaseResource)target;

			Undo.RecordObject(database, "Database Changes");
			if (categoryList == null)
			{
				categoryList = typeof(EiDatabaseResource).GetField("categories", BindingFlags.NonPublic | BindingFlags.Instance);
				if (categoryList == null)
					return;
			}
			if (subCategoryList == null)
			{
				subCategoryList = typeof(EiDatabaseCategory).GetField("subCategories", BindingFlags.NonPublic | BindingFlags.Instance);
				if (subCategoryList == null)
					return;
			}
			if (entryList == null)
			{
				entryList = typeof(EiDatabaseCategory).GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance);
				if (entryList == null)
					return;
			}
			DrawDatabase(database);
			CheckUniqueId(database);
			EditorUtility.SetDirty(database);
		}

		#endregion

		#region Unique Id Generator

		private void CheckUniqueId(EiDatabaseResource db)
		{
			if (allocateId == null)
				allocateId = typeof(EiDatabaseResource).GetField("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
			allocateId.SetValue(db, 0);

			var dbLength = db._Length;
			for (int i = 0; i < db._Length; i++)
			{
				var cat = db[i];
				CheckSubCategory(db, cat);
			}
		}

		private void CheckSubCategory(EiDatabaseResource db, EiDatabaseCategory cat)
		{
			var subs = GetSubCategories(cat);
			for (int i = 0; i < subs.Count; i++)
			{
				CheckSubCategory(db, subs[i]);
			}
			var items = GetEntries(cat);
			for (int i = 0; i < items.Count; i++)
			{
				CheckItem(db, items[i]);
			}
		}

		private void CheckItem(EiDatabaseResource db, EiDatabaseItem item)
		{
			AllocateUniqueId(db, item);
		}

		private int AllocateUniqueId(EiDatabaseResource db, EiDatabaseItem item)
		{
			if (allocateId == null)
				allocateId = typeof(EiDatabaseResource).GetField("uniqueIdGenerator", BindingFlags.NonPublic | BindingFlags.Instance);
			if (itemUniqueId == null)
				itemUniqueId = typeof(EiDatabaseItem).GetField("uniqueId", BindingFlags.NonPublic | BindingFlags.Instance);


			var val = (int)allocateId.GetValue(db) + 1;
			allocateId.SetValue(db, val);
			itemUniqueId.SetValue(item, val);
			return val;
		}

		#endregion

		#region Draw

		private void DrawDatabase(EiDatabaseResource db)
		{
			var dbLength = db._Length;
			EditorGUILayout.LabelField(string.Format("Categories ({0})", dbLength));

			for (int i = 0; i < db._Length; i++)
			{
				var cat = db[i];
				if (!DrawCategory(db, cat, i))
				{
					DeleteCategory(cat);
					GetCategories(db).RemoveAt(i);
					cat.DestroyFile();
					i--;
				}
			}

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Add Category", GUILayout.Width(100f)))
			{
				var dc = EiDatabaseCategory.CreateAsset(simplePath);
				GetCategories(db).Add(dc);
			}

			if (GUILayout.Button("Clear", GUILayout.Width(100f)))
			{
				if (EditorUtility.DisplayDialog("Clear Database", "Do you really wanna clear the database?", "Yes", "No"))
				{
					ClearDatabase(db);
					GetCategories(db).Clear();
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private bool DrawCategory(EiDatabaseResource database, EiDatabaseCategory category, int index)
		{
			EditorGUILayout.BeginHorizontal();
			category.isFolded = EditorGUILayout.Foldout(category.isFolded, "Entries (" + category.GetEntriesLength() + ")", true);
			SetCategoryName(category, EditorGUILayout.TextField(category.CategoryName));
			if (GUILayout.Button("X", GUILayout.Width(24f)))
			{
				EditorGUILayout.EndHorizontal();
				return false;
			}
			EditorGUILayout.EndHorizontal();

			if (category.isFolded)
			{
				BeginSubCategory();

				var subCategories = GetSubCategories(category);
				for (int i = 0; i < subCategories.Count; i++)
				{
					var cat = subCategories[i];
					if (!DrawCategory(database, cat, i))
					{
						DeleteCategory(cat);
						GetSubCategories(category).RemoveAt(i);
						cat.DestroyFile();
						i--;
					}
				}

				for (int i = 0; i < category.GetEntriesLength(); i++)
				{
					var ent = category.GetEntry(i);
					if (!DrawEntry(ent, i))
					{
						GetEntries(category).RemoveAt(i);
						ent.DestroyFile();
						i--;
					}
				}

				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("Add Item", GUILayout.Width(100f)))
				{
					var entryToAdd = EiDatabaseItem.CreateAsset(simplePath);
					Undo.RecordObject(entryToAdd, "Entry Change");
					ApplyDatabase(entryToAdd, database);
					GetEntries(category).Add(entryToAdd);
					EditorUtility.SetDirty(entryToAdd);
				}
				if (GUILayout.Button("Add Sub Category", GUILayout.Width(160f)))
				{
					subCategories.Add(new EiDatabaseCategory());
				}
				var obj = EditorGUILayout.ObjectField(null, typeof(UnityEngine.Object), false, GUILayout.Width(100f));
				if (obj)
				{
					var entryToAdd = EiDatabaseItem.CreateAsset(simplePath);
					Undo.RecordObject(entryToAdd, "Entry Change");
					ApplyDatabase(entryToAdd, database);
					SetEntryObject(entryToAdd, obj);
					GetEntries(category).Add(entryToAdd);
					EditorUtility.SetDirty(entryToAdd);
				}

				EditorGUILayout.EndHorizontal();

				EndSubCategory();
			}
			return true;
		}

		private bool DrawEntry(EiDatabaseItem entry, int index)
		{
			EditorGUILayout.BeginHorizontal();
			Undo.RecordObject(entry, "Entry Change");
			SetEntryName(entry, entry.Item ? entry.Item.name : "empty");
			SetEntryObject(entry, EditorGUILayout.ObjectField(entry.Item, typeof(UnityEngine.Object), false));
			if (GUILayout.Button("X", GUILayout.Width(24f)))
			{
				EditorGUILayout.EndHorizontal();
				return false;
			}
			EditorGUILayout.EndHorizontal();
			EditorUtility.SetDirty(entry);
			return true;
		}

		#endregion

		#region Offset Fixes

		private void BeginSubCategory()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(12f);
			EditorGUILayout.BeginVertical();
		}

		private void EndSubCategory()
		{
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		#endregion

		#region Helpers

		public void ClearDatabase(EiDatabaseResource resource)
		{
			for (int i = resource._Length - 1; i >= 0; i--)
			{
				DeleteCategory(resource[i]);
				resource[i].DestroyFile();
			}
		}

		public void DeleteCategory(EiDatabaseCategory category)
		{
			var entries = GetEntries(category);
			for (int i = 0; i < entries.Count; i++)
			{
				entries[i].DestroyFile();
			}
		}

		public void ApplyDatabase(EiDatabaseItem item, EiDatabaseResource database)
		{
			typeof(EiDatabaseItem).GetField("database", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, database);
		}

		public void SetCategoryName(EiDatabaseCategory category, string name)
		{
			var field = typeof(EiDatabaseCategory).GetField("categoryName", BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue(category, name);
		}

		public void SetEntryName(EiDatabaseItem entry, string name)
		{
			entry.GetType().GetField("itemName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(entry, name);
		}

		public void SetEntryObject(EiDatabaseItem entry, UnityEngine.Object item)
		{
			entry.GetType().GetField("item", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(entry, item);
		}

		public List<EiDatabaseCategory> GetSubCategories(EiDatabaseCategory category)
		{
			return subCategoryList.GetValue(category) as List<EiDatabaseCategory>;
		}

		public List<EiDatabaseCategory> GetCategories(EiDatabaseResource database)
		{
			return categoryList.GetValue(database) as List<EiDatabaseCategory>;
		}

		public List<EiDatabaseItem> GetEntries(EiDatabaseCategory category)
		{
			return entryList.GetValue(category) as List<EiDatabaseItem>;
		}

		#endregion
	}
}