using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class LustBossController : MonoBehaviour
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
    public float DashCooldown;
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

        rb = GetComponent<Rigidbody2D>();

        DashTimer = DashCooldown;
	}

	void FixedUpdate()
    {
        Sight();
        BulletTimer -= Time.deltaTime;

        DashAttack();

        if (!Dashing)
        {
            agent.SetDestination(Target.position);
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
            GameObject TheBullet = Instantiate(Bullet);
            TheBullet.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            TheBullet.GetComponent<Rigidbody2D>().linearVelocity = force * BulletSpeed;
            BulletTimer = BulletCooldown;
        }
    }

    private void DashAttack()
    {
        if (DashTimer <= 0)
        {
            DashTimer = DashCooldown;
            Dashing = true;
			agent.SetDestination(Target.position);
			agent.speed = DashPower;
			agent.acceleration = 10000;
            agent.angularSpeed = 0;
            agent.autoBraking = false;
			StartCoroutine(DashEnd());
        } 
        else
        {
            DashTimer -= Time.deltaTime;
        }
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
}
