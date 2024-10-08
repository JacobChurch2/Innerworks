using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NearObject : MonoBehaviour
{
    public bool InRange = false;

    [SerializeField]
    public string OtherTag;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == OtherTag)
        {
            InRange = true;
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == OtherTag)
		{
			InRange = false;
		}
	}
}
