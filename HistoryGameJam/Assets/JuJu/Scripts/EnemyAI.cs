using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Wave[] waves;
    [SerializeField]
    private BattleManager battleManager;

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
                BattleManager.activeEnemyUnits.Add(newUnit); //add the newly instantiated unit to the list of enemys
                
            }
            else Debug.Log("Unit out of grid!");
            index++;
        }
        //Debug.Log(BattleManager.activeEnemyUnits.Count);
    }

    public IEnumerator ExecuteEnemyTurn()
    {
        if(BattleManager.battleState == BattleManager.BattleState.ENEMYTURN)
        {
            yield return new WaitForSeconds(1);
            foreach (GameObject unit in BattleManager.activeEnemyUnits)
            {
                //Get information about the current unit
                List<Vector2> availableTilesToMoveTo = new List<Vector2>();
                List<Vector2> availableTilesToAttack = new List<Vector2>();                                  
                Dictionary<GameObject, float> distanceAndUnits = new Dictionary<GameObject, float>();
                GameObject unitToTrack; //the closest ally unit to this unit, which should be followed
                Dictionary<Vector2, float> distanceOfMovetilesAndUnits = new Dictionary<Vector2, float>();
                Vector2 currentPos = unit.GetComponentInParent<Tile>().gridPosition;
                int xComponentOfCurrentPos = (int)currentPos.x;
                bool alreadyAttacked = false;
                //get all available tiles to move to and store them in a list
                foreach (Vector2 moveVector in unit.GetComponent<Tank>().moveVectors)
                {
                    if(BattleField.tilesDictionary.ContainsKey(currentPos - moveVector) && !BattleField.tilesDictionary[currentPos - moveVector].GetComponent<Tile>().occupied)
                    {
                        availableTilesToMoveTo.Add(currentPos - moveVector);
                    }
                }
                //attack the first unit in range immediately and go to the next unit
                foreach (Vector2 attackVector in unit.GetComponent<Tank>().attackVectors)
                {
                    Tile tile;
                    if(BattleField.tilesDictionary.ContainsKey(currentPos - attackVector)) 
                    {
                        tile = BattleField.tilesDictionary[currentPos - attackVector].GetComponent<Tile>();
                    }
                    else continue;
                    if(tile.occupied == true && !tile.GetUnit().IsEnemy())
                    {
                        tile.AttackAlly();
                        alreadyAttacked = true;
                        break;
                    }
                }
                if(alreadyAttacked)
                {
                    yield return new WaitForSeconds(1);
                    continue; 
                } 
                
                for (int i = 0; i < BattleManager.activePlayerUnits.Count; i++)
                {
                    //gets the length of the vector from allyUnit to this unit
                    float distance = (BattleManager.activePlayerUnits[i].transform.position - unit.transform.position).magnitude; 
                    distanceAndUnits.Add(BattleManager.activePlayerUnits[i], distance);
                }      
                Dictionary<GameObject, float>.ValueCollection distances = distanceAndUnits.Values;
                //access unit by first matching value
                unitToTrack = distanceAndUnits.FirstOrDefault(x => x.Value == distances.Min()).Key;
                //Debug.Log(unitToTrack);

                foreach (Vector2 vector in availableTilesToMoveTo)
                {               
                    //the distance between the current Tile that can be moved to and the target unit
                    float distance = (BattleField.tilesDictionary[vector].transform.position - unitToTrack.transform.position).magnitude;
                    distanceOfMovetilesAndUnits.Add(vector, distance);
                }
                //all the values of the dictionary that contains
                Dictionary<Vector2, float>.ValueCollection distancesOfMoveTiles = distanceOfMovetilesAndUnits.Values; 
                Vector2 tileToMoveToPos = distanceOfMovetilesAndUnits.FirstOrDefault(x => x.Value == distancesOfMoveTiles.Min()).Key;
                Tile tileToMoveTo = BattleField.tilesDictionary[tileToMoveToPos].GetComponent<Tile>();
                //move to the corresponding tile:
                BattleField.tilesDictionary[currentPos].GetComponent<Tile>().occupied = false; //unoccupy former tile
                unit.transform.position = tileToMoveTo.gameObject.transform.position;
                tileToMoveTo.occupied = true;
                unit.transform.parent = tileToMoveTo.transform;

                /*availableTilesToMoveTo.Clear();
                availableTilesToAttack.Clear();
                distanceAndUnits.Clear();
                distanceOfMovetilesAndUnits.Clear(); */
                alreadyAttacked = false;            

                yield return new WaitForSeconds(1);
            }
            battleManager.startButton.GetComponent<Button>().enabled = true;
            battleManager.startButton.GetComponent<Button>().onClick.Invoke();
           /*if(BattleManager.waveCount != waves.Length) {
               BattleManager.SetState(BattleManager.BattleState.NEWWAVE);
           } */
            //BattleManager.turnsSinceWaveStart++; //both players moved == 1 turn
        }
    }
}

[System.Serializable]
public class Wave 
{
    public Vector2[] unitPlacements;
    public GameObject[] units;
}
