using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    private Vector2 positionInGrid;

    private void Awake() {
        this.positionInGrid = BattleField.newUnitPosition;
    }

    public override void Attack()
    {
        
    }
    
    public override void ShowMoveRange()
    {
        BattleField.tilesDictionary[positionInGrid - Vector2.up].GetComponent<SpriteRenderer>().color = Color.green;
        BattleField.tilesDictionary[positionInGrid - Vector2.up].GetComponent<Tile>().clickable = true;
        BattleField.tilesDictionary[positionInGrid - new Vector2(1,1)].GetComponent<SpriteRenderer>().color = Color.green;
        BattleField.tilesDictionary[positionInGrid - new Vector2(-1,1)].GetComponent<SpriteRenderer>().color = Color.green;
    }

    public override void ShowAttackRange()
    {
        
    }

    private void OnMouseDown() {
        BattleField.activeUnit = this.gameObject;
        ShowMoveRange();
    }
}
