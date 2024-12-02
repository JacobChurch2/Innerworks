using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamYDamping : MonoBehaviour
{
	[SerializeField]
	public CinemachineFollow cam;

	public float yDamping;
	//public float xOffset;

	public float TransitionTime;

	private float yDampingOriginal;
	//private float xOffsetOriginal;

	private static int Boxes;

	private void Start()
	{
		//xOffsetOriginal = cam.FollowOffset.x;
		yDampingOriginal = cam.TrackerSettings.PositionDamping.y;
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
			StartCoroutine(yDampingLerp(cam.TrackerSettings.PositionDamping.y, yDamping, TransitionTime));
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
				//StartCoroutine(xOffsetLerp(cam.FollowOffset.x, xOffsetOriginal, TransitionTime));
				StartCoroutine(yDampingLerp(cam.TrackerSettings.PositionDamping.y, yDampingOriginal, TransitionTime));
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

	IEnumerator yDampingLerp(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			cam.TrackerSettings.PositionDamping.y = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		cam.TrackerSettings.PositionDamping.y = endValue;
	}
}
