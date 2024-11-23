using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class LustBossVulnerablePhaseTwo: MonoBehaviour
{
	[SerializeField]
	private Transform RestLocation;
	public float MoveTimer;

	private bool started = false;

	[SerializeField]
	PlayableDirector FinalAnim;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!started) return;

		MoveTimer -= Time.deltaTime;

		if (MoveTimer <= 0)
		{
			FinalAnim.Play();
			EndPhase();
		}
	}

	public void StartVulnerablePhase()
	{
		started = true;

		transform.DOLocalMove(RestLocation.position, MoveTimer);

		GetComponent<LustBossControllerPhaseThree>().enabled = false;
	}

	private void EndPhase()
	{
		started = false;

		this.enabled = false;
	}
}
