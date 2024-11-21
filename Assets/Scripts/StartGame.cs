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
	TextMeshProUGUI Title;

	[SerializeField]
	Camera Cam;

	public void StartTheGame()
	{
		StartCoroutine(LerpButtonVisibility());
		StartCoroutine(LerpTitleVisibility());
		StartCoroutine(LerpCamZ());
		StartCoroutine(LoadNextScene());		
	}

	IEnumerator LerpButtonVisibility()
	{
		float timeElapsed = 0;

		while (timeElapsed < 0.25f)
		{
			StartButton.alpha = Mathf.Lerp(1, 0, timeElapsed / 0.25f);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		StartButton.alpha = 0;
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
			Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y,  Mathf.Lerp(-200, 0, timeElapsed / 1f));
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		Cam.transform.position = new Vector3(Cam.transform.position.x, Cam.transform.position.y, 0);
	}

	IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(1);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
