using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject Game1Canvas;
    // Start is called before the first frame update
    public void SwitchToMENUCanvas() 
    {
        Game1Canvas.SetActive(false);
        MenuCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
