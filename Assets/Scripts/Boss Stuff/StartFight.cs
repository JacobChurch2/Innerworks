using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class StartFight : MonoBehaviour
{
	[SerializeField]
	private Collider2D newCamCollider;
	[SerializeField]
	private CinemachineConfiner2D confiner;
	[SerializeField]
	private BossMusic music;

	[SerializeField]
	PlayerInput playerInputs;
	[SerializeField]
	Rigidbody2D PlayerRB;

	[SerializeField]
	TalkingController Talking;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			foreach (ParallaxEffect parallax in Resources.FindObjectsOfTypeAll<ParallaxEffect>())
			{
				parallax.enabled = false;
			}

			confiner.BoundingShape2D = newCamCollider;

			playerInputs.enabled = false;

			PlayerRB.linearVelocity = Vector2.zero;

			music.StartAudio();

			StartCoroutine(AnimThings());

			Destroy(GetComponent<Collider2D>());
		}
	}

	private IEnumerator AnimThings()
	{
		PlayableDirector AnimStart = GetComponent<PlayableDirector>();

		AnimStart.Play();

		yield return new WaitForSeconds((float) AnimStart.duration);

		Talking.StartText();
	}
}
