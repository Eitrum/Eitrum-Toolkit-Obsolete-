using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Eitrum.PasswordEditor {
    public class EiPasswordGenerator : EditorWindow {

        private string input = "";

        [MenuItem ("Eitrum/Password Generator")]
        public static void ShowWindow () {
            var win = GetWindow<EiPasswordGenerator> ();
            win.Show ();
        }

        void OnGUI () {
            var area = new Rect (0, 0, this.position.width, this.position.height);
            GUILayout.BeginArea (area);
            input = EditorGUILayout.TextField ("Key", input);

            string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            int poolLength = pool.Length;
            var inputKey = input;
            var hashKey = inputKey.ToByte().Select(x=>(int)x).Sum();
            EiRandom random = new EiRandom (hashKey);
            var length = 16;

            string outputKey = "";
            for (int i = 0; i < length; i++) {
                var t = "" + (pool[random._Range (0, poolLength)]);
                if (random._Range (2) == 0)
                    t = t.ToUpper ();
                outputKey += t;
            }
            EditorGUILayout.TextField("Output", outputKey);

            GUILayout.EndArea ();
        }
    }
}