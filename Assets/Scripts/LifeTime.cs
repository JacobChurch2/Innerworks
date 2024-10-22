using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float lifeTime;
    private float Timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Timer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
