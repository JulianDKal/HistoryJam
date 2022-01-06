using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private GameObject newUnit;

    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE) 
        {
            if(!occupied)
                rend.color = Color.green;
            else
                rend.color = Color.red;
        }
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
            if(Card.currentCardSelected.GetComponent<Card>().cost <= Game_Manager.currentMoney)
            {
            PlaceNewUnit(Card.currentUnitToPlace, out GameObject newUnit);
            BattleManager.activePlayerUnits.Add(newUnit);
            Card.RemoveMoney(Card.currentCardSelected.GetComponent<Card>().cost);
            }
            else Debug.Log("You don't have enough money!");           
        }
        
        else if(BattleManager.battleState == BattleManager.BattleState.PLAYERTURN)
        {
            //player picks unit
            if(occupied && !GetUnit().IsEnemy())
                GetUnit().Select();
    
            //player picks target
            if(movable || attackable)
            {
                //move mode
                if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE)
                    MoveActiveUnit();
                //attack mode
                else {
                    Attack();
                }
                BattleField.ClearGrid();
            }
        }
        else
            Debug.Log("Unknown state");

    }

    public void PlaceNewUnit(GameObject unit, out GameObject instantiatedUnit)
    {
        BattleField.newUnitPosition = this.gridPosition;       
        if(!occupied)
        {
            //all units are created as a child of Tile
            newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
            newUnit.transform.parent = transform;
            occupied = true;
            instantiatedUnit = newUnit;           
        }
        else instantiatedUnit = null;
    }

        public void PlaceNewUnit(GameObject unit)
    {
        BattleField.newUnitPosition = this.gridPosition;       
        if(!occupied)
        {
            //all units are created as a child of Tile
            newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
            newUnit.transform.parent = transform;
            occupied = true;          
        }
    }

    private void MoveActiveUnit()
    {
        //update unit position
        BattleField.activeUnit.transform.position = gameObject.transform.position;
        //unoccupy former tile
        BattleField.activeUnit.GetComponent<Tank>().GetTile().occupied = false;

        //update unit grid position
        BattleField.activeUnit.GetComponent<Tank>().UpdatePosition(gridPosition);
        //make child of this tile
        BattleField.activeUnit.transform.parent = transform;
        //occupy this tile
        occupied = true;
        //unselect unit
        BattleField.activeUnit = null;
    }

    public void Attack()
    {
        if(occupied)
        {
            Tank unitUnderAttack = GetUnit();

            if(unitUnderAttack.IsEnemy()) //if enemy unit
            {
                unitUnderAttack.GetTile().occupied = false;
                BattleManager.activeEnemyUnits.Remove(unitUnderAttack.gameObject);
                //animation somewhere here?
                Destroy(unitUnderAttack.gameObject);
            }
        }
    }

    public void AttackAlly() //method for attacking the player's units for the enemy script
    {
        if(occupied)
        {
            Tank unitUnderAttack = GetUnit();

            if(!unitUnderAttack.IsEnemy()) 
            {
                unitUnderAttack.GetTile().occupied = false;
                BattleManager.activePlayerUnits.Remove(unitUnderAttack.gameObject);
                //animation somewhere here?
                Destroy(unitUnderAttack.gameObject);
            }
        }
    }

    //access unit on this tile
    public Tank GetUnit() { return transform.GetChild(0).gameObject.GetComponent<Tank>(); }

    public Vector2 gridPosition;
    public bool movable = false;
    public bool attackable = false;
    public bool occupied = false;
}
