using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BattleManager battleManager;

    [SerializeField]
    private Vector2[] wave1 = { new Vector2(1.0f, 1.0f),
                                new Vector2(2.0f, 2.0f),
                                new Vector2(3.0f, 3.0f) };


    public void PlaceWave()
    {
        foreach(Vector2 vector in wave1)
        {
            GameObject tile;
            if(BattleField.tilesDictionary.ContainsKey(vector)) 
            {
                tile = BattleField.tilesDictionary[vector];
                //instantiate tanks by default
                tile.GetComponent<Tile>().PlaceNewUnit(battleManager.listOfUnitsPlaceable[3]);
                Debug.Log("Instantiated");
            }
            else Debug.Log("Unit out of grid!");
        }
    }
}
