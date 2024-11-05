using Unity.Cinemachine;
using UnityEngine;

public class StartPhase3 : MonoBehaviour
{
	[SerializeField]
	private GameObject Boss;

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
			Boss.GetComponent<LustBossControllerPhaseOne>().enabled = false;
			Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;
			Boss.GetComponent<LustBossControllerPhaseThree>().enabled = true;

			confiner.BoundingShape2D = newCamCollider;

			foreach (ParallaxEffect parallax in Resources.FindObjectsOfTypeAll<ParallaxEffect>())
			{
				parallax.enabled = false;
			}
		}
	}
}
