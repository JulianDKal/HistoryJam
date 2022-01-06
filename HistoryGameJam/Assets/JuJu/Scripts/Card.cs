using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private int unitToSelect;
    [SerializeField]
    private Material outlineMat;
    [SerializeField]
    private int cost;
    private Material startMat;
    private SpriteRenderer rend;

    public BattleManager battleManager;
    public static GameObject currentUnitToPlace;
    public static GameObject currentCardSelected;

    private void Start() {
        currentUnitToPlace = battleManager.listOfUnitsPlaceable[0]; //setting the unit to allyinfantry by default so it never becomes null
        startMat = GetComponent<SpriteRenderer>().material;
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        currentUnitToPlace = battleManager.listOfUnitsPlaceable[unitToSelect];;
        currentCardSelected = this.gameObject;
    }

    private void Update() {
        if(currentCardSelected == this.gameObject && BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE)
        {
            rend.material = outlineMat;
        }
        else rend.material = startMat;
    }

}
