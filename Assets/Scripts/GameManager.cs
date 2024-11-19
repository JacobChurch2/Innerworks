using UnityEngine;
using UnityEngine.Splines;

public class GameManager : MonoBehaviour
{
    //private NativeSpline[] Splines;

    public bool DevMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown("k") && Input.GetKeyDown("d") && Input.GetKeyDown("a"))
        {
            print("Dev Mode Switched");
            DevMode = !DevMode;
        }

        UpdateSplines();
    }

    private void UpdateSplines()
    {
  //      if (Splines != null && Splines.Length > 0)
  //      {
  //          for (int i = 0; i < Splines.Length; i++) {
  //              using (var nativeSpline = Splines[i])
  //              {
  //                  // Use nativeSpline
  //              }
  //              //Atomatically dispose
  //          }
		//}
    }
}
