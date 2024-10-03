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

    bool SequenceStart = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        Frozen = GetComponent<FreezeGame>();
        Near = GetComponent<NearObject>();
    }

    // Update is called once per frame
    void Update()
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
        yield return new WaitForSecondsRealtime(3);
		StartCoroutine(DashText.FadeOutRealTime());
	}
}
