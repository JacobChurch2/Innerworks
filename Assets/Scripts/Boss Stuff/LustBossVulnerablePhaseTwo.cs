using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class LustBossVulnerablePhaseTwo: MonoBehaviour
{
	[SerializeField]
	private Transform RestLocation;
	public float RestTimer;

	private bool started = false;

	// Update is called once per frame
	void Update()
	{
		if (!started) return;

		RestTimer -= Time.deltaTime;

		if (RestTimer <= 0)
		{
			//TODO: connect animation and then start next phase
			EndPhase();
		}
	}

	public void StartVulnerablePhase()
	{
		started = true;

		transform.DOLocalMove(RestLocation.position, 2f);
	}

	private void EndPhase()
	{
		started = false;

		GetComponent<LustBossControllerPhaseThree>().enabled = true;

		this.enabled = false;
	}
}
