using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<PlayerController> players = new List<PlayerController>();
    public PlayerController currActivePlayer;
    public GameObject playerEthalon;
    public Canvas canvas;
    public GameObject mainMenu;

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
        //Debug.Log(unitsPrefabs);

        //Загрузка префабов для клеток
        cellPrefabsArray = Resources.LoadAll<GameObject>("Prefabs/Cells");
        cellPrefabs = new Dictionary<string, GameObject>();
        foreach (var item in cellPrefabsArray)
        {
            cellPrefabs.Add(item.name, item);
        }

        players.Add(Instantiate(playerEthalon).GetComponent<PlayerController>());
        players.Add(Instantiate(playerEthalon).GetComponent<PlayerController>());
        currActivePlayer = players[0];
    }

    public void EndOfPlayerTurn()
    {
        currActivePlayer.SetActiveHex(currActivePlayer.activeHex);
        foreach (Unit item in currActivePlayer.units)
        {
            item.currAbleSteps = 0;
            //item.gameObject.GetComponent<Animation>().Play("Unit_Idle");
            //item.gameObject.GetComponent<Animation>()["Unit_Idle"]

            item.tmpAnimNormTime = item.gameObject.GetComponent<Animation>()["Unit_Idle"].normalizedTime;
            item.gameObject.GetComponent<Animation>().Stop();
        }
        if (players.IndexOf(currActivePlayer) + 1 >= players.Count)
        {
            currActivePlayer = players[0];
        }
        else
        {
            currActivePlayer = players[players.IndexOf(currActivePlayer) + 1];
        }
        foreach (Unit item in currActivePlayer.units)
        {
            item.currAbleSteps = item.maxAbleSteps;
            item.gameObject.GetComponent<Animation>().Play("Unit_Idle");
            item.gameObject.GetComponent<Animation>()["Unit_Idle"].normalizedTime = item.tmpAnimNormTime;
        }
        Debug.Log("CurrActivePlayer = " + players.IndexOf(currActivePlayer));
    }
}
