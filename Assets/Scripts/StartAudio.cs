using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartAudio : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			GetComponent<AudioSource>().Play();
		}
	}
}
