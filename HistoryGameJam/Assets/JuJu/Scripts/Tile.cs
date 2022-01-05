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
            PlaceNewUnit(Card.currentUnitToPlace);
        }
        //player's turn
        else if(BattleManager.battleState == BattleManager.BattleState.PLAYERTURN && (movable || attackable))
        {
            //move mode
            if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE)
                MoveActiveUnit();
            //attack mode
            else {
            }
        }
        else
            Debug.Log("Unknown state");

        BattleField.ClearGrid();
    }

    public void PlaceNewUnit(GameObject unit)
    {
        BattleField.newUnitPosition = this.gridPosition;
        if(!occupied)
        {
            newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
            occupied = true;
        }
    }

    private void MoveActiveUnit()
    {
        BattleField.activeUnit.transform.position = gameObject.transform.position;
        BattleField.activeUnit.GetComponent<Tank>().GetTile().occupied = false;
        BattleField.activeUnit.GetComponent<Tank>().UpdatePosition(gridPosition);
        occupied = true;
        BattleField.activeUnit = null;
    }

    public Vector2 gridPosition;
    public bool movable = false;
    public bool attackable = false;
    public bool occupied = false;
}
