using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FreezeGame))]
public class DoubleJumpUnlock : MonoBehaviour
{
	FreezeGame Frozen;

	[SerializeField]
	PlayerController Player;
	[SerializeField]
	FadingPanel FallTextOne, FallTextTwo, FallTextThree, FallTextFour, FallTextFive;
	[SerializeField]
	PlayerInput playerInputs;

	bool SequenceStart = false;
	bool SequenceEnd = false;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Frozen = GetComponent<FreezeGame>();
	}

	private IEnumerator DoubleJumpSequence()
	{
		yield return new WaitForSecondsRealtime(2);
		StartCoroutine(FallTextOne.FadeInRealTime());
		yield return new WaitForSecondsRealtime(4);
		StartCoroutine(FallTextOne.FadeOutRealTime());

		StartCoroutine(FallTextTwo.FadeInRealTime());
		yield return new WaitForSecondsRealtime(4);
		StartCoroutine(FallTextTwo.FadeOutRealTime());

		StartCoroutine(FallTextThree.FadeInRealTime());
		yield return new WaitForSecondsRealtime(4);
		StartCoroutine(FallTextThree.FadeOutRealTime());

		StartCoroutine(FallTextFour.FadeInRealTime());

		yield return new WaitForSecondsRealtime(4);

		StartCoroutine(FallTextFive.FadeInRealTime());

		SequenceEnd = true;
		Player.jumpsAvliable = 2;
		Player.jumpCount++;
		//PlayerData.DashUnlocked = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && !SequenceStart)
		{
			SequenceStart = true;
			playerInputs.enabled = false;
			StartCoroutine(Frozen.SlowdownToAFreeze(0.75f));
			StartCoroutine(DoubleJumpSequence());
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started && SequenceEnd)
		{
			Time.timeScale = 1f;
			StartCoroutine(FallTextFour.FadeOutRealTime());
			StartCoroutine(FallTextFive.FadeOutRealTime());
			SequenceEnd = false;
			Player.OnJump(context);
			playerInputs.enabled = true;
		}
	}
}
