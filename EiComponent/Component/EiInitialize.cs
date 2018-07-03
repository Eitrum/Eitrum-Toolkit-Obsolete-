using System;
using UnityEngine;

namespace Eitrum
{
	[AddComponentMenu ("Eitrum/Core/Initialize")]
	public class EiInitialize : EiComponent
	{
		void Awake ()
		{
			var updateSystem = EiUpdateSystem.Instance;
			var timer = EiTimer.Instance;
			var threadingFix = EiUnityThreading.Instance;

		}
	}
}