using UnityEngine;

public class HurtPlayerIFOffScreen : MonoBehaviour
{
    [SerializeField]
    Transform PlayerLocal;

    [SerializeField]
    PlayerController Player;

    [SerializeField]
    BoxCollider2D Bounds;

    [SerializeField]
    private float DamageCooldown;

    private float DamageTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DamageTimer = DamageCooldown;
    }

    // Update is called once per frame
    void Update()
    {
		float minX = Bounds.transform.position.x - Bounds.size.x * 0.5f;
		float minY = Bounds.transform.position.y - Bounds.size.y * 0.5f;
		float maxX = Bounds.transform.position.x + Bounds.size.x * 0.5f;
		float maxY = Bounds.transform.position.y + Bounds.size.y * 0.5f;

        if ((PlayerLocal.position.x < minX || PlayerLocal.position.x > maxX ||
            PlayerLocal.position.y < minY || PlayerLocal.position.y > maxY) &&
            DamageTimer <=0)
        {
            Player.TakeDamage(1);
            DamageTimer = DamageCooldown;
        }

        DamageTimer -= Time.deltaTime;
	}
}
