using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Eitrum.Database
{
	public class EiPrefabDatabase : EiComponentSingleton<EiPrefabDatabase>
	{
		#region Singleton

		public override bool KeepInResources()
		{
			return true;
		}

		#endregion

		#region Variables

		[SerializeField]
		private List<EiPrefabSubCategory> subCategories = new List<EiPrefabSubCategory>();

		[SerializeField]
		private List<EiPrefab> cachedItems = new List<EiPrefab>();

		#endregion

		#region Core



		#endregion

		#region Static Getters

		#endregion
	}
}
