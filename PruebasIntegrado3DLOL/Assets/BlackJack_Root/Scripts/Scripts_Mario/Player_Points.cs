using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Points : MonoBehaviour
{
    public Text PlayerPoints;
    public int points;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerPointsText();  
    }

    // Update is called once per frame
    void UpdatePlayerPointsText()
    {
        if (points < 0)
        {
            points = 0;   
        }
        PlayerPoints.text = points.ToString(); 
    }
    public void AddPoints(int amount) 
    
    {
        points += amount;
        UpdatePlayerPointsText();
    }
}
