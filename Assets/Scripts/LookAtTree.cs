using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class LookAtTree : MonoBehaviour
{
	[SerializeField]
	public CinemachineFollow cam;

	private IEnumerator OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			for (float CamYOffset = 0f; CamYOffset <= 5; CamYOffset += 0.05f)
			{
				cam.FollowOffset.y = CamYOffset;
				yield return new WaitForSecondsRealtime(0.01f);
			}
			if (cam.FollowOffset.y != 5f)
			{
				cam.FollowOffset.y = 5f;
			}
		}
	}

	private IEnumerator OnTriggerExit2D(Collider2D collision)
	{
		for (float CamYOffset = 5f; CamYOffset >= 0; CamYOffset -= 0.05f)
		{
			cam.FollowOffset.y = CamYOffset;
			yield return new WaitForSecondsRealtime(0.01f);
		}
		if (cam.FollowOffset.y != 0f)
		{
			cam.FollowOffset.y = 0f;
		}
	}
}
