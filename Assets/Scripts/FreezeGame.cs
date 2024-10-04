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

	public IEnumerator SlowdownToAFreeze(float duration = 1)
	{
		float SubBy = .01f / duration;
		if (!Frozen)
		{
			for (float timeScale = 1f; timeScale >= 0; timeScale -= SubBy)
			{
				Time.timeScale = timeScale;
				yield return new WaitForSecondsRealtime(0.01f);
			}
			if(Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
			Frozen = true;
		}
	}
}
