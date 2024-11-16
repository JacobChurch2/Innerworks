using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class StopCamX : MonoBehaviour
{
    [SerializeField]
    CamXMatchesPlayerX CamXMovement;

	[SerializeField]
	CinemachineFollow Cam;

	[SerializeField]
	float CamXOffset;

	[SerializeField]
	float Duration;

	[SerializeField]
	Rigidbody2D player;

	private float OriginalXOffset;
	private float OriginalXOffsetNegitive;

	private void Start()
	{
		OriginalXOffset = Cam.FollowOffset.x;
		OriginalXOffsetNegitive = CamXMovement.XLookBack;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			CamXMovement.StopAllCoroutines();
			CamXMovement.FollowingPlayer = false;

			StopAllCoroutines();
			StartCoroutine(xOffsetLerp(Cam.FollowOffset.x, CamXOffset, Duration));
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			CamXMovement.StopAllCoroutines();
			CamXMovement.FollowingPlayer = true;

			StopAllCoroutines();
			float NewX;
			if(player.linearVelocityX > 0)
			{
				NewX = OriginalXOffset;
				CamXMovement.Left = false;
			} else
			{
				NewX = OriginalXOffsetNegitive;
				CamXMovement.Left = true;
			}
			
			StartCoroutine(xOffsetLerp(Cam.FollowOffset.x, NewX, Duration));
		}
	}

	private IEnumerator xOffsetLerp(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			Cam.FollowOffset.x = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		Cam.FollowOffset.x = endValue;
	}
}
