using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LustBossControllerPhaseThree : MonoBehaviour
{
	[SerializeField]
	Transform Target;

	[SerializeField]
	Transform BulletHellPlace;

	NavMeshAgent agent;
	Rigidbody2D rb;

	public float sight = 10;
	public LayerMask player;

	[SerializeField]
	AnimationClip DashChargeClip;
	private Animator animator;

	#region Bullet Variables

	public GameObject Bullet;
	public float BulletSpeed;
	public float BulletCooldown;
	private float BulletTimer;

	public int damage = 5;

	#endregion

	#region Dash Variables

	public float DashPower;
	public float DashCooldown = 5f;
	public float DashChargeTime = .25f;
	private float DashTimer;
	public bool Dashing = false;

	#endregion

	#region Bullet Hell Variables

	public float BulletHellSpeed;
	public float BulletHellCooldown;
	public float BulletHellTimer;
	public bool BulletHellActive;

	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!enabled) return;
		if (collision.tag.Equals("Player"))
		{
			if (collision.GetComponent<PlayerController>().Dashing) return;
			print("Phase Three boss hit");
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

		DashTimer = DashCooldown;
		BulletHellTimer = BulletHellCooldown;

		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		if (!BulletHellActive)
		{
			Sight();
		}

		BulletTimer -= Time.deltaTime;

		if (!Dashing && !BulletHellActive)
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

	private IEnumerator DashAttack()
	{
		animator.SetBool("Dash", true);

		agent.SetDestination(Target.position);
		agent.speed = DashPower;
		agent.acceleration = 10000;
		agent.angularSpeed = 0;
		agent.autoBraking = false;

		yield return new WaitForSeconds(1f);

		DashEnd();
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

	#region BulletHell

	private void BulletHellUpdate()
	{
		BulletHellTimer -= Time.deltaTime;
		if (BulletHellTimer <= 0 && !BulletHellActive)
		{
			StartCoroutine(BulletHellStart());
		}
	}

	public IEnumerator BulletHellStart(bool finalStand = false)
	{
		transform.DOLocalMove(BulletHellPlace.position, 2f);
		agent.enabled = false;
		BulletHellActive = true;

		yield return new WaitForSeconds(2f);

		if (finalStand)
		{
			StartCoroutine(FinalStand());
		}
		else
		{
			switch (Random.Range(1, 3))
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

		StartCoroutine(BulletHellEnd());
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

		StartCoroutine(BulletHellEnd());
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

		StartCoroutine(BulletHellEnd());
	}

	private IEnumerator BulletHellEnd()
	{
		yield return new WaitForSeconds(3f);
		BulletHellActive = false;
		agent.enabled = true;
		BulletHellTimer = BulletHellCooldown;
	}

	#region Final Stand
	private IEnumerator FinalStand()
	{
		StartCoroutine(FinalStandCircle());

		yield return FinalStandSpin();

		yield return FinalStandBackwardSpin();

		yield return FinalStandAreas();

		StartCoroutine(FinalstandCircleTired());
	}

	private IEnumerator FinalStandCircle()
	{
		for (int j = 0; j < 45; j++)
		{
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f);

			for (int i = 20; i < 360; i += 40)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f);
		}
	}

	private IEnumerator FinalStandSpin()
	{
		for (int j = 0; j < 360; j += 3)
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
	}

	private IEnumerator FinalStandBackwardSpin()
	{
		for (int j = 360; j >= 0; j -= 3)
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
	}

	private IEnumerator FinalStandAreas()
	{
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
			yield return new WaitForSeconds(1.5f);
		}

		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < 90; i += 3)
			{
				Vector2 ray = Quaternion.AngleAxis(i + (90 * j), Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}
			yield return new WaitForSeconds(1.5f);
		}
	}

	private IEnumerator FinalstandCircleTired()
	{
		for (int j = 0; j < 10; j++)
		{
			for (int i = 0; i < 360; i += 40)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f + (.1f * j));

			for (int i = 20; i < 360; i += 40)
			{
				Vector2 ray = Quaternion.AngleAxis(i, Vector3.forward) * Vector2.right;
				BlowBulletHell(ray);
			}

			yield return new WaitForSeconds(.5f + (.1f * j));
		}

		GetComponent<LustBossVulnerablePhaseTwo>().enabled = true;
		GetComponent<LustBossVulnerablePhaseTwo>().StartVulnerablePhase();
		GetComponent<PhaseManager>().Phase = 4;
	}

	#endregion

	private void BlowBulletHell(Vector2 force)
	{
		GameObject TheBullet = Instantiate(Bullet);
		TheBullet.GetComponent<Bullet>().CanDamageIfPlayerIsDashing = true;
		TheBullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		TheBullet.GetComponent<Rigidbody2D>().linearVelocity = force * BulletHellSpeed;
	}

	#endregion
}
