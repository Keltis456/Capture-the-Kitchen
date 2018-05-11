using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public List<Hex> hices = new List<Hex>();

    public GameObject[] cell_prefabs;
    public Vector2 sizeOfMap = new Vector2(1,1);
    Vector2 startPos;
    GameObject hex;
    string hexName;
    string outputTmp = "";
    string[] cellJson;

    #region Singleton
    public static Map instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    void Start () {
        startPos = transform.position;
        //hices = new Hex[(int)sizeOfMap.x, (int)sizeOfMap.y];
        InitializeNewMap();
        DebugMap();
        //SaveMap();
    }

    public void InitializeNewMap()
    {
        for (int i = 0; i < sizeOfMap.x; i++)
        {
            for (int j = 0; j < sizeOfMap.y; j++)
            {
                hex = getRandomCellPrefab();
                hexName = hex.name;
                hex = Instantiate(hex, new Vector2(startPos.x + j * 3.84f, startPos.y + i * 4.43f - j * 2.215f), Quaternion.identity);
                hex.name = hexName;
                hices.Add(hex.GetComponent<Hex>());
                hex.GetComponent<Hex>().posAtMap = new Vector2(i, j);
                hex.transform.SetParent(transform);
            }
        }
    }

    public Hex FindHexByPos(int posX, int posY)
    {
        foreach (Hex item in hices)
        {
            if (item.posAtMap.x == posX || item.posAtMap.y == posY)
            {
                return item;
            }
        }
        return null;
    }

    public void SaveMap()
    {
        outputTmp = "";
        foreach (var item in hices)
        {
            outputTmp += item.Serialize() + "|";
        }
        Debug.Log(outputTmp);
    }
    /*
    public void SaveMap(string fileName)
    {
        outputTmp = "";
        foreach (var item in hices)
        {
            outputTmp += JsonUtility.ToJson(item) + "|";
        }
        Debug.Log(outputTmp);
    }
    */
    public void LoadMap()
    {
        if (outputTmp != null || outputTmp != "")
        {
            foreach (var item in hices)
            {
                Destroy(item.gameObject);
            }
            hices.Clear();
            foreach (string item in outputTmp.Split((char)124))
            {
                Debug.Log(item);
                cellJson = item.Split((char)92);
                hices.Add(Instantiate(GameManager.instance.cellPrefabs[cellJson[0]], transform).GetComponent<Hex>());
                hices[hices.Count].Deserialize(cellJson);
            } 
        }
    }

    public void DebugMap()
    {/*
        hices[0, 0].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        hices[hices.GetLength(0) - 1, 0].GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        hices[0, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        hices[hices.GetLength(0) - 1, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.green;*/
    }

    GameObject getRandomCellPrefab()
    {
        return cell_prefabs[Random.Range(0, cell_prefabs.Length)];
    }
}
