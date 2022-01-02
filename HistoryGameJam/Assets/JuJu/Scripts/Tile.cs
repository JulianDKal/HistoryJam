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
        rend.color = Color.green;
        
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown() {
        Debug.Log(gridPosition);
        BattleField.newUnitPosition = this.gridPosition;
        if(!clickable) newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
        else BattleField.activeUnit.transform.position = gameObject.transform.position;
        BattleField.ClearGrid();
    }

    public Vector2 gridPosition;
    public bool clickable = false;
}