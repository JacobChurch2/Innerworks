using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent (typeof(CinemachineFollow))]
public class CamXMatchesPlayerX : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D Player;

    public float XLookBack = -5;
    public float Duration = .25f;

    private CinemachineFollow follow;
    private bool Left;
    private float StartOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        follow = GetComponent<CinemachineFollow>();
        StartOffset = follow.FollowOffset.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.linearVelocityX < -0.1 && !Left)
        {
			StopAllCoroutines();
			StartCoroutine(OffsetLerp(XLookBack));
            Left = true;
        } 
        else if(Player.linearVelocityX > 0.1 && Left)
        {
			StopAllCoroutines();
			StartCoroutine(OffsetLerp(StartOffset));
            Left = false;
        }
    }

	private IEnumerator OffsetLerp(float offset)
	{
		float timeElapsed = 0;
        float start = follow.FollowOffset.x;

		while (timeElapsed < Duration)
		{
			follow.FollowOffset.x = Mathf.Lerp(start, offset, timeElapsed / Duration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		follow.FollowOffset.x = offset;
	}
}
