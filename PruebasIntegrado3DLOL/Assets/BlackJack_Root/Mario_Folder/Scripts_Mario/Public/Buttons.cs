using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject Game1Canvas;
    public GameObject cardDeck;
    // Start is called before the first frame update
    public void SwitchToMENUCanvas() 
    {
        Game1Canvas.gameObject.SetActive(false);
        MenuCanvas.SetActive(true);
        cardDeck.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
