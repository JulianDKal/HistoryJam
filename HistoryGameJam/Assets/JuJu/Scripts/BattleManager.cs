using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> listOfUnitsPlaceable;
    public EnemyAI enemy;

    public static AttackOrMove attackOrMove;
    public static BattleState battleState;
    public static List<Unit> activePlayerUnits;
    public static List<Unit> activeEnemyUnits;
    public static int turnsSinceWaveStart;
    public static int waveCount;

    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
        battleState = BattleState.NEWWAVE;
        waveCount = 0;
        NextWave();
    }

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
        ENEMYTURN
    }

    public void NextWave()
    {
        if(battleState == BattleState.NEWWAVE)
        {
            enemy.PlaceWave();
            waveCount++;
            Debug.Log("Wave placed");
            SetState(BattleState.PLAYERPLACE);
        }
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
    }

    public static void SetState(BattleState bs)
    {
        battleState = bs;
        //Debug.Log("State change");
    }

    private void Update() {
        
    }
}
