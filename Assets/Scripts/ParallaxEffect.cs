using UnityEngine;

[ExecuteInEditMode]
public class ParallaxEffect : MonoBehaviour
{
	public float parallaxFactor;

	public void Move(float delta)
	{
		Vector3 newPos = transform.localPosition;
		newPos.x -= delta * parallaxFactor;

		transform.localPosition = newPos;
	}

}

