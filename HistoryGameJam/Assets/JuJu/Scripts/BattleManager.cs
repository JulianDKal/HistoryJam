using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> listOfUnitsPlacable;

    public static AttackOrMove attackOrMove;
    public static BattleState battleState;
    public static List<Unit> activePlayerUnits;
    public static List<Unit> activeEnemyUnits;
    public static int turnsSinceWaveStart;

    public delegate void OnMove(Vector2 vector);
    public static OnMove onMove;

    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
        battleState = BattleState.PLAYERPLACE;
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
            GameObject.Find("StartButton").SetActive(false);
        }
    }

    public static void SetState(BattleState bs)
    {
        battleState = bs;
        Debug.Log("State change");
    }

    private void Update() {
        
    }
}
