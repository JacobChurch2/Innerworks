using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			collision.transform.SetParent(transform);
			collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			collision.transform.SetParent(null);
			collision.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
		}
	}
}
