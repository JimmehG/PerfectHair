using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSoundtrack : MonoBehaviour
{
	[Header("Soundtrack Stages")]

	[SerializeField]
	AudioSource[] audioSources;

	[Header("Current State")]

	[ReadOnlyAttribute]
	[SerializeField]
	int currentStage;

	void Start()
	{
		currentStage = 0;
		if (audioSources.Length > 1)
		{
			for (int i = 1; i < audioSources.Length; i++)
			{
				audioSources [i].volume = 0.0f;
			}
		}
	}

	public void IncreaseSoundtrackStage()
	{
		if ((currentStage + 1) < audioSources.Length)
		{
			StartCoroutine (TransitionToNextStage());
		}
	}

	private IEnumerator TransitionToNextStage()
	{
		int nextStage = currentStage + 1;
		while (audioSources[nextStage].volume < 1.0f)
		{
			audioSources[nextStage].volume = audioSources[nextStage].volume + 0.1f;
			audioSources[currentStage].volume = audioSources[currentStage].volume - 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		currentStage = nextStage;
	}
}
