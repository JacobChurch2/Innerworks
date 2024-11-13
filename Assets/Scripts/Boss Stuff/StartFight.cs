using Unity.Cinemachine;
using UnityEngine;

public class StartFight : MonoBehaviour
{
	[SerializeField]
	private GameObject Door;
	[SerializeField]
	private GameObject Boss;
	[SerializeField]
	private GameObject BossUI;
	[SerializeField]
	private GameObject ChocolateDrops;
	[SerializeField]
	private Collider2D newCamCollider;
	[SerializeField]
	private CinemachineConfiner2D confiner;
	[SerializeField]
	private BossMusic music;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			Door.SetActive(true);

			ChocolateDrops.SetActive(true);
			if (!Boss.activeSelf)
			{
				Boss.SetActive(true);
			}
			Boss.GetComponent<LustBossControllerPhaseOne>().enabled = true;
			Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;
			Boss.GetComponent<LustBossControllerPhaseThree>().enabled = false;

			BossUI.SetActive(true);

			foreach (ParallaxEffect parallax in Resources.FindObjectsOfTypeAll<ParallaxEffect>())
			{
				parallax.enabled = false;
			}

			confiner.BoundingShape2D = newCamCollider;

			music.StartAudio();

			Destroy(gameObject, 0.1f);
		}
	}
}
