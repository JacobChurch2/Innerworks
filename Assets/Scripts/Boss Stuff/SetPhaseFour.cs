using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TalkingController))]
public class SetPhaseFour : MonoBehaviour
{
	[SerializeField]
	PhaseManager phaseManager;

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
			this.enabled = false;
			//gameObject.SetActive(false);
		}
	}
}
