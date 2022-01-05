using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private int unitToSelect;

    public BattleManager battleManager;
    public static GameObject currentUnitToPlace;

    private void Start() {
        currentUnitToPlace = battleManager.listOfUnitsPlacable[0]; //setting the unit to allyinfantry by default so it never becomes null
    }

    private void OnMouseDown() {
        currentUnitToPlace = battleManager.listOfUnitsPlacable[unitToSelect];
        Debug.Log(currentUnitToPlace);
    }

}
