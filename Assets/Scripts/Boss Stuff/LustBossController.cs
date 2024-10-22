using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LustBossController : MonoBehaviour
{
    private int phase = 1;

    [SerializeField]
    Transform NavTarget;

    NavMeshAgent agent;

    public float sight = 100;
    public LayerMask player;

	#region bullet variables
	public GameObject Bullet;
    public float BulletSpeed;
    public float BulletCooldown;
    private float BulletTimer;
	#endregion 

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
	}

	void FixedUpdate()
    {
        Sight();
        BulletTimer -= Time.deltaTime;

        agent.SetDestination(NavTarget.position);
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
}
