using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Hex : MonoBehaviour
{
    SpriteRenderer cellRenderer;
    GameObject graphics;
    Color defaultColor;
    Color currColor;

    bool isHighlightedForUnitAvalibleMove;
    bool isHighlightedForUnitAvalibleAttack;

    [SerializeField]
    Color onMouseEnterColor = Color.HSVToRGB(0, 0.5f, 1);
    [SerializeField]
    Color onMouseDownColor = Color.HSVToRGB(0, 1, 1);
    [SerializeField]
    Color avalibleForUnitMoveColor = Color.HSVToRGB(1, 1, 1);
    [SerializeField]
    Color avalibleForUnitAttackColor = Color.HSVToRGB(1, 1, 1);


    [NonSerialized]
    public Unit unit;
    AvalibleActions avalibleActions;
    Unit _unit;

    #region Serializable

    public string hexName;
    public Vector2 posAtMap = new Vector2(0, 0);
    string[] unitJson;

    #endregion

    #region UnityMethods

    void Start () {
        graphics = transform.Find("Graphics").gameObject;
        cellRenderer = graphics.GetComponentInChildren<SpriteRenderer>();
        defaultColor = cellRenderer.color;
        currColor = defaultColor;

        if (hexName != null && hexName != "")
        {
            gameObject.name = hexName;
        }
        else
        {
            hexName = gameObject.name;
        }
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
            unit.transform.SetParent(transform);
            unit.transform.rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);
           
        }
    }

    public void ShowAvalibleHicesForMove()
    {
        if (unit == null) return;
        avalibleActions = unit.GetAvalibleMoves(this);
        foreach (var item in avalibleActions.moveList.moves)
        {
            item.hex.SwitchAvalibleForUnitMove();
        }
        foreach (var item in avalibleActions.enemyList.moves)
        {
            item.hex.SwitchAvalibleForUnitAttack();
        }

    }

    public void HideAvalibleHicesForMove()
    {
        if (unit == null) return;
        foreach (Move item in unit.avalibleHices.moves)
        {
            item.hex.SwitchAvalibleForUnitMove();
        }
        foreach (Move item in unit.avalibleEnemyHices.moves)
        {
            item.hex.SwitchAvalibleForUnitAttack();
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

    public void SwitchAvalibleForUnitAttack()
    {
        if (!isHighlightedForUnitAvalibleAttack)
        {
            isHighlightedForUnitAvalibleAttack = true;
            currColor = avalibleForUnitAttackColor;
            cellRenderer.color = currColor;
        }
        else
        {
            isHighlightedForUnitAvalibleAttack = false;
            currColor = defaultColor;
            cellRenderer.color = currColor;
        }
    }

    public UnitMoveResponse MoveUnitTo(Hex _hex)
    {
        if(unit.avalibleHices.FindByHex(_hex) == null && unit.avalibleEnemyHices.FindByHex(_hex) == null)
            return UnitMoveResponse.CantMove;

        if (_hex.unit == null)
        {
            unit.currAbleSteps -= unit.avalibleHices.FindByHex(_hex).price;
            _hex.SetUnit(unit);
            unit = null;
            return UnitMoveResponse.Move;
        }
        if (_hex.unit != null)
        {
            if (_hex.unit.owner != unit.owner)
            {
                return UnitMoveResponse.Attack;
            }
        }
        return UnitMoveResponse.CantMove;
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

    #region Serialization

    public string Serialize()
    {
        if (unit != null)
        {
            return hexName + "\\" + posAtMap.x + "\\" + posAtMap.y + "\\" + unit.Serialize();
        }
        else
        {
            return hexName + "\\" + posAtMap.x + "\\" + posAtMap.y;
        }
    }

    public void Deserialize(string[] vs)
    {
        hexName = vs[0];
        posAtMap = new Vector2(int.Parse(vs[1]), int.Parse(vs[2]));
        if (vs.Length >= 4)
        {
            if (vs[3] != "" && vs[3] != null && vs[3] != " " && vs[3] != "/n")
            {
                unitJson = vs[3].Split((char)47);
                Debug.Log(unitJson[0]);
                unit = Instantiate(GameManager.instance.unitsPrefabs[unitJson[0]]).GetComponent<Unit>();
                unit.Deserialize(unitJson);
                UpdateUnit();
            }
        }
    }

    #endregion

    public enum UnitMoveResponse
    {
        CantMove,
        Move,
        Attack
    }
}
