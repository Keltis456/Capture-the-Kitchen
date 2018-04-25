using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAssembler : MonoBehaviour {
    
    public GameObject[] cell_prefabs;
    public Vector2 sizeOfMap = new Vector2(1,1);
    Vector2 startPos;
    
	void Start () {
        startPos = transform.position;
        Map.instance.hices = new Hex[(int)sizeOfMap.x, (int)sizeOfMap.y];
        for (int i = 0; i < sizeOfMap.x; i++)
        {
            for (int j = 0; j < sizeOfMap.y; j++)
            {
                GameObject hex = Instantiate(getRandomCellPrefab(), new Vector2(startPos.x + j * 3.84f, startPos.y + i*4.43f - j * 2.215f), Quaternion.identity);
                Map.instance.hices[i,j] = (hex.GetComponent<Hex>());
                hex.GetComponent<Hex>().posAtMap = new Vector2(i, j);
                hex.transform.SetParent(Map.instance.transform);
            }
        }
    }

    GameObject getRandomCellPrefab()
    {
        return cell_prefabs[Random.Range(0, cell_prefabs.Length)];
    }
}
