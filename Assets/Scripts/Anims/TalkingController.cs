using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(PlayerInput))]
public class TalkingController : MonoBehaviour
{
	[SerializeField]
	PlayerInput playerInputs;

	[SerializeField]
	Rigidbody2D PlayerRB;

	[SerializeField]
	CanvasGroup group;

	[SerializeField]
	TextMeshProUGUI TalkingText;

	[SerializeField]
	string[] messages;
	[SerializeField]
	TMP_FontAsset[] fonts;
	[SerializeField]
	int[] FontIndexForMessages;

	public int DefultFontIndex = 0;

	public float letterDelay = 0.05f;

	private Dictionary<string, TMP_FontAsset> MessagesAndFonts = new Dictionary<string, TMP_FontAsset>();

	private int MessageIndex = 0;

	private string currentFullMessage;
	private string currentMessage;

	public bool started = false;
	public bool done = false;

	private PlayerInput UIInput;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		group.alpha = 0;
		UIInput = gameObject.GetComponent<PlayerInput>();
		UIInput.enabled = false;
		for (int i = 0; i < messages.Length; i++)
		{
			if (i >= FontIndexForMessages.Length)
			{
				MessagesAndFonts.Add(messages[i], fonts[FontIndexForMessages[DefultFontIndex]]);
			} else
			{
				MessagesAndFonts.Add(messages[i], fonts[FontIndexForMessages[i]]);
			}
		}
	}

	public void StartText()
	{
		//TODO: add an intro animation
  		started = true;
		UIInput.enabled = true;
		group.alpha = 1f;
		playerInputs.enabled = false;
		PlayerRB.linearVelocity = Vector2.zero;
		done = false;
		StartCoroutine(UpdateMessage(0));
	}

	private void EndText()
	{
		//TODO: add an ending animation
		group.alpha = 0f;
		playerInputs.enabled = true;
		done = true;
		MessageIndex = 0;
	}

	private IEnumerator UpdateMessage(int index)
	{
		if (index >= messages.Length)
		{
			EndText();
			yield break;
		}

		currentFullMessage = messages[index];
		TalkingText.font = MessagesAndFonts[currentFullMessage];
		for (int i = 1; i <= currentFullMessage.Length; i++)
		{
			currentMessage = currentFullMessage.Substring(0, i);
			TalkingText.text = currentMessage;
			yield return new WaitForSeconds(letterDelay);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && !started)
		{
			StartText();
		}
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (!currentFullMessage.Equals(currentMessage))
			{
				StopAllCoroutines();
				currentMessage = currentFullMessage;
				TalkingText.text = currentMessage;
			}
			else
			{
				StartCoroutine(UpdateMessage(++MessageIndex));
			}
		}
	}
}
