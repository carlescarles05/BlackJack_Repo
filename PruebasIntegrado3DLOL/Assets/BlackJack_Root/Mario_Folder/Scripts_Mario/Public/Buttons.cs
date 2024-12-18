using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [Header("This Scene Canvas Event Handler")]
    public GameObject Register;
    public GameObject Game1Canvas;
    public GameObject cardDeck;
  

    //ACTIVATE CANVAS0 OR REGISTER MENU
    public void SwitchToMENUCanvas() 
    {

        Game1Canvas.gameObject.SetActive(false);
        Register.SetActive(true);
        cardDeck.gameObject.SetActive(false);

    }

    //ACTIVATE CANVAS1
    public void ButtonGameCardCANVA() 
    {
        Game1Canvas.gameObject.SetActive(true);
        Register.gameObject.SetActive(false);
        cardDeck.gameObject.SetActive(true);
    }
    
}

