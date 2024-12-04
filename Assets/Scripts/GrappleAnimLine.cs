using UnityEngine;

public class GrappleAnimLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField]
    Transform PlayerTransform, grapplePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lineRenderer.SetPositions(new Vector3[]{ PlayerTransform.position, grapplePoint.position });
    }
}
