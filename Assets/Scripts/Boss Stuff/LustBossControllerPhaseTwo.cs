using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class LustBossControllerPhaseTwo : MonoBehaviour
{
	public enum State
	{
		PhaseOne,
		Tired,
		PhaseTwo,
		PhaseThree,
	}

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


	#region Dash Variables

	public float DashPower;
	public float DashCooldown = 3f;
	public float DashTime = .25f;
	public float DashCharge = .1f;
	private float DashTimer;
	private bool Dashing = false;

	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			//TODO::DamagePlayer
			print("boss hit");
		}
	}

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		agent.autoBraking = false;

		rb = GetComponent<Rigidbody2D>();

		DashTimer = DashCooldown;
	}

	void FixedUpdate()
	{
		if (!Dashing)
		{
			StartCoroutine(Dash());
			agent.SetDestination(Target.position);
		}
		else
		{
			DestoryTiles();
		}
		ClampPositionToScreen();

	}

	private IEnumerator Dash()
	{
		DashTimer -= Time.deltaTime;
		if (DashTimer <= 0)
		{
			DashTimer = DashCooldown;
			Dashing = true;
			agent.enabled = false;
			yield return new WaitForSeconds(DashCharge);
			rb.linearVelocity = (Target.position - transform.position).normalized * DashPower;
			yield return new WaitForSeconds(DashTime);
			rb.linearVelocity = Vector2.zero;
			agent.enabled = true;
			Dashing = false;
		}
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
				TileBase tile = Tilemap.GetTile(new Vector3Int (position.x + i, position.y + j, position.z));
				if (tile)
				{
					Instantiate(ParticleSystem, transform.position, new Quaternion());
				}
				Tilemap.SetTile(GridLayout.WorldToCell(new Vector3(transform.position.x + i, transform.position.y + j, transform.position.z)), null);
			}
		}
	}
}
