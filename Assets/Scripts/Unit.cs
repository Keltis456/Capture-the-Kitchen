using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public bool isAbleToMove = false;
    public int currAbleSteps = 1;
    public int maxAbleSteps = 0;

    public List<Hex> avalibleHices = new List<Hex>();

    public List<Hex> GetAvalibleMoves(Hex hex)
    {
        if (hex != null)
        {
            return GetAvalibleHices(hex, currAbleSteps);
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
                        GetAvalibleHices(tmpHex, _currAbleSteps);
                    }
                }

                tmpX = (int)hex.posAtMap.x - 1;
                tmpY = (int)hex.posAtMap.y;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex, _currAbleSteps);
                    }
                }

                tmpX = (int)hex.posAtMap.x + 1;
                tmpY = (int)hex.posAtMap.y + 1;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex, _currAbleSteps);
                    }
                }

                tmpX = (int)hex.posAtMap.x - 1;
                tmpY = (int)hex.posAtMap.y - 1;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex, _currAbleSteps);
                    }
                }

                tmpX = (int)hex.posAtMap.x;
                tmpY = (int)hex.posAtMap.y + 1;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex, _currAbleSteps);
                    }
                }

                tmpX = (int)hex.posAtMap.x;
                tmpY = (int)hex.posAtMap.y - 1;
                if (Map.instance.hices.GetLength(0) > tmpX && Map.instance.hices.GetLength(1) > tmpY && tmpX >= 0 && tmpY >= 0)
                {
                    tmpHex = Map.instance.hices[tmpX, tmpY];
                    if (!avalibleHices.Contains(tmpHex))
                    {
                        avalibleHices.Add(tmpHex);
                        GetAvalibleHices(tmpHex, _currAbleSteps);
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
