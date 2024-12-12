using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Symbol : MonoBehaviour
{
    public string symbolName;
    public int payoutValue;
    public bool isWild;
    public bool isJackpot;

    public static bool IsWinningLine(List<Slot_Symbol> symbols)
    {
        string firstSymbol = null;

        foreach (var symbol in symbols)
        {
            if (symbol.isWild) continue; // Wild symbols can match any other symbol
            if (firstSymbol == null) firstSymbol = symbol.symbolName; // Set the first symbol
            if (symbol.symbolName != firstSymbol) return false; // Mismatch means it's not a winning line
        }

        return true; // All symbols match or are wild
    }
}

