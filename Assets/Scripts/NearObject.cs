using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NearObject : MonoBehaviour
{
    public bool InRange = false;

    void OnTriggerEnter2D()
    {
        InRange = true;
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
        InRange = false;
	}
}
