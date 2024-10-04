using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(FreezeGame))]
[RequireComponent(typeof(NearObject))]
public class DashUnlockController : MonoBehaviour
{
    FreezeGame Frozen;
    NearObject Near;

    [SerializeField]
    FadingPanel CloseText;
	[SerializeField]
	FadingPanel DashText;
    [SerializeField]
    PlayerController Player;

    bool SequenceStart = false;
    bool SequenceEnd = false;

	void Start()
    {
        Frozen = GetComponent<FreezeGame>();
        Near = GetComponent<NearObject>();
    }

	private void Update()
	{
		if (SequenceEnd && Player.Dashing)
		{
			Time.timeScale = 1.0f;
		}
	}

	void FixedUpdate()
    {
        if (Near.InRange && !SequenceStart)
        {
            StartCoroutine(Frozen.SlowdownToAFreeze(0.25f));
            StartCoroutine(DashSequence());
            SequenceStart = true;
        }
    }

    private IEnumerator DashSequence()
    {
        StartCoroutine(CloseText.FadeInRealTime());
		yield return new WaitForSecondsRealtime(3);
		StartCoroutine(CloseText.FadeOutRealTime());
		StartCoroutine(DashText.FadeInRealTime());
        SequenceEnd = true;
        Player.DashUnlocked = true;
        yield return new WaitForSecondsRealtime(3);
		StartCoroutine(DashText.FadeOutRealTime());
	}
}
