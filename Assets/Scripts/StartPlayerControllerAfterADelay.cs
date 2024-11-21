using UnityEngine;
using UnityEngine.InputSystem;

public class StartPlayerControllerAfterADelay : MonoBehaviour
{
    [SerializeField]
    PlayerInput PlayerInput;
    [SerializeField]
    Rigidbody2D PlayerBody;

    [SerializeField]
    private float Delay;

    // Update is called once per frame
    void Update()
    {
        Delay -= Time.deltaTime;

        if (Delay <= 0)
        {
			if (PlayerInput != null)
			{
				PlayerInput.enabled = true;
			}

			if (PlayerBody != null)
			{
				PlayerBody.bodyType = RigidbodyType2D.Dynamic;
			}

            this.enabled = false;
		}
    }
}
