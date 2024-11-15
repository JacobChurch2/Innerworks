using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class StopNavMeshRotation : MonoBehaviour
{
	private NavMeshAgent agent;
	private Quaternion initialRotation;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;

		initialRotation = transform.rotation;
	}

	private void Update()
	{
		transform.rotation = initialRotation;
	}
}
