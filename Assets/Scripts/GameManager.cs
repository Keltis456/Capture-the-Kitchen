using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<PlayerController> players = new List<PlayerController>();
    public PlayerController currActivePlayer;

    [SerializeField]
    GameObject playerEthalon;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject mainMenu;

    GameObject[] unitsPrefabsArray;
    public Dictionary<string, GameObject> unitsPrefabs;

    GameObject[] cellPrefabsArray;
    public Dictionary<string, GameObject> cellPrefabs;

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    private void Start()
    {
        Instantiate(mainMenu).name = mainMenu.name;

        //Загрузка префабов для юнитов
        unitsPrefabsArray = Resources.LoadAll<GameObject>("Prefabs/Units");
        unitsPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in unitsPrefabsArray)
        {
            unitsPrefabs.Add(item.name, item);
        }
        //Загрузка префабов для клеток
        cellPrefabsArray = Resources.LoadAll<GameObject>("Prefabs/Cells");
        cellPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in cellPrefabsArray)
        {
            cellPrefabs.Add(item.name, item);
        }
        UIManager.instance.ForcedStart();
        UIManager.instance.UIUpdate();
    }

    public void StartGame(int playersCount, int currPlayer, string[] vs)
    {
        players.Clear();
        for (int i = 0; i < playersCount; i++)
        {
            players.Add(Instantiate(playerEthalon).GetComponent<PlayerController>());
            players[i].Deserialize(vs[i].Split((char)47));
        }
        SetCurrentPlayer(currPlayer);
    }

    void SetCurrentPlayer(int num)
    {
        if(players[num] != null)
        {
            if (currActivePlayer != null) currActivePlayer.SetActiveHex(currActivePlayer.activeHex);
            foreach (PlayerController player in players)
            {
                foreach (Unit item in player.units)
                {
                    item.movePoints = 0;
                }
            }
            currActivePlayer = players[num];
            foreach (Unit item in currActivePlayer.units)
            {
                item.movePoints = item.maxMovePoints;
            }
            Debug.Log("CurrActivePlayer = " + players.IndexOf(currActivePlayer));
        }
        UIManager.instance.UIUpdate();
    }

    public void EndOfPlayerTurn()
    {
        if (players.IndexOf(currActivePlayer) + 1 >= players.Count)
        {
            SetCurrentPlayer(0);
        }
        else
        {
            SetCurrentPlayer(players.IndexOf(currActivePlayer) + 1);
        }
    }
}
