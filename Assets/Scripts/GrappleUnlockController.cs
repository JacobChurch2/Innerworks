using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[RequireComponent(typeof(FreezeGame))]
[RequireComponent(typeof(Collider2D))]
public class GrappleUnlockController : MonoBehaviour
{
	[SerializeField]
	PlayerController Player;
	[SerializeField]
	PlayerInput playerInputs;
	[SerializeField]
	PlayableDirector endingAnim;
	[SerializeField]
    FadingPanel GrappleTextOne, GrappleTextTwo, GrappleTextThree;

	private FreezeGame freeze;
	private bool SequenceStart;
	private bool SequenceEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freeze = GetComponent<FreezeGame>();
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && !SequenceStart)
		{
			SequenceStart = true;
			playerInputs.enabled = false;
			StartCoroutine(freeze.SlowdownToAFreeze(0.75f));
			StartCoroutine(GrappleSequence());
		}
	}

	private IEnumerator GrappleSequence()
	{
		yield return new WaitForSecondsRealtime(2);
		StartCoroutine(GrappleTextOne.FadeInRealTime());
		yield return new WaitForSecondsRealtime(4);
		StartCoroutine(GrappleTextOne.FadeOutRealTime());

		StartCoroutine(GrappleTextTwo.FadeInRealTime());
		yield return new WaitForSecondsRealtime(4);
		StartCoroutine(GrappleTextTwo.FadeOutRealTime());

		StartCoroutine(GrappleTextThree.FadeInRealTime());

		SequenceEnd = true;
		Player.GrappleUnlocked = true;
		//PlayerData.GrappleUnlocked = true;
	}

	public void OnGrapple(InputAction.CallbackContext context)
	{
		if (context.started && SequenceEnd)
		{
			Time.timeScale = 1f;
			StartCoroutine(GrappleTextThree.FadeOutRealTime());
			endingAnim.Play();
			SequenceEnd = false;
		}
	}
}
