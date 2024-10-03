using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.Interpolators;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
	#region animation variables
	//animaiton variables
	private bool _isMoving = false;
	public bool _isFacingRight = true;

    private float IdleTimer = 0;
	private AnimatorClipInfo[] animatorinfo;
	private string current_animation;

	public bool IsFacingRight { 
		get { return _isFacingRight; } 
		private set
		{
			if(_isFacingRight != value)
			{
				//flip the local scale to make the player face the opposite direction
				transform.localScale *= new Vector2(-1, 1);
			}
			_isFacingRight = value;
		}
	}
	#endregion

	#region movement variables
	//movement variables
	public float walkSpeed = 5f;
    Vector2 moveInput;
	TouchingDirections touchingDirections;
	public float jumpImpulse = 10f;
	private bool dashAvalible = false;
	private bool dashing = false;

	public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }

	public float CurrentMoveSpeed { get
		{
			if (IsMoving && !touchingDirections.IsOnWall)
			{
				return walkSpeed;
			} else
			{
				// Idle
				return 0;
			}

		}
	}
	#endregion

	#region Component variables
	//Components
	Rigidbody2D rb;
    Animator animator;
	#endregion

	#region basic methods
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		touchingDirections = GetComponent<TouchingDirections>();
	}

	private void FixedUpdate()
	{
		if (!dashing)
		{
			rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

			animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);

			if (!IsMoving)
			{
				SetIdleAnimation();
			}
		}

		if (!dashAvalible && touchingDirections.IsGrounded && !dashing)
		{
			dashAvalible = true;
		}
	}
	#endregion

	#region movement things
	public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = (moveInput != Vector2.zero);

        SetFacingDirection(moveInput);
    }

	public void OnJump(InputAction.CallbackContext context)
	{
		//TODO check if alive as well
		if (context.started && touchingDirections.IsGrounded)
		{
			animator.SetTrigger(AnimationStrings.Jump);
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		//TODO: Screen shake
		//TODO: Aninimation
		if (dashAvalible)
		{
			float x = moveInput.x;
			float y = moveInput.y;
			rb.linearVelocity = Vector2.zero;
			rb.linearVelocity += new Vector2(x, y).normalized * 30;
			dashAvalible = false;
			StartCoroutine(DashBeforeAndAfter());
		}
	}

	private IEnumerator DashBeforeAndAfter()
	{
		dashing = true;
		rb.gravityScale = 0;
		rb.linearDamping = 4;
		yield return new WaitForSeconds(0.15f);
		StartCoroutine(DashDamping(0, 0.2f));
		rb.linearDamping = 0;
		dashing = false;
		rb.gravityScale = 5;
	}

	IEnumerator DashDamping(float endValue, float duration)
	{
		float time = 0;
		float startValue = rb.linearDamping;

		while (time < duration)
		{
			rb.linearDamping = Mathf.Lerp(startValue, endValue, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		rb.linearDamping = endValue;
	}

	#endregion

	#region Animation stuff
	private void SetFacingDirection(Vector2 moveInput)
	{
		if(moveInput.x > 0 && !IsFacingRight)
		{
			//face right
			IsFacingRight = true;
		}
		else if(moveInput.x < 0 && IsFacingRight)
		{
			//face left
			IsFacingRight = false;
		}
	}

	private void SetIdleAnimation()
    {
		animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);
		current_animation = animatorinfo[0].clip.name;
		if (current_animation.Equals("Player_Idle") || current_animation.Equals("Player_Sleepy"))
		{
			IdleTimer += Time.deltaTime;
			animator.SetFloat(AnimationStrings.IdleTimer, IdleTimer);
		}
		else
		{
			IdleTimer = 0;
			animator.SetFloat(AnimationStrings.IdleTimer, IdleTimer);
		}
	}
	#endregion
}
