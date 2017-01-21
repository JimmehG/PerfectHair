using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Audio", menuName = "Hair/Button Audio")]
public class ButtonAudio : ScriptableObject
{
	public string buttonName;

	[SerializeField]
	AudioClip[] audioClips;

	public AudioClip getRandomAudioClip()
	{
		return audioClips [Random.Range (0, audioClips.Length)];
	}
}

