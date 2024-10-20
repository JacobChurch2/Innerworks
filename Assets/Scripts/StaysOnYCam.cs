using UnityEngine;

public class StaysOnYCam : MonoBehaviour
{
    public Transform Cam;
    private float CamYStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamYStart = Cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, Cam.position.y, transform.localPosition.z);
        print(Cam.position.y);
    }
}
