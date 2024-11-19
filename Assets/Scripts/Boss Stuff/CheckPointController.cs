using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class CheckPointController : MonoBehaviour
{
	[Header("General")]
	[SerializeField]
	private GameObject Boss;

	[SerializeField]
	private PlayerController Player;

	[SerializeField]
	private AnimationClip PlayerDeathAnim;

	[SerializeField]
	private CinemachineConfiner2D Cam;

	private bool resetActive = false;

	[Header("Phase One")]
	[SerializeField]
	private GameObject LustFire;

	[SerializeField]
	private GameObject PhaseOneChocolateDrops;

	[SerializeField]
	private GameObject PhaseOneEntrence;

	private Vector3 PhaseOneEntrenceTransform;

	[SerializeField]
	private GameObject PhaseOneExit;

	private Vector3 PhaseOneExitTransform;

	[SerializeField]
	private GameObject PhaseOneAnim;

	[SerializeField]
	private Collider2D AnimOneColider;

	[SerializeField]
	private Collider2D PhaseOneCamColider;

	[Header("Phase Two")]
	[SerializeField]
	private GameObject PhaseTwoRespawnLocal;

	[SerializeField]
	private GameObject MovingCamFollow;

	[SerializeField]
	private GameObject MovingCamStart;

	[SerializeField]
	private Tilemap FloatingPlatforms;

	private Dictionary<Vector3Int, TileBase> originalTiles;

	private bool PhaseTwoSpawnPointSet = false;

	[Header("Phase Three")]
	[SerializeField]
	private GameObject PhaseThreeRespawnLocal;

	[SerializeField]
	private GameObject PhaseThreeTrigger;

	[SerializeField]
	private EndAnim PhaseThreeEndingAnim;

	[SerializeField]
	private GameObject PhaseThreeChocolateDrops;

	[SerializeField]
	private GameObject PhaseThreeEntrence;

	private Vector3 PhaseThreeEntrenceLocal;

	private bool PhaseThreeSpawnPointSet = false;

	private void Start()
	{
		PhaseOneEntrenceTransform = PhaseOneEntrence.transform.position;
		PhaseOneExitTransform = PhaseOneExit.transform.position;
		PhaseThreeEntrenceLocal = PhaseThreeEntrence.transform.position;

		SaveInitialTilemapState();
	}

	private void Update()
	{
		if (Player.IsDead && !resetActive)
		{
			resetActive = true;
			if (Boss.GetComponent<LustBossControllerPhaseOne>().enabled)
			{
				StartCoroutine(ResetPhaseOne());
			}
			else if (Boss.GetComponent<LustBossControllerPhaseTwo>().enabled)
			{
				StartCoroutine(ResetPhaseTwo());
			}
			else if (Boss.GetComponent<LustBossControllerPhaseThree>().enabled)
			{
				StartCoroutine(ResetPhaseThree());
			}
		}

		if (!PhaseTwoSpawnPointSet && Boss.GetComponent<LustBossControllerPhaseTwo>().enabled)
		{
			Player.GetComponent<DeathController>().ResapwnPoint = PhaseTwoRespawnLocal.transform;
			PhaseTwoSpawnPointSet = true;
		}
		else if (!PhaseThreeSpawnPointSet && Boss.GetComponent<LustBossControllerPhaseThree>().enabled)
		{
			Player.GetComponent<DeathController>().ResapwnPoint = PhaseThreeRespawnLocal.transform;
			PhaseThreeSpawnPointSet = true;
		}
	}

	private IEnumerator ResetPhaseOne()
	{
		yield return new WaitForSeconds(PlayerDeathAnim.length);

		Boss.GetComponent<LustBossControllerPhaseOne>().DashEnd();
		Boss.GetComponent<LustBossControllerPhaseOne>().PhaseTimer = Boss.GetComponent<LustBossControllerPhaseOne>().PhaseTime;
		Boss.GetComponent<LustBossControllerPhaseOne>().enabled = false;
		Boss.GetComponent<Animator>().SetBool("BattleStarted", false);
		LustFire.SetActive(false);
		Boss.SetActive(false);

		PhaseOneChocolateDrops.SetActive(false);

		PhaseOneEntrence.transform.position = PhaseOneEntrenceTransform;
		PhaseOneExit.transform.position = PhaseOneExitTransform;

		PhaseOneAnim.SetActive(true);

		AnimOneColider.enabled = true;

		Cam.BoundingShape2D = PhaseOneCamColider;

		foreach (LifeTime life in Resources.FindObjectsOfTypeAll<LifeTime>())
		{
			if (life != null)
			{
				Destroy(life.gameObject);
			}
		}

		//yield return new WaitForSeconds(1);
		resetActive = false;
	}

	private IEnumerator ResetPhaseTwo()
	{
		yield return new WaitForSeconds(PlayerDeathAnim.length);

		Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;
		Boss.GetComponent<LustBossVulnerablePhaseOne>().enabled = true;
		Boss.GetComponent<LustBossVulnerablePhaseOne>().started = true;
		StartCoroutine(Boss.GetComponent<LustBossVulnerablePhaseOne>().StartAnimation());

		MovingCamFollow.transform.position = MovingCamStart.transform.position;
		MovingCamFollow.GetComponent<MoveRight>().setStart(false);
		MovingCamFollow.GetComponent<MoveRight>().StopAllCoroutines();

		PhaseOneExit.SetActive(true);

		foreach (var kvp in originalTiles)
		{
			FloatingPlatforms.SetTile(kvp.Key, kvp.Value);
		}

		//yield return new WaitForSeconds(1);
		resetActive = false;
	}

	private IEnumerator ResetPhaseThree()
	{
		yield return new WaitForSeconds(PlayerDeathAnim.length);

		Boss.GetComponent<LustBossControllerPhaseThree>().StopAllCoroutines();
		Boss.GetComponent<LustBossControllerPhaseThree>().enabled = false;
		Boss.GetComponent<LustBossControllerPhaseThree>().DashEnd();
		Boss.GetComponent<LustBossControllerPhaseThree>().BulletHellActive = false;
		Boss.GetComponent<LustBossControllerPhaseThree>().BulletHellTimer = Boss.GetComponent<LustBossControllerPhaseThree>().BulletHellCooldown;
		Boss.GetComponent<LustBossControllerPhaseThree>().BulletHellPhase = 1;

		PhaseThreeTrigger.SetActive(true);
		PhaseThreeTrigger.GetComponent<Collider2D>().enabled = false;
		PhaseThreeTrigger.GetComponent<StartPhase3>().SetUp(2);

		PhaseThreeChocolateDrops.SetActive(false);

		PhaseThreeEntrence.transform.position = PhaseThreeEntrenceLocal;

		PhaseThreeEndingAnim.started = false;

		foreach (LifeTime life in Resources.FindObjectsOfTypeAll<LifeTime>())
		{
			if (life != null)
			{
				Destroy(life.gameObject);
			}
		}

		resetActive = false;
	}

	void SaveInitialTilemapState()
	{
		originalTiles = new Dictionary<Vector3Int, TileBase>();

		// Iterate through all the tiles in the tilemap's bounds
		BoundsInt bounds = FloatingPlatforms.cellBounds;
		TileBase[] allTiles = FloatingPlatforms.GetTilesBlock(bounds);

		for (int x = 0; x < bounds.size.x; x++)
		{
			for (int y = 0; y < bounds.size.y; y++)
			{
				Vector3Int tilePosition = new Vector3Int(bounds.x + x, bounds.y + y, 0);
				TileBase tile = allTiles[x + y * bounds.size.x];
				if (tile != null)
				{
					originalTiles[tilePosition] = tile;
				}
			}
		}
	}
}
