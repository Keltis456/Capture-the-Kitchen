using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    SpriteRenderer cellRenderer;
    GameObject graphics;
    Color defaultColor;
    Color currColor;

    bool isHighlightedForUnitAvalibleMove;

    public Unit unit; //{ get; private set; }
    public Color onMouseEnterColor = Color.HSVToRGB(0, 0.5f, 1);
    public Color onMouseDownColor = Color.HSVToRGB(0, 1, 1);
    public Color avalibleForUnitMoveColor = Color.HSVToRGB(1, 1, 1);
    public Vector2 posAtMap = new Vector2(0, 0);

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
            GameManager.instance.currActivePlayer.InteractWithHex(this);
        }
        if (Input.GetMouseButtonDown(3) || Input.GetKeyDown(KeyCode.D))
        {
            DestroyUnit();
        }
        if (Input.GetMouseButtonDown(4) || Input.GetKeyDown(KeyCode.C))
        {
            GameManager.instance.currActivePlayer.CreateDebugUnitOnHex(this);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameManager.instance.currActivePlayer.CreateDebugUnitOnHex(this, GameManager.instance.currActivePlayer.debugUnitGO2);
        }
    }

    private void OnMouseDown()
    {
        GameManager.instance.currActivePlayer.SetActiveHex(this);
    }

    #endregion

    #region PublicCustomMethods

    public void ChangeColor()
    {
        if (!isHighlightedForUnitAvalibleMove)
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

    public Unit SetUnit(Unit _unit)
    {
        if (unit == null)
        {
            unit = _unit;
            UpdateUnit();
        }
        return _unit;
    }

    public void UpdateUnit()
    {
        if (unit != null)
        {
            unit.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            unit.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
            unit.gameObject.GetComponent<Animation>().Play("Unit_Idle");
            unit.ShowUnitHealth();
        }
    }

    public void ShowAvalibleHicesForMove()
    {
        if (unit == null) return;
        foreach (var item in unit.GetAvalibleMoves(this).moves)
        {
            item.hex.SwitchAvalibleForUnitMove();
        }

    }

    public void HideAvalibleHicesForMove()
    {
        if (unit == null) return;
        foreach (Move item in unit.avalibleHices.moves)
        {
            item.hex.SwitchAvalibleForUnitMove();
        }
    }

    public void SwitchAvalibleForUnitMove()
    {
        if (!isHighlightedForUnitAvalibleMove)
        {
            isHighlightedForUnitAvalibleMove = true;
            currColor = avalibleForUnitMoveColor;
            cellRenderer.color = currColor;
        }
        else
        {
            isHighlightedForUnitAvalibleMove = false;
            currColor = defaultColor;
            cellRenderer.color = currColor;
        }
    }

    public bool MoveUnitTo(Hex _hex)
    {
        if (_hex.unit == null && unit.avalibleHices.FindByHex(_hex) != null)
        {
            unit.currAbleSteps -= unit.avalibleHices.FindByHex(_hex).price;
            _hex.SetUnit(unit);
            unit = null;
            return true;
        }
        return false;
    }

    public void DestroyUnit()
    {
        if (unit != null && GameManager.instance.currActivePlayer.units.Contains(unit))
        {
            GameManager.instance.currActivePlayer.units.Remove(unit);
            Destroy(unit.gameObject);
            unit = null;
        }
    }

    #endregion
}
