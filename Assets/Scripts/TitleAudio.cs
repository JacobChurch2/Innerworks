using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TitleAudio : MonoBehaviour
{
	[SerializeField]
	private AudioClip IdleMusic;

	private AudioSource TheSource;

	private float DelayTimer = 90;

	private bool Switched = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		TheSource = GetComponent<AudioSource>();
		StartCoroutine(FadeMusic(0, 1, 1));
	}

	// Update is called once per frame
	void Update()
	{
		if (Switched) return;

		DelayTimer -= Time.deltaTime;

		if (DelayTimer <= 0)
		{
			StartCoroutine(SwitchMusic());
			Switched = true;
		}
	}

	private IEnumerator SwitchMusic()
	{
		yield return FadeMusic(1, 0, 1f);
		TheSource.clip = IdleMusic;
		TheSource.Play();
		StartCoroutine(FadeMusic(0, 1, 1f));
	}

	IEnumerator FadeMusic(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			TheSource.volume = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		TheSource.volume = endValue;
	}
}
