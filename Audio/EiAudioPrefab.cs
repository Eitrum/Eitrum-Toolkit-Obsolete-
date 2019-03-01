using System;
using UnityEngine;

namespace Eitrum.Audio
{
	[CreateAssetMenu (fileName = "Audio Prefab", menuName = "Eitrum/Audio/Audio Prefab", order = 20)]
	public class EiAudioPrefab : EiScriptableObject
	{
		#region Variables

		[Header ("Settings")]
		public string prefabName = "";
		public string path = "";
		public int uniqueId = 0;
		
		[Header ("Pool Settings")]
		public int maxInstances = 8;

		public EiAudio[] audioFiles;

		#endregion

		#region Properties

		public string Name {
			get {
				return prefabName;
			}
		}

		public int Id {
			get {
				return uniqueId;
			}
		}

		#endregion

		#region Audio Search

		public EiAudio GetAudioByIndex (int index)
		{
			return audioFiles [index];
		}

		public EiAudio GetAudioByName (string name)
		{
			for (int i = 0; i < audioFiles.Length; i++) {
				if (audioFiles [i].name == name)
					return audioFiles [i];
			}
			return null;
		}

		public EiAudio GetRandomVariation ()
		{
			return audioFiles.RandomElement ();
		}

		#endregion

		#region Play Methods

		public void Play ()
		{
			
		}

		#endregion
	}
}