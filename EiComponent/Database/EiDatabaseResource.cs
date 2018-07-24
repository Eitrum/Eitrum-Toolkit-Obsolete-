using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Eitrum
{
    public class EiDatabaseResource : EiComponentSingleton<EiDatabaseResource>
    {
        #region Singleton

        public override bool KeepInResources()
        {
            return true;
        }

        #endregion

        #region Variables

        [SerializeField]
        private List<EiDatabaseCategory> categories = new List<EiDatabaseCategory>();
        [SerializeField]
        private int uniqueIdGenerator = 0;
		#endregion

		#region Properties

		public int _Length {
			get {
				return categories.Count;
			}
		}

		public EiDatabaseCategory this [int index] {
			get {
				return categories [index];
			}
		}

		#endregion

		#region Categories

		public EiDatabaseCategory _GetCategory (int index)
		{
			return categories [index];
		}

		public int _CategoriesLength ()
		{
			return categories.Count;
		}

		#endregion
	}
}