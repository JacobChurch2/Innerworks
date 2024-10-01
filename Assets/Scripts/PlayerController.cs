using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
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

	public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("IsMoving", value);
        }
    }

	//Components
    Rigidbody2D rb;
    Animator animator;

    private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb.linearVelocity.y);

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
			animator.SetFloat("IdleTimer", IdleTimer);
		}
		else
		{
			IdleTimer = 0;
			animator.SetFloat("IdleTimer", IdleTimer);
		}
	}
}
