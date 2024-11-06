//using System.Collections;
//using UnityEngine;

//public class BossMusic : MonoBehaviour
//{
//	enum eState
//	{
//		IntroState, PhaseOneState, TransitionOneState, PhaseTwoState, TransitionTwoState, PhaseThreeState, FinalState
//	}

//	private AudioSource Intro, PhaseOne, TransitionOne, PhaseTwo, TransitionTwo, PhaseThree, Final;

//	[SerializeField]
//	AudioClip IntroClip, PhaseOneClip, TransitionOneClip, PhaseTwoClip, TransitionTwoClip, PhaseThreeClip, FinalClip;

//	[SerializeField]
//	LustBossControllerPhaseOne BossPhaseOne;
//	[SerializeField]
//	LustBossControllerPhaseTwo BossPhaseTwo;
//	[SerializeField]
//	LustBossControllerPhaseThree BossPhaseThree;

//	private eState TheState;
//	private bool Started = false;
//	public bool Finish = false;

//	void Start()
//	{
//		Intro = gameObject.AddComponent<AudioSource>();
//		Intro.clip = IntroClip;
//		PhaseOne = gameObject.AddComponent<AudioSource>();
//		PhaseOne.clip = PhaseOneClip;
//		TransitionOne = gameObject.AddComponent<AudioSource>();
//		TransitionOne.clip = TransitionOneClip;
//		PhaseTwo = gameObject.AddComponent<AudioSource>();
//		PhaseTwo.clip = PhaseTwoClip;
//		TransitionTwo = gameObject.AddComponent<AudioSource>();
//		TransitionTwo.clip = TransitionTwoClip;
//		PhaseThree = gameObject.AddComponent<AudioSource>();
//		PhaseThree.clip = PhaseThreeClip;
//		Final = gameObject.AddComponent<AudioSource>();
//		Final.clip = FinalClip;

//		foreach (AudioSource source in gameObject.GetComponents<AudioSource>())
//		{
//			source.volume = 1.0f;
//			source.priority = 1;
//		}

//		TheState = eState.IntroState;
//	}

//	void FixedUpdate()
//	{
//		if (!Started) return;
//		foreach (AudioSource source in gameObject.GetComponents<AudioSource>())
//		{
//			if (source.isPlaying) return;
//		}

//		UpdateState();
//		UpdateClip();
//	}

//	public void StartAudio()
//	{
//		Intro.Play();
//		Started = true;
//	}

//	private void UpdateClip()
//	{
//		switch (TheState)
//		{
//			case eState.IntroState:
//				Intro.Play();
//				break;
//			case eState.PhaseOneState:
//				PhaseOne.Play();
//				break;
//			case eState.TransitionOneState:
//				TransitionOne.Play();
//				break;
//			case eState.PhaseTwoState:
//				PhaseTwo.Play();
//				break;
//			case eState.TransitionTwoState:
//				TransitionTwo.Play();
//				break;
//			case eState.PhaseThreeState:
//				PhaseThree.Play();
//				break;
//			case eState.FinalState:
//				Final.Play();
//				break;

//		}
//	}

//	private void UpdateState()
//	{
//		if (BossPhaseOne.enabled)
//		{
//			TheState = eState.PhaseOneState;
//		}
//		else if (BossPhaseTwo.enabled)
//		{
//			TheState = TheState == eState.PhaseOneState ? eState.TransitionOneState : eState.PhaseTwoState;
//		}
//		else if (BossPhaseThree.enabled)
//		{
//			if (Finish)
//			{
//				TheState = eState.FinalState;
//			}
//			else
//			{
//				TheState = TheState == eState.PhaseTwoState ? eState.TransitionTwoState : eState.PhaseThreeState;
//			}
//		}
//	}
//}

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class BossMusic : MonoBehaviour
{
	enum eState
	{
		IntroState, PhaseOneState, TransitionOneState, PhaseTwoState, TransitionTwoState, PhaseThreeState, FinalState
	}

	private AudioSource IntroSource, PhaseOneSource, TransitionOneSource, PhaseTwoSource, TransitionTwoSource, PhaseThreeSource, FinalSource;

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
	AudioMixerGroup TheMixerOne, TheMixerTwo;

	//private AudioSource sourceA;
	//private AudioSource sourceB;
	private AudioSource currentSource;
	private eState currentState;
	private bool started = false;
	public bool finish = false;

	private float timer;

	private float CrossFadeTime = 0.42f;

	private void Start()
	{
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
		source.outputAudioMixerGroup = TheMixerOne;
	}

	private void FixedUpdate()
	{
		if (!started)
			return;

		// Check if the currently playing audio source is nearing the end
		//if ((currentSource.isPlaying || currentSource.loop) && currentSource.time >= currentSource.clip.length - CrossFadeTime)
		//{
		//	UpdateState();
		//	if (!currentSource.loop)
		//	{
		//		CrossfadeToNextClip();
		//	}
		//}

		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			UpdateState();
			if (!currentSource.loop)
			{
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

	//private void CrossfadeToNextClip()
	//{
	//	AudioClip nextClip = GetNextClip();
	//	AudioSource activeSource = sourceA.isPlaying ? sourceA : sourceB;
	//	AudioSource inactiveSource = activeSource == sourceA ? sourceB : sourceA;

	//	// Set up the next clip on the inactive source
	//	inactiveSource.clip = nextClip;
	//	inactiveSource.Play();

	//	if (nextClip == PhaseOne || nextClip == PhaseTwo || nextClip == PhaseThree)
	//	{
	//		inactiveSource.loop = true;
	//	}
	//	else
	//	{
	//		inactiveSource.loop = false;
	//	}

	//	//float timer = 0.0f;

	//	//while (timer < CrossFadeTime)
	//	//{
	//	//	activeSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", Mathf.Lerp(0.0f, -80.0f, timer / CrossFadeTime));
	//	//	inactiveSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", Mathf.Lerp(-80.0f, 0.0f, timer / CrossFadeTime));
	//	//	timer += Time.deltaTime;
	//	//	yield return null;
	//	//}

	//	// Ensure active source is fully stopped after crossfade
	//	//activeSource.Stop();
	//	//activeSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", -80);
	//	//inactiveSource.outputAudioMixerGroup.audioMixer.SetFloat("Volume", 0); 
	//	currentSource = inactiveSource;
	//}

	//private IEnumerator CrossfadeToNextClip()
	//{
	//	AudioClip nextClip = GetNextClip();
	//	AudioSource nextSource = (currentSource == sourceA) ? sourceB : sourceA;

	//	// Prepare the next source with new clip and volume
	//	nextSource.clip = nextClip;
	//	nextSource.volume = 0f;
	//	nextSource.Play();

	//	if(nextClip == PhaseOne || nextClip == PhaseTwo || nextClip == PhaseThree)
	//	{
	//		nextSource.loop = true;
	//	} else
	//	{
	//		nextSource.loop = false;
	//	}

	//	// Perform crossfade
	//	float timer = 0.0f;

	//	while (timer < CrossFadeTime)
	//	{
	//		timer += Time.deltaTime;
	//		currentSource.volume = Mathf.Lerp(1.0f, 0.0f, timer / CrossFadeTime);
	//		nextSource.volume = Mathf.Lerp(0.0f, 1.0f, timer / CrossFadeTime);
	//		yield return null;
	//	}

	//	// Stop the previous source and swap to the new one
	//	currentSource.Stop();
	//	currentSource = nextSource;
	//}

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
}

