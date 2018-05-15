using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Unit : MonoBehaviour
{
    Text hpText;
    [NonSerialized]
    public float tmpAnimNormTime;
    [NonSerialized]
    public MoveList avalibleHices = new MoveList();
    public MoveList avalibleEnemyHices = new MoveList();
    Hex tmpHex;

    #region Serializable

    public string unitName;
    [HideInInspector]
    public int currAbleSteps = 2;
    public int maxAbleSteps = 2;
    [SerializeField]
    int maxHP;
    [SerializeField]
    int currHP;
    private PlayerController _owner;
    public PlayerController owner
    {
        get
        {
            return _owner;
        }
        set
        {
            if (owner != null)
                if (!GameManager.instance.players[GameManager.instance.players.IndexOf(owner)].units.Contains(this))
                    GameManager.instance.players[GameManager.instance.players.IndexOf(owner)].units.Remove(this);
            if (!GameManager.instance.players[GameManager.instance.players.IndexOf(value)].units.Contains(this))
                GameManager.instance.players[GameManager.instance.players.IndexOf(value)].units.Add(this);
            _owner = value;
        }
    }

    #endregion

    private void Start()
    {
        if (owner == null && GameManager.instance.currActivePlayer != null)
            owner = GameManager.instance.currActivePlayer;
        currHP = maxHP;
        if (unitName != null && unitName != "")
        {
            gameObject.name = unitName;
        }
        else
        {
            unitName = gameObject.name;
        }
    }

    public AvalibleActions GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            avalibleHices.Clear();
            avalibleEnemyHices.Clear();
            var temp = GetAvalibleHices(hex, currAbleSteps);
            temp.RemoveByHex(hex);
            return new AvalibleActions(temp, avalibleEnemyHices);
        }
        return new AvalibleActions(null, null);
    }

    MoveList GetAvalibleHices(Hex hex, int _currAbleSteps)
    {
        if (hex != null)
        {
            if (_currAbleSteps > 0)
            {
                _currAbleSteps--;
                
                int tmpX;
                int tmpY;

                tmpX = (int)hex.posAtMap.x + 1;
                tmpY = (int)hex.posAtMap.y;
                CheckHex(tmpX, tmpY, _currAbleSteps);

                tmpX = (int)hex.posAtMap.x - 1;
                tmpY = (int)hex.posAtMap.y;
                CheckHex(tmpX, tmpY, _currAbleSteps);

                tmpX = (int)hex.posAtMap.x + 1;
                tmpY = (int)hex.posAtMap.y + 1;
                CheckHex(tmpX, tmpY, _currAbleSteps);

                tmpX = (int)hex.posAtMap.x - 1;
                tmpY = (int)hex.posAtMap.y - 1;
                CheckHex(tmpX, tmpY, _currAbleSteps);

                tmpX = (int)hex.posAtMap.x;
                tmpY = (int)hex.posAtMap.y + 1;
                CheckHex(tmpX, tmpY, _currAbleSteps);

                tmpX = (int)hex.posAtMap.x;
                tmpY = (int)hex.posAtMap.y - 1;
                CheckHex(tmpX, tmpY, _currAbleSteps);
            }
            else
            {
                return avalibleHices;
            }
        }
        return avalibleHices;
    }

    void CheckHex(int tmpX, int tmpY, int _currAbleSteps)
    {
        tmpHex = Map.instance.FindHexByPos(tmpX, tmpY);
        if (tmpHex != null)
        {
            if (tmpHex.unit == null)
            {
                if (avalibleHices.FindByHex(tmpHex) == null || avalibleHices.FindByHex(tmpHex).price > currAbleSteps - _currAbleSteps)
                {
                    avalibleHices.RemoveByHex(tmpHex);
                    avalibleHices.Add(new Move(tmpHex, currAbleSteps - _currAbleSteps));
                }
                GetAvalibleHices(tmpHex, _currAbleSteps);
            }
            else
            {
                if (avalibleEnemyHices.FindByHex(tmpHex) == null || avalibleEnemyHices.FindByHex(tmpHex).price > currAbleSteps - _currAbleSteps)
                {
                    if (tmpHex.unit.owner != owner)
                    {
                        avalibleEnemyHices.RemoveByHex(tmpHex);
                        avalibleEnemyHices.Add(new Move(tmpHex, currAbleSteps - _currAbleSteps));
                    }
                }
            }
        }
    }

    public void ShowUnitHealth()
    {
        hpText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));
        hpText.transform.position = new Vector3(hpText.transform.position.x, hpText.transform.position.y, 0);
        hpText.text = currHP.ToString();
    }

    #region Serialization

    public string Serialize()
    {
        return unitName + "/" + currAbleSteps + "/" + maxAbleSteps + "/" + maxHP + "/" + currHP + "/" + GameManager.instance.players.IndexOf(owner);
    }
    
    public void Deserialize(string[] vs)
    {
        unitName = vs[0];
        currAbleSteps = int.Parse(vs[1]);
        maxAbleSteps = int.Parse(vs[2]);
        maxHP = int.Parse(vs[3]);
        currHP = int.Parse(vs[4]);
        owner = GameManager.instance.players[int.Parse(vs[5])];
    }

    #endregion

}
