using UnityEngine;
using UnityEngine.UIElements;

public class Spring : MonoBehaviour
{
	public float power = 10000;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			Vector3 rotation = transform.rotation.eulerAngles;
			Vector2 rotatedVector = (Quaternion.AngleAxis(rotation.z, Vector3.forward) * Vector2.up).normalized;
			collision.GetComponent<Rigidbody2D>().linearVelocity = (rotatedVector * power);
			collision.GetComponent<PlayerController>().isAffectedBySpring = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			Vector3 rotation = transform.rotation.eulerAngles;
			Vector2 rotatedVector = (Quaternion.AngleAxis(rotation.z, Vector3.forward) * Vector2.up).normalized;
			collision.rigidbody.linearVelocity = (rotatedVector * power);
			collision.gameObject.GetComponent<PlayerController>().isAffectedBySpring = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			Vector3 rotation = transform.rotation.eulerAngles;
			Vector2 rotatedVector = (Quaternion.AngleAxis(rotation.z, Vector3.forward) * Vector2.up).normalized;
			collision.rigidbody.linearVelocity = (rotatedVector * power);
			collision.gameObject.GetComponent<PlayerController>().isAffectedBySpring = true;
		}
	}
}