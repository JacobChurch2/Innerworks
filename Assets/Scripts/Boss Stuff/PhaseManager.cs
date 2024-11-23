using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
	[SerializeField]
	private LustBossVulnerablePhaseOne vulnerablePhaseOne;

	[SerializeField]
	private GameObject PhaseOneEndLocal;

	[SerializeField]
	BossMusic music;

	public int Phase = 1;

	private Animator animator;

	private bool FinalStandActivated = false;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void UpdatePhase(float health, float MaxHealth)
	{
		switch (Phase)
		{
			case 1:
				if (health <= MaxHealth / 2)
				{
					Phase = 2;
					StartCoroutine(StartPhaseTwo());
				}
				break;
			case 2:
				if (GetComponent<LustBossControllerPhaseThree>().enabled)
				{
					Phase = 3;
				}
				break;
			case 3:
				if (health <= 0 && !FinalStandActivated)
				{
					FinalStandActivated = true;
					music.finish = true;
					GetComponent<LustBossControllerPhaseThree>().StopAllCoroutines();
					StartCoroutine(GetComponent<LustBossControllerPhaseThree>().BulletHellStart(true));
				}
				break;
		}
	}

	private IEnumerator StartPhaseTwo()
	{
		animator.SetBool("Sad", true);
		transform.DOMove(PhaseOneEndLocal.transform.position, 2f);
		GetComponent<LustBossControllerPhaseOne>().enabled = false;
		yield return new WaitForSeconds(2);
		GetComponent<LustBossVulnerablePhaseOne>().enabled = true;
		GetComponent<LustBossVulnerablePhaseOne>().started = true;
		StartCoroutine(vulnerablePhaseOne.StartAnimation());
	}
}
