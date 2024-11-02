using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public int damage = 1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			print("kiss hit");
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player)
			{
				player.TakeDamage(damage);
			}

			Destroy(gameObject);
		}
	}
}
