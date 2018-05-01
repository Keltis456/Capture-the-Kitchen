using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAssembler : MonoBehaviour {

    public Hex[,] hices;

    public GameObject[] cell_prefabs;
    public Vector2 sizeOfMap = new Vector2(1,1);
    Vector2 startPos;
    GameObject hex;
    string hexName;
    string outputTmp = "";

    #region Singleton
    public static MapAssembler instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    void Start () {
        startPos = transform.position;
        hices = new Hex[(int)sizeOfMap.x, (int)sizeOfMap.y];
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
                hices[i, j] = (hex.GetComponent<Hex>());
                hex.GetComponent<Hex>().posAtMap = new Vector2(i, j);
                hex.transform.SetParent(transform);
            }
        }
    }

    public void SaveMap()
    {
        outputTmp = "";
        foreach (var item in hices)
        {
            outputTmp += JsonUtility.ToJson(item) + "\n";
        }
        Debug.Log(outputTmp);

    }

    public void DebugMap()
    {
        hices[0, 0].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        hices[hices.GetLength(0) - 1, 0].GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        hices[0, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        hices[hices.GetLength(0) - 1, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.green;
    }

    GameObject getRandomCellPrefab()
    {
        return cell_prefabs[Random.Range(0, cell_prefabs.Length)];
    }
}
