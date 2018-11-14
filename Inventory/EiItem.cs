using System;
using System.Collections.Generic;
using UnityEngine;
using Eitrum.Mathematics;

namespace Eitrum.Inventory
{
    [AddComponentMenu ("Eitrum/Inventory/Item")]
    public class EiItem : EiComponent, EiBufferInterface
    {
        protected struct EiItemSerializationConfiguration
        {
            #region Variables

            private string key;
            private Action<string> load;
            private Func<string> save;

            #endregion

            #region Properties

            public string Key {
                get {
                    return key;
                }
            }

            public Action<string> Load {
                get {
                    return load;
                }
            }

            public Func<string> Save {
                get {
                    return save;
                }
            }

            #endregion

            #region Constructor

            public EiItemSerializationConfiguration (string key, Action<string> load, Func<string> save)
            {
                this.key = key;
                this.load = load;
                this.save = save;
            }

            #endregion
        }

        #region Variables

        [Header ("Settings")]
        [Readonly]
        [Tooltip ("This can only be set if object is in asset folder")]
        [SerializeField]
        protected string prefabName;
        [SerializeField]
        protected int itemId = 0;
        [SerializeField]
        protected Sprite itemIcon;
        [SerializeField]
        protected Vector2 inventoryGridSize = new Vector2 (1, 1);
        [SerializeField]
        protected int maxStack = 1;
        [Space (8f)]
        [Tooltip ("Equipment id of -1 can't be equiped on gear")]
        [SerializeField]
        protected int equipmentId = -1;
        [SerializeField]
        protected bool saveItemBeforeWritingToDisk = true;
        [SerializeField]
        [Tooltip ("Only used when weight component is active and storage has weight calculation turned on")]
        protected bool useWeightInStorage = true;
        [SerializeField]
        protected EiWeight weight;

        [Header ("Non Serialized Data")]
        [SerializeField]
        protected string description = "";
        [Tooltip ("Quest item id at -1 will count as not being a quest item")]
        [SerializeField]
        protected int questItemId = -1;

        [Header ("Item Data")]
        [SerializeField]
        [Tooltip ("Seed used for random generation of item\nIf Left at 0, it will get a random seed each time its instantiated in scene.\nSeed will be saved in inventory.")]
        protected int seed = 0;
        [SerializeField]
        [Tooltip ("Only to be used if levels a thing and will be used in random generation")]
        protected int level = 0;
        [SerializeField]
        protected int amount = 1;
        [SerializeField]
        [Tooltip ("Serialized String Data for item generation")]
        protected string[] data = new string[0];

        protected Dictionary<string, EiItemSerializationConfiguration> savingPipeline = new Dictionary<string, EiItemSerializationConfiguration> ();

        [Header ("Custom Item Settings")]
        [SerializeField]
        protected bool saveUniqueItemName = false;

        #endregion

        #region Properties

        public virtual string PrefabName {
            get {
                return prefabName;
            }
        }

        public virtual int Seed {
            get {
                return seed;
            }
            set {
                seed = value;
            }
        }

        public virtual int Level {
            get {
                return level;
            }
            set {
                level = value;
            }
        }

        public virtual int Amount {
            get {
                return amount;
            }
            set {
                amount = Math.Min (maxStack, Math.Max (0, value));
            }
        }

        public virtual string Description {
            get {
                return description;
            }
        }

        public virtual int EquipmentId {
            get {
                return equipmentId;
            }
        }

        public virtual int ItemId {
            get {
                return itemId;
            }
        }

        public virtual int MaxStacks {
            get {
                return maxStack;
            }
        }

        public virtual bool UseWeightInStorage {
            get {
                return useWeightInStorage;
            }
        }

        public virtual float Weight {
            get {
                if (weight)
                    return weight.TotalWeight;
                return 0f;
            }
        }

        public virtual string ItemName {
            get {
                return Entity.EntityName;
            }
        }

        public virtual bool IsQuestItem {
            get {
                return questItemId >= 0;
            }
        }

        public virtual int QuestItemId {
            get {
                return questItemId;
            }
        }

