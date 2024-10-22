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
	private Collider2D newCamCollider;
	[SerializeField]
	private CinemachineConfiner2D confiner;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			Door.SetActive(true);
			Boss.SetActive(true);
			BossUI.SetActive(true);

			confiner.BoundingShape2D = newCamCollider;
		}
	}
}
