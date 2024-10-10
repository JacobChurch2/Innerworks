using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
	private CinemachineCamera CinemachineVirtualCamera;
	private float timer;
	private CinemachineBasicMultiChannelPerlin _cbmcp;

	void Awake()
	{
		CinemachineVirtualCamera = GetComponent<CinemachineCamera>();
	}

	private void Start()
	{
		StopShake();
	}

	public void ShakeCamera(float ShakeIntensity = 1f, float ShakeTime = 0.2f)
	{
		CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
		_cbmcp.AmplitudeGain = ShakeIntensity;

		timer = ShakeTime;
	}

	void StopShake()
	{
		CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
		_cbmcp.AmplitudeGain = 0f;
		timer = 0;
	}

	void Update()
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				StopShake();
			}
		}
	}
}
