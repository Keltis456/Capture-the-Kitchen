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

    [HideInInspector]
    public Hex hex;

    Hex tmpHex;
    
    public UnitRace unitRace;

    #region Unit Stats
    [Space(10)]
    [Header("Unit Stats")]
    [SerializeField]
    int _maxHealthPoints;
    public int maxHealthPoints
    {
        get
        {
            return _maxHealthPoints;
        }
        private set
        {
            _maxHealthPoints = value;
        }
    }
    [SerializeField]
    int _damagePoints;
    public int damagePoints
    {
        get
        {
            return _damagePoints;
        }
        private set
        {
            _damagePoints = value;
        }
    }
    [SerializeField]
    int _armourPoints;
    public int armourPoints
    {
        get
        {
            return _armourPoints;
        }
        private set
        {
            _armourPoints = value;
        }
    }
    [SerializeField]
    int _experiencePointsPerLevel;
    public int experiencePointsPerLevel
    {
        get
        {
            return _experiencePointsPerLevel;
        }
        private set
        {
            _experiencePointsPerLevel = value;
        }
    }
    [SerializeField]
    int _maxLevel;
    public int maxLevel
    {
        get
        {
            return _maxLevel;
        }
        private set
        {
            _maxLevel = value;
        }
    }
    public int maxMovePoints;
    [SerializeField]
    int _foodPointsForReproduction;
    public int foodPointsForReproduction
    {
        get
        {
            return _foodPointsForReproduction;
        }
        private set
        {
            _foodPointsForReproduction = value;
        }
    }
    [SerializeField]
    int _foodPointsConsumption;
    public int foodPointsConsumption
    {
        get
        {
            return _foodPointsConsumption;
        }
        private set
        {
            _foodPointsConsumption = value;
        }
    }
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
    public int movePoints;
    public int currHP { get; private set; }
    public int experiencePoints { get; private set; }
    public int currLevel { get; private set; }
    public int currLegsMutation { get; private set; }
    public int currBodyMutation { get; private set; }
    public int currHeadMutation { get; private set; }

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
        //movePoints = maxMovePoints;

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

    #region Combat
    public void AttackUnit(Unit target)
    {
        if (target == null) return;
        movePoints = 0;
        target.TakeDamage(damagePoints, this);
    }

    public void TakeDamage(int value, Unit from)
    {
        currHP = Mathf.Clamp(currHP - Mathf.Clamp(value - armourPoints, 0, value), 0, currHP);
        Debug.Log(unitName + " suffered a " + Mathf.Clamp(value - armourPoints, 0, value) + "damage. Current HP = " + currHP);
        if (currHP <= 0)
        {
            Debug.Log(unitName + " dead!");
            DestroyUnit(from);
        }
    }

    void DestroyUnit(Unit killer)
    {
        if (killer != null)
        {
            killer.GetExperiencePoints(experiencePointsBounty);
            killer.GetFoodPoints(foodPointsBounty);
        }
        hex.DestroyUnit();
    }

    public void GetExperiencePoints(int count)
    {
        //TODO
        if (count <= 0) return;
        if (currLevel >= maxLevel) return;
        if (experiencePointsPerLevel <= 0) return;
        experiencePoints += count;
        while (experiencePoints >= experiencePointsPerLevel)
        {
            experiencePoints -= experiencePointsPerLevel;
            DebugLvlUp(); //TODO
        }

    }

    public void GetFoodPoints(int count)
    {
        if (count <= 0) return;
        if (owner == null) return;
        owner.foodCount += count;
    }
    #endregion

    #region Serialization

    public string Serialize()
    {
        return unitName 
            + "/" + movePoints 
            + "/" + currHP 
            + "/" + experiencePoints 
            + "/" + currLevel 
            + "/" + currLegsMutation 
            + "/" + currHeadMutation 
            + "/" + currBodyMutation 
            + "/" + GameManager.instance.players.IndexOf(owner);
    }
    
    public void Deserialize(string[] vs)
    {
        unitName = vs[0];
        movePoints = int.Parse(vs[1]);
        currHP = int.Parse(vs[2]);
        experiencePoints = int.Parse(vs[3]);
        currLevel = int.Parse(vs[4]);
        currLegsMutation = int.Parse(vs[5]);
        currHeadMutation = int.Parse(vs[6]);
        currBodyMutation = int.Parse(vs[7]);
        owner = GameManager.instance.players[int.Parse(vs[8])];
    }

    #endregion

    public enum UnitRace
    {
        Ant,
        Spider,
        Cockroach
    }
}
