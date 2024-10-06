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

	public bool IsFacingRight
	{
		get { return _isFacingRight; }
		private set
		{
			if (_isFacingRight != value)
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
	public bool DashUnlocked = false;
	private bool DashAvalible = true;
	public bool Dashing = false;

	public bool IsMoving
	{
		get
		{
			return _isMoving;
		}
		private set
		{
			_isMoving = value;
			animator.SetBool(AnimationStrings.IsMoving, value);
		}
	}

	public float CurrentMoveSpeed
	{
		get
		{
			if (IsMoving && !touchingDirections.IsOnWall)
			{
				return walkSpeed;
			}
			else
			{
				// Idle
				return 0;
			}

		}
	}

	#endregion

	#region management variables
	private bool dead;
	public bool Dead { get => dead; set => dead = value; }
	#endregion

	#region component variables
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

	private void Start()
	{
		//DashUnlocked = PlayerData.DashUnlocked;
	}

	private void FixedUpdate()
	{
		if (!Dashing)
		{
			int xMove = (moveInput.x != 0) ? (moveInput.x > 0 ? 1 : -1) : 0;
			rb.linearVelocity = new Vector2(xMove * CurrentMoveSpeed, rb.linearVelocity.y);

			animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);

			if (!IsMoving)
			{
				SetIdleAnimation();
			}
		}

		if (!DashAvalible && touchingDirections.IsGrounded && !Dashing)
		{
			DashAvalible = true;
		}
	}
	#endregion

	#region movement things
	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();

		IsMoving = (moveInput.x != 0);

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

	#region Dash
	public void OnDash(InputAction.CallbackContext context)
	{
		if (moveInput.x != 0 || (moveInput.y != 0))
		{
			if (DashAvalible && DashUnlocked)
			{
				//TODO: Screen shake
				//TODO: Aninimation
				animator.SetBool(AnimationStrings.SideDash, true);
				DashSetUp();
				Dash();
				StartCoroutine(DashAfter());
			}
		}
	}

	private void DashSetUp()
	{
		Dashing = true;
		rb.gravityScale = 0;
		rb.linearDamping = 4;
		DashAvalible = false;
	}

	private void Dash()
	{
		float x = moveInput.x;
		float y = moveInput.y;
		rb.linearVelocity = Vector2.zero;
		rb.linearVelocity += new Vector2(x, y).normalized * 40;
	}

	private IEnumerator DashAfter()
	{
		yield return new WaitForSeconds(0.15f);
		animator.SetBool(AnimationStrings.SideDash, false);
		Dashing = false;
		rb.linearVelocity = Vector2.zero;
		yield return new WaitForSeconds(0.1f);
		rb.linearDamping = 0;
		rb.gravityScale = 5;
	}
	#endregion

	#endregion

	#region Animation stuff
	private void SetFacingDirection(Vector2 moveInput)
	{
		if (moveInput.x > 0 && !IsFacingRight)
		{
			//face right
			IsFacingRight = true;
		}
		else if (moveInput.x < 0 && IsFacingRight)
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
