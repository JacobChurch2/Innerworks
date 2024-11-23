using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class BossHealth : MonoBehaviour
{
	[SerializeField]
	Slider HealthBar;

	[SerializeField]
	public float MaxHealth;

	[SerializeField]
	private float damage;

	[SerializeField]
	private PhaseManager phaseManager;

	[SerializeField]
	SpriteRenderer BossSprite;

	[NonSerialized]
	public float health;

	[NonSerialized]
	public bool dead = false;

	private bool CanTakeDamage = true;


	private Collider2D HitBox;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		HitBox = GetComponent<Collider2D>();
		health = MaxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		HealthBar.value = health / MaxHealth;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && collision.GetComponent<PlayerController>().Dashing && CanTakeDamage)
		{
			TakeDamage();
		}
	}

	private void TakeDamage()
	{
		if (phaseManager.Phase == 2) return;

		if (phaseManager.Phase == 4)
		{
			dead = true;
			GetComponent<BossDeath>().DeathSeqenceStart();
		}

		CanTakeDamage = false;
		health -= damage;
		phaseManager.UpdatePhase(health, MaxHealth);
		StartCoroutine(DamageAnim());

		
	}

	private IEnumerator DamageAnim()
	{
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 0);
		yield return new WaitForSeconds(.05f);
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 1);
		yield return new WaitForSeconds(.05f);
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 0);
		yield return new WaitForSeconds(.05f);
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 1);
		yield return new WaitForSeconds(.05f);
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 0);
		yield return new WaitForSeconds(.05f);
		BossSprite.color = new Color(BossSprite.color.r, BossSprite.color.g, BossSprite.color.b, 1);
		CanTakeDamage = true;
	}
}
