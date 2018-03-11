using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    SpriteRenderer cellRenderer;
    GameObject graphics;
    Color defaultColor;
    Color currColor;

    public Unit unit; //{ get; private set; }
    public Color onMouseEnterColor = Color.HSVToRGB(0, 0.5f, 1);
    public Color onMouseDownColor = Color.HSVToRGB(0, 1, 1);

    #region UnityMethods

    void Start () {
        graphics = transform.Find("Graphics").gameObject;
        cellRenderer = graphics.GetComponentInChildren<SpriteRenderer>();
        defaultColor = cellRenderer.color;
        currColor = defaultColor;
	}

    private void OnMouseEnter()
    {
        cellRenderer.color = onMouseEnterColor;
    }

    private void OnMouseExit()
    {
        cellRenderer.color = currColor;
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1)){
            PlayerController.instance.InteractWithHex(this);
        }
        if (Input.GetMouseButtonDown(3) || Input.GetKeyDown(KeyCode.D))
        {
            DestroyUnit();
        }
        if (Input.GetMouseButtonDown(4) || Input.GetKeyDown(KeyCode.C))
        {
            PlayerController.instance.CreateDebugUnitOnHex(this);
        }
    }

    private void OnMouseDown()
    {
        PlayerController.instance.SetActiveHex(this);
    }

    

    #endregion

    #region PublicCustomMethods

    public void ChangeColor()
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

    public void SetUnit(Unit _unit)
    {
        if (unit == null)
        {
            unit = _unit;
            UpdateUnit();
        }
    }

    public void UpdateUnit()
    {
        if (unit != null)
        {
            unit.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
    }

    public bool MoveUnitTo(Hex _hex)
    {
        if (_hex.unit == null)
        {
            _hex.SetUnit(unit);
            unit = null;
            return true;
        }
        return false;
    }

    public void DestroyUnit()
    {
        if (unit != null)
        {
            Destroy(unit.gameObject);
            unit = null;
        }
    }

    #endregion
}
