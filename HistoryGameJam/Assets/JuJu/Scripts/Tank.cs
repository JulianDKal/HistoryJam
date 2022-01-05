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
            if(!tile.GetComponent<Tile>().occupied) tile.GetComponent<SpriteRenderer>().color = Color.green;
            else tile.GetComponent<SpriteRenderer>().color = Color.red;

            tile.GetComponent<Tile>().movable = true;
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
            tile.GetComponent<Tile>().underAttack = true;
        }
    }

    private void OnMouseDown() {
        BattleField.activeUnit = this.gameObject;
        if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE) 
        {
            ShowMoveRange();
            BattleManager.onMove += UpdatePosition;
        }
        else ShowAttackRange();
    }

    private void UpdatePosition(Vector2 newPositionVector)
    {
        positionInGrid = newPositionVector;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GetComponent<Animator>().SetTrigger("ShootTrigger");
        }
    }

}
