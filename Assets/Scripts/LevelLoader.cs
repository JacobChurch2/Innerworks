using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	[SerializeField]
    public Animator m_Animator;

    public float transitionTime = 1f;

	public bool PlayTransitionOnOpening = false;
	public bool PlayTransitionOnClosing = false;

	private void Start()
	{
		m_Animator.SetBool("PlayOnEntry", PlayTransitionOnOpening);
		m_Animator.SetBool("PlayOnClosing", PlayTransitionOnClosing);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			LoadNextLevel();
		}
	}

	public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelWithtranstion(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevelWithtranstion(int levelIndex)
    {
        m_Animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

		PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex + 1);

		SceneManager.LoadScene(levelIndex);
	}
}
