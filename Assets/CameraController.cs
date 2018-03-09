﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 mouseDragStartPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Camera.main.orthographicSize - Input.mouseScrollDelta.y > 0)
            {
                Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            mouseDragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (mouseDragStartPos.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x), Camera.main.transform.position.y + (mouseDragStartPos.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Camera.main.transform.position.z);
            mouseDragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(true);
        }

	}

    
}
