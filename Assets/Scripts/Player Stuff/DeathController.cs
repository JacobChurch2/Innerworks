using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class DeathController : MonoBehaviour
{
	Animator animator;
	Rigidbody2D rb;
	PlayerInput playerInput;

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
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Death"))
		{
			animator.SetTrigger(AnimationStrings.IsDead);
			rb.gravityScale = 0;
			rb.linearVelocity = Vector2.zero;
			if (playerInput)
			{
				playerInput.enabled = false;
			}
		}
	}

	private void DeathSequence()
	{
		transform.position = ResapwnPoint.position;
		rb.gravityScale = 5;
		if (playerInput)
		{
			playerInput.enabled = true;
		}
	}
}
