using System;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu ("Eitrum/Initialize Fix")]
	public class EiInitialize
	{
		void Awake ()
		{
			var updateSystem = EiUpdateSystem.Instance;
			var timer = EiTimer.Instance;
			var threadingFix = EiUnityThreading.Instance;

		}
	}
}