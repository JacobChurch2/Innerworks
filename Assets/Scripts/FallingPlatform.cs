using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class FallingPlatform : MonoBehaviour
{
    public float FallDelay;
    public float DestoryDelay;
    public float RespawnDelay;
    public float RespawnFadeTime;
    public float FallSpeed;

	public float RegisterTime;
	private float RegisterTimer;

    public float ShakeStrength;

    private bool Falling = false;
    
    private Rigidbody2D rb;
    private Vector3 OriginalPlace;
    private SpriteRenderer TheSprite;
    private Collider2D TheCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TheSprite = GetComponent<SpriteRenderer>();
        TheCollider = GetComponent<Collider2D>();
        OriginalPlace = rb.transform.position;
		RegisterTimer = RegisterTime;
    }

	private void OnCollisionExit2D(Collision2D collision)
	{
		RegisterTimer = RegisterTime;
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (!Falling && collision.gameObject.tag.Equals("Player"))
		{
			RegisterTimer -= Time.deltaTime;
			if (RegisterTimer <= 0)
			{
				transform.DOShakePosition(FallDelay, ShakeStrength);
				StartCoroutine(Fall());
				RegisterTimer = RegisterTime;
				Falling = true;
			}
		}
	}

	private IEnumerator Fall()
    {
        yield return new WaitForSeconds(FallDelay);
		rb.linearVelocityY = Vector2.down.y * FallSpeed;
        //Destroy(gameObject, DestoryDelay);
        StartCoroutine(FadeOut());
        StartCoroutine(Death());
	}
    private IEnumerator FadeOut()
    {
		float timeElapsed = 0;

		while (timeElapsed < DestoryDelay)
		{
			TheSprite.color = new Color(TheSprite.color.r, TheSprite.color.g, TheSprite.color.b, Mathf.Lerp(1, 0, timeElapsed / DestoryDelay));
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		TheSprite.color = new Color(TheSprite.color.r, TheSprite.color.g, TheSprite.color.b, 0);
	}

    private IEnumerator Death()
    {
		yield return new WaitForSeconds(DestoryDelay);
		TheSprite.enabled = false;
		TheCollider.enabled = false;
		rb.simulated = false;
        StartCoroutine(Respawn());
	}

    private IEnumerator Respawn()
    {
		yield return new WaitForSeconds(RespawnDelay);
		transform.position = OriginalPlace;
		TheSprite.enabled = true;
		TheCollider.enabled = true;
		rb.linearVelocity = Vector2.zero;
		Falling = false;
		rb.simulated = true;
		StartCoroutine(FadeIn());
	}

	private IEnumerator FadeIn()
	{
		float timeElapsed = 0;

		while (timeElapsed < RespawnFadeTime)
		{
			TheSprite.color = new Color(TheSprite.color.r, TheSprite.color.g, TheSprite.color.b, Mathf.Lerp(0, 1, timeElapsed / RespawnFadeTime));
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		TheSprite.color = new Color(TheSprite.color.r, TheSprite.color.g, TheSprite.color.b, 1);
	}
}
