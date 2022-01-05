using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> listOfUnitsPlacable;

    public static AttackOrMove attackOrMove;
    public static List<Unit> activePlayerUnits;
    public static List<Unit> activeEnemyUnits;
    public static int turnsSinceWaveStart;

    public delegate void OnMove(Vector2 vector);
    public static OnMove onMove;

    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
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
        PLAYERMOVE,
        TURNEND
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

    private void Update() {
        
    }
}
