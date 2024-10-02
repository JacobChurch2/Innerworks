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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("f"))
		{
			StartCoroutine(FadeOut());
		}
	}

	IEnumerator FadeOut()
	{
		for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator FadeIn()
	{
		for (float alpha = 0f; alpha >= 1; alpha += 0.1f)
		{
			Text.color = new Color(1f, 1f, 1f, alpha);
			yield return new WaitForSeconds(.01f);
		}
	}
}
