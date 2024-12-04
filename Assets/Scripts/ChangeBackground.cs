using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer bg11, bg12, bg13, bg21, bg22, bg23;

    private PlayableDirector anim;

	private void Start()
	{
		anim = GetComponent<PlayableDirector>();
		anim.played += ChangeTheBackground;
	}

	private void ChangeTheBackground(PlayableDirector playable)
	{
		StartCoroutine(FadeBackground(bg11, 255, 0));
		StartCoroutine(FadeBackground(bg12, 255, 0));
		StartCoroutine(FadeBackground(bg13, 255, 0));
		StartCoroutine(FadeBackground(bg21, 0, 255));
		StartCoroutine(FadeBackground(bg22, 0, 255));
		StartCoroutine(FadeBackground(bg23, 0, 255));
	}

	private IEnumerator FadeBackground(SpriteRenderer bg, float start, float end)
	{
		float timeElapsed = 0;
		float duration = (float) anim.duration;

		while (timeElapsed < duration)
		{
			bg.color =new Color(bg.color.r, bg.color.g, bg.color.b, Mathf.Lerp(start, end, timeElapsed / duration));
			timeElapsed += Time.fixedDeltaTime;

			yield return null;
		}

		bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, end);
	}
}
