using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;

public class Tank : Unit
{
    private Material startMat;
    private SpriteRenderer rend;

    [SerializeField]
    private UnitTemplate unitTemplate;
    [SerializeField]
    private Material outlineMaterial;

    [HideInInspector]
    public List<Vector2> moveVectors;
    [HideInInspector]
    public List<Vector2> attackVectors;
    private Vector2 positionInGrid;
    private bool isEnemy;
    public bool wasActiveThisTurn = false;

    private void Awake() {
        this.positionInGrid = BattleField.newUnitPosition;
        moveVectors = unitTemplate.moveVectors;
        attackVectors = unitTemplate.attackVectors;
        isEnemy = unitTemplate.isEnemy;
        rend = GetComponent<SpriteRenderer>();
        startMat = rend.material;
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

    //-removed the OnMouseDown from Tanks to do all interaction through Tiles
    public void Select() {
        BattleField.ClearGrid();
        BattleField.activeUnit = this.gameObject;

        if(BattleManager.attackOrMove == BattleManager.AttackOrMove.MOVE) 
            ShowMoveRange();

        else
            ShowAttackRange();
    }

    //to deselect on double click
    public void Deselect() {
        //BattleField.ClearGrid();
        //BattleField.activeUnit = null;
    }


    //helpers    
    public void UpdatePosition(Vector2 newPositionVector) { positionInGrid = newPositionVector; }
    public Tile GetTile() { return transform.parent.GetComponent<Tile>(); }
    public bool IsEnemy() { return isEnemy; }

    private void Update() {
        if(BattleField.activeUnit == this.gameObject)
        {
            rend.material = outlineMaterial;
        }
        else rend.material = startMat;
    }

}
