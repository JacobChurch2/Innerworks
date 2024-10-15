using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField]
    DeathController playerDeath;
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			playerDeath.ResapwnPoint = transform;
		}
	}
}
