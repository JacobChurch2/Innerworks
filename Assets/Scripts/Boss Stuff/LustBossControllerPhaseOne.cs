using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LustBossControllerPhaseOne : MonoBehaviour
{
	[SerializeField]
	Transform Target;

	[SerializeField]
	Transform RestLocation;

	NavMeshAgent agent;
	Rigidbody2D rb;

	public float sight = 10;
	public LayerMask player;

	[SerializeField]
	AnimationClip DashChargeClip;
	private Animator animator;

	public float PhaseTime;
	public float PhaseTimer;

	#region Bullet Variables

	public GameObject Bullet;
	public float BulletSpeed;
	public float BulletCooldown;
	private float BulletTimer;

	#endregion

	#region Dash Variables

	public float DashPower;
	public float DashCooldown = 5f;
	public float DashChargeTime = .25f;
	private float DashTimer;
	private bool Dashing = false;

	public int damage = 5;

	private AudioSource DashSound;

	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!enabled) return;
		if (collision.tag.Equals("Player"))
		{
			if (collision.GetComponent<PlayerController>().Dashing) return;
			print("Phase One boss hit");
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
		agent.speed = 30f;

		rb = GetComponent<Rigidbody2D>();

		if (GetComponent<AudioSource>())
		{
			DashSound = GetComponent<AudioSource>();
		}

		DashTimer = DashCooldown;

		animator = GetComponent<Animator>();

		PhaseTimer = PhaseTime;
	}

	void FixedUpdate()
	{
		Sight();
		BulletTimer -= Time.deltaTime;

		PhaseTimer -= Time.deltaTime;

		if (!Dashing)
		{
			DashUpdate();
			agent.SetDestination(Target.position);
		}

		if(PhaseTimer <= 0)
		{
			EndPhase();
		}
	}

	private void Sight()
	{
		RaycastHit2D hit;
		for (int i = 0; i < 360; i += 10)
		{
			Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
			hit = Physics2D.Raycast(transform.position, ray, sight, player);
			Debug.DrawRay(transform.position, ray, Color.yellow);
			if (hit)
			{
				Debug.DrawRay(transform.position, ray, Color.red);
				BlowBullet(ray);
				break;
			}
		}
	}

	private void BlowBullet(Vector2 force)
	{
		if (BulletTimer <= 0)
		{
			animator.SetTrigger("Kiss");
			GameObject TheBullet = Instantiate(Bullet);
			TheBullet.GetComponent<Bullet>().CanDamageIfPlayerIsDashing = false;
			TheBullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			TheBullet.GetComponent<Rigidbody2D>().linearVelocity = force * BulletSpeed;
			BulletTimer = BulletCooldown;
		}
	}


	#region Dash
	private void DashUpdate()
	{
		if (DashTimer <= 0)
		{
			StartCoroutine(DashCharge());
		}
		else
		{
			DashTimer -= Time.deltaTime;
		}
	}

	private IEnumerator DashCharge()
	{
		animator.SetTrigger("DashCharge");
		animator.speed = DashChargeClip.length / DashChargeTime;

		agent.speed = 0;
		agent.acceleration = 1000;
		//rb.AddForce(-(Target.position - transform.position) * DashChargePower);
		DashTimer = DashCooldown;
		Dashing = true;
		yield return new WaitForSeconds(DashChargeTime);
		animator.speed = 1;
		StartCoroutine(DashAttack());
	}

	private IEnumerator DashAttack()
	{
		animator.SetBool("Dash", true);
		
		DashSound.Play();
		agent.SetDestination(Target.position);
		agent.speed = DashPower;
		agent.acceleration = 10000;
		agent.angularSpeed = 0;
		agent.autoBraking = false;
		yield return new WaitForSeconds(1f);
		DashEnd();
	}


	public void DashEnd()
	{
		animator.SetBool("Dash", false);

		Dashing = false;
		agent.speed = 10;
		agent.acceleration = 8;
		agent.autoBraking = true;
		agent.angularSpeed = 120;
	}
	#endregion

	private void EndPhase()
	{
		agent.enabled = false;
		GetComponent<LustBossVulnerablePhaseOne>().enabled = true;
		GetComponent<LustBossVulnerablePhaseOne>().StartVulnerablePhase();
		this.enabled = false;
	}

	public void StartPhase()
	{
		agent.enabled = true;
		PhaseTimer = PhaseTime;
	}
}
