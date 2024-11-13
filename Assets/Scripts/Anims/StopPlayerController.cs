using UnityEngine;
using UnityEngine.InputSystem;

public class StopPlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerInput PlayerInput;

	[SerializeField]
	Rigidbody2D PlayerBody;

	void Awake()
    {
        PlayerInput.enabled = false;
        PlayerBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
