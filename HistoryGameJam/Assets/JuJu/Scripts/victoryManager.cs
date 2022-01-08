using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class victoryManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI rewardText;
    [SerializeField]
    private TextMeshProUGUI lostText;
    [SerializeField]
    private TextMeshProUGUI killedText;

    private void Awake() {
        rewardText.text = "Rewards: " + BattleManager.reward;
        lostText.text = "units lost: " + BattleManager.unitsLost;
        killedText.text = "units killed: " + BattleManager.unitsKilled;
    } 
}
