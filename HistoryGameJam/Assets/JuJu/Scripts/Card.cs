using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{

    [SerializeField]
    private int unitToSelect;
    [SerializeField]
    private Material outlineMat;
    public int cost;
    [SerializeField]
    private TextMeshProUGUI moneyText;
    private Material startMat;
    private SpriteRenderer rend;

    public BattleManager battleManager;
    public static GameObject currentUnitToPlace;
    public static GameObject currentCardSelected;

    private void Awake() {
        currentUnitToPlace = battleManager.listOfUnitsPlaceable[0]; //setting the unit to allyinfantry by default so it never becomes null
        currentCardSelected = this.gameObject;
        startMat = GetComponent<SpriteRenderer>().material;
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        currentUnitToPlace = battleManager.listOfUnitsPlaceable[unitToSelect];
        currentCardSelected = this.gameObject;
    }

    public static void RemoveMoney(int cost)
    {
        Game_Manager.currentMoney -= cost;       
    }

    public static void GiveMoney(int cost)
    {
        Game_Manager.currentMoney += cost;   
    }

    private void Update() {
        if(currentCardSelected == this.gameObject && BattleManager.battleState == BattleManager.BattleState.PLAYERPLACE)
        {
            rend.material = outlineMat;
        }
        else rend.material = startMat;
        
        moneyText.text = Game_Manager.currentMoney.ToString();
    }

}
