using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Hex activeHex;
    public GameObject debugUnitGO;
    public GameObject debugUnitGO2;

    #region Singleton
    static public PlayerController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
#endregion

    public void SetActiveHex(Hex _hex)
    {
        if (activeHex == _hex)
        {
            activeHex.ChangeColor();
            activeHex = null;
            return;
        }

        if (activeHex != null)
        {
            activeHex.ChangeColor();
        }
        _hex.ChangeColor();
        activeHex = _hex;
    }

    public void InteractWithHex(Hex _hex)
    {
        if (activeHex == null) return;

        if (activeHex == _hex)
        {
            activeHex.ChangeColor();
            activeHex = null;
            return;
        }

        if (activeHex.unit == null)
        {
            activeHex.ChangeColor();
            activeHex = null;
            return;
        }

        if (!activeHex.MoveUnitTo(_hex))
        {
            Debug.Log("Cant move unit to occupied hex!");
        }

        activeHex.ChangeColor();
        activeHex = null;
    }

    public void CreateDebugUnitOnHex(Hex _hex)
    {
        if (_hex.unit == null)
        {
            _hex.SetUnit(Instantiate(debugUnitGO, _hex.transform).GetComponent<Unit>());
        }
    }

    public void CreateDebugUnitOnHex(Hex _hex, GameObject _unitGO)
    {
        if (_hex.unit == null)
        {
            _hex.SetUnit(Instantiate(_unitGO, _hex.transform).GetComponent<Unit>());
        }
    }
}
