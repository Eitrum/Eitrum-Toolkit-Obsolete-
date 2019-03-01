using System;
using UnityEngine;

namespace Eitrum.Audio
{
	public static class EiAudioHelper
	{
		#region Assign Configuration

		public static void AssignAudioSource (AudioSource source, float volume)
		{
			source.volume = volume;
			
		}

		#endregion
	}
}