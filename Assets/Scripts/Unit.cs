using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Unit : MonoBehaviour
{
    public MoveList avalibleHices = new MoveList();
    public MoveList avalibleEnemyHices = new MoveList();
    
    Transform legsMutationParent;
    Transform headMutationParent;
    Transform bodyMutationParent;

    Hex tmpHex;

    
    #region Unit Stats
    [Space(10)]
    [Header("Unit Stats")]
    [SerializeField]
    int maxHealthPoints;
    [SerializeField]
    int damagePoints;
    [SerializeField]
    int armourPoints;
    [SerializeField]
    int experiencePointsPerLevel;
    public int maxMovePoints = 2;
    [SerializeField]
    int foodPointsForReproduction;
    [SerializeField]
    int foodPointsConsumption;
    [Space(10)]
    #endregion

    #region Bounty
    [Header("Bounty")]
    [SerializeField]
    int foodPointsBounty;
    [SerializeField]
    int experiencePointsBounty;
    [Space(10)]
    #endregion

    #region Serializable

    string unitName;
    [HideInInspector]
    public int movePoints = 2;
    int currHP;
    int experiencePoints;
    int currLevel;
    int currLegsMutation;
    int currBodyMutation;
    int currHeadMutation;

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

    #region Unity Methods
    private void Start()
    {
        if (owner == null && GameManager.instance.currActivePlayer != null)
            owner = GameManager.instance.currActivePlayer;
        currHP = maxHealthPoints;
        movePoints = maxMovePoints;

        legsMutationParent = transform.FindDeepChild("Legs");
        bodyMutationParent = transform.FindDeepChild("Head");
        headMutationParent = transform.FindDeepChild("Body");

        UpdateMutationGraphics();

        if (unitName != null && unitName != "")
        {
            gameObject.name = unitName;
        }
        else
        {
            unitName = gameObject.name;
        }
    }
    #endregion

    #region Mutation Controls
    void UpdateMutationGraphics()
    {
        foreach (Transform item in legsMutationParent)
        {
            item.gameObject.SetActive(false);
            if (item.GetSiblingIndex() == currLegsMutation)
            {
                item.gameObject.SetActive(true);
            }
        }
        
        foreach (Transform item in bodyMutationParent)
        {
            item.gameObject.SetActive(false);
            if (item.GetSiblingIndex() == currBodyMutation)
            {
                item.gameObject.SetActive(true);
            }
        }

        foreach (Transform item in headMutationParent)
        {
            item.gameObject.SetActive(false);
            if (item.GetSiblingIndex() == currHeadMutation)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    public void DebugLvlUp()
    {
        if(currLevel != 1)
        {
            currLevel = 1;
            currLegsMutation = 1;
            currHeadMutation = 1;
            currBodyMutation = 1;
            UpdateMutationGraphics();
        }
    }    
    #endregion

    #region Navigation
    public AvalibleActions GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            avalibleHices.Clear();
            avalibleEnemyHices.Clear();
            var temp = GetAvalibleHices(hex, movePoints);
            temp.RemoveByHex(hex);
            return new AvalibleActions(temp, avalibleEnemyHices);
        }
        return new AvalibleActions(null, null);
    }

    public AvalibleActions GetAvalibleMoves(Hex hex, int _currAbleSteps)
    {
        if (hex != null)
        {
            avalibleHices.Clear();
            avalibleEnemyHices.Clear();
            var temp = GetAvalibleHices(hex, _currAbleSteps);
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
                if (avalibleHices.FindByHex(tmpHex) == null || avalibleHices.FindByHex(tmpHex).price > movePoints - _currAbleSteps)
                {
                    avalibleHices.RemoveByHex(tmpHex);
                    avalibleHices.Add(new Move(tmpHex, movePoints - _currAbleSteps));
                }
                GetAvalibleHices(tmpHex, _currAbleSteps);
            }
            else
            {
                if (avalibleEnemyHices.FindByHex(tmpHex) == null || avalibleEnemyHices.FindByHex(tmpHex).price > movePoints - _currAbleSteps)
                {
                    if (tmpHex.unit.owner != owner)
                    {
                        avalibleEnemyHices.RemoveByHex(tmpHex);
                        avalibleEnemyHices.Add(new Move(tmpHex, movePoints - _currAbleSteps));
                    }
                }
            }
        }
    }
    #endregion
    
    #region Serialization

    public string Serialize()
    {
        return unitName + "/" + movePoints + "/" + currHP + "/" + experiencePoints + "/" + currLevel + "/" + GameManager.instance.players.IndexOf(owner);
    }
    
    public void Deserialize(string[] vs)
    {
        unitName = vs[0];
        movePoints = int.Parse(vs[1]);
        currHP = int.Parse(vs[2]);
        experiencePoints = int.Parse(vs[3]);
        currLevel = int.Parse(vs[4]);
        owner = GameManager.instance.players[int.Parse(vs[5])];
    }

    #endregion

}
