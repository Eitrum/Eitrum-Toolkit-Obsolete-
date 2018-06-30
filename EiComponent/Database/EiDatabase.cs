using System;
using UnityEngine;
using System.Collections.Generic;

namespace Eitrum {
    public class EiDatabase : EiComponentSingleton<EiDatabase> {

        #region Singleton

        public override bool KeepInResources() {
            return true;
        }

        public override void SingletonCreation() {
            for(int i = 0; i < categories.Count; i++) {
                var entries = categories[i].entries;
                for(int e = 0; e < entries.Count; e++) {
                    var uid = entries[e].uniqueId;
                    var item = entries[e].item;
                    dictionaryLookup.Add(uid, item);
                }
            }
        }

        #endregion

        #region Variables

        public int uniqueIdGeneration = 0;
        public List<EiCategory> categories = new List<EiCategory>();
        private Dictionary<int, UnityEngine.Object> dictionaryLookup = new Dictionary<int, UnityEngine.Object>();

        #endregion

        #region Get Object

        public UnityEngine.Object GetObject(int uniqueId) {
            if(dictionaryLookup.ContainsKey(uniqueId))
                return dictionaryLookup[uniqueId];
            return null;
        }

        public GameObject GetGameObject(int uniqueId) {
            return GetObject(uniqueId) as GameObject;
        }

        public AudioClip GetAudioClip(int uniqueId) {
            return GetObject(uniqueId) as AudioClip;
        }

        public Animation GetAnimation(int uniqueId) {
            return GetObject(uniqueId) as Animation;
        }

        public T GetObjectAs<T>(int uniqueId) where T : UnityEngine.Object {
            return GetObject(uniqueId) as T;
        }

        #endregion
    }
}