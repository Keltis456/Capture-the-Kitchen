using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public Hex[,] hices;

    #region Singleton
    public static Map instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion
    
    public void DebugMap()
    {
        hices[0, 0].GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        Debug.Log(hices.GetLength(0));
        Debug.Log(hices.GetLength(1));
        hices[hices.GetLength(0) - 1,0].GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
        hices[0, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        hices[hices.GetLength(0) - 1, hices.GetLength(1) - 1].GetComponentInChildren<SpriteRenderer>().color = Color.green;
    }
}
