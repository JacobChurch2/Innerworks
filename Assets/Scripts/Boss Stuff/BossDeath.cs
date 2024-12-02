using UnityEngine;
using UnityEngine.Playables;

public class BossDeath : MonoBehaviour
{
    [SerializeField]
    GameObject Boss;

    [SerializeField]
    PlayableDirector Anim;

    public void DeathSeqenceStart()
    {
        Anim.Play();
    }
}
