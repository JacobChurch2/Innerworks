using UnityEngine;
using UnityEngine.Playables;

[RequireComponent (typeof(PlayableDirector))]
public class StartTalkingAfterAnim : MonoBehaviour
{
    [SerializeField]
    private TalkingController Talking;
    private PlayableDirector Anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Anim = GetComponent<PlayableDirector>();
        Anim.played += OnPlayableDirectorHasStopped;
	}

    void OnPlayableDirectorHasStopped(PlayableDirector director)
    {
		Talking.StartText();
	}
}
