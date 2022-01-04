using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private static GameObject currentUnit;
    private GameObject newUnit;

    public void Awake() {
        ToggleUnits();
    }

    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.green;     
    }

    private void OnMouseExit() {
        if(!clickable) GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown() {
        Debug.Log(gridPosition);
        BattleField.newUnitPosition = this.gridPosition;
        if(!clickable)
        {
            newUnit = Instantiate(currentUnit, gameObject.transform.position, Quaternion.identity);
            BattleManager.ToggleTurn();
        }
        else 
        {
            BattleField.activeUnit.transform.position = gameObject.transform.position;
            BattleManager.onMove.Invoke(gridPosition);
            BattleManager.onMove = null;
        }
        BattleField.ClearGrid();
    }

    public static void ToggleUnits() {
        BattleField battleField = GameObject.Find("Instantiator").GetComponent<BattleField>();

        if(BattleManager.gameState == BattleManager.GameState.playerTurn)
        {
            //tiles set to ally tanks
            currentUnit = battleField.units[0];
        }
        else if(BattleManager.gameState == BattleManager.GameState.enemyTurn)
        {
            //tiles set to enemy tanks
            currentUnit = battleField.units[1];
        }
        else
        Debug.Log("Unknown game state");
    }

    public Vector2 gridPosition;
    public bool clickable = false;
    public bool occupied = false;
}