        #endregion

        #region Core

        void Awake ()
        {
            if (amount > maxStack)
                Debug.LogWarning (ItemName + " has an amount higher then max stacks, items will be lost");
            if (seed == 0)
                seed = EiRandom.Int;
            if (saveUniqueItemName)
                AddSavingPipeline ("itemName", obj => Entity.EntityName = obj, () => Entity.EntityName);
        }

        public virtual string GetName ()
        {
            return Entity.EntityName;
        }

        #endregion

        #region Save/Load + Save pipeline

        public void Save ()
        {
            data = new string[savingPipeline.Count];
            int index = 0;
            foreach (var obj in savingPipeline) {
                data [index] = string.Format ("{0}={1}", obj.Key, obj.Value.Save ());
                index++;
            }
        }

        public void Load ()
        {
            var length = data.Length;
            for (int i = 0; i < length; i++) {
                var temp = data [i].Split ('=');
                string key = temp [0];
                string value = temp [1];
                if (savingPipeline.ContainsKey (key))
                    savingPipeline [key].Load (value);
                else {
                    Debug.LogWarning ("Saved Key is not in saving pipeline\nRemoving saved data from list");
                    data = data.Remove (i);
                    i--;
                }
            }
        }

        public void AddSavingPipeline (string keyword, Action<string> onLoadMethod, Func<string> onSaveMethod)
        {
            if (!savingPipeline.ContainsKey (keyword))
                savingPipeline.Add (keyword, new EiItemSerializationConfiguration (keyword, onLoadMethod, onSaveMethod));
            else
                Debug.LogWarning ("Item already has " + keyword + " as a keyword in its saving pipeline");
        }

        public void RemoveSavingPipeline (string keyword)
        {
            if (!savingPipeline.Remove (keyword))
                Debug.LogWarning ("Item do not contain " + keyword + " as an keyword and can not be deleted because of that");
        }

        [ContextMenu ("Clone Item")]
        public GameObject Clone ()
        {
            Save ();
            var reference = Resources.Load<GameObject> (prefabName);
            if (reference) {
                var go = Instantiate (reference);
                var itemClone = go.GetComponent<EiItem> ();
                itemClone.seed = seed;
                itemClone.level = level;
                itemClone.data = data;
                itemClone.Load ();
                return go;
            }
            return null;
        }

        #endregion

        #region EiBuffer Implementation

        public void WriteTo (EiBuffer buffer)
        {
            if (saveItemBeforeWritingToDisk)
                Save ();
            buffer.Write (seed);
            buffer.Write (level);
            buffer.Write (data.Length);
            buffer.Write (amount);
            for (int i = 0; i < data.Length; i++) {
                buffer.WriteASCII (data [i]);
            }
        }

        public void ReadFrom (EiBuffer buffer)
        {
            seed = buffer.ReadInt ();
            level = buffer.ReadInt ();
            data = new string[buffer.ReadInt ()];
            amount = buffer.ReadInt ();
            for (int i = 0; i < data.Length; i++) {
                data [i] = buffer.ReadASCII ();
            }
            Load ();
        }

        #endregion

        #if UNITY_EDITOR

        protected override void AttachComponents ()
        {
            ConnectPrefab ();
            weight = GetComponent<EiWeight> ();
            base.AttachComponents ();
        }

        /// <summary>
        /// Connects the prefab. Editor Only
        /// </summary>
        [ContextMenu ("Connect Prefab(Asset Objects Only)")]
        protected void ConnectPrefab ()
        {
            if (UnityEditor.AssetDatabase.Contains (this.gameObject)) {
                var path = UnityEditor.AssetDatabase.GetAssetPath (this.gameObject);

                if (path.Contains ("Resources")) {
                    path = path.Split (new string[]{ "Resources" }, StringSplitOptions.None) [1].Remove (0, 1).Replace (".prefab", "");
                    prefabName = path;
                    if (itemId == 0)
                        itemId = prefabName.GetHashCode ();
                } else {
                    Debug.LogWarning ("Object do not have an path in resources");
                }
            }
        }

        #endif
    }
}