using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamOffset : MonoBehaviour
{
	[SerializeField]
	public CinemachineFollow cam;

	public float yOffset;
	//public float xOffset;

	public float TransitionTime;

	private float yOffsetOriginal;
	//private float xOffsetOriginal;

	private static int Boxes;

	private void Start()
	{
		//xOffsetOriginal = cam.FollowOffset.x;
		yOffsetOriginal = cam.FollowOffset.y;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			foreach (CamOffset camera in Resources.FindObjectsOfTypeAll<CamOffset>())
			{
				camera.StopAllCoroutines();
			}
			//StartCoroutine(xOffsetLerp(cam.FollowOffset.x, xOffset, TransitionTime));
			StartCoroutine(yOffsetLerp(cam.FollowOffset.y, yOffset, TransitionTime));
			Boxes++;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			if ((Boxes <= 1)){
				foreach (CamOffset camera in Resources.FindObjectsOfTypeAll<CamOffset>())
				{
					camera.StopAllCoroutines();
				}
				//StartCoroutine(xOffsetLerp(cam.FollowOffset.x, xOffsetOriginal, TransitionTime));
				StartCoroutine(yOffsetLerp(cam.FollowOffset.y, yOffsetOriginal, TransitionTime));
			}
			Boxes--;
		}
	}

	//IEnumerator xOffsetLerp(float startValue, float endValue, float lerpDuration)
	//{
	//	float timeElapsed = 0;

	//	while (timeElapsed < lerpDuration)
	//	{
	//		cam.FollowOffset.x = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
	//		timeElapsed += Time.deltaTime;

	//		yield return null;
	//	}

	//	cam.FollowOffset.x = endValue;
	//}

	IEnumerator yOffsetLerp(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			cam.FollowOffset.y = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		cam.FollowOffset.y = endValue;
	}
}
