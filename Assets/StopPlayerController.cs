using UnityEngine;
using UnityEngine.InputSystem;

public class StopPlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerInput PlayerInput;

    void Awake()
    {
        PlayerInput.enabled = false;
    }
}
