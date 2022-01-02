using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuJu
{

public class BattleField : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private int fieldSize = 4;
    

    private void Start() {
        int index = 0;
        Vector2 firstTileInRow = Vector2.zero;
        for (int i = 0; i < Mathf.Pow(fieldSize, 2); i++)
        {
            
            float offsetX = -0.66f;
            float offsetY = -0.41f;
            if(i % fieldSize == 0)
            {
                offsetX = 0.66f; 
                index = i / fieldSize;
                firstTileInRow = new Vector2(index * offsetX, index * offsetY);
            }
            Vector2 instantiatonPos = new Vector2(firstTileInRow.x + ((i % fieldSize) * offsetX), index * offsetY);
            GameObject newTile = Instantiate(tilePrefab, instantiatonPos, Quaternion.identity);
            index++;
            //if(i % fieldSize == 0) { firstTileInRow = instantiatonPos; }
        }
    }
}


}
