using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public int damage = 1;

	public bool CanDamageIfPlayerIsDashing;

	private void Start()
	{
		if (GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().volume = .2f;
			GetComponent<AudioSource>().priority = 255;
			GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			if (!CanDamageIfPlayerIsDashing && collision.GetComponent<PlayerController>().Dashing) return;

			print("kiss hit");
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player)
			{
				player.TakeDamage(damage);
			}

			Destroy(gameObject);
		}
	}

	Bullet(bool canDamageIfPlayerDashing = true)
	{
		CanDamageIfPlayerIsDashing = canDamageIfPlayerDashing;
	}
}
