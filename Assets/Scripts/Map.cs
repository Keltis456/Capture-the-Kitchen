using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour {

    public List<Hex> hices = new List<Hex>();

    [SerializeField]
    GameObject[] cell_prefabs;
    [SerializeField]
    Vector2 sizeOfMap = new Vector2(1,1);
    Vector2 startPos;
    GameObject hex;
    string hexName;
    string outputTmp = "";
    string[] saveFileContent;
    string[] cellJson;
    string saveFileName;
    string[] stringSeparators = new string[] { "[stop]" };

    #region Singleton
    public static Map instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion
    
    public void PlayGame()
    {
        startPos = transform.position;
        GameManager.instance.StartGame(2,0);
        InitializeNewMap();
    }

    public void InitializeNewMap()
    {
        for (int i = 0; i < sizeOfMap.x; i++)
        {
            for (int j = 0; j < sizeOfMap.y; j++)
            {
                hex = cell_prefabs[Random.Range(0, cell_prefabs.Length)];
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
            if (item.posAtMap.x == posX && item.posAtMap.y == posY)
            {
                return item;
            }
        }
        return null;
    }

    public void SaveMap()
    {
        outputTmp = "";
        outputTmp += GameManager.instance.players.Count + "|" + GameManager.instance.players.IndexOf(GameManager.instance.currActivePlayer) + "[stop]";
        foreach (var item in hices)
        {
            outputTmp += item.Serialize() + "|";
        }
        Debug.Log(outputTmp);
        saveFileName = Application.persistentDataPath + "/" + System.DateTime.Now.Year + "_" + System.DateTime.Now.DayOfYear + "_" + System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute + "_" + System.DateTime.Now.Second;
        if (File.Exists(saveFileName))
        {
            File.Delete(saveFileName);
        }
        File.WriteAllText(saveFileName, outputTmp);
        Debug.Log(Application.persistentDataPath);
        
    }

    public void LoadLastSave()
    {
        outputTmp = File.ReadAllText(GetLastSave());
        saveFileContent = outputTmp.Split(stringSeparators, System.StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log(saveFileContent[0]);
        outputTmp = saveFileContent[1];
        saveFileContent = saveFileContent[0].Split((char)124);
        GameManager.instance.StartGame(int.Parse(saveFileContent[0]), int.Parse(saveFileContent[1]));
        
        if (outputTmp != null && outputTmp != "")
        {
            foreach (var item in hices)
            {
                Destroy(item.gameObject);
            }
            hices.Clear();
            foreach (string item in outputTmp.Split((char)124))
            {
                if (item != "" && item != null)
                {
                    //Debug.Log(item);
                    cellJson = item.Split((char)92);
                    hices.Add(Instantiate(GameManager.instance.cellPrefabs[cellJson[0]], new Vector2(startPos.x + int.Parse(cellJson[2]) * 3.84f, startPos.y + int.Parse(cellJson[1]) * 4.43f - int.Parse(cellJson[2]) * 2.215f), Quaternion.identity).GetComponent<Hex>());
                    hices[hices.Count - 1].Deserialize(cellJson);

                }
            } 
        }
    }

    string GetLastSave()
    {
        string maxFileName = null;
        System.DateTime maxFileCreationDateTime = System.DateTime.MinValue;
        foreach (string fileName in Directory.GetFiles(Application.persistentDataPath))
        {
            if(File.GetCreationTime(fileName) > maxFileCreationDateTime) maxFileCreationDateTime = File.GetCreationTime(fileName);
            maxFileName = fileName;
        }
        return maxFileName;
    }
}
