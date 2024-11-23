using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class BossMusic : MonoBehaviour
{
	enum eState
	{
		IntroState, PhaseOneState, TransitionOneState, PhaseTwoState, TransitionTwoState, PhaseThreeState, FinalState
	}

	private AudioSource IntroSource, PhaseOneSource, TransitionOneSource, PhaseTwoSource, TransitionTwoSource, PhaseThreeSource, FinalSource, EndingSource;

	private AudioSource[] AudioSources = new AudioSource[7];
	private int AudioSourceIndex = 0;

	[SerializeField]
	private AudioClip Intro, PhaseOne, TransitionOne, PhaseTwo, TransitionTwo, PhaseThree, Final;

	[SerializeField]
	private LustBossControllerPhaseOne BossPhaseOne;
	[SerializeField]
	private LustBossControllerPhaseTwo BossPhaseTwo;
	[SerializeField]
	private LustBossControllerPhaseThree BossPhaseThree;

	[SerializeField]
	private PlayableDirector FinalAnimStart;

	private AudioSource currentSource;
	private eState currentState;
	private bool started = false;
	public bool finish = false;

	private float timer;

	private void Start()
	{
		EndingSource = GetComponent<AudioSource>();

		IntroSource = gameObject.AddComponent<AudioSource>();
		IntroSource.clip = Intro;
		PhaseOneSource = gameObject.AddComponent<AudioSource>();
		PhaseOneSource.clip = PhaseOne;
		TransitionOneSource = gameObject.AddComponent<AudioSource>();
		TransitionOneSource.clip = TransitionOne;
		PhaseTwoSource = gameObject.AddComponent<AudioSource>();
		PhaseTwoSource.clip = PhaseTwo;
		TransitionTwoSource = gameObject.AddComponent<AudioSource>();
		TransitionTwoSource.clip = TransitionTwo;
		PhaseThreeSource = gameObject.AddComponent<AudioSource>();
		PhaseThreeSource.clip = PhaseThree;
		FinalSource = gameObject.AddComponent<AudioSource>();
		FinalSource.clip = Final;

		AudioSources[0] = IntroSource;
		AudioSources[1] = PhaseOneSource;
		AudioSources[2] = TransitionOneSource;
		AudioSources[3] = PhaseTwoSource;
		AudioSources[4] = TransitionTwoSource;
		AudioSources[5] = PhaseThreeSource;
		AudioSources[6] = FinalSource;

		foreach (AudioSource source in AudioSources) 
		{
			InitializeAudioSource(source);
		}

		currentSource = AudioSources[0];

		currentState = eState.IntroState;
	}

	private void InitializeAudioSource(AudioSource source)
	{
		source.playOnAwake = false;
		source.volume = 0.5f;
		source.priority = 1;
	}

	private void FixedUpdate()
	{
		if (!started)
			return;

		timer -= Time.deltaTime;

		if (FinalAnimStart.time > 0)
		{
			StartCoroutine(FadeToEndMusic());
		}

		if (timer <= 0)
		{
			UpdateState();
			if (!currentSource.loop)
			{
				currentSource.volume = 0;
				AudioSourceIndex++;
				currentSource = AudioSources[AudioSourceIndex];
				if (currentSource.clip == PhaseOne || currentSource.clip == PhaseTwo || currentSource.clip == PhaseThree)
				{
					currentSource.loop = true;
				}
				else
				{
					currentSource.loop = false;
				}
				currentSource.Play();
			}

			timer = currentSource.clip.length;
		}
	}

	public void StartAudio()
	{
		currentSource.Play();
		started = true;
		timer = Intro.length;
	}

	private AudioClip GetNextClip()
	{
		switch (currentState)
		{
			case eState.IntroState:
				return Intro;
			case eState.PhaseOneState:
				return PhaseOne;
			case eState.TransitionOneState:
				return TransitionOne;
			case eState.PhaseTwoState:
				return PhaseTwo;
			case eState.TransitionTwoState:
				return TransitionTwo;
			case eState.PhaseThreeState:
				return PhaseThree;
			case eState.FinalState:
				return Final;
			default:
				return null;
		}
	}

	private void UpdateState()
	{
		if (BossPhaseOne.enabled)
		{
			currentState = eState.PhaseOneState;
		}
		else if (BossPhaseTwo.enabled)
		{
			if (currentState == eState.PhaseOneState)
			{
				currentState = eState.TransitionOneState;
				currentSource.loop = false;
			}
			else
			{
				currentState = eState.PhaseTwoState;
			}
		}
		else if (BossPhaseThree.enabled)
		{
			if (finish)
			{
				currentState = eState.FinalState;
			}
			else if (currentState == eState.PhaseTwoState)
			{
				currentState = eState.TransitionTwoState;
				currentSource.loop = false;
			}
			else
			{
				currentState = eState.PhaseThreeState;
			}
		}
	}

	private IEnumerator FadeToEndMusic()
	{
		float timeElapsed = 0;

		while (timeElapsed < 1)
		{
			currentSource.volume = Mathf.Lerp(currentSource.volume, 0, timeElapsed / 1);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		currentSource.volume = 0;

		EndingSource.Play();

		timeElapsed = 0;

		while (timeElapsed < 1)
		{
			EndingSource.volume = Mathf.Lerp(0, 1, timeElapsed / 1);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		EndingSource.volume = 1;

	}
}

