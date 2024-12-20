using UnityEngine;

public class Drops : MonoBehaviour
{
	public int damage = 2;

	private void Start()
	{
		if (GetComponent<AudioSource>())
		{
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			print("drop hit");
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player)
			{
				player.TakeDamage(damage);
			}

			Destroy(gameObject);
		}
	}
}
