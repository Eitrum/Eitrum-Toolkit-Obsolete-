using System.Diagnostics;
using Eitrum;
using UnityEngine;

public class PerformanceTest : EiComponent
{


	#region Hide block 1
	public int testsPerFrame = 10;
	public int iterationsPerTest = 100;

	public Stopwatch sw = new Stopwatch();

	private void Awake()
	{
		SubscribeUpdate();
	}

	public override void UpdateComponent(float time)
	{
		long ticks1 = 0;
		long ticks2 = 0;

		for (int tests = 0; tests < testsPerFrame; tests++)
		{
			sw.Reset();
			sw.Start();
			for (int i = 0; i < iterationsPerTest; i++)
			{
				#endregion
				/// Test Case 1 ///



				/// ----------- ///
				#region Hide Block 2
			}
			ticks1 += sw.ElapsedTicks;
			sw.Reset();
			sw.Start();
			for (int i = 0; i < iterationsPerTest; i++)
			{
				#endregion
				/// Test Case 2 ///



				/// ----------- ///
				#region Hide Block 3
			}
			ticks2 += sw.ElapsedTicks;
		}

		UnityEngine.Debug.LogFormat("Test Case 1 - {0} ticks", ticks1 / testsPerFrame);
		UnityEngine.Debug.LogFormat("Test Case 2 - {0} ticks", ticks2 / testsPerFrame);
	}
	#endregion


}