using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<GameObject> listOfUnitsPlaceable;
    public EnemyAI enemy;
    public GameObject startButton;
    public int maxTurnsPerWave;
    [SerializeField]
    private int moneyPerWave = 100;
    [SerializeField]
    private TextMeshProUGUI turnsText;
    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private int goldReward = 100;
    [SerializeField]
    private TextMeshProUGUI waveText;

    public static AttackOrMove attackOrMove;
    public static BattleState battleState;
    public static List<GameObject> activePlayerUnits = new List<GameObject>();
    public static List<GameObject> activeEnemyUnits = new List<GameObject>();
    public static int turnsSinceWaveStart;
    public static int waveCount;

    public static int unitsKilled;
    public static int unitsLost;
    public static int reward;
    public static int moneyAtStartOfLevel;

    public enum AttackOrMove
    {
        ATTACK,
        MOVE
    }

    public enum BattleState 
    {
        NEWWAVE, 
        PLAYERPLACE, 
        PLAYERTURN,
        ENEMYTURN,
        WIN,
        LOSE
    }

    private void Start() {
        attackOrMove = AttackOrMove.MOVE;
        waveCount = 0;
        turnsSinceWaveStart = 0;
        reward = goldReward;
        unitsKilled = 0;
        unitsLost = 0;
        moneyAtStartOfLevel = Game_Manager.currentMoney - moneyPerWave;

        SetState(BattleState.NEWWAVE);
        StartCoroutine(WaitForNextWave());
        StartCoroutine(WaitForWinOrLose());
        
    }

    private IEnumerator WaitForNextWave()
    {
        while(waveCount <= enemy.waves.Length){
            yield return new WaitUntil(() => battleState == BattleState.NEWWAVE || activeEnemyUnits.Count <= 0);
            if(waveCount != enemy.waves.Length) NextWave();
            else waveCount++;
        }
        //if all waves are complete, you win
        SetState(BattleState.WIN);
    }

    private IEnumerator WaitForWinOrLose()
    {
        while(battleState != BattleState.WIN && battleState != BattleState.LOSE)
        {
            yield return new WaitUntil(() => battleState == BattleState.WIN || battleState == BattleState.LOSE);
            if(battleState == BattleState.WIN) Game_Manager.instance.Win(); 
            else if (battleState == BattleState.LOSE) Game_Manager.instance.GameOver(); 
        }
    }

    private void NextWave()
    {
        turnsSinceWaveStart = 0;
        enemy.PlaceWave();
        BattleField.ClearGrid();
        Card.GiveMoney(moneyPerWave);
        waveCount++;
        Debug.Log(waveCount);
        foreach (GameObject unit in activePlayerUnits)
        {
            unit.GetComponent<Tank>().wasActiveThisTurn = false;
        }

        foreach (KeyValuePair<Vector2, GameObject> tile in BattleField.tilesDictionary)
        {
            if(tile.Value.GetComponent<Tile>().occupied == false && tile.Value.GetComponent<Tile>().gridPosition.x >= 5)
            {
                tile.Value.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        SetState(BattleState.PLAYERPLACE);
        startButton.SetActive(true);
    }

    public void EnemyTurn()
    {
        //if(turnsSinceWaveStart < maxTurnsPerWave)
        //{
            SetState(BattleManager.BattleState.ENEMYTURN);
            StartCoroutine(enemy.ExecuteEnemyTurn());
        //}
        //else
          //  SetState(BattleState.NEWWAVE);
    }

    public void ToggleAttackMove(TextMeshProUGUI textMeshPro)
    {
        if(attackOrMove == AttackOrMove.MOVE)
        {
            attackOrMove = AttackOrMove.ATTACK;
            textMeshPro.text = "ATTACK";
        }
        else
        {
            attackOrMove = AttackOrMove.MOVE;
            textMeshPro.text = "MOVE";
        }
    }

    public void ToggleStartButton(TextMeshProUGUI textMeshPro)
    {
        switch(battleState)
        {
            case BattleState.PLAYERPLACE : {
                if(activePlayerUnits.Count > 0)
                {
                SetState(BattleState.PLAYERTURN);
                startButton.GetComponent<Button>().enabled = true;
                textMeshPro.text = "NEXT";
                stateText.text = "YOUR TURN";
                BattleField.ClearGrid();
                turnsSinceWaveStart++;
                foreach (GameObject unit in activePlayerUnits)
                {
                    unit.GetComponent<Tank>().wasActiveThisTurn = false;
                }
                }
                
            }
            break;
            case BattleState.PLAYERTURN : {
                textMeshPro.gameObject.SetActive(false);
                startButton.GetComponent<Button>().enabled = false; //prevents the player from clicking on the button while it's the enemy's turn
                SetState(BattleState.ENEMYTURN);
                stateText.text = "ENEMY TURN";
                EnemyTurn();
                foreach (GameObject unit in activePlayerUnits)
                {
                    unit.GetComponent<Tank>().wasActiveThisTurn = false;
                }
                }
            break;
            case BattleState.ENEMYTURN : { 
                textMeshPro.gameObject.SetActive(true);
                startButton.GetComponent<Button>().enabled = true;
                stateText.text = "YOUR TURN";
                textMeshPro.text = "NEXT";
                turnsSinceWaveStart++;
                if(turnsSinceWaveStart <= maxTurnsPerWave) BattleManager.SetState(BattleState.PLAYERTURN);
                else if(turnsSinceWaveStart > maxTurnsPerWave) SetState(BattleState.NEWWAVE);
             }
            break;
        }
    }

    public static void SetState(BattleState bs)
    {
        battleState = bs;
    }

    private void Update() {
        turnsText.text = "Turn: " + turnsSinceWaveStart + "/" + maxTurnsPerWave; 
        waveText.text = "Wave: " + waveCount + "/" + enemy.waves.Length;
        if(battleState == BattleState.PLAYERPLACE) 
        {
            stateText.text = "PLACE UNITS";
            //startButton.GetComponentInChildren<TextMeshProUGUI>().text = "START";
        }
    }
}
