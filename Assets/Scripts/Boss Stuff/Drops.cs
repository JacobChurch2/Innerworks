using UnityEngine;

public class Drops : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			//TODO::DamagePlayer
			print("drop hit");
			Destroy(gameObject);
		}
	}
}
