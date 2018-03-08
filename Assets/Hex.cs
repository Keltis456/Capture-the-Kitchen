using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    SpriteRenderer cellRenderer;
    GameObject graphics;
    Color defaultColor;
    Color currColor;
    public Color onMouseEnterColor = Color.HSVToRGB(0, 0.5f, 1);
    public Color onMouseDownColor = Color.HSVToRGB(0, 1, 1);

	// Use this for initialization
	void Start () {
        graphics = transform.Find("Graphics").gameObject;
        cellRenderer = graphics.GetComponentInChildren<SpriteRenderer>();
        defaultColor = cellRenderer.color;
        currColor = defaultColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseEnter()
    {
        cellRenderer.color = onMouseEnterColor;
    }

    private void OnMouseExit()
    {
        cellRenderer.color = currColor;
    }

    private void OnMouseDown()
    {
        if (currColor != defaultColor)
        {
            currColor = defaultColor;
            cellRenderer.color = currColor;
        }
        else
        {
            currColor = onMouseDownColor;
            cellRenderer.color = currColor;
        }
    }
}
