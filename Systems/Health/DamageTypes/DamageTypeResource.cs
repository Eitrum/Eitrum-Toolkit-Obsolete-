using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eitrum.Health {
    [CreateAssetMenu(fileName = "DamageTypeResource", menuName = "Eitrum/Database/Damage Type Resource", order = 10)]
    public class DamageTypeResource : EiScriptableObjectSingleton<DamageTypeResource> {
        [SerializeField]
        private List<string> damageCategories = new List<string>();

        public int Length {
            get {
                return damageCategories.Count;
            }
        }

        public string this[int index] {
            get {
                return damageCategories[index];
            }
        }
    }
}