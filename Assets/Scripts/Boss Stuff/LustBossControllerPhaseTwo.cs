using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LustBossControllerPhaseTwo : MonoBehaviour
{
	[SerializeField]
	Transform Target;

	[SerializeField]
	BoxCollider2D Bounds;

	[SerializeField]
	GridLayout GridLayout;

	[SerializeField]
	Tilemap Tilemap;

	[SerializeField]
	ParticleSystem ParticleSystem;

	NavMeshAgent agent;
	Rigidbody2D rb;

	[SerializeField]
	AnimationClip DashChargeClip;
	private Animator animator;


	#region Dash Variables

	public float DashPower;
	public float DashCooldown = 3f;
	public float DashTime = .25f;
	public float DashCharge = .1f;
	private float DashTimer;
	private bool Dashing = false;
	private bool ChargingDash = false;

	#endregion

	public int damage = 5;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!enabled) return;
		if (collision.tag.Equals("Player"))
		{
			print("boss hit");
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player)
			{
				player.TakeDamage(damage);
			}
		}
	}

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		agent.autoBraking = false;
		agent.enabled = false;

		rb = GetComponent<Rigidbody2D>();

		DashTimer = DashCooldown;

		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		DashUpdate();

		if (!Dashing && !ChargingDash)
		{
			//agent.SetDestination(Target.position);
		}
		else
		{
			DestoryTiles();
		}

		ClampPositionToScreen();

	}

	private void DashUpdate()
	{
		if (!Dashing && !ChargingDash)
		{
			DashTimer -= Time.deltaTime;
			if (DashTimer <= 0)
			{
				StartCoroutine(DashStart());
			}
		}
	}

	private IEnumerator DashStart()
	{
		DashTimer = DashCooldown;
		ChargingDash = true;
		agent.enabled = false;

		animator.SetTrigger("DashCharge");
		animator.speed = DashChargeClip.length / DashCharge;

		yield return new WaitForSeconds(DashCharge);
		StartCoroutine(Dash());
	}

	private IEnumerator Dash()
	{
		ChargingDash = false;
		Dashing = true;
		animator.speed = 1;
		animator.SetBool("Dash", true);

		rb.linearVelocity = (Target.position - transform.position).normalized * DashPower;

		yield return new WaitForSeconds(DashTime);
		EndDash();
	}

	private void EndDash()
	{
		StopAllCoroutines();
		animator.SetBool("Dash", false);

		rb.linearVelocity = Vector2.zero;
		//agent.enabled = true;
		Dashing = false;
	}

	private void ClampPositionToScreen()
	{
		float minX = Bounds.transform.position.x - Bounds.size.x * 0.5f;
		float minY = Bounds.transform.position.y - Bounds.size.y * 0.5f;
		float maxX = Bounds.transform.position.x + Bounds.size.x * 0.5f;
		float maxY = Bounds.transform.position.y + Bounds.size.y * 0.5f;

		transform.position = new Vector3(Math.Clamp(transform.position.x, minX, maxX), Math.Clamp(transform.position.y, minY, maxY), transform.position.z);
	}

	private void DestoryTiles()
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				Vector3Int position = GridLayout.WorldToCell(transform.position);
				TileBase tile = Tilemap.GetTile(new Vector3Int(position.x + i, position.y + j, position.z));
				if (tile)
				{
					Instantiate(ParticleSystem, transform.position, new Quaternion());
				}
				Tilemap.SetTile(GridLayout.WorldToCell(new Vector3(transform.position.x + i, transform.position.y + j, transform.position.z)), null);
			}
		}
	}

	public void EndPhase()
	{
		agent.enabled = false;
		GetComponent<LustBossVulnerablePhaseTwo>().StartVulnerablePhase();
	}
}
