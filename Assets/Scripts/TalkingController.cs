using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

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

	public float letterDelay = 0.05f;

    private Dictionary<string, TMP_FontAsset> MessagesAndFonts = new Dictionary<string, TMP_FontAsset>();

    private int MessageIndex = 0;

    private string currentFullMessage;
    private string currentMessage;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        group.alpha = 0;
        for (int i = 0; i < messages.Length; i++)
        {
            MessagesAndFonts.Add(messages[i], fonts[FontIndexForMessages[i]]);
        }
    }

    private void StartText()
    {
        //TODO: add an intro animation
        group.alpha = 1f;
        playerInputs.enabled = false;
        PlayerRB.linearVelocity = Vector2.zero;
        StartCoroutine(UpdateMessage(0));
    }

    private void EndText()
    {
        //TODO: add an ending animation
		group.alpha = 0f;
		playerInputs.enabled = true;
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
        for (int i = 0; i < currentFullMessage.Length; i++)
        {
            currentMessage = currentFullMessage.Substring(0, i);
            TalkingText.text = currentMessage;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
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
