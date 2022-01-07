using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> listOfUnitsPlaceable;
    public EnemyAI enemy;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private int maxTurnsPerWave;
    [SerializeField]
    private int moneyPerWave = 100;

    public static AttackOrMove attackOrMove;
    public static BattleState battleState;
    public static List<GameObject> activePlayerUnits = new List<GameObject>();
    public static List<GameObject> activeEnemyUnits = new List<GameObject>();
    public static int turnsSinceWaveStart;
    public static int waveCount;

    public enum AttackOrMove
    {
        ATTACK,
        MOVE
    }

    public enum BattleState 
    {
        NEWWAVE, 
        PLAYERPLACE, 
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
        waveCount = 0;

        SetState(BattleState.NEWWAVE);
        StartCoroutine(WaitForNextWave());
    }

    private IEnumerator WaitForNextWave()
    {
        while(waveCount <= enemy.waves.Length){
            yield return new WaitUntil(() => battleState == BattleState.NEWWAVE);
            NextWave();
        }
        //if all waves are complete, you win
        SetState(BattleState.WIN);
    }

    private void NextWave()
    {
        turnsSinceWaveStart = 0;
        enemy.PlaceWave();
        Card.GiveMoney(moneyPerWave);
        waveCount++;

        SetState(BattleState.PLAYERPLACE);
        startButton.SetActive(true);
    }

    public void EnemyTurn()
    {
        if(turnsSinceWaveStart < maxTurnsPerWave)
        {
            SetState(BattleManager.BattleState.ENEMYTURN);
            StartCoroutine(enemy.ExecuteEnemyTurn());
        }
        else
            SetState(BattleState.NEWWAVE);
    }

    public void ToggleAttackMove(TextMeshProUGUI textMeshPro)
    {
        if(attackOrMove == AttackOrMove.MOVE)
        {
            attackOrMove = AttackOrMove.ATTACK;
            textMeshPro.text = "ATTACK";
        }
        else
        {
            attackOrMove = AttackOrMove.MOVE;
            textMeshPro.text = "MOVE";
        }
    }

    public void ToggleStartCombat(TextMeshProUGUI textMeshPro)
    {
        if(battleState == BattleState.PLAYERPLACE)
        {
            SetState(BattleState.PLAYERTURN);
        }
        //when move undo is implemented, we may move enemy turn here
    }

    public static void SetState(BattleState bs)
    {
        battleState = bs;
        //Debug.Log("State change");
    }

    private void Update() {

    }
}
