using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI StartButton;

	[SerializeField]
	TextMeshProUGUI ContinueButton;

	[SerializeField]
	TextMeshProUGUI Title;

	[SerializeField]
	Camera Cam;

	public void StartTheGame()
	{
		StartCoroutine(LerpButtonVisibility(StartButton));
		StartCoroutine(LerpButtonVisibility(ContinueButton));
		StartCoroutine(LerpTitleVisibility());
		StartCoroutine(LerpCamZ());
		StartCoroutine(LoadNextScene());
	}

	public void Continue()
	{
		StartCoroutine(LerpButtonVisibility(StartButton));
		StartCoroutine(LerpButtonVisibility(ContinueButton));
		StartCoroutine(LerpTitleVisibility());
		StartCoroutine(LerpCamZ());
		StartCoroutine(LoadSavedScene());
	}

	IEnumerator LerpButtonVisibility(TextMeshProUGUI btn)
	{
		float timeElapsed = 0;

		while (timeElapsed < 0.25f)
		{
			btn.alpha = Mathf.Lerp(1, 0, timeElapsed / 0.25f);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		btn.alpha = 0;
	}

	IEnumerator LerpTitleVisibility()
	{
		float timeElapsed = 0;

		while (timeElapsed < 0.25f)
		{
			Title.alpha = Mathf.Lerp(1, 0, timeElapsed / 0.25f);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		Title.alpha = 0;
	}

	IEnumerator LerpCamZ()
	{
		float timeElapsed = 0;

		while (timeElapsed < 1f)
		{
			Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y, Mathf.Lerp(-200, 0, timeElapsed / 1f));
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y, 0);
	}

	IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(1);
		PlayerPrefs.SetInt("CurrentLevel", 1);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	IEnumerator LoadSavedScene()
	{
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel", 1));
	}
}
