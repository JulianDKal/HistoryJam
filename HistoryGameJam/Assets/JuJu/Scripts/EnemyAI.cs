using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Wave[] waves;

    public void PlaceWave()
    {
        int index = 0;
        foreach(Vector2 vector in waves[BattleManager.waveCount].unitPlacements)
        {
            GameObject tile;
            if(BattleField.tilesDictionary.ContainsKey(vector)) 
            {
                tile = BattleField.tilesDictionary[vector];
                //instantiate tanks by default
                tile.GetComponent<Tile>().PlaceNewUnit(waves[BattleManager.waveCount].units[index]);
                Debug.Log("Instantiated");
                
            }
            else Debug.Log("Unit out of grid!");
            index++;
        }
    }
}

[System.Serializable]
public class Wave 
{
    public Vector2[] unitPlacements;
    public GameObject[] units;
}
