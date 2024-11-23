using UnityEngine;

public class BossDeath : MonoBehaviour
{
    [SerializeField]
    GameObject Boss;

    public void DeathSeqenceStart()
    {
        //TODO: creat death sequence
        Destroy(Boss);
    }
}
