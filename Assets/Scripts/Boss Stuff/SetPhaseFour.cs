using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TalkingController))]
public class SetPhaseFour : MonoBehaviour
{
	[SerializeField]
	PhaseManager phaseManager;

	[SerializeField]
	Animator BossAnimator;

	[SerializeField]
	GameObject ChocolateDropsPhaseThree;

	private TalkingController talk;

	private void Start()
	{
		talk = GetComponent<TalkingController>();
	}

	void Update()
	{
		if (talk.done)
		{
			phaseManager.Phase = 4;
			BossAnimator.SetTrigger("Defeated");
			ChocolateDropsPhaseThree.SetActive(false);
			this.enabled = false;
			//gameObject.SetActive(false);
		}
	}
}
