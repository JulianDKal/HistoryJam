using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
    public List<string> dates = new List<string>();
    public List<GameObject> buttons = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI dateText;

    private void Awake() {
        dateText.text = dates[Game_Manager.instance.currentLevelIndex - 3];
        buttons[Game_Manager.instance.currentLevelIndex - 3].GetComponent<Image>().color = Color.green;
    }
}
