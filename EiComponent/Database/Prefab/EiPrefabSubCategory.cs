using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Database
{
	public class EiPrefabSubCategory : EiScriptableObject<EiPrefabSubCategory>
	{
		#region Variables
		
		public string categoryName = "";
		public List<EiPrefabSubCategory> subCategories = new List<EiPrefabSubCategory>();
		public List<EiPrefab> items = new List<EiPrefab>();

#if UNITY_EDITOR

		[SerializeField]
		public bool isFolded = false;

#endif

		#endregion

		#region Properties

		public string CategoryName
		{
			get
			{
				return categoryName;
			}
		}

		#endregion
	}
}
