using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class EndAnim : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D PlayerBody;

	[SerializeField]
	private PlayerInput PlayerInputs;

	[SerializeField]
	private bool PlayerCanMove;

	private PlayableDirector anim;

	public bool started = false;

	private float timer;
	private bool startTimer = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		anim = GetComponent<PlayableDirector>();
	}

	// Update is called once per frame
	void Update()
	{
		if (anim.time > 0 && !startTimer)
		{
			startTimer = true;
		}

		if (startTimer)
		{
			timer += Time.deltaTime;
		}

		if (!started && (timer >= anim.duration))
		{
			started = true;
			PlayerBody.linearVelocity = Vector2.zero;
			PlayerBody.bodyType = RigidbodyType2D.Dynamic;
			PlayerInputs.enabled = PlayerCanMove;
			anim.Stop();
			Destroy(gameObject);
		}
	}
}
