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
        if(BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE && gridPosition.x >= 5)
        {
            if(Card.currentCardSelected.GetComponent<Card>().cost <= Game_Manager.currentMoney)
            {
                PlaceNewUnit(Card.currentUnitToPlace, out GameObject newUnit);
                if(newUnit!= null)
                {
                BattleManager.activePlayerUnits.Add(newUnit);
                Card.RemoveMoney(Card.currentCardSelected.GetComponent<Card>().cost);
                }
                
            }
            else Debug.Log("You don't have enough money!");           
        }
        
        else if(BattleManager.battleState == BattleManager.BattleState.PLAYERTURN)
        {
            //player picks unit
            if(occupied && !GetUnit().IsEnemy() && !GetUnit().wasActiveThisTurn)
                GetUnit().Select();
    
            //player picks target
            if(movable || attackable)
            {
                //move mode
                if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE) 
                {
                    BattleField.activeUnit.GetComponent<Tank>().wasActiveThisTurn = true;
                    MoveActiveUnit();
                }
                //attack mode
                else StartCoroutine(Attack());
                
                BattleField.ClearGrid();
                //GameObject.Find("BattleManager").GetComponent<BattleManager>().EnemyTurn();
            }
        }
        else
            Debug.Log("Unknown state: " + BattleManager.battleState);

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

    public IEnumerator Attack()
    {
        if(occupied)
        {
            Tank unitUnderAttack = GetUnit();

            if(unitUnderAttack.IsEnemy()) //if enemy unit
            {
                unitUnderAttack.GetTile().occupied = false;
                BattleManager.activeEnemyUnits.Remove(unitUnderAttack.gameObject);
                //animation somewhere here?
                BattleField.activeUnit.GetComponent<Animator>().SetTrigger("ShootTrigger");
                BattleField.activeUnit.GetComponent<AudioSource>().Play();
                unitUnderAttack.GetComponent<Animator>().SetTrigger("DieTrigger");
                yield return new WaitForSeconds(0.4f);
                Destroy(unitUnderAttack.gameObject);
                BattleField.activeUnit.GetComponent<Tank>().wasActiveThisTurn = true;
                BattleManager.unitsKilled++;
            }
        }
    }

    public IEnumerator AttackAlly() //method for attacking the player's units for the enemy script
    {
        if(occupied)
        {
            Tank unitUnderAttack = GetUnit();

            if(!unitUnderAttack.IsEnemy()) 
            {
                unitUnderAttack.GetTile().occupied = false;
                unitUnderAttack.GetComponent<AudioSource>().Play();
                BattleManager.activePlayerUnits.Remove(unitUnderAttack.gameObject);
                //animation somewhere here?
                unitUnderAttack.GetComponent<Animator>().SetTrigger("DieTrigger");
                yield return new WaitForSeconds(0.4f);
                Destroy(unitUnderAttack.gameObject);
                BattleManager.unitsLost++;
            }
            if(BattleManager.activePlayerUnits.Count <= 0) BattleManager.SetState(BattleManager.BattleState.LOSE);
        }
    }

    //access unit on this tile
    public Tank GetUnit() { return transform.GetChild(0).gameObject.GetComponent<Tank>(); }

    public Vector2 gridPosition;
    public bool movable = false;
    public bool attackable = false;
    public bool occupied = false;
}
