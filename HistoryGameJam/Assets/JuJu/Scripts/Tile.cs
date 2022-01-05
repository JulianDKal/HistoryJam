using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private GameObject newUnit;

    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE) rend.color = Color.green;     
    }

    private void OnMouseExit() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE) rend.color = Color.white;
    }

    private void OnMouseDown() {
        //Debug.Log(gridPosition);
        
        //placement phase
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE)
        {
            BattleField.newUnitPosition = this.gridPosition;
            if(!occupied)
            {
                newUnit = Instantiate(Card.currentUnitToPlace, gameObject.transform.position, Quaternion.identity);
                occupied = true;
            }
        }
        //player's turn
        else if(BattleManager.battleState == BattleManager.BattleState.PLAYERTURN && (movable || attackable))
        {
            //move mode
            if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE){
                BattleField.activeUnit.transform.position = gameObject.transform.position;
                BattleField.activeUnit.GetComponent<Tank>().UpdatePosition(gridPosition);
                BattleField.activeUnit = null;
                occupied = true;
            }
            //attack mode
            else {

            }
        }
        BattleField.ClearGrid();
    }

    public Vector2 gridPosition;
    public bool movable = false;
    public bool attackable = false;
    public bool occupied = false;
}
