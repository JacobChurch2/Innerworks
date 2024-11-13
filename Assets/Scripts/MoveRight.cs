using System.Collections;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    private bool start = false;

    [SerializeField]
    private Transform startLocal, endLocal;

    [SerializeField]
    private float time;

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Move());
        }
    }

    public void setStart(bool start)
    {
        this.start = start;
    }

    private IEnumerator Move()
    {
		float currentTime = 0;
		while (currentTime < time)
		{
			transform.position = new Vector3(Mathf.Lerp(startLocal.position.x, endLocal.position.x, currentTime / time), transform.position.y, transform.position.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
	}
}
