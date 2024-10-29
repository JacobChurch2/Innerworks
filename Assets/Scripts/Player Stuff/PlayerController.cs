using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TouchingDirections))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
	#region Variables
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
	[Header("Movement")]
	public float walkSpeed = 5f;
	public float Acceleration = 2.5f;
	public float Decelaration = 5f;

	Vector2 moveInput;
	TouchingDirections touchingDirections;

	[Header("Jump")]
	public float jumpImpulse = 10f;
	private bool Jumping = false;
	private int jumpCount = 1;
	public int jumpsAvliable = 1;

	public float jumpCutMultiplier = 0.5f;

	public float jumpBufferTime = .1f;
	public float jumpCoyoteTime = .5f;
	private float lastGroundedTime = 0;
	private float lastJumpTime = 0;
	public float fallGravityMultiplier = 1.5f;

	[Header("Springs")]
	public bool isAffectedBySpring = false;
	public float SpringAffectTime = 1;
	private float SpringTime;
	public float SpringDamping = 30;

	private float gravityScale = 5;

	[Header("Dash")]
	public bool DashUnlocked = false;
	private bool DashAvalible = true;
	public bool Dashing = false;

	[Header("Grapple")]
	public bool GrappleUnlocked = false;
	public float GrappleDistance = 10f;
	public float GrappleStength = 10f;
	private bool GrappleAvalible = true;
	private bool Grappling = false;
	private RaycastHit2D GrappleHit;
	private Vector3 MousePos;
	private LineRenderer GrappleLine;
	private Vector2 GrappleMousePoint;
	private float ActualGrappleLength;
	public float GrappleCoolDown;
	private float GrappleTimer;
	private SpringJoint2D GrappleJoint;

	[Header("States")]
	public bool IsDead = false;
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
	[Header("Management")]
	private Vector2 colliderSize;
	private float slopeDownAngle;
	private Vector2 slopeNormalPerp;
	public bool isOnSlope;
	private float slopeSideAngle;

	[SerializeField]
	LayerMask Ground;

	[SerializeField]
	private float slopeCheckDistance;

	private Camera cam;
	#endregion

	#region component variables
	//Components
	Rigidbody2D rb;
	Animator animator;
	SpriteRenderer Renderer;
	BoxCollider2D cc;
	[SerializeField]
	CameraShake CamShake;
	[SerializeField]
	PhysicsMaterial2D NormalPhysics;
	[SerializeField]
	PhysicsMaterial2D SlopePhysics;
	#endregion
	#endregion

	#region basic methods
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		touchingDirections = GetComponent<TouchingDirections>();
		Renderer = GetComponent<SpriteRenderer>();
		cc = GetComponent<BoxCollider2D>();

		if (GetComponent<SpringJoint2D>())
		{
			GrappleJoint = GetComponent<SpringJoint2D>();
		}

		if (GetComponent<LineRenderer>())
		{
			GrappleLine = GetComponent<LineRenderer>();
		}

		gravityScale = rb.gravityScale;
	}

	private void Start()
	{
		//DashUnlocked = PlayerData.DashUnlocked;
		SpringTime = SpringAffectTime;
		colliderSize = cc.size;
		cam = Camera.main;
	}

	private void FixedUpdate()
	{
		WalkUpdate();

		GravityUpdate();

		CheckDashUpdate();

		JumpUpdate();

		SlopeUpdate();

		GrappleUpdate();

		AnimatorUpdate();
	}
	#endregion

	#region Movement things

	#region Walk
	public void OnWalk(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();

		IsMoving = (moveInput.x != 0);

		SetFacingDirection(moveInput);
	}

	private void WalkUpdate()
	{
		if (!Dashing)
		{
			Walk();

			if (!IsMoving)
			{
				SetIdleAnimation();
			}
		}
	}

	private void Walk()
	{
		int xMove = (moveInput.x != 0) ? (moveInput.x > 0 ? 1 : -1) : 0;
		float targetSpeed = xMove * walkSpeed;
		float speedDif = targetSpeed - rb.linearVelocityX;

		float accelRate = 0;

		if (isAffectedBySpring)
		{

			SpringTime -= Time.deltaTime;
			rb.linearDamping = SpringDamping;

			if (SpringTime <= 0)
			{
				isAffectedBySpring = false;
				SpringTime = SpringAffectTime;
				rb.linearDamping = 0;
			}
		}

		accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Acceleration : Decelaration;

		float movement = speedDif * accelRate;

		if (Grappling)
		{
			switch (xMove)
			{
				case 0:
					movement = 0;
					break;
				default:
					movement = speedDif * accelRate * .5f;
					break;
			}
		}

		if (isOnSlope)
		{
			rb.AddForce((slopeNormalPerp * -1) * movement, ForceMode2D.Force);
			Debug.DrawRay(transform.position, (slopeNormalPerp * -1) * movement, Color.blue);
		}
		else
		{
			rb.AddForce(movement * (Vector2.right), ForceMode2D.Force);
			Debug.DrawRay(transform.position, movement * (Vector2.right), Color.blue);
		}
	}
	#endregion

	#region Jump
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started && !isAffectedBySpring)
		{
			lastJumpTime = jumpBufferTime;
		}

		if (rb.linearVelocityY > 0 && Jumping)
		{
			rb.AddForce(Vector2.down * rb.linearVelocityY * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
		}
	}

	private void JumpAction()
	{
		animator.SetTrigger(AnimationStrings.Jump);
		rb.linearVelocityY = 0;
		rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
		Jumping = true;
	}

	private void JumpUpdate()
	{
		if (touchingDirections.IsGrounded)
		{
			lastGroundedTime = jumpCoyoteTime;
			jumpCount = jumpsAvliable;
		}
		else
		{
			lastGroundedTime -= Time.deltaTime;
		}

		if ((lastGroundedTime > 0 || jumpCount > 1) && lastJumpTime > 0 && !Jumping)
		{
			JumpAction();
			if (!(lastGroundedTime > 0))
			{
				jumpCount--;
			}
		}

		if (Jumping && rb.linearVelocityY < 0)
		{
			Jumping = false;
		}

		lastJumpTime -= Time.deltaTime;
	}
	#endregion

	#region Gravity
	private void GravityUpdate()
	{
		if (!Dashing && !Grappling)
		{
			if (rb.linearVelocityY < 0 && !IsDead)
			{
				rb.gravityScale = gravityScale * fallGravityMultiplier;
			}
			else if (!IsDead)
			{
				rb.gravityScale = gravityScale;
			}
		}
	}
	#endregion

	#region Slopes
	private void SlopeUpdate()
	{
		Vector2 checkPos = transform.position - new Vector3(0, colliderSize.y / 2);

		SlopeCheckVertical(checkPos);
		SlopeMaterialCheck();
	}

	private void SlopeMaterialCheck()
	{
		if (isOnSlope && moveInput.x == 0f)
		{
			rb.sharedMaterial = SlopePhysics;
		}
		else
		{
			rb.sharedMaterial = NormalPhysics;
		}
	}

	private void SlopeCheckVertical(Vector2 checkPos)
	{
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, Ground);

		if (hit)
		{
			slopeNormalPerp = Vector2.Perpendicular(hit.normal);

			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

			transform.eulerAngles = new Vector3(0f, 0f, -Vector2.SignedAngle(hit.normal, Vector2.up));

			if (slopeDownAngle < 50 && slopeDownAngle > 0)
			{
				isOnSlope = true;
			}
			else
			{
				isOnSlope = false;
			}

			Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
			Debug.DrawRay(hit.point, hit.normal, Color.green);
		}
		else
		{
			isOnSlope = false;
			transform.eulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	#endregion

	#region Dash
	public void OnDash(InputAction.CallbackContext context)
	{
		if (moveInput.x != 0 || (moveInput.y != 0))
		{
			if (DashAvalible && DashUnlocked && !IsDead)
			{
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
		DashAnimation(x, y);
		rb.linearVelocity = Vector2.zero;
		rb.linearVelocity += new Vector2(x, y).normalized * 40;
	}

	private void DashAnimation(float x, float y)
	{
		if (x != 0)
		{
			if (y != 0)
			{
				animator.SetBool(AnimationStrings.DiagonalDash, true);
				y = (float)Math.Round(y);
			}
			else
			{
				animator.SetBool(AnimationStrings.SideDash, true);
				y = 1;
			}
		}
		else
		{
			animator.SetBool(AnimationStrings.VerticalDash, true);
		}
		Renderer.flipY = (y == -1);
		CamShake.ShakeCamera(1.5f, .15f);
		if (FindFirstObjectByType<RippleEffect>())
		{
			FindFirstObjectByType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
		}
	}

	private IEnumerator DashAfter()
	{
		yield return new WaitForSeconds(0.15f);
		if (!IsDead)
		{
			rb.linearVelocityY = 0f;
		}
		else
		{
			Dashing = false;
			Renderer.flipY = false;
		}
		animator.SetBool(AnimationStrings.SideDash, false);
		animator.SetBool(AnimationStrings.VerticalDash, false);
		animator.SetBool(AnimationStrings.DiagonalDash, false);
		yield return new WaitForSeconds(0.1f);
		if (!IsDead)
		{
			rb.gravityScale = 5;
		}
		rb.linearDamping = 0;
		Renderer.flipY = false;
		Dashing = false;
	}

	private void endDashBecauseOfSpring()
	{
		animator.SetBool(AnimationStrings.SideDash, false);
		animator.SetBool(AnimationStrings.VerticalDash, false);
		animator.SetBool(AnimationStrings.DiagonalDash, false);
		if (!IsDead)
		{
			rb.gravityScale = 5;
		}
		Renderer.flipY = false;
		Dashing = false;
	}

	private void CheckDashUpdate()
	{
		if (!DashAvalible && touchingDirections.IsGrounded && !Dashing)
		{
			DashAvalible = true;
		}

		if (isAffectedBySpring)
		{
			DashAvalible = true;
			if (Dashing)
			{
				endDashBecauseOfSpring();
			}
		}
	}
	#endregion

	#region Grapple
	public void OnGrapple(InputAction.CallbackContext context)
	{
		if (context.started && GrappleAvalible && GrappleUnlocked && (GrappleTimer < 0))
		{
			GrappleStart();
		}

		if (context.canceled)
		{
			GrappleEnd();
		}
	}

	private void GrappleStart()
	{
		MousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

		if (Vector2.Distance(MousePos, transform.position) < GrappleDistance)
		{
			ActualGrappleLength = Vector2.Distance(MousePos, transform.position);

		}
		else
		{
			ActualGrappleLength = GrappleDistance;
		}

		Grappling = true;

		Debug.DrawLine(transform.position, MousePos, Color.magenta);

		GrappleMousePoint = MousePos - transform.position;

		GrappleHit = Physics2D.Raycast(transform.position, GrappleMousePoint, ActualGrappleLength, Ground);

		if (GrappleHit)
		{
			GrappleJoint.enabled = true;
			GrappleJoint.connectedAnchor = GrappleHit.point;
			GrappleJoint.distance = 0;

			Vector2 direction = ((Vector3)GrappleHit.point - transform.position);
			rb.AddForce(direction * GrappleStength, ForceMode2D.Impulse);
		}

		GrappleTimer = GrappleCoolDown;

		animator.SetBool(AnimationStrings.Grappling, true);
		rb.gravityScale = 0;
	}

	private void GrappleUpdate()
	{
		if (Grappling)
		{
			GrappleLine.enabled = true;
			if (GrappleHit)
			{
				Debug.DrawLine(transform.position, GrappleHit.point, Color.cyan);

				Vector3[] GrapplePoints = { transform.position, GrappleHit.point };
				GrappleLine.SetPositions(GrapplePoints);
			}
			else
			{
				StartCoroutine(FailedGrapple());
			}
		}
		GrappleTimer -= Time.deltaTime;
	}

	private IEnumerator FailedGrapple()
	{
		rb.linearVelocity = Vector3.zero;

		Vector3 EndPoint = transform.position + (Vector3)GrappleMousePoint.normalized * ActualGrappleLength;
		Vector3[] GrapplePoints = { transform.position, EndPoint };
		GrappleLine.SetPositions(GrapplePoints);

		yield return new WaitForSeconds(.1f);

		rb.gravityScale = gravityScale;
		GrappleEnd();
	}

	private void GrappleEnd()
	{
		Grappling = false;
		GrappleLine.enabled = false;
		rb.gravityScale = gravityScale;
		animator.SetBool(AnimationStrings.Grappling, false);
		GrappleJoint.enabled = false;
	}

	#endregion

	#endregion

	#region Animation stuff
	private void AnimatorUpdate()
	{
		animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);
		animator.SetBool(AnimationStrings.IsOnSlope, isOnSlope);
	}

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
