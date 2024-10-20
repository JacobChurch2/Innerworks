using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamSizeChange : MonoBehaviour
{
	[SerializeField]
	public CinemachineCamera cam;

	public float OrthographicSize;

	public float TransitionTime;

	private float OriginalSize;

	private static int Boxes;

	private void Start()
	{
		OriginalSize = cam.Lens.OrthographicSize;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			foreach (CamSizeChange camera in Resources.FindObjectsOfTypeAll<CamSizeChange>())
			{
				camera.StopAllCoroutines();
			}
			StartCoroutine(SizeLerp(cam.Lens.OrthographicSize, OrthographicSize, TransitionTime));
			Boxes++;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			if ((Boxes <= 1))
			{
				foreach (CamOffset camera in Resources.FindObjectsOfTypeAll<CamOffset>())
				{
					camera.StopAllCoroutines();
				}
				StartCoroutine(SizeLerp(cam.Lens.OrthographicSize, OriginalSize, TransitionTime));
			}
			Boxes--;
		}
	}

	IEnumerator SizeLerp(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			cam.Lens.OrthographicSize = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		cam.Lens.OrthographicSize = endValue;
	}
}
