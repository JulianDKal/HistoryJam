using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tank : Unit
{
    [SerializeField]
    private UnitTemplate unitTemplate;


    private List<Vector2> moveVectors;
    private List<Vector2> attackVectors;
    private Vector2 positionInGrid;
    private bool isEnemy;

    private void Awake() {
        this.positionInGrid = BattleField.newUnitPosition;
        moveVectors = unitTemplate.moveVectors;
        attackVectors = unitTemplate.attackVectors;
        isEnemy = unitTemplate.isEnemy;
    }

    public override void Attack()
    {
        
    }
    
    public override void ShowMoveRange()
    {
        if(isEnemy ^ BattleManager.gameState == BattleManager.GameState.playerTurn)
        foreach (Vector2 vector in moveVectors)
        {   
            GameObject tile;
            Debug.Log(positionInGrid - vector);
            if(BattleField.tilesDictionary.ContainsKey(positionInGrid - vector)) 
            {
                tile = BattleField.tilesDictionary[positionInGrid - vector];
            }
            else return;
            if(!tile.GetComponent<Tile>().occupied) tile.GetComponent<SpriteRenderer>().color = Color.green;
            else tile.GetComponent<SpriteRenderer>().color = Color.red;

            tile.GetComponent<Tile>().clickable = true;
        }
    }

    public override void ShowAttackRange()
    {
        foreach (Vector2 vector in attackVectors)
        {   
            GameObject tile;
            if(BattleField.tilesDictionary.ContainsKey(positionInGrid - vector)) 
            {
                tile = BattleField.tilesDictionary[positionInGrid - vector];
            }
            else return;          
            tile.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    private void OnMouseDown() {
        if(isEnemy ^ BattleManager.gameState == BattleManager.GameState.playerTurn)
        {
            BattleField.activeUnit = this.gameObject;
            if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE) 
            {
                ShowMoveRange();
                BattleManager.onMove += UpdatePosition;
                BattleManager.ToggleTurn();
            }
            else ShowAttackRange();
        }
    }

    private void UpdatePosition(Vector2 newPositionVector)
    {
        positionInGrid = newPositionVector;
    }
}
