using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
    
    [HideInInspector]
    public int currAbleSteps = 2;
    public int maxAbleSteps = 2;
    public int maxHP;
    public int currHP;
    public Text hpText;
    public float tmpAnimNormTime;
    public MoveList avalibleHices = new MoveList();

    private void Start()
    {
        hpText = Instantiate(hpText.gameObject, GameManager.instance.canvas.transform).GetComponent<Text>();
        currHP = maxHP;
        ShowUnitHealth();
    }

    private void OnGUI()
    {
        ShowUnitHealth();
    }

    public MoveList GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            avalibleHices.Clear();
            var temp = GetAvalibleHices(hex, currAbleSteps);
            temp.RemoveByHex(hex);
            return temp;
        }
        return null;
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
        if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
        {
            Hex tmpHex;
            tmpHex = Map.instance.hices[tmpX, tmpY];
            if (tmpHex.unit == null)
            {
                if (avalibleHices.FindByHex(tmpHex) == null || avalibleHices.FindByHex(tmpHex).price > currAbleSteps - _currAbleSteps)
                {
                    avalibleHices.RemoveByHex(tmpHex);
                    avalibleHices.Add(new Move(tmpHex, currAbleSteps - _currAbleSteps));
                }
                GetAvalibleHices(tmpHex, _currAbleSteps);
            }
        }
    }

    public void ShowUnitHealth()
    {
        hpText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));
        hpText.transform.position = new Vector3(hpText.transform.position.x, hpText.transform.position.y, 0);
        hpText.text = currHP.ToString();
    }
}
