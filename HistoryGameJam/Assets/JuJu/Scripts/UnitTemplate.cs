using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Unit", menuName ="Unit")]
public class UnitTemplate : ScriptableObject
{
    public List<Vector2> moveVectors = new List<Vector2>();
    public List<Vector2> attackVectors = new List<Vector2>();
    public bool isEnemy;
}
