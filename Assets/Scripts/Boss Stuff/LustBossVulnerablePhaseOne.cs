using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class LustBossVulnerablePhaseOne : MonoBehaviour
{
	[SerializeField]
	private Transform RestLocation;
	public float RestTimer;

	[SerializeField]
	CinemachineConfiner2D Confiner;

	[SerializeField]
	GameObject MovingCamFollow;

	[SerializeField]
	CinemachineCamera Cam;

	[SerializeField]
	Collider2D NextPhaseCam;

	[SerializeField]
	PlayableDirector AnimStart;

	[SerializeField]
	TalkingController Talking;

	[SerializeField]
	EndAnim Finish;

	[SerializeField]
	DestroyGrounds Destruction;

	public bool started = false;
	private bool ending = false;

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
	void Update()
	{
		if (!started) return;

		RestTimer -= Time.deltaTime;

		if (RestTimer <= 0 && !ending)
		{
			StartCoroutine(StartAnimation());
		}

		if (Finish.started)
		{
			EndPhase();
			Finish.started = false;
		}
	}

	public void StartVulnerablePhase()
	{
 		if (animator)
		{
			animator.SetBool("Tired", true);
		}
		started = true;
		transform.DOLocalMove(RestLocation.position, 2f);
	}

	public IEnumerator StartAnimation()
	{
		if (animator)
		{
			animator.SetBool("Tired", false);
		}
		ending = true;
		AnimStart.gameObject.SetActive(true);
		AnimStart.Play();
		yield return new WaitForSeconds((float)AnimStart.duration);
		Talking.gameObject.SetActive(true);
		Talking.StartText();
		Destruction.enabled = true;
	}

	private void EndPhase()
	{
		started = false;

		Destruction.enabled = false;

		GetComponent<LustBossControllerPhaseTwo>().enabled = true;
		Confiner.BoundingShape2D = NextPhaseCam;
		Cam.Follow = MovingCamFollow.transform;

		if (MovingCamFollow.GetComponent<MoveRight>())
		{
			MovingCamFollow.GetComponent<MoveRight>().setStart(true);
		}

		if (Cam.GetComponent<CamXMatchesPlayerX>())
		{
			Cam.GetComponent<CamXMatchesPlayerX>().FollowingPlayer = false;
		}

		this.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!this.enabled) return;
		if (collision.tag.Equals("Player") && !ending)
		{
			if (collision.GetComponent<PlayerController>().Dashing == true)
			{
				StartCoroutine(StartAnimation());
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!this.enabled) return;
		if (collision.tag.Equals("Player") && !ending)
		{
			if (collision.GetComponent<PlayerController>().Dashing == true)
			{
				StartCoroutine(StartAnimation());
			}
		}
	}
}
