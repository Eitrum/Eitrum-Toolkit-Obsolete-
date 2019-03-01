using System;
using UnityEngine;

namespace Eitrum.Audio
{
	public class EiAudioEngine : EiScriptableObject
	{
		#region Properties

		public static double Time {
			get {
				return AudioSettings.dspTime;
			}
		}

		#endregion
	}
}