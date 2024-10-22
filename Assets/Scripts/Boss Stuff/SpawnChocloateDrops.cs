using UnityEngine;

public class SpawnChocloateDrops : MonoBehaviour
{
    [SerializeField]
    GameObject Drop;

    [SerializeField]
    Quaternion Rotation;

    public float xBoundLeft;
    public float xBoundRight;
    public float yBoundTop;
    public float yBoundBottom;
    public float CoolDownMax;
    public float CoolDownMin;
    private float Timer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Timer = Random.Range(CoolDownMin,CoolDownMax);
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer <= 0)
        {
            float x = Random.Range(xBoundLeft, xBoundRight);
            float y = Random.Range(yBoundTop, yBoundBottom);
            Vector3 location = new Vector3(x, y, 0);
            Instantiate(Drop, location, Rotation);

			Timer = Random.Range(CoolDownMin, CoolDownMax);
		}
    }
}
