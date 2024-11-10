using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(TalkingController))]
public class DoneTalkingSoStartNextAnimation : MonoBehaviour
{
    [SerializeField]
    PlayableDirector anim;

    private TalkingController talk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        talk = GetComponent<TalkingController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (talk.done)
        {
            anim.Play();
			Destroy(gameObject);
		}
    }
}