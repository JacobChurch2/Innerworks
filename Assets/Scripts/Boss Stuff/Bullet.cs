using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag.Equals("Player"))
        {
            //TODO: damage player
            print("kiss hit");

            Destroy(gameObject);
        }
	}
}
