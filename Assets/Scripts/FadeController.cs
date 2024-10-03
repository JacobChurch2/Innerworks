using UnityEngine;

[RequireComponent(typeof(FadingPanel))]
[RequireComponent(typeof(NearObject))]
public class FadeController : MonoBehaviour
{
    private FadingPanel Fade;
    private NearObject Near;
    public bool CanSee;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Fade = GetComponent<FadingPanel>();
        Near = GetComponent<NearObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!CanSee && Near.InRange)
        {
			StartCoroutine(Fade.FadeIn());
            CanSee = true;
        } 
        else if(CanSee && !Near.InRange)
        {
			StartCoroutine(Fade.FadeOut());
            CanSee = false;
        }
    }
}
