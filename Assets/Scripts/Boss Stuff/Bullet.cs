using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{

    public float lifeTime = 5;
    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > lifeTime)
        {
            Destroy(gameObject);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag.Equals("Player"))
        {
            //TODO: damage player
            print("hit");
        }
	}
}
