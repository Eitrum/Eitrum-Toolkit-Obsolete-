using System;
using UnityEngine;

namespace Eitrum.Audio
{
	[Serializable]
	public class EiAudio
	{
		#region Variables

		public string name;
		public AudioClip audioClip;

		[Header ("Volume Settings")]
		public float volume = 1f;
		public float pitch = 1f;
		public float minDistance = 1f;
		public float maxDistance = 500f;
		public AnimationCurve volumeRolloffCurve;

		[Header ("Audio Mode Settings")]
		[Range (-1f, 1f)]
		public float pan2D = 0f;
		public AnimationCurve panLevelCurve;

		[Header ("Other Settings")]
		public int priority = 128;
		public float dopplerLevel = 1f;
		public AnimationCurve speadCurve;

		[Header ("Reverb Zone Settings")]
		public bool bypassEffects = false;
		public bool bypassListenerEffects = false;
		public bool bypassReverbZones = false;
		public AnimationCurve reverbZoneMixCurve;

		[Header ("Spatialize Settings")]
		public bool spatialize;
		public bool spatializePostEffects;

		#endregion
	}
}