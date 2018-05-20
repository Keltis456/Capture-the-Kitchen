using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Hex activeHex;
    public List<Unit> units = new List<Unit>();
    Hex.UnitMoveResponse unitMoveResponse;

    #region Serializable
    public string playerName;
    public int foodCount;
    public Unit.UnitRace unitRace;
    #endregion

    private void Start()
    {
        DebugUnitsInit();
    }

    public void SetActiveHex(Hex _hex)
    {
        if (_hex == null)
        {
            return;
        }

        UIManager.instance.UIUpdate();

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

        UIManager.instance.UIUpdate();

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
        unitMoveResponse = activeHex.MoveUnitTo(_hex);
        if (unitMoveResponse == Hex.UnitMoveResponse.Attack)
        {
            Move theBestMoveForAttack = new Move(null, int.MaxValue);
            Move tmpMove = theBestMoveForAttack;
            foreach (Move adjcell in _hex.unit.GetAvalibleMoves(_hex, 1).moveList.moves)
            {
                tmpMove = activeHex.unit.avalibleHices.FindByHex(adjcell.hex);
                if (tmpMove != null)
                {
                    if (tmpMove.price < theBestMoveForAttack.price)
                    {
                        theBestMoveForAttack = tmpMove;
                    }
                }
            }
            foreach (Move adjcell in _hex.unit.avalibleEnemyHices.moves)
            {
                if (adjcell.hex == activeHex)
                {
                    theBestMoveForAttack.hex = null;
                    theBestMoveForAttack.price = 0;
                    break;
                }
            }
            if (theBestMoveForAttack.hex != null)
            {
                unitMoveResponse = activeHex.MoveUnitTo(theBestMoveForAttack.hex);
                activeHex = theBestMoveForAttack.hex;
            }
            else Debug.Log("Alredy in position");
            Debug.Log("Can attack");
            activeHex.unit.AttackUnit(_hex.unit);

        }
        if (unitMoveResponse == Hex.UnitMoveResponse.CantMove)
        {
            Debug.Log("Cant move unit!");
        }
        
        activeHex = null;
    }


    #region Serialization

    public string Serialize()
    {
        Debug.Log("unitRace : " + unitRace);
        string race;
        switch (unitRace)
        {
            case Unit.UnitRace.Ant:
                race = "0";
                break;
            case Unit.UnitRace.Spider:
                race = "1";
                break;
            case Unit.UnitRace.Cockroach:
                race = "2";
                break;
            default:
                race = "3";
                break;
        }
        return playerName
            + "/" + foodCount
            + "/" + race;
    }

    public void Deserialize(string[] vs)
    {
        playerName = vs[0];
        foodCount = int.Parse(vs[1]);
        switch (vs[2])
        {
            case "0":
                unitRace = Unit.UnitRace.Ant;
                break;
            case "1":
                unitRace = Unit.UnitRace.Spider;
                break;
            case "2":
                unitRace = Unit.UnitRace.Cockroach;
                break;
            default:
                unitRace = Unit.UnitRace.Cockroach;
                break;
        }
        //unitRace = vs[2];
    }

    #endregion


    #region Debug
    public GameObject debugUnitGO;
    public GameObject debugUnitGO2;

    void DebugUnitsInit()
    {
        /*
        if (debugUnitGO == null)
        {
            debugUnitGO = GameManager.instance.unitsPrefabs["Unit01"];
            Debug.Log(debugUnitGO);
        }*/
        if (debugUnitGO2 == null)
        {
            debugUnitGO2 = GameManager.instance.unitsPrefabs["Unit02"];
            Debug.Log(debugUnitGO2);
        }
        switch (unitRace)
        {
            case Unit.UnitRace.Ant:
                debugUnitGO = GameManager.instance.unitsPrefabs["AntUnit00"];
                break;
            case Unit.UnitRace.Spider:
                debugUnitGO = GameManager.instance.unitsPrefabs["SpiderUnit00"];
                break;
            case Unit.UnitRace.Cockroach:
                debugUnitGO = GameManager.instance.unitsPrefabs["RoachUnit00"];
                break;
            default:
                debugUnitGO = GameManager.instance.unitsPrefabs["Unit01"];
                break;
        }
    }

    public void CreateDebugUnitOnHex(Hex _hex)
    {
        if (_hex.unit == null)
        {
            units.Add(_hex.SetUnit(Instantiate(debugUnitGO, _hex.transform).GetComponent<Unit>()));
            _hex.unit.name = debugUnitGO.name;
        }
    }

    public void CreateDebugUnitOnHex(Hex _hex, GameObject _unitGO)
    {
        if (_hex.unit == null)
        {
            units.Add(_hex.SetUnit(Instantiate(_unitGO, _hex.transform).GetComponent<Unit>()));
            _hex.unit.name = _unitGO.name;
        }
    }
    #endregion
}
