using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAssembler : MonoBehaviour {

    public GameObject hexagonal_cell;
    public Vector2 sizeOfMap = new Vector2(1,1);
    Vector2 startPos;
    
	void Start () {
        startPos = transform.position;
        
        for (int i = 0; i <= sizeOfMap.x; i++)
        {
            for (int j = 0; j <= sizeOfMap.y; j++)
            {
                Instantiate(hexagonal_cell, new Vector2(startPos.x + j * 3.84f, startPos.y + i*4.43f - j * 2.215f), Quaternion.identity);
            }
        }
        
    }
}
