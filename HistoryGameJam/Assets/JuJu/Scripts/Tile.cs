using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    private GameObject unit;

    private GameObject newUnit;

    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if(!movable && !underAttack) rend.color = Color.green;     
    }

    private void OnMouseExit() {
        if(!movable && !underAttack) GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown() {
        //Debug.Log(gridPosition);
        BattleField.newUnitPosition = this.gridPosition;
        if(!movable) newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
        else 
        {
            BattleField.activeUnit.transform.position = gameObject.transform.position;
            occupied = true;
            BattleManager.onMove.Invoke(gridPosition);
            BattleManager.onMove = null;
        }
        BattleField.ClearGrid();
    }

    public Vector2 gridPosition;
    public bool movable = false;
    public bool underAttack = false;
    public bool occupied = false;
}
