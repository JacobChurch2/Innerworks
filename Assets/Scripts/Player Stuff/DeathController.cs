using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class DeathController : MonoBehaviour
{
	Animator animator;
	Rigidbody2D rb;
	PlayerInput playerInput;
	PlayerController Player;

	[SerializeField]
	Transform ResapwnPoint;

	public void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		if (GetComponent<PlayerInput>() != null)
		{
			playerInput = GetComponent<PlayerInput>();
		}
		if (GetComponent<PlayerController>())
		{
			Player = GetComponent<PlayerController>();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Death"))
		{
			animator.SetBool(AnimationStrings.IsDead, true);
			rb.gravityScale = 0;
			rb.linearVelocity = Vector2.zero;
			if (playerInput)
			{
				playerInput.enabled = false;
			}
			if (Player)
			{
				Player.IsDead = true;
			}
			StartCoroutine(DeathSequence());
		}
	}

	private IEnumerator DeathSequence()
	{
		yield return new WaitForSeconds(0.83333f);
		transform.position = ResapwnPoint.position;
		rb.gravityScale = 5;
		if (playerInput)
		{
			playerInput.enabled = true;
		}
		if (Player)
		{
			Player.IsDead = false;
		}
		animator.SetBool(AnimationStrings.IsDead, false);
	}
}
