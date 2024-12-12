using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Slot_Machine : MonoBehaviour
{
    public string Name;
    public int Rarity; //1(common) to 5(legendary)
    public int Payout;
    public bool isWild;
    public bool isJackpot;
    public GameObject[] symbolPrefabs;
    public GridLayoutGroup gridLayoutGroup;
    private List<GameObject> currentSymbols = new List<GameObject>();
    // Start is called before the first frame update
    public void SpinReel()
    {
        foreach (GameObject symbol in currentSymbols)
        {
            Destroy(symbol); 
        }
        currentSymbols.Clear();
           //RandomSymbols in the grid
         for(int i =0; i< gridLayoutGroup.transform.childCount;i++)
        {
         //select random symbol first
            int randomIndex = Random.Range(0, symbolPrefabs.Length);
            GameObject randomSymbol = Instantiate(symbolPrefabs[randomIndex],gridLayoutGroup.transform);
         //store for future reference
            currentSymbols.Add(randomSymbol);
        }
         //check IF ther´s a price after.
        StartCoroutine(CheckResults());
    }
   private IEnumerator CheckResults()
    {
        yield return new WaitForSeconds(1); //for visual effects purpose.

        //Add extra logic to determine extra prizes based on the resulting symbols/
        Debug.Log("Checking Results...");
    }
    private void CheckMatch()//win
    {
        List<int[]> selectedLines = new List<int[]>
        {
            //Divided by lines
         new int[] {0,1,2,3,4 },//1
         new int[] {5,6,7,8,9 },//2
         new int[] {10,11,12,13,14 },//3
         new int[] {0,6,12,8,4}, //D1
         new int[] {10,6,2,8,14}, //D2
        };
        foreach (int[] line in selectedLines)
        {
            //all same ?
            string IndexName = currentSymbols[line[0]].name;
            bool isFavLineSelected = true;

            foreach (int index in line) 
            {
                if (currentSymbols[index].name != IndexName) 
                {
                    isFavLineSelected = false;
                    break;
                }
            }
            if (isFavLineSelected)
            {
                //Space for rewards
                Debug.Log("You win!Line:"+string.Join(",",line));

               
            }
        }
    }
}
