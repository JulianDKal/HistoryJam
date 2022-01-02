using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameObject unit;

    private void OnMouseEnter() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.green;
        Debug.Log("mouse over tile");
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMouseDown() {
        Debug.Log("clicked");
        GameObject newUnit = Instantiate(unit, gameObject.transform.position, Quaternion.identity);
    }
}
