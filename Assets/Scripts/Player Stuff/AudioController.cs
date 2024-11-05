using UnityEngine;

public class AudioController : MonoBehaviour
{
	[SerializeField]
	private AudioClip movement, jump, land, death, grapple, dash;

	[SerializeField]
	private AudioSource Source;

	private bool movementPlaying = false;

	public void PlayAudio(string name)
	{
		Source.loop = false;
		switch (name)
		{
			case "jump":
				Source.priority = 1;
				Source.PlayOneShot(jump);
				break;
			case "land":
				Source.PlayOneShot(land);
				break;
			case "death":
				Source.PlayOneShot(death);
				break;
			case "grapple":
				Source.PlayOneShot(grapple);
				break;
			case "dash":
				Source.PlayOneShot(dash);
				break;
		}

		if (movementPlaying)
		{
			Source.loop = true;
		}
	}

	public void PlayMovement(bool Start = true)
	{
		if (!movementPlaying && Start)
		{
			Source.clip = movement;
			Source.loop = true;
			Source.Play();
			movementPlaying = true;
		}
		else if (movementPlaying && !Start)
		{
			Source.Stop();
			Source.loop = false;
			movementPlaying = false;
		}
	}
}
