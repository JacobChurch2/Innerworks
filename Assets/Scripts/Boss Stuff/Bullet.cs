using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag.Equals("Player"))
        {
            //TODO: damage player
            print("kiss hit");
        }
	}
}
