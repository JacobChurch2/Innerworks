using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
	public ParallaxCamera parallaxCamera;
	List<ParallaxEffect> parallaxLayers = new List<ParallaxEffect>();

	void Start()
	{
		if (parallaxCamera == null)
			parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

		if (parallaxCamera != null)
			parallaxCamera.onCameraTranslate += Move;

		SetLayers();
	}

	void SetLayers()
	{
		parallaxLayers.Clear();

		for (int i = 0; i < transform.childCount; i++)
		{
			ParallaxEffect layer = transform.GetChild(i).GetComponent<ParallaxEffect>();

			if (layer != null)
			{
				layer.name = "Layer-" + i;
				parallaxLayers.Add(layer);
			}
		}
	}

	void Move(float delta)
	{
		foreach (ParallaxEffect layer in parallaxLayers)
		{
			layer.Move(delta);
		}
	}
}
