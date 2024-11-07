using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class FreezeGame : MonoBehaviour
{
	public bool Frozen;

	public void Freeze()
	{
		if (!Frozen)
		{
			Time.timeScale = 0;
			Frozen = true;
		}
	}

	public IEnumerator SlowdownToAFreeze(float duration = .25f)
	{
		if (!Frozen)
		{
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				Time.timeScale = Mathf.Lerp(1, 0, timeElapsed / duration);
				timeElapsed += Time.unscaledDeltaTime;

				yield return null;
			}
			Time.timeScale = 0f;
			Frozen = true;
		}
	}
}
