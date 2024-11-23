using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class StartPhase3 : MonoBehaviour
{
	[SerializeField]
	ControlPlayerInput playerInput;

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
			SetUp();
		}
	}

	public void SetUp(float time = 5)
	{
		playerInput.ChangePlayerInput(false);
		Boss.GetComponent<LustBossControllerPhaseOne>().enabled = false;
		Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;

		foreach (ParallaxEffect parallax in Resources.FindObjectsOfTypeAll<ParallaxEffect>())
		{
			parallax.enabled = false;
		}

		StartCoroutine(MoveBossToPoint(time));
	}

	private IEnumerator MoveBossToPoint(float time = 5)
	{
		Boss.transform.DOMove(AnimLocal.position, time);
		yield return new WaitForSeconds(time);
		StartTalking();
	}

	private void StartTalking()
	{
		talk.gameObject.SetActive(true);
		talk.StartText();
	}

	private void Update()
	{
		if (endAnim.started)
		{
			endAnim.started = false;
			ChocolateDrops.SetActive(true);
			Boss.GetComponent<LustBossControllerPhaseThree>().enabled = true;
			Boss.GetComponent<PhaseManager>().UpdatePhase(50f, 100f);
			confiner.BoundingShape2D = newCamCollider;
			Boss.GetComponent<NavMeshAgent>().enabled = true;

			gameObject.SetActive(false);
		}
	}
}
