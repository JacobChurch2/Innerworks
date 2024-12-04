using UnityEngine;

public class DisableMouse : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = false;
	}
}
