using UnityEngine;

public class StartPhase1 : MonoBehaviour
{
	[SerializeField]
    private EndAnim AnimEnding;

	[SerializeField]
	GameObject ChocolateDrops;

	[SerializeField]
	GameObject Boss;

	[SerializeField]
	GameObject BossUI;

	[SerializeField]
	GameObject StartFight;

	[SerializeField]
	GameObject LustsFire;

	[SerializeField]
	Animator BossAnimator;

    // Update is called once per frame
    void Update()
    {
        if (AnimEnding.started)
        {
			ChocolateDrops.SetActive(true);
			LustsFire.SetActive(true);

			if (!Boss.activeSelf)
			{
				Boss.SetActive(true);
			}
			Boss.GetComponent<LustBossControllerPhaseOne>().enabled = true;
			Boss.GetComponent<LustBossControllerPhaseTwo>().enabled = false;
			Boss.GetComponent<LustBossControllerPhaseThree>().enabled = false;

			BossUI.SetActive(true);

			BossAnimator.SetBool("BattleStarted", true);

			Destroy(StartFight, 0.1f);
		}
	}
}
