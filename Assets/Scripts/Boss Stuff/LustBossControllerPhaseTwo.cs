using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

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
		transform.position = new Vector3 (Math.Clamp(transform.position.x, minX, maxX), Math.Clamp(transform.position.y, minY, maxY), transform.position.z);
	}
}
