using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerController))]
public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    Slider HealthSlider;

    [SerializeField]
    CanvasGroup Group;

    private bool SliderVisable = true;

    private float SliderVisableCooldown;
    private float SliderVisableTimer;

    PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();

        SliderVisable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthSlider)
        {
            HealthSlider.value = (float) player.Health / (float) player.MaxHealth;

            if (HealthSlider.value == 1 && SliderVisable)
            {
                StopAllCoroutines();
                StartCoroutine(LerpVisablity(1, 0, .25f));
                SliderVisable = false;
            }

            if (HealthSlider.value != 1 && !SliderVisable)
            {
                StopAllCoroutines();
                StartCoroutine(LerpVisablity(0, 1, .1f));
                SliderVisable = true;
            }
        }
	}

	private IEnumerator LerpVisablity(float startValue, float endValue, float lerpDuration)
	{
		float timeElapsed = 0;

		while (timeElapsed < lerpDuration)
		{
			Group.alpha = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		Group.alpha = endValue;
	}
}
