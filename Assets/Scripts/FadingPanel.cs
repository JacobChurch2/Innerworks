using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class FadingPanel : MonoBehaviour
{
    TextMeshPro Text;

	private void Awake()
	{
		Text = GetComponent<TextMeshPro>();
	}

	public IEnumerator FadeOut()
	{
		for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator FadeIn()
	{
		for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator FadeOutRealTime()
	{
		for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSecondsRealtime(0.01f);
		}
	}

	public IEnumerator FadeInRealTime()
	{
		for (float alpha = 0f; alpha <= 1; alpha += 0.01f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSecondsRealtime(0.01f);
		}
	}
}
