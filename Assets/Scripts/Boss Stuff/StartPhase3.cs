using Unity.Cinemachine;
using UnityEngine;

public class StartPhase3 : MonoBehaviour
{
	[SerializeField]
	private GameObject ChocolateDrops;

	[SerializeField]
	private Collider2D newCamCollider;
	[SerializeField]
	private CinemachineConfiner2D confiner;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			ChocolateDrops.SetActive(true);

			confiner.BoundingShape2D = newCamCollider;
		}
	}
}
