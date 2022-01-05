using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;

public class Tank : Unit
{
    [SerializeField]
    private UnitTemplate unitTemplate;

    private List<Vector2> moveVectors;
    private List<Vector2> attackVectors;
    private Vector2 positionInGrid;

    private void Awake() {
        this.positionInGrid = BattleField.newUnitPosition;
        moveVectors = unitTemplate.moveVectors;
        attackVectors = unitTemplate.attackVectors;
    }

    public override void Attack()
    {
        
    }
    
    public override void ShowMoveRange()
    {

        foreach (Vector2 vector in moveVectors)
        {   
            GameObject tile;
            if(BattleField.tilesDictionary.ContainsKey(positionInGrid - vector)) 
            {
                tile = BattleField.tilesDictionary[positionInGrid - vector];
            }
            else continue;

            if(!tile.GetComponent<Tile>().occupied) 
            {
                tile.GetComponent<SpriteRenderer>().color = Color.green;
                tile.GetComponent<Tile>().movable = true;
            }
            else tile.GetComponent<SpriteRenderer>().color = Color.red;
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
            else continue;          
            tile.GetComponent<SpriteRenderer>().color = Color.yellow;
            tile.GetComponent<Tile>().attackable = true;
        }
    }

    private void OnMouseDown() {
        //only allow to select units during player turn
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERTURN)
            {
            BattleField.ClearGrid();
            BattleField.activeUnit = this.gameObject;

            if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE) 
            {
                ShowMoveRange();
            }
            else
            {
                ShowAttackRange();
            }
        }
    }

    public void UpdatePosition(Vector2 newPositionVector)
    {
        positionInGrid = newPositionVector;
    }

    public Tile GetTile()
    {
        Tile tile;
        if(BattleField.tilesDictionary.ContainsKey(positionInGrid)) 
        {
            tile = BattleField.tilesDictionary[positionInGrid].GetComponent<Tile>();
            return tile;
        }
        else
        {
            Debug.Log("Unit out of grid!");
            return null;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GetComponent<Animator>().SetTrigger("ShootTrigger");
        }
    }

}
