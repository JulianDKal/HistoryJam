using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleField : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private int fieldSize = 4;
    [SerializeField, Tooltip("Change this value to reposition the battlefield")]
    private Vector2 gridOffset = Vector2.zero;

    public static Vector2 newUnitPosition;
    public static GameObject activeUnit;
    
    public static int[,] gridArray;
    public static Dictionary<Vector2, GameObject> tilesDictionary = new Dictionary<Vector2, GameObject>();

    private void Start() {
        int index = 0;
        Vector2 firstTileInRow = Vector2.zero;
        int yPosition = 0; //just for detecting in which row the current tile is
        for (int i = 0; i < Mathf.Pow(fieldSize, 2); i++)
        {
            
            float offsetX = -1f;
            float offsetY = -0.5f;
            if(i % fieldSize == 0)
            {
                offsetX *= -1; 
                index = i / fieldSize;
                firstTileInRow = new Vector2(index * offsetX, index * offsetY);
                yPosition++;
            }
            Vector2 instantiatonPos = new Vector2(firstTileInRow.x + ((i % fieldSize) * offsetX) + gridOffset.x, index * offsetY + gridOffset.y);
            GameObject newTile = Instantiate(tilePrefab, instantiatonPos, Quaternion.identity);
            Vector2 positionInGrid = new Vector2(i % fieldSize + 1, yPosition); //the position of the newly instantiated tile in the grid
            newTile.GetComponent<Tile>().gridPosition = positionInGrid;
            index++;
            tilesDictionary.Add(positionInGrid, newTile); //add position and tile to the dictionary for later use
        }
    }

    public static void ClearGrid()
    {
        for (int i = 0; i < tilesDictionary.Count; i++)
        {
            KeyValuePair<Vector2, GameObject> entry = tilesDictionary.ElementAt(i);
            tilesDictionary[entry.Key].GetComponent<SpriteRenderer>().color = Color.white;
            entry.Value.GetComponent<Tile>().clickable = false;
        }
    }
}



