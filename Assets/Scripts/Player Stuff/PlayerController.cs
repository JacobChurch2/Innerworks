using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
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


	//movement variables
	public float walkSpeed = 5f;
    Vector2 moveInput;
	TouchingDirections touchingDirections;
	public float jumpImpulse = 10f;

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

	//Components
	Rigidbody2D rb;
    Animator animator;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		touchingDirections = GetComponent<TouchingDirections>();
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

		animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);

        if (!IsMoving)
        {
			SetIdleAnimation();
        }
	}

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
}
