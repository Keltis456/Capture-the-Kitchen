using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public bool isAbleToMove = false;
    public int currAbleSteps = 2;
    public int maxAbleSteps = 2;

    public MoveList avalibleHices = new MoveList();

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
}
