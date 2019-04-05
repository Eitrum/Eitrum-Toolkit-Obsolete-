using System.Diagnostics;
using Eitrum;
using UnityEngine;

public class PerformanceTest : EiComponent {
    
    #region Hide block 1
    [Header("Basic Settings")]
    public int testsPerFrame = 10;
    public int iterationsPerTest = 100;
    public bool drawGUI = false;

    public Stopwatch sw = new Stopwatch();

    private long ticks1Result = 0;
    private long ticks2Result = 0;

    private void Awake() {
        SubscribeUpdate();
    }

    public override void UpdateComponent(float time) {
        long ticks1 = 0;
        long ticks2 = 0;

        for (int tests = 0; tests < testsPerFrame; tests++) {
            sw.Reset();
            sw.Start();
            for (int i = 0; i < iterationsPerTest; i++) {
                #endregion
                /// Test Case 1 ///
                


                /// ----------- ///
                #region Hide Block 2
            }
            ticks1 += sw.ElapsedTicks;
            sw.Reset();
            sw.Start();
            for (int i = 0; i < iterationsPerTest; i++) {
                #endregion
                /// Test Case 2 ///

                

                /// ----------- ///
                #region Hide Block 3
            }
            ticks2 += sw.ElapsedTicks;
        }

        UnityEngine.Debug.LogFormat("Test Case 1 - {0} ticks", ticks1Result = (ticks1 / testsPerFrame));
        UnityEngine.Debug.LogFormat("Test Case 2 - {0} ticks", ticks2Result = (ticks2 / testsPerFrame));
    }
    #endregion

    #region Hide Block 4

    private void OnGUI() {
        if (!drawGUI)
            return;
        var box = new Rect(10, 10, 300, 300);
        GUI.Box(box, "Performance Test");

        GUILayout.BeginArea(box);
        GUILayout.Space(20f);
        GUILayout.Label(string.Format("Test Case 1 - {0} ticks", ticks1Result));
        GUILayout.Label(string.Format("Test Case 2 - {0} ticks", ticks2Result));

        GUILayout.EndArea();
    }

    #endregion
}
