using UnityEngine;

public class NearObject : MonoBehaviour
{
    [SerializeField]
    Transform otherTransform;
    [SerializeField]
    float distance;

    public bool InRange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((this.transform.position - otherTransform.position).magnitude < distance)
        {
            InRange = true;
        } else
        {
            InRange = false;
        }
    }
}
