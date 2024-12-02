using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField]
    float WaitToStartMoving;

	[SerializeField]
	float WaitToEnd;

	[SerializeField]
    float length;

    [SerializeField]
    float endY;

    [SerializeField]
    TextMeshPro EndingText;

	[SerializeField]
	float fadeTime;

	private float WaitTimer;
    private float EndTimer;
    private bool moving = false;
    private bool ending = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WaitTimer = WaitToStartMoving;
        EndTimer = WaitToEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			SceneManager.LoadScene(0);
		}

        if (transform.position.y == endY && !ending)
        {
            EndTimer -= Time.deltaTime;
            if (EndTimer < 0)
            {
                StartCoroutine(FadeOutSleepyFruit());
                ending = true;
            }
		}

        if (moving) return;

        WaitTimer -= Time.deltaTime;

        if (WaitTimer < 0)
        {
            StartCoroutine(LerpCamPosition());
            moving = true;
        }
    }

	IEnumerator LerpCamPosition()
	{
		float timeElapsed = 0;

		while (timeElapsed < length)
		{
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, endY, timeElapsed / length), transform.position.z);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		transform.position = new Vector3(transform.position.x, endY, transform.position.z);
	}

	IEnumerator FadeOutSleepyFruit()
	{
		float timeElapsed = 0;

		while (timeElapsed < fadeTime)
		{
			EndingText.alpha = Mathf.Lerp(1, 0, timeElapsed / fadeTime);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

        EndingText.alpha = 0;

        yield return new WaitForSeconds(.5f);

		SceneManager.LoadScene(0);
	}
}
