using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public bool isAbleToMove = false;
    public int currAbleSteps = 0;
    public int maxAbleSteps = 0;

    public List<Hex> avalibleHices = new List<Hex>();

    public List<Hex> GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            return GetAvalibleHices(hex);
        }
        return null;
    }

    List<Hex> GetAvalibleHices(Hex hex)
    {
        if (hex != null)
        {
            if (currAbleSteps > 0)
            {
                currAbleSteps--;

                Hex tmpHex;
                int tmpX;
                int tmpY;

                tmpX = (int)hex.posAtMap.x + 1;
                tmpY = (int)hex.posAtMap.y;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex);
                    }
                }
            }
            else
            {
                return avalibleHices;
            }
        }
        return null;
    }
}
