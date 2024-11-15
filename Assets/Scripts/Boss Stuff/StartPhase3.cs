using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

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

	[SerializeField]
	private Transform AnimLocal;

	[SerializeField]
	private TalkingController talk;

	[SerializeField]
	private EndAnim endAnim;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			Boss.GetComponent<LustBossControllerPhaseOne>().enabled = false;
			Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;

			foreach (ParallaxEffect parallax in Resources.FindObjectsOfTypeAll<ParallaxEffect>())
			{
				parallax.enabled = false;
			}

			StartCoroutine(MoveBossToPoint());
		}
	}

	private IEnumerator MoveBossToPoint()
	{
		Boss.transform.DOMove(AnimLocal.position, 5);
		yield return new WaitForSeconds(5);
		StartTalking();
	}

	private void StartTalking()
	{
		talk.StartText();
	}

	private void Update()
	{
		if (endAnim.started)
		{
			ChocolateDrops.SetActive(true);
			Boss.GetComponent<LustBossControllerPhaseThree>().enabled = true;
			confiner.BoundingShape2D = newCamCollider;
			Boss.GetComponent<NavMeshAgent>().enabled = true;

			Destroy(gameObject);
		}
	}
}
