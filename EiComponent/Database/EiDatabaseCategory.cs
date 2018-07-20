using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum
{
    [Serializable]
    public class EiDatabaseCategory
    {
        #region Variables

        [SerializeField]
        private string categoryName = "";
        [SerializeField]
        #pragma warning disable
        private List<EiDatabaseCategory> subCategories = new List<EiDatabaseCategory>();
        [SerializeField]
        private List<EiDatabaseItem> entries = new List<EiDatabaseItem>();

#if UNITY_EDITOR
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

        #region Core

        public int GetSubCategoriesLength()
        {
            return subCategories.Count;
        }

        public EiDatabaseCategory GetSubCategory(int index)
        {
            return subCategories[index];
        }

        public int GetEntriesLength()
        {
            return entries.Count;
        }

        public EiDatabaseItem GetEntry(int index)
        {
            return entries[index];
        }

        #endregion
    }
}