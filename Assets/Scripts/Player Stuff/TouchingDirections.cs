using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class TouchingDirections : MonoBehaviour
{
	//Rigidbody2D rb;
	BoxCollider2D touchingCol;
	Animator animator;

	public ContactFilter2D castFilter;

	RaycastHit2D[] groundHits = new RaycastHit2D[5];
	public float groundDistance = 0.05f;
	RaycastHit2D[] wallHits = new RaycastHit2D[5];
	public float wallDistance = 0.02f;
	RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
	public float ceilingDistance = 0.05f;


	private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

	[SerializeField]
	private bool _isGrounded = true;


	public bool IsGrounded
	{
		get { return _isGrounded; }
		private set
		{
			_isGrounded = value;
			animator.SetBool(AnimationStrings.IsGrounded, value);
		}
	}

	[SerializeField]
	private bool _isOnWall = true;

	public bool IsOnWall
	{
		get { return _isOnWall; }
		private set
		{
			_isOnWall = value;
			animator.SetBool(AnimationStrings.IsOnWall, value);
		}
	}

	[SerializeField]
	private bool _isOnCeiling = true;

	public bool IsOnCeiling
	{
		get { return _isOnCeiling; }
		private set
		{
			_isOnCeiling = value;
			animator.SetBool(AnimationStrings.IsOnCeiling, value);
		}
	}

	private void Awake()
	{
		//rb = GetComponent<Rigidbody2D>(); 
		touchingCol = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
		IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
		IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
	}
}
