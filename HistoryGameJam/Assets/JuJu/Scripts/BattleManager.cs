using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static AttackOrMove attackOrMove;
    public static GameState gameState;
    public static List<Unit> activePlayerUnits;
    public static List<Unit> activeEnemyUnits;
    public static int turnsSinceWaveStart;

    public delegate void OnMove(Vector2 vector);
    public static OnMove onMove;

    public enum AttackOrMove
    {
        ATTACK,
        MOVE
    }

    public enum GameState
    {
        playerTurn,
        enemyTurn
    }


    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
        gameState = GameState.playerTurn;
    }

    public static void ToggleTurn()
    {
        TextMeshProUGUI textMeshPro = GameObject.Find("TurnPanel/TurnText").GetComponent<TextMeshProUGUI>();
        switch(gameState)
        {
            case GameState.playerTurn:
            gameState = GameState.enemyTurn;
            textMeshPro.text = "ENEMY TURN";
            Tile.ToggleUnits();
            break;

            case GameState.enemyTurn:
            gameState = GameState.playerTurn;
            textMeshPro.text = "YOUR TURN";
            Tile.ToggleUnits();
            break;
        }
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
