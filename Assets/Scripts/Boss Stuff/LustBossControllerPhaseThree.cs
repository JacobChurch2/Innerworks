using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class LustBossControllerPhaseThree : MonoBehaviour
{
	[SerializeField]
	Transform Target;

	NavMeshAgent agent;
	Rigidbody2D rb;

	public float sight = 10;
	public LayerMask player;

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

	#endregion

	#region Bullet Hell Variables

	public float BulletHellSpeed;
	public float BulletHellCooldown;
	private float BulletHellTimer;
	private bool BulletHellActive;
	private int BulletHellPhase = 1;

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
		agent.enabled = false;
		agent.updateRotation = false;
		agent.updateUpAxis = false;
		agent.speed = 30f;

		rb = GetComponent<Rigidbody2D>();

		DashTimer = DashCooldown;
		BulletHellTimer = BulletHellCooldown;
	}

	void FixedUpdate()
	{
		if (!BulletHellActive)
		{
			Sight();
		}

		BulletTimer -= Time.deltaTime;

		if (!Dashing)
		{
			DashUpdate();
			agent.SetDestination(Target.position);
		}

		BulletHellUpdate();

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
			GameObject TheBullet = Instantiate(Bullet);
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

	private void DashAttack()
	{
		agent.SetDestination(Target.position);
		agent.speed = DashPower;
		agent.acceleration = 10000;
		agent.angularSpeed = 0;
		agent.autoBraking = false;
		StartCoroutine(DashEnd());
	}

	private IEnumerator DashCharge()
	{
		agent.speed = 0;
		agent.acceleration = 1000;
		//rb.AddForce(-(Target.position - transform.position) * DashChargePower);
		DashTimer = DashCooldown;
		Dashing = true;
		yield return new WaitForSeconds(DashChargeTime);
		DashAttack();
	}

	private IEnumerator DashEnd()
	{
		yield return new WaitForSeconds(1f);
		Dashing = false;
		agent.speed = 10;
		agent.acceleration = 8;
		agent.autoBraking = true;
		agent.angularSpeed = 120;
	}

	#endregion

	#region BulletHell

	private void BulletHellUpdate()
	{
		BulletHellTimer -= Time.deltaTime;
		if (BulletHellTimer <= 0 && !BulletHellActive)
		{
			switch (BulletHellPhase)
			{
				case 1:
					StartCoroutine(BulletVersionOne());
					break;
				case 2:
					StartCoroutine(BulletVersionTwo());
					break;
				case 3:
					StartCoroutine(BulletVersionThree());
					break;
			}
			BulletHellTimer = BulletHellCooldown;
			BulletHellActive = true;
			BulletHellPhase++;
		}
	}

	private IEnumerator BulletVersionOne()
	{
		for (int j = 0; j < 10; j++)
		{
			for (int i = 0; i < 360; i += 20)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f);

			for (int i = 10; i < 360; i += 20)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f);
		}
		yield return new WaitForSeconds(3f);
		BulletHellActive = false;
	}

	private IEnumerator BulletVersionTwo()
	{
		for (int j = 0; j < 720; j += 3)
		{

			Vector2 ray = Quaternion.AngleAxis(0 + j, Vector3.forward) * Vector2.right;
			BlowBulletHell(ray);

			ray = Quaternion.AngleAxis(90 + j, Vector3.forward) * Vector2.right;
			BlowBulletHell(ray);

			ray = Quaternion.AngleAxis(180 + j, Vector3.forward) * Vector2.right;
			BlowBulletHell(ray);

			ray = Quaternion.AngleAxis(270 + j, Vector3.forward) * Vector2.right;
			BlowBulletHell(ray);

			yield return new WaitForSeconds(.1f);
		}

		yield return new WaitForSeconds(3f);
		BulletHellActive = false;
	}

	private IEnumerator BulletVersionThree()
	{
		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 * j), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}
			yield return new WaitForSeconds(1.5f);
		}

		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 * j), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 + (90 * j)), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}
			yield return new WaitForSeconds(1.5f);
		}

		for (int j = 0; j < 2; j++)
		{
			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 * j), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (180 + (90 * j)), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}
			yield return new WaitForSeconds(1.5f);
		}

		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 * j), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 + (90 * j)), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (180 + (90 * j)), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}
			yield return new WaitForSeconds(1.5f);
		}

		for (int i = 0; i < 360; i += 20)
		{
			Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
			BlowBulletHell(ray);
		}

		yield return new WaitForSeconds(3f);
		BulletHellActive = false;
	}

	private void BlowBulletHell(Vector2 force)
	{
		GameObject TheBullet = Instantiate(Bullet);
		TheBullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		TheBullet.GetComponent<Rigidbody2D>().linearVelocity = force * BulletHellSpeed;
	}

	#endregion
}
