using UnityEngine;

public class StaysOnYCam : MonoBehaviour
{
    public Transform Cam;
    private float CamYStart;
    private float TransformYStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamYStart = Cam.transform.position.y;
        TransformYStart = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float y = TransformYStart + (Cam.position.y - CamYStart);
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        print(y);
    }
}
