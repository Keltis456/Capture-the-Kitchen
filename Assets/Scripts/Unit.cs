using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public bool isAbleToMove = false;
    int currAbleSteps = 2;
    public int maxAbleSteps = 0;

    public List<Hex> avalibleHices = new List<Hex>();
    
    public List<Hex> GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            avalibleHices.Clear();
            var temp = GetAvalibleHices(hex, currAbleSteps);
            temp.Remove(hex);
            return temp;
        }
        return null;
    }

    List<Hex> GetAvalibleHices(Hex hex, int _currAbleSteps)
    {
        if (hex != null)
        {
            if (_currAbleSteps > 0)
            {
                _currAbleSteps--;

                //Hex tmpHex;
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
                if (!avalibleHices.Contains(tmpHex))
                {
                    avalibleHices.Add(tmpHex);
                }
                GetAvalibleHices(tmpHex, _currAbleSteps);
            }
        }
    }
}
