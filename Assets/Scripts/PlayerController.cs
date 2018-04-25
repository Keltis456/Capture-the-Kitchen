using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Hex activeHex;
    public GameObject debugUnitGO;
    public GameObject debugUnitGO2;
    public List<Unit> units = new List<Unit>();

    private void Start()
    {
        if (debugUnitGO == null)
        {
            debugUnitGO = (GameObject)Resources.Load("Unit02");
            Debug.Log(debugUnitGO);
        }
        if (debugUnitGO2 == null)
        {
            debugUnitGO2 = (GameObject)Resources.Load("Unit02");
            Debug.Log(debugUnitGO2);
        }
    }

    public void SetActiveHex(Hex _hex)
    {
        if (_hex == null)
        {
            return;
        }

        if (activeHex == _hex)
        {
            if (activeHex.unit != null)
            {
                activeHex.HideAvalibleHicesForMove();
            }
            activeHex.ChangeColor();
            activeHex = null;
            return;
        }

        if (activeHex != null)
        {
            if (activeHex.unit != null)
            {
                activeHex.HideAvalibleHicesForMove();
            }
            activeHex.ChangeColor();
        }

        activeHex = _hex;

        if (activeHex != null)
        {
            if (activeHex.unit != null)
            {
                activeHex.ShowAvalibleHicesForMove();
            }
            activeHex.ChangeColor();
        }
    }

    public void InteractWithHex(Hex _hex)
    {
        if (activeHex == null) return;

        if (activeHex == _hex)
        {
            activeHex.ChangeColor();
            activeHex.HideAvalibleHicesForMove();
            activeHex = null;
            return;
        }

        if (activeHex.unit == null)
        {
            activeHex.ChangeColor();
            activeHex = null;
            return;
        }

        activeHex.HideAvalibleHicesForMove();
        activeHex.ChangeColor();
        if (!activeHex.MoveUnitTo(_hex))
        {
            Debug.Log("Cant move unit!");
        }
        
        activeHex = null;
    }

    public void CreateDebugUnitOnHex(Hex _hex)
    {
        if (_hex.unit == null)
        {
            units.Add(_hex.SetUnit(Instantiate(debugUnitGO, _hex.transform).GetComponent<Unit>()));
        }
    }

    public void CreateDebugUnitOnHex(Hex _hex, GameObject _unitGO)
    {
        if (_hex.unit == null)
        {
            units.Add(_hex.SetUnit(Instantiate(_unitGO, _hex.transform).GetComponent<Unit>()));
        }
    }
}
