using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Symbol : MonoBehaviour
{
    public string symbolName;
    public int payoutValue;
    public bool isWild;
    public bool isJackpot;

   /* bool isWinningLine(List<SlotSymbol> symbols)
    {
        string firstSymbol = null;
        foreach (var symbol in symbols)
        {
            if (symbol.isWild) continue;
            if (firstSymbol == null) firstSymbol = symbol.symbolName;
            if(symbol.symbolName != firstSymbol)return false;
        }
      }return true;
    }*/
}
