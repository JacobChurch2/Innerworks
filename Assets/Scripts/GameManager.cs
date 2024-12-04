using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
	public bool DevMode;

	[SerializeField]
	GameObject PauseScreen;

	[SerializeField]
	GameObject Music;

	[SerializeField]
	PlayerInput playerInput;

	[SerializeField]
	Transform ResumeBtn;

	private AudioSource PauseMusic;

	private AudioSource LevelMusic;
	private float levelMusicVolume;

	private bool paused = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
		PauseMusic = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("p"))
		{
			foreach (TalkingController talk in FindObjectsByType<TalkingController>(FindObjectsSortMode.None))
			{
				if (talk.started) return;
			}

			paused = !paused;
			if (paused)
			{
				OnPause();
			} 
			else
			{
				OnResume();
			}
		}

		if (Input.GetKeyDown("k") && Input.GetKeyDown("d") && Input.GetKeyDown("a"))
		{
			print("Dev Mode Switched");
			DevMode = !DevMode;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (PauseScreen && !paused)
			{
				foreach (TalkingController talk in FindObjectsByType<TalkingController>(FindObjectsSortMode.None))
				{
					if (talk.started) return;
				}

				OnPause();
			}
			else
			{
				OnQuit();
			}
		}
	}

	private void OnPause()
	{
		Time.timeScale = 0;
		PauseScreen.SetActive(true);
		if (Music)
		{
			foreach (AudioSource audio in Music.GetComponents<AudioSource>())
			{
				if (audio.isPlaying)
				{
					LevelMusic = audio;
					levelMusicVolume = audio.volume;
					audio.Pause();
				}
			}
		}
		PauseMusic.Play();
		StartCoroutine(FadeMusic(PauseMusic, 0, 1, 2));
		playerInput.enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void OnResume()
	{
		Time.timeScale = 1;
		PauseScreen.SetActive(false);
		if (LevelMusic)
		{
			LevelMusic.UnPause();
			StartCoroutine(FadeMusic(LevelMusic, 0, levelMusicVolume, 2));
		}
		PauseMusic.Stop();
		ResumeBtn.localScale = new Vector3(1, 1, 1);
		playerInput.enabled = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	IEnumerator FadeMusic(AudioSource TheSource, float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			TheSource.volume = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.fixedDeltaTime;

			yield return null;
		}

		TheSource.volume = endValue;
	}
}
