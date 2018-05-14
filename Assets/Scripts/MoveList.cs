using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveList {

    public List<Move> moves = new List<Move>();

    public Move Add(Move move)
    {
        moves.Add(move);
        return move;
    }

    public Move FindByHex(Hex hex)
    {
        foreach (var item in moves)
        {
            if (item.hex != null)
            {
                if (item.hex == hex)
                {
                    return item;
                }
            }
        }

        return null;
    }

    public void Clear()
    {
        moves.Clear();
    }

    public void RemoveByHex(Hex hex)
    {
        foreach (var item in moves)
        {
            if (item.hex == hex)
            {
                moves.Remove(item);
                return;
            }
        }
    }
}

public struct AvalibleActions
{
    public MoveList moveList;
    public MoveList enemyList;

    public AvalibleActions(MoveList _moveList, MoveList _enemyList) : this()
    {
        moveList = _moveList;
        enemyList = _enemyList;
    }
}