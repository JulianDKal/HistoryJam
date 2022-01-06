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
                tile.GetComponent<Tile>().PlaceNewUnit(waves[BattleManager.waveCount].units[index], out GameObject newUnit);
                //Debug.Log("Instantiated");
                BattleManager.activeEnemyUnits.Add(newUnit); //add the newly instantiated unit to the list of enemys
                
            }
            else Debug.Log("Unit out of grid!");
            index++;
        }
        Debug.Log(BattleManager.activeEnemyUnits.Count);
    }

    public IEnumerator ExecuteEnemyTurn()
    {
        foreach (GameObject unit in BattleManager.activeEnemyUnits)
        {
            //Get information about the current unit
            List<Vector2> availableTilesToMoveTo = new List<Vector2>();
            List<Vector2> availableTilesToAttack = new List<Vector2>();
            Vector2 currentPos = unit.GetComponentInParent<Tile>().gridPosition;
            int xComponentOfCurrentPos = (int)currentPos.x;

            //get all available tiles to move to and store them in a list
            foreach (Vector2 moveVector in unit.GetComponent<Tank>().moveVectors)
            {
                if(BattleField.tilesDictionary.ContainsKey(currentPos - moveVector) && !BattleField.tilesDictionary[currentPos - moveVector].GetComponent<Tile>().occupied)
                {
                    availableTilesToMoveTo.Add(currentPos - moveVector);
                }
            }
            //get all available tiles to attack and store them in a list
            foreach (Vector2 attackVector in unit.GetComponent<Tank>().attackVectors)
            {
                if(BattleField.tilesDictionary.ContainsKey(currentPos - attackVector))
                {
                    availableTilesToAttack.Add(attackVector);
                }
            }           
            yield return new WaitForSeconds(1);
        }

        BattleManager.battleState = BattleManager.BattleState.PLAYERTURN;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) StartCoroutine("ExecuteEnemyTurn");
    }
}

[System.Serializable]
public class Wave 
{
    public Vector2[] unitPlacements;
    public GameObject[] units;
}
