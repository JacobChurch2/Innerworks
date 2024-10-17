using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	[SerializeField]
    public Animator m_Animator;

    public float transitionTime = 1f;

	public bool PlayTransitionOnOpening = false;

	private void Start()
	{
		m_Animator.SetBool("PlayOnEntry", PlayTransitionOnOpening);
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

		SceneManager.LoadScene(levelIndex);
	}
}
