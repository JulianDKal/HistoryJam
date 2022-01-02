using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.green;
        Debug.Log("mouse over tile");
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
